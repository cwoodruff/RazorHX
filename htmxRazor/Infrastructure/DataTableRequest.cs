namespace htmxRazor.Infrastructure;

/// <summary>
/// Represents a data table request with sort, filter, and pagination parameters.
/// Bound automatically from query parameters by <see cref="DataTableRequestModelBinder"/>.
/// </summary>
public class DataTableRequest
{
    /// <summary>
    /// The field name to sort by.
    /// </summary>
    public string? Sort { get; set; }

    /// <summary>
    /// Sort direction: "asc" or "desc".
    /// </summary>
    public string? SortDirection { get; set; }

    /// <summary>
    /// Current page number (1-based). Default: 1.
    /// </summary>
    public int Page { get; set; } = 1;

    /// <summary>
    /// Number of rows per page. Default: 10.
    /// </summary>
    public int PageSize { get; set; } = 10;

    /// <summary>
    /// Filter values keyed by field name. Populated from <c>filter_{field}</c> query parameters.
    /// </summary>
    public Dictionary<string, string> Filters { get; set; } = new();
}
