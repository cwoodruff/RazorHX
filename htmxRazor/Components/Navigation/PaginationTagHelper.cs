using System.Net;
using htmxRazor.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace htmxRazor.Components.Navigation;

/// <summary>
/// Renders a pagination navigation control with htmx-powered page navigation.
/// Generates page buttons with <c>hx-get</c> URLs for server-side paging.
/// </summary>
/// <example>
/// <code>
/// &lt;rhx-pagination rhx-current-page="@Model.Page"
///                  rhx-total-pages="@Model.TotalPages"
///                  hx-get="/items" hx-target="#results" hx-swap="innerHTML"
///                  hx-push-url="true" /&gt;
/// </code>
/// </example>
[HtmlTargetElement("rhx-pagination")]
public class PaginationTagHelper : htmxRazorTagHelperBase
{
    /// <inheritdoc/>
    protected override string BlockName => "pagination";

    // ──────────────────────────────────────────────
    //  Pagination properties
    // ──────────────────────────────────────────────

    /// <summary>
    /// The current (active) page number. 1-based. Default: 1.
    /// </summary>
    [HtmlAttributeName("rhx-current-page")]
    public int CurrentPage { get; set; } = 1;

    /// <summary>
    /// The total number of pages. Required unless <see cref="TotalItems"/> and <see cref="PageSize"/> are set.
    /// </summary>
    [HtmlAttributeName("rhx-total-pages")]
    public int TotalPages { get; set; }

    /// <summary>
    /// Total number of items. When set with <see cref="PageSize"/>, <see cref="TotalPages"/> is calculated automatically.
    /// </summary>
    [HtmlAttributeName("rhx-total-items")]
    public int? TotalItems { get; set; }

    /// <summary>
    /// Number of items per page. Used with <see cref="TotalItems"/> to calculate total pages. Default: 10.
    /// </summary>
    [HtmlAttributeName("rhx-page-size")]
    public int PageSize { get; set; } = 10;

    /// <summary>
    /// Maximum number of page buttons to show (including first/last). Default: 7.
    /// </summary>
    [HtmlAttributeName("rhx-max-visible")]
    public int MaxVisible { get; set; } = 7;

    /// <summary>
    /// The query parameter name for the page number. Default: "page".
    /// </summary>
    [HtmlAttributeName("rhx-page-param")]
    public string PageParam { get; set; } = "p";

    /// <summary>
    /// Whether to show "Page X of Y" info text. Default: false.
    /// </summary>
    [HtmlAttributeName("rhx-show-info")]
    public bool ShowInfo { get; set; }

    /// <summary>
    /// Whether to show First/Last navigation buttons. Default: true.
    /// </summary>
    [HtmlAttributeName("rhx-show-edges")]
    public bool ShowEdges { get; set; } = true;

    /// <summary>
    /// The size variant. Options: small, medium, large.
    /// </summary>
    [HtmlAttributeName("rhx-size")]
    public string? Size { get; set; }

    /// <summary>
    /// Accessible label for the pagination navigation. Default: "Pagination".
    /// </summary>
    [HtmlAttributeName("rhx-label")]
    public string Label { get; set; } = "Pagination";

    public PaginationTagHelper(IUrlHelperFactory urlHelperFactory) : base(urlHelperFactory) { }

    // ──────────────────────────────────────────────
    //  Rendering
    // ──────────────────────────────────────────────

