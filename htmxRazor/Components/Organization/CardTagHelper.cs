using htmxRazor.Infrastructure;
using htmxRazor.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace htmxRazor.Components.Organization;

/// <summary>
/// Renders a card container with optional header, footer, and image slots.
/// Child tag helpers (<c>&lt;rhx-card-header&gt;</c>, <c>&lt;rhx-card-footer&gt;</c>,
/// <c>&lt;rhx-card-image&gt;</c>) register their content into named slots;
/// remaining child content becomes the card body.
/// </summary>
/// <example>
/// <code>
/// &lt;rhx-card&gt;
///     &lt;rhx-card-image rhx-src="/photo.jpg" rhx-alt="Product" /&gt;
///     &lt;rhx-card-header&gt;&lt;h3&gt;Product Name&lt;/h3&gt;&lt;/rhx-card-header&gt;
///     &lt;p&gt;Description text.&lt;/p&gt;
///     &lt;rhx-card-footer&gt;
///         &lt;rhx-button&gt;Buy&lt;/rhx-button&gt;
///     &lt;/rhx-card-footer&gt;
/// &lt;/rhx-card&gt;
/// </code>
/// </example>
[HtmlTargetElement("rhx-card")]
public class CardTagHelper : htmxRazorTagHelperBase
{
    /// <inheritdoc/>
    protected override string BlockName => "card";

    /// <summary>
    /// Creates a new CardTagHelper with URL generation support.
    /// </summary>
    public CardTagHelper(IUrlHelperFactory urlHelperFactory) : base(urlHelperFactory) { }

    /// <inheritdoc/>
    public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
    {
        // Set up slots for child tag helpers
        var slots = SlotRenderer.CreateForContext(context);

        // Process children — they register into slots
        var childContent = await output.GetChildContentAsync();

        // Render outer container
        output.TagName = "div";
        output.TagMode = TagMode.StartTagAndEndTag;

        var css = CreateCssBuilder();
        ApplyBaseAttributes(output, css);
        RenderHtmxAttributes(output);

        // Assemble inner HTML in order: image, header, body, footer
        output.Content.Clear();

        // Image slot
        if (slots.Has("image"))
        {
            output.Content.AppendHtml($"<div class=\"{GetElementClass("image")}\">");
            output.Content.AppendHtml(slots.Get("image")!);
            output.Content.AppendHtml("</div>");
        }

        // Header slot
        if (slots.Has("header"))
        {
            output.Content.AppendHtml($"<div class=\"{GetElementClass("header")}\">");
            output.Content.AppendHtml(slots.Get("header")!);
            output.Content.AppendHtml("</div>");
        }

        // Body — remaining child content
        var bodyHtml = childContent.GetContent();
        if (!string.IsNullOrWhiteSpace(bodyHtml))
        {
            output.Content.AppendHtml($"<div class=\"{GetElementClass("body")}\">");
            output.Content.AppendHtml(bodyHtml);
            output.Content.AppendHtml("</div>");
        }

        // Footer slot
        if (slots.Has("footer"))
        {
            output.Content.AppendHtml($"<div class=\"{GetElementClass("footer")}\">");
            output.Content.AppendHtml(slots.Get("footer")!);
            output.Content.AppendHtml("</div>");
        }
    }
}
