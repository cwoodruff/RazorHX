using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using htmxRazor.Components.Navigation;
using htmxRazor.Demo.Models;

namespace htmxRazor.Demo.Pages.Docs.Components;

public class LoadMoreModel : PageModel
{
    public List<ComponentProperty> Properties { get; } =
    [
        new("page", "string", "-", "Razor Page path for hx-get URL generation"),
        new("page-handler", "string", "-", "Page handler name (e.g., \"LoadMore\")"),
        new("route-*", "string", "-", "Route parameter values for URL generation"),
        new("target", "string", "-", "CSS selector of the target for appended content"),
        new("swap", "string", "beforeend", "How the response is swapped relative to the target"),
        new("rhx-variant", "string", "neutral", "Button variant: neutral, brand, success, warning, danger"),
        new("rhx-loading-text", "string", "Loading…", "Screen-reader text on the loading indicator"),
        new("rhx-disabled", "bool", "false", "Whether the load more button is disabled"),
    ];

    public List<string> Items { get; set; } = [];
    public int CurrentPage { get; set; } = 1;

    public string BasicCode => @"<div id=""item-list"">
    @foreach (var item in Model.Items)
    {
        <div class=""rhx-card"" style=""padding: var(--rhx-space-md); margin-bottom: var(--rhx-space-xs);"">@item</div>
    }
</div>
<rhx-load-more page=""/Docs/Components/LoadMore"" page-handler=""LoadMore""
               route-page=""2"" target=""#item-list"">
    Load more items
</rhx-load-more>";

    public string VariantCode => @"<rhx-load-more page=""/Docs/Components/LoadMore"" page-handler=""LoadMore""
               route-page=""2"" target=""#item-list2"" rhx-variant=""brand"">
    Show more
</rhx-load-more>";

    public void OnGet()
    {
        ViewData["Breadcrumbs"] = new List<BreadcrumbItem>
        {
            new("Home", "/"),
            new("Patterns", "/Patterns"),
            new("Load More")
        };

        Items = Enumerable.Range(1, 5).Select(i => $"Item {i}").ToList();
    }

    public IActionResult OnGetLoadMore(int page = 1, string target = "#item-list", string variant = "neutral", string label = "Load more items")
    {
        return Partial("_LoadMoreItems", (Page: page, PageSize: 5, MaxPages: 4, Target: target, Variant: variant, Label: label));
    }
}
