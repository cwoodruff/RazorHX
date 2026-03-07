using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using htmxRazor.Components.Navigation;
using htmxRazor.Demo.Models;

namespace htmxRazor.Demo.Pages.Docs.Components;

public class LiveRegionModel : PageModel
{
    public List<ComponentProperty> Properties { get; } = new()
    {
        new("rhx-politeness", "string", "polite", "Politeness level: polite, assertive, off"),
        new("rhx-atomic", "bool", "true", "Whether the entire region is announced as a whole"),
        new("rhx-relevant", "string", "-", "Which changes to announce: additions, removals, text, all"),
        new("rhx-visually-hidden", "bool", "false", "Visually hidden but accessible to screen readers"),
        new("rhx-role", "string", "status", "ARIA role: status, alert, log"),
    };

    public string BasicCode => @"<rhx-live-region id=""search-results"">
    <!-- htmx-swapped content is announced to screen readers -->
</rhx-live-region>

<rhx-input rhx-label=""Search""
           hx-get=""/search""
           hx-trigger=""input changed delay:300ms""
           hx-target=""#search-results"" />";

    public string AssertiveCode => @"<!-- Use assertive for urgent messages that interrupt -->
<rhx-live-region rhx-politeness=""assertive"" rhx-role=""alert"" id=""error-output"">
    <!-- Error messages appear here -->
</rhx-live-region>";

    public string VisuallyHiddenCode => @"<!-- Screen reader only — no visual output -->
<rhx-live-region rhx-visually-hidden=""true"" id=""sr-status"">
    3 results found
</rhx-live-region>";

    public string HtmxCode => @"<!-- Status updated via htmx polling -->
<rhx-live-region id=""order-status""
                  rhx-politeness=""polite""
                  hx-get=""/api/order/status""
                  hx-trigger=""every 10s""
                  hx-swap=""innerHTML"">
    Checking order status...
</rhx-live-region>";

    private static readonly string[] StatusMessages =
    {
        "Searching...",
        "Found 3 results for your query.",
        "Found 12 results for your query.",
        "No results found.",
        "Found 1 result for your query.",
    };

    public void OnGet()
    {
        ViewData["Breadcrumbs"] = new List<BreadcrumbItem>
        {
            new("Home", "/"),
            new("Components", "/Docs/Components/LiveRegion"),
            new("Live Region")
        };
    }

    public IActionResult OnGetSearch(string? q)
    {
        if (string.IsNullOrWhiteSpace(q))
            return Content("<p style=\"color:var(--rhx-color-text-muted);\">Type to search...</p>", "text/html");

        var count = q.Length % 5;
        var message = count == 0
            ? "No results found."
            : $"Found {count} result{(count == 1 ? "" : "s")} for \"{System.Net.WebUtility.HtmlEncode(q)}\".";

        var html = $"<p>{message}</p>";
        for (var i = 1; i <= count; i++)
        {
            html += $"<p style=\"padding:var(--rhx-space-xs) 0; border-bottom:1px solid var(--rhx-color-neutral-200);\">Result {i} for \"{System.Net.WebUtility.HtmlEncode(q)}\"</p>";
        }

        return Content(html, "text/html");
    }
}
