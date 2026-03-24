using Microsoft.AspNetCore.Mvc.RazorPages;
using htmxRazor.Components.Navigation;
using htmxRazor.Demo.Models;

namespace htmxRazor.Demo.Pages.Docs.Components;

public class TimelineModel : PageModel
{
    public List<ComponentProperty> Properties { get; } =
    [
        new("rhx-layout", "string", "vertical", "Timeline direction: vertical, horizontal"),
        new("rhx-align", "string", "start", "Item alignment: start, center, alternate"),
    ];

    public List<ComponentProperty> ItemProperties { get; } =
    [
        new("rhx-variant", "string", "neutral", "Connector dot color: neutral, brand, success, warning, danger"),
        new("rhx-label", "string", "-", "Label text (e.g., date or time) shown above the body"),
        new("rhx-active", "bool", "false", "Highlights this item as the current step"),
    ];

    public string BasicCode => @"<rhx-timeline>
    <rhx-timeline-item rhx-variant=""success"" rhx-label=""March 10, 2026"">
        Order placed
    </rhx-timeline-item>
    <rhx-timeline-item rhx-variant=""success"" rhx-label=""March 12, 2026"">
        Payment confirmed
    </rhx-timeline-item>
    <rhx-timeline-item rhx-variant=""brand"" rhx-label=""March 14, 2026"" rhx-active=""true"">
        Shipped
    </rhx-timeline-item>
    <rhx-timeline-item rhx-label=""Pending"">
        Delivered
    </rhx-timeline-item>
</rhx-timeline>";

    public string VariantsCode => @"<rhx-timeline>
    <rhx-timeline-item rhx-variant=""success"">Completed</rhx-timeline-item>
    <rhx-timeline-item rhx-variant=""brand"">In Progress</rhx-timeline-item>
    <rhx-timeline-item rhx-variant=""warning"">Review Needed</rhx-timeline-item>
    <rhx-timeline-item rhx-variant=""danger"">Failed</rhx-timeline-item>
</rhx-timeline>";

    public string HorizontalCode => @"<rhx-timeline rhx-layout=""horizontal"">
    <rhx-timeline-item rhx-variant=""success"" rhx-label=""Step 1"">Done</rhx-timeline-item>
    <rhx-timeline-item rhx-variant=""brand"" rhx-label=""Step 2"" rhx-active=""true"">Active</rhx-timeline-item>
    <rhx-timeline-item rhx-label=""Step 3"">Pending</rhx-timeline-item>
</rhx-timeline>";

    public string AlternateCode => @"<rhx-timeline rhx-align=""alternate"">
    <rhx-timeline-item rhx-variant=""brand"" rhx-label=""9:00 AM"">Morning standup</rhx-timeline-item>
    <rhx-timeline-item rhx-variant=""success"" rhx-label=""11:30 AM"">Code review</rhx-timeline-item>
    <rhx-timeline-item rhx-variant=""warning"" rhx-label=""2:00 PM"">Design review</rhx-timeline-item>
</rhx-timeline>";

    public void OnGet()
    {
        ViewData["Breadcrumbs"] = new List<BreadcrumbItem>
        {
            new("Home", "/"),
            new("Components", "/Docs/Components/Timeline"),
            new("Timeline")
        };
    }
}
