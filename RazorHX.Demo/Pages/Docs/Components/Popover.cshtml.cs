using Microsoft.AspNetCore.Mvc.RazorPages;
using RazorHX.Components.Navigation;
using RazorHX.Demo.Models;

namespace RazorHX.Demo.Pages.Docs.Components;

public class PopoverModel : PageModel
{
    public List<ComponentProperty> Properties { get; } = new()
    {
        new("rhx-trigger", "string", "-", "CSS selector of the trigger element"),
        new("rhx-trigger-event", "string", "click", "Trigger event: click, hover"),
        new("rhx-placement", "string", "bottom", "Position: top, bottom, left, right"),
        new("rhx-arrow", "bool", "true", "Show a directional arrow"),
    };

    public string ClickTriggerCode => @"<button class=""rhx-button rhx-button--brand"" id=""pop-click-trigger"">
    Show Popover
</button>
<rhx-popover rhx-trigger=""#pop-click-trigger"" rhx-placement=""bottom"">
    <h4>Popover Title</h4>
    <p>This is a rich content popover. It supports
       <strong>HTML</strong>, images, and interactive elements.</p>
</rhx-popover>";

    public string HoverTriggerCode => @"<button class=""rhx-button rhx-button--neutral"" id=""pop-hover-trigger"">
    Hover Me
</button>
<rhx-popover rhx-trigger=""#pop-hover-trigger""
             rhx-trigger-event=""hover""
             rhx-placement=""top"">
    <p>This popover appears on hover.</p>
</rhx-popover>";

    public string PlacementsCode => @"<button id=""pop-top"">Top</button>
<rhx-popover rhx-trigger=""#pop-top"" rhx-placement=""top"">
    <p>Top placement</p>
</rhx-popover>

<button id=""pop-bottom"">Bottom</button>
<rhx-popover rhx-trigger=""#pop-bottom"" rhx-placement=""bottom"">
    <p>Bottom placement</p>
</rhx-popover>

<button id=""pop-left"">Left</button>
<rhx-popover rhx-trigger=""#pop-left"" rhx-placement=""left"">
    <p>Left placement</p>
</rhx-popover>

<button id=""pop-right"">Right</button>
<rhx-popover rhx-trigger=""#pop-right"" rhx-placement=""right"">
    <p>Right placement</p>
</rhx-popover>";

    public string NoArrowCode => @"<button class=""rhx-button rhx-button--brand"" id=""pop-noarrow"">
    No Arrow
</button>
<rhx-popover rhx-trigger=""#pop-noarrow""
             rhx-placement=""bottom""
             rhx-arrow=""false"">
    <p>This popover has no directional arrow.</p>
</rhx-popover>";

    public void OnGet()
    {
        ViewData["Breadcrumbs"] = new List<BreadcrumbItem>
        {
            new("Home", "/"),
            new("Components", "/Docs/Components/Popover"),
            new("Popover")
        };
    }
}
