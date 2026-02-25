using Microsoft.AspNetCore.Mvc.RazorPages;
using htmxRazor.Components.Navigation;
using htmxRazor.Demo.Models;

namespace htmxRazor.Demo.Pages.Docs.Components;

public class BreadcrumbModel : PageModel
{
    public List<BreadcrumbItem> BreadcrumbItems { get; set; } = new()
    {
        new("Home", "/"),
        new("Components", "/Docs/Components/Breadcrumb"),
        new("Breadcrumb")
    };

    public List<ComponentProperty> Properties { get; } = new()
    {
        new("rhx-items", "List&lt;BreadcrumbItem&gt;", "-", "Server-side items to render"),
        new("rhx-separator", "string", "/", "Character used between breadcrumb items"),
        new("rhx-label", "string", "Breadcrumb", "Accessible label for the nav element"),
    };

    public string ModelBoundCode => @"<rhx-breadcrumb rhx-items=""@Model.BreadcrumbItems"" />";

    public string ChildItemsCode => @"<rhx-breadcrumb>
    <rhx-breadcrumb-item rhx-href=""/"">Home</rhx-breadcrumb-item>
    <rhx-breadcrumb-item rhx-href=""/Docs/Components/Breadcrumb"">Navigation</rhx-breadcrumb-item>
    <rhx-breadcrumb-item>Breadcrumb</rhx-breadcrumb-item>
</rhx-breadcrumb>";

    public string CustomSeparatorCode => @"<rhx-breadcrumb rhx-separator=""›"" rhx-label=""Site navigation""
                rhx-items=""@Model.BreadcrumbItems"" />";

    public void OnGet()
    {
        ViewData["Breadcrumbs"] = new List<BreadcrumbItem>
        {
            new("Home", "/"),
            new("Components", "/Docs/Components/Breadcrumb"),
            new("Breadcrumb")
        };
    }
}
