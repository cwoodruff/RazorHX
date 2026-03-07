using htmxRazor.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace htmxRazor.Components.Utilities;

/// <summary>
/// Renders an ARIA live region that announces content changes to screen readers.
/// Ideal for wrapping htmx swap targets so dynamic content updates are accessible.
/// </summary>
/// <example>
/// <code>
/// &lt;rhx-live-region rhx-politeness="polite" id="search-results"&gt;
///     &lt;!-- htmx-swapped content announced to screen readers --&gt;
/// &lt;/rhx-live-region&gt;
/// </code>
/// </example>
[HtmlTargetElement("rhx-live-region")]
public class LiveRegionTagHelper : htmxRazorTagHelperBase
{
    /// <inheritdoc/>
    protected override string BlockName => "live-region";

    /// <summary>
    /// The politeness level for the live region.
    /// Options: polite (waits for idle), assertive (interrupts), off (disabled).
    /// Default: polite.
    /// </summary>
    [HtmlAttributeName("rhx-politeness")]
    public string Politeness { get; set; } = "polite";

    /// <summary>
    /// Whether the entire region should be announced as a whole when changes occur.
    /// Default: true.
    /// </summary>
    [HtmlAttributeName("rhx-atomic")]
    public bool Atomic { get; set; } = true;

    /// <summary>
    /// Which types of changes should be announced.
    /// Options: additions, removals, text, all.
    /// Default: null (browser default — additions text).
    /// </summary>
    [HtmlAttributeName("rhx-relevant")]
    public string? Relevant { get; set; }

    /// <summary>
    /// Whether the live region is visually hidden but still accessible to screen readers.
    /// Default: false.
    /// </summary>
    [HtmlAttributeName("rhx-visually-hidden")]
    public bool VisuallyHidden { get; set; }

    /// <summary>
    /// The ARIA role for the region. Default: status.
    /// Use "alert" for urgent notifications or "log" for sequential information.
    /// </summary>
    [HtmlAttributeName("rhx-role")]
    public string Role { get; set; } = "status";

    public LiveRegionTagHelper(IUrlHelperFactory urlHelperFactory) : base(urlHelperFactory) { }

    /// <inheritdoc/>
    public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
    {
        output.TagName = "div";
        output.TagMode = TagMode.StartTagAndEndTag;

        var css = CreateCssBuilder();
        if (VisuallyHidden)
            css.Add(GetModifierClass("visually-hidden"));

        ApplyBaseAttributes(output, css);

        output.Attributes.SetAttribute("role", Role);
        AriaAttributeHelper.AriaLive(output, Politeness.ToLowerInvariant());

        if (Atomic)
            output.Attributes.SetAttribute("aria-atomic", "true");

        if (!string.IsNullOrWhiteSpace(Relevant))
            output.Attributes.SetAttribute("aria-relevant", Relevant.ToLowerInvariant());

        RenderHtmxAttributes(output);

        var childContent = await output.GetChildContentAsync();
        output.Content.SetHtmlContent(childContent);
    }
}
