using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Razor.TagHelpers;
using RazorHX.Infrastructure;

namespace RazorHX.Components.Actions;

/// <summary>
/// Available button visual variants.
/// </summary>
public enum ButtonVariant
{
    /// <summary>Default neutral button.</summary>
    Default,
    /// <summary>Brand/primary color button.</summary>
    Brand,
    /// <summary>Success/positive action button.</summary>
    Success,
    /// <summary>Warning/caution action button.</summary>
    Warning,
    /// <summary>Danger/destructive action button.</summary>
    Danger,
    /// <summary>Ghost button with no background or border.</summary>
    Ghost,
    /// <summary>Text-only link-style button.</summary>
    Text
}

/// <summary>
/// Available button sizes.
/// </summary>
public enum ButtonSize
{
    /// <summary>Small button.</summary>
    Small,
    /// <summary>Default/medium button.</summary>
    Default,
    /// <summary>Large button.</summary>
    Large
}

/// <summary>
/// Renders a styled button element with full htmx and ARIA support.
/// Supports variants, sizes, loading states, and ASP.NET Core route URL generation.
/// </summary>
/// <example>
/// <code>
/// &lt;rhx-button variant="Brand" hx-post="/api/submit" hx-target="#result"&gt;
///     Submit
/// &lt;/rhx-button&gt;
///
/// &lt;rhx-button variant="Danger" hx-post="" hx-page="/Items/Delete" hx-route-id="42"
///             hx-confirm="Are you sure?"&gt;
///     Delete Item
/// &lt;/rhx-button&gt;
/// </code>
/// </example>
[HtmlTargetElement("rhx-button")]
public class ButtonTagHelper : RazorHXTagHelperBase
{
    /// <inheritdoc/>
    protected override string BlockName => "button";

    /// <summary>
    /// The visual variant of the button.
    /// Default: <see cref="ButtonVariant.Default"/>.
    /// </summary>
    [HtmlAttributeName("variant")]
    public ButtonVariant Variant { get; set; } = ButtonVariant.Default;

    /// <summary>
    /// The size of the button.
    /// Default: <see cref="ButtonSize.Default"/>.
    /// </summary>
    [HtmlAttributeName("size")]
    public ButtonSize Size { get; set; } = ButtonSize.Default;

    /// <summary>
    /// Whether the button is disabled.
    /// Sets both the HTML disabled attribute and the ARIA disabled state.
    /// </summary>
    [HtmlAttributeName("disabled")]
    public bool Disabled { get; set; }

    /// <summary>
    /// Whether the button is in a loading state.
    /// Disables the button and shows a loading indicator.
    /// </summary>
    [HtmlAttributeName("loading")]
    public bool Loading { get; set; }

    /// <summary>
    /// Whether the button should take the full width of its container.
    /// </summary>
    [HtmlAttributeName("full-width")]
    public bool FullWidth { get; set; }

    /// <summary>
    /// Whether this is an icon-only button (square aspect ratio).
    /// </summary>
    [HtmlAttributeName("icon-only")]
    public bool IconOnly { get; set; }

    /// <summary>
    /// The HTML button type attribute.
    /// Default: "button".
    /// </summary>
    [HtmlAttributeName("type")]
    public string ButtonType { get; set; } = "button";

    /// <summary>
    /// Accessible label for the button. Required for icon-only buttons.
    /// </summary>
    [HtmlAttributeName("aria-label")]
    public string? AriaLabel { get; set; }

    /// <summary>
    /// Creates a new ButtonTagHelper with URL generation support.
    /// </summary>
    public ButtonTagHelper(IUrlHelperFactory urlHelperFactory) : base(urlHelperFactory) { }

    /// <inheritdoc/>
    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        output.TagName = "button";
        output.TagMode = TagMode.StartTagAndEndTag;

        var css = CreateCssBuilder()
            .Add(GetModifierClass(Variant.ToString().ToLowerInvariant()))
            .AddIf(GetModifierClass("small"), Size == ButtonSize.Small)
            .AddIf(GetModifierClass("large"), Size == ButtonSize.Large)
            .AddIf(GetModifierClass("full"), FullWidth)
            .AddIf(GetModifierClass("icon-only"), IconOnly)
            .AddIf(GetModifierClass("loading"), Loading)
            .AddIf(GetModifierClass("disabled"), Disabled);

        ApplyBaseAttributes(output, css);

        output.Attributes.SetAttribute("type", ButtonType);

        if (Disabled || Loading)
        {
            output.Attributes.SetAttribute("disabled", "disabled");
        }

        if (!string.IsNullOrWhiteSpace(AriaLabel))
        {
            AriaAttributeHelper.AriaLabel(output, AriaLabel);
        }

        RenderHtmxAttributes(output);
    }
}
