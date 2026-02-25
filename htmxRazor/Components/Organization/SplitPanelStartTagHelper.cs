using htmxRazor.Rendering;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace htmxRazor.Components.Organization;

/// <summary>
/// Registers its child content into the parent split panel's "start" slot.
/// </summary>
/// <example>
/// <code>
/// &lt;rhx-split-start&gt;Left panel content&lt;/rhx-split-start&gt;
/// </code>
/// </example>
[HtmlTargetElement("rhx-split-start", ParentTag = "rhx-split-panel")]
public class SplitPanelStartTagHelper : TagHelper
{
    /// <inheritdoc/>
    public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
    {
        var slots = SlotRenderer.FromContext(context);
        if (slots == null)
        {
            output.TagName = null;
            return;
        }

        var childContent = await output.GetChildContentAsync();
        slots.Set("start", childContent);
        output.SuppressOutput();
    }
}
