using System.Globalization;
using System.Text;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace htmxRazor.Components.Forms;

/// <summary>
/// Renders a custom range slider with fill bar, tooltip, and native range input.
/// Supports model binding via <c>rhx-for</c>, htmx integration on the native input,
/// and configurable min/max/step values.
/// </summary>
/// <example>
/// <code>
/// &lt;rhx-slider name="volume" rhx-label="Volume" rhx-min="0" rhx-max="100" rhx-step="1" /&gt;
///
/// &lt;rhx-slider rhx-for="Brightness" rhx-tooltip="top"
///              hx-post="/settings" hx-trigger="change" /&gt;
/// </code>
/// </example>
[HtmlTargetElement("rhx-slider")]
public class SliderTagHelper : FormControlTagHelperBase
{
    /// <inheritdoc/>
    protected override string BlockName => "slider";

    // ──────────────────────────────────────────────
    //  Slider-specific properties
    // ──────────────────────────────────────────────

    /// <summary>Minimum value for the slider. Default: "0".</summary>
    [HtmlAttributeName("rhx-min")]
    public string Min { get; set; } = "0";

    /// <summary>Maximum value for the slider. Default: "100".</summary>
    [HtmlAttributeName("rhx-max")]
    public string Max { get; set; } = "100";

    /// <summary>Step increment for the slider. Default: "1".</summary>
    [HtmlAttributeName("rhx-step")]
    public string Step { get; set; } = "1";

    /// <summary>Tooltip display mode: "none", "top", "bottom". Default: "none".</summary>
    [HtmlAttributeName("rhx-tooltip")]
    public string Tooltip { get; set; } = "none";

    // ──────────────────────────────────────────────
    //  Constructor
    // ──────────────────────────────────────────────

    public SliderTagHelper(IUrlHelperFactory urlHelperFactory) : base(urlHelperFactory) { }

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
        var resolvedValue = ResolveValue() ?? Min;
        var resolvedRequired = ResolveRequired();
        var hasError = HasError();
        var size = Size.ToLowerInvariant();
        var tooltip = Tooltip.ToLowerInvariant();

        var hintId = $"{resolvedId}-hint";
        var errorId = $"{resolvedId}-error";

        // ── CSS classes on wrapper ──
        var css = CreateCssBuilder()
            .AddIf(GetModifierClass(size), size != "medium")
            .AddIf(GetModifierClass("disabled"), Disabled)
            .AddIf(GetModifierClass("error"), hasError);

        ApplyWrapperAttributes(output, css);
        output.Attributes.SetAttribute("data-rhx-slider", "");
        if (tooltip != "none")
            output.Attributes.SetAttribute("data-rhx-tooltip", tooltip);

        // ── Calculate fill percentage ──
        var fillPercent = CalculateFillPercent(resolvedValue);

        // ── Build inner HTML ──
        var sb = new StringBuilder();

        // Label
        sb.Append(BuildLabelHtml(resolvedId));

        // Track
        sb.Append($"<div class=\"{GetElementClass("track")}\">");

        // Fill bar
        sb.Append($"<div class=\"{GetElementClass("fill")}\" style=\"width: {fillPercent}%\"></div>");

        // Native range input
        sb.Append($"<input type=\"range\" class=\"{GetElementClass("native")}\"");
        sb.Append($" id=\"{Enc(resolvedId)}\"");
        if (!string.IsNullOrEmpty(resolvedName))
            sb.Append($" name=\"{Enc(resolvedName)}\"");
        sb.Append($" value=\"{Enc(resolvedValue)}\"");
        sb.Append($" min=\"{Enc(Min)}\"");
        sb.Append($" max=\"{Enc(Max)}\"");
        sb.Append($" step=\"{Enc(Step)}\"");

        if (Disabled) sb.Append(" disabled");
        if (resolvedRequired) sb.Append(" required");

        // ARIA
        if (!string.IsNullOrEmpty(AriaLabel))
            sb.Append($" aria-label=\"{Enc(AriaLabel)}\"");

        var describedBy = BuildAriaDescribedBy(hintId, errorId);
        if (describedBy != null)
            sb.Append($" aria-describedby=\"{Enc(describedBy)}\"");

        if (hasError) sb.Append(" aria-invalid=\"true\"");
        if (resolvedRequired) sb.Append(" aria-required=\"true\"");

        // htmx and validation on native input
        sb.Append(BuildHtmxAttributeString());
        sb.Append(BuildValidationAttributeString());
        sb.Append(" />");

        // Tooltip
        if (tooltip != "none")
        {
            sb.Append($"<div class=\"{GetElementClass("tooltip")}\" aria-hidden=\"true\">{Enc(resolvedValue)}</div>");
        }

        sb.Append("</div>"); // close track

        // Hint
        sb.Append(BuildHintHtml(hintId));

        // Error
        sb.Append(BuildErrorHtml(errorId));

        output.Content.SetHtmlContent(sb.ToString());
    }

    // ──────────────────────────────────────────────
    //  Fill calculation
    // ──────────────────────────────────────────────

    private string CalculateFillPercent(string value)
    {
        if (double.TryParse(value, NumberStyles.Float, CultureInfo.InvariantCulture, out var val) &&
            double.TryParse(Min, NumberStyles.Float, CultureInfo.InvariantCulture, out var min) &&
            double.TryParse(Max, NumberStyles.Float, CultureInfo.InvariantCulture, out var max) &&
            max > min)
        {
            var percent = ((val - min) / (max - min)) * 100.0;
            percent = Math.Max(0, Math.Min(100, percent));
            return percent.ToString("F1", CultureInfo.InvariantCulture);
        }
        return "0.0";
    }
}
