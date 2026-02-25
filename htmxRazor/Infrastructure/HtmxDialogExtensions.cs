using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace htmxRazor.Infrastructure;

/// <summary>
/// Extension methods for <see cref="PageModel"/> to coordinate htmx responses
/// with htmxRazor dialog components. After swapping content into a dialog's body,
/// the HX-Trigger-After-Settle header fires an event that JavaScript uses to
/// open the dialog via <c>showModal()</c>.
/// </summary>
public static class HtmxDialogExtensions
{
    /// <summary>
    /// Returns a partial result and sets the <c>HX-Trigger-After-Settle</c> header
    /// to open the specified dialog after the htmx swap completes.
    /// </summary>
    /// <param name="page">The Razor Page model.</param>
    /// <param name="dialogSelector">CSS selector of the dialog element (e.g., "#edit-dialog").</param>
    /// <param name="partial">The partial view result containing the dialog body content.</param>
    /// <returns>The partial view result with the dialog-open trigger header set.</returns>
    public static PartialViewResult HtmxDialog(this PageModel page, string dialogSelector, PartialViewResult partial)
    {
        page.Response.Headers["HX-Trigger-After-Settle"] =
            System.Text.Json.JsonSerializer.Serialize(new Dictionary<string, object>
            {
                ["rhx:dialog:open"] = new { target = dialogSelector }
            });
        return partial;
    }

    /// <summary>
    /// Returns an HTML content result and sets the <c>HX-Trigger-After-Settle</c> header
    /// to open the specified dialog after the htmx swap completes.
    /// </summary>
    /// <param name="page">The Razor Page model.</param>
    /// <param name="dialogSelector">CSS selector of the dialog element (e.g., "#edit-dialog").</param>
    /// <param name="html">The HTML string content for the dialog body.</param>
    /// <returns>A <see cref="ContentResult"/> with the dialog-open trigger header set.</returns>
    public static ContentResult HtmxDialogHtml(this PageModel page, string dialogSelector, string html)
    {
        page.Response.Headers["HX-Trigger-After-Settle"] =
            System.Text.Json.JsonSerializer.Serialize(new Dictionary<string, object>
            {
                ["rhx:dialog:open"] = new { target = dialogSelector }
            });

        return new ContentResult
        {
            Content = html,
            ContentType = "text/html",
            StatusCode = 200
        };
    }
}
