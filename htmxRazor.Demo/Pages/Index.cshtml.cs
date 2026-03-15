using Microsoft.AspNetCore.Mvc.RazorPages;

namespace htmxRazor.Demo.Pages;

public class IndexModel : PageModel
{
    public List<CategoryInfo> Categories { get; } = new()
    {
        new("Actions", "cursor", 2, "/Docs/Components/Button"),
        new("Forms", "edit", 12, "/Docs/Components/Input"),
        new("Feedback", "bell", 9, "/Docs/Components/Callout"),
        new("Navigation", "arrow-right", 5, "/Docs/Components/Tabs"),
        new("Organization", "grid", 4, "/Docs/Components/Card"),
        new("Overlays", "layers", 4, "/Docs/Components/Dialog"),
        new("Imagery", "image", 5, "/Docs/Components/Icon"),
        new("Data Display", "table", 2, "/Docs/Components/DataTable"),
        new("Formatting", "clock", 4, "/Docs/Components/FormatBytes"),
        new("Utilities", "settings", 6, "/Docs/Components/CopyButton"),
        new("Patterns", "refresh", 4, "/Patterns"),
    };

    public void OnGet()
    {
    }

    public record CategoryInfo(string Name, string Icon, int Count, string Href);
}
