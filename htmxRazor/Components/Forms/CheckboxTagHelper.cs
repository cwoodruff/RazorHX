using System.Text;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace htmxRazor.Components.Forms;

/// <summary>
/// Renders a custom-styled checkbox with hidden native input, visual control with checkmark SVG,
/// and hidden false input for ASP.NET Core form binding. Supports indeterminate state,
/// model binding via <c>rhx-for</c>, and htmx integration on the native input.
/// </summary>
/// <example>
/// <code>
/// &lt;rhx-checkbox name="agree" rhx-label="I agree to the terms" /&gt;
///
/// &lt;rhx-checkbox rhx-for="DarkMode" rhx-label="Enable dark mode"
///                hx-post="/preferences" hx-trigger="change" /&gt;
/// </code>
/// </example>
[HtmlTargetElement("rhx-checkbox")]
public class CheckboxTagHelper : FormControlTagHelperBase
{
    /// <inheritdoc/>
    protected override string BlockName => "checkbox";

    // ──────────────────────────────────────────────
    //  Checkbox-specific properties
    // ──────────────────────────────────────────────

    /// <summary>Whether the checkbox is checked. Auto-resolved from bool model properties.</summary>
    [HtmlAttributeName("rhx-checked")]
    public bool Checked { get; set; }

    /// <summary>Whether the checkbox is in an indeterminate (mixed) state.</summary>
    [HtmlAttributeName("rhx-indeterminate")]
    public bool Indeterminate { get; set; }

    // ──────────────────────────────────────────────
    //  Constructor
    // ──────────────────────────────────────────────

    public CheckboxTagHelper(IUrlHelperFactory urlHelperFactory) : base(urlHelperFactory) { }

    // ──────────────────────────────────────────────
    //  Rendering
    // ──────────────────────────────────────────────

    /// <inheritdoc/>
    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        output.TagName = "div";
        output.TagMode = TagMode.StartTagAndEndTag;

        var resolvedName = ResolveName();
        var resolvedId = ResolveId();
        var resolvedRequired = ResolveRequired();
        var hasError = HasError();
        var size = Size.ToLowerInvariant();
        var isChecked = ResolveChecked();

        var hintId = $"{resolvedId}-hint";
        var errorId = $"{resolvedId}-error";

        // ── CSS classes on wrapper ──
        var css = CreateCssBuilder()
            .AddIf(GetModifierClass(size), size != "medium")
            .AddIf(GetModifierClass("disabled"), Disabled)
            .AddIf(GetModifierClass("error"), hasError);

        ApplyWrapperAttributes(output, css);
        output.Attributes.SetAttribute("data-rhx-checkbox", "");
        if (Indeterminate)
            output.Attributes.SetAttribute("data-rhx-indeterminate", "");

        // ── Build inner HTML ──
        var sb = new StringBuilder();

        // Label wrapper (makes entire row clickable)
        sb.Append($"<label class=\"{GetElementClass("label")}\">");

        // Hidden false input (ASP.NET Core convention: ensures a value is always submitted)
        if (!string.IsNullOrEmpty(resolvedName))
            sb.Append($"<input type=\"hidden\" name=\"{Enc(resolvedName)}\" value=\"false\" />");

        // Native checkbox (visually hidden but accessible)
        sb.Append($"<input type=\"checkbox\" class=\"{GetElementClass("native")} rhx-sr-only\"");
        sb.Append($" id=\"{Enc(resolvedId)}\"");
        if (!string.IsNullOrEmpty(resolvedName))
            sb.Append($" name=\"{Enc(resolvedName)}\"");
        sb.Append(" value=\"true\"");
        if (isChecked) sb.Append(" checked");
        if (Disabled) sb.Append(" disabled");
        if (resolvedRequired) sb.Append(" required");

        // ARIA
        var describedBy = BuildAriaDescribedBy(hintId, errorId);
        if (describedBy != null)
            sb.Append($" aria-describedby=\"{Enc(describedBy)}\"");
        if (hasError) sb.Append(" aria-invalid=\"true\"");
        if (resolvedRequired) sb.Append(" aria-required=\"true\"");
        if (!string.IsNullOrEmpty(AriaLabel))
            sb.Append($" aria-label=\"{Enc(AriaLabel)}\"");

        // htmx and validation on native input
        sb.Append(BuildHtmxAttributeString());
        sb.Append(BuildValidationAttributeString());
        sb.Append(" />");

        // Custom visual control
        sb.Append($"<span class=\"{GetElementClass("control")}\" aria-hidden=\"true\">");
        sb.Append($"<svg class=\"{GetElementClass("check")}\" xmlns=\"http://www.w3.org/2000/svg\" viewBox=\"0 0 24 24\" fill=\"none\" stroke=\"currentColor\" stroke-width=\"3\" stroke-linecap=\"round\" stroke-linejoin=\"round\">");
        sb.Append("<polyline points=\"20 6 9 17 4 12\"></polyline>");
        sb.Append("</svg>");
        sb.Append($"<svg class=\"{GetElementClass("dash")}\" xmlns=\"http://www.w3.org/2000/svg\" viewBox=\"0 0 24 24\" fill=\"none\" stroke=\"currentColor\" stroke-width=\"3\" stroke-linecap=\"round\" stroke-linejoin=\"round\">");
        sb.Append("<line x1=\"5\" y1=\"12\" x2=\"19\" y2=\"12\"></line>");
        sb.Append("</svg>");
        sb.Append("</span>");

        // Label text
        var labelText = ResolveLabelText();
        if (!string.IsNullOrEmpty(labelText))
            sb.Append($"<span class=\"{GetElementClass("text")}\">{Enc(labelText)}</span>");

        sb.Append("</label>");

        // Hint
        sb.Append(BuildHintHtml(hintId));

        // Error
        sb.Append(BuildErrorHtml(errorId));

        output.Content.SetHtmlContent(sb.ToString());
    }

    // ──────────────────────────────────────────────
    //  Checked state resolution
    // ──────────────────────────────────────────────

    private bool ResolveChecked()
    {
        if (Checked) return true;
        if (For?.Model is bool b) return b;
        if (Value != null)
            return string.Equals(Value, "true", StringComparison.OrdinalIgnoreCase);
        return false;
    }
}
