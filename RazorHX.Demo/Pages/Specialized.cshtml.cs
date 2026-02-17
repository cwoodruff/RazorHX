using Microsoft.AspNetCore.Mvc.RazorPages;

namespace RazorHX.Demo.Pages;

public class SpecializedModel : PageModel
{
    public int Volume { get; set; } = 50;

    public double Brightness { get; set; } = 75;

    public int ProductRating { get; set; } = 3;

    public double DetailedRating { get; set; } = 3.5;

    public string BrandColor { get; set; } = "#3b82f6";

    public string ThemeColor { get; set; } = "#8b5cf6";

    public void OnGet()
    {
    }
}
