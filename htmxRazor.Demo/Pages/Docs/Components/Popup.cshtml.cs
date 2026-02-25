using Microsoft.AspNetCore.Mvc.RazorPages;
using htmxRazor.Components.Navigation;
using htmxRazor.Demo.Models;

namespace htmxRazor.Demo.Pages.Docs.Components;

public class PopupModel : PageModel
{
    public List<ComponentProperty> Properties { get; } = new()
    {
        new("rhx-anchor", "string", "-", "CSS selector of the anchor element"),
        new("rhx-placement", "string", "bottom", "Position: top, bottom, left, right, bottom-start, bottom-end, top-start, top-end, left-start, left-end, right-start, right-end"),
        new("rhx-arrow", "bool", "false", "Show a directional arrow element"),
    };

    public string BasicCode => @"<div style=""position: relative;"">
    <button class=""rhx-button rhx-button--neutral"" id=""popup-anchor""
            onclick=""
                var p = document.getElementById('demo-popup');
                p.hidden = !p.hidden;
                p.classList.toggle('rhx-popup--active');
            "">Toggle Popup</button>
    <rhx-popup rhx-anchor=""#popup-anchor""
               rhx-placement=""bottom-start""
               id=""demo-popup"">
        <div style=""padding: var(--rhx-space-md);
                    background: var(--rhx-color-surface);
                    border: var(--rhx-border-width) solid var(--rhx-color-border);
                    border-radius: var(--rhx-radius-md);
                    box-shadow: var(--rhx-shadow-lg);
                    min-width: 12rem;"">
            <p>Positioned popup content.</p>
        </div>
    </rhx-popup>
</div>";

    public string WithArrowCode => @"<div style=""position: relative;"">
    <button class=""rhx-button rhx-button--brand"" id=""popup-arrow-anchor""
            onclick=""
                var p = document.getElementById('demo-popup-arrow');
                p.hidden = !p.hidden;
                p.classList.toggle('rhx-popup--active');
            "">Toggle Popup with Arrow</button>
    <rhx-popup rhx-anchor=""#popup-arrow-anchor""
               rhx-placement=""bottom""
               rhx-arrow=""true""
               id=""demo-popup-arrow"">
        <div style=""padding: var(--rhx-space-md);
                    background: var(--rhx-color-surface);
                    border: var(--rhx-border-width) solid var(--rhx-color-border);
                    border-radius: var(--rhx-radius-md);
                    box-shadow: var(--rhx-shadow-lg);
                    min-width: 12rem;"">
            <p>This popup has a directional arrow.</p>
        </div>
    </rhx-popup>
</div>";

    public void OnGet()
    {
        ViewData["Breadcrumbs"] = new List<BreadcrumbItem>
        {
            new("Home", "/"),
            new("Components", "/Docs/Components/Popup"),
            new("Popup")
        };
    }
}
