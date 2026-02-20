using Microsoft.AspNetCore.Mvc.RazorPages;
using RazorHX.Components.Navigation;
using RazorHX.Demo.Models;

namespace RazorHX.Demo.Pages.Docs.Components;

public class ScrollerModel : PageModel
{
    public List<ComponentProperty> Properties { get; } = new()
    {
        new("rhx-orientation", "string", "horizontal", "Scroll direction: horizontal, vertical, both"),
    };

    public string HorizontalCode => @"<rhx-scroller rhx-orientation=""horizontal"">
    <div style=""display: flex; gap: var(--rhx-space-md);
                padding: var(--rhx-space-md); white-space: nowrap;"">
        <div class=""scroll-item"">Item 1</div>
        <div class=""scroll-item"">Item 2</div>
        <!-- ... more items ... -->
        <div class=""scroll-item"">Item 20</div>
    </div>
</rhx-scroller>";

    public string VerticalCode => @"<rhx-scroller rhx-orientation=""vertical"" style=""height: 200px;"">
    <div style=""padding: var(--rhx-space-md);"">
        <div>List item 1</div>
        <div>List item 2</div>
        <!-- ... more items ... -->
        <div>List item 30</div>
    </div>
</rhx-scroller>";

    public string BothCode => @"<rhx-scroller rhx-orientation=""both"" style=""height: 250px;"">
    <div style=""display: grid;
                grid-template-columns: repeat(10, 160px);
                gap: var(--rhx-space-sm);
                padding: var(--rhx-space-md);"">
        <!-- 10x10 grid of cells -->
        <div>1,1</div>
        <div>1,2</div>
        <!-- ... -->
    </div>
</rhx-scroller>";

    public string CardGalleryCode => @"<rhx-scroller rhx-orientation=""horizontal"">
    <div style=""display: flex; gap: var(--rhx-space-lg);
                padding: var(--rhx-space-md);"">
        <rhx-card style=""flex: 0 0 280px;"">
            <rhx-card-header><h3>Card 1</h3></rhx-card-header>
            <p>Card content here.</p>
            <rhx-card-footer>
                <rhx-button rhx-variant=""ghost"" rhx-size=""small"">View</rhx-button>
            </rhx-card-footer>
        </rhx-card>
        <!-- ... more cards ... -->
    </div>
</rhx-scroller>";

    public string NoOverflowCode => @"<rhx-scroller rhx-orientation=""horizontal"">
    <div style=""display: flex; gap: var(--rhx-space-md);
                padding: var(--rhx-space-md);"">
        <div class=""scroll-item"">A</div>
        <div class=""scroll-item"">B</div>
        <div class=""scroll-item"">C</div>
    </div>
</rhx-scroller>";

    public void OnGet()
    {
        ViewData["Breadcrumbs"] = new List<BreadcrumbItem>
        {
            new("Home", "/"),
            new("Components", "/Docs/Components/Scroller"),
            new("Scroller")
        };
    }
}
