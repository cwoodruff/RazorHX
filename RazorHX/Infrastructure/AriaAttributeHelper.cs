using Microsoft.AspNetCore.Razor.TagHelpers;

namespace RazorHX.Infrastructure;

/// <summary>
/// Helper for generating ARIA attributes on tag helper output.
/// </summary>
public static class AriaAttributeHelper
{
    public static void SetRole(TagHelperOutput output, string role)
        => output.Attributes.SetAttribute("role", role);

    public static void SetLabel(TagHelperOutput output, string label)
        => output.Attributes.SetAttribute("aria-label", label);

    public static void SetLabelledBy(TagHelperOutput output, string id)
        => output.Attributes.SetAttribute("aria-labelledby", id);

    public static void SetDescribedBy(TagHelperOutput output, string id)
        => output.Attributes.SetAttribute("aria-describedby", id);

    public static void SetExpanded(TagHelperOutput output, bool expanded)
        => output.Attributes.SetAttribute("aria-expanded", expanded.ToString().ToLowerInvariant());

    public static void SetHasPopup(TagHelperOutput output, string type = "true")
        => output.Attributes.SetAttribute("aria-haspopup", type);

    public static void SetControls(TagHelperOutput output, string id)
        => output.Attributes.SetAttribute("aria-controls", id);

    public static void SetHidden(TagHelperOutput output, bool hidden)
        => output.Attributes.SetAttribute("aria-hidden", hidden.ToString().ToLowerInvariant());

    public static void SetDisabled(TagHelperOutput output, bool disabled)
        => output.Attributes.SetAttribute("aria-disabled", disabled.ToString().ToLowerInvariant());

    public static void SetCurrent(TagHelperOutput output, string value = "page")
        => output.Attributes.SetAttribute("aria-current", value);

    public static void SetLive(TagHelperOutput output, string politeness = "polite")
        => output.Attributes.SetAttribute("aria-live", politeness);

    public static void SetRequired(TagHelperOutput output, bool required)
        => output.Attributes.SetAttribute("aria-required", required.ToString().ToLowerInvariant());

    public static void SetInvalid(TagHelperOutput output, bool invalid)
        => output.Attributes.SetAttribute("aria-invalid", invalid.ToString().ToLowerInvariant());

    public static void SetSelected(TagHelperOutput output, bool selected)
        => output.Attributes.SetAttribute("aria-selected", selected.ToString().ToLowerInvariant());

    /// <summary>
    /// Sets multiple ARIA attributes from a dictionary.
    /// </summary>
    public static void SetAttributes(TagHelperOutput output, IDictionary<string, string> attributes)
    {
        foreach (var (key, value) in attributes)
        {
            var attrName = key.StartsWith("aria-", StringComparison.OrdinalIgnoreCase)
                ? key
                : $"aria-{key}";
            output.Attributes.SetAttribute(attrName, value);
        }
    }
}
