using htmxRazor.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace htmxRazor.Components.Organization;

/// <summary>
/// Container for a vertical or horizontal timeline. Renders a <c>&lt;div&gt;</c>
/// with <c>role="list"</c> that holds <see cref="TimelineItemTagHelper"/> children.
/// </summary>
/// <example>
/// <code>
/// &lt;rhx-timeline rhx-layout="vertical" rhx-align="start"&gt;
///     &lt;rhx-timeline-item rhx-variant="success" rhx-label="March 15, 2026"&gt;
///         Order shipped
///     &lt;/rhx-timeline-item&gt;
/// &lt;/rhx-timeline&gt;
/// </code>
/// </example>
[HtmlTargetElement("rhx-timeline")]
public class TimelineTagHelper : htmxRazorTagHelperBase
{
    /// <inheritdoc/>
    protected override string BlockName => "timeline";

    /// <summary>
    /// The timeline layout direction. Default: "vertical".
    /// Accepted values: <c>vertical</c>, <c>horizontal</c>.
    /// </summary>
    [HtmlAttributeName("rhx-layout")]
    public string Layout { get; set; } = "vertical";

    /// <summary>
    /// The alignment of timeline items. Default: "start".
    /// Accepted values: <c>start</c>, <c>center</c>, <c>alternate</c>.
    /// </summary>
    [HtmlAttributeName("rhx-align")]
    public string Align { get; set; } = "start";

    /// <summary>
    /// Creates a new <see cref="TimelineTagHelper"/> instance.
    /// </summary>
    public TimelineTagHelper(IUrlHelperFactory urlHelperFactory) : base(urlHelperFactory) { }

    /// <inheritdoc/>
    public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
    {
        output.TagName = "div";
        output.TagMode = TagMode.StartTagAndEndTag;

        var css = CreateCssBuilder()
            .Add(GetModifierClass(Layout))
            .AddIf(GetModifierClass(Align), Align != "start");

        ApplyBaseAttributes(output, css);

        output.Attributes.SetAttribute("role", "list");

        RenderHtmxAttributes(output);

        var childContent = await output.GetChildContentAsync();
        output.Content.SetHtmlContent(childContent);
    }
}
