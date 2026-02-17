using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Razor.TagHelpers;
using RazorHX.Infrastructure;

namespace RazorHX.Components.Actions;

/// <summary>
/// Groups multiple <c>&lt;rhx-button&gt;</c> elements together with joined border
/// and radius styling. Renders as a <c>&lt;div role="group"&gt;</c>.
/// </summary>
/// <remarks>
/// <para>
/// CSS handles joined borders/radius automatically: the first child gets left radius,
/// the last child gets right radius, and middle children get no radius. Adjacent
/// borders are collapsed to avoid double-thickness borders.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// &lt;rhx-button-group label="Actions"&gt;
///     &lt;rhx-button rhx-variant="neutral"&gt;Edit&lt;/rhx-button&gt;
///     &lt;rhx-button rhx-variant="neutral"&gt;Copy&lt;/rhx-button&gt;
///     &lt;rhx-button rhx-variant="danger"&gt;Delete&lt;/rhx-button&gt;
/// &lt;/rhx-button-group&gt;
/// </code>
/// </example>
[HtmlTargetElement("rhx-button-group")]
public class ButtonGroupTagHelper : RazorHXTagHelperBase
{
    /// <inheritdoc/>
    protected override string BlockName => "button-group";

    /// <summary>
    /// Accessible label for the button group.
    /// Sets <c>aria-label</c> on the group container.
    /// </summary>
    [HtmlAttributeName("label")]
    public string? Label { get; set; }

    /// <summary>
    /// Creates a new ButtonGroupTagHelper with URL generation support.
    /// </summary>
    public ButtonGroupTagHelper(IUrlHelperFactory urlHelperFactory) : base(urlHelperFactory) { }

    /// <inheritdoc/>
    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        output.TagName = "div";
        output.TagMode = TagMode.StartTagAndEndTag;

        var css = CreateCssBuilder();
        ApplyBaseAttributes(output, css);

        AriaAttributeHelper.RoleGroup(output);

        if (!string.IsNullOrWhiteSpace(Label))
        {
            AriaAttributeHelper.AriaLabel(output, Label);
        }
    }
}
