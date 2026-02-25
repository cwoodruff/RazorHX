using Microsoft.AspNetCore.Razor.TagHelpers;

namespace htmxRazor.Components.Navigation;

/// <summary>
/// A single item within an <c>&lt;rhx-breadcrumb&gt;</c> trail. The child content
/// becomes the item's label. When <c>href</c> is set, the item renders as a link;
/// otherwise as static text. The parent breadcrumb determines which item is last
/// and applies <c>aria-current="page"</c> accordingly.
/// </summary>
/// <example>
/// <code>
/// &lt;rhx-breadcrumb-item href="/products"&gt;Products&lt;/rhx-breadcrumb-item&gt;
/// &lt;rhx-breadcrumb-item&gt;Widget Pro&lt;/rhx-breadcrumb-item&gt;
/// </code>
/// </example>
[HtmlTargetElement("rhx-breadcrumb-item", ParentTag = "rhx-breadcrumb")]
public class BreadcrumbItemTagHelper : TagHelper
{
    /// <summary>
    /// The navigation URL. When set, the item renders as a link.
    /// When null, the item renders as static text (typically the current page).
    /// </summary>
    [HtmlAttributeName("href")]
    public string? Href { get; set; }

    /// <inheritdoc/>
    public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
    {
        var childContent = await output.GetChildContentAsync();
        var label = childContent.GetContent();

        if (context.Items.TryGetValue("RhxBreadcrumbItems", out var listObj)
            && listObj is List<(string, string?)> items)
        {
            items.Add((label, Href));
        }

        output.SuppressOutput();
    }
}
