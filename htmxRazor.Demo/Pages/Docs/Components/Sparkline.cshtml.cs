using Microsoft.AspNetCore.Mvc.RazorPages;
using htmxRazor.Components.Navigation;
using htmxRazor.Demo.Models;

namespace htmxRazor.Demo.Pages.Docs.Components;

public class SparklineModel : PageModel
{
    public double[] CpuHistory { get; set; } = [];
    public double[] Sales { get; set; } = [];
    public double[] Scores { get; set; } = [];
    public double[] Temperature { get; set; } = [];
    public double[] StockPrice { get; set; } = [];
    public double[] NegativeValues { get; set; } = [];

    public List<ComponentProperty> Properties { get; } = new()
    {
        new("rhx-values", "double[]", "-", "Array of numeric data points to plot"),
        new("rhx-type", "string", "line", "Chart type: line, area, bar"),
        new("rhx-min", "double?", "auto", "Minimum Y-axis value (auto-calculated from data if omitted)"),
        new("rhx-max", "double?", "auto", "Maximum Y-axis value (auto-calculated from data if omitted)"),
        new("rhx-width", "string", "100%", "SVG width as CSS value"),
        new("rhx-height", "string", "2rem", "SVG height as CSS value"),
        new("rhx-stroke-width", "double", "2", "Line stroke width in pixels"),
        new("rhx-stroke-color", "string", "currentColor", "Stroke color (any CSS color value)"),
        new("rhx-fill-color", "string?", "-", "Fill color for area/bar types (area defaults to stroke at 20% opacity)"),
        new("rhx-label", "string?", "-", "Accessible label; sets role=\"img\" and aria-label. If omitted, aria-hidden=\"true\""),
    };

    public string LineCode => @"<!-- Default line sparkline -->
<rhx-sparkline rhx-values=""new[] { 23, 45, 67, 34, 56, 78, 45, 89 }""
               rhx-label=""CPU history"" />

<!-- Custom stroke color and width -->
<rhx-sparkline rhx-values=""new[] { 142.5, 145.2, 143.8, 148.1, 150.6 }""
               rhx-stroke-color=""#6366f1""
               rhx-stroke-width=""3""
               rhx-label=""Stock price trend"" />

<!-- Custom size -->
<rhx-sparkline rhx-values=""new[] { 18, 19, 21, 24, 26, 28, 30, 29 }""
               rhx-width=""300px""
               rhx-height=""3rem""
               rhx-stroke-color=""#f59e0b""
               rhx-label=""Daily temperatures"" />";

    public string AreaCode => @"<!-- Area with auto fill (stroke color at 20% opacity) -->
<rhx-sparkline rhx-values=""new[] { 120, 200, 150, 80, 230, 180, 260 }""
               rhx-type=""area""
               rhx-stroke-color=""#10b981""
               rhx-label=""Monthly sales"" />

<!-- Area with custom fill color -->
<rhx-sparkline rhx-values=""new[] { 23, 45, 67, 34, 56, 78, 45, 89 }""
               rhx-type=""area""
               rhx-stroke-color=""#3b82f6""
               rhx-fill-color=""#3b82f633""
               rhx-label=""CPU usage area chart"" />";

    public string BarCode => @"<!-- Bar chart with custom fill -->
<rhx-sparkline rhx-values=""new[] { 8, 6, 9, 7, 5, 8, 10, 6, 7, 9 }""
               rhx-type=""bar""
               rhx-fill-color=""#3b82f6""
               rhx-label=""Score distribution"" />

<!-- Bar chart with default fill (uses stroke color) -->
<rhx-sparkline rhx-values=""new[] { 120, 200, 150, 80, 230, 180, 260 }""
               rhx-type=""bar""
               rhx-stroke-color=""#8b5cf6""
               rhx-label=""Monthly sales bars"" />";

    public string MinMaxCode => @"<!-- Fixed Y-axis range: 0 to 100 -->
<rhx-sparkline rhx-values=""new[] { 23, 45, 67, 34, 56, 78, 45, 89 }""
               rhx-min=""0""
               rhx-max=""100""
               rhx-stroke-color=""#ef4444""
               rhx-label=""CPU percentage (0-100)"" />

<!-- Negative values with area fill -->
<rhx-sparkline rhx-values=""new[] { -10, -5, 0, 5, 10, 15, 10, 5, 0, -5 }""
               rhx-type=""area""
               rhx-stroke-color=""#f97316""
               rhx-fill-color=""#f9731633""
               rhx-label=""Values crossing zero"" />";

    public string InlineCode => @"<p>
    CPU usage
    <rhx-sparkline rhx-values=""@Model.CpuHistory""
                   rhx-width=""80px"" rhx-height=""1rem""
                   rhx-stroke-color=""#10b981""
                   rhx-label=""CPU sparkline"" />
    72%
</p>
<p>
    Revenue
    <rhx-sparkline rhx-values=""@Model.Sales""
                   rhx-type=""area""
                   rhx-width=""80px"" rhx-height=""1rem""
                   rhx-stroke-color=""#6366f1""
                   rhx-fill-color=""#6366f133""
                   rhx-label=""Revenue sparkline"" />
    $280k
</p>
<p>
    Scores
    <rhx-sparkline rhx-values=""@Model.Scores""
                   rhx-type=""bar""
                   rhx-width=""80px"" rhx-height=""1rem""
                   rhx-fill-color=""#f59e0b""
                   rhx-label=""Score sparkline"" />
    avg 7.5
</p>";

    public void OnGet()
    {
        ViewData["Breadcrumbs"] = new List<BreadcrumbItem>
        {
            new("Home", "/"),
            new("Components", "/Docs/Components/Sparkline"),
            new("Sparkline")
        };

        CpuHistory = [23, 45, 67, 34, 56, 78, 45, 89, 34, 56, 72, 41, 63, 85, 52];
        Sales = [120, 200, 150, 80, 230, 180, 260, 210, 190, 300, 250, 280];
        Scores = [8, 6, 9, 7, 5, 8, 10, 6, 7, 9];
        Temperature = [18, 19, 21, 24, 26, 28, 30, 29, 27, 23, 20, 18];
        StockPrice = [142.5, 145.2, 143.8, 148.1, 147.3, 150.6, 149.2, 152.4, 155.1, 153.8];
        NegativeValues = [-10, -5, 0, 5, 10, 15, 10, 5, 0, -5];
    }
}
