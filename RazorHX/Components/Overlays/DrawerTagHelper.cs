using System.Net;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Razor.TagHelpers;
using RazorHX.Infrastructure;
using RazorHX.Rendering;

namespace RazorHX.Components.Overlays;

/// <summary>
/// Renders a slide-out drawer panel from any edge of the viewport (or a containing element).
/// Includes overlay, focus trap, ESC to close, and slide animation.
/// </summary>
/// <remarks>
/// <para>
/// Child tag helpers (<c>&lt;rhx-drawer-footer&gt;</c>) register content into slots.
/// Remaining child content becomes the drawer body.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// &lt;rhx-drawer id="nav-drawer" rhx-label="Navigation" rhx-placement="start"&gt;
///     &lt;nav&gt;...&lt;/nav&gt;
///     &lt;rhx-drawer-footer&gt;
///         &lt;rhx-button&gt;Close&lt;/rhx-button&gt;
///     &lt;/rhx-drawer-footer&gt;
/// &lt;/rhx-drawer&gt;
/// </code>
/// </example>
[HtmlTargetElement("rhx-drawer")]
public class DrawerTagHelper : RazorHXTagHelperBase
{
    /// <inheritdoc/>
    protected override string BlockName => "drawer";

    /// <summary>
    /// Whether the drawer is initially open.
    /// </summary>
    [HtmlAttributeName("rhx-open")]
    public bool Open { get; set; }

    /// <summary>
    /// The title text displayed in the drawer header.
    /// </summary>
    [HtmlAttributeName("rhx-label")]
    public string? Label { get; set; }

    /// <summary>
    /// The edge from which the drawer slides: start, end, top, bottom. Default: end.
    /// </summary>
    [HtmlAttributeName("rhx-placement")]
    public string Placement { get; set; } = "end";

    /// <summary>
    /// When true, the drawer is positioned relative to its parent element
    /// instead of the viewport.
    /// </summary>
    [HtmlAttributeName("rhx-contained")]
    public bool Contained { get; set; }

    /// <summary>
    /// When true, the default header with title and close button is not rendered.
    /// </summary>
    [HtmlAttributeName("rhx-no-header")]
    public bool NoHeader { get; set; }

    /// <summary>
    /// Creates a new DrawerTagHelper with URL generation support.
    /// </summary>
    public DrawerTagHelper(IUrlHelperFactory urlHelperFactory) : base(urlHelperFactory) { }

    /// <inheritdoc/>
    public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
    {
        var slots = SlotRenderer.CreateForContext(context);
        var childContent = await output.GetChildContentAsync();

        output.TagName = "div";
        output.TagMode = TagMode.StartTagAndEndTag;

        var placement = Placement.ToLowerInvariant();
        var css = CreateCssBuilder()
            .Add(GetModifierClass(placement))
            .AddIf(GetModifierClass("open"), Open)
            .AddIf(GetModifierClass("contained"), Contained);
        ApplyBaseAttributes(output, css);

        output.Attributes.SetAttribute("data-rhx-drawer", "");
        output.Attributes.SetAttribute("data-rhx-placement", placement);

        if (!Open)
            output.Attributes.SetAttribute("aria-hidden", "true");

        if (!string.IsNullOrWhiteSpace(Label))
            output.Attributes.SetAttribute("aria-label", Label);

        if (Contained)
            output.Attributes.SetAttribute("data-rhx-contained", "");

        RenderHtmxAttributes(output);

        // Assemble inner HTML
        output.Content.Clear();

        // Overlay
        output.Content.AppendHtml($"<div class=\"{GetElementClass("overlay")}\"></div>");

        // Panel
        output.Content.AppendHtml($"<div class=\"{GetElementClass("panel")}\" role=\"dialog\">");

        // Header
        if (!NoHeader)
        {
            output.Content.AppendHtml($"<header class=\"{GetElementClass("header")}\">");
            if (!string.IsNullOrWhiteSpace(Label))
            {
                output.Content.AppendHtml(
                    $"<h2 class=\"{GetElementClass("title")}\">{Enc(Label)}</h2>");
            }
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
