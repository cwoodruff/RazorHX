using Microsoft.AspNetCore.Razor.TagHelpers;
using RazorHX.Infrastructure;

namespace RazorHX.Components.Utilities;

/// <summary>
/// A content-rich popover supporting arbitrary HTML content.
/// More feature-rich than a tooltip — supports click/hover/focus triggers,
/// arrow placement, and rich HTML content.
/// </summary>
/// <example>
/// <code>
/// &lt;button id="pop-trigger"&gt;Info&lt;/button&gt;
/// &lt;rhx-popover rhx-trigger="#pop-trigger"&gt;
///     &lt;h3&gt;Details&lt;/h3&gt;
///     &lt;p&gt;Rich content here.&lt;/p&gt;
/// &lt;/rhx-popover&gt;
///
/// &lt;button id="hover-pop"&gt;Hover me&lt;/button&gt;
/// &lt;rhx-popover rhx-trigger="#hover-pop" rhx-trigger-event="hover"&gt;
///     Hover popover content
/// &lt;/rhx-popover&gt;
/// </code>
/// </example>
[HtmlTargetElement("rhx-popover")]
public class PopoverTagHelper : RazorHXTagHelperBase
{
    protected override string BlockName => "popover";

    /// <summary>
    /// CSS selector for the trigger element, or "previous" for previous sibling.
    /// </summary>
    [HtmlAttributeName("rhx-trigger")]
    public string? Trigger { get; set; }

    /// <summary>
    /// Whether to show a directional arrow. Default: true.
    /// </summary>
    [HtmlAttributeName("rhx-arrow")]
    public bool Arrow { get; set; } = true;

    /// <summary>
    /// Placement relative to trigger: top, top-start, top-end, bottom,
    /// bottom-start, bottom-end, left, right. Default: bottom.
    /// </summary>
    [HtmlAttributeName("rhx-placement")]
    public string Placement { get; set; } = "bottom";

    /// <summary>
    /// Distance in pixels from the trigger element. Default: 8.
    /// </summary>
    [HtmlAttributeName("rhx-distance")]
    public int Distance { get; set; } = 8;

    /// <summary>
    /// Whether the popover is initially open. Default: false.
    /// </summary>
    [HtmlAttributeName("rhx-open")]
    public bool Open { get; set; }

    /// <summary>
    /// Trigger event: "click" (default), "hover", or "focus".
    /// </summary>
    [HtmlAttributeName("rhx-trigger-event")]
    public string TriggerEvent { get; set; } = "click";

    public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
    {
        output.TagName = "div";
        output.TagMode = TagMode.StartTagAndEndTag;

        var popoverId = $"rhx-popover-{context.UniqueId}";

        var css = CreateCssBuilder()
            .AddIf(GetModifierClass("open"), Open);
        ApplyBaseAttributes(output, css);

        output.Attributes.SetAttribute("data-rhx-popover", "");
        output.Attributes.SetAttribute("id", Id ?? popoverId);

        if (!string.IsNullOrWhiteSpace(Trigger))
            output.Attributes.SetAttribute("data-rhx-trigger", Trigger);

        output.Attributes.SetAttribute("data-rhx-placement", Placement.ToLowerInvariant());

        if (Distance != 8)
            output.Attributes.SetAttribute("data-rhx-distance", Distance.ToString());

        var triggerEvent = TriggerEvent.ToLowerInvariant();
        if (triggerEvent != "click")
            output.Attributes.SetAttribute("data-rhx-trigger-event", triggerEvent);

        if (!Open)
        {
            output.Attributes.SetAttribute("aria-hidden", "true");
            output.Attributes.SetAttribute("hidden", "hidden");
        }

        var childContent = await output.GetChildContentAsync();

        output.Content.AppendHtml("<div class=\"rhx-popover__content\">");
        output.Content.AppendHtml(childContent);
        output.Content.AppendHtml("</div>");

        if (Arrow)
            output.Content.AppendHtml("<div class=\"rhx-popover__arrow\"></div>");

        RenderHtmxAttributes(output);
    }
}
