using htmxRazor.Rendering;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace htmxRazor.Components.Organization;

/// <summary>
/// Slot child for <see cref="TimelineItemTagHelper"/> that provides custom
/// icon content to replace the default dot indicator.
/// </summary>
/// <example>
/// <code>
/// &lt;rhx-timeline-item rhx-variant="success"&gt;
///     &lt;rhx-timeline-icon&gt;
///         &lt;rhx-icon name="check" /&gt;
///     &lt;/rhx-timeline-icon&gt;
///     Order completed
/// &lt;/rhx-timeline-item&gt;
/// </code>
/// </example>
[HtmlTargetElement("rhx-timeline-icon", ParentTag = "rhx-timeline-item")]
public class TimelineIconTagHelper : TagHelper
{
    /// <inheritdoc/>
    public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
    {
        var slots = SlotRenderer.FromContext(context);
        if (slots == null)
        {
            output.SuppressOutput();
            return;
        }

        var childContent = await output.GetChildContentAsync();
        slots.Set("icon", childContent);
        output.SuppressOutput();
    }
}
