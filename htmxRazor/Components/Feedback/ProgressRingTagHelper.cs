using System.Globalization;
using htmxRazor.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace htmxRazor.Components.Feedback;

/// <summary>
/// Renders a circular SVG progress ring with track, fill arc, and optional center label.
/// </summary>
/// <example>
/// <code>
/// &lt;rhx-progress-ring rhx-value="65" rhx-label="Upload" /&gt;
///
/// &lt;rhx-progress-ring rhx-value="80" rhx-track-width="4" rhx-indicator-width="4" /&gt;
/// </code>
/// </example>
[HtmlTargetElement("rhx-progress-ring")]
public class ProgressRingTagHelper : htmxRazorTagHelperBase
{
    /// <inheritdoc/>
    protected override string BlockName => "progress-ring";

    // Radius chosen so circumference ≈ 100 for easy percentage math
    private const string Radius = "15.9155";
    private const string Center = "18";
    private const string ViewBox = "0 0 36 36";

    // ──────────────────────────────────────────────
    //  Properties
    // ──────────────────────────────────────────────

    /// <summary>
    /// The current progress value (0–100). Default: 0.
    /// </summary>
    [HtmlAttributeName("rhx-value")]
    public int Value { get; set; }

    /// <summary>
    /// Accessible label for the progress ring (sets aria-label).
    /// </summary>
    [HtmlAttributeName("rhx-label")]
    public string? Label { get; set; }

    /// <summary>
    /// Stroke width of the background track circle. Default: 3.
    /// </summary>
    [HtmlAttributeName("rhx-track-width")]
    public int TrackWidth { get; set; } = 3;

    /// <summary>
    /// Stroke width of the progress indicator arc. Default: 3.
    /// </summary>
    [HtmlAttributeName("rhx-indicator-width")]
    public int IndicatorWidth { get; set; } = 3;

    // ──────────────────────────────────────────────
    //  Constructor
    // ──────────────────────────────────────────────

    public ProgressRingTagHelper(IUrlHelperFactory urlHelperFactory) : base(urlHelperFactory) { }

    // ──────────────────────────────────────────────
    //  Rendering
    // ──────────────────────────────────────────────

    /// <inheritdoc/>
    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        output.TagName = "svg";
        output.TagMode = TagMode.StartTagAndEndTag;

        var css = CreateCssBuilder();
        ApplyBaseAttributes(output, css);

        output.Attributes.SetAttribute("viewBox", ViewBox);
        output.Attributes.SetAttribute("role", "progressbar");

        var clampedValue = Math.Clamp(Value, 0, 100);

        output.Attributes.SetAttribute("aria-valuenow", clampedValue.ToString());
        output.Attributes.SetAttribute("aria-valuemin", "0");
        output.Attributes.SetAttribute("aria-valuemax", "100");

        if (!string.IsNullOrEmpty(Label))
            output.Attributes.SetAttribute("aria-label", Label);

        // ── Build SVG content ──
        output.Content.Clear();

        // Track circle (full background ring)
        output.Content.AppendHtml(
            $"<circle class=\"{GetElementClass("track")}\" cx=\"{Center}\" cy=\"{Center}\" r=\"{Radius}\" " +
            $"fill=\"none\" stroke-width=\"{TrackWidth}\" />");

        // Fill circle (progress arc, rotated to start from top)
        output.Content.AppendHtml(
            $"<circle class=\"{GetElementClass("fill")}\" cx=\"{Center}\" cy=\"{Center}\" r=\"{Radius}\" " +
            $"fill=\"none\" stroke-width=\"{IndicatorWidth}\" " +
            $"stroke-dasharray=\"{clampedValue} 100\" stroke-linecap=\"round\" " +
            $"transform=\"rotate(-90 {Center} {Center})\" />");

        // Center label text
        output.Content.AppendHtml(
            $"<text class=\"{GetElementClass("label")}\" x=\"{Center}\" y=\"20.5\" " +
            $"text-anchor=\"middle\">{clampedValue}%</text>");

        // ── htmx attributes ──
        RenderHtmxAttributes(output);
    }
}
