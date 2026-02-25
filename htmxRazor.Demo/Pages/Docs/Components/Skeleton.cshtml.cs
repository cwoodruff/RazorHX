using Microsoft.AspNetCore.Mvc.RazorPages;
using htmxRazor.Components.Navigation;
using htmxRazor.Demo.Models;

namespace htmxRazor.Demo.Pages.Docs.Components;

public class SkeletonModel : PageModel
{
    public List<ComponentProperty> Properties { get; } = new()
    {
        new("rhx-effect", "string", "pulse", "Animation effect: none, pulse, sheen"),
        new("rhx-shape", "string", "rectangle", "Shape: rectangle, rounded, circle"),
        new("rhx-width", "string", "100%", "Width CSS value"),
        new("rhx-height", "string", "1rem", "Height CSS value"),
    };

    public string EffectsCode => @"<rhx-skeleton rhx-effect=""none"" rhx-width=""200px"" rhx-height=""24px"" />
<rhx-skeleton rhx-effect=""pulse"" rhx-width=""200px"" rhx-height=""24px"" />
<rhx-skeleton rhx-effect=""sheen"" rhx-width=""200px"" rhx-height=""24px"" />";

    public string ShapesCode => @"<rhx-skeleton rhx-shape=""rectangle"" rhx-width=""120px"" rhx-height=""24px"" />
<rhx-skeleton rhx-shape=""rounded"" rhx-width=""120px"" rhx-height=""24px"" />
<rhx-skeleton rhx-shape=""circle"" rhx-width=""48px"" rhx-height=""48px"" />";

    public string PlaceholderCode => @"<div style=""display: flex; gap: var(--rhx-space-md); align-items: center;"">
    <rhx-skeleton rhx-shape=""circle"" rhx-width=""48px"" rhx-height=""48px"" />
    <div style=""flex: 1; display: flex; flex-direction: column; gap: var(--rhx-space-xs);"">
        <rhx-skeleton rhx-width=""60%"" rhx-height=""16px"" />
        <rhx-skeleton rhx-width=""40%"" rhx-height=""12px"" />
    </div>
</div>
<rhx-skeleton rhx-width=""100%"" rhx-height=""80px"" />
<rhx-skeleton rhx-width=""75%"" rhx-height=""16px"" />";

    public void OnGet()
    {
        ViewData["Breadcrumbs"] = new List<BreadcrumbItem>
        {
            new("Home", "/"),
            new("Components", "/Docs/Components/Skeleton"),
            new("Skeleton")
        };
    }
}
