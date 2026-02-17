using Microsoft.AspNetCore.Razor.TagHelpers;

namespace RazorHX.Infrastructure;

/// <summary>
/// Static helper for generating ARIA attributes on tag helper output.
/// Provides both direct-set methods (operating on TagHelperOutput) and
/// key-value pair methods for batch attribute composition.
/// </summary>
public static class AriaAttributeHelper
{
    // ──────────────────────────────────────────────
    //  Role helpers
    // ──────────────────────────────────────────────

    /// <summary>Sets the role attribute to "button".</summary>
    public static void RoleButton(TagHelperOutput output)
        => output.Attributes.SetAttribute("role", "button");

    /// <summary>Sets the role attribute to "dialog".</summary>
    public static void RoleDialog(TagHelperOutput output)
        => output.Attributes.SetAttribute("role", "dialog");

    /// <summary>Sets the role attribute to "alertdialog".</summary>
    public static void RoleAlertDialog(TagHelperOutput output)
        => output.Attributes.SetAttribute("role", "alertdialog");

    /// <summary>Sets the role attribute to "tab".</summary>
    public static void RoleTab(TagHelperOutput output)
        => output.Attributes.SetAttribute("role", "tab");

    /// <summary>Sets the role attribute to "tabpanel".</summary>
    public static void RoleTabPanel(TagHelperOutput output)
        => output.Attributes.SetAttribute("role", "tabpanel");

    /// <summary>Sets the role attribute to "tablist".</summary>
    public static void RoleTabList(TagHelperOutput output)
        => output.Attributes.SetAttribute("role", "tablist");

    /// <summary>Sets the role attribute to "menu".</summary>
    public static void RoleMenu(TagHelperOutput output)
        => output.Attributes.SetAttribute("role", "menu");

    /// <summary>Sets the role attribute to "menuitem".</summary>
    public static void RoleMenuItem(TagHelperOutput output)
        => output.Attributes.SetAttribute("role", "menuitem");

    /// <summary>Sets the role attribute to "navigation".</summary>
    public static void RoleNavigation(TagHelperOutput output)
        => output.Attributes.SetAttribute("role", "navigation");

    /// <summary>Sets the role attribute to "alert".</summary>
    public static void RoleAlert(TagHelperOutput output)
        => output.Attributes.SetAttribute("role", "alert");

    /// <summary>Sets the role attribute to "status".</summary>
    public static void RoleStatus(TagHelperOutput output)
        => output.Attributes.SetAttribute("role", "status");

    /// <summary>Sets the role attribute to "progressbar".</summary>
    public static void RoleProgressBar(TagHelperOutput output)
        => output.Attributes.SetAttribute("role", "progressbar");

    /// <summary>Sets the role attribute to "group".</summary>
    public static void RoleGroup(TagHelperOutput output)
        => output.Attributes.SetAttribute("role", "group");

    /// <summary>Sets the role attribute to "toolbar".</summary>
    public static void RoleToolbar(TagHelperOutput output)
        => output.Attributes.SetAttribute("role", "toolbar");

    /// <summary>Sets a custom role attribute.</summary>
    /// <param name="output">The tag helper output.</param>
    /// <param name="role">The role value.</param>
    public static void SetRole(TagHelperOutput output, string role)
        => output.Attributes.SetAttribute("role", role);

    // ──────────────────────────────────────────────
    //  Label & description
    // ──────────────────────────────────────────────

    /// <summary>Sets the aria-label attribute.</summary>
    /// <param name="output">The tag helper output.</param>
    /// <param name="label">The accessible label text.</param>
    public static void AriaLabel(TagHelperOutput output, string label)
        => output.Attributes.SetAttribute("aria-label", label);

    /// <summary>Sets the aria-labelledby attribute.</summary>
    /// <param name="output">The tag helper output.</param>
    /// <param name="id">The id of the labelling element.</param>
    public static void AriaLabelledBy(TagHelperOutput output, string id)
        => output.Attributes.SetAttribute("aria-labelledby", id);

    /// <summary>Sets the aria-describedby attribute.</summary>
    /// <param name="output">The tag helper output.</param>
    /// <param name="id">The id of the describing element.</param>
    public static void AriaDescribedBy(TagHelperOutput output, string id)
        => output.Attributes.SetAttribute("aria-describedby", id);

    // ──────────────────────────────────────────────
    //  State attributes
    // ──────────────────────────────────────────────

    /// <summary>Sets the aria-expanded attribute.</summary>
    /// <param name="output">The tag helper output.</param>
    /// <param name="expanded">Whether the element is expanded.</param>
    public static void AriaExpanded(TagHelperOutput output, bool expanded)
        => output.Attributes.SetAttribute("aria-expanded", expanded.ToString().ToLowerInvariant());

    /// <summary>Sets the aria-selected attribute.</summary>
    /// <param name="output">The tag helper output.</param>
    /// <param name="selected">Whether the element is selected.</param>
    public static void AriaSelected(TagHelperOutput output, bool selected)
        => output.Attributes.SetAttribute("aria-selected", selected.ToString().ToLowerInvariant());

    /// <summary>Sets the aria-disabled attribute.</summary>
    /// <param name="output">The tag helper output.</param>
    /// <param name="disabled">Whether the element is disabled.</param>
    public static void AriaDisabled(TagHelperOutput output, bool disabled)
        => output.Attributes.SetAttribute("aria-disabled", disabled.ToString().ToLowerInvariant());

