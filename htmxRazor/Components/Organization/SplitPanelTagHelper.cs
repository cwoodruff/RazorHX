using htmxRazor.Infrastructure;
using htmxRazor.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace htmxRazor.Components.Organization;

/// <summary>
/// Renders a resizable two-panel layout with a draggable divider.
/// Child content is split between <c>&lt;rhx-split-start&gt;</c> and
/// <c>&lt;rhx-split-end&gt;</c> slot helpers. JavaScript handles drag,
/// keyboard resize, snap points, and ARIA value updates.
/// </summary>
/// <example>
/// <code>
/// &lt;rhx-split-panel rhx-position="30" rhx-snap="25,50,75"&gt;
///     &lt;rhx-split-start&gt;Sidebar&lt;/rhx-split-start&gt;
///     &lt;rhx-split-end&gt;Main content&lt;/rhx-split-end&gt;
/// &lt;/rhx-split-panel&gt;
/// </code>
/// </example>
[HtmlTargetElement("rhx-split-panel")]
public class SplitPanelTagHelper : htmxRazorTagHelperBase
{
    /// <inheritdoc/>
    protected override string BlockName => "split-panel";

    /// <summary>
    /// The initial position of the divider as a percentage (0–100). Default: 50.
    /// </summary>
    [HtmlAttributeName("rhx-position")]
    public int Position { get; set; } = 50;

    /// <summary>
    /// When true, the split is vertical (top/bottom) instead of horizontal (left/right).
    /// </summary>
    [HtmlAttributeName("rhx-vertical")]
    public bool Vertical { get; set; }

    /// <summary>
    /// When true, the divider cannot be dragged.
    /// </summary>
    [HtmlAttributeName("rhx-disabled")]
    public bool Disabled { get; set; }

    /// <summary>
    /// Which panel keeps its size when the container resizes: "start" or "end".
    /// </summary>
    [HtmlAttributeName("rhx-primary")]
    public string? Primary { get; set; }

    /// <summary>
    /// Comma-separated snap points (percentages). The divider snaps to these
    /// positions when dragged within the snap threshold.
    /// </summary>
    [HtmlAttributeName("rhx-snap")]
    public string? Snap { get; set; }

    /// <summary>
    /// Distance in pixels within which the divider snaps to a snap point. Default: 12.
    /// </summary>
    [HtmlAttributeName("rhx-snap-threshold")]
    public int SnapThreshold { get; set; } = 12;

    /// <summary>
    /// Creates a new SplitPanelTagHelper with URL generation support.
    /// </summary>
    public SplitPanelTagHelper(IUrlHelperFactory urlHelperFactory) : base(urlHelperFactory) { }

    /// <inheritdoc/>
    public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
    {
        // Set up slots
        var slots = SlotRenderer.CreateForContext(context);

        // Process children
        await output.GetChildContentAsync();

        // Outer container
        output.TagName = "div";
        output.TagMode = TagMode.StartTagAndEndTag;

        var css = CreateCssBuilder()
            .AddIf(GetModifierClass("vertical"), Vertical)
            .AddIf(GetModifierClass("disabled"), Disabled);
        ApplyBaseAttributes(output, css);

        // Data attributes for JS
        output.Attributes.SetAttribute("data-rhx-split-panel", "");
        output.Attributes.SetAttribute("data-rhx-position", Position.ToString());

        if (Vertical)
            output.Attributes.SetAttribute("data-rhx-vertical", "");

        if (Disabled)
            output.Attributes.SetAttribute("data-rhx-disabled", "");

        if (!string.IsNullOrWhiteSpace(Primary))
            output.Attributes.SetAttribute("data-rhx-primary", Primary.ToLowerInvariant());

        if (!string.IsNullOrWhiteSpace(Snap))
            output.Attributes.SetAttribute("data-rhx-snap", Snap);

        if (SnapThreshold != 12)
            output.Attributes.SetAttribute("data-rhx-snap-threshold", SnapThreshold.ToString());

        RenderHtmxAttributes(output);

        // Clamp position
        var pos = Math.Max(0, Math.Min(100, Position));

        // Assemble inner HTML
        output.Content.Clear();

        // Start panel
        output.Content.AppendHtml(
            $"<div class=\"{GetElementClass("start")}\" style=\"flex-basis: {pos}%\">");
        if (slots.Has("start"))
            output.Content.AppendHtml(slots.Get("start")!);
        output.Content.AppendHtml("</div>");

        // Divider
        var orientation = Vertical ? "horizontal" : "vertical";
        output.Content.AppendHtml(
            $"<div class=\"{GetElementClass("divider")}\" role=\"separator\"" +
            $" aria-valuenow=\"{pos}\" aria-valuemin=\"0\" aria-valuemax=\"100\"" +
            $" aria-orientation=\"{orientation}\"" +
            $" tabindex=\"{(Disabled ? "-1" : "0")}\">");
        output.Content.AppendHtml($"<div class=\"{GetElementClass("divider-handle")}\"></div>");
        output.Content.AppendHtml("</div>");

        // End panel
        output.Content.AppendHtml($"<div class=\"{GetElementClass("end")}\">");
        if (slots.Has("end"))
            output.Content.AppendHtml(slots.Get("end")!);
        output.Content.AppendHtml("</div>");
    }
}
