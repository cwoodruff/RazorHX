using htmxRazor.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace htmxRazor.Components.Patterns;

/// <summary>
/// Declarative wrapper for the htmx SSE extension. Renders a container with
/// <c>hx-ext="sse"</c>, <c>sse-connect</c>, and <c>sse-swap</c> attributes
/// for real-time streaming content via Server-Sent Events.
/// </summary>
/// <example>
/// <code>
/// &lt;rhx-sse-stream page="/Dashboard" page-handler="StatusStream"
///                  rhx-event="status-update" rhx-swap="innerHTML"&gt;
///     &lt;rhx-spinner /&gt;
/// &lt;/rhx-sse-stream&gt;
/// </code>
/// </example>
[HtmlTargetElement("rhx-sse-stream")]
public class SseStreamTagHelper : htmxRazorTagHelperBase
{
    /// <inheritdoc/>
    protected override string BlockName => "sse-stream";

    // ──────────────────────────────────────────────
    //  Connection properties
    // ──────────────────────────────────────────────

    /// <summary>
    /// An explicit URL for the SSE endpoint. Mutually exclusive with page/page-handler route generation.
    /// </summary>
    [HtmlAttributeName("rhx-url")]
    public string? Url { get; set; }

    /// <summary>
    /// The Razor Page path to generate the SSE endpoint URL for.
    /// </summary>
    [HtmlAttributeName("page")]
    public string? Page { get; set; }

    /// <summary>
    /// The page handler name for SSE endpoint URL generation.
    /// </summary>
    [HtmlAttributeName("page-handler")]
    public string? PageHandler { get; set; }

    /// <summary>
    /// Route parameter values for URL generation. Bound from route-* attributes.
    /// </summary>
    [HtmlAttributeName("route-", DictionaryAttributePrefix = "route-")]
    public Dictionary<string, string> RouteValues { get; set; } = new(StringComparer.OrdinalIgnoreCase);

    // ──────────────────────────────────────────────
    //  SSE behavior properties
    // ──────────────────────────────────────────────

    /// <summary>
    /// The SSE event name to listen for. Default: "message".
    /// </summary>
    [HtmlAttributeName("rhx-event")]
    public string EventName { get; set; } = "message";

    /// <summary>
    /// How the SSE event data is swapped into the container. Default: "innerHTML".
    /// </summary>
    [HtmlAttributeName("rhx-swap")]
    public string SseSwap { get; set; } = "innerHTML";

    /// <summary>
    /// An SSE event name that triggers closing the connection.
    /// </summary>
    [HtmlAttributeName("rhx-close-on")]
    public string? CloseOnEvent { get; set; }

    /// <summary>
    /// Whether to automatically reconnect on connection loss. Default: true.
    /// </summary>
    [HtmlAttributeName("rhx-reconnect")]
    public bool Reconnect { get; set; } = true;

    // ──────────────────────────────────────────────
    //  Constructor
    // ──────────────────────────────────────────────

    /// <summary>
    /// Creates a new <see cref="SseStreamTagHelper"/> instance.
    /// </summary>
    public SseStreamTagHelper(IUrlHelperFactory urlHelperFactory) : base(urlHelperFactory) { }

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

        // Merge "sse" into hx-ext (user may have set additional extensions)
        var existingExt = HxExt;
        HxExt = string.IsNullOrWhiteSpace(existingExt) ? "sse" : $"sse,{existingExt}";

        // Determine the SSE connection URL
        var connectUrl = Url;
        if (string.IsNullOrWhiteSpace(connectUrl))
        {
            connectUrl = GenerateRouteUrl();
        }

        if (!string.IsNullOrWhiteSpace(connectUrl))
            output.Attributes.SetAttribute("sse-connect", connectUrl);

        output.Attributes.SetAttribute("sse-swap", EventName);
        output.Attributes.SetAttribute("hx-swap", SseSwap);

        if (!string.IsNullOrWhiteSpace(CloseOnEvent))
            output.Attributes.SetAttribute("sse-close", CloseOnEvent);

        if (!Reconnect)
            output.Attributes.SetAttribute("sse-reconnect", "false");

        // Accessibility — streaming content announced to screen readers
        output.Attributes.SetAttribute("aria-live", "polite");
        output.Attributes.SetAttribute("aria-atomic", "false");

        RenderHtmxAttributes(output);

        var childContent = await output.GetChildContentAsync();
        output.Content.SetHtmlContent(childContent);
    }
}
