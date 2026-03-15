using System.Net;
using htmxRazor.Rendering;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace htmxRazor.Components.DataDisplay;

/// <summary>
/// Renders pagination controls inside an <c>&lt;rhx-data-table&gt;</c>.
/// Uses the parent's slot system to position itself below the table.
/// </summary>
/// <example>
/// <code>
/// &lt;rhx-data-table-pagination rhx-page="1" rhx-page-size="10"
///     rhx-total-items="142" rhx-url="/Products?handler=TableData" /&gt;
/// </code>
/// </example>
[HtmlTargetElement("rhx-data-table-pagination", ParentTag = "rhx-data-table")]
public class DataTablePaginationTagHelper : TagHelper
{
    /// <summary>Current page number (1-based). Default: 1.</summary>
    [HtmlAttributeName("rhx-page")]
    public int Page { get; set; } = 1;

    /// <summary>Rows per page. Default: 10.</summary>
    [HtmlAttributeName("rhx-page-size")]
    public int PageSize { get; set; } = 10;

    /// <summary>Total row count.</summary>
    [HtmlAttributeName("rhx-total-items")]
    public int TotalItems { get; set; }

    /// <summary>Base URL for pagination requests.</summary>
    [HtmlAttributeName("rhx-url")]
    public string? Url { get; set; }

    /// <summary>The htmx target for page requests. If not set, inherits from parent table body.</summary>
    [HtmlAttributeName("rhx-target")]
    public string? Target { get; set; }

    /// <inheritdoc/>
    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        var slots = SlotRenderer.FromContext(context);
        if (slots == null)
        {
            output.SuppressOutput();
            return;
        }

        var totalPages = PageSize > 0 ? (int)Math.Ceiling((double)TotalItems / PageSize) : 1;
        var page = Math.Clamp(Page, 1, Math.Max(1, totalPages));
        var start = (page - 1) * PageSize + 1;
        var end = Math.Min(page * PageSize, TotalItems);
        var isFirst = page <= 1;
        var isLast = page >= totalPages;
        var baseUrl = Enc(Url ?? "");
        var target = !string.IsNullOrWhiteSpace(Target) ? Enc(Target) : "";

        var hxTarget = !string.IsNullOrWhiteSpace(target) ? $" hx-target=\"{target}\"" : "";
        var hxSwap = " hx-swap=\"innerHTML\"";

        var html = $"<div class=\"rhx-data-table__pagination\">" +
            $"<span class=\"rhx-data-table__pagination-info\">" +
            (TotalItems > 0 ? $"Showing {start}\u2013{end} of {TotalItems}" : "No items") +
            "</span>" +
            "<div class=\"rhx-data-table__pagination-controls\">" +
            $"<nav class=\"rhx-data-table__pagination-nav\" aria-label=\"Table pagination\">" +
            RenderButton(baseUrl, 1, "First page", "\u27E8\u27E8", isFirst, hxTarget, hxSwap) +
            RenderButton(baseUrl, page - 1, "Previous page", "\u27E8", isFirst, hxTarget, hxSwap) +
            $"<span class=\"rhx-data-table__page-indicator\">Page {page} of {Math.Max(1, totalPages)}</span>" +
            RenderButton(baseUrl, page + 1, "Next page", "\u27E9", isLast, hxTarget, hxSwap) +
            RenderButton(baseUrl, totalPages, "Last page", "\u27E9\u27E9", isLast, hxTarget, hxSwap) +
            "</nav></div></div>";

        slots.SetHtml("pagination", html);
        output.SuppressOutput();
    }

    private static string RenderButton(string baseUrl, int page, string label, string text, bool disabled, string hxTarget, string hxSwap)
    {
        if (disabled)
            return $"<button class=\"rhx-data-table__pagination-button\" type=\"button\" aria-label=\"{Enc(label)}\" disabled>{text}</button>";

        return $"<button class=\"rhx-data-table__pagination-button\" type=\"button\"" +
            $" hx-get=\"{baseUrl}&amp;page={page}\"{hxTarget}{hxSwap}" +
            $" aria-label=\"{Enc(label)}\">{text}</button>";
    }

    private static string Enc(string? value) => WebUtility.HtmlEncode(value ?? "") ?? "";
}
