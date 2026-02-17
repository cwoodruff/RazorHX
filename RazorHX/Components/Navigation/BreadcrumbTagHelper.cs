using System.Net;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Razor.TagHelpers;
using RazorHX.Infrastructure;

namespace RazorHX.Components.Navigation;

/// <summary>
/// Renders a breadcrumb navigation trail. Supports both model-bound items
/// (via the <c>rhx-items</c> property) and manual child items
/// (via <c>&lt;rhx-breadcrumb-item&gt;</c> children).
/// </summary>
/// <remarks>
/// <para>
/// The last item automatically receives <c>aria-current="page"</c>. Items with
/// an <c>href</c> render as links; items without render as static text. Separators
/// are placed between items with <c>aria-hidden="true"</c>.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// &lt;!-- Model-bound --&gt;
/// &lt;rhx-breadcrumb rhx-items="@Model.Breadcrumbs" /&gt;
///
/// &lt;!-- Manual items --&gt;
/// &lt;rhx-breadcrumb&gt;
///     &lt;rhx-breadcrumb-item href="/"&gt;Home&lt;/rhx-breadcrumb-item&gt;
///     &lt;rhx-breadcrumb-item href="/products"&gt;Products&lt;/rhx-breadcrumb-item&gt;
///     &lt;rhx-breadcrumb-item&gt;Widget Pro&lt;/rhx-breadcrumb-item&gt;
/// &lt;/rhx-breadcrumb&gt;
/// </code>
/// </example>
[HtmlTargetElement("rhx-breadcrumb")]
public class BreadcrumbTagHelper : RazorHXTagHelperBase
{
    /// <inheritdoc/>
    protected override string BlockName => "breadcrumb";

    /// <summary>
    /// Accessible label for the navigation landmark. Default: "Breadcrumb".
    /// </summary>
    [HtmlAttributeName("rhx-label")]
    public string Label { get; set; } = "Breadcrumb";

    /// <summary>
    /// The separator character(s) between items. Default: "/".
    /// </summary>
    [HtmlAttributeName("rhx-separator")]
    public string Separator { get; set; } = "/";

    /// <summary>
    /// Model-bound breadcrumb items. When set, child content is ignored.
    /// </summary>
    [HtmlAttributeName("rhx-items")]
    public IList<BreadcrumbItem>? Items { get; set; }

    /// <summary>
    /// Creates a new BreadcrumbTagHelper with URL generation support.
    /// </summary>
    public BreadcrumbTagHelper(IUrlHelperFactory urlHelperFactory) : base(urlHelperFactory) { }

    /// <inheritdoc/>
    public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
    {
        output.TagName = "nav";
        output.TagMode = TagMode.StartTagAndEndTag;

        var css = CreateCssBuilder();
        ApplyBaseAttributes(output, css);
        output.Attributes.SetAttribute("aria-label", Label);

        output.Content.Clear();
        output.Content.AppendHtml($"<ol class=\"{GetElementClass("list")}\">");

        if (Items != null && Items.Count > 0)
        {
            // Render from model — labels need encoding
            for (var i = 0; i < Items.Count; i++)
            {
                var item = Items[i];
                var isLast = i == Items.Count - 1;
                RenderItem(output, Enc(item.Label), item.Href, isLast);
            }
        }
        else
        {
            // Use child items
            var itemsList = new List<(string LabelHtml, string? Href)>();
            context.Items["RhxBreadcrumbItems"] = itemsList;

            await output.GetChildContentAsync();

            for (var i = 0; i < itemsList.Count; i++)
            {
                var (labelHtml, href) = itemsList[i];
                var isLast = i == itemsList.Count - 1;
                // labelHtml is already HTML-safe from Razor engine
                RenderItem(output, labelHtml, href, isLast);
            }
        }

        output.Content.AppendHtml("</ol>");
    }

    /// <summary>
    /// Renders a single breadcrumb item (li with link/span and optional separator).
    /// </summary>
    /// <param name="output">Tag helper output.</param>
    /// <param name="safeLabel">HTML-safe label text.</param>
    /// <param name="href">Raw href (will be encoded), or null for current page item.</param>
    /// <param name="isLast">Whether this is the last item in the trail.</param>
    private void RenderItem(TagHelperOutput output, string safeLabel, string? href, bool isLast)
    {
        // Open li — last item gets aria-current
        output.Content.AppendHtml(isLast
            ? $"<li class=\"{GetElementClass("item")}\" aria-current=\"page\">"
            : $"<li class=\"{GetElementClass("item")}\">");

        // Link or current text
        if (!string.IsNullOrWhiteSpace(href))
        {
            output.Content.AppendHtml(
                $"<a class=\"{GetElementClass("link")}\" href=\"{Enc(href)}\">{safeLabel}</a>");
        }
        else
        {
            output.Content.AppendHtml(
                $"<span class=\"{GetElementClass("current")}\">{safeLabel}</span>");
        }

        // Separator (except for last item)
        if (!isLast)
        {
            output.Content.AppendHtml(
                $"<span class=\"{GetElementClass("separator")}\" aria-hidden=\"true\">{Enc(Separator)}</span>");
        }

        output.Content.AppendHtml("</li>");
    }

    private static string Enc(string? value) => WebUtility.HtmlEncode(value ?? "") ?? "";
}
