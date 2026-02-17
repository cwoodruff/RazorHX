using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Razor.TagHelpers;
using RazorHX.Infrastructure;

namespace RazorHX.Components.Actions;

/// <summary>
/// Groups multiple <c>&lt;rhx-button&gt;</c> elements together with connected styling.
/// Renders as a <c>&lt;div role="group"&gt;</c> with appropriate ARIA semantics.
/// </summary>
/// <example>
/// <code>
/// &lt;rhx-button-group&gt;
///     &lt;rhx-button variant="Default"&gt;Left&lt;/rhx-button&gt;
///     &lt;rhx-button variant="Default"&gt;Center&lt;/rhx-button&gt;
///     &lt;rhx-button variant="Default"&gt;Right&lt;/rhx-button&gt;
/// &lt;/rhx-button-group&gt;
///
/// &lt;rhx-button-group orientation="vertical" aria-label="Alignment"&gt;
///     &lt;rhx-button&gt;Top&lt;/rhx-button&gt;
///     &lt;rhx-button&gt;Bottom&lt;/rhx-button&gt;
/// &lt;/rhx-button-group&gt;
/// </code>
/// </example>
[HtmlTargetElement("rhx-button-group")]
public class ButtonGroupTagHelper : RazorHXTagHelperBase
{
    /// <inheritdoc/>
    protected override string BlockName => "button-group";

    /// <summary>
    /// The orientation of the button group.
    /// Default: "horizontal".
    /// </summary>
    [HtmlAttributeName("orientation")]
    public string Orientation { get; set; } = "horizontal";

    /// <summary>
    /// Accessible label for the button group.
    /// </summary>
    [HtmlAttributeName("aria-label")]
    public string? AriaLabel { get; set; }

    /// <summary>
    /// Creates a new ButtonGroupTagHelper with URL generation support.
    /// </summary>
    public ButtonGroupTagHelper(IUrlHelperFactory urlHelperFactory) : base(urlHelperFactory) { }

    /// <inheritdoc/>
    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        output.TagName = "div";
        output.TagMode = TagMode.StartTagAndEndTag;

        var css = CreateCssBuilder()
            .AddIf(GetModifierClass("vertical"), Orientation == "vertical");

        ApplyBaseAttributes(output, css);
        AriaAttributeHelper.RoleGroup(output);

        if (!string.IsNullOrWhiteSpace(AriaLabel))
        {
            AriaAttributeHelper.AriaLabel(output, AriaLabel);
        }
    }
}
