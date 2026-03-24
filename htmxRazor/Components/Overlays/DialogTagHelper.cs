using System.Net;
using htmxRazor.Infrastructure;
using htmxRazor.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace htmxRazor.Components.Overlays;

/// <summary>
/// Renders a modal dialog using the native HTML <c>&lt;dialog&gt;</c> element.
/// The native element provides built-in backdrop, ESC to close, focus trap,
/// <c>showModal()</c>/<c>close()</c> API, and scroll lock.
/// </summary>
/// <remarks>
/// <para>
/// Child tag helpers (<c>&lt;rhx-dialog-footer&gt;</c>) register content into slots.
/// Remaining child content becomes the dialog body.
/// </para>
/// <para>
/// For htmx integration, use <c>data-rhx-dialog-open="dialog-id"</c> on a trigger element,
/// and the server can respond with an <c>HX-Trigger</c> header containing
/// <c>rhx:dialog:open</c> to open the dialog after swap.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// &lt;rhx-dialog id="edit-dialog" rhx-label="Edit Item" rhx-size="large"&gt;
///     &lt;form&gt;...&lt;/form&gt;
///     &lt;rhx-dialog-footer&gt;
///         &lt;rhx-button rhx-variant="ghost"&gt;Cancel&lt;/rhx-button&gt;
///         &lt;rhx-button rhx-variant="brand"&gt;Save&lt;/rhx-button&gt;
///     &lt;/rhx-dialog-footer&gt;
/// &lt;/rhx-dialog&gt;
/// </code>
/// </example>
[HtmlTargetElement("rhx-dialog")]
public class DialogTagHelper : htmxRazorTagHelperBase
{
    /// <inheritdoc/>
    protected override string BlockName => "dialog";

    /// <summary>
    /// Whether the dialog is initially open.
    /// </summary>
    [HtmlAttributeName("rhx-open")]
    public bool Open { get; set; }

    /// <summary>
    /// CSS selector of the element to receive focus after an htmx swap.
    /// Defaults to "first" so focus moves to the first focusable element
    /// in swapped dialog content, addressing WCAG 2.4.3 Focus Order.
    /// Set to "none" to disable.
    /// </summary>
    [HtmlAttributeName("rhx-focus-after-swap")]
    public new string? FocusAfterSwap { get; set; } = "first";

    /// <summary>
    /// The title text displayed in the dialog header.
    /// When set, a header with title and close button is rendered.
    /// </summary>
    [HtmlAttributeName("rhx-label")]
    public string? Label { get; set; }

    /// <summary>
    /// When true, the default header with title and close button is not rendered.
    /// The dialog body receives the full child content.
    /// </summary>
    [HtmlAttributeName("rhx-no-header")]
    public bool NoHeader { get; set; }

    /// <summary>
    /// The size of the dialog. Preset values: <c>small</c> (24rem), <c>medium</c> (32rem),
    /// <c>large</c> (48rem), <c>full</c> (90vw). Any other value is treated as a custom
    /// CSS width (e.g., <c>40rem</c>, <c>600px</c>, <c>80%</c>) and applied via an
    /// inline <c>--rhx-dialog-width</c> custom property.
    /// When omitted the dialog expands to fit its content up to <c>max-width</c>.
    /// </summary>
    [HtmlAttributeName("rhx-size")]
    public string? Size { get; set; }

    /// <summary>
    /// Creates a new DialogTagHelper with URL generation support.
    /// </summary>
    public DialogTagHelper(IUrlHelperFactory urlHelperFactory) : base(urlHelperFactory) { }

    /// <inheritdoc/>
    public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
    {
        var slots = SlotRenderer.CreateForContext(context);
        var childContent = await output.GetChildContentAsync();

        output.TagName = "dialog";
        output.TagMode = TagMode.StartTagAndEndTag;

        var css = CreateCssBuilder();

        // Size modifier
        if (!string.IsNullOrWhiteSpace(Size))
        {
            var normalized = Size.Trim().ToLowerInvariant();
            if (normalized is "small" or "medium" or "large" or "full")
            {
                css.Add($"rhx-dialog--{normalized}");
            }
            else
            {
                css.Add("rhx-dialog--custom");
                // Apply custom width via CSS custom property
                var existingStyle = output.Attributes["style"]?.Value?.ToString() ?? "";
                var styleValue = string.IsNullOrWhiteSpace(existingStyle)
                    ? $"--rhx-dialog-width: {Size}"
                    : $"{existingStyle.TrimEnd(';')}; --rhx-dialog-width: {Size}";
                output.Attributes.SetAttribute("style", styleValue);
            }
        }

        ApplyBaseAttributes(output, css);

        output.Attributes.SetAttribute("data-rhx-dialog", "");

        // aria-labelledby points to the title element
        var dialogId = Id ?? $"rhx-dialog-{context.UniqueId}";
        if (!string.IsNullOrWhiteSpace(Id))
            output.Attributes.SetAttribute("id", dialogId);

        var titleId = $"{dialogId}-title";
        if (!NoHeader && !string.IsNullOrWhiteSpace(Label))
            output.Attributes.SetAttribute("aria-labelledby", titleId);

        if (Open)
            output.Attributes.SetAttribute("open", "open");

        if (!string.IsNullOrWhiteSpace(FocusAfterSwap) && FocusAfterSwap != "none")
            output.Attributes.SetAttribute("data-rhx-focus-after-swap", FocusAfterSwap);

        RenderHtmxAttributes(output);

        // Assemble inner HTML
        output.Content.Clear();

        // Panel wrapper
        output.Content.AppendHtml($"<div class=\"{GetElementClass("panel")}\">");

        // Header
        if (!NoHeader && !string.IsNullOrWhiteSpace(Label))
        {
            output.Content.AppendHtml($"<header class=\"{GetElementClass("header")}\">");
            output.Content.AppendHtml(
                $"<h2 class=\"{GetElementClass("title")}\" id=\"{Enc(titleId)}\">{Enc(Label)}</h2>");
            output.Content.AppendHtml(
                $"<button class=\"{GetElementClass("close")}\" type=\"button\" aria-label=\"Close\">" +
                "&times;</button>");
            output.Content.AppendHtml("</header>");
        }

        // Body
        output.Content.AppendHtml($"<div class=\"{GetElementClass("body")}\">");
        output.Content.AppendHtml(childContent);
        output.Content.AppendHtml("</div>");

        // Footer
        if (slots.Has("footer"))
        {
            output.Content.AppendHtml($"<footer class=\"{GetElementClass("footer")}\">");
            output.Content.AppendHtml(slots.Get("footer")!);
            output.Content.AppendHtml("</footer>");
        }

        output.Content.AppendHtml("</div>"); // close panel
    }

    private static string Enc(string? value) => WebUtility.HtmlEncode(value ?? "") ?? "";
}
