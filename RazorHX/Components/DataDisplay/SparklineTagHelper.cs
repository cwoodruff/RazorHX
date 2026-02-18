using System.Globalization;
using System.Text;
using Microsoft.AspNetCore.Razor.TagHelpers;
using RazorHX.Infrastructure;

namespace RazorHX.Components.DataDisplay;

/// <summary>
/// Renders an inline SVG sparkline chart. All rendering is done server-side —
/// no JavaScript required. Supports line, area, and bar chart types.
/// </summary>
/// <example>
/// <code>
/// &lt;rhx-sparkline rhx-values="@Model.CpuHistory" /&gt;
/// &lt;rhx-sparkline rhx-values="@Model.Sales" rhx-type="area"
///                rhx-stroke-color="#10b981" rhx-fill-color="#10b98133" /&gt;
/// &lt;rhx-sparkline rhx-values="@Model.Scores" rhx-type="bar" rhx-fill-color="#3b82f6" /&gt;
/// </code>
/// </example>
[HtmlTargetElement("rhx-sparkline")]
public class SparklineTagHelper : TagHelper
{
    // ── ViewBox dimensions (internal coordinate system) ──
    private const double VbWidth = 200;
    private const double VbHeight = 40;
    private const double Padding = 2;

    // ──────────────────────────────────────────────
    //  Properties
    // ──────────────────────────────────────────────

    /// <summary>
    /// The data points to plot.
    /// </summary>
    [HtmlAttributeName("rhx-values")]
    public double[]? Values { get; set; }

    /// <summary>
    /// Chart type: "line" (default), "area", or "bar".
    /// </summary>
    [HtmlAttributeName("rhx-type")]
    public string Type { get; set; } = "line";

    /// <summary>
    /// Minimum value for the Y axis. Auto-computed from data if not set.
    /// </summary>
    [HtmlAttributeName("rhx-min")]
    public double? Min { get; set; }

    /// <summary>
    /// Maximum value for the Y axis. Auto-computed from data if not set.
    /// </summary>
    [HtmlAttributeName("rhx-max")]
    public double? Max { get; set; }

    /// <summary>
    /// CSS width of the SVG element. Default: "100%".
    /// </summary>
    [HtmlAttributeName("rhx-width")]
    public string Width { get; set; } = "100%";

    /// <summary>
    /// CSS height of the SVG element. Default: "2rem".
    /// </summary>
    [HtmlAttributeName("rhx-height")]
    public string Height { get; set; } = "2rem";

    /// <summary>
    /// Stroke width for line/area charts. Default: 2.
    /// </summary>
    [HtmlAttributeName("rhx-stroke-width")]
    public double StrokeWidth { get; set; } = 2;

    /// <summary>
    /// Stroke color (CSS color value). Default: "currentColor".
    /// </summary>
    [HtmlAttributeName("rhx-stroke-color")]
    public string StrokeColor { get; set; } = "currentColor";

    /// <summary>
    /// Fill color for area/bar types (CSS color value).
    /// Defaults to stroke color at 20% opacity for area, full opacity for bar.
    /// </summary>
    [HtmlAttributeName("rhx-fill-color")]
    public string? FillColor { get; set; }

    /// <summary>
    /// Accessible label for the sparkline (sets aria-label on the SVG).
    /// </summary>
    [HtmlAttributeName("rhx-label")]
    public string? Label { get; set; }

    /// <summary>
    /// Additional CSS classes.
    /// </summary>
    [HtmlAttributeName("class")]
    public string? CssClass { get; set; }

    // ──────────────────────────────────────────────
    //  Rendering
    // ──────────────────────────────────────────────

    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        output.TagName = "svg";
        output.TagMode = TagMode.StartTagAndEndTag;

        var classes = string.IsNullOrWhiteSpace(CssClass)
            ? "rhx-sparkline"
            : $"rhx-sparkline {CssClass}";
        output.Attributes.SetAttribute("class", classes);

        var viewBox = $"0 0 {F(VbWidth)} {F(VbHeight)}";
        output.Attributes.SetAttribute("viewBox", viewBox);
        output.Attributes.SetAttribute("preserveAspectRatio", "none");
        output.Attributes.SetAttribute("role", "img");

        if (!string.IsNullOrWhiteSpace(Label))
            output.Attributes.SetAttribute("aria-label", Label);
        else
            output.Attributes.SetAttribute("aria-hidden", "true");

        output.Attributes.SetAttribute("style", $"width:{Width};height:{Height}");

        // ── Generate SVG content ──
        output.Content.Clear();

        if (Values == null || Values.Length == 0)
            return;

        var type = Type?.ToLowerInvariant() ?? "line";

