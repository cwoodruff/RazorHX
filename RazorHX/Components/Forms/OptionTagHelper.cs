using Microsoft.AspNetCore.Razor.TagHelpers;

namespace RazorHX.Components.Forms;

/// <summary>
/// Renders an option element for use inside <c>&lt;rhx-select&gt;</c> or <c>&lt;rhx-combobox&gt;</c>.
/// Reads the parent component's class prefix and selected values from the tag helper context
/// to apply appropriate BEM classes and ARIA attributes.
/// </summary>
/// <example>
/// <code>
/// &lt;rhx-select name="country"&gt;
///     &lt;rhx-option value="us"&gt;United States&lt;/rhx-option&gt;
///     &lt;rhx-option value="ca"&gt;Canada&lt;/rhx-option&gt;
///     &lt;rhx-option value="mx" rhx-disabled="true"&gt;Mexico&lt;/rhx-option&gt;
/// &lt;/rhx-select&gt;
/// </code>
/// </example>
[HtmlTargetElement("rhx-option")]
public class OptionTagHelper : TagHelper
{
    /// <summary>The value submitted with the form when this option is selected.</summary>
    [HtmlAttributeName("value")]
    public string? Value { get; set; }

    /// <summary>Whether this option is disabled and cannot be selected.</summary>
    [HtmlAttributeName("rhx-disabled")]
    public bool Disabled { get; set; }

    /// <inheritdoc/>
    public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
    {
        var childContent = await output.GetChildContentAsync();
        var text = childContent.GetContent().Trim();

        var prefix = context.Items.TryGetValue("OptionClassPrefix", out var p)
            ? p as string ?? "select"
            : "select";
        var selectedValues = context.Items.TryGetValue("SelectedValues", out var sv)
            ? sv as HashSet<string>
            : null;

        var value = Value ?? text;
        var isSelected = selectedValues?.Contains(value) == true;

        output.TagName = "div";
        output.TagMode = TagMode.StartTagAndEndTag;

        var cls = $"rhx-{prefix}__option";
        if (isSelected) cls += $" rhx-{prefix}__option--selected";
        if (Disabled) cls += $" rhx-{prefix}__option--disabled";

        output.Attributes.SetAttribute("class", cls);
        output.Attributes.SetAttribute("role", "option");
        output.Attributes.SetAttribute("data-value", value);
        output.Attributes.SetAttribute("aria-selected", isSelected.ToString().ToLowerInvariant());
        output.Attributes.SetAttribute("tabindex", "-1");

        if (Disabled)
            output.Attributes.SetAttribute("aria-disabled", "true");

        // Content from GetContent() is already HTML-safe (Razor encodes expressions)
        output.Content.SetHtmlContent(text);
    }
}
