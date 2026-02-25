using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Text;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace htmxRazor.Components.Forms;

/// <summary>
/// Renders a fieldset wrapper with <c>role="radiogroup"</c>, a legend from the label,
/// and auto-generated radio buttons from <c>rhx-items</c> or enum model binding.
/// Supports child <c>&lt;rhx-radio&gt;</c> elements, inline layout, and htmx on the fieldset.
/// </summary>
/// <example>
/// <code>
/// &lt;rhx-radio-group rhx-label="Priority" name="priority" rhx-for="SelectedPriority" /&gt;
///
/// &lt;rhx-radio-group rhx-label="Size" name="size" rhx-inline="true"&gt;
///     &lt;rhx-radio value="sm"&gt;Small&lt;/rhx-radio&gt;
///     &lt;rhx-radio value="md"&gt;Medium&lt;/rhx-radio&gt;
///     &lt;rhx-radio value="lg"&gt;Large&lt;/rhx-radio&gt;
/// &lt;/rhx-radio-group&gt;
/// </code>
/// </example>
[HtmlTargetElement("rhx-radio-group")]
public class RadioGroupTagHelper : FormControlTagHelperBase
{
    /// <inheritdoc/>
    protected override string BlockName => "radio-group";

    // ──────────────────────────────────────────────
    //  RadioGroup-specific properties
    // ──────────────────────────────────────────────

    /// <summary>
    /// Collection of SelectListItem to generate radio buttons from.
    /// When set, child rhx-radio elements are ignored.
    /// </summary>
    [HtmlAttributeName("rhx-items")]
    public IEnumerable<SelectListItem>? Items { get; set; }

    /// <summary>Arrange radio buttons in a horizontal row instead of vertical stack.</summary>
    [HtmlAttributeName("rhx-inline")]
    public bool Inline { get; set; }

    // ──────────────────────────────────────────────
    //  Constructor
    // ──────────────────────────────────────────────

    public RadioGroupTagHelper(IUrlHelperFactory urlHelperFactory) : base(urlHelperFactory) { }

    // ──────────────────────────────────────────────
    //  Rendering
    // ──────────────────────────────────────────────

    /// <inheritdoc/>
    public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
    {
        output.TagName = "fieldset";
        output.TagMode = TagMode.StartTagAndEndTag;

        var resolvedName = ResolveName();
        var resolvedValue = ResolveValue();
        var resolvedRequired = ResolveRequired();
        var hasError = HasError();
        var size = Size.ToLowerInvariant();
        var resolvedId = ResolveId();

        var hintId = $"{resolvedId}-hint";
        var errorId = $"{resolvedId}-error";

        // Store context for child <rhx-radio> elements
        context.Items["RadioGroupName"] = resolvedName;
        context.Items["RadioGroupValue"] = resolvedValue;
        context.Items["RadioGroupDisabled"] = Disabled;

        // Process children (needed for <rhx-radio> elements)
        var childContent = await output.GetChildContentAsync();

        // ── CSS classes on wrapper ──
        var css = CreateCssBuilder()
            .AddIf(GetModifierClass(size), size != "medium")
            .AddIf(GetModifierClass("inline"), Inline)
            .AddIf(GetModifierClass("disabled"), Disabled)
            .AddIf(GetModifierClass("error"), hasError);

        ApplyWrapperAttributes(output, css);
        output.Attributes.SetAttribute("role", "radiogroup");
        output.Attributes.SetAttribute("data-rhx-radio-group", "");

        if (!string.IsNullOrEmpty(AriaLabel))
            output.Attributes.SetAttribute("aria-label", AriaLabel);

        var describedBy = BuildAriaDescribedBy(hintId, errorId);
        if (describedBy != null)
            output.Attributes.SetAttribute("aria-describedby", describedBy);
        if (hasError)
            output.Attributes.SetAttribute("aria-invalid", "true");
        if (resolvedRequired)
            output.Attributes.SetAttribute("aria-required", "true");

        // htmx on fieldset (change events bubble from radio inputs)
        RenderHtmxAttributes(output);

        // ── Build inner HTML ──
        var sb = new StringBuilder();

        // Legend (from label)
        var labelText = ResolveLabelText();
        if (!string.IsNullOrEmpty(labelText))
            sb.Append($"<legend class=\"{GetElementClass("legend")}\">{Enc(labelText)}</legend>");

        // Items container
        sb.Append($"<div class=\"{GetElementClass("items")}\">");

        var generatedRadios = GenerateRadios(resolvedName, resolvedValue);
        if (!string.IsNullOrEmpty(generatedRadios))
            sb.Append(generatedRadios);
        else
            sb.Append(childContent.GetContent());

        sb.Append("</div>");

        // Hint
        sb.Append(BuildHintHtml(hintId));

        // Error
        sb.Append(BuildErrorHtml(errorId));

        output.Content.SetHtmlContent(sb.ToString());
    }

    // ──────────────────────────────────────────────
    //  Radio generation
    // ──────────────────────────────────────────────

    private string? GenerateRadios(string name, string? selectedValue)
    {
        if (Items != null)
            return GenerateRadiosFromItems(name, selectedValue);

        if (For != null)
        {
            var modelType = Nullable.GetUnderlyingType(For.Metadata.ModelType) ?? For.Metadata.ModelType;
            if (modelType.IsEnum)
                return GenerateRadiosFromEnum(modelType, name, selectedValue);
        }

        return null;
    }

    private string GenerateRadiosFromItems(string name, string? selectedValue)
    {
        var sb = new StringBuilder();
        foreach (var item in Items!)
        {
            var isSelected = string.Equals(item.Value, selectedValue, StringComparison.OrdinalIgnoreCase);
            var isDisabled = item.Disabled || Disabled;

            sb.Append("<label class=\"rhx-radio");
            if (isDisabled) sb.Append(" rhx-radio--disabled");
            sb.Append("\">");

            sb.Append("<input type=\"radio\" class=\"rhx-radio__native rhx-sr-only\"");
            sb.Append($" name=\"{Enc(name)}\" value=\"{Enc(item.Value)}\"");
            if (isSelected) sb.Append(" checked");
            if (isDisabled) sb.Append(" disabled");
            sb.Append(" />");

            sb.Append("<span class=\"rhx-radio__control\" aria-hidden=\"true\"></span>");
            sb.Append($"<span class=\"rhx-radio__text\">{Enc(item.Text)}</span>");
            sb.Append("</label>");
        }
        return sb.ToString();
    }

    private string GenerateRadiosFromEnum(Type enumType, string name, string? selectedValue)
    {
        var sb = new StringBuilder();
        foreach (var val in Enum.GetValues(enumType))
        {
            var enumName = val.ToString()!;
            var member = enumType.GetMember(enumName).FirstOrDefault();
            var displayAttr = member?.GetCustomAttribute<DisplayAttribute>();
            var text = displayAttr?.Name ?? enumName;

            var isSelected = string.Equals(enumName, selectedValue, StringComparison.OrdinalIgnoreCase);

            sb.Append("<label class=\"rhx-radio\">");

            sb.Append("<input type=\"radio\" class=\"rhx-radio__native rhx-sr-only\"");
            sb.Append($" name=\"{Enc(name)}\" value=\"{Enc(enumName)}\"");
            if (isSelected) sb.Append(" checked");
            if (Disabled) sb.Append(" disabled");
            sb.Append(" />");

            sb.Append("<span class=\"rhx-radio__control\" aria-hidden=\"true\"></span>");
            sb.Append($"<span class=\"rhx-radio__text\">{Enc(text)}</span>");
            sb.Append("</label>");
        }
        return sb.ToString();
    }
}
