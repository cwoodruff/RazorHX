using System.Net;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace RazorHX.Components.Feedback;

/// <summary>
/// Wraps child content in a span with tooltip data attributes.
/// The tooltip popup is created and positioned by JavaScript on the configured trigger events.
/// </summary>
/// <example>
/// <code>
/// &lt;rhx-tooltip rhx-content="Save your changes"&gt;
///     &lt;rhx-button rhx-variant="brand"&gt;Save&lt;/rhx-button&gt;
/// &lt;/rhx-tooltip&gt;
///
/// &lt;rhx-tooltip rhx-content="Click to copy" rhx-placement="bottom" rhx-trigger="hover"&gt;
///     &lt;rhx-button&gt;Copy&lt;/rhx-button&gt;
/// &lt;/rhx-tooltip&gt;
/// </code>
/// </example>
[HtmlTargetElement("rhx-tooltip")]
public class TooltipTagHelper : TagHelper
{
    // ──────────────────────────────────────────────
    //  Properties
    // ──────────────────────────────────────────────

    /// <summary>
    /// The tooltip text content.
    /// </summary>
    [HtmlAttributeName("rhx-content")]
    public string Content { get; set; } = "";

    /// <summary>
    /// The placement of the tooltip relative to the trigger element.
    /// Options: top, bottom, left, right. Default: top.
    /// </summary>
    [HtmlAttributeName("rhx-placement")]
    public string Placement { get; set; } = "top";

    /// <summary>
    /// Whether the tooltip is disabled. Default: false.
    /// </summary>
    [HtmlAttributeName("rhx-disabled")]
    public bool Disabled { get; set; }

    /// <summary>
    /// Space-separated trigger events. Options: hover, click, focus.
    /// Default: "hover focus".
    /// </summary>
    [HtmlAttributeName("rhx-trigger")]
    public string Trigger { get; set; } = "hover focus";

    // ──────────────────────────────────────────────
    //  Rendering
    // ──────────────────────────────────────────────

    /// <inheritdoc/>
    public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
    {
        output.TagName = "span";
        output.TagMode = TagMode.StartTagAndEndTag;

        output.Attributes.SetAttribute("data-rhx-tooltip", WebUtility.HtmlEncode(Content));
        output.Attributes.SetAttribute("data-rhx-tooltip-placement", Placement.ToLowerInvariant());

        if (Disabled)
            output.Attributes.SetAttribute("data-rhx-tooltip-disabled", "");

        var trigger = Trigger.ToLowerInvariant();
        if (trigger != "hover focus")
            output.Attributes.SetAttribute("data-rhx-tooltip-trigger", trigger);

        var childContent = await output.GetChildContentAsync();
        output.Content.SetHtmlContent(childContent);
    }
}
