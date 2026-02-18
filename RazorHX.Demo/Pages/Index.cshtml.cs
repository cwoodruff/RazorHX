using Microsoft.AspNetCore.Mvc.RazorPages;

namespace RazorHX.Demo.Pages;

public class IndexModel : PageModel
{
    public List<CategoryInfo> Categories { get; } = new()
    {
        new("Actions", "cursor", 2, "/Docs/Components/Button"),
        new("Forms", "edit", 12, "/Docs/Components/Input"),
        new("Feedback", "bell", 8, "/Feedback"),
        new("Navigation", "arrow-right", 4, "/Tabs"),
        new("Organization", "grid", 4, "/Organization"),
        new("Overlays", "layers", 3, "/Docs/Components/Dialog"),
        new("Imagery", "image", 5, "/Imagery"),
        new("Formatting", "clock", 4, "/Formatting"),
        new("Utilities", "settings", 5, "/Utilities"),
        new("Patterns", "refresh", 4, "/Patterns"),
    };

    public void OnGet()
    {
    }

    public record CategoryInfo(string Name, string Icon, int Count, string Href);
}
