using Microsoft.AspNetCore.Mvc.RazorPages;
using RazorHX.Components.Navigation;

namespace RazorHX.Demo.Pages.Docs.Components;

public class SparklineModel : PageModel
{
    public double[] CpuHistory { get; set; } = [];
    public double[] Sales { get; set; } = [];
    public double[] Scores { get; set; } = [];
    public double[] Temperature { get; set; } = [];
    public double[] StockPrice { get; set; } = [];
    public double[] NegativeValues { get; set; } = [];

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
