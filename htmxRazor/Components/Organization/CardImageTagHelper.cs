using System.Net;
using htmxRazor.Rendering;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace htmxRazor.Components.Organization;

/// <summary>
/// Registers an image into the parent card's "image" slot.
/// Renders an <c>&lt;img&gt;</c> element from the <c>rhx-src</c> and <c>rhx-alt</c> properties.
/// </summary>
/// <example>
/// <code>
/// &lt;rhx-card-image rhx-src="/photo.jpg" rhx-alt="Product photo" /&gt;
/// </code>
/// </example>
[HtmlTargetElement("rhx-card-image", ParentTag = "rhx-card")]
public class CardImageTagHelper : TagHelper
{
    /// <summary>
    /// The image source URL.
    /// </summary>
    [HtmlAttributeName("rhx-src")]
    public string Src { get; set; } = "";

    /// <summary>
    /// The image alt text.
    /// </summary>
    [HtmlAttributeName("rhx-alt")]
    public string Alt { get; set; } = "";

    /// <inheritdoc/>
    public override Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
    {
        var slots = SlotRenderer.FromContext(context);
        if (slots == null)
        {
            output.TagName = null;
            return Task.CompletedTask;
        }

        var imgHtml = $"<img src=\"{Enc(Src)}\" alt=\"{Enc(Alt)}\" />";
        slots.SetHtml("image", imgHtml);
        output.SuppressOutput();
        return Task.CompletedTask;
    }

    private static string Enc(string? value) => WebUtility.HtmlEncode(value ?? "") ?? "";
}
