using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Razor.TagHelpers;
using RazorHX.Infrastructure;

namespace RazorHX.Components.Patterns;

/// <summary>
/// Convenience wrapper for the reveal-triggered infinite scroll / load-more pattern.
/// Generates a div with <c>hx-get</c>, <c>hx-trigger="revealed"</c>, and configurable
/// target/swap so new content is appended when the element scrolls into view.
/// </summary>
/// <example>
/// <code>
/// &lt;rhx-infinite-scroll page="/Items" page-handler="LoadMore"
///                      route-page="@(Model.Page + 1)"
///                      target="#item-list" swap="beforeend"&gt;
///     &lt;rhx-spinner /&gt;
/// &lt;/rhx-infinite-scroll&gt;
/// </code>
/// </example>
[HtmlTargetElement("rhx-infinite-scroll")]
public class InfiniteScrollTagHelper : RazorHXTagHelperBase
{
    /// <inheritdoc/>
    protected override string BlockName => "infinite-scroll";

    // ──────────────────────────────────────────────
    //  Route properties
    // ──────────────────────────────────────────────

    /// <summary>
    /// The Razor Page path to generate the hx-get URL for.
    /// </summary>
    [HtmlAttributeName("page")]
    public string? Page { get; set; }

    /// <summary>
    /// The page handler name (e.g., "LoadMore").
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
    /// CSS selector of the target element for appended content.
    /// </summary>
    [HtmlAttributeName("target")]
    public string? Target { get; set; }

    /// <summary>
    /// How the response is swapped relative to the target. Default: beforeend.
    /// </summary>
    [HtmlAttributeName("swap")]
    public string Swap { get; set; } = "beforeend";

    // ──────────────────────────────────────────────
    //  Constructor
    // ──────────────────────────────────────────────

    public InfiniteScrollTagHelper(IUrlHelperFactory urlHelperFactory) : base(urlHelperFactory) { }

    // ──────────────────────────────────────────────
    //  Rendering
    // ──────────────────────────────────────────────

    /// <inheritdoc/>
    public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
    {
        // Bridge convenience properties to base class for URL generation
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

        output.Attributes.SetAttribute("hx-trigger", "revealed");

        if (!string.IsNullOrWhiteSpace(Target))
            output.Attributes.SetAttribute("hx-target", Target);

        output.Attributes.SetAttribute("hx-swap", Swap);

        var childContent = await output.GetChildContentAsync();
        output.Content.SetHtmlContent(childContent);
    }
}
