using Microsoft.AspNetCore.Mvc.RazorPages;
using RazorHX.Components.Navigation;
using RazorHX.Demo.Models;

namespace RazorHX.Demo.Pages.Docs.Components;

public class ProgressRingModel : PageModel
{
    public List<ComponentProperty> Properties { get; } = new()
    {
        new("rhx-value", "int", "0", "Current progress value (0-100)"),
        new("rhx-label", "string", "-", "Accessible label for the progress ring"),
        new("rhx-track-width", "int", "3", "Width of the background track stroke"),
        new("rhx-indicator-width", "int", "3", "Width of the progress indicator stroke"),
    };

    public string ValuesCode => @"<rhx-progress-ring rhx-value=""0"" rhx-label=""Not started"" />
<rhx-progress-ring rhx-value=""25"" rhx-label=""Quarter"" />
<rhx-progress-ring rhx-value=""65"" rhx-label=""Upload"" />
<rhx-progress-ring rhx-value=""100"" rhx-label=""Complete"" />";

    public string StrokesCode => @"<rhx-progress-ring rhx-value=""75"" rhx-track-width=""2"" rhx-indicator-width=""2"" />
<rhx-progress-ring rhx-value=""75"" rhx-track-width=""4"" rhx-indicator-width=""4"" />
<rhx-progress-ring rhx-value=""75"" rhx-track-width=""3"" rhx-indicator-width=""5"" />";

    public void OnGet()
    {
        ViewData["Breadcrumbs"] = new List<BreadcrumbItem>
        {
            new("Home", "/"),
            new("Components", "/Docs/Components/ProgressRing"),
            new("Progress Ring")
        };
    }
}
