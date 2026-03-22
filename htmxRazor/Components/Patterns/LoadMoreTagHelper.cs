using htmxRazor.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace htmxRazor.Components.Patterns;

/// <summary>
/// Button-triggered pagination pattern. Renders a styled button that issues an
/// <c>hx-get</c> request to load additional content, then removes itself.
/// Simpler alternative to <see cref="InfiniteScrollTagHelper"/> for content feeds.
/// </summary>
/// <example>
/// <code>
/// &lt;rhx-load-more page="/Items" page-handler="LoadMore"
///                route-page="@(Model.Page + 1)"
///                target="#item-list"&gt;
///     Load more items
/// &lt;/rhx-load-more&gt;
/// </code>
/// </example>
[HtmlTargetElement("rhx-load-more")]
public class LoadMoreTagHelper : htmxRazorTagHelperBase
{
    /// <inheritdoc/>
    protected override string BlockName => "load-more";

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

    /// <summary>
    /// The button variant. Default: neutral.
    /// </summary>
    [HtmlAttributeName("rhx-variant")]
    public string Variant { get; set; } = "neutral";

    /// <summary>
    /// Screen-reader text shown on the loading indicator. Default: "Loading…".
    /// </summary>
    [HtmlAttributeName("rhx-loading-text")]
    public string LoadingText { get; set; } = "Loading\u2026";

    /// <summary>
    /// Whether the load more button is disabled.
    /// </summary>
    [HtmlAttributeName("rhx-disabled")]
    public bool Disabled { get; set; }

    // ──────────────────────────────────────────────
    //  Constructor
    // ──────────────────────────────────────────────

    /// <summary>
    /// Creates a new <see cref="LoadMoreTagHelper"/> instance.
    /// </summary>
    public LoadMoreTagHelper(IUrlHelperFactory urlHelperFactory) : base(urlHelperFactory) { }

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

        // Generate the URL for the button
        var url = GenerateRouteUrl();

        // Build htmx attribute fragments for the inner button
        var htmxAttrs = new List<string>();
        if (!string.IsNullOrWhiteSpace(url))
            htmxAttrs.Add($"hx-get=\"{url}\"");

        if (!string.IsNullOrWhiteSpace(Target))
            htmxAttrs.Add($"hx-target=\"{Target}\"");

        htmxAttrs.Add($"hx-swap=\"{Swap}\"");
        htmxAttrs.Add("hx-indicator=\"closest .rhx-load-more\"");

        // Self-remove after loading, unless the user has set a custom handler
        if (!HxOn.ContainsKey("after-request"))
            htmxAttrs.Add("hx-on::after-request=\"this.closest('.rhx-load-more').remove()\"");

        foreach (var kvp in HxOn)
            htmxAttrs.Add($"hx-on:{kvp.Key}=\"{kvp.Value}\"");

        var htmxAttrString = htmxAttrs.Count > 0 ? " " + string.Join(" ", htmxAttrs) : "";
        var disabledAttr = Disabled ? " disabled aria-disabled=\"true\"" : "";

        var childContent = await output.GetChildContentAsync();
        var labelContent = childContent.IsEmptyOrWhiteSpace ? "Load more" : childContent.GetContent();

        var buttonClass = $"rhx-load-more__button rhx-button rhx-button--{Variant} rhx-button--outlined";

        output.Content.SetHtmlContent(
            $"""
            <button class="{buttonClass}" type="button"{htmxAttrString}{disabledAttr}>
              <span class="rhx-load-more__label">{labelContent}</span>
              <span class="rhx-load-more__indicator rhx-spinner rhx-spinner--current htmx-indicator" role="status">
                <span class="rhx-sr-only">{LoadingText}</span>
              </span>
            </button>
            """);
    }
}
