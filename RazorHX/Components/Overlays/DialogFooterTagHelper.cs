using Microsoft.AspNetCore.Razor.TagHelpers;
using RazorHX.Rendering;

namespace RazorHX.Components.Overlays;

/// <summary>
/// Registers its child content into the parent dialog's "footer" slot.
/// Suppresses its own output — the dialog renders the footer in the correct position.
/// </summary>
[HtmlTargetElement("rhx-dialog-footer", ParentTag = "rhx-dialog")]
public class DialogFooterTagHelper : TagHelper
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
        slots.Set("footer", childContent);
        output.SuppressOutput();
    }
}