    /// <inheritdoc/>
    public override Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
    {
        // Calculate total pages from total items if needed
        if (TotalItems.HasValue && TotalItems.Value > 0 && PageSize > 0)
        {
            TotalPages = (int)Math.Ceiling((double)TotalItems.Value / PageSize);
        }

        if (TotalPages <= 1)
        {
            output.SuppressOutput();
            return Task.CompletedTask;
        }

        // Clamp current page
        CurrentPage = Math.Clamp(CurrentPage, 1, TotalPages);

        output.TagName = "nav";
        output.TagMode = TagMode.StartTagAndEndTag;

        var css = CreateCssBuilder();
        if (!string.IsNullOrWhiteSpace(Size))
            css.Add(GetModifierClass(Size));

        ApplyBaseAttributes(output, css);
        output.Attributes.SetAttribute("aria-label", Label);

        output.Content.Clear();
        output.Content.AppendHtml($"<ul class=\"{GetElementClass("list")}\">");

        // Resolve the base URL for page links
        var baseUrl = ResolveBaseUrl();

        // First button
        if (ShowEdges)
            RenderNavButton(output, baseUrl, 1, "First page", FirstSvg, "first", CurrentPage == 1);

        // Previous button
        RenderNavButton(output, baseUrl, CurrentPage - 1, "Previous page", PrevSvg, "prev", CurrentPage == 1);

        // Page numbers
        var pages = CalculateVisiblePages();
        var lastRendered = 0;
        foreach (var page in pages)
        {
            if (lastRendered > 0 && page > lastRendered + 1)
            {
                // Ellipsis
                output.Content.AppendHtml(
                    $"<li class=\"{GetElementClass("item")} {GetElementClass("item")}--ellipsis\" aria-hidden=\"true\">" +
                    $"<span class=\"{GetElementClass("ellipsis")}\">&hellip;</span></li>");
            }

            if (page == CurrentPage)
            {
                output.Content.AppendHtml(
                    $"<li class=\"{GetElementClass("item")}\">" +
                    $"<button type=\"button\" class=\"{GetElementClass("link")} {GetElementClass("link")}--current\" " +
                    $"aria-current=\"page\" aria-label=\"Page {page}\" disabled>{page}</button></li>");
            }
            else
            {
                output.Content.AppendHtml(
                    $"<li class=\"{GetElementClass("item")}\">" +
                    $"<button type=\"button\" class=\"{GetElementClass("link")}\" " +
                    $"aria-label=\"Go to page {page}\"{BuildHtmxAttrsForPage(baseUrl, page)}>{page}</button></li>");
            }

            lastRendered = page;
        }

        // Next button
        RenderNavButton(output, baseUrl, CurrentPage + 1, "Next page", NextSvg, "next", CurrentPage == TotalPages);

        // Last button
        if (ShowEdges)
            RenderNavButton(output, baseUrl, TotalPages, "Last page", LastSvg, "last", CurrentPage == TotalPages);

        output.Content.AppendHtml("</ul>");

        // Info text
        if (ShowInfo)
        {
            output.Content.AppendHtml(
                $"<span class=\"{GetElementClass("info")}\" aria-live=\"polite\">" +
                $"Page {CurrentPage} of {TotalPages}</span>");
        }

        return Task.CompletedTask;
    }

    // ──────────────────────────────────────────────
    //  Page calculation
    // ──────────────────────────────────────────────

    private List<int> CalculateVisiblePages()
    {
        var pages = new List<int>();

        if (TotalPages <= MaxVisible)
        {
            for (var i = 1; i <= TotalPages; i++)
                pages.Add(i);
            return pages;
        }

        // Always include first and last
        pages.Add(1);

        // Calculate window around current page
        var sideCount = (MaxVisible - 3) / 2; // subtract first, last, and current
        var start = Math.Max(2, CurrentPage - sideCount);
        var end = Math.Min(TotalPages - 1, CurrentPage + sideCount);

        // Adjust if window is too small
        if (CurrentPage - start < sideCount)
            end = Math.Min(TotalPages - 1, end + (sideCount - (CurrentPage - start)));
        if (end - CurrentPage < sideCount)
            start = Math.Max(2, start - (sideCount - (end - CurrentPage)));

        for (var i = start; i <= end; i++)
            pages.Add(i);

        if (TotalPages > 1)
            pages.Add(TotalPages);

        return pages;
    }

    // ──────────────────────────────────────────────
    //  URL and attribute helpers
    // ──────────────────────────────────────────────

    private string? ResolveBaseUrl()
    {
        // Use HxGet if explicitly provided
        if (!string.IsNullOrWhiteSpace(HxGet))
            return HxGet;

        // Try route-based URL generation
        return GenerateRouteUrl();
    }

