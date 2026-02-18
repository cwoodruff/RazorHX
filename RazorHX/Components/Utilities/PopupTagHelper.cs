using Microsoft.AspNetCore.Razor.TagHelpers;
using RazorHX.Infrastructure;

namespace RazorHX.Components.Utilities;

/// <summary>
/// A lower-level positioning utility — a container that positions itself
/// relative to an anchor element. Used internally by dropdown, tooltip,
/// popover, and select.
/// </summary>
/// <example>
/// <code>
/// &lt;button id="my-trigger"&gt;Open&lt;/button&gt;
/// &lt;rhx-popup rhx-anchor="#my-trigger" rhx-placement="bottom-start" rhx-active="true"&gt;
///     Positioned content
/// &lt;/rhx-popup&gt;
/// </code>
/// </example>
[HtmlTargetElement("rhx-popup")]
public class PopupTagHelper : RazorHXTagHelperBase
{
    protected override string BlockName => "popup";

    /// <summary>
    /// CSS selector for the anchor element to position relative to.
    /// </summary>
    [HtmlAttributeName("rhx-anchor")]
    public string? Anchor { get; set; }

    /// <summary>
    /// Placement relative to anchor: top, top-start, top-end, bottom,
    /// bottom-start, bottom-end, left, left-start, left-end, right,
    /// right-start, right-end. Default: bottom-start.
    /// </summary>
    [HtmlAttributeName("rhx-placement")]
    public string Placement { get; set; } = "bottom-start";

    /// <summary>
    /// Distance in pixels from the anchor along the placement axis. Default: 4.
    /// </summary>
    [HtmlAttributeName("rhx-distance")]
    public int Distance { get; set; } = 4;

    /// <summary>
    /// Offset in pixels along the cross axis. Default: 0.
    /// </summary>
    [HtmlAttributeName("rhx-skidding")]
    public int Skidding { get; set; }

    /// <summary>
    /// Whether the popup is currently shown. Default: false.
    /// </summary>
    [HtmlAttributeName("rhx-active")]
    public bool Active { get; set; }

    /// <summary>
    /// Positioning strategy: "absolute" or "fixed". Default: absolute.
    /// </summary>
    [HtmlAttributeName("rhx-strategy")]
    public string Strategy { get; set; } = "absolute";

    /// <summary>
    /// Whether to flip placement when the popup overflows the viewport. Default: true.
    /// </summary>
    [HtmlAttributeName("rhx-flip")]
    public bool Flip { get; set; } = true;

    /// <summary>
    /// Whether to shift the popup to keep it in viewport bounds. Default: true.
    /// </summary>
    [HtmlAttributeName("rhx-shift")]
    public bool Shift { get; set; } = true;

    /// <summary>
    /// Whether to show a directional arrow. Default: false.
    /// </summary>
    [HtmlAttributeName("rhx-arrow")]
    public bool Arrow { get; set; }

    /// <summary>
    /// Padding around the arrow from the popup edges in pixels. Default: 8.
    /// </summary>
    [HtmlAttributeName("rhx-arrow-padding")]
    public int ArrowPadding { get; set; } = 8;

    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        output.TagName = "div";
        output.TagMode = TagMode.StartTagAndEndTag;

        var css = CreateCssBuilder()
            .AddIf(GetModifierClass("active"), Active);
        ApplyBaseAttributes(output, css);

        output.Attributes.SetAttribute("data-rhx-popup", "");

        if (!string.IsNullOrWhiteSpace(Anchor))
            output.Attributes.SetAttribute("data-rhx-anchor", Anchor);

        output.Attributes.SetAttribute("data-rhx-placement", Placement.ToLowerInvariant());

        if (Distance != 4)
            output.Attributes.SetAttribute("data-rhx-distance", Distance.ToString());

        if (Skidding != 0)
            output.Attributes.SetAttribute("data-rhx-skidding", Skidding.ToString());

        if (!Strategy.Equals("absolute", StringComparison.OrdinalIgnoreCase))
            output.Attributes.SetAttribute("data-rhx-strategy", Strategy.ToLowerInvariant());

        if (!Flip)
            output.Attributes.SetAttribute("data-rhx-no-flip", "");

        if (!Shift)
            output.Attributes.SetAttribute("data-rhx-no-shift", "");

        if (Arrow)
        {
            output.Attributes.SetAttribute("data-rhx-arrow", "");
            if (ArrowPadding != 8)
                output.Attributes.SetAttribute("data-rhx-arrow-padding", ArrowPadding.ToString());
        }

        if (!Active)
        {
            output.Attributes.SetAttribute("aria-hidden", "true");
            output.Attributes.SetAttribute("hidden", "hidden");
        }

        if (Arrow)
            output.PostContent.AppendHtml("<div class=\"rhx-popup__arrow\" data-rhx-popup-arrow></div>");

        RenderHtmxAttributes(output);
    }
}
