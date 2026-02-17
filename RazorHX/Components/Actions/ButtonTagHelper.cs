using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Razor.TagHelpers;
using RazorHX.Infrastructure;

namespace RazorHX.Components.Actions;

/// <summary>
/// Renders a styled <c>&lt;button&gt;</c> (or <c>&lt;a&gt;</c> when Href is set) element
/// with full htmx integration, ARIA support, and structured inner content.
/// This is the foundational interactive component that establishes the rendering
/// pattern for all RazorHX components.
/// </summary>
/// <remarks>
/// <para>
/// The rendered HTML uses BEM inner elements for structured content:
/// <c>__prefix</c> (icon slot), <c>__label</c> (text content),
/// <c>__suffix</c> (trailing icon slot), and <c>__spinner</c> (loading state).
/// </para>
/// <para>
/// When <see cref="Href"/> is set, the component renders as an <c>&lt;a&gt;</c> tag
/// with link semantics. When <see cref="Loading"/> is true, the button shows a
/// spinner and sets <c>aria-busy="true"</c>.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// &lt;rhx-button rhx-variant="brand" rhx-size="large"&gt;Save&lt;/rhx-button&gt;
///
/// &lt;rhx-button rhx-variant="danger" rhx-appearance="outlined"
///             hx-delete="/api/items/42" hx-confirm="Are you sure?"&gt;
///     Delete
/// &lt;/rhx-button&gt;
///
/// &lt;rhx-button rhx-href="/about" rhx-variant="brand" rhx-appearance="plain"&gt;
///     Learn More
/// &lt;/rhx-button&gt;
/// </code>
/// </example>
[HtmlTargetElement("rhx-button")]
public class ButtonTagHelper : RazorHXTagHelperBase
{
    /// <inheritdoc/>
    protected override string BlockName => "button";

    // ──────────────────────────────────────────────
    //  Visual properties
    // ──────────────────────────────────────────────

    /// <summary>
    /// The color variant of the button.
    /// Options: neutral, brand, success, warning, danger.
    /// Default: neutral.
    /// </summary>
    [HtmlAttributeName("rhx-variant")]
    public string Variant { get; set; } = "neutral";

    /// <summary>
    /// The visual appearance/style of the button.
    /// Options: filled, outlined, plain.
    /// Default: filled.
    /// </summary>
    [HtmlAttributeName("rhx-appearance")]
    public string Appearance { get; set; } = "filled";

    /// <summary>
    /// The size of the button.
    /// Options: small, medium, large.
    /// Default: medium.
    /// </summary>
    [HtmlAttributeName("rhx-size")]
    public string Size { get; set; } = "medium";

    // ──────────────────────────────────────────────
    //  State properties
    // ──────────────────────────────────────────────

    /// <summary>
    /// Whether the button is disabled.
    /// Sets the HTML disabled attribute and ARIA disabled state.
    /// </summary>
    [HtmlAttributeName("rhx-disabled")]
    public bool Disabled { get; set; }

    /// <summary>
    /// Whether the button is in a loading state.
    /// Shows a spinner, disables interaction, and sets <c>aria-busy="true"</c>.
    /// </summary>
    [HtmlAttributeName("rhx-loading")]
    public bool Loading { get; set; }

    // ──────────────────────────────────────────────
    //  Shape modifiers
    // ──────────────────────────────────────────────

    /// <summary>
    /// Renders the button with fully rounded (pill-shaped) corners.
    /// </summary>
    [HtmlAttributeName("rhx-pill")]
    public bool Pill { get; set; }

    /// <summary>
    /// Renders the button as a circle. Intended for icon-only buttons.
    /// </summary>
    [HtmlAttributeName("rhx-circle")]
    public bool Circle { get; set; }

    // ──────────────────────────────────────────────
    //  Link properties (render as <a>)
    // ──────────────────────────────────────────────

    /// <summary>
    /// When set, the button renders as an <c>&lt;a&gt;</c> tag with this URL.
    /// </summary>
    [HtmlAttributeName("rhx-href")]
    public string? Href { get; set; }

    /// <summary>
    /// The link target (e.g., _blank). Only used when <see cref="Href"/> is set.
    /// </summary>
    [HtmlAttributeName("rhx-target")]
    public string? LinkTarget { get; set; }

    /// <summary>
    /// The download filename. Only used when <see cref="Href"/> is set.
    /// </summary>
    [HtmlAttributeName("rhx-download")]
    public string? Download { get; set; }

    // ──────────────────────────────────────────────
    //  Form properties
    // ──────────────────────────────────────────────