        switch (type)
        {
            case "bar":
                RenderBars(output);
                break;
            case "area":
                RenderArea(output);
                RenderLine(output);
                break;
            default: // line
                RenderLine(output);
                break;
        }
    }

    // ──────────────────────────────────────────────
    //  SVG rendering methods
    // ──────────────────────────────────────────────

    private void RenderLine(TagHelperOutput output)
    {
        var points = ComputePoints(Values!, Min, Max, VbWidth, VbHeight, Padding);

        output.Content.AppendHtml(
            $"<polyline class=\"rhx-sparkline__line\" fill=\"none\" " +
            $"stroke=\"{StrokeColor}\" stroke-width=\"{F(StrokeWidth)}\" " +
            $"stroke-linejoin=\"round\" stroke-linecap=\"round\" " +
            $"points=\"{FormatPoints(points)}\" />");
    }

    private void RenderArea(TagHelperOutput output)
    {
        var points = ComputePoints(Values!, Min, Max, VbWidth, VbHeight, Padding);
        var fill = FillColor ?? $"{StrokeColor}33";

        // Polygon: start at bottom-left, trace the line, close at bottom-right
        var areaPoints = new List<(double x, double y)>
        {
            (points[0].x, VbHeight)
        };
        areaPoints.AddRange(points);
        areaPoints.Add((points[^1].x, VbHeight));

        output.Content.AppendHtml(
            $"<polygon class=\"rhx-sparkline__area\" " +
            $"fill=\"{fill}\" " +
            $"points=\"{FormatPoints(areaPoints)}\" />");
    }

    private void RenderBars(TagHelperOutput output)
    {
        var values = Values!;
        var count = values.Length;
        var min = Min ?? values.Min();
        var max = Max ?? values.Max();

        if (Math.Abs(max - min) < double.Epsilon)
        {
            max = min + 1;
        }

        var barGap = 1.0;
        var totalGap = barGap * (count - 1);
        var barWidth = (VbWidth - totalGap) / count;
        if (barWidth < 1) barWidth = 1;

        var fill = FillColor ?? StrokeColor;
        var usableHeight = VbHeight - Padding * 2;

        for (var i = 0; i < count; i++)
        {
            var normalized = (values[i] - min) / (max - min);
            normalized = Math.Clamp(normalized, 0, 1);
            var barHeight = Math.Max(normalized * usableHeight, 0.5);
            var x = i * (barWidth + barGap);
            var y = VbHeight - Padding - barHeight;

            output.Content.AppendHtml(
                $"<rect class=\"rhx-sparkline__bar\" " +
                $"x=\"{F(x)}\" y=\"{F(y)}\" " +
                $"width=\"{F(barWidth)}\" height=\"{F(barHeight)}\" " +
                $"fill=\"{fill}\" />");
        }
    }

    // ──────────────────────────────────────────────
    //  Point computation (public for testability)
    // ──────────────────────────────────────────────

    /// <summary>
    /// Computes SVG coordinate points from data values.
    /// X positions are evenly distributed across the viewBox width.
    /// Y positions are inverted (SVG y goes down) and padded.
    /// </summary>
    /// <param name="values">Data values.</param>
    /// <param name="min">Minimum value (auto-computed if null).</param>
    /// <param name="max">Maximum value (auto-computed if null).</param>
    /// <param name="width">ViewBox width.</param>
    /// <param name="height">ViewBox height.</param>
    /// <param name="padding">Vertical padding.</param>
    /// <returns>Array of (x, y) coordinate pairs.</returns>
    public static (double x, double y)[] ComputePoints(
        double[] values, double? min, double? max,
        double width, double height, double padding)
    {
        if (values.Length == 0)
            return [];

        var dataMin = min ?? values.Min();
        var dataMax = max ?? values.Max();

        // Prevent division by zero when all values are the same
        if (Math.Abs(dataMax - dataMin) < double.Epsilon)
        {
            dataMax = dataMin + 1;
        }

        var usableHeight = height - padding * 2;
        var count = values.Length;
        var points = new (double x, double y)[count];

        for (var i = 0; i < count; i++)
        {
            // X: evenly distributed across width
            var x = count == 1 ? width / 2 : (double)i / (count - 1) * width;

            // Y: normalize to 0-1, invert (high values at top), apply padding
            var normalized = (values[i] - dataMin) / (dataMax - dataMin);
            normalized = Math.Clamp(normalized, 0, 1);
            var y = height - padding - normalized * usableHeight;

            points[i] = (Math.Round(x, 2), Math.Round(y, 2));
        }

        return points;
    }

    /// <summary>
    /// Formats an array of coordinate points as an SVG points attribute string.
    /// </summary>
    public static string FormatPoints(IEnumerable<(double x, double y)> points)
    {
        return string.Join(" ", points.Select(p => $"{F(p.x)},{F(p.y)}"));
    }

    // Format a double with invariant culture, no trailing zeros
    private static string F(double value)
    {
        return value.ToString("G", CultureInfo.InvariantCulture);
    }
}
