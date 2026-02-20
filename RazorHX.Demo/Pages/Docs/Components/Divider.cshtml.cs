using Microsoft.AspNetCore.Mvc.RazorPages;
using RazorHX.Components.Navigation;
using RazorHX.Demo.Models;

namespace RazorHX.Demo.Pages.Docs.Components;

public class DividerModel : PageModel
{
    public List<ComponentProperty> Properties { get; } = new()
    {
        new("rhx-vertical", "bool", "false", "Renders a vertical divider instead of horizontal"),
    };

    public string HorizontalCode => @"<p>Content above the divider.</p>
<rhx-divider />
<p>Content below the divider.</p>";

    public string VerticalCode => @"<div style=""display: flex; align-items: stretch; height: 48px; gap: var(--rhx-space-md);"">
    <span>Left</span>
    <rhx-divider rhx-vertical />
    <span>Right</span>
</div>";

    public void OnGet()
    {
        ViewData["Breadcrumbs"] = new List<BreadcrumbItem>
        {
            new("Home", "/"),
            new("Components", "/Docs/Components/Divider"),
            new("Divider")
        };
    }
}
