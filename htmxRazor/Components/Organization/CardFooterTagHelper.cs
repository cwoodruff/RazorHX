using htmxRazor.Rendering;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace htmxRazor.Components.Organization;

/// <summary>
/// Registers its child content into the parent card's "footer" slot.
/// Suppresses its own output — the card renders the footer in the correct position.
/// </summary>
/// <example>
/// <code>
/// &lt;rhx-card-footer&gt;
///     &lt;rhx-button&gt;Action&lt;/rhx-button&gt;
/// &lt;/rhx-card-footer&gt;
/// </code>
/// </example>
[HtmlTargetElement("rhx-card-footer", ParentTag = "rhx-card")]
public class CardFooterTagHelper : TagHelper
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
