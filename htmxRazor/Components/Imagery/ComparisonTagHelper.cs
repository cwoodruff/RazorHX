using System.Net;
using htmxRazor.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace htmxRazor.Components.Imagery;

/// <summary>
/// Renders a before/after image comparison slider with a draggable handle.
/// Supports mouse, touch, and keyboard interaction.
/// </summary>
/// <example>
/// <code>
/// &lt;rhx-comparison rhx-before="before.jpg" rhx-before-alt="Before"
///                  rhx-after="after.jpg" rhx-after-alt="After"
///                  rhx-position="50" /&gt;
/// </code>
/// </example>
[HtmlTargetElement("rhx-comparison")]
public class ComparisonTagHelper : htmxRazorTagHelperBase
{
    /// <inheritdoc/>
    protected override string BlockName => "comparison";

    /// <summary>
    /// Source URL of the "before" image.
    /// </summary>
    [HtmlAttributeName("rhx-before")]
    public string Before { get; set; } = "";

    /// <summary>
    /// Alt text for the "before" image.
    /// </summary>
    [HtmlAttributeName("rhx-before-alt")]
    public string BeforeAlt { get; set; } = "Before";

    /// <summary>
    /// Source URL of the "after" image.
    /// </summary>
    [HtmlAttributeName("rhx-after")]
    public string After { get; set; } = "";

    /// <summary>
    /// Alt text for the "after" image.
    /// </summary>
    [HtmlAttributeName("rhx-after-alt")]
    public string AfterAlt { get; set; } = "After";

    /// <summary>
    /// Initial slider position as a percentage (0–100). Default: 50.
    /// </summary>
    [HtmlAttributeName("rhx-position")]
    public int Position { get; set; } = 50;

    /// <summary>
    /// Creates a new ComparisonTagHelper with URL generation support.
    /// </summary>
    public ComparisonTagHelper(IUrlHelperFactory urlHelperFactory) : base(urlHelperFactory) { }

    /// <inheritdoc/>
    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        output.TagName = "div";
        output.TagMode = TagMode.StartTagAndEndTag;

        var pos = Math.Clamp(Position, 0, 100);

        var css = CreateCssBuilder();
        ApplyBaseAttributes(output, css);

        output.Attributes.SetAttribute("data-rhx-comparison", "");

        RenderHtmxAttributes(output);

        output.Content.Clear();

        // Before (full image, bottom layer)
        output.Content.AppendHtml(
            $"<div class=\"{GetElementClass("before")}\">" +
            $"<img src=\"{Enc(Before)}\" alt=\"{Enc(BeforeAlt)}\" />" +
            "</div>");

        // After (clipped image, top layer)
        output.Content.AppendHtml(
            $"<div class=\"{GetElementClass("after")}\" style=\"clip-path: inset(0 {100 - pos}% 0 0)\">" +
            $"<img src=\"{Enc(After)}\" alt=\"{Enc(AfterAlt)}\" />" +
            "</div>");

        // Handle
        output.Content.AppendHtml(
            $"<div class=\"{GetElementClass("handle")}\" role=\"slider\" " +
            $"aria-valuenow=\"{pos}\" aria-valuemin=\"0\" aria-valuemax=\"100\" " +
            $"aria-label=\"Comparison slider\" tabindex=\"0\" " +
            $"style=\"left: {pos}%\">" +
            $"<div class=\"{GetElementClass("handle-line")}\"></div>" +
            $"<div class=\"{GetElementClass("handle-grip")}\">" +
            "<svg viewBox=\"0 0 24 24\" fill=\"none\" stroke=\"currentColor\" stroke-width=\"2\" aria-hidden=\"true\">" +
            "<path d=\"M8 18l-6-6 6-6\" /><path d=\"M16 6l6 6-6 6\" />" +
            "</svg></div>" +
            $"<div class=\"{GetElementClass("handle-line")}\"></div>" +
            "</div>");
    }

    private static string Enc(string? value) => WebUtility.HtmlEncode(value ?? "") ?? "";
}
