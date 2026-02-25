using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Text;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace RazorHX.Components.Forms;

/// <summary>
/// Renders a searchable/filterable select that combines a text input with a dropdown listbox.
/// Supports client-side filtering by default, or server-side filtering via htmx
/// when <c>rhx-server-filter="true"</c> is set.
/// </summary>
/// <example>
/// <code>
/// &lt;rhx-combobox rhx-for="City" rhx-placeholder="Search cities..." /&gt;
///
/// &lt;rhx-combobox name="city" rhx-label="City" rhx-server-filter="true"
///                hx-get="/api/cities" hx-trigger="input changed delay:200ms"
///                hx-target="next .rhx-combobox__listbox" /&gt;
/// </code>
/// </example>
[HtmlTargetElement("rhx-combobox")]
public class ComboboxTagHelper : FormControlTagHelperBase
{
    /// <inheritdoc/>
    protected override string BlockName => "combobox";

    // ──────────────────────────────────────────────
    //  Combobox-specific properties
    // ──────────────────────────────────────────────

    /// <summary>Placeholder text for the text input.</summary>
    [HtmlAttributeName("rhx-placeholder")]
    public string? Placeholder { get; set; }

    /// <summary>Maximum number of options visible before scrolling. Default: 8.</summary>
    [HtmlAttributeName("rhx-max-visible")]
    public int MaxOptionsVisible { get; set; } = 8;

    /// <summary>Use the filled appearance variant.</summary>
    [HtmlAttributeName("rhx-filled")]
    public bool Filled { get; set; }

    /// <summary>
    /// Collection of SelectListItem to generate options from.
    /// When set, child rhx-option elements are ignored.
    /// </summary>
    [HtmlAttributeName("rhx-items")]
    public IEnumerable<SelectListItem>? Items { get; set; }

    /// <summary>
    /// When true, options are filtered server-side via htmx instead of client-side JS filtering.
    /// Set htmx attributes (hx-get, hx-trigger, hx-target) on this component for server filtering.
    /// </summary>
    [HtmlAttributeName("rhx-server-filter")]
    public bool ServerFilter { get; set; }

    /// <summary>
    /// The query parameter name used for the search text when <see cref="ServerFilter"/> is true.
    /// This name is set on the visible text input so htmx includes the typed value in requests.
    /// Default: "q".
    /// </summary>
    [HtmlAttributeName("rhx-search-param")]
    public string SearchParam { get; set; } = "q";

    // ──────────────────────────────────────────────
    //  Constructor
    // ──────────────────────────────────────────────

    public ComboboxTagHelper(IUrlHelperFactory urlHelperFactory) : base(urlHelperFactory) { }

    // ──────────────────────────────────────────────
    //  Rendering
    // ──────────────────────────────────────────────

    /// <inheritdoc/>
    public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
    {
        output.TagName = "div";
        output.TagMode = TagMode.StartTagAndEndTag;

        var resolvedName = ResolveName();
        var resolvedId = ResolveId();
        var resolvedValue = ResolveValue();
        var resolvedRequired = ResolveRequired();
        var hasError = HasError();
        var size = Size.ToLowerInvariant();

        var hintId = $"{resolvedId}-hint";
        var errorId = $"{resolvedId}-error";
        var listboxId = $"{resolvedId}-listbox";
        var labelId = $"{resolvedId}-label";
        var inputId = $"{resolvedId}-input";

        // Store context for child <rhx-option> elements
        context.Items["OptionClassPrefix"] = "combobox";
        context.Items["SelectedValues"] = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            resolvedValue ?? ""
        };

        var childContent = await output.GetChildContentAsync();

        // ── CSS classes on wrapper ──
        var css = CreateCssBuilder()
            .AddIf(GetModifierClass(size), size != "medium")
            .AddIf(GetModifierClass("filled"), Filled)
            .AddIf(GetModifierClass("disabled"), Disabled)
            .AddIf(GetModifierClass("readonly"), Readonly)
            .AddIf(GetModifierClass("error"), hasError);

