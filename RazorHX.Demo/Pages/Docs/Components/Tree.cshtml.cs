using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RazorHX.Components.Navigation;
using RazorHX.Demo.Models;

namespace RazorHX.Demo.Pages.Docs.Components;

public class TreeModel : PageModel
{
    public List<ComponentProperty> Properties { get; } = new()
    {
        new("rhx-selection", "string", "single", "Selection mode: single, multiple, leaf"),
        new("aria-label", "string", "-", "Accessible label for the tree"),
    };

    public string BasicCode => @"<rhx-tree aria-label=""File explorer"">
    <rhx-tree-item rhx-label=""Documents"" rhx-expanded>
        <rhx-tree-item>README.md</rhx-tree-item>
        <rhx-tree-item>LICENSE</rhx-tree-item>
        <rhx-tree-item rhx-label=""Reports"">
            <rhx-tree-item>Q1 Report.pdf</rhx-tree-item>
            <rhx-tree-item>Q2 Report.pdf</rhx-tree-item>
        </rhx-tree-item>
    </rhx-tree-item>
    <rhx-tree-item rhx-label=""Images"">
        <rhx-tree-item>logo.png</rhx-tree-item>
        <rhx-tree-item>banner.jpg</rhx-tree-item>
    </rhx-tree-item>
</rhx-tree>";

    public string MultipleCode => @"<rhx-tree rhx-selection=""multiple"" aria-label=""Multi-select tree"">
    <rhx-tree-item>Apple</rhx-tree-item>
    <rhx-tree-item>Banana</rhx-tree-item>
    <rhx-tree-item>Cherry</rhx-tree-item>
    <rhx-tree-item rhx-label=""Citrus"">
        <rhx-tree-item>Orange</rhx-tree-item>
        <rhx-tree-item>Lemon</rhx-tree-item>
        <rhx-tree-item>Grapefruit</rhx-tree-item>
    </rhx-tree-item>
</rhx-tree>";

    public string LeafCode => @"<rhx-tree rhx-selection=""leaf"" aria-label=""Leaf selection tree"">
    <rhx-tree-item rhx-label=""Frontend"" rhx-expanded>
        <rhx-tree-item>React</rhx-tree-item>
        <rhx-tree-item>Vue</rhx-tree-item>
        <rhx-tree-item>Angular</rhx-tree-item>
    </rhx-tree-item>
    <rhx-tree-item rhx-label=""Backend"">
        <rhx-tree-item>ASP.NET Core</rhx-tree-item>
        <rhx-tree-item>Express</rhx-tree-item>
    </rhx-tree-item>
</rhx-tree>";

    public string StatesCode => @"<rhx-tree aria-label=""States example"">
    <rhx-tree-item rhx-label=""Available"" rhx-expanded>
        <rhx-tree-item rhx-selected>Selected item</rhx-tree-item>
        <rhx-tree-item>Normal item</rhx-tree-item>
    </rhx-tree-item>
    <rhx-tree-item rhx-label=""Locked"" rhx-disabled>
        <rhx-tree-item>Cannot interact</rhx-tree-item>
    </rhx-tree-item>
</rhx-tree>";

    public string LazyLoadCode => @"<rhx-tree aria-label=""Lazy-loaded tree"">
    <rhx-tree-item rhx-label=""Lazy Folder""
                   rhx-lazy
                   hx-get=""/Docs/Components/Tree?handler=TreeChildren&folder=root""
                   hx-target=""find .rhx-tree__children""
                   hx-swap=""innerHTML""
                   hx-trigger=""toggle once"">
    </rhx-tree-item>
    <rhx-tree-item>Static leaf item</rhx-tree-item>
</rhx-tree>";

    public void OnGet()
    {
        ViewData["Breadcrumbs"] = new List<BreadcrumbItem>
        {
            new("Home", "/"),
            new("Components", "/Docs/Components/Tree"),
            new("Tree")
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
