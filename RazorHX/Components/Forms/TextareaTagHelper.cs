using System.Text;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace RazorHX.Components.Forms;

/// <summary>
/// Renders a styled textarea with label, hint, error display, and optional auto-resize.
/// Supports model binding via <c>rhx-for</c>.
/// </summary>
/// <example>
/// <code>
/// &lt;rhx-textarea rhx-for="Bio" rhx-rows="5" rhx-resize="auto" /&gt;
///
/// &lt;rhx-textarea rhx-label="Notes" name="notes" rhx-placeholder="Enter notes..."
///               rhx-maxlength="500" /&gt;
/// </code>
/// </example>
[HtmlTargetElement("rhx-textarea")]
public class TextareaTagHelper : FormControlTagHelperBase
{
    /// <inheritdoc/>
    protected override string BlockName => "textarea";

    // ──────────────────────────────────────────────
    //  Textarea-specific properties
    // ──────────────────────────────────────────────

    /// <summary>Placeholder text.</summary>
    [HtmlAttributeName("rhx-placeholder")]
    public string? Placeholder { get; set; }

    /// <summary>Number of visible text rows. Default: 3.</summary>
    [HtmlAttributeName("rhx-rows")]
    public int Rows { get; set; } = 3;

    /// <summary>
    /// Resize behavior. Options: none, vertical, auto. Default: vertical.
    /// When "auto", the textarea grows to fit content via JavaScript.
    /// </summary>
    [HtmlAttributeName("rhx-resize")]
    public string Resize { get; set; } = "vertical";

    /// <summary>Minimum text length. Auto-set from [StringLength] or [MinLength].</summary>
    [HtmlAttributeName("rhx-minlength")]
    public int? Minlength { get; set; }

    /// <summary>Maximum text length. Auto-set from [StringLength] or [MaxLength].</summary>
    [HtmlAttributeName("rhx-maxlength")]
    public int? Maxlength { get; set; }

    // ──────────────────────────────────────────────
    //  Constructor
    // ──────────────────────────────────────────────

    public TextareaTagHelper(IUrlHelperFactory urlHelperFactory) : base(urlHelperFactory) { }

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
        var resolvedValue = ResolveValue();
        var resolvedRequired = ResolveRequired();
        var hasError = HasError();
        var size = Size.ToLowerInvariant();
        var resize = Resize.ToLowerInvariant();

        var hintId = $"{resolvedId}-hint";
        var errorId = $"{resolvedId}-error";

        // ── CSS classes on wrapper ──
        var css = CreateCssBuilder()
            .AddIf(GetModifierClass(size), size != "medium")
            .AddIf(GetModifierClass($"resize-{resize}"), resize != "vertical")
            .AddIf(GetModifierClass("disabled"), Disabled)
            .AddIf(GetModifierClass("readonly"), Readonly)
            .AddIf(GetModifierClass("error"), hasError);

        ApplyWrapperAttributes(output, css);
        output.Attributes.SetAttribute("data-rhx-textarea", "");

        // ── Build inner HTML ──
        var sb = new StringBuilder();

        // Label
        sb.Append(BuildLabelHtml(resolvedId));

        // Control wrapper
        sb.Append($"<div class=\"{GetElementClass("control")}\">");

        // Native textarea
        sb.Append($"<textarea class=\"{GetElementClass("native")}\"");
        sb.Append($" id=\"{Enc(resolvedId)}\"");

        if (!string.IsNullOrEmpty(resolvedName))
            sb.Append($" name=\"{Enc(resolvedName)}\"");

        if (!string.IsNullOrEmpty(Placeholder))
            sb.Append($" placeholder=\"{Enc(Placeholder)}\"");

        sb.Append($" rows=\"{Rows}\"");

        if (resolvedRequired)
            sb.Append(" required");
        if (Disabled)
            sb.Append(" disabled");
        if (Readonly)
            sb.Append(" readonly");

        // Constraints
        var minlength = Minlength ?? ExtractMinlength();
        var maxlength = Maxlength ?? ExtractMaxlength();

        if (minlength.HasValue)
            sb.Append($" minlength=\"{minlength.Value}\"");
        if (maxlength.HasValue)
            sb.Append($" maxlength=\"{maxlength.Value}\"");

        // ARIA
        if (!string.IsNullOrEmpty(AriaLabel))
            sb.Append($" aria-label=\"{Enc(AriaLabel)}\"");

        var describedBy = BuildAriaDescribedBy(hintId, errorId);
        if (describedBy != null)
            sb.Append($" aria-describedby=\"{Enc(describedBy)}\"");

        if (hasError)
            sb.Append(" aria-invalid=\"true\"");

        if (resolvedRequired)
            sb.Append(" aria-required=\"true\"");

        if (resize == "auto")
            sb.Append(" data-rhx-auto-resize");

        // htmx attributes on native textarea
        sb.Append(BuildHtmxAttributeString());

        // data-val attributes
        sb.Append(BuildValidationAttributeString());

        // Close opening tag and add value as content
        sb.Append('>');
        if (resolvedValue != null)
            sb.Append(Enc(resolvedValue));
        sb.Append("</textarea>");

        sb.Append("</div>"); // close control

        // Hint
        sb.Append(BuildHintHtml(hintId));

        // Error
        sb.Append(BuildErrorHtml(errorId));

        output.Content.SetHtmlContent(sb.ToString());
    }
}
