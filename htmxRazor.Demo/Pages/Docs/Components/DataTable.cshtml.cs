using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using htmxRazor.Components.Navigation;
using htmxRazor.Demo.Models;
using htmxRazor.Infrastructure;

namespace htmxRazor.Demo.Pages.Docs.Components;

public class DataTableModel : PageModel
{
    public List<ComponentProperty> Properties { get; } =
    [
        new("rhx-striped", "bool", "false", "Alternating row backgrounds"),
        new("rhx-hoverable", "bool", "false", "Row hover highlight"),
        new("rhx-bordered", "bool", "false", "Cell borders"),
        new("rhx-compact", "bool", "false", "Reduced padding"),
        new("rhx-sticky-header", "bool", "false", "Sticky header on scroll"),
        new("rhx-loading", "bool", "false", "Show loading overlay"),
        new("rhx-empty-message", "string", "\"No data\"", "Message when table body is empty"),
        new("rhx-sort-url", "string", "-", "Base URL for sort/filter htmx requests"),
        new("rhx-caption", "string", "-", "Accessible caption text"),
        new("rhx-label", "string", "-", "aria-label for the table"),
    ];

    public List<ComponentProperty> ColumnProperties { get; } =
    [
        new("rhx-field", "string", "-", "Property name for sort/filter query params"),
        new("rhx-header", "string", "-", "Display header text"),
        new("rhx-sortable", "bool", "false", "Enable sorting on this column"),
        new("rhx-sort-direction", "string?", "null", "Current sort: asc, desc, or null"),
        new("rhx-filterable", "bool", "false", "Enable filter input in header"),
        new("rhx-filter-value", "string?", "null", "Current filter value"),
        new("rhx-width", "string?", "null", "CSS width (e.g., 200px, 30%)"),
        new("rhx-align", "string", "start", "Text alignment: start, center, end"),
    ];

    public List<ComponentProperty> PaginationProperties { get; } =
    [
        new("rhx-page", "int", "1", "Current page number (1-based)"),
        new("rhx-page-size", "int", "10", "Rows per page"),
        new("rhx-total-items", "int", "0", "Total row count"),
        new("rhx-url", "string", "-", "Base URL for page requests"),
        new("rhx-target", "string?", "-", "htmx target for page requests"),
    ];

    // ── Sample data ──
    public record Product(int Id, string Name, string Category, decimal Price, int Stock);

    public static readonly List<Product> AllProducts =
    [
        new(1, "Wireless Mouse", "Electronics", 29.99m, 150),
        new(2, "Mechanical Keyboard", "Electronics", 89.99m, 75),
        new(3, "USB-C Hub", "Electronics", 45.00m, 200),
        new(4, "Standing Desk", "Furniture", 499.99m, 30),
        new(5, "Monitor Arm", "Furniture", 119.00m, 60),
        new(6, "Desk Lamp", "Furniture", 35.50m, 120),
        new(7, "Notebook", "Stationery", 4.99m, 500),
        new(8, "Gel Pens (10 pack)", "Stationery", 12.99m, 300),
        new(9, "Webcam HD", "Electronics", 69.99m, 90),
        new(10, "Ergonomic Chair", "Furniture", 349.00m, 25),
        new(11, "Sticky Notes", "Stationery", 3.49m, 800),
        new(12, "Headset", "Electronics", 59.99m, 110),
    ];

    /// <summary>
    /// View model for the reusable table partial.
    /// </summary>
    public class TableState
    {
        public List<Product> Items { get; set; } = [];
        public int TotalItems { get; set; }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 5;
        public string? Sort { get; set; }
        public string? SortDirection { get; set; }
    }

    public List<Product> Items { get; set; } = AllProducts;

