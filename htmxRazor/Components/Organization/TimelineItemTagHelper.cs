using htmxRazor.Infrastructure;
using htmxRazor.Rendering;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace htmxRazor.Components.Organization;

/// <summary>
/// A single item within an <see cref="TimelineTagHelper"/>. Renders a connector
/// (line + dot) and a content region with optional label and body.
/// Supports an icon slot via <see cref="TimelineIconTagHelper"/>.
/// </summary>
[HtmlTargetElement("rhx-timeline-item", ParentTag = "rhx-timeline")]
public class TimelineItemTagHelper : htmxRazorTagHelperBase
{
    /// <inheritdoc/>
    protected override string BlockName => "timeline-item";

    /// <summary>
    /// The color variant for the connector dot.
    /// Accepted values: <c>neutral</c>, <c>brand</c>, <c>success</c>, <c>warning</c>, <c>danger</c>.
    /// </summary>
    [HtmlAttributeName("rhx-variant")]
    public string Variant { get; set; } = "neutral";

    /// <summary>
    /// Optional label text (e.g., a date or time) displayed above the body content.
    /// </summary>
    [HtmlAttributeName("rhx-label")]
    public string? Label { get; set; }

    /// <summary>
    /// Whether this item represents the current/active step.
    /// Adds a highlight ring to the dot and sets <c>aria-current="step"</c>.
    /// </summary>
    [HtmlAttributeName("rhx-active")]
    public bool Active { get; set; }

    /// <summary>
    /// Creates a new <see cref="TimelineItemTagHelper"/> instance.
    /// </summary>
    public TimelineItemTagHelper(IUrlHelperFactory urlHelperFactory) : base(urlHelperFactory) { }

    /// <inheritdoc/>
    public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
    {
        // Set up slot renderer so TimelineIconTagHelper can register content.
        // Reuse existing renderer if one was already registered (e.g., in tests).
        var slots = SlotRenderer.FromContext(context) ?? SlotRenderer.CreateForContext(context);

        // Process children first so icon slot content is captured
        var childContent = await output.GetChildContentAsync();

        output.TagName = "div";
        output.TagMode = TagMode.StartTagAndEndTag;

        var css = CreateCssBuilder()
            .AddIf(GetModifierClass(Variant), Variant != "neutral")
            .AddIf(GetModifierClass("active"), Active);

        ApplyBaseAttributes(output, css);

        output.Attributes.SetAttribute("role", "listitem");
        if (Active)
            output.Attributes.SetAttribute("aria-current", "step");

        RenderHtmxAttributes(output);

        // Connector: line + dot + line
        output.Content.AppendHtml("<div class=\"rhx-timeline-item__connector\" aria-hidden=\"true\">");
        output.Content.AppendHtml("<span class=\"rhx-timeline-item__line\"></span>");
        output.Content.AppendHtml("<span class=\"rhx-timeline-item__dot\">");
        if (slots.Has("icon"))
            output.Content.AppendHtml(slots.Get("icon")!);
        output.Content.AppendHtml("</span>");
        output.Content.AppendHtml("<span class=\"rhx-timeline-item__line\"></span>");
        output.Content.AppendHtml("</div>");

        // Content: label + body
        output.Content.AppendHtml("<div class=\"rhx-timeline-item__content\">");
        if (!string.IsNullOrWhiteSpace(Label))
        {
            output.Content.AppendHtml("<div class=\"rhx-timeline-item__label\">");
            output.Content.Append(Label); // HTML-encoded
            output.Content.AppendHtml("</div>");
        }
        output.Content.AppendHtml("<div class=\"rhx-timeline-item__body\">");
        output.Content.AppendHtml(childContent);
        output.Content.AppendHtml("</div>");
        output.Content.AppendHtml("</div>");
    }
}
