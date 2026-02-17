using Microsoft.AspNetCore.Razor.TagHelpers;

namespace RazorHX.Components.Actions;

/// <summary>
/// Renders a visual divider between groups of dropdown items.
/// Outputs a <c>&lt;div role="separator"&gt;</c> with the appropriate BEM class.
/// </summary>
/// <example>
/// <code>
/// &lt;rhx-dropdown-item&gt;Edit&lt;/rhx-dropdown-item&gt;
/// &lt;rhx-dropdown-divider /&gt;
/// &lt;rhx-dropdown-item&gt;Delete&lt;/rhx-dropdown-item&gt;
/// </code>
/// </example>
[HtmlTargetElement("rhx-dropdown-divider")]
public class DropdownDividerTagHelper : TagHelper
{
    /// <inheritdoc/>
    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        output.TagName = "div";
        output.TagMode = TagMode.StartTagAndEndTag;
        output.Attributes.SetAttribute("class", "rhx-dropdown__divider");
        output.Attributes.SetAttribute("role", "separator");
        output.Content.Clear();
    }
}
