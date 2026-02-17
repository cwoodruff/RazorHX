using Microsoft.AspNetCore.Razor.TagHelpers;
using RazorHX.Infrastructure;

namespace RazorHX.Components.Actions;

/// <summary>
/// Groups multiple rhx-button elements together with connected styling.
/// Usage: &lt;rhx-button-group&gt;&lt;rhx-button&gt;A&lt;/rhx-button&gt;&lt;rhx-button&gt;B&lt;/rhx-button&gt;&lt;/rhx-button-group&gt;
/// </summary>
[HtmlTargetElement("rhx-button-group")]
public class ButtonGroupTagHelper : RazorHXTagHelperBase
{
    protected override string BlockName => "button-group";

    [HtmlAttributeName("orientation")]
    public string Orientation { get; set; } = "horizontal";

    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        output.TagName = "div";
        output.TagMode = TagMode.StartTagAndEndTag;

        var css = CreateCssBuilder()
            .AddIf(GetModifierClass("vertical"), Orientation == "vertical");

        ApplyBaseAttributes(output, css);
        AriaAttributeHelper.SetRole(output, "group");
    }
}
