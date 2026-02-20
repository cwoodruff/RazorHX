using Microsoft.AspNetCore.Mvc.RazorPages;
using RazorHX.Components.Navigation;
using RazorHX.Demo.Models;

namespace RazorHX.Demo.Pages.Docs.Components;

public class SpinnerModel : PageModel
{
    public List<ComponentProperty> Properties { get; } = new()
    {
        new("rhx-size", "string", "medium", "Spinner size: small, medium, large, or any CSS value"),
        new("rhx-label", "string", "Loading", "Accessible label for the spinner"),
    };

    public string SizesCode => @"<rhx-spinner rhx-size=""small"" />
<rhx-spinner />
<rhx-spinner rhx-size=""large"" />
<rhx-spinner rhx-size=""3rem"" rhx-label=""Custom size"" />";

    public string HtmxIndicatorCode => @"<rhx-spinner id=""my-indicator"" class=""htmx-indicator"" />

<!-- Use with hx-indicator on a trigger element -->
<rhx-button hx-get=""/api/data""
            hx-indicator=""#my-indicator"">
    Load Data
</rhx-button>";

    public void OnGet()
    {
        ViewData["Breadcrumbs"] = new List<BreadcrumbItem>
        {
            new("Home", "/"),
            new("Components", "/Docs/Components/Spinner"),
            new("Spinner")
        };
    }
}
