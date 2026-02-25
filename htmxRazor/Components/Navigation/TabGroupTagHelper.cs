using System.Net;
using htmxRazor.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace htmxRazor.Components.Navigation;

/// <summary>
/// Renders a tabbed interface container. Contains <c>&lt;rhx-tab&gt;</c> children
/// (which register themselves into the tab nav) and <c>&lt;rhx-tab-panel&gt;</c>
/// children (which render into the body area).
/// </summary>
/// <remarks>
/// <para>
/// JavaScript (<c>rhx-tabs.js</c>) handles click activation, keyboard navigation
/// (arrow keys, Home/End), ARIA management, and auto/manual activation modes.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// &lt;rhx-tab-group aria-label="Settings"&gt;
///     &lt;rhx-tab rhx-panel="general" rhx-active&gt;General&lt;/rhx-tab&gt;
///     &lt;rhx-tab rhx-panel="advanced"&gt;Advanced&lt;/rhx-tab&gt;
///     &lt;rhx-tab-panel rhx-name="general" rhx-active&gt;General content&lt;/rhx-tab-panel&gt;
///     &lt;rhx-tab-panel rhx-name="advanced"&gt;Advanced content&lt;/rhx-tab-panel&gt;
/// &lt;/rhx-tab-group&gt;
/// </code>
/// </example>
[HtmlTargetElement("rhx-tab-group")]
public class TabGroupTagHelper : htmxRazorTagHelperBase
{
    /// <inheritdoc/>
    protected override string BlockName => "tab-group";

    /// <summary>
    /// The placement of the tab nav relative to the panel body.
    /// Options: top (default), bottom, start, end.
    /// </summary>
    [HtmlAttributeName("rhx-placement")]
    public string Placement { get; set; } = "top";

    /// <summary>
    /// The activation mode for tabs.
    /// "auto" (default): tabs activate on focus (arrow key navigation selects).
    /// "manual": tabs activate only on Enter/Space (arrow keys move focus only).
    /// </summary>
    [HtmlAttributeName("rhx-activation")]
    public string Activation { get; set; } = "auto";

    /// <summary>
    /// Accessible label for the tablist navigation region.
    /// </summary>
    [HtmlAttributeName("aria-label")]
    public string? AriaLabel { get; set; }

    /// <summary>
    /// Creates a new TabGroupTagHelper with URL generation support.
    /// </summary>
    public TabGroupTagHelper(IUrlHelperFactory urlHelperFactory) : base(urlHelperFactory) { }

    /// <inheritdoc/>
    public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
    {
        // Register nav list for child tabs to populate
        var tabs = new List<string>();
        context.Items["RhxTabsNav"] = tabs;
        context.Items[typeof(TabGroupTagHelper)] = this;

        // Process children — tabs register their HTML, panels render normally
        var childContent = await output.GetChildContentAsync();

        // Render outer container
        output.TagName = "div";
        output.TagMode = TagMode.StartTagAndEndTag;

        var placement = Placement.ToLowerInvariant();
        var activation = Activation.ToLowerInvariant();

        var css = CreateCssBuilder()
            .AddIf(GetModifierClass(placement), placement != "top");
        ApplyBaseAttributes(output, css);

        output.Attributes.SetAttribute("data-rhx-tabs", "");
        output.Attributes.SetAttribute("data-rhx-placement", placement);

        if (activation != "auto")
        {
            output.Attributes.SetAttribute("data-rhx-activation", activation);
        }

        RenderHtmxAttributes(output);

        // Assemble inner content
        output.Content.Clear();

        // Nav (tablist)
        var ariaLabelAttr = !string.IsNullOrWhiteSpace(AriaLabel)
            ? $" aria-label=\"{WebUtility.HtmlEncode(AriaLabel)}\""
            : "";

        var isVertical = placement == "start" || placement == "end";
        var orientationAttr = isVertical ? " aria-orientation=\"vertical\"" : "";

        output.Content.AppendHtml(
            $"<div class=\"{GetElementClass("nav")}\" role=\"tablist\"{ariaLabelAttr}{orientationAttr}>");

        foreach (var tab in tabs)
        {
            output.Content.AppendHtml(tab);
        }

        output.Content.AppendHtml("</div>");

        // Body
        output.Content.AppendHtml($"<div class=\"{GetElementClass("body")}\">");
        output.Content.AppendHtml(childContent);
        output.Content.AppendHtml("</div>");
    }
}
