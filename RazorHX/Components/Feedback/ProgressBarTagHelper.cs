using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Razor.TagHelpers;
using RazorHX.Infrastructure;

namespace RazorHX.Components.Feedback;

/// <summary>
/// Renders a horizontal progress bar with track, fill, and optional label.
/// Supports determinate (value-based) and indeterminate (animated) variants.
/// </summary>
/// <example>
/// <code>
/// &lt;rhx-progress-bar rhx-value="65" rhx-label="Upload progress" /&gt;
///
/// &lt;rhx-progress-bar rhx-indeterminate="true" rhx-label="Loading" /&gt;
/// </code>
/// </example>
[HtmlTargetElement("rhx-progress-bar")]
public class ProgressBarTagHelper : RazorHXTagHelperBase
{
    /// <inheritdoc/>
    protected override string BlockName => "progress-bar";

    // ──────────────────────────────────────────────
    //  Properties
    // ──────────────────────────────────────────────

    /// <summary>
    /// The current progress value (0–100). Default: 0.
    /// </summary>
    [HtmlAttributeName("rhx-value")]
    public int Value { get; set; }

    /// <summary>
    /// Whether the progress bar is indeterminate (animated, no specific value).
    /// Default: false.
    /// </summary>
    [HtmlAttributeName("rhx-indeterminate")]
    public bool Indeterminate { get; set; }

    /// <summary>
    /// Accessible label for the progress bar (sets aria-label).
    /// </summary>
    [HtmlAttributeName("rhx-label")]
    public string? Label { get; set; }

    // ──────────────────────────────────────────────
    //  Constructor
    // ──────────────────────────────────────────────

    public ProgressBarTagHelper(IUrlHelperFactory urlHelperFactory) : base(urlHelperFactory) { }

    // ──────────────────────────────────────────────
    //  Rendering
    // ──────────────────────────────────────────────

    /// <inheritdoc/>
    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        output.TagName = "div";
        output.TagMode = TagMode.StartTagAndEndTag;

        var css = CreateCssBuilder()
            .AddIf(GetModifierClass("indeterminate"), Indeterminate);

        ApplyBaseAttributes(output, css);

        output.Attributes.SetAttribute("role", "progressbar");
        output.Attributes.SetAttribute("aria-valuemin", "0");
        output.Attributes.SetAttribute("aria-valuemax", "100");

        var clampedValue = Math.Clamp(Value, 0, 100);

        if (!Indeterminate)
            output.Attributes.SetAttribute("aria-valuenow", clampedValue.ToString());

        if (!string.IsNullOrEmpty(Label))
            output.Attributes.SetAttribute("aria-label", Label);

        // ── Build inner content ──
        output.Content.Clear();

        // Track with fill
        output.Content.AppendHtml($"<div class=\"{GetElementClass("track")}\">");
        if (Indeterminate)
        {
            output.Content.AppendHtml($"<div class=\"{GetElementClass("fill")}\"></div>");
        }
        else
        {
            output.Content.AppendHtml($"<div class=\"{GetElementClass("fill")}\" style=\"width: {clampedValue}%\"></div>");
        }
        output.Content.AppendHtml("</div>");

        // Label (visible percentage)
        if (!Indeterminate)
        {
            output.Content.AppendHtml($"<span class=\"{GetElementClass("label")}\">{clampedValue}%</span>");
        }

        // ── htmx attributes ──
        RenderHtmxAttributes(output);
    }
}
