using System.Net;
using htmxRazor.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace htmxRazor.Components.Imagery;

/// <summary>
/// Renders a GIF or APNG with a play/pause control button.
/// JavaScript captures a canvas frame on pause and restores the original source on play.
/// </summary>
/// <example>
/// <code>
/// &lt;rhx-animated-image rhx-src="animation.gif" rhx-alt="Loading spinner" /&gt;
/// </code>
/// </example>
[HtmlTargetElement("rhx-animated-image")]
public class AnimatedImageTagHelper : htmxRazorTagHelperBase
{
    /// <inheritdoc/>
    protected override string BlockName => "animated-image";

    /// <summary>
    /// The source URL of the animated image (GIF, APNG, WebP).
    /// </summary>
    [HtmlAttributeName("rhx-src")]
    public string Src { get; set; } = "";

    /// <summary>
    /// Alt text for the image.
    /// </summary>
    [HtmlAttributeName("rhx-alt")]
    public string Alt { get; set; } = "";

    /// <summary>
    /// Whether the animation is initially playing. Default: true.
    /// </summary>
    [HtmlAttributeName("rhx-play")]
    public bool Play { get; set; } = true;

    /// <summary>
    /// Creates a new AnimatedImageTagHelper with URL generation support.
    /// </summary>
    public AnimatedImageTagHelper(IUrlHelperFactory urlHelperFactory) : base(urlHelperFactory) { }

    /// <inheritdoc/>
    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        output.TagName = "div";
        output.TagMode = TagMode.StartTagAndEndTag;

        var css = CreateCssBuilder();
        if (!Play)
            css.Add(GetModifierClass("paused"));
        ApplyBaseAttributes(output, css);

        output.Attributes.SetAttribute("data-rhx-animated-image", "");
        if (!Play)
            output.Attributes.SetAttribute("data-rhx-paused", "");

        RenderHtmxAttributes(output);

        output.Content.Clear();
        output.Content.AppendHtml(
            $"<img class=\"{GetElementClass("img")}\" src=\"{Enc(Src)}\" alt=\"{Enc(Alt)}\" />");
        output.Content.AppendHtml(
            $"<canvas class=\"{GetElementClass("canvas")}\" aria-hidden=\"true\"></canvas>");

        var label = Play ? "Pause animation" : "Play animation";
        var iconName = Play ? "pause" : "play";
        var iconSvg = IconRegistry.Get(iconName) ?? "";
        output.Content.AppendHtml(
            $"<button class=\"{GetElementClass("control")}\" type=\"button\" aria-label=\"{Enc(label)}\">" +
            $"<svg class=\"rhx-icon rhx-icon--small\" viewBox=\"0 0 24 24\" fill=\"none\" stroke=\"currentColor\" stroke-width=\"2\" stroke-linecap=\"round\" stroke-linejoin=\"round\" aria-hidden=\"true\">{iconSvg}</svg>" +
            "</button>");
    }

    private static string Enc(string? value) => WebUtility.HtmlEncode(value ?? "") ?? "";
}
