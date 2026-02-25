using htmxRazor.Rendering;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace htmxRazor.Components.Organization;

/// <summary>
/// Registers its child content into the parent split panel's "end" slot.
/// </summary>
/// <example>
/// <code>
/// &lt;rhx-split-end&gt;Right panel content&lt;/rhx-split-end&gt;
/// </code>
/// </example>
[HtmlTargetElement("rhx-split-end", ParentTag = "rhx-split-panel")]
public class SplitPanelEndTagHelper : TagHelper
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
        slots.Set("end", childContent);
        output.SuppressOutput();
    }
}
