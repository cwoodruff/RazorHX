using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace RazorHX.Demo.Pages;

public class CarouselModel : PageModel
{
    public void OnGet()
    {
    }

    public IActionResult OnGetSlideContent()
    {
        var html = """
            <div style="padding: var(--rhx-space-xl); text-align: center;">
                <h3 style="margin-bottom: var(--rhx-space-sm);">Slide 3 — Loaded via htmx!</h3>
                <p style="color: var(--rhx-color-text-muted);">
                    This content was fetched from the server on demand.
                </p>
            </div>
            """;

        return Content(html, "text/html");
    }
}
