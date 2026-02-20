using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RazorHX.Components.Navigation;
using RazorHX.Infrastructure;

namespace RazorHX.Demo.Pages.Docs.Components;

public class OverlaysModel : PageModel
{
    public void OnGet()
    {
        ViewData["Breadcrumbs"] = new List<BreadcrumbItem>
        {
            new("Home", "/"),
            new("Components", "/Docs/Components/Overlays"),
            new("Drawer")
        };
    }

    /// <summary>
    /// Returns dialog body content and triggers the dialog to open via HX-Trigger-After-Settle.
    /// </summary>
    public IActionResult OnGetDialogContent()
    {
        var html = """
            <p>This content was loaded dynamically via htmx.</p>
            <p>The dialog was opened automatically using the <code>HX-Trigger-After-Settle</code> header.</p>
            """;

        return this.HtmxDialogHtml("#htmx-dialog", html);
    }

    /// <summary>
    /// Returns lazy-loaded content for a details element.
    /// </summary>
    public IActionResult OnGetDetailsContent()
    {
        var html = """
            <table style="width: 100%; border-collapse: collapse;">
                <thead>
                    <tr style="border-bottom: 2px solid var(--rhx-color-border);">
                        <th style="text-align: left; padding: var(--rhx-space-sm);">Order</th>
                        <th style="text-align: left; padding: var(--rhx-space-sm);">Date</th>
                        <th style="text-align: right; padding: var(--rhx-space-sm);">Total</th>
                    </tr>
                </thead>
                <tbody>
                    <tr style="border-bottom: 1px solid var(--rhx-color-border-muted);">
                        <td style="padding: var(--rhx-space-sm);">#1042</td>
                        <td style="padding: var(--rhx-space-sm);">Feb 10, 2026</td>
                        <td style="text-align: right; padding: var(--rhx-space-sm);">$89.99</td>
                    </tr>
                    <tr style="border-bottom: 1px solid var(--rhx-color-border-muted);">
                        <td style="padding: var(--rhx-space-sm);">#1038</td>
                        <td style="padding: var(--rhx-space-sm);">Jan 25, 2026</td>
                        <td style="text-align: right; padding: var(--rhx-space-sm);">$124.50</td>
                    </tr>
                    <tr>
                        <td style="padding: var(--rhx-space-sm);">#1031</td>
                        <td style="padding: var(--rhx-space-sm);">Jan 12, 2026</td>
                        <td style="text-align: right; padding: var(--rhx-space-sm);">$45.00</td>
                    </tr>
                </tbody>
            </table>
            """;

        return new ContentResult
        {
            Content = html,
            ContentType = "text/html",
            StatusCode = 200
        };
    }
}
