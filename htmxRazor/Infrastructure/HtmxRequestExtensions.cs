using Microsoft.AspNetCore.Http;

namespace htmxRazor.Infrastructure;

/// <summary>
/// Extension methods for <see cref="HttpRequest"/> to inspect htmx request headers.
/// These headers are set by htmx on requests it initiates and provide context
/// about the triggering element and request type.
/// </summary>
public static class HtmxRequestExtensions
{
    private const string HxRequestHeader = "HX-Request";
    private const string HxBoostedHeader = "HX-Boosted";
    private const string HxTargetHeader = "HX-Target";
    private const string HxTriggerHeader = "HX-Trigger";
    private const string HxTriggerNameHeader = "HX-Trigger-Name";
    private const string HxPromptHeader = "HX-Prompt";
    private const string HxCurrentUrlHeader = "HX-Current-URL";
    private const string HxHistoryRestoreHeader = "HX-History-Restore-Request";

    /// <summary>
    /// Returns true if this is an htmx-initiated request.
    /// Checks for the presence of the HX-Request header.
    /// </summary>
    /// <param name="request">The HTTP request.</param>
    /// <returns>True if the request was made by htmx.</returns>
    public static bool IsHtmxRequest(this HttpRequest request)
        => request.Headers.ContainsKey(HxRequestHeader);

    /// <summary>
    /// Returns true if this request was initiated via hx-boost.
    /// Boosted requests look like regular navigation but are handled by htmx.
    /// </summary>
    /// <param name="request">The HTTP request.</param>
    /// <returns>True if this is a boosted request.</returns>
    public static bool IsHtmxBoosted(this HttpRequest request)
        => string.Equals(request.Headers[HxBoostedHeader], "true", StringComparison.OrdinalIgnoreCase);

    /// <summary>
    /// Returns true if this is an htmx history restoration request.
    /// These occur when the user navigates back/forward and htmx restores from cache.
    /// </summary>
    /// <param name="request">The HTTP request.</param>
    /// <returns>True if this is a history restore request.</returns>
    public static bool IsHtmxHistoryRestore(this HttpRequest request)
        => string.Equals(request.Headers[HxHistoryRestoreHeader], "true", StringComparison.OrdinalIgnoreCase);

    /// <summary>
    /// Gets the CSS id of the target element for the htmx request.
    /// Returns the value of the HX-Target header.
    /// </summary>
    /// <param name="request">The HTTP request.</param>
    /// <returns>The target element id, or null if not set.</returns>
    public static string? GetHxTarget(this HttpRequest request)
        => GetHeaderValue(request, HxTargetHeader);

    /// <summary>
    /// Gets the id of the element that triggered the htmx request.
    /// Returns the value of the HX-Trigger header.
    /// </summary>
    /// <param name="request">The HTTP request.</param>
    /// <returns>The trigger element id, or null if not set.</returns>
    public static string? GetHxTriggerId(this HttpRequest request)
        => GetHeaderValue(request, HxTriggerHeader);

    /// <summary>
    /// Gets the name of the element that triggered the htmx request.
    /// Returns the value of the HX-Trigger-Name header.
    /// </summary>
    /// <param name="request">The HTTP request.</param>
    /// <returns>The trigger element name, or null if not set.</returns>
    public static string? GetHxTriggerName(this HttpRequest request)
        => GetHeaderValue(request, HxTriggerNameHeader);

    /// <summary>
    /// Gets the user's response to an hx-prompt dialog.
    /// Returns the value of the HX-Prompt header.
    /// </summary>
    /// <param name="request">The HTTP request.</param>
    /// <returns>The user's prompt response, or null if not set.</returns>
    public static string? GetHxPrompt(this HttpRequest request)
        => GetHeaderValue(request, HxPromptHeader);

    /// <summary>
    /// Gets the URL of the page that initiated the htmx request.
    /// Returns the value of the HX-Current-URL header.
    /// </summary>
    /// <param name="request">The HTTP request.</param>
    /// <returns>The current page URL, or null if not set.</returns>
    public static string? GetCurrentUrl(this HttpRequest request)
        => GetHeaderValue(request, HxCurrentUrlHeader);

    private static string? GetHeaderValue(HttpRequest request, string header)
    {
        return request.Headers.TryGetValue(header, out var values)
            ? values.ToString()
            : null;
    }
}
