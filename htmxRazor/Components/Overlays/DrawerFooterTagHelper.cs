using htmxRazor.Rendering;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace htmxRazor.Components.Overlays;

/// <summary>
/// Registers its child content into the parent drawer's "footer" slot.
/// Suppresses its own output — the drawer renders the footer in the correct position.
/// </summary>
[HtmlTargetElement("rhx-drawer-footer", ParentTag = "rhx-drawer")]
public class DrawerFooterTagHelper : TagHelper
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
