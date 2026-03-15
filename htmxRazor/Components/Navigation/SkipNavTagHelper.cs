using System.Net;
using htmxRazor.Infrastructure;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace htmxRazor.Components.Navigation;

/// <summary>
/// Renders a skip navigation link that is visually hidden until focused.
/// Allows keyboard users to bypass navigation and jump to main content,
/// addressing WCAG 2.4.1 Bypass Blocks.
/// </summary>
/// <example>
/// <code>
/// &lt;rhx-skip-nav /&gt;
///
/// &lt;rhx-skip-nav rhx-target="#content" rhx-label="Skip to content" /&gt;
/// </code>
/// </example>
[HtmlTargetElement("rhx-skip-nav")]
public class SkipNavTagHelper : htmxRazorTagHelperBase
{
    /// <inheritdoc/>
    protected override string BlockName => "skip-nav";

    /// <summary>
    /// CSS selector (href fragment) for the skip target element. Default: "#main-content".
    /// </summary>
    [HtmlAttributeName("rhx-target")]
    public string Target { get; set; } = "#main-content";

    /// <summary>
    /// The visible link text. Default: "Skip to main content".
    /// </summary>
    [HtmlAttributeName("rhx-label")]
    public string Label { get; set; } = "Skip to main content";

    /// <inheritdoc/>
    public override Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
    {
        output.TagName = "a";
        output.TagMode = TagMode.StartTagAndEndTag;

        var css = CreateCssBuilder();
        ApplyBaseAttributes(output, css);

        output.Attributes.SetAttribute("href", Target);

        output.Content.SetContent(Label);

        return Task.CompletedTask;
    }
}
