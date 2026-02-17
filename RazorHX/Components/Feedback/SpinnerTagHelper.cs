using System.Net;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Razor.TagHelpers;
using RazorHX.Infrastructure;

namespace RazorHX.Components.Feedback;

/// <summary>
/// Renders a spinning loading indicator. Supports named sizes (small, medium, large)
/// or custom CSS size values. Works as an htmx indicator with <c>class="htmx-indicator"</c>.
/// </summary>
/// <example>
/// <code>
/// &lt;rhx-spinner /&gt;
///
/// &lt;rhx-spinner rhx-size="large" rhx-label="Processing" /&gt;
///
/// &lt;rhx-spinner id="loading" class="htmx-indicator" /&gt;
/// </code>
/// </example>
[HtmlTargetElement("rhx-spinner")]
public class SpinnerTagHelper : RazorHXTagHelperBase
{
    /// <inheritdoc/>
    protected override string BlockName => "spinner";

    // ──────────────────────────────────────────────
    //  Properties
    // ──────────────────────────────────────────────

    /// <summary>
    /// The size of the spinner. Named sizes: small, medium (default), large.
    /// Or a custom CSS value (e.g., "2rem", "48px").
    /// </summary>
    [HtmlAttributeName("rhx-size")]
    public string Size { get; set; } = "medium";

    /// <summary>
    /// Accessible label for the spinner. Default: "Loading".
    /// </summary>
    [HtmlAttributeName("rhx-label")]
    public string? Label { get; set; } = "Loading";

    // ──────────────────────────────────────────────
    //  Constructor
    // ──────────────────────────────────────────────

    public SpinnerTagHelper(IUrlHelperFactory urlHelperFactory) : base(urlHelperFactory) { }

    // ──────────────────────────────────────────────
    //  Rendering
    // ──────────────────────────────────────────────

    /// <inheritdoc/>
    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        output.TagName = "span";
        output.TagMode = TagMode.StartTagAndEndTag;

        var size = Size.ToLowerInvariant();
        var isNamedSize = size is "small" or "medium" or "large";

        var css = CreateCssBuilder()
            .AddIf(GetModifierClass(size), isNamedSize && size != "medium");

        ApplyBaseAttributes(output, css);

        output.Attributes.SetAttribute("role", "status");
        output.Attributes.SetAttribute("data-rhx-spinner", "");

        if (!string.IsNullOrEmpty(Label))
            output.Attributes.SetAttribute("aria-label", Label);

        // Custom CSS size
        if (!isNamedSize && !string.IsNullOrEmpty(Size))
        {
            output.Attributes.SetAttribute("style", $"width: {WebUtility.HtmlEncode(Size)}; height: {WebUtility.HtmlEncode(Size)};");
        }

        // ── SVG spinner ──
        output.Content.Clear();
        output.Content.AppendHtml(
            "<svg viewBox=\"0 0 24 24\" fill=\"none\">" +
            "<circle cx=\"12\" cy=\"12\" r=\"10\" stroke=\"currentColor\" stroke-width=\"3\" />" +
            "</svg>");
    }
}
