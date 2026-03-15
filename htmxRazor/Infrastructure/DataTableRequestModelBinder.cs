using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace htmxRazor.Infrastructure;

/// <summary>
/// Model binder that populates a <see cref="DataTableRequest"/> from query parameters.
/// Reads <c>sort</c>, <c>dir</c>, <c>page</c>, <c>pageSize</c>, and <c>filter_*</c> parameters.
/// </summary>
public class DataTableRequestModelBinder : IModelBinder
{
    /// <inheritdoc/>
    public Task BindModelAsync(ModelBindingContext bindingContext)
    {
        ArgumentNullException.ThrowIfNull(bindingContext);

        var query = bindingContext.HttpContext.Request.Query;
        var request = new DataTableRequest();

        if (query.TryGetValue("sort", out var sort))
            request.Sort = sort.ToString();

        if (query.TryGetValue("dir", out var dir))
            request.SortDirection = dir.ToString();

        if (query.TryGetValue("page", out var page) && int.TryParse(page, out var pageNum))
            request.Page = Math.Max(1, pageNum);

        if (query.TryGetValue("pageSize", out var pageSize) && int.TryParse(pageSize, out var pageSizeNum))
            request.PageSize = Math.Clamp(pageSizeNum, 1, 500);

        // Collect filter_* parameters
        foreach (var key in query.Keys)
        {
            if (key.StartsWith("filter_", StringComparison.OrdinalIgnoreCase) && key.Length > 7)
            {
                var field = key[7..];
                var value = query[key].ToString();
                if (!string.IsNullOrEmpty(value))
                    request.Filters[field] = value;
            }
        }

        bindingContext.Result = ModelBindingResult.Success(request);
        return Task.CompletedTask;
    }
}
