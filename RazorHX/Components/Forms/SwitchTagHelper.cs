using System.Text;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace RazorHX.Components.Forms;

/// <summary>
/// Renders a toggle switch with animated thumb on a track, hidden native checkbox input,
/// and hidden false input for ASP.NET Core form binding. Uses <c>role="switch"</c>
/// and <c>aria-checked</c> for accessible toggle semantics.
/// </summary>
/// <example>
/// <code>
/// &lt;rhx-switch name="darkMode" rhx-label="Dark mode" rhx-checked="true" /&gt;
///
/// &lt;rhx-switch rhx-for="Notifications" rhx-label="Enable notifications"
///              hx-post="/settings" hx-trigger="change" /&gt;
/// </code>
/// </example>
[HtmlTargetElement("rhx-switch")]
public class SwitchTagHelper : FormControlTagHelperBase
{
    /// <inheritdoc/>
    protected override string BlockName => "switch";

    // ──────────────────────────────────────────────
    //  Switch-specific properties
    // ──────────────────────────────────────────────

    /// <summary>Whether the switch is on. Auto-resolved from bool model properties.</summary>
    [HtmlAttributeName("rhx-checked")]
    public bool Checked { get; set; }

    // ──────────────────────────────────────────────
    //  Constructor
    // ──────────────────────────────────────────────

    public SwitchTagHelper(IUrlHelperFactory urlHelperFactory) : base(urlHelperFactory) { }

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
        output.Attributes.SetAttribute("data-rhx-switch", "");

        // ── Build inner HTML ──
        var sb = new StringBuilder();

        // Label wrapper
        sb.Append($"<label class=\"{GetElementClass("label")}\">");

        // Hidden false input (ASP.NET Core convention)
        if (!string.IsNullOrEmpty(resolvedName))
            sb.Append($"<input type=\"hidden\" name=\"{Enc(resolvedName)}\" value=\"false\" />");

        // Native checkbox with switch role (visually hidden but accessible)
        sb.Append($"<input type=\"checkbox\" class=\"{GetElementClass("native")} rhx-sr-only\"");
        sb.Append(" role=\"switch\"");
        sb.Append($" id=\"{Enc(resolvedId)}\"");
        if (!string.IsNullOrEmpty(resolvedName))
            sb.Append($" name=\"{Enc(resolvedName)}\"");
        sb.Append(" value=\"true\"");
        sb.Append($" aria-checked=\"{isChecked.ToString().ToLowerInvariant()}\"");
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

        // Track with thumb
        sb.Append($"<span class=\"{GetElementClass("track")}\" aria-hidden=\"true\">");
        sb.Append($"<span class=\"{GetElementClass("thumb")}\"></span>");
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