        ApplyWrapperAttributes(output, css);
        output.Attributes.SetAttribute("data-rhx-combobox", "");
        if (ServerFilter)
            output.Attributes.SetAttribute("data-rhx-server-filter", "");

        var sb = new StringBuilder();

        // ── Label ──
        var labelText = ResolveLabelText();
        if (!string.IsNullOrEmpty(labelText))
        {
            sb.Append($"<label class=\"{GetElementClass("label")}\" id=\"{Enc(labelId)}\" for=\"{Enc(inputId)}\">{Enc(labelText)}</label>");
        }

        // ── Control wrapper ──
        sb.Append($"<div class=\"{GetElementClass("control")}\">");

        // Text input
        sb.Append($"<input class=\"{GetElementClass("input")}\"");
        sb.Append(" type=\"text\"");
        sb.Append($" id=\"{Enc(inputId)}\"");
        sb.Append(" role=\"combobox\"");
        sb.Append(" aria-expanded=\"false\"");
        sb.Append(" aria-autocomplete=\"list\"");
        sb.Append($" aria-controls=\"{Enc(listboxId)}\"");
        sb.Append(" autocomplete=\"off\"");
        if (ServerFilter && !string.IsNullOrEmpty(SearchParam))
            sb.Append($" name=\"{Enc(SearchParam)}\"");
        if (!string.IsNullOrEmpty(labelText))
            sb.Append($" aria-labelledby=\"{Enc(labelId)}\"");
        if (!string.IsNullOrEmpty(AriaLabel))
            sb.Append($" aria-label=\"{Enc(AriaLabel)}\"");
        if (!string.IsNullOrEmpty(Placeholder))
            sb.Append($" placeholder=\"{Enc(Placeholder)}\"");
        if (!string.IsNullOrEmpty(resolvedValue))
            sb.Append($" value=\"{Enc(GetDisplayTextForValue(resolvedValue))}\"");
        var describedBy = BuildAriaDescribedBy(hintId, errorId);
        if (describedBy != null)
            sb.Append($" aria-describedby=\"{Enc(describedBy)}\"");
        if (hasError)
            sb.Append(" aria-invalid=\"true\"");
        if (resolvedRequired)
            sb.Append(" aria-required=\"true\"");
        if (Disabled)
            sb.Append(" disabled");
        if (Readonly)
            sb.Append(" readonly");
        // htmx attributes on text input (for server-side filtering)
        sb.Append(BuildHtmxAttributeString());
        sb.Append(" />");

        // Trigger button (toggle)
        sb.Append($"<button class=\"{GetElementClass("trigger")}\" type=\"button\" tabindex=\"-1\" aria-label=\"Toggle\"");
        if (Disabled) sb.Append(" disabled");
        sb.Append(">");
        sb.Append("<svg xmlns=\"http://www.w3.org/2000/svg\" width=\"16\" height=\"16\" viewBox=\"0 0 24 24\" fill=\"none\" stroke=\"currentColor\" stroke-width=\"2\" stroke-linecap=\"round\" stroke-linejoin=\"round\">");
        sb.Append("<polyline points=\"6 9 12 15 18 9\"></polyline>");
        sb.Append("</svg></button>");

        sb.Append("</div>"); // close control

        // ── Listbox ──
        sb.Append($"<div class=\"{GetElementClass("listbox")}\" role=\"listbox\" id=\"{Enc(listboxId)}\"");
        if (!string.IsNullOrEmpty(labelText))
            sb.Append($" aria-labelledby=\"{Enc(labelId)}\"");
        sb.Append($" data-rhx-max-visible=\"{MaxOptionsVisible}\"");
        sb.Append(" hidden>");

        var generatedOptions = GenerateOptions(resolvedValue);
        if (!string.IsNullOrEmpty(generatedOptions))
            sb.Append(generatedOptions);
        else
            sb.Append(childContent.GetContent());

        sb.Append("</div>");

