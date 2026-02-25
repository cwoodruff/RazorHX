using System.Text.Json;
using Microsoft.AspNetCore.Http;

namespace htmxRazor.Infrastructure;

/// <summary>
/// Extension methods for <see cref="HttpResponse"/> to set htmx response headers.
/// These headers control how htmx processes the response on the client side.
/// </summary>
public static class HtmxResponseExtensions
{
    /// <summary>
    /// Instructs htmx to perform a client-side redirect to the given URL.
    /// Sets the HX-Redirect response header.
    /// </summary>
    /// <param name="response">The HTTP response.</param>
    /// <param name="url">The URL to redirect to.</param>
    public static void HxRedirect(this HttpResponse response, string url)
        => response.Headers["HX-Redirect"] = url;

    /// <summary>
    /// Instructs htmx to perform a full page refresh on the client.
    /// Sets the HX-Refresh response header to "true".
    /// </summary>
    /// <param name="response">The HTTP response.</param>
    public static void HxRefresh(this HttpResponse response)
        => response.Headers["HX-Refresh"] = "true";

    /// <summary>
    /// Changes the target element for the response content swap.
    /// Overrides the hx-target set on the triggering element.
    /// Sets the HX-Retarget response header.
    /// </summary>
    /// <param name="response">The HTTP response.</param>
    /// <param name="target">CSS selector of the new target element.</param>
    public static void HxRetarget(this HttpResponse response, string target)
        => response.Headers["HX-Retarget"] = target;

    /// <summary>
    /// Changes the swap strategy for the response content.
    /// Overrides the hx-swap set on the triggering element.
    /// Sets the HX-Reswap response header.
    /// </summary>
    /// <param name="response">The HTTP response.</param>
    /// <param name="swap">The swap strategy (e.g., "innerHTML", "outerHTML", "beforeend").</param>
    public static void HxReswap(this HttpResponse response, string swap)
        => response.Headers["HX-Reswap"] = swap;

    /// <summary>
    /// Instructs htmx to push the given URL into the browser history.
    /// Sets the HX-Push-Url response header.
    /// </summary>
    /// <param name="response">The HTTP response.</param>
    /// <param name="url">The URL to push into the browser history.</param>
    public static void HxPushUrl(this HttpResponse response, string url)
        => response.Headers["HX-Push-Url"] = url;

    /// <summary>
    /// Instructs htmx to replace the current URL in the browser history.
    /// Sets the HX-Replace-Url response header.
    /// </summary>
    /// <param name="response">The HTTP response.</param>
    /// <param name="url">The URL to replace in the browser history.</param>
    public static void HxReplaceUrl(this HttpResponse response, string url)
        => response.Headers["HX-Replace-Url"] = url;

    /// <summary>
    /// Triggers a client-side event after the response is received.
    /// Sets the HX-Trigger response header.
    /// </summary>
    /// <param name="response">The HTTP response.</param>
    /// <param name="eventName">The event name to trigger.</param>
    /// <param name="detail">Optional JSON-serializable detail object to include with the event.</param>
    public static void HxTrigger(this HttpResponse response, string eventName, object? detail = null)
    {
        if (detail is null)
        {
            response.Headers["HX-Trigger"] = eventName;
        }
        else
        {
            var json = JsonSerializer.Serialize(new Dictionary<string, object> { [eventName] = detail });
            response.Headers["HX-Trigger"] = json;
        }
    }

    /// <summary>
    /// Triggers a client-side event after the htmx settling step completes.
    /// Sets the HX-Trigger-After-Settle response header.
    /// </summary>
    /// <param name="response">The HTTP response.</param>
    /// <param name="eventName">The event name to trigger.</param>
    /// <param name="detail">Optional JSON-serializable detail object to include with the event.</param>
    public static void HxTriggerAfterSettle(this HttpResponse response, string eventName, object? detail = null)
    {
        if (detail is null)
        {
            response.Headers["HX-Trigger-After-Settle"] = eventName;
        }
        else
        {
            var json = JsonSerializer.Serialize(new Dictionary<string, object> { [eventName] = detail });
            response.Headers["HX-Trigger-After-Settle"] = json;
        }
    }

    /// <summary>
    /// Triggers a client-side event after the htmx swap step completes.
    /// Sets the HX-Trigger-After-Swap response header.
    /// </summary>
    /// <param name="response">The HTTP response.</param>
    /// <param name="eventName">The event name to trigger.</param>
    /// <param name="detail">Optional JSON-serializable detail object to include with the event.</param>
    public static void HxTriggerAfterSwap(this HttpResponse response, string eventName, object? detail = null)
    {
        if (detail is null)
        {
            response.Headers["HX-Trigger-After-Swap"] = eventName;
        }
        else
        {
            var json = JsonSerializer.Serialize(new Dictionary<string, object> { [eventName] = detail });
            response.Headers["HX-Trigger-After-Swap"] = json;
        }
    }

    /// <summary>
    /// Changes the URL in the browser's location bar without adding a history entry.
    /// Sets the HX-Location response header.
    /// </summary>
    /// <param name="response">The HTTP response.</param>
    /// <param name="url">The URL to set in the location bar.</param>
    public static void HxLocation(this HttpResponse response, string url)
        => response.Headers["HX-Location"] = url;
}
