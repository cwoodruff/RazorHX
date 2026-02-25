using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace htmxRazor.Infrastructure;

/// <summary>
/// Extension methods for <see cref="PageModel"/> that implement the common
/// full-page-vs-partial rendering pattern used with htmx.
/// When the request is an htmx request, returns only the partial view content.
/// When it's a normal navigation request, returns the full page.
/// </summary>
public static class PartialResultHelper
{
    /// <summary>
    /// Returns a partial view for htmx requests, or the full page for normal requests.
    /// This is the standard pattern for progressive enhancement with htmx:
    /// htmx gets a fragment, browsers get the full page.
    /// </summary>
    /// <param name="page">The page model to extend.</param>
    /// <param name="partialName">The name of the partial view to render for htmx requests.</param>
    /// <param name="model">Optional model to pass to the partial view.</param>
    /// <returns>
    /// A <see cref="PartialViewResult"/> if the request is from htmx,
    /// or a <see cref="PageResult"/> for normal browser navigation.
    /// </returns>
    /// <example>
    /// <code>
    /// // In a Razor Page handler:
    /// public IActionResult OnGetItems()
    /// {
    ///     var items = _service.GetItems();
    ///     return this.HtmxResult("_ItemList", items);
    /// }
    /// </code>
    /// </example>
    public static IActionResult HtmxResult(this PageModel page, string partialName, object? model = null)
    {
        if (page.Request.IsHtmxRequest())
        {
            return page.Partial(partialName, model);
        }

        return page.Page();
    }

    /// <summary>
    /// Returns a partial view for htmx requests, or the full page for normal requests,
    /// with the option to use the page's own model for the partial.
    /// </summary>
    /// <param name="page">The page model to extend.</param>
    /// <param name="partialName">The name of the partial view to render for htmx requests.</param>
    /// <returns>
    /// A <see cref="PartialViewResult"/> if the request is from htmx,
    /// or a <see cref="PageResult"/> for normal browser navigation.
    /// </returns>
    public static IActionResult HtmxPartialOrPage(this PageModel page, string partialName)
    {
        if (page.Request.IsHtmxRequest())
        {
            return page.Partial(partialName);
        }

        return page.Page();
    }

    /// <summary>
    /// Returns a partial view for htmx requests, or redirects to a URL for normal requests.
    /// Useful for form submission handlers that need to redirect on full page loads
    /// but return a fragment for htmx.
    /// </summary>
    /// <param name="page">The page model to extend.</param>
    /// <param name="partialName">The name of the partial view to render for htmx requests.</param>
    /// <param name="model">Optional model to pass to the partial view.</param>
    /// <param name="redirectUrl">The URL to redirect to for non-htmx requests.</param>
    /// <returns>
    /// A <see cref="PartialViewResult"/> if the request is from htmx,
    /// or a <see cref="RedirectResult"/> for normal browser navigation.
    /// </returns>
    public static IActionResult HtmxResultOrRedirect(
        this PageModel page,
        string partialName,
        object? model,
        string redirectUrl)
    {
        if (page.Request.IsHtmxRequest())
        {
            return page.Partial(partialName, model);
        }

        return new RedirectResult(redirectUrl);
    }
}
