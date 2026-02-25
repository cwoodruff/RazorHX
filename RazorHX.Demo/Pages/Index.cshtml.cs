using Microsoft.AspNetCore.Mvc.RazorPages;

namespace RazorHX.Demo.Pages;

public class IndexModel : PageModel
{
    public List<CategoryInfo> Categories { get; } = new()
    {
        new("Actions", "cursor", 2, "/Docs/Components/Button"),
        new("Forms", "edit", 12, "/Docs/Components/Input"),
        new("Feedback", "bell", 8, "/Docs/Components/Callout"),
        new("Navigation", "arrow-right", 4, "/Docs/Components/Tab"),
        new("Organization", "grid", 4, "/Docs/Components/Card"),
        new("Overlays", "layers", 3, "/Docs/Components/Dialog"),
        new("Imagery", "image", 5, "/Docs/Components/Icon"),
        new("Formatting", "clock", 4, "/Docs/Components/FormatBytes"),
        new("Utilities", "settings", 5, "/Docs/Components/CopyButton"),
        new("Patterns", "refresh", 4, "/Patterns"),
    };

    public void OnGet()
    {
    }

    public record CategoryInfo(string Name, string Icon, int Count, string Href);
}
