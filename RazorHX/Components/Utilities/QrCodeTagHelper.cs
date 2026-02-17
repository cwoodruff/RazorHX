using Microsoft.AspNetCore.Razor.TagHelpers;
using RazorHX.Infrastructure;

namespace RazorHX.Components.Utilities;

/// <summary>
/// Renders a QR code as a <c>&lt;canvas&gt;</c> element. The QR code is generated
/// client-side by a lightweight built-in algorithm (no external dependency).
/// Supports dynamic updates via <c>MutationObserver</c> for htmx swap compatibility.
/// </summary>
/// <example>
/// <code>
/// &lt;rhx-qr-code rhx-value="https://example.com" rhx-size="200" /&gt;
///
/// &lt;rhx-qr-code rhx-value="@Model.ShareUrl"
///               rhx-label="Share link QR code"
///               rhx-error-correction="H" /&gt;
/// </code>
/// </example>
[HtmlTargetElement("rhx-qr-code")]
public class QrCodeTagHelper : RazorHXTagHelperBase
{
    /// <inheritdoc/>
    protected override string BlockName => "qr-code";

    /// <summary>
    /// The data to encode in the QR code.
    /// </summary>
    [HtmlAttributeName("rhx-value")]
    public string Value { get; set; } = "";

    /// <summary>
    /// Accessible label for the QR code image.
    /// </summary>
    [HtmlAttributeName("rhx-label")]
    public string? Label { get; set; }

    /// <summary>
    /// Pixel dimensions (width and height) of the canvas.
    /// Default: 128.
    /// </summary>
    [HtmlAttributeName("rhx-size")]
    public int Size { get; set; } = 128;

    /// <summary>
    /// Foreground (module) color.
    /// Default: "#000000".
    /// </summary>
    [HtmlAttributeName("rhx-fill")]
    public string Fill { get; set; } = "#000000";

    /// <summary>
    /// Background color.
    /// Default: "#ffffff".
    /// </summary>
    [HtmlAttributeName("rhx-background")]
    public string Background { get; set; } = "#ffffff";

    /// <summary>
    /// Corner radius for each module dot, as a fraction of the cell size (0–0.5).
    /// Default: 0.
    /// </summary>
    [HtmlAttributeName("rhx-radius")]
    public double Radius { get; set; }

    /// <summary>
    /// Reed-Solomon error correction level.
    /// Options: L, M, Q, H.
    /// Default: M.
    /// </summary>
    [HtmlAttributeName("rhx-error-correction")]
    public string ErrorCorrection { get; set; } = "M";

    /// <inheritdoc/>
    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        output.TagName = "canvas";
        output.TagMode = TagMode.StartTagAndEndTag;

        // ── CSS classes ──
        var css = CreateCssBuilder();
        ApplyBaseAttributes(output, css);

        // ── Data attributes for JS ──
        output.Attributes.SetAttribute("data-rhx-qr-code", "");
        output.Attributes.SetAttribute("data-rhx-qr-value", Value);
        output.Attributes.SetAttribute("data-rhx-qr-size", Size.ToString());
        output.Attributes.SetAttribute("data-rhx-qr-fill", Fill);
        output.Attributes.SetAttribute("data-rhx-qr-background", Background);
        output.Attributes.SetAttribute("data-rhx-qr-ec", ErrorCorrection.ToUpperInvariant());

        if (Radius > 0)
            output.Attributes.SetAttribute("data-rhx-qr-radius", Radius.ToString("F2"));

        // ── Canvas dimensions ──
        output.Attributes.SetAttribute("width", Size.ToString());
        output.Attributes.SetAttribute("height", Size.ToString());

        // ── Accessibility ──
        output.Attributes.SetAttribute("role", "img");

        if (!string.IsNullOrWhiteSpace(Label))
            AriaAttributeHelper.AriaLabel(output, Label);
    }
}
