using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Razor.TagHelpers;
using RazorHX.Infrastructure;

namespace RazorHX.Components.Patterns;

/// <summary>
/// Convenience wrapper for server polling. Generates a div with <c>hx-get</c> and
/// <c>hx-trigger="every {interval}"</c> that periodically fetches fresh content.
/// The server response should include a new polling element to continue the cycle.
/// </summary>
/// <example>
/// <code>
/// &lt;rhx-poll page="/Dashboard" page-handler="Stats" interval="5s"
///           target="this" swap="outerHTML"&gt;
///     &lt;rhx-skeleton rhx-effect="sheen" rhx-width="100%" rhx-height="200px" /&gt;
/// &lt;/rhx-poll&gt;
/// </code>
/// </example>
[HtmlTargetElement("rhx-poll")]
public class PollTagHelper : RazorHXTagHelperBase
{
    /// <inheritdoc/>
    protected override string BlockName => "poll";

    // ──────────────────────────────────────────────
    //  Route properties
    // ──────────────────────────────────────────────

    /// <summary>
    /// The Razor Page path to generate the hx-get URL for.
    /// </summary>
    [HtmlAttributeName("page")]
    public string? Page { get; set; }

    /// <summary>
    /// The page handler name (e.g., "Stats").
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
    /// The polling interval (e.g., "5s", "10s", "1m"). Default: 5s.
    /// </summary>
    [HtmlAttributeName("interval")]
    public string Interval { get; set; } = "5s";

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

    // ──────────────────────────────────────────────
    //  Constructor
    // ──────────────────────────────────────────────

    public PollTagHelper(IUrlHelperFactory urlHelperFactory) : base(urlHelperFactory) { }

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

        output.Attributes.SetAttribute("hx-trigger", $"every {Interval}");
        output.Attributes.SetAttribute("hx-target", Target);
        output.Attributes.SetAttribute("hx-swap", Swap);

        var childContent = await output.GetChildContentAsync();
        output.Content.SetHtmlContent(childContent);
    }
}
