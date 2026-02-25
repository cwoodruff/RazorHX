using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace htmxRazor.Infrastructure;

/// <summary>
/// Extension methods for <see cref="PageModel"/> to return callout flash messages
/// as htmx partial responses.
/// </summary>
public static class HtmxCalloutExtensions
{
    /// <summary>
    /// Returns a <see cref="ContentResult"/> containing an <c>&lt;rhx-callout&gt;</c> HTML fragment
    /// suitable for htmx swap responses. The callout renders with the specified variant and
    /// optional auto-dismiss duration.
    /// </summary>
    /// <param name="page">The Razor Page model.</param>
    /// <param name="message">The callout message text.</param>
    /// <param name="variant">The color variant: neutral, brand, success, warning, danger. Default: neutral.</param>
    /// <param name="duration">Auto-close duration in milliseconds. 0 = no auto-close. Default: 0.</param>
    /// <param name="closable">Whether to show a close button. Default: true.</param>
    /// <returns>A <see cref="ContentResult"/> with the callout HTML.</returns>
    public static ContentResult HtmxCallout(
        this PageModel page,
        string message,
        string variant = "neutral",
        int duration = 0,
        bool closable = true)
    {
        var durationAttr = duration > 0 ? $" rhx-duration=\"{duration}\"" : "";
        var closableAttr = closable ? " rhx-closable=\"true\"" : "";

        var html = $"<rhx-callout rhx-variant=\"{System.Net.WebUtility.HtmlEncode(variant)}\"{closableAttr}{durationAttr}>" +
                   $"{System.Net.WebUtility.HtmlEncode(message)}" +
                   "</rhx-callout>";

        return new ContentResult
        {
            Content = html,
            ContentType = "text/html",
            StatusCode = 200
        };
    }
}
