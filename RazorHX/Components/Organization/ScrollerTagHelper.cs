using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Razor.TagHelpers;
using RazorHX.Infrastructure;

namespace RazorHX.Components.Organization;

/// <summary>
/// Renders a scroll container with gradient shadow indicators that appear when
/// content overflows, signaling to the user that more content is available
/// in the scroll direction.
/// </summary>
/// <remarks>
/// <para>
/// The component wraps child content in a scrollable <c>&lt;div&gt;</c> and adds
/// start/end shadow overlays. JavaScript observes the scroll position to toggle
/// shadow visibility classes automatically.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// &lt;rhx-scroller rhx-orientation="horizontal"&gt;
///     &lt;div style="display: flex; gap: 1rem;"&gt;
///         &lt;div&gt;Item 1&lt;/div&gt;
///         &lt;div&gt;Item 2&lt;/div&gt;
///         ...
///     &lt;/div&gt;
/// &lt;/rhx-scroller&gt;
/// </code>
/// </example>
[HtmlTargetElement("rhx-scroller")]
public class ScrollerTagHelper : RazorHXTagHelperBase
{
    /// <inheritdoc/>
    protected override string BlockName => "scroller";

    /// <summary>
    /// The scroll direction: horizontal, vertical, or both. Default: horizontal.
    /// </summary>
    [HtmlAttributeName("rhx-orientation")]
    public string Orientation { get; set; } = "horizontal";

    /// <summary>
    /// Creates a new ScrollerTagHelper with URL generation support.
    /// </summary>
    public ScrollerTagHelper(IUrlHelperFactory urlHelperFactory) : base(urlHelperFactory) { }

    /// <inheritdoc/>
    public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
    {
        var childContent = await output.GetChildContentAsync();

        output.TagName = "div";
        output.TagMode = TagMode.StartTagAndEndTag;

        var orientation = Orientation.ToLowerInvariant();
        var css = CreateCssBuilder()
            .Add(GetModifierClass(orientation));
        ApplyBaseAttributes(output, css);

        output.Attributes.SetAttribute("data-rhx-scroller", "");
        output.Attributes.SetAttribute("data-rhx-orientation", orientation);

        RenderHtmxAttributes(output);

        // Assemble inner HTML
        output.Content.Clear();

        // Scrollable content wrapper
        output.Content.AppendHtml($"<div class=\"{GetElementClass("content")}\">");
        output.Content.AppendHtml(childContent);
        output.Content.AppendHtml("</div>");

        // Shadow indicators
        var shadowClass = GetElementClass("shadow");
        output.Content.AppendHtml(
            $"<div class=\"{shadowClass} {shadowClass}--start\" aria-hidden=\"true\"></div>");
        output.Content.AppendHtml(
            $"<div class=\"{shadowClass} {shadowClass}--end\" aria-hidden=\"true\"></div>");
    }
}
