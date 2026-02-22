using Microsoft.AspNetCore.Razor.TagHelpers;
using RazorHX.Infrastructure;

namespace RazorHX.Components.Utilities;

/// <summary>
/// Renders a clipboard copy button that copies text to the clipboard on click,
/// with visual success feedback. Supports copying a static value or reading
/// text content from a target element via CSS selector.
/// </summary>
/// <example>
/// <code>
/// &lt;rhx-copy-button rhx-value="dotnet add package htmxRazor" /&gt;
///
/// &lt;pre id="code"&gt;&lt;code&gt;npm install razorhx&lt;/code&gt;&lt;/pre&gt;
/// &lt;rhx-copy-button rhx-from="#code code" /&gt;
/// </code>
/// </example>
[HtmlTargetElement("rhx-copy-button")]
public class CopyButtonTagHelper : RazorHXTagHelperBase
{
    /// <inheritdoc/>
    protected override string BlockName => "copy-button";

    /// <summary>
    /// The text value to copy to the clipboard.
    /// </summary>
    [HtmlAttributeName("rhx-value")]
    public string? Value { get; set; }

    /// <summary>
    /// CSS selector of the element whose textContent should be copied.
    /// Used as an alternative to <see cref="Value"/>.
    /// </summary>
    [HtmlAttributeName("rhx-from")]
    public string? From { get; set; }

    /// <summary>
    /// Whether the copy button is disabled.
    /// </summary>
    [HtmlAttributeName("rhx-disabled")]
    public bool Disabled { get; set; }

    /// <summary>
    /// The tooltip/aria-label shown before copying.
    /// Default: "Copy".
    /// </summary>
    [HtmlAttributeName("rhx-copy-label")]
    public string CopyLabel { get; set; } = "Copy";

    /// <summary>
    /// The tooltip/aria-label shown after a successful copy.
    /// Default: "Copied!".
    /// </summary>
    [HtmlAttributeName("rhx-success-label")]
    public string SuccessLabel { get; set; } = "Copied!";

    /// <summary>
    /// Duration in milliseconds to show the success feedback state.
    /// Default: 2000.
    /// </summary>
    [HtmlAttributeName("rhx-feedback-duration")]
    public int FeedbackDuration { get; set; } = 2000;

    /// <inheritdoc/>
    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        output.TagName = "button";
        output.TagMode = TagMode.StartTagAndEndTag;

        // ── CSS classes ──
        var css = CreateCssBuilder()
            .AddIf(GetModifierClass("disabled"), Disabled);

        ApplyBaseAttributes(output, css);

        // ── HTML attributes ──
        output.Attributes.SetAttribute("type", "button");
        output.Attributes.SetAttribute("data-rhx-copy-button", "");

        if (!string.IsNullOrWhiteSpace(Value))
            output.Attributes.SetAttribute("data-rhx-copy-value", Value);

        if (!string.IsNullOrWhiteSpace(From))
            output.Attributes.SetAttribute("data-rhx-copy-from", From);

        if (FeedbackDuration != 2000)
            output.Attributes.SetAttribute("data-rhx-copy-duration", FeedbackDuration.ToString());

        output.Attributes.SetAttribute("data-rhx-copy-success-label", SuccessLabel);

        // ── ARIA ──
        AriaAttributeHelper.AriaLabel(output, CopyLabel);

        // ── Disabled state ──
        if (Disabled)
        {
            output.Attributes.SetAttribute("disabled", "disabled");
            AriaAttributeHelper.AriaDisabled(output, true);
        }

        // ── Inner content: copy icon + success icon ──
        var iconClass = GetElementClass("icon");

        var copyIcon =
            $"<span class=\"{iconClass} {iconClass}--copy\" aria-hidden=\"true\">" +
            "<svg xmlns=\"http://www.w3.org/2000/svg\" width=\"16\" height=\"16\" viewBox=\"0 0 24 24\" fill=\"none\" stroke=\"currentColor\" stroke-width=\"2\" stroke-linecap=\"round\" stroke-linejoin=\"round\">" +
            "<rect x=\"9\" y=\"9\" width=\"13\" height=\"13\" rx=\"2\" ry=\"2\"></rect>" +
            "<path d=\"M5 15H4a2 2 0 0 1-2-2V4a2 2 0 0 1 2-2h9a2 2 0 0 1 2 2v1\"></path>" +
            "</svg></span>";

        var successIcon =
            $"<span class=\"{iconClass} {iconClass}--success\" aria-hidden=\"true\">" +
            "<svg xmlns=\"http://www.w3.org/2000/svg\" width=\"16\" height=\"16\" viewBox=\"0 0 24 24\" fill=\"none\" stroke=\"currentColor\" stroke-width=\"2\" stroke-linecap=\"round\" stroke-linejoin=\"round\">" +
            "<polyline points=\"20 6 9 17 4 12\"></polyline>" +
            "</svg></span>";

        output.Content.SetHtmlContent(copyIcon + successIcon);
    }
}
