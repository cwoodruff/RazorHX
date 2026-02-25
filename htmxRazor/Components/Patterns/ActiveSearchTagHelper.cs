using System.Net;
using System.Text;
using htmxRazor.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace htmxRazor.Components.Patterns;

/// <summary>
/// Convenience wrapper for live/active search. Renders a styled search input with
/// pre-configured <c>hx-get</c>, <c>hx-trigger</c> (debounced input), and <c>hx-target</c>
/// so results update as the user types.
/// </summary>
/// <example>
/// <code>
/// &lt;rhx-active-search page="/Search" page-handler="Results"
///                    target="#results" debounce="300" min-length="2"
///                    placeholder="Search..." with-clear="true" name="q" /&gt;
/// </code>
/// </example>
[HtmlTargetElement("rhx-active-search", TagStructure = TagStructure.WithoutEndTag)]
public class ActiveSearchTagHelper : htmxRazorTagHelperBase
{
    /// <inheritdoc/>
    protected override string BlockName => "active-search";

    // ──────────────────────────────────────────────
    //  Route properties
    // ──────────────────────────────────────────────

    /// <summary>
    /// The Razor Page path to generate the hx-get URL for.
    /// </summary>
    [HtmlAttributeName("page")]
    public string? Page { get; set; }

    /// <summary>
    /// The page handler name (e.g., "Results").
    /// </summary>
    [HtmlAttributeName("page-handler")]
    public string? PageHandler { get; set; }

    /// <summary>
    /// Route parameter values for URL generation. Bound from route-* attributes.
    /// </summary>
    [HtmlAttributeName("route-", DictionaryAttributePrefix = "route-")]
    public Dictionary<string, string> RouteValues { get; set; } = new(StringComparer.OrdinalIgnoreCase);

    // ──────────────────────────────────────────────
    //  Search behavior properties
    // ──────────────────────────────────────────────

    /// <summary>
    /// CSS selector of the target element for search results.
    /// </summary>
    [HtmlAttributeName("target")]
    public string? Target { get; set; }

    /// <summary>
    /// Debounce delay in milliseconds before triggering the request. Default: 300.
    /// </summary>
    [HtmlAttributeName("debounce")]
    public int Debounce { get; set; } = 300;

    /// <summary>
    /// Minimum input length before triggering search. Default: 1.
    /// </summary>
    [HtmlAttributeName("min-length")]
    public int MinLength { get; set; } = 1;

    /// <summary>
    /// Placeholder text for the search input.
    /// </summary>
    [HtmlAttributeName("placeholder")]
    public string? Placeholder { get; set; }

    /// <summary>
    /// Show a clear button when the input has a value. Default: false.
    /// </summary>
    [HtmlAttributeName("with-clear")]
    public bool WithClear { get; set; }

    /// <summary>
    /// The query parameter name sent with the request. Default: q.
    /// </summary>
    [HtmlAttributeName("name")]
    public string Name { get; set; } = "q";

    /// <summary>
    /// CSS selector of the element to show as a loading indicator during the request.
    /// </summary>
    [HtmlAttributeName("indicator")]
    public string? Indicator { get; set; }

    /// <summary>
    /// The size of the input. Options: small, medium, large. Default: medium.
    /// </summary>
    [HtmlAttributeName("size")]
    public string Size { get; set; } = "medium";

    // ──────────────────────────────────────────────
    //  Constructor
    // ──────────────────────────────────────────────

    public ActiveSearchTagHelper(IUrlHelperFactory urlHelperFactory) : base(urlHelperFactory) { }

    // ──────────────────────────────────────────────
    //  Rendering
    // ──────────────────────────────────────────────

    /// <inheritdoc/>
    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        HxPage = Page;
        HxHandler = PageHandler;
        foreach (var kv in RouteValues)
            HxRouteValues[kv.Key] = kv.Value;

        output.TagName = "div";
        output.TagMode = TagMode.StartTagAndEndTag;

        var size = Size.ToLowerInvariant();
        var css = CreateCssBuilder()
            .AddIf(GetModifierClass(size), size != "medium");
        ApplyBaseAttributes(output, css);

        var url = GenerateRouteUrl();
        var trigger = $"input changed delay:{Debounce}ms";

        // Build inner HTML using rhx-input CSS classes for visual consistency
        var sb = new StringBuilder();
        sb.Append("<div class=\"rhx-input__control\">");
        sb.Append("<input class=\"rhx-input__native\"");
        sb.Append(" type=\"search\"");
        sb.Append($" name=\"{Enc(Name)}\"");

        if (!string.IsNullOrEmpty(Placeholder))
            sb.Append($" placeholder=\"{Enc(Placeholder)}\"");

        if (!string.IsNullOrWhiteSpace(url))
            sb.Append($" hx-get=\"{Enc(url)}\"");

        sb.Append($" hx-trigger=\"{Enc(trigger)}\"");

        if (!string.IsNullOrWhiteSpace(Target))
            sb.Append($" hx-target=\"{Enc(Target)}\"");

        sb.Append(" hx-include=\"this\"");

        if (!string.IsNullOrWhiteSpace(Indicator))
            sb.Append($" hx-indicator=\"{Enc(Indicator)}\"");

        if (MinLength > 0)
            sb.Append($" minlength=\"{MinLength}\"");

        sb.Append(" autocomplete=\"off\"");
        sb.Append(" />");

        if (WithClear)
        {
            sb.Append("<button class=\"rhx-input__clear\" type=\"button\" aria-label=\"Clear\" hidden>");
            sb.Append("<svg xmlns=\"http://www.w3.org/2000/svg\" width=\"16\" height=\"16\" viewBox=\"0 0 24 24\" fill=\"none\" stroke=\"currentColor\" stroke-width=\"2\" stroke-linecap=\"round\" stroke-linejoin=\"round\">");
            sb.Append("<line x1=\"18\" y1=\"6\" x2=\"6\" y2=\"18\"></line><line x1=\"6\" y1=\"6\" x2=\"18\" y2=\"18\"></line>");
            sb.Append("</svg></button>");
        }

        sb.Append("</div>");

        output.Content.SetHtmlContent(sb.ToString());
    }

    private static string Enc(string? value) => WebUtility.HtmlEncode(value ?? "") ?? "";
}
