using Microsoft.AspNetCore.Mvc.RazorPages;
using RazorHX.Components.Navigation;
using RazorHX.Demo.Models;

namespace RazorHX.Demo.Pages.Docs.Components;

public class ProgressBarModel : PageModel
{
    public List<ComponentProperty> Properties { get; } = new()
    {
        new("rhx-value", "int", "0", "Current progress value (0-100)"),
        new("rhx-label", "string", "-", "Accessible label for the progress bar"),
        new("rhx-indeterminate", "bool", "false", "Shows an indeterminate animation"),
    };

    public string DeterminateCode => @"<rhx-progress-bar rhx-value=""0"" rhx-label=""Not started"" />
<rhx-progress-bar rhx-value=""25"" rhx-label=""Quarter complete"" />
<rhx-progress-bar rhx-value=""65"" rhx-label=""Upload progress"" />
<rhx-progress-bar rhx-value=""100"" rhx-label=""Complete"" />";

    public string IndeterminateCode => @"<rhx-progress-bar rhx-indeterminate=""true"" rhx-label=""Loading"" />";

    public void OnGet()
    {
        ViewData["Breadcrumbs"] = new List<BreadcrumbItem>
        {
            new("Home", "/"),
            new("Components", "/Docs/Components/ProgressBar"),
            new("Progress Bar")
        };
    }
}
