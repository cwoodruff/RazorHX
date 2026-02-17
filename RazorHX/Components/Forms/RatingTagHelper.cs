using System.Globalization;
using System.Text;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace RazorHX.Components.Forms;

/// <summary>
/// Renders an interactive star rating component with support for half-star precision,
/// keyboard navigation, hover preview, and model binding.
/// </summary>
/// <example>
/// <code>
/// &lt;rhx-rating name="rating" rhx-label="Rate this product" rhx-max="5" /&gt;
///
/// &lt;rhx-rating rhx-for="Score" rhx-precision="0.5" rhx-readonly="true" /&gt;
/// </code>
/// </example>
[HtmlTargetElement("rhx-rating")]
public class RatingTagHelper : FormControlTagHelperBase
{
    /// <inheritdoc/>
    protected override string BlockName => "rating";

    // ──────────────────────────────────────────────
    //  Rating-specific properties
    // ──────────────────────────────────────────────

    /// <summary>Maximum number of stars. Default: 5.</summary>
    [HtmlAttributeName("rhx-max")]
    public int Max { get; set; } = 5;

    /// <summary>Rating precision: 1 for whole stars, 0.5 for half stars. Default: 1.</summary>
    [HtmlAttributeName("rhx-precision")]
    public double Precision { get; set; } = 1;

    // ──────────────────────────────────────────────
    //  Constructor
    // ──────────────────────────────────────────────

    public RatingTagHelper(IUrlHelperFactory urlHelperFactory) : base(urlHelperFactory) { }

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
        var resolvedValue = ResolveValue() ?? "0";
        var resolvedRequired = ResolveRequired();
        var hasError = HasError();
        var size = Size.ToLowerInvariant();
        var isReadonly = Readonly;

        var hintId = $"{resolvedId}-hint";
        var errorId = $"{resolvedId}-error";

        if (!double.TryParse(resolvedValue, NumberStyles.Float, CultureInfo.InvariantCulture, out var currentValue))
            currentValue = 0;

        // ── CSS classes on wrapper ──
        var css = CreateCssBuilder()
            .AddIf(GetModifierClass(size), size != "medium")
            .AddIf(GetModifierClass("disabled"), Disabled)
            .AddIf(GetModifierClass("readonly"), isReadonly)
            .AddIf(GetModifierClass("error"), hasError);

        ApplyWrapperAttributes(output, css);
        output.Attributes.SetAttribute("data-rhx-rating", "");
        output.Attributes.SetAttribute("data-rhx-precision", Precision.ToString("G", CultureInfo.InvariantCulture));
        output.Attributes.SetAttribute("data-rhx-max", Max.ToString(CultureInfo.InvariantCulture));

        if (!isReadonly && !Disabled)
        {
            output.Attributes.SetAttribute("role", "slider");
            output.Attributes.SetAttribute("aria-valuemin", "0");
            output.Attributes.SetAttribute("aria-valuemax", Max.ToString(CultureInfo.InvariantCulture));
            output.Attributes.SetAttribute("aria-valuenow", currentValue.ToString("G", CultureInfo.InvariantCulture));
            output.Attributes.SetAttribute("tabindex", "0");
        }

        // ── Build inner HTML ──
        var sb = new StringBuilder();

        // Label
        sb.Append(BuildLabelHtml(resolvedId));

        // Stars container
        sb.Append($"<div class=\"{GetElementClass("stars")}\">");

        for (var i = 1; i <= Max; i++)
        {
            var starClass = GetElementClass("star");
            if (currentValue >= i)
                starClass += $" {GetElementClass("star")}--filled";
            else if (Precision <= 0.5 && currentValue >= i - 0.5)
                starClass += $" {GetElementClass("star")}--half";

            sb.Append($"<span class=\"{starClass}\" data-value=\"{i}\">");
            // Star SVG
            sb.Append("<svg xmlns=\"http://www.w3.org/2000/svg\" viewBox=\"0 0 24 24\" fill=\"currentColor\" stroke=\"currentColor\" stroke-width=\"1\" stroke-linecap=\"round\" stroke-linejoin=\"round\">");
            sb.Append("<polygon points=\"12 2 15.09 8.26 22 9.27 17 14.14 18.18 21.02 12 17.77 5.82 21.02 7 14.14 2 9.27 8.91 8.26 12 2\"></polygon>");
            sb.Append("</svg>");
            sb.Append("</span>");
        }

        sb.Append("</div>"); // close stars

        // Hidden input
        sb.Append($"<input type=\"hidden\" class=\"{GetElementClass("value")}\"");
        sb.Append($" id=\"{Enc(resolvedId)}\"");
        if (!string.IsNullOrEmpty(resolvedName))
            sb.Append($" name=\"{Enc(resolvedName)}\"");
        sb.Append($" value=\"{Enc(resolvedValue)}\"");

        if (resolvedRequired) sb.Append(" required");

        // ARIA
        if (!string.IsNullOrEmpty(AriaLabel))
            sb.Append($" aria-label=\"{Enc(AriaLabel)}\"");

        var describedBy = BuildAriaDescribedBy(hintId, errorId);
        if (describedBy != null)
            sb.Append($" aria-describedby=\"{Enc(describedBy)}\"");

        if (hasError) sb.Append(" aria-invalid=\"true\"");
        if (resolvedRequired) sb.Append(" aria-required=\"true\"");

        // htmx and validation
        sb.Append(BuildHtmxAttributeString());
        sb.Append(BuildValidationAttributeString());
        sb.Append(" />");

        // Hint
        sb.Append(BuildHintHtml(hintId));

        // Error
        sb.Append(BuildErrorHtml(errorId));

        output.Content.SetHtmlContent(sb.ToString());
    }
}
