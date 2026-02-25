using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace htmxRazor.Infrastructure;

/// <summary>
/// Extension methods for <see cref="PageModel"/> that implement common
/// htmx validation response patterns. These helpers set the correct
/// HTTP status codes and response headers for htmx form submissions.
/// </summary>
public static class HtmxValidationExtensions
{
    /// <summary>
    /// Returns an HTTP 422 (Unprocessable Entity) response with the re-rendered
    /// form partial containing validation errors. This is the standard pattern
    /// for htmx form validation: the server returns 422 + the form HTML with
    /// error states, and htmx swaps it into the page.
    /// </summary>
    /// <param name="page">The page model to extend.</param>
    /// <param name="partialName">The name of the partial view to render.</param>
    /// <param name="model">Optional model. Defaults to the page itself.</param>
    /// <returns>A <see cref="PartialViewResult"/> with a 422 status code.</returns>
    /// <example>
    /// <code>
    /// public IActionResult OnPost()
    /// {
    ///     if (!ModelState.IsValid)
    ///         return this.HtmxValidationFailure("_ContactForm");
    ///     // ...
    /// }
    /// </code>
    /// </example>
    public static IActionResult HtmxValidationFailure(
        this PageModel page, string partialName, object? model = null)
    {
        page.Response.StatusCode = 422;
        return page.Partial(partialName, model ?? page);
    }

    /// <summary>
    /// Returns a success partial view response, optionally triggering a toast
    /// notification on the client via HX-Trigger-After-Settle.
    /// </summary>
    /// <param name="page">The page model to extend.</param>
    /// <param name="partialName">The name of the partial view to render.</param>
    /// <param name="model">Optional model. Defaults to the page itself.</param>
    /// <param name="message">Optional success message to show as a toast.</param>
    /// <returns>A <see cref="PartialViewResult"/>.</returns>
    /// <example>
    /// <code>
    /// public IActionResult OnPost()
    /// {
    ///     // ... process form ...
    ///     return this.HtmxSuccess("_ContactForm", message: "Message sent!");
    /// }
    /// </code>
    /// </example>
    public static IActionResult HtmxSuccess(
        this PageModel page, string partialName, object? model = null,
        string? message = null)
    {
        if (message != null)
            page.Response.HxTriggerAfterSettle("rhx:toast",
                new { message, variant = "success" });
        return page.Partial(partialName, model ?? page);
    }
}