    public string BasicCode => @"<rhx-data-table id=""products"" rhx-striped=""true""
                rhx-hoverable=""true"" rhx-caption=""Product inventory"">
    <rhx-column rhx-field=""name"" rhx-header=""Name"" />
    <rhx-column rhx-field=""category"" rhx-header=""Category"" />
    <rhx-column rhx-field=""price"" rhx-header=""Price"" />
    <rhx-column rhx-field=""stock"" rhx-header=""Stock"" />
    @foreach (var p in Model.Items)
    {
        <tr><td>@p.Name</td><td>@p.Category</td>
            <td>@p.Price.ToString(""C"")</td><td>@p.Stock</td></tr>
    }
</rhx-data-table>";

    public string ModifiersCode => @"<!-- Bordered + Compact -->
<rhx-data-table rhx-bordered=""true"" rhx-compact=""true"" ...>
    ...
</rhx-data-table>";

    public string PaginationCode => @"<rhx-data-table id=""pag-table"" rhx-striped=""true"">
    <rhx-column rhx-field=""name"" rhx-header=""Name"" />
    <rhx-column rhx-field=""category"" rhx-header=""Category"" />
    <!-- rows... -->
    <rhx-data-table-pagination rhx-page=""1"" rhx-page-size=""5""
        rhx-total-items=""12"" />
</rhx-data-table>";

    public string HtmxCode => @"<!-- The table is in a partial view: _DataTablePartial.cshtml -->
<div id=""htmx-table-container"">
    @await Html.PartialAsync(""_DataTablePartial"", state)
</div>

<!-- Partial re-renders the full table on each request -->
<!-- Sort buttons and pagination target #htmx-table-container -->
<!-- Server handler uses DataTableRequest model binder: -->
public IActionResult OnGetTableData(DataTableRequest request)
{
    // sort, filter, paginate...
    return Partial(""_DataTablePartial"", state);
}";

    public void OnGet()
    {
        ViewData["Breadcrumbs"] = new List<BreadcrumbItem>
        {
            new("Home", "/"),
            new("Components", "/Docs/Components/DataTable"),
            new("Data Table")
        };
    }

    public IActionResult OnGetTableData(DataTableRequest request)
    {
        var query = AllProducts.AsEnumerable();

        // Sort
        if (!string.IsNullOrEmpty(request.Sort))
        {
            query = (request.Sort.ToLowerInvariant(), request.SortDirection?.ToLowerInvariant()) switch
            {
                ("name", "desc") => query.OrderByDescending(p => p.Name),
                ("name", _) => query.OrderBy(p => p.Name),
                ("price", "desc") => query.OrderByDescending(p => p.Price),
                ("price", _) => query.OrderBy(p => p.Price),
                ("stock", "desc") => query.OrderByDescending(p => p.Stock),
                ("stock", _) => query.OrderBy(p => p.Stock),
                _ => query
            };
        }

        var items = query.ToList();
        var totalItems = items.Count;

        // Paginate
        var pageSize = Math.Clamp(request.PageSize, 1, 50);
        var totalPages = (int)Math.Ceiling((double)totalItems / pageSize);
        var page = Math.Clamp(request.Page, 1, Math.Max(1, totalPages));
        var paged = items.Skip((page - 1) * pageSize).Take(pageSize).ToList();

        var state = new TableState
        {
            Items = paged,
            TotalItems = totalItems,
            Page = page,
            PageSize = pageSize,
            Sort = request.Sort,
            SortDirection = request.SortDirection
        };

        return Partial("_DataTablePartial", state);
    }

    public IActionResult OnGetPaginatedData(DataTableRequest request)
    {
        var pageSize = Math.Clamp(request.PageSize, 1, 50);
        var totalItems = AllProducts.Count;
        var totalPages = (int)Math.Ceiling((double)totalItems / pageSize);
        var page = Math.Clamp(request.Page, 1, Math.Max(1, totalPages));
        var paged = AllProducts.Skip((page - 1) * pageSize).Take(pageSize).ToList();

        var state = new TableState
        {
            Items = paged,
            TotalItems = totalItems,
            Page = page,
            PageSize = pageSize
        };

        return Partial("_DataTablePaginatedPartial", state);
    }
}
