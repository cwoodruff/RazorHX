using Microsoft.AspNetCore.Razor.TagHelpers;
using RazorHX.Rendering;

namespace RazorHX.Components.Organization;

/// <summary>
/// Registers its child content into the parent card's "header" slot.
/// Suppresses its own output — the card renders the header in the correct position.
/// </summary>
/// <example>
/// <code>
/// &lt;rhx-card-header&gt;&lt;h3&gt;Title&lt;/h3&gt;&lt;/rhx-card-header&gt;
/// </code>
/// </example>
[HtmlTargetElement("rhx-card-header", ParentTag = "rhx-card")]
public class CardHeaderTagHelper : TagHelper
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
        slots.Set("header", childContent);
        output.SuppressOutput();
    }
}
