using System.Net;
using htmxRazor.Infrastructure;
using htmxRazor.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace htmxRazor.Components.DataDisplay;

/// <summary>
/// Renders a server-driven data table with sortable columns, filterable headers,
/// and pagination support. Child <c>&lt;rhx-column&gt;</c> elements define the columns.
/// Child content provides the <c>&lt;tr&gt;</c> rows for the table body.
/// </summary>
/// <example>
/// <code>
/// &lt;rhx-data-table id="products" rhx-striped="true" rhx-hoverable="true"
///                   rhx-sort-url="/Products?handler=TableData" rhx-caption="Product list"&gt;
///     &lt;rhx-column rhx-field="name" rhx-header="Name" rhx-sortable="true" /&gt;
///     &lt;rhx-column rhx-field="price" rhx-header="Price" rhx-sortable="true" rhx-align="end" /&gt;
///     @foreach (var item in Model.Items)
///     {
///         &lt;tr&gt;&lt;td&gt;@item.Name&lt;/td&gt;&lt;td&gt;@item.Price&lt;/td&gt;&lt;/tr&gt;
///     }
///     &lt;rhx-data-table-pagination rhx-page="1" rhx-total-items="100" rhx-url="/Products?handler=TableData" /&gt;
/// &lt;/rhx-data-table&gt;
/// </code>
/// </example>
[HtmlTargetElement("rhx-data-table")]
public class DataTableTagHelper : htmxRazorTagHelperBase
{
    /// <inheritdoc/>
    protected override string BlockName => "data-table";

    /// <summary>Alternating row backgrounds.</summary>
    [HtmlAttributeName("rhx-striped")]
    public bool Striped { get; set; }

    /// <summary>Row hover highlight.</summary>
    [HtmlAttributeName("rhx-hoverable")]
    public bool Hoverable { get; set; }

    /// <summary>Cell borders.</summary>
    [HtmlAttributeName("rhx-bordered")]
    public bool Bordered { get; set; }

    /// <summary>Reduced padding.</summary>
    [HtmlAttributeName("rhx-compact")]
    public bool Compact { get; set; }

    /// <summary>Sticky thead on scroll.</summary>
    [HtmlAttributeName("rhx-sticky-header")]
    public bool StickyHeader { get; set; }

    /// <summary>Show a loading overlay.</summary>
    [HtmlAttributeName("rhx-loading")]
    public bool Loading { get; set; }

    /// <summary>Message shown when the table body is empty. Default: "No data".</summary>
    [HtmlAttributeName("rhx-empty-message")]
    public string EmptyMessage { get; set; } = "No data";

    /// <summary>Base URL for sort/filter htmx requests.</summary>
    [HtmlAttributeName("rhx-sort-url")]
    public string? SortUrl { get; set; }

    /// <summary>Accessible caption text for the table.</summary>
    [HtmlAttributeName("rhx-caption")]
    public string? Caption { get; set; }

    /// <summary>Accessible label for the table (aria-label).</summary>
    [HtmlAttributeName("rhx-label")]
    public string? Label { get; set; }

    /// <summary>
    /// Override the htmx target for sort/filter buttons. When set, sort and filter
    /// requests target this selector instead of the table body. Useful when a partial
    /// view re-renders the entire table (headers + body + pagination).
    /// </summary>
    [HtmlAttributeName("rhx-hx-target")]
    public string? HxSortTarget { get; set; }

    /// <summary>
    /// Creates a new DataTableTagHelper with URL generation support.
    /// </summary>
    public DataTableTagHelper(IUrlHelperFactory urlHelperFactory) : base(urlHelperFactory) { }

