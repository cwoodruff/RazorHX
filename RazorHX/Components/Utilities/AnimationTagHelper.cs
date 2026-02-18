using Microsoft.AspNetCore.Razor.TagHelpers;
using RazorHX.Infrastructure;

namespace RazorHX.Components.Utilities;

/// <summary>
/// Wraps child content and applies CSS animations when triggered.
/// Animations can be triggered on load or via JavaScript/htmx events.
/// </summary>
/// <example>
/// <code>
/// &lt;rhx-animation rhx-name="fadeIn"&gt;Hello&lt;/rhx-animation&gt;
/// &lt;rhx-animation rhx-name="slideInUp" rhx-duration="500" rhx-delay="200"&gt;Content&lt;/rhx-animation&gt;
/// </code>
/// </example>
[HtmlTargetElement("rhx-animation")]
public class AnimationTagHelper : RazorHXTagHelperBase
{
    protected override string BlockName => "animation";

    /// <summary>
    /// The predefined animation name (e.g., fadeIn, slideInLeft, bounceIn, zoomIn).
    /// </summary>
    [HtmlAttributeName("rhx-name")]
    public string Name { get; set; } = "fadeIn";

    /// <summary>
    /// Animation duration in milliseconds. Default: 300.
    /// </summary>
    [HtmlAttributeName("rhx-duration")]
    public int Duration { get; set; } = 300;

    /// <summary>
    /// Animation delay in milliseconds. Default: 0.
    /// </summary>
    [HtmlAttributeName("rhx-delay")]
    public int Delay { get; set; }

    /// <summary>
    /// Animation direction: normal, reverse, alternate, alternate-reverse.
    /// </summary>
    [HtmlAttributeName("rhx-direction")]
    public string? Direction { get; set; }

    /// <summary>
    /// Animation easing: ease, linear, ease-in, ease-out, ease-in-out, or a cubic-bezier value.
    /// </summary>
    [HtmlAttributeName("rhx-easing")]
    public string? Easing { get; set; }

    /// <summary>
    /// Number of iterations, or "infinite". Default: "1".
    /// </summary>
    [HtmlAttributeName("rhx-iterations")]
    public string? Iterations { get; set; }

    /// <summary>
    /// Animation fill mode: none, forwards, backwards, both. Default: both.
    /// </summary>
    [HtmlAttributeName("rhx-fill")]
    public string Fill { get; set; } = "both";

    /// <summary>
    /// Whether the animation should play immediately. Default: true.
    /// </summary>
    [HtmlAttributeName("rhx-play")]
    public bool Play { get; set; } = true;

    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        output.TagName = "div";
        output.TagMode = TagMode.StartTagAndEndTag;

        var css = CreateCssBuilder()
            .AddIf(GetModifierClass("playing"), Play);
        ApplyBaseAttributes(output, css);

        output.Attributes.SetAttribute("data-rhx-animation", Name.ToLowerInvariant());
        output.Attributes.SetAttribute("data-rhx-duration", Duration.ToString());
        output.Attributes.SetAttribute("data-rhx-delay", Delay.ToString());

        if (!string.IsNullOrWhiteSpace(Direction) &&
            !Direction.Equals("normal", StringComparison.OrdinalIgnoreCase))
            output.Attributes.SetAttribute("data-rhx-direction", Direction.ToLowerInvariant());

        if (!string.IsNullOrWhiteSpace(Easing))
            output.Attributes.SetAttribute("data-rhx-easing", Easing);

        if (!string.IsNullOrWhiteSpace(Iterations) &&
            !Iterations.Equals("1", StringComparison.Ordinal))
            output.Attributes.SetAttribute("data-rhx-iterations", Iterations);

        if (!Fill.Equals("both", StringComparison.OrdinalIgnoreCase))
            output.Attributes.SetAttribute("data-rhx-fill", Fill.ToLowerInvariant());

        if (!Play)
            output.Attributes.SetAttribute("data-rhx-paused", "");

        RenderHtmxAttributes(output);
    }
}
