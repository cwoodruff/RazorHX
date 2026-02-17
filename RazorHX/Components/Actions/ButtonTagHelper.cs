using Microsoft.AspNetCore.Razor.TagHelpers;
using RazorHX.Infrastructure;

namespace RazorHX.Components.Actions;

public enum ButtonVariant
{
    Default,
    Brand,
    Success,
    Warning,
    Danger,
    Ghost,
    Text
}

public enum ButtonSize
{
    Small,
    Default,
    Large
}

/// <summary>
/// Renders a styled button with htmx support.
/// Usage: &lt;rhx-button variant="brand" hx-post="/api/submit"&gt;Submit&lt;/rhx-button&gt;
/// </summary>
[HtmlTargetElement("rhx-button")]
public class ButtonTagHelper : RazorHXTagHelperBase, IHtmxAttributes
{
    protected override string BlockName => "button";

    [HtmlAttributeName("variant")]
    public ButtonVariant Variant { get; set; } = ButtonVariant.Default;

    [HtmlAttributeName("size")]
    public ButtonSize Size { get; set; } = ButtonSize.Default;

    [HtmlAttributeName("disabled")]
    public bool Disabled { get; set; }

    [HtmlAttributeName("loading")]
    public bool Loading { get; set; }

    [HtmlAttributeName("full-width")]
    public bool FullWidth { get; set; }

    [HtmlAttributeName("icon-only")]
    public bool IconOnly { get; set; }

    [HtmlAttributeName("type")]
    public string ButtonType { get; set; } = "button";

    [HtmlAttributeName("aria-label")]
    public string? AriaLabel { get; set; }

    // htmx attributes
    [HtmlAttributeName("hx-get")] public string? HxGet { get; set; }
    [HtmlAttributeName("hx-post")] public string? HxPost { get; set; }
    [HtmlAttributeName("hx-put")] public string? HxPut { get; set; }
    [HtmlAttributeName("hx-patch")] public string? HxPatch { get; set; }
    [HtmlAttributeName("hx-delete")] public string? HxDelete { get; set; }
    [HtmlAttributeName("hx-target")] public string? HxTarget { get; set; }
    [HtmlAttributeName("hx-swap")] public string? HxSwap { get; set; }
    [HtmlAttributeName("hx-trigger")] public string? HxTrigger { get; set; }
    [HtmlAttributeName("hx-indicator")] public string? HxIndicator { get; set; }
    [HtmlAttributeName("hx-confirm")] public string? HxConfirm { get; set; }
    [HtmlAttributeName("hx-vals")] public string? HxVals { get; set; }
    [HtmlAttributeName("hx-headers")] public string? HxHeaders { get; set; }
    [HtmlAttributeName("hx-include")] public string? HxInclude { get; set; }
    [HtmlAttributeName("hx-select")] public string? HxSelect { get; set; }
    [HtmlAttributeName("hx-push-url")] public string? HxPushUrl { get; set; }
    [HtmlAttributeName("hx-boost")] public bool? HxBoost { get; set; }
    [HtmlAttributeName("hx-disabled-elt")] public bool? HxDisabledElt { get; set; }

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
            AriaAttributeHelper.SetLabel(output, AriaLabel);
        }

        HtmxAttributesMixin.ApplyHtmxAttributes(output, this);
    }
}