    private string BuildHtmxAttrsForPage(string? baseUrl, int page)
    {
        var sb = new System.Text.StringBuilder();

        if (!string.IsNullOrWhiteSpace(baseUrl))
        {
            var separator = baseUrl.Contains('?') ? "&" : "?";
            sb.Append($" hx-get=\"{Enc(baseUrl)}{separator}{Enc(PageParam)}={page}\"");
        }

        if (!string.IsNullOrWhiteSpace(HxTarget))
            sb.Append($" hx-target=\"{Enc(HxTarget)}\"");

        if (!string.IsNullOrWhiteSpace(HxSwap))
        {
            var swap = HxSwap;
            if (EnableViewTransition && !swap.Contains("transition:true", StringComparison.OrdinalIgnoreCase))
                swap += " transition:true";
            sb.Append($" hx-swap=\"{Enc(swap)}\"");
        }

        if (!string.IsNullOrWhiteSpace(HxPushUrl))
            sb.Append($" hx-push-url=\"{Enc(HxPushUrl)}\"");

        if (!string.IsNullOrWhiteSpace(HxIndicator))
            sb.Append($" hx-indicator=\"{Enc(HxIndicator)}\"");

        return sb.ToString();
    }

    private void RenderNavButton(TagHelperOutput output, string? baseUrl, int page, string ariaLabel, string svg, string modifier, bool disabled)
    {
        var linkClass = $"{GetElementClass("link")} {GetElementClass("link")}--{modifier}";

        if (disabled)
        {
            output.Content.AppendHtml(
                $"<li class=\"{GetElementClass("item")}\">" +
                $"<button type=\"button\" class=\"{linkClass}\" aria-label=\"{Enc(ariaLabel)}\" disabled>{svg}</button></li>");
        }
        else
        {
            output.Content.AppendHtml(
                $"<li class=\"{GetElementClass("item")}\">" +
                $"<button type=\"button\" class=\"{linkClass}\" aria-label=\"{Enc(ariaLabel)}\"{BuildHtmxAttrsForPage(baseUrl, page)}>{svg}</button></li>");
        }
    }

    // ──────────────────────────────────────────────
    //  Icon SVGs
    // ──────────────────────────────────────────────

    private const string FirstSvg =
        "<svg xmlns=\"http://www.w3.org/2000/svg\" width=\"16\" height=\"16\" viewBox=\"0 0 24 24\" fill=\"none\" stroke=\"currentColor\" stroke-width=\"2\" stroke-linecap=\"round\" stroke-linejoin=\"round\" aria-hidden=\"true\">" +
        "<polyline points=\"11 17 6 12 11 7\"></polyline><polyline points=\"18 17 13 12 18 7\"></polyline></svg>";

    private const string PrevSvg =
        "<svg xmlns=\"http://www.w3.org/2000/svg\" width=\"16\" height=\"16\" viewBox=\"0 0 24 24\" fill=\"none\" stroke=\"currentColor\" stroke-width=\"2\" stroke-linecap=\"round\" stroke-linejoin=\"round\" aria-hidden=\"true\">" +
        "<polyline points=\"15 18 9 12 15 6\"></polyline></svg>";

    private const string NextSvg =
        "<svg xmlns=\"http://www.w3.org/2000/svg\" width=\"16\" height=\"16\" viewBox=\"0 0 24 24\" fill=\"none\" stroke=\"currentColor\" stroke-width=\"2\" stroke-linecap=\"round\" stroke-linejoin=\"round\" aria-hidden=\"true\">" +
        "<polyline points=\"9 18 15 12 9 6\"></polyline></svg>";

    private const string LastSvg =
        "<svg xmlns=\"http://www.w3.org/2000/svg\" width=\"16\" height=\"16\" viewBox=\"0 0 24 24\" fill=\"none\" stroke=\"currentColor\" stroke-width=\"2\" stroke-linecap=\"round\" stroke-linejoin=\"round\" aria-hidden=\"true\">" +
        "<polyline points=\"13 17 18 12 13 7\"></polyline><polyline points=\"6 17 11 12 6 7\"></polyline></svg>";

    private static string Enc(string? value) => WebUtility.HtmlEncode(value ?? "") ?? "";
}
