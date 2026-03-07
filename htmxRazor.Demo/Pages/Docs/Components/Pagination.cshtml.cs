using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using htmxRazor.Components.Navigation;
using htmxRazor.Demo.Models;

namespace htmxRazor.Demo.Pages.Docs.Components;

/// <summary>
/// View model passed to the _PaginationDemo partial.
/// </summary>
public class PaginationDemoModel
{
    public int CurrentPage { get; set; } = 1;
    public int TotalItems { get; set; } = 195;
    public int PageSize { get; set; } = 10;
    public bool ShowInfo { get; set; }
    public bool ShowEdges { get; set; } = true;
    public string? Size { get; set; }
    public string ContainerId { get; set; } = "paging-demo";
    public List<string> Items { get; set; } = new();

    /// <summary>
    /// Full hx-get URL with all variant config encoded as query params.
    /// The pagination component appends &amp;p=N to this URL for each page button.
    /// </summary>
    public string HxGetUrl
    {
        get
        {
            var url = $"/Docs/Components/Pagination?handler=Items&containerId={Uri.EscapeDataString(ContainerId)}";
            if (ShowInfo) url += "&showInfo=true";
            if (!ShowEdges) url += "&showEdges=false";
            if (!string.IsNullOrEmpty(Size)) url += $"&size={Uri.EscapeDataString(Size)}";
            return url;
        }
    }
}

public class PaginationModel : PageModel
{
    public PaginationDemoModel LiveDemo { get; set; } = new() { ContainerId = "paging-live" };
    public PaginationDemoModel BasicDemo { get; set; } = new() { ContainerId = "paging-basic", CurrentPage = 5 };
    public PaginationDemoModel TotalItemsDemo { get; set; } = new() { ContainerId = "paging-totalitems", CurrentPage = 3 };
    public PaginationDemoModel InfoDemo { get; set; } = new() { ContainerId = "paging-info", CurrentPage = 7, ShowInfo = true };
    public PaginationDemoModel SmallDemo { get; set; } = new() { ContainerId = "paging-small", CurrentPage = 3, TotalItems = 100, Size = "small" };
    public PaginationDemoModel MediumDemo { get; set; } = new() { ContainerId = "paging-medium", CurrentPage = 3, TotalItems = 100 };
    public PaginationDemoModel LargeDemo { get; set; } = new() { ContainerId = "paging-large", CurrentPage = 3, TotalItems = 100, Size = "large" };
    public PaginationDemoModel NoEdgesDemo { get; set; } = new() { ContainerId = "paging-noedges", CurrentPage = 5, ShowEdges = false };

    public List<ComponentProperty> Properties { get; } = new()
    {
        new("rhx-current-page", "int", "1", "The active page number (1-based)"),
        new("rhx-total-pages", "int", "0", "Total number of pages"),
        new("rhx-total-items", "int?", "-", "Total items (auto-calculates total pages with page-size)"),
        new("rhx-page-size", "int", "10", "Items per page (used with total-items)"),
        new("rhx-max-visible", "int", "7", "Maximum page buttons shown (including first/last)"),
        new("rhx-page-param", "string", "p", "Query parameter name for the page number"),
        new("rhx-show-info", "bool", "false", "Show \"Page X of Y\" text"),
        new("rhx-show-edges", "bool", "true", "Show First/Last navigation buttons"),
        new("rhx-size", "string", "-", "Size variant: small, medium, large"),
        new("rhx-label", "string", "Pagination", "Accessible label for the nav element"),
    };

    public string BasicCode => @"<rhx-pagination rhx-current-page=""@Model.CurrentPage""
                 rhx-total-pages=""20""
                 hx-get=""/items""
                 hx-target=""#results""
                 hx-swap=""innerHTML"" />";

    public string TotalItemsCode => @"<rhx-pagination rhx-current-page=""@Model.CurrentPage""
                 rhx-total-items=""195""
                 rhx-page-size=""10""
                 hx-get=""/items""
                 hx-target=""#results""
                 hx-swap=""innerHTML"" />";

    public string InfoCode => @"<rhx-pagination rhx-current-page=""@Model.CurrentPage""
                 rhx-total-pages=""20""
                 rhx-show-info=""true""
                 hx-get=""/items""
                 hx-target=""#results""
                 hx-swap=""innerHTML"" />";

    public string SizesCode => @"<rhx-pagination rhx-current-page=""3"" rhx-total-pages=""10"" rhx-size=""small"" />
<rhx-pagination rhx-current-page=""3"" rhx-total-pages=""10"" />
<rhx-pagination rhx-current-page=""3"" rhx-total-pages=""10"" rhx-size=""large"" />";

    public string NoEdgesCode => @"<rhx-pagination rhx-current-page=""5""
                 rhx-total-pages=""20""
                 rhx-show-edges=""false"" />";

    public string HtmxCode => @"<!-- Container holds items + pagination; handler returns both -->
<div id=""paging-demo"">
    @await Html.PartialAsync(""_PaginationDemo"", Model.DemoModel)
</div>

<!-- The partial (_PaginationDemo.cshtml): -->
<ul>
    @foreach (var item in Model.Items)
    {
        <li>@item</li>
    }
</ul>
<rhx-pagination rhx-current-page=""@Model.CurrentPage""
                 rhx-total-items=""195""
                 rhx-page-size=""10""
                 hx-get=""/items?handler=Items""
                 hx-target=""#paging-demo""
                 hx-swap=""innerHTML"" />";

    public void OnGet()
    {
        ViewData["Breadcrumbs"] = new List<BreadcrumbItem>
        {
            new("Home", "/"),
            new("Components", "/Docs/Components/Pagination"),
            new("Pagination")
        };

        // Populate items for each demo
        PopulateDemoItems(LiveDemo);
        PopulateDemoItems(BasicDemo);
        PopulateDemoItems(TotalItemsDemo);
        PopulateDemoItems(InfoDemo);
        PopulateDemoItems(SmallDemo);
        PopulateDemoItems(MediumDemo);
        PopulateDemoItems(LargeDemo);
        PopulateDemoItems(NoEdgesDemo);
    }

    public IActionResult OnGetItems(
        int p = 1,
        string containerId = "paging-live",
        bool showInfo = false,
        bool showEdges = true,
        string size = "")
    {
        var model = new PaginationDemoModel
        {
            ContainerId = containerId,
            ShowInfo = showInfo,
            ShowEdges = showEdges,
            Size = string.IsNullOrEmpty(size) ? null : size
        };
        PopulateDemoItems(model, p);
        return Partial("Docs/Components/_PaginationDemo", model);
    }

    private static void PopulateDemoItems(PaginationDemoModel model, int? page = null)
    {
        var totalPages = (int)Math.Ceiling((double)model.TotalItems / model.PageSize);
        model.CurrentPage = Math.Clamp(page ?? model.CurrentPage, 1, totalPages);
        var start = (model.CurrentPage - 1) * model.PageSize + 1;
        var end = Math.Min(start + model.PageSize - 1, model.TotalItems);
        for (var i = start; i <= end; i++)
        {
            model.Items.Add($"Item {i}");
        }
    }
}
