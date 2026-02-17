using System.Net;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Razor.TagHelpers;
using RazorHX.Infrastructure;

namespace RazorHX.Components.Imagery;

/// <summary>
/// Renders an image or iframe with pinch-to-zoom and scroll-to-zoom capability.
/// Supports configurable initial scale, min/max zoom bounds, and reset on double-click.
/// </summary>
/// <example>
/// <code>
/// &lt;rhx-zoomable-frame rhx-src="/diagram.png" rhx-min-scale="0.5" rhx-max-scale="5" /&gt;
/// </code>
/// </example>
[HtmlTargetElement("rhx-zoomable-frame")]
public class ZoomableFrameTagHelper : RazorHXTagHelperBase
{
    /// <inheritdoc/>
    protected override string BlockName => "zoomable-frame";

    /// <summary>
    /// Source URL of the content (image or iframe src).
    /// </summary>
    [HtmlAttributeName("rhx-src")]
    public string Src { get; set; } = "";

    /// <summary>
    /// Alt text for the image. When set, renders an <c>&lt;img&gt;</c>; otherwise an <c>&lt;iframe&gt;</c>.
    /// </summary>
    [HtmlAttributeName("rhx-alt")]
    public string? Alt { get; set; }

    /// <summary>
    /// Initial zoom scale. Default: 1.
    /// </summary>
    [HtmlAttributeName("rhx-scale")]
    public double Scale { get; set; } = 1;

    /// <summary>
    /// Minimum zoom scale. Default: 0.5.
    /// </summary>
    [HtmlAttributeName("rhx-min-scale")]
    public double MinScale { get; set; } = 0.5;

    /// <summary>
    /// Maximum zoom scale. Default: 5.
    /// </summary>
    [HtmlAttributeName("rhx-max-scale")]
    public double MaxScale { get; set; } = 5;

    /// <summary>
    /// Creates a new ZoomableFrameTagHelper with URL generation support.
    /// </summary>
    public ZoomableFrameTagHelper(IUrlHelperFactory urlHelperFactory) : base(urlHelperFactory) { }

    /// <inheritdoc/>
    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        output.TagName = "div";
        output.TagMode = TagMode.StartTagAndEndTag;

        var css = CreateCssBuilder();
        ApplyBaseAttributes(output, css);

        output.Attributes.SetAttribute("data-rhx-zoomable-frame", "");
        output.Attributes.SetAttribute("data-rhx-scale", Scale.ToString("F2"));
        output.Attributes.SetAttribute("data-rhx-min-scale", MinScale.ToString("F2"));
        output.Attributes.SetAttribute("data-rhx-max-scale", MaxScale.ToString("F2"));
        output.Attributes.SetAttribute("tabindex", "0");
        output.Attributes.SetAttribute("role", "application");
        output.Attributes.SetAttribute("aria-label", "Zoomable content. Use scroll wheel or pinch to zoom.");

        RenderHtmxAttributes(output);

        output.Content.Clear();

        var contentClass = GetElementClass("content");
        var transform = Scale != 1 ? $" style=\"transform: scale({Scale.ToString("F2")})\"" : "";

        if (Alt is not null)
        {
            output.Content.AppendHtml(
                $"<div class=\"{contentClass}\"{transform}>" +
                $"<img src=\"{Enc(Src)}\" alt=\"{Enc(Alt)}\" draggable=\"false\" />" +
                "</div>");
        }
        else
        {
            output.Content.AppendHtml(
                $"<div class=\"{contentClass}\"{transform}>" +
                $"<iframe src=\"{Enc(Src)}\" frameborder=\"0\"></iframe>" +
                "</div>");
        }
    }

    private static string Enc(string? value) => WebUtility.HtmlEncode(value ?? "") ?? "";
}
