using Microsoft.AspNetCore.Razor.TagHelpers;

namespace htmxRazor.Components.DataDisplay;

/// <summary>
/// Metadata for a single data table column, registered by <see cref="ColumnTagHelper"/>.
/// </summary>
public sealed class ColumnDefinition
{
    public string Field { get; set; } = "";
    public string Header { get; set; } = "";
    public bool Sortable { get; set; }
    public string? SortDirection { get; set; }
    public bool Filterable { get; set; }
    public string? FilterValue { get; set; }
    public string? Width { get; set; }
    public string Align { get; set; } = "start";
}

/// <summary>
/// Defines a column in an <c>&lt;rhx-data-table&gt;</c>. This is a child-only Tag Helper
/// that suppresses its output and registers column metadata into the parent context.
/// </summary>
/// <example>
/// <code>
/// &lt;rhx-column rhx-field="name" rhx-header="Name" rhx-sortable="true" /&gt;
/// </code>
/// </example>
[HtmlTargetElement("rhx-column", ParentTag = "rhx-data-table")]
public class ColumnTagHelper : TagHelper
{
    /// <summary>Property name used in sort/filter query parameters.</summary>
    [HtmlAttributeName("rhx-field")]
    public string Field { get; set; } = "";

    /// <summary>Display header text.</summary>
    [HtmlAttributeName("rhx-header")]
    public string Header { get; set; } = "";

    /// <summary>Enable sorting on this column.</summary>
    [HtmlAttributeName("rhx-sortable")]
    public bool Sortable { get; set; }

    /// <summary>Current sort direction: "asc", "desc", or null.</summary>
    [HtmlAttributeName("rhx-sort-direction")]
    public string? SortDirection { get; set; }

    /// <summary>Enable a filter input in the header.</summary>
    [HtmlAttributeName("rhx-filterable")]
    public bool Filterable { get; set; }

    /// <summary>Current filter value.</summary>
    [HtmlAttributeName("rhx-filter-value")]
    public string? FilterValue { get; set; }

    /// <summary>CSS width (e.g., "200px", "30%").</summary>
    [HtmlAttributeName("rhx-width")]
    public string? Width { get; set; }

    /// <summary>Text alignment: start, center, end.</summary>
    [HtmlAttributeName("rhx-align")]
    public string Align { get; set; } = "start";

    /// <inheritdoc/>
    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        if (context.Items.TryGetValue("RhxColumns", out var obj) && obj is List<ColumnDefinition> columns)
        {
            columns.Add(new ColumnDefinition
            {
                Field = Field,
                Header = Header,
                Sortable = Sortable,
                SortDirection = SortDirection?.ToLowerInvariant(),
                Filterable = Filterable,
                FilterValue = FilterValue,
                Width = Width,
                Align = Align.ToLowerInvariant()
            });
        }

        output.SuppressOutput();
    }
}
