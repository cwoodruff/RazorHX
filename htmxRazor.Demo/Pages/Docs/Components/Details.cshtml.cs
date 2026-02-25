using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using htmxRazor.Components.Navigation;
using htmxRazor.Demo.Models;

namespace htmxRazor.Demo.Pages.Docs.Components;

public class DetailsModel : PageModel
{
    public List<ComponentProperty> Properties { get; } = new()
    {
        new("rhx-summary", "string", "-", "The text shown in the clickable summary/header"),
        new("rhx-open", "bool", "false", "Whether the details starts expanded"),
        new("rhx-disabled", "bool", "false", "Prevents toggling"),
    };

    public string BasicCode => @"<rhx-details rhx-summary=""What is htmxRazor?"">
    <p>htmxRazor is an ASP.NET Core component library built with
       Razor Tag Helpers and first-class htmx integration.</p>
</rhx-details>";

    public string OpenCode => @"<rhx-details rhx-summary=""System Requirements"" rhx-open>
    <ul>
        <li>.NET 9.0 or later</li>
        <li>ASP.NET Core Razor Pages or MVC</li>
        <li>Any modern browser</li>
    </ul>
</rhx-details>";

    public string FaqCode => @"<rhx-details rhx-summary=""How do I install htmxRazor?"">
    <p>Add the NuGet package and register the tag helpers
       in _ViewImports.cshtml.</p>
</rhx-details>
<rhx-details rhx-summary=""Does it work with htmx?"">
    <p>Yes! Every component supports hx-get, hx-post,
       hx-trigger, hx-target, and hx-swap attributes.</p>
</rhx-details>
<rhx-details rhx-summary=""Is it accessible?"">
    <p>htmxRazor uses semantic HTML, ARIA attributes,
       and keyboard navigation throughout all components.</p>
</rhx-details>";

    public string DisabledCode => @"<rhx-details rhx-summary=""Locked Section"" rhx-disabled>
    <p>This content cannot be accessed.</p>
</rhx-details>";

    public string HtmxCode => @"<rhx-details rhx-summary=""Load Order History""
             hx-get=""/Docs/Components/Details?handler=DetailsContent""
             hx-trigger=""toggle once""
             hx-target=""find .rhx-details__content"">
    <rhx-spinner rhx-size=""small"" />
</rhx-details>";

    public void OnGet()
    {
        ViewData["Breadcrumbs"] = new List<BreadcrumbItem>
        {
            new("Home", "/"),
            new("Components", "/Docs/Components/Details"),
            new("Details")
        };
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
                        <td style="text-align: right; padding: var(--rhx-space-sm);">$45.00</td>
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
