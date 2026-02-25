using htmxRazor.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace htmxRazor.Components.Patterns;

/// <summary>
/// Convenience wrapper for lazy-loading content from the server.
/// Generates a div with <c>hx-get</c> and <c>hx-trigger="load"</c> (or <c>"revealed"</c>)
/// that replaces itself with server-rendered content. Child content serves as the placeholder.
/// </summary>
/// <example>
/// <code>
/// &lt;rhx-lazy-load page="/Dashboard" page-handler="Chart"
///                target="this" swap="outerHTML"&gt;
///     &lt;rhx-skeleton rhx-effect="sheen" rhx-width="100%" rhx-height="300px" /&gt;
/// &lt;/rhx-lazy-load&gt;
/// </code>
/// </example>
[HtmlTargetElement("rhx-lazy-load")]
public class LazyLoadTagHelper : htmxRazorTagHelperBase
{
    /// <inheritdoc/>
    protected override string BlockName => "lazy-load";

    // ──────────────────────────────────────────────
    //  Route properties
    // ──────────────────────────────────────────────

    /// <summary>
    /// The Razor Page path to generate the hx-get URL for.
    /// </summary>
    [HtmlAttributeName("page")]
    public string? Page { get; set; }

    /// <summary>
    /// The page handler name (e.g., "Chart").
    /// </summary>
    [HtmlAttributeName("page-handler")]
    public string? PageHandler { get; set; }

    /// <summary>
    /// Route parameter values for URL generation. Bound from route-* attributes.
    /// </summary>
    [HtmlAttributeName("route-", DictionaryAttributePrefix = "route-")]
    public Dictionary<string, string> RouteValues { get; set; } = new(StringComparer.OrdinalIgnoreCase);

    // ──────────────────────────────────────────────
    //  Behavior properties
    // ──────────────────────────────────────────────

    /// <summary>
    /// CSS selector of the target element. Default: this.
    /// </summary>
    [HtmlAttributeName("target")]
    public string Target { get; set; } = "this";

    /// <summary>
    /// How the response is swapped. Default: outerHTML.
    /// </summary>
    [HtmlAttributeName("swap")]
    public string Swap { get; set; } = "outerHTML";

    /// <summary>
    /// The htmx trigger for loading. Options: load (fires on DOM insertion),
    /// revealed (fires when scrolled into view). Default: load.
    /// </summary>
    [HtmlAttributeName("trigger")]
    public string Trigger { get; set; } = "load";

    // ──────────────────────────────────────────────
    //  Constructor
    // ──────────────────────────────────────────────

    public LazyLoadTagHelper(IUrlHelperFactory urlHelperFactory) : base(urlHelperFactory) { }

    // ──────────────────────────────────────────────
    //  Rendering
    // ──────────────────────────────────────────────

    /// <inheritdoc/>
    public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
    {
        HxPage = Page;
        HxHandler = PageHandler;
        foreach (var kv in RouteValues)
            HxRouteValues[kv.Key] = kv.Value;

        output.TagName = "div";
        output.TagMode = TagMode.StartTagAndEndTag;

        var css = CreateCssBuilder();
        ApplyBaseAttributes(output, css);

        var url = GenerateRouteUrl();
        if (!string.IsNullOrWhiteSpace(url))
            output.Attributes.SetAttribute("hx-get", url);

        output.Attributes.SetAttribute("hx-trigger", Trigger);
        output.Attributes.SetAttribute("hx-target", Target);
        output.Attributes.SetAttribute("hx-swap", Swap);

        var childContent = await output.GetChildContentAsync();
        output.Content.SetHtmlContent(childContent);
    }
}
