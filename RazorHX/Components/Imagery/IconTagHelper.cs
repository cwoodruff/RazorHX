using System.Net;
using Microsoft.AspNetCore.Razor.TagHelpers;
using RazorHX.Infrastructure;

namespace RazorHX.Components.Imagery;

/// <summary>
/// Renders an inline SVG icon from the <see cref="IconRegistry"/>.
/// When <c>rhx-label</c> is set, the icon is accessible with <c>aria-label</c>;
/// otherwise it is decorative with <c>aria-hidden="true"</c>.
/// </summary>
/// <example>
/// <code>
/// &lt;rhx-icon rhx-name="search" /&gt;
/// &lt;rhx-icon rhx-name="settings" rhx-label="Settings" rhx-size="large" /&gt;
/// </code>
/// </example>
[HtmlTargetElement("rhx-icon")]
public class IconTagHelper : TagHelper
{
    /// <summary>
    /// The icon name (must be registered in <see cref="IconRegistry"/>).
    /// </summary>
    [HtmlAttributeName("rhx-name")]
    public string Name { get; set; } = "";

    /// <summary>
    /// Accessible label. When set, uses <c>role="img"</c> and <c>aria-label</c>
    /// instead of <c>aria-hidden="true"</c>.
    /// </summary>
    [HtmlAttributeName("rhx-label")]
    public string? Label { get; set; }

    /// <summary>
    /// Icon size: small (16px), medium (24px, default), large (32px).
    /// </summary>
    [HtmlAttributeName("rhx-size")]
    public string? Size { get; set; }

    /// <summary>
    /// Additional CSS classes.
    /// </summary>
    [HtmlAttributeName("class")]
    public string? CssClass { get; set; }

    /// <inheritdoc/>
    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        var svgContent = IconRegistry.Get(Name);
        if (svgContent is null)
        {
            output.SuppressOutput();
            return;
        }

        output.TagName = "svg";
        output.TagMode = TagMode.StartTagAndEndTag;

        // CSS classes
        var css = new CssClassBuilder("rhx-icon");
        if (!string.IsNullOrWhiteSpace(Size))
            css.Add($"rhx-icon--{Size.ToLowerInvariant()}");
        if (!string.IsNullOrWhiteSpace(CssClass))
            css.Add(CssClass);
        output.Attributes.SetAttribute("class", css.Build());

        // SVG attributes
        output.Attributes.SetAttribute("viewBox", "0 0 24 24");
        output.Attributes.SetAttribute("fill", "none");
        output.Attributes.SetAttribute("stroke", "currentColor");
        output.Attributes.SetAttribute("stroke-width", "2");
        output.Attributes.SetAttribute("stroke-linecap", "round");
        output.Attributes.SetAttribute("stroke-linejoin", "round");

        // Accessibility
        if (!string.IsNullOrWhiteSpace(Label))
        {
            output.Attributes.SetAttribute("role", "img");
            output.Attributes.SetAttribute("aria-label", Label);
        }
        else
        {
            output.Attributes.SetAttribute("aria-hidden", "true");
        }

        output.Content.SetHtmlContent(svgContent);
    }
}
