using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace htmxRazor.Demo.Pages;

public class PatternsModel : PageModel
{
    public List<string> Items { get; set; } = new();

    public void OnGet()
    {
        Items = Enumerable.Range(1, 10).Select(i => $"Item {i}").ToList();
    }

    public IActionResult OnGetLoadMore(int pageNumber = 1)
    {
        var start = (pageNumber - 1) * 10 + 1;
        var items = Enumerable.Range(start, 10).Select(i => $"Item {i}").ToList();

        var html = string.Join("", items.Select(i =>
            $"<div class=\"rhx-card\" style=\"padding: var(--rhx-space-md); margin-bottom: var(--rhx-space-xs);\">" +
            $"{WebUtility.HtmlEncode(i)}</div>"));

        if (pageNumber < 5)
        {
            var next = pageNumber + 1;
            html += $"<div class=\"rhx-infinite-scroll\" " +
                    $"hx-get=\"/Patterns?handler=LoadMore&pageNumber={next}\" " +
                    $"hx-trigger=\"revealed\" hx-target=\"#item-list\" hx-swap=\"beforeend\" " +
                    $"hx-on::after-request=\"this.remove()\">" +
                    $"<span class=\"rhx-spinner\" role=\"status\" data-rhx-spinner=\"\" aria-label=\"Loading\">" +
                    $"<svg viewBox=\"0 0 24 24\" fill=\"none\">" +
                    $"<circle cx=\"12\" cy=\"12\" r=\"10\" stroke=\"currentColor\" stroke-width=\"3\" />" +
                    $"</svg></span></div>";
        }

        return Content(html, "text/html");
    }

    public IActionResult OnGetSearch(string q = "")
    {
        var allItems = Enumerable.Range(1, 50).Select(i => $"Item {i}").ToList();
        var filtered = string.IsNullOrWhiteSpace(q)
            ? allItems.Take(10)
            : allItems.Where(i => i.Contains(q, StringComparison.OrdinalIgnoreCase));

        var html = string.Join("", filtered.Select(i =>
            $"<div style=\"padding: var(--rhx-space-sm); border-bottom: 1px solid var(--rhx-color-border);\">" +
            $"{WebUtility.HtmlEncode(i)}</div>"));

        if (!filtered.Any())
            html = "<div style=\"padding: var(--rhx-space-md); color: var(--rhx-color-text-muted);\">No results found.</div>";

        return Content(html, "text/html");
    }

    public IActionResult OnGetClock()
    {
        var time = DateTime.Now.ToString("HH:mm:ss");
        var html = $"<div class=\"rhx-poll\" " +
                   $"hx-get=\"/Patterns?handler=Clock\" hx-trigger=\"every 2s\" " +
                   $"hx-target=\"this\" hx-swap=\"outerHTML\" " +
                   $"style=\"padding: var(--rhx-space-md); font-size: var(--rhx-font-size-lg);\">" +
                   $"<strong>Server Time:</strong> {WebUtility.HtmlEncode(time)}</div>";
        return Content(html, "text/html");
    }

    public IActionResult OnGetChart()
    {
        var html = "<div class=\"rhx-card\" style=\"padding: var(--rhx-space-lg);\">" +
                   "<strong>Chart Loaded!</strong><br/>" +
                   "This content was lazily loaded from the server on page load." +
                   "</div>";
        return Content(html, "text/html");
    }

    public IActionResult OnGetDeferredContent()
    {
        var html = "<div class=\"rhx-card\" style=\"padding: var(--rhx-space-lg);\">" +
                   "<strong>Revealed Content!</strong><br/>" +
                   "This content loaded when you scrolled it into view." +
                   "</div>";
        return Content(html, "text/html");
    }
}
