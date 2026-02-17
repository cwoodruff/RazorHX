using System.Net;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Razor.TagHelpers;
using RazorHX.Infrastructure;

namespace RazorHX.Components.Feedback;

/// <summary>
/// Renders a tag/chip with label text, optional remove button, and htmx integration.
/// Named TagItem to avoid conflict with System.Tag.
/// </summary>
/// <example>
/// <code>
/// &lt;rhx-tag rhx-variant="brand"&gt;C#&lt;/rhx-tag&gt;
///
/// &lt;rhx-tag rhx-variant="danger" rhx-removable="true"
///           hx-delete="/tags/42" hx-trigger="click from:find .rhx-tag__remove"
///           hx-target="closest .rhx-tag" hx-swap="outerHTML swap:200ms"&gt;
///     Deprecated
/// &lt;/rhx-tag&gt;
/// </code>
/// </example>
[HtmlTargetElement("rhx-tag")]
public class TagItemTagHelper : RazorHXTagHelperBase
{
    /// <inheritdoc/>
    protected override string BlockName => "tag";

    // ──────────────────────────────────────────────
    //  Tag-specific properties
    // ──────────────────────────────────────────────

    /// <summary>
    /// The color variant of the tag.
    /// Options: neutral, brand, success, warning, danger.
    /// Default: neutral.
    /// </summary>
    [HtmlAttributeName("rhx-variant")]
    public string Variant { get; set; } = "neutral";

    /// <summary>
    /// The size of the tag.
    /// Options: small, medium, large.
    /// Default: medium.
    /// </summary>
    [HtmlAttributeName("rhx-size")]
    public string Size { get; set; } = "medium";

    /// <summary>
    /// Renders the tag with fully rounded (pill) corners. Default: false.
    /// </summary>
    [HtmlAttributeName("rhx-pill")]
    public bool Pill { get; set; }

    /// <summary>
    /// Shows a remove button on the tag. Default: false.
    /// </summary>
    [HtmlAttributeName("rhx-removable")]
    public bool Removable { get; set; }

    // ──────────────────────────────────────────────
    //  Constructor
    // ──────────────────────────────────────────────

    public TagItemTagHelper(IUrlHelperFactory urlHelperFactory) : base(urlHelperFactory) { }

    // ──────────────────────────────────────────────
    //  Rendering
    // ──────────────────────────────────────────────

    /// <inheritdoc/>
    public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
    {
        output.TagName = "span";
        output.TagMode = TagMode.StartTagAndEndTag;

        var variant = Variant.ToLowerInvariant();
        var size = Size.ToLowerInvariant();

        var css = CreateCssBuilder()
            .Add(GetModifierClass(variant))
            .AddIf(GetModifierClass(size), size != "medium")
            .AddIf(GetModifierClass("pill"), Pill)
            .AddIf(GetModifierClass("removable"), Removable);

        ApplyBaseAttributes(output, css);

        // ── htmx attributes on the tag element ──
        RenderHtmxAttributes(output);

        // ── Build inner content ──
        var childContent = await output.GetChildContentAsync();
        output.Content.Clear();

        // Label wrapper
        output.Content.AppendHtml($"<span class=\"{GetElementClass("label")}\">");
        output.Content.AppendHtml(childContent);
        output.Content.AppendHtml("</span>");

        // Remove button
        if (Removable)
        {
            output.Content.AppendHtml(
                $"<button class=\"{GetElementClass("remove")}\" type=\"button\" aria-label=\"Remove\">" +
                "<svg xmlns=\"http://www.w3.org/2000/svg\" width=\"12\" height=\"12\" viewBox=\"0 0 24 24\" fill=\"none\" stroke=\"currentColor\" stroke-width=\"2\" stroke-linecap=\"round\" stroke-linejoin=\"round\">" +
                "<line x1=\"18\" y1=\"6\" x2=\"6\" y2=\"18\"></line><line x1=\"6\" y1=\"6\" x2=\"18\" y2=\"18\"></line>" +
                "</svg></button>");
        }
    }
}
