using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace htmxRazor.Infrastructure;

/// <summary>
/// Registers the <see cref="DataTableRequestModelBinder"/> for <see cref="DataTableRequest"/> parameters.
/// </summary>
public class DataTableRequestModelBinderProvider : IModelBinderProvider
{
    /// <inheritdoc/>
    public IModelBinder? GetBinder(ModelBinderProviderContext context)
    {
        ArgumentNullException.ThrowIfNull(context);

        if (context.Metadata.ModelType == typeof(DataTableRequest))
            return new DataTableRequestModelBinder();

        return null;
    }
}
