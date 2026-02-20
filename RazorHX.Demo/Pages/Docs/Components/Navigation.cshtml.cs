using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RazorHX.Components.Navigation;

namespace RazorHX.Demo.Pages.Docs.Components;

public class NavigationModel : PageModel
{
    public List<BreadcrumbItem> BreadcrumbItems { get; set; } = new()
    {
        new("Home", "/"),
        new("Components", "/Docs/Components/Navigation"),
        new("Breadcrumb")
    };

    public void OnGet()
    {
        ViewData["Breadcrumbs"] = new List<BreadcrumbItem>
        {
            new("Home", "/"),
            new("Components", "/Docs/Components/Navigation"),
            new("Breadcrumb")
        };
    }

    public ContentResult OnGetTreeChildren(string folder)
    {
        var html = folder switch
        {
            "root" => """
                <div role="treeitem" tabindex="-1" class="rhx-tree__item rhx-tree__item--leaf">
                    <div class="rhx-tree__item-content">
                        <span class="rhx-tree__item-label">Loaded child 1</span>
                    </div>
                </div>
                <div role="treeitem" tabindex="-1" class="rhx-tree__item rhx-tree__item--leaf">
                    <div class="rhx-tree__item-content">
                        <span class="rhx-tree__item-label">Loaded child 2</span>
                    </div>
                </div>
                <div role="treeitem" tabindex="-1" class="rhx-tree__item rhx-tree__item--leaf">
                    <div class="rhx-tree__item-content">
                        <span class="rhx-tree__item-label">Loaded child 3</span>
                    </div>
                </div>
                """,
            _ => "<div role=\"treeitem\" tabindex=\"-1\" class=\"rhx-tree__item rhx-tree__item--leaf\"><div class=\"rhx-tree__item-content\"><span class=\"rhx-tree__item-label\">Unknown folder</span></div></div>"
        };

        return Content(html, "text/html");
    }
}
