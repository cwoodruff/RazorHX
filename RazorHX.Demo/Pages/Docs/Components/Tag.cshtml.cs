using Microsoft.AspNetCore.Mvc.RazorPages;
using RazorHX.Components.Navigation;
using RazorHX.Demo.Models;

namespace RazorHX.Demo.Pages.Docs.Components;

public class TagComponentModel : PageModel
{
    public List<ComponentProperty> Properties { get; } = new()
    {
        new("rhx-variant", "string", "neutral", "Color variant: neutral, brand, success, warning, danger"),
        new("rhx-size", "string", "medium", "Tag size: small, medium, large"),
        new("rhx-pill", "bool", "false", "Renders with fully rounded corners"),
        new("rhx-removable", "bool", "false", "Shows a remove button"),
    };

    public string VariantsCode => @"<rhx-tag rhx-variant=""neutral"">Neutral</rhx-tag>
<rhx-tag rhx-variant=""brand"">Brand</rhx-tag>
<rhx-tag rhx-variant=""success"">Success</rhx-tag>
<rhx-tag rhx-variant=""warning"">Warning</rhx-tag>
<rhx-tag rhx-variant=""danger"">Danger</rhx-tag>";

    public string SizesCode => @"<rhx-tag rhx-variant=""brand"" rhx-size=""small"">Small</rhx-tag>
<rhx-tag rhx-variant=""brand"">Medium (default)</rhx-tag>
<rhx-tag rhx-variant=""brand"" rhx-size=""large"">Large</rhx-tag>";

    public string PillCode => @"<rhx-tag rhx-variant=""brand"" rhx-pill=""true"">C#</rhx-tag>
<rhx-tag rhx-variant=""success"" rhx-pill=""true"">TypeScript</rhx-tag>
<rhx-tag rhx-variant=""danger"" rhx-pill=""true"">Rust</rhx-tag>";

    public string RemovableCode => @"<rhx-tag rhx-variant=""brand"" rhx-removable=""true"">JavaScript</rhx-tag>
<rhx-tag rhx-variant=""success"" rhx-removable=""true"">Python</rhx-tag>
<rhx-tag rhx-variant=""danger"" rhx-removable=""true""
         rhx-pill=""true"">Deprecated</rhx-tag>";

    public string HtmxDeleteCode => @"<rhx-tag rhx-variant=""brand"" rhx-removable=""true""
         hx-delete=""/api/tags/1""
         hx-trigger=""click from:find .rhx-tag__remove""
         hx-target=""closest .rhx-tag""
         hx-swap=""outerHTML swap:200ms"">
    htmx Tag
</rhx-tag>";

    public void OnGet()
    {
        ViewData["Breadcrumbs"] = new List<BreadcrumbItem>
        {
            new("Home", "/"),
            new("Components", "/Docs/Components/Tag"),
            new("Tag")
        };
    }
}
