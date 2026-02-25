using System.Net;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace htmxRazor.Components.Forms;

/// <summary>
/// Renders all ModelState validation errors as a callout with an error list.
/// When no errors exist, the output is suppressed entirely.
/// Reuses the existing callout CSS classes for visual consistency.
/// </summary>
/// <example>
/// <code>
/// &lt;rhx-validation-summary /&gt;
/// &lt;rhx-validation-summary rhx-variant="warning" /&gt;
/// </code>
/// </example>
[HtmlTargetElement("rhx-validation-summary")]
public class ValidationSummaryTagHelper : TagHelper
{
    /// <summary>
    /// The callout variant used for styling. Default: "danger".
    /// Options: neutral, brand, success, warning, danger.
    /// </summary>
    [HtmlAttributeName("rhx-variant")]
    public string Variant { get; set; } = "danger";

    /// <summary>
    /// Additional CSS classes.
    /// </summary>
    [HtmlAttributeName("class")]
    public string? CssClass { get; set; }

    /// <summary>
    /// The current ViewContext (auto-injected).
    /// </summary>
    [HtmlAttributeNotBound]
    [ViewContext]
    public ViewContext ViewContext { get; set; } = default!;

    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        // Collect all errors from ModelState
        var errors = new List<string>();
        foreach (var kvp in ViewContext.ModelState)
        {
            foreach (var error in kvp.Value.Errors)
            {
                if (!string.IsNullOrEmpty(error.ErrorMessage))
                    errors.Add(error.ErrorMessage);
            }
        }

        if (errors.Count == 0)
        {
            output.SuppressOutput();
            return;
        }

        output.TagName = "div";
        output.TagMode = TagMode.StartTagAndEndTag;

        var variant = Variant.ToLowerInvariant();
        var classes = $"rhx-callout rhx-callout--{variant}";
        if (!string.IsNullOrWhiteSpace(CssClass))
            classes = $"{classes} {CssClass}";

        output.Attributes.SetAttribute("class", classes);
        output.Attributes.SetAttribute("role", "alert");

        output.Content.Clear();

        // Icon
        var iconSvg = GetIconSvg(variant);
        output.Content.AppendHtml(
            $"<span class=\"rhx-callout__icon\" aria-hidden=\"true\">{iconSvg}</span>");

        // Content with error list
        output.Content.AppendHtml("<div class=\"rhx-callout__content\">");
        output.Content.AppendHtml("<ul class=\"rhx-validation-summary__list\">");

        foreach (var error in errors)
        {
            output.Content.AppendHtml($"<li>{Enc(error)}</li>");
        }

        output.Content.AppendHtml("</ul>");
        output.Content.AppendHtml("</div>");
    }

    private static string GetIconSvg(string variant) => variant switch
    {
        "success" => CheckCircleSvg,
        "warning" => ExclamationTriangleSvg,
        "danger" => ExclamationCircleSvg,
        _ => InfoCircleSvg
    };

    private static string Enc(string? value) => WebUtility.HtmlEncode(value ?? "") ?? "";

    private const string InfoCircleSvg =
        "<svg xmlns=\"http://www.w3.org/2000/svg\" width=\"20\" height=\"20\" viewBox=\"0 0 24 24\" fill=\"none\" stroke=\"currentColor\" stroke-width=\"2\" stroke-linecap=\"round\" stroke-linejoin=\"round\">" +
        "<circle cx=\"12\" cy=\"12\" r=\"10\"></circle><line x1=\"12\" y1=\"16\" x2=\"12\" y2=\"12\"></line><line x1=\"12\" y1=\"8\" x2=\"12.01\" y2=\"8\"></line>" +
        "</svg>";

    private const string CheckCircleSvg =
        "<svg xmlns=\"http://www.w3.org/2000/svg\" width=\"20\" height=\"20\" viewBox=\"0 0 24 24\" fill=\"none\" stroke=\"currentColor\" stroke-width=\"2\" stroke-linecap=\"round\" stroke-linejoin=\"round\">" +
        "<path d=\"M22 11.08V12a10 10 0 1 1-5.93-9.14\"></path><polyline points=\"22 4 12 14.01 9 11.01\"></polyline>" +
        "</svg>";

    private const string ExclamationTriangleSvg =
        "<svg xmlns=\"http://www.w3.org/2000/svg\" width=\"20\" height=\"20\" viewBox=\"0 0 24 24\" fill=\"none\" stroke=\"currentColor\" stroke-width=\"2\" stroke-linecap=\"round\" stroke-linejoin=\"round\">" +
        "<path d=\"M10.29 3.86L1.82 18a2 2 0 0 0 1.71 3h16.94a2 2 0 0 0 1.71-3L13.71 3.86a2 2 0 0 0-3.42 0z\"></path><line x1=\"12\" y1=\"9\" x2=\"12\" y2=\"13\"></line><line x1=\"12\" y1=\"17\" x2=\"12.01\" y2=\"17\"></line>" +
        "</svg>";

    private const string ExclamationCircleSvg =
        "<svg xmlns=\"http://www.w3.org/2000/svg\" width=\"20\" height=\"20\" viewBox=\"0 0 24 24\" fill=\"none\" stroke=\"currentColor\" stroke-width=\"2\" stroke-linecap=\"round\" stroke-linejoin=\"round\">" +
        "<circle cx=\"12\" cy=\"12\" r=\"10\"></circle><line x1=\"12\" y1=\"8\" x2=\"12\" y2=\"12\"></line><line x1=\"12\" y1=\"16\" x2=\"12.01\" y2=\"16\"></line>" +
        "</svg>";
}
