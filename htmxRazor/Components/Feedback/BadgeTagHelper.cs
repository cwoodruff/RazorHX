using htmxRazor.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace htmxRazor.Components.Feedback;

/// <summary>
/// Renders a small badge/label span with color variants, pill shape, and pulse animation.
/// </summary>
/// <example>
/// <code>
/// &lt;rhx-badge rhx-variant="brand"&gt;New&lt;/rhx-badge&gt;
///
/// &lt;rhx-badge rhx-variant="danger" rhx-pill="true" rhx-pulse="true"&gt;3&lt;/rhx-badge&gt;
/// </code>
/// </example>
[HtmlTargetElement("rhx-badge")]
public class BadgeTagHelper : htmxRazorTagHelperBase
{
    /// <inheritdoc/>
    protected override string BlockName => "badge";

    // ──────────────────────────────────────────────
    //  Badge-specific properties
    // ──────────────────────────────────────────────

    /// <summary>
    /// The color variant of the badge.
    /// Options: neutral, brand, success, warning, danger.
    /// Default: neutral.
    /// </summary>
    [HtmlAttributeName("rhx-variant")]
    public string Variant { get; set; } = "neutral";

    /// <summary>
    /// Renders the badge with fully rounded (pill) corners. Default: false.
    /// </summary>
    [HtmlAttributeName("rhx-pill")]
    public bool Pill { get; set; }

    /// <summary>
    /// Adds a pulsing animation for notification indicators. Default: false.
    /// </summary>
    [HtmlAttributeName("rhx-pulse")]
    public bool Pulse { get; set; }

    // ──────────────────────────────────────────────
    //  Constructor
    // ──────────────────────────────────────────────

    public BadgeTagHelper(IUrlHelperFactory urlHelperFactory) : base(urlHelperFactory) { }

    // ──────────────────────────────────────────────
    //  Rendering
    // ──────────────────────────────────────────────

    /// <inheritdoc/>
    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        output.TagName = "span";
        output.TagMode = TagMode.StartTagAndEndTag;

        var variant = Variant.ToLowerInvariant();

        var css = CreateCssBuilder()
            .Add(GetModifierClass(variant))
            .AddIf(GetModifierClass("pill"), Pill)
            .AddIf(GetModifierClass("pulse"), Pulse);

        ApplyBaseAttributes(output, css);
    }
}
