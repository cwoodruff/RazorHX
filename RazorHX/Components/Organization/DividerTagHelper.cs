using Microsoft.AspNetCore.Razor.TagHelpers;
using RazorHX.Infrastructure;

namespace RazorHX.Components.Organization;

/// <summary>
/// Renders a visual divider (horizontal rule or vertical separator).
/// Horizontal dividers render as <c>&lt;hr&gt;</c>; vertical dividers
/// render as <c>&lt;div&gt;</c> with a vertical modifier class.
/// </summary>
/// <example>
/// <code>
/// &lt;!-- Horizontal --&gt;
/// &lt;rhx-divider /&gt;
///
/// &lt;!-- Vertical --&gt;
/// &lt;rhx-divider rhx-vertical /&gt;
/// </code>
/// </example>
[HtmlTargetElement("rhx-divider")]
public class DividerTagHelper : RazorHXTagHelperBase
{
    /// <inheritdoc/>
    protected override string BlockName => "divider";

    /// <summary>
    /// When true, renders a vertical divider instead of a horizontal rule.
    /// </summary>
    [HtmlAttributeName("rhx-vertical")]
    public bool Vertical { get; set; }

    /// <inheritdoc/>
    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        var css = CreateCssBuilder()
            .AddIf(GetModifierClass("vertical"), Vertical);

        if (Vertical)
        {
            output.TagName = "div";
            output.TagMode = TagMode.StartTagAndEndTag;
            ApplyBaseAttributes(output, css);
            output.Attributes.SetAttribute("role", "separator");
            output.Attributes.SetAttribute("aria-orientation", "vertical");
        }
        else
        {
            output.TagName = "hr";
            output.TagMode = TagMode.SelfClosing;
            ApplyBaseAttributes(output, css);
        }
    }
}
