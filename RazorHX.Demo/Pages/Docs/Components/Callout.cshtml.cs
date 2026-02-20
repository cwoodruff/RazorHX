using Microsoft.AspNetCore.Mvc.RazorPages;
using RazorHX.Components.Navigation;
using RazorHX.Demo.Models;

namespace RazorHX.Demo.Pages.Docs.Components;

public class CalloutModel : PageModel
{
    public List<ComponentProperty> Properties { get; } = new()
    {
        new("rhx-variant", "string", "neutral", "Color variant: neutral, brand, success, warning, danger"),
        new("rhx-closable", "bool", "false", "Show a close/dismiss button"),
        new("rhx-duration", "int?", "-", "Auto-dismiss after N milliseconds"),
        new("rhx-icon", "string", "-", "Override the default icon for the variant"),
        new("rhx-open", "bool", "true", "Whether the callout is visible"),
    };

    public string VariantsCode => @"<rhx-callout rhx-variant=""neutral"">
    This is a neutral callout with informational content.
</rhx-callout>
<rhx-callout rhx-variant=""brand"">
    This is a brand callout for highlighting features.
</rhx-callout>
<rhx-callout rhx-variant=""success"">
    Item saved successfully!
</rhx-callout>
<rhx-callout rhx-variant=""warning"">
    Please review your changes before proceeding.
</rhx-callout>
<rhx-callout rhx-variant=""danger"">
    An error occurred while processing your request.
</rhx-callout>";

    public string ClosableCode => @"<rhx-callout rhx-variant=""success"" rhx-closable=""true"">
    This callout can be dismissed by clicking the close button.
</rhx-callout>
<rhx-callout rhx-variant=""warning"" rhx-closable=""true"">
    Warning: this will also close when you click the X.
</rhx-callout>";

    public string AutoDismissCode => @"<rhx-callout rhx-variant=""brand""
             rhx-closable=""true""
             rhx-duration=""5000"">
    This callout will auto-dismiss after 5 seconds.
</rhx-callout>";

    public string CustomIconCode => @"<rhx-callout rhx-variant=""brand""
             rhx-icon=""check-circle"">
    Brand callout with a check circle icon override.
</rhx-callout>";

    public void OnGet()
    {
        ViewData["Breadcrumbs"] = new List<BreadcrumbItem>
        {
            new("Home", "/"),
            new("Components", "/Docs/Components/Callout"),
            new("Callout")
        };
    }
}