    /// <summary>Sets the aria-hidden attribute.</summary>
    /// <param name="output">The tag helper output.</param>
    /// <param name="hidden">Whether the element is hidden from assistive technologies.</param>
    public static void AriaHidden(TagHelperOutput output, bool hidden)
        => output.Attributes.SetAttribute("aria-hidden", hidden.ToString().ToLowerInvariant());

    /// <summary>Sets the aria-checked attribute.</summary>
    /// <param name="output">The tag helper output.</param>
    /// <param name="checked">Whether the element is checked.</param>
    public static void AriaChecked(TagHelperOutput output, bool @checked)
        => output.Attributes.SetAttribute("aria-checked", @checked.ToString().ToLowerInvariant());

    /// <summary>Sets the aria-pressed attribute for toggle buttons.</summary>
    /// <param name="output">The tag helper output.</param>
    /// <param name="pressed">Whether the button is pressed.</param>
    public static void AriaPressed(TagHelperOutput output, bool pressed)
        => output.Attributes.SetAttribute("aria-pressed", pressed.ToString().ToLowerInvariant());

    /// <summary>Sets the aria-required attribute.</summary>
    /// <param name="output">The tag helper output.</param>
    /// <param name="required">Whether the element is required.</param>
    public static void AriaRequired(TagHelperOutput output, bool required)
        => output.Attributes.SetAttribute("aria-required", required.ToString().ToLowerInvariant());

    /// <summary>Sets the aria-invalid attribute.</summary>
    /// <param name="output">The tag helper output.</param>
    /// <param name="invalid">Whether the element's value is invalid.</param>
    public static void AriaInvalid(TagHelperOutput output, bool invalid)
        => output.Attributes.SetAttribute("aria-invalid", invalid.ToString().ToLowerInvariant());

    // ──────────────────────────────────────────────
    //  Relationship & live region attributes
    // ──────────────────────────────────────────────

    /// <summary>Sets the aria-current attribute.</summary>
    /// <param name="output">The tag helper output.</param>
    /// <param name="value">The current value (e.g., "page", "step", "location", "true").</param>
    public static void AriaCurrent(TagHelperOutput output, string value = "page")
        => output.Attributes.SetAttribute("aria-current", value);

    /// <summary>Sets the aria-live attribute for live regions.</summary>
    /// <param name="output">The tag helper output.</param>
    /// <param name="politeness">The politeness level: "polite", "assertive", or "off".</param>
    public static void AriaLive(TagHelperOutput output, string politeness = "polite")
        => output.Attributes.SetAttribute("aria-live", politeness);

    /// <summary>Sets the aria-controls attribute.</summary>
    /// <param name="output">The tag helper output.</param>
    /// <param name="id">The id of the controlled element.</param>
    public static void AriaControls(TagHelperOutput output, string id)
        => output.Attributes.SetAttribute("aria-controls", id);

    /// <summary>Sets the aria-haspopup attribute.</summary>
    /// <param name="output">The tag helper output.</param>
    /// <param name="type">The popup type: "true", "menu", "listbox", "tree", "grid", "dialog".</param>
    public static void AriaHasPopup(TagHelperOutput output, string type = "true")
        => output.Attributes.SetAttribute("aria-haspopup", type);

    /// <summary>Sets the aria-owns attribute.</summary>
    /// <param name="output">The tag helper output.</param>
    /// <param name="ids">Space-separated ids of owned elements.</param>
    public static void AriaOwns(TagHelperOutput output, string ids)
        => output.Attributes.SetAttribute("aria-owns", ids);

    /// <summary>Sets the aria-activedescendant attribute.</summary>
    /// <param name="output">The tag helper output.</param>
    /// <param name="id">The id of the active descendant element.</param>
    public static void AriaActiveDescendant(TagHelperOutput output, string id)
        => output.Attributes.SetAttribute("aria-activedescendant", id);

    // ──────────────────────────────────────────────
    //  Value attributes
    // ──────────────────────────────────────────────

    /// <summary>Sets the aria-valuemin attribute.</summary>
    public static void AriaValueMin(TagHelperOutput output, double value)
        => output.Attributes.SetAttribute("aria-valuemin", value.ToString());

    /// <summary>Sets the aria-valuemax attribute.</summary>
    public static void AriaValueMax(TagHelperOutput output, double value)
        => output.Attributes.SetAttribute("aria-valuemax", value.ToString());

    /// <summary>Sets the aria-valuenow attribute.</summary>
    public static void AriaValueNow(TagHelperOutput output, double value)
        => output.Attributes.SetAttribute("aria-valuenow", value.ToString());

    /// <summary>Sets the aria-valuetext attribute.</summary>
    public static void AriaValueText(TagHelperOutput output, string text)
        => output.Attributes.SetAttribute("aria-valuetext", text);

    // ──────────────────────────────────────────────
    //  Batch setter
    // ──────────────────────────────────────────────

    /// <summary>
    /// Sets multiple ARIA attributes from a dictionary.
    /// Keys without the "aria-" prefix will have it prepended automatically.
    /// </summary>
    /// <param name="output">The tag helper output.</param>
    /// <param name="attributes">Dictionary of attribute names to values.</param>
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