        // ── Hidden input for form submission ──
        sb.Append($"<input type=\"hidden\" class=\"{GetElementClass("hidden")}\" data-rhx-combobox-value");
        if (!string.IsNullOrEmpty(resolvedName))
            sb.Append($" name=\"{Enc(resolvedName)}\"");
        sb.Append($" value=\"{Enc(resolvedValue ?? "")}\"");
        sb.Append(BuildValidationAttributeString());
        sb.Append(" />");

        // ── Hint ──
        sb.Append(BuildHintHtml(hintId));

        // ── Error ──
        sb.Append(BuildErrorHtml(errorId));

        output.Content.SetHtmlContent(sb.ToString());
    }

    // ──────────────────────────────────────────────
    //  Option generation
    // ──────────────────────────────────────────────

    private string? GenerateOptions(string? selectedValue)
    {
        if (Items != null)
            return GenerateOptionsFromItems(selectedValue);

        if (For != null)
        {
            var modelType = Nullable.GetUnderlyingType(For.Metadata.ModelType) ?? For.Metadata.ModelType;
            if (modelType.IsEnum)
                return GenerateOptionsFromEnum(modelType, selectedValue);
        }

        return null;
    }

    private string GenerateOptionsFromItems(string? selectedValue)
    {
        var sb = new StringBuilder();
        foreach (var item in Items!)
        {
            var isSelected = string.Equals(item.Value, selectedValue, StringComparison.OrdinalIgnoreCase) || item.Selected;
            sb.Append($"<div class=\"{GetElementClass("option")}");
            if (isSelected) sb.Append($" {GetElementClass("option")}--selected");
            if (item.Disabled) sb.Append($" {GetElementClass("option")}--disabled");
            sb.Append("\" role=\"option\"");
            sb.Append($" data-value=\"{Enc(item.Value)}\"");
            sb.Append($" aria-selected=\"{isSelected.ToString().ToLowerInvariant()}\"");
            if (item.Disabled) sb.Append(" aria-disabled=\"true\"");
            sb.Append(" tabindex=\"-1\">");
            sb.Append(Enc(item.Text));
            sb.Append("</div>");
        }
        return sb.ToString();
    }

    private string GenerateOptionsFromEnum(Type enumType, string? selectedValue)
    {
        var sb = new StringBuilder();
        foreach (var val in Enum.GetValues(enumType))
        {
            var name = val.ToString()!;
            var member = enumType.GetMember(name).FirstOrDefault();
            var displayAttr = member?.GetCustomAttribute<DisplayAttribute>();
            var text = displayAttr?.Name ?? name;
            var isSelected = string.Equals(name, selectedValue, StringComparison.OrdinalIgnoreCase);

            sb.Append($"<div class=\"{GetElementClass("option")}");
            if (isSelected) sb.Append($" {GetElementClass("option")}--selected");
            sb.Append("\" role=\"option\"");
            sb.Append($" data-value=\"{Enc(name)}\"");
            sb.Append($" aria-selected=\"{isSelected.ToString().ToLowerInvariant()}\"");
            sb.Append(" tabindex=\"-1\">");
            sb.Append(Enc(text));
            sb.Append("</div>");
        }
        return sb.ToString();
    }

    private string GetDisplayTextForValue(string? value)
    {
        if (string.IsNullOrEmpty(value)) return "";

        if (Items != null)
        {
            var item = Items.FirstOrDefault(i =>
                string.Equals(i.Value, value, StringComparison.OrdinalIgnoreCase));
            if (item != null) return item.Text;
        }

        if (For != null)
        {
            var modelType = Nullable.GetUnderlyingType(For.Metadata.ModelType) ?? For.Metadata.ModelType;
            if (modelType.IsEnum)
            {
                var member = modelType.GetMember(value).FirstOrDefault();
                var displayAttr = member?.GetCustomAttribute<DisplayAttribute>();
                if (displayAttr?.Name != null) return displayAttr.Name;
            }
        }

        return value;
    }
}
