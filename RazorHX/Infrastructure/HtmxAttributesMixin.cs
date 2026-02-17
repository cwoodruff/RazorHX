using Microsoft.AspNetCore.Razor.TagHelpers;

namespace RazorHX.Infrastructure;

/// <summary>
/// Provides shared htmx attribute handling for tag helpers.
/// Apply common hx-* attributes to any RazorHX component.
/// </summary>
public static class HtmxAttributesMixin
{
    /// <summary>
    /// Applies htmx attributes from a set of properties to the tag output.
    /// Only sets attributes that have non-null values.
    /// </summary>
    public static void ApplyHtmxAttributes(TagHelperOutput output, IHtmxAttributes attrs)
    {
        SetIfPresent(output, "hx-get", attrs.HxGet);
        SetIfPresent(output, "hx-post", attrs.HxPost);
        SetIfPresent(output, "hx-put", attrs.HxPut);
        SetIfPresent(output, "hx-patch", attrs.HxPatch);
        SetIfPresent(output, "hx-delete", attrs.HxDelete);
        SetIfPresent(output, "hx-target", attrs.HxTarget);
        SetIfPresent(output, "hx-swap", attrs.HxSwap);
        SetIfPresent(output, "hx-trigger", attrs.HxTrigger);
        SetIfPresent(output, "hx-indicator", attrs.HxIndicator);
        SetIfPresent(output, "hx-confirm", attrs.HxConfirm);
        SetIfPresent(output, "hx-vals", attrs.HxVals);
        SetIfPresent(output, "hx-headers", attrs.HxHeaders);
        SetIfPresent(output, "hx-include", attrs.HxInclude);
        SetIfPresent(output, "hx-select", attrs.HxSelect);
        SetIfPresent(output, "hx-push-url", attrs.HxPushUrl);

        if (attrs.HxBoost == true)
        {
            output.Attributes.SetAttribute("hx-boost", "true");
        }

        if (attrs.HxDisabledElt == true)
        {
            output.Attributes.SetAttribute("hx-disabled-elt", "this");
        }
    }

    private static void SetIfPresent(TagHelperOutput output, string name, string? value)
    {
        if (!string.IsNullOrWhiteSpace(value))
        {
            output.Attributes.SetAttribute(name, value);
        }
    }
}

/// <summary>
/// Interface for components that support htmx attributes.
/// </summary>
public interface IHtmxAttributes
{
    string? HxGet { get; set; }
    string? HxPost { get; set; }
    string? HxPut { get; set; }
    string? HxPatch { get; set; }
    string? HxDelete { get; set; }
    string? HxTarget { get; set; }
    string? HxSwap { get; set; }
    string? HxTrigger { get; set; }
    string? HxIndicator { get; set; }
    string? HxConfirm { get; set; }
    string? HxVals { get; set; }
    string? HxHeaders { get; set; }
    string? HxInclude { get; set; }
    string? HxSelect { get; set; }
    string? HxPushUrl { get; set; }
    bool? HxBoost { get; set; }
    bool? HxDisabledElt { get; set; }
}
