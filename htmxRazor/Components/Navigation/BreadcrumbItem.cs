namespace htmxRazor.Components.Navigation;

/// <summary>
/// Data model for a breadcrumb navigation item.
/// Used with the <c>rhx-items</c> property on <c>&lt;rhx-breadcrumb&gt;</c>
/// for server-side model binding.
/// </summary>
/// <param name="Label">The display text of the breadcrumb item.</param>
/// <param name="Href">The URL to navigate to. Null for the current page (last item).</param>
public record BreadcrumbItem(string Label, string? Href = null);
