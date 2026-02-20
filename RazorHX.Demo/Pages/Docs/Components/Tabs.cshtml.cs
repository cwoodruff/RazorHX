using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RazorHX.Components.Navigation;

namespace RazorHX.Demo.Pages.Docs.Components;

public class TabsModel : PageModel
{
    public void OnGet()
    {
        ViewData["Breadcrumbs"] = new List<BreadcrumbItem>
        {
            new("Home", "/"),
            new("Components", "/Docs/Components/Tabs"),
            new("Tabs")
        };
    }

    public ContentResult OnGetLazyContent(string tab)
    {
        var html = tab switch
        {
            "one" => "<p><strong>Lazy Tab 1</strong> — This content was loaded via htmx when the tab was first clicked.</p>",
            "two" => "<p><strong>Lazy Tab 2</strong> — Another lazy-loaded panel. The spinner was replaced with this content.</p>",
            _ => "<p>Unknown tab content.</p>"
        };

        return Content(html, "text/html");
    }
}
