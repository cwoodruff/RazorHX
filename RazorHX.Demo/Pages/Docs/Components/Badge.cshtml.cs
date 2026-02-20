using Microsoft.AspNetCore.Mvc.RazorPages;
using RazorHX.Components.Navigation;
using RazorHX.Demo.Models;

namespace RazorHX.Demo.Pages.Docs.Components;

public class BadgeModel : PageModel
{
    public List<ComponentProperty> Properties { get; } = new()
    {
        new("rhx-variant", "string", "neutral", "Color variant: neutral, brand, success, warning, danger"),
        new("rhx-pill", "bool", "false", "Renders with fully rounded corners"),
        new("rhx-pulse", "bool", "false", "Adds a pulsing animation"),
    };

    public string VariantsCode => @"<rhx-badge rhx-variant=""neutral"">Neutral</rhx-badge>
<rhx-badge rhx-variant=""brand"">Brand</rhx-badge>
<rhx-badge rhx-variant=""success"">Success</rhx-badge>
<rhx-badge rhx-variant=""warning"">Warning</rhx-badge>
<rhx-badge rhx-variant=""danger"">Danger</rhx-badge>";

    public string PillCode => @"<rhx-badge rhx-variant=""neutral"" rhx-pill=""true"">12</rhx-badge>
<rhx-badge rhx-variant=""brand"" rhx-pill=""true"">New</rhx-badge>
<rhx-badge rhx-variant=""success"" rhx-pill=""true"">Active</rhx-badge>
<rhx-badge rhx-variant=""danger"" rhx-pill=""true"">3</rhx-badge>";

    public string PulseCode => @"<rhx-badge rhx-variant=""danger"" rhx-pulse=""true"">3</rhx-badge>
<rhx-badge rhx-variant=""brand"" rhx-pulse=""true"">Live</rhx-badge>
<rhx-badge rhx-variant=""success"" rhx-pill=""true""
           rhx-pulse=""true"">Online</rhx-badge>";

    public void OnGet()
    {
        ViewData["Breadcrumbs"] = new List<BreadcrumbItem>
        {
            new("Home", "/"),
            new("Components", "/Docs/Components/Badge"),
            new("Badge")
        };
    }
}