    /// <summary>
    /// The HTML button type attribute.
    /// Options: button, submit, reset.
    /// Default: button.
    /// </summary>
    [HtmlAttributeName("type")]
    public string ButtonType { get; set; } = "button";

    /// <summary>
    /// The form submission name for the button.
    /// </summary>
    [HtmlAttributeName("name")]
    public string? Name { get; set; }

    /// <summary>
    /// The form submission value for the button.
    /// </summary>
    [HtmlAttributeName("value")]
    public string? FormValue { get; set; }

    // ──────────────────────────────────────────────
    //  Accessibility
    // ──────────────────────────────────────────────

    /// <summary>
    /// Accessible label for the button. Recommended for icon-only, circle, and
    /// icon-slot-only buttons.
    /// </summary>
    [HtmlAttributeName("aria-label")]
    public string? AriaLabel { get; set; }

    // ──────────────────────────────────────────────
    //  Constructor
    // ──────────────────────────────────────────────

    /// <summary>
    /// Creates a new ButtonTagHelper with route URL generation support.
    /// </summary>
    public ButtonTagHelper(IUrlHelperFactory urlHelperFactory) : base(urlHelperFactory) { }

    // ──────────────────────────────────────────────
    //  Rendering
    // ──────────────────────────────────────────────

    /// <inheritdoc/>
    public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
    {
        var isLink = !string.IsNullOrWhiteSpace(Href);

        // ── Tag name ──
        output.TagName = isLink ? "a" : "button";
        output.TagMode = TagMode.StartTagAndEndTag;

        // ── CSS classes ──
        var variant = Variant.ToLowerInvariant();
        var appearance = Appearance.ToLowerInvariant();
        var size = Size.ToLowerInvariant();

        var css = CreateCssBuilder()
            .Add(GetModifierClass(variant))
            .Add(GetModifierClass(appearance))
            .AddIf(GetModifierClass(size), size != "medium")
            .AddIf(GetModifierClass("pill"), Pill)
            .AddIf(GetModifierClass("circle"), Circle)
            .AddIf(GetModifierClass("loading"), Loading)
            .AddIf(GetModifierClass("disabled"), Disabled && isLink);

        ApplyBaseAttributes(output, css);

        // ── Element-specific attributes ──
        if (isLink)
        {
            output.Attributes.SetAttribute("href", Href);

            if (!string.IsNullOrWhiteSpace(LinkTarget))
            {
                output.Attributes.SetAttribute("target", LinkTarget);
                if (LinkTarget == "_blank")
                {
                    output.Attributes.SetAttribute("rel", "noopener noreferrer");
                }
            }

            if (!string.IsNullOrWhiteSpace(Download))
            {
                output.Attributes.SetAttribute("download", Download);
            }

            if (Disabled)
            {
                AriaAttributeHelper.AriaDisabled(output, true);
                output.Attributes.SetAttribute("tabindex", "-1");
            }

            // Links use role="button" for semantic consistency
            AriaAttributeHelper.SetRole(output, "button");
        }
        else
        {
            output.Attributes.SetAttribute("type", ButtonType);

            if (Disabled || Loading)
            {
                output.Attributes.SetAttribute("disabled", "disabled");
            }

            if (!string.IsNullOrWhiteSpace(Name))
            {
                output.Attributes.SetAttribute("name", Name);
            }

            if (!string.IsNullOrWhiteSpace(FormValue))
            {
                output.Attributes.SetAttribute("value", FormValue);
            }
        }

        // ── Loading state ARIA ──
        if (Loading)
        {
            output.Attributes.SetAttribute("aria-busy", "true");
        }

        // ── Accessible label ──
        if (!string.IsNullOrWhiteSpace(AriaLabel))
        {
            AriaAttributeHelper.AriaLabel(output, AriaLabel);
        }

        // ── htmx attributes ──
        RenderHtmxAttributes(output);

        // ── Inner content structure ──
        var childContent = await output.GetChildContentAsync();
        output.Content.Clear();

        // Spinner (prepended when loading)
        if (Loading)
        {
            output.Content.AppendHtml(
                "<span class=\"rhx-button__spinner\">" +
                "<span class=\"rhx-spinner rhx-spinner--current\" role=\"status\">" +
                "<span class=\"rhx-sr-only\">Loading</span>" +
                "</span>" +
                "</span>");
        }

        // Label wrapper
        output.Content.AppendHtml("<span class=\"rhx-button__label\">");
        output.Content.AppendHtml(childContent);
        output.Content.AppendHtml("</span>");
    }
}