    /// <inheritdoc/>
    public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
    {
        // Register column list and slot renderer for children
        if (!context.Items.TryGetValue("RhxColumns", out var existingCols) || existingCols is not List<ColumnDefinition>)
        {
            context.Items["RhxColumns"] = new List<ColumnDefinition>();
        }
        var slots = SlotRenderer.CreateForContext(context);

        var childContent = await output.GetChildContentAsync();

        var columns = (List<ColumnDefinition>)context.Items["RhxColumns"];

        output.TagName = "div";
        output.TagMode = TagMode.StartTagAndEndTag;

        var css = CreateCssBuilder()
            .AddIf(GetModifierClass("striped"), Striped)
            .AddIf(GetModifierClass("hoverable"), Hoverable)
            .AddIf(GetModifierClass("bordered"), Bordered)
            .AddIf(GetModifierClass("compact"), Compact)
            .AddIf(GetModifierClass("sticky-header"), StickyHeader)
            .AddIf(GetModifierClass("loading"), Loading);

        ApplyBaseAttributes(output, css);
        output.Attributes.SetAttribute("data-rhx-data-table", "");

        if (Loading)
            output.Attributes.SetAttribute("aria-busy", "true");

        RenderHtmxAttributes(output);

        // Resolve table ID
        var tableId = Id ?? $"rhx-dt-{context.UniqueId}";
        var bodyId = $"{tableId}-body";

        // Build inner HTML
        output.Content.Clear();

        // Scroll container
        output.Content.AppendHtml($"<div class=\"{GetElementClass("scroll-container")}\">");

        // Table
        output.Content.AppendHtml($"<table class=\"{GetElementClass("table")}\" role=\"grid\"");
        if (!string.IsNullOrWhiteSpace(Label))
            output.Content.AppendHtml($" aria-label=\"{Enc(Label)}\"");
        output.Content.AppendHtml(">");

        // Caption
        if (!string.IsNullOrWhiteSpace(Caption))
            output.Content.AppendHtml($"<caption class=\"{GetElementClass("caption")}\">{Enc(Caption)}</caption>");

        // Thead
        if (columns.Count > 0)
        {
            output.Content.AppendHtml($"<thead class=\"{GetElementClass("head")}\">");
            output.Content.AppendHtml("<tr>");

            foreach (var col in columns)
            {
                RenderColumnHeader(output, col, bodyId);
            }

            output.Content.AppendHtml("</tr>");
            output.Content.AppendHtml("</thead>");
        }

        // Tbody
        output.Content.AppendHtml($"<tbody class=\"{GetElementClass("body")}\" id=\"{Enc(bodyId)}\">");
        output.Content.AppendHtml(childContent);
        output.Content.AppendHtml("</tbody>");

        output.Content.AppendHtml("</table>");
        output.Content.AppendHtml("</div>"); // close scroll-container

        // Pagination slot
        if (slots.Has("pagination"))
        {
            output.Content.AppendHtml(slots.Get("pagination")!);
        }

        // Loading overlay
        if (Loading)
        {
            output.Content.AppendHtml(
                $"<div class=\"{GetElementClass("loading-overlay")}\" aria-hidden=\"true\">" +
                "<span class=\"rhx-spinner rhx-spinner--current\" role=\"status\"><span class=\"rhx-sr-only\">Loading</span></span>" +
                "</div>");
        }
    }

    private void RenderColumnHeader(TagHelperOutput output, ColumnDefinition col, string bodyId)
    {
        var headerClass = GetElementClass("header");
        var classes = headerClass;
        if (col.Sortable)
            classes += $" {headerClass}--sortable";
        if (col.SortDirection == "asc")
            classes += $" {headerClass}--asc";
        else if (col.SortDirection == "desc")
            classes += $" {headerClass}--desc";
        if (col.Align != "start")
            classes += $" {headerClass}--{col.Align}";

        output.Content.AppendHtml($"<th class=\"{classes}\" scope=\"col\"");
        if (!string.IsNullOrWhiteSpace(col.Width))
            output.Content.AppendHtml($" style=\"width: {Enc(col.Width)}\"");
        output.Content.AppendHtml(">");

        // Determine htmx target and swap for sort/filter buttons
        var targetSelector = !string.IsNullOrWhiteSpace(HxSortTarget)
            ? Enc(HxSortTarget)
            : $"#{Enc(bodyId)}";
        var swapStrategy = !string.IsNullOrWhiteSpace(HxSortTarget)
            ? "innerHTML"
            : "innerHTML";

        if (col.Sortable && !string.IsNullOrWhiteSpace(SortUrl))
        {
            var nextDir = col.SortDirection == "asc" ? "desc" : "asc";
            var ariaSortValue = col.SortDirection switch
            {
                "asc" => "ascending",
                "desc" => "descending",
                _ => "none"
            };

            output.Content.AppendHtml(
                $"<button class=\"{GetElementClass("sort-button")}\"" +
                $" hx-get=\"{Enc(SortUrl)}&amp;sort={Enc(col.Field)}&amp;dir={nextDir}\"" +
                $" hx-target=\"{targetSelector}\"" +
                $" hx-swap=\"{swapStrategy}\"" +
                $" aria-sort=\"{ariaSortValue}\"" +
                " type=\"button\">");
            output.Content.AppendHtml(Enc(col.Header));
            output.Content.AppendHtml($"<span class=\"{GetElementClass("sort-icon")}\" aria-hidden=\"true\"></span>");
            output.Content.AppendHtml("</button>");
        }
        else
        {
            output.Content.AppendHtml($"<span class=\"{GetElementClass("header-text")}\">{Enc(col.Header)}</span>");
        }

        if (col.Filterable && !string.IsNullOrWhiteSpace(SortUrl))
        {
            output.Content.AppendHtml(
                $"<input class=\"{GetElementClass("filter-input")}\"" +
                " type=\"text\"" +
                $" name=\"filter_{Enc(col.Field)}\"" +
                $" value=\"{Enc(col.FilterValue ?? "")}\"" +
                " placeholder=\"Filter...\"" +
                $" hx-get=\"{Enc(SortUrl)}\"" +
                $" hx-target=\"{targetSelector}\"" +
                $" hx-swap=\"{swapStrategy}\"" +
                " hx-trigger=\"input changed delay:300ms\"" +
                $" hx-include=\"closest table .{GetElementClass("filter-input")}\"" +
                $" aria-label=\"Filter {Enc(col.Header)}\" />");
        }

        output.Content.AppendHtml("</th>");
    }

    private static string Enc(string? value) => WebUtility.HtmlEncode(value ?? "") ?? "";
}
