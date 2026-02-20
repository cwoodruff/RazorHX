using Microsoft.AspNetCore.Mvc.RazorPages;
using RazorHX.Components.Navigation;

namespace RazorHX.Demo.Pages.Docs.Components;

public class UtilitiesModel : PageModel
{
    public void OnGet()
    {
        ViewData["Breadcrumbs"] = new List<BreadcrumbItem>
        {
            new("Home", "/"),
            new("Components", "/Docs/Components/Utilities"),
            new("Copy Button")
        };
    }
}
