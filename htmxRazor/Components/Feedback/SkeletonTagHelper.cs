using System.Net;
using htmxRazor.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace htmxRazor.Components.Feedback;

/// <summary>
/// Renders a skeleton placeholder element with configurable animation effect and shape.
/// Ideal for content-loading placeholders in htmx lazy-load patterns.
/// </summary>
/// <example>
/// <code>
/// &lt;rhx-skeleton rhx-effect="sheen" rhx-width="200px" rhx-height="24px" /&gt;
///
/// &lt;rhx-skeleton rhx-effect="pulse" rhx-shape="circle"
///               rhx-width="48px" rhx-height="48px" /&gt;
/// </code>
/// </example>
[HtmlTargetElement("rhx-skeleton")]
public class SkeletonTagHelper : htmxRazorTagHelperBase
{
    /// <inheritdoc/>
    protected override string BlockName => "skeleton";

    // ──────────────────────────────────────────────
    //  Properties
    // ──────────────────────────────────────────────

    /// <summary>
    /// The animation effect. Options: none, pulse, sheen. Default: sheen.
    /// </summary>
    [HtmlAttributeName("rhx-effect")]
    public string Effect { get; set; } = "sheen";

    /// <summary>
    /// The width of the skeleton element (CSS value). Default: 100%.
    /// </summary>
    [HtmlAttributeName("rhx-width")]
    public string Width { get; set; } = "100%";

    /// <summary>
    /// The height of the skeleton element (CSS value). Default: 1rem.
    /// </summary>
    [HtmlAttributeName("rhx-height")]
    public string Height { get; set; } = "1rem";

    /// <summary>
    /// The shape of the skeleton. Options: rectangle, rounded, circle.
    /// Default: rounded.
    /// </summary>
    [HtmlAttributeName("rhx-shape")]
    public string Shape { get; set; } = "rounded";

    // ──────────────────────────────────────────────
    //  Constructor
    // ──────────────────────────────────────────────

    public SkeletonTagHelper(IUrlHelperFactory urlHelperFactory) : base(urlHelperFactory) { }

    // ──────────────────────────────────────────────
    //  Rendering
    // ──────────────────────────────────────────────

    /// <inheritdoc/>
    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        output.TagName = "div";
        output.TagMode = TagMode.StartTagAndEndTag;

        var effect = Effect.ToLowerInvariant();
        var shape = Shape.ToLowerInvariant();

        var css = CreateCssBuilder()
            .AddIf(GetModifierClass(effect), effect != "none")
            .AddIf(GetModifierClass(shape), shape != "rounded");

        ApplyBaseAttributes(output, css);

        output.Attributes.SetAttribute("aria-hidden", "true");

        // Inline styles for dimensions and border-radius
        var borderRadius = shape switch
        {
            "circle" => "var(--rhx-radius-full)",
            "rectangle" => "0",
            _ => "var(--rhx-radius-md)" // rounded
        };

        var style = $"width: {WebUtility.HtmlEncode(Width)}; height: {WebUtility.HtmlEncode(Height)}; border-radius: {borderRadius};";
        output.Attributes.SetAttribute("style", style);
    }
}
