using System.Text;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace RazorHX.Components.Forms;

/// <summary>
/// Renders a color picker with swatch preview, hue/saturation area, hue slider,
/// optional opacity slider, hex/RGB/HSL text input, and optional preset swatches.
/// All color conversion is handled client-side by the companion JavaScript.
/// </summary>
/// <example>
/// <code>
/// &lt;rhx-color-picker name="color" rhx-label="Brand Color" value="#3b82f6" /&gt;
///
/// &lt;rhx-color-picker rhx-for="ThemeColor" rhx-format="rgb" rhx-opacity="true"
///                     rhx-swatches="#ef4444,#f97316,#eab308,#22c55e,#3b82f6,#8b5cf6" /&gt;
/// </code>
/// </example>
[HtmlTargetElement("rhx-color-picker")]
public class ColorPickerTagHelper : FormControlTagHelperBase
{
    /// <inheritdoc/>
    protected override string BlockName => "color-picker";

    // ──────────────────────────────────────────────
    //  Color picker-specific properties
    // ──────────────────────────────────────────────

    /// <summary>Color format: "hex", "rgb", or "hsl". Default: "hex".</summary>
    [HtmlAttributeName("rhx-format")]
    public string Format { get; set; } = "hex";

    /// <summary>Show the picker inline (always open) instead of as a dropdown. Default: false.</summary>
    [HtmlAttributeName("rhx-inline")]
    public bool Inline { get; set; }

    /// <summary>Preset color swatches as a comma-separated list of hex colors.</summary>
    [HtmlAttributeName("rhx-swatches")]
    public string? Swatches { get; set; }

    /// <summary>Enable opacity/alpha slider. Default: false.</summary>
    [HtmlAttributeName("rhx-opacity")]
    public bool Opacity { get; set; }

    // ──────────────────────────────────────────────
    //  Constructor
    // ──────────────────────────────────────────────

    public ColorPickerTagHelper(IUrlHelperFactory urlHelperFactory) : base(urlHelperFactory) { }

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
        var resolvedValue = ResolveValue() ?? "#000000";
        var resolvedRequired = ResolveRequired();
        var hasError = HasError();
        var size = Size.ToLowerInvariant();
        var format = Format.ToLowerInvariant();

        var hintId = $"{resolvedId}-hint";
        var errorId = $"{resolvedId}-error";

        // ── CSS classes on wrapper ──
        var css = CreateCssBuilder()
            .AddIf(GetModifierClass(size), size != "medium")
            .AddIf(GetModifierClass("inline"), Inline)
            .AddIf(GetModifierClass("disabled"), Disabled)
            .AddIf(GetModifierClass("error"), hasError);

        ApplyWrapperAttributes(output, css);
        output.Attributes.SetAttribute("data-rhx-color-picker", "");
        output.Attributes.SetAttribute("data-rhx-format", format);
        if (Opacity)
            output.Attributes.SetAttribute("data-rhx-opacity", "");
        if (Inline)
            output.Attributes.SetAttribute("data-rhx-inline", "");

        // ── Build inner HTML ──
        var sb = new StringBuilder();

        // Label
        sb.Append(BuildLabelHtml(resolvedId));

        // Trigger (swatch preview + text input) — hidden when inline
        if (!Inline)
        {
            sb.Append($"<button type=\"button\" class=\"{GetElementClass("trigger")}\"");
            if (Disabled) sb.Append(" disabled");
            sb.Append(">");
            sb.Append($"<span class=\"{GetElementClass("swatch")}\" style=\"background-color: {Enc(resolvedValue)}\"></span>");
            sb.Append($"<span class=\"{GetElementClass("text")}\">{Enc(resolvedValue)}</span>");
            sb.Append("</button>");
        }

        // Panel
        var panelHidden = Inline ? "" : " hidden";
        sb.Append($"<div class=\"{GetElementClass("panel")}\"{panelHidden}>");

        // Saturation area
        sb.Append($"<div class=\"{GetElementClass("saturation")}\">");
        sb.Append($"<div class=\"{GetElementClass("saturation-cursor")}\"></div>");
        sb.Append("</div>");

        // Hue slider
        sb.Append($"<div class=\"{GetElementClass("sliders")}\">");
        sb.Append($"<div class=\"{GetElementClass("hue")}\">");
        sb.Append($"<input type=\"range\" class=\"{GetElementClass("hue-input")}\" min=\"0\" max=\"360\" step=\"1\" value=\"0\" aria-label=\"Hue\" />");
        sb.Append("</div>");

        // Opacity slider
        if (Opacity)
        {
            sb.Append($"<div class=\"{GetElementClass("opacity")}\">");
            sb.Append($"<input type=\"range\" class=\"{GetElementClass("opacity-input")}\" min=\"0\" max=\"100\" step=\"1\" value=\"100\" aria-label=\"Opacity\" />");
            sb.Append("</div>");
        }

        sb.Append("</div>"); // close sliders

        // Color text input
        sb.Append($"<div class=\"{GetElementClass("input-row")}\">");
        sb.Append($"<input type=\"text\" class=\"{GetElementClass("input")}\" value=\"{Enc(resolvedValue)}\" aria-label=\"Color value\" />");
        sb.Append("</div>");

        // Preset swatches
        if (!string.IsNullOrWhiteSpace(Swatches))
        {
            sb.Append($"<div class=\"{GetElementClass("swatches")}\">");
            foreach (var swatch in Swatches.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries))
            {
                sb.Append($"<button type=\"button\" class=\"{GetElementClass("preset")}\" data-color=\"{Enc(swatch)}\" style=\"background-color: {Enc(swatch)}\" aria-label=\"{Enc(swatch)}\"></button>");
            }
            sb.Append("</div>");
        }

        sb.Append("</div>"); // close panel

        // Hidden input for form submission
        sb.Append($"<input type=\"hidden\" class=\"{GetElementClass("value")}\"");
        sb.Append($" id=\"{Enc(resolvedId)}\"");
        if (!string.IsNullOrEmpty(resolvedName))
            sb.Append($" name=\"{Enc(resolvedName)}\"");
        sb.Append($" value=\"{Enc(resolvedValue)}\"");

        if (resolvedRequired) sb.Append(" required");

        // ARIA
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
