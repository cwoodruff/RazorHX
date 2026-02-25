using Microsoft.AspNetCore.Mvc.RazorPages;
using htmxRazor.Components.Navigation;

namespace htmxRazor.Demo.Pages.Docs;

public class GettingStartedModel : PageModel
{
    public void OnGet()
    {
        ViewData["Breadcrumbs"] = new List<BreadcrumbItem>
        {
            new("Home", "/"),
            new("Getting Started")
        };
    }
}
