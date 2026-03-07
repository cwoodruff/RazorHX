using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace htmxRazor.Infrastructure;

/// <summary>
/// Extension methods for triggering toast notifications from server-side handlers.
/// Supports both event-based (JS creates the toast) and OOB-swap (server renders the toast) patterns.
/// </summary>
public static class HtmxToastExtensions
{
    /// <summary>
    /// Triggers a toast notification on the client by setting the HX-Trigger-After-Settle header.
    /// The toast container's JavaScript listens for the <c>rhx:toast</c> event and creates the toast dynamically.
    /// </summary>
    /// <param name="response">The HTTP response.</param>
    /// <param name="message">The toast message text.</param>
    /// <param name="variant">The color variant: neutral, brand, success, warning, danger. Default: success.</param>
    /// <param name="duration">Auto-dismiss duration in milliseconds. Null uses container default. Default: null.</param>
    public static void HxToast(this HttpResponse response, string message, string variant = "success", int? duration = null)
    {
        var detail = duration.HasValue
            ? (object)new { message, variant, duration = duration.Value }
            : new { message, variant };

        response.HxTriggerAfterSettle("rhx:toast", detail);
    }

    /// <summary>
    /// Returns a <see cref="ContentResult"/> containing an <c>&lt;rhx-toast&gt;</c> HTML fragment
    /// with <c>hx-swap-oob="beforeend:#rhx-toasts"</c> for out-of-band insertion into the toast container.
    /// Use this when you need custom toast content or want server-rendered toast HTML.
    /// </summary>
    /// <param name="page">The Razor Page model.</param>
    /// <param name="message">The toast message text.</param>
    /// <param name="variant">The color variant: neutral, brand, success, warning, danger. Default: success.</param>
    /// <param name="duration">Auto-dismiss duration in milliseconds. 0 = no auto-dismiss. Default: 0 (uses container default).</param>
    /// <param name="closable">Whether to show a close button. Default: true.</param>
    /// <param name="containerId">The id of the toast container. Default: rhx-toasts.</param>
    /// <returns>A <see cref="ContentResult"/> with the toast HTML fragment.</returns>
    public static ContentResult HxToastOob(
        this PageModel page,
        string message,
        string variant = "success",
        int duration = 0,
        bool closable = true,
        string containerId = "rhx-toasts")
    {
        var durationAttr = duration > 0 ? $" rhx-duration=\"{duration}\"" : "";
        var closableAttr = closable ? " rhx-closable=\"true\"" : "";

        var html = $"<rhx-toast rhx-variant=\"{System.Net.WebUtility.HtmlEncode(variant)}\"{closableAttr}{durationAttr} " +
                   $"hx-swap-oob=\"beforeend:#{System.Net.WebUtility.HtmlEncode(containerId)}\">" +
                   $"{System.Net.WebUtility.HtmlEncode(message)}" +
                   "</rhx-toast>";

        return new ContentResult
        {
            Content = html,
            ContentType = "text/html",
            StatusCode = 200
        };
    }
}
