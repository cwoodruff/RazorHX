using Microsoft.AspNetCore.Mvc.RazorPages;
using RazorHX.Components.Navigation;
using RazorHX.Demo.Models;

namespace RazorHX.Demo.Pages.Docs.Components;

public class TooltipModel : PageModel
{
    public List<ComponentProperty> Properties { get; } = new()
    {
        new("rhx-content", "string", "-", "The tooltip text"),
        new("rhx-placement", "string", "top", "Position: top, bottom, left, right"),
        new("rhx-trigger", "string", "hover", "Trigger mode: hover, click"),
        new("rhx-disabled", "bool", "false", "Whether the tooltip is disabled"),
    };

    public string BasicCode => @"<rhx-tooltip rhx-content=""Save your changes"">
    <rhx-button rhx-variant=""brand"">Save</rhx-button>
</rhx-tooltip>

<rhx-tooltip rhx-content=""Delete this item permanently"" rhx-placement=""bottom"">
    <rhx-button rhx-variant=""danger"">Delete</rhx-button>
</rhx-tooltip>

<rhx-tooltip rhx-content=""Edit settings"" rhx-placement=""right"">
    <rhx-button rhx-variant=""neutral"" rhx-appearance=""outlined"">Settings</rhx-button>
</rhx-tooltip>";

    public string PlacementsCode => @"<rhx-tooltip rhx-content=""Top tooltip"" rhx-placement=""top"">
    <rhx-button rhx-variant=""neutral"" rhx-appearance=""outlined"">Top</rhx-button>
</rhx-tooltip>

<rhx-tooltip rhx-content=""Bottom tooltip"" rhx-placement=""bottom"">
    <rhx-button rhx-variant=""neutral"" rhx-appearance=""outlined"">Bottom</rhx-button>
</rhx-tooltip>

<rhx-tooltip rhx-content=""Left tooltip"" rhx-placement=""left"">
    <rhx-button rhx-variant=""neutral"" rhx-appearance=""outlined"">Left</rhx-button>
</rhx-tooltip>

<rhx-tooltip rhx-content=""Right tooltip"" rhx-placement=""right"">
    <rhx-button rhx-variant=""neutral"" rhx-appearance=""outlined"">Right</rhx-button>
</rhx-tooltip>";

    public string ClickTriggerCode => @"<rhx-tooltip rhx-content=""Click to toggle this tooltip"" rhx-trigger=""click"">
    <rhx-button rhx-variant=""brand"" rhx-appearance=""outlined"">Click Me</rhx-button>
</rhx-tooltip>";

    public string DisabledCode => @"<rhx-tooltip rhx-content=""This tooltip is disabled"" rhx-disabled=""true"">
    <rhx-button rhx-variant=""neutral"">Hover (no tooltip)</rhx-button>
</rhx-tooltip>";

    public void OnGet()
    {
        ViewData["Breadcrumbs"] = new List<BreadcrumbItem>
        {
            new("Home", "/"),
            new("Components", "/Docs/Components/Tooltip"),
            new("Tooltip")
        };
    }
}
