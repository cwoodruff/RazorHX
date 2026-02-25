using Microsoft.AspNetCore.Mvc.RazorPages;
using htmxRazor.Components.Navigation;
using htmxRazor.Demo.Models;

namespace htmxRazor.Demo.Pages.Docs.Components;

public class AnimatedImageModel : PageModel
{
    public List<ComponentProperty> Properties { get; } = new()
    {
        new("rhx-src", "string", "-", "URL of the animated image or video"),
        new("rhx-alt", "string", "-", "Alt text for the image"),
        new("rhx-play", "bool", "true", "Whether the animation starts playing"),
    };

    public string PlayPauseCode => @"<rhx-animated-image
    rhx-src=""https://media.giphy.com/media/xT9IgzoKnwFNmISR8I/giphy.gif""
    rhx-alt=""Animated loading"" />";

    public string InitiallyPausedCode => @"<rhx-animated-image
    rhx-src=""https://media.giphy.com/media/xT9IgzoKnwFNmISR8I/giphy.gif""
    rhx-alt=""Paused animation""
    rhx-play=""false"" />";

    public void OnGet()
    {
        ViewData["Breadcrumbs"] = new List<BreadcrumbItem>
        {
            new("Home", "/"),
            new("Components", "/Docs/Components/AnimatedImage"),
            new("Animated Image")
        };
    }
}
