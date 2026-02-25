using System.Net;
using htmxRazor.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace htmxRazor.Components.Overlays;

/// <summary>
/// Renders a collapsible disclosure widget using the native HTML
/// <c>&lt;details&gt;</c> element. Supports lazy loading via htmx:
/// set <c>hx-trigger="toggle once"</c> to load content on first expand.
/// </summary>
/// <example>
/// <code>
/// &lt;rhx-details rhx-summary="Order History"
///              hx-get="/api/orders" hx-trigger="toggle once"
///              hx-target="find .rhx-details__content"&gt;
///     &lt;rhx-spinner /&gt;
/// &lt;/rhx-details&gt;
/// </code>
/// </example>
[HtmlTargetElement("rhx-details")]
public class DetailsTagHelper : htmxRazorTagHelperBase
{
    /// <inheritdoc/>
    protected override string BlockName => "details";

    /// <summary>
    /// The summary text displayed as the disclosure header.
    /// </summary>
    [HtmlAttributeName("rhx-summary")]
    public string Summary { get; set; } = "Details";

    /// <summary>
    /// Whether the details element is initially open.
    /// </summary>
    [HtmlAttributeName("rhx-open")]
    public bool Open { get; set; }

    /// <summary>
    /// Whether the details element is disabled. Prevents toggling.
    /// </summary>
    [HtmlAttributeName("rhx-disabled")]
    public bool Disabled { get; set; }

    /// <summary>
    /// Creates a new DetailsTagHelper with URL generation support.
    /// </summary>
    public DetailsTagHelper(IUrlHelperFactory urlHelperFactory) : base(urlHelperFactory) { }

    /// <inheritdoc/>
    public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
    {
        var childContent = await output.GetChildContentAsync();

        output.TagName = "details";
        output.TagMode = TagMode.StartTagAndEndTag;

        var css = CreateCssBuilder()
            .AddIf(GetModifierClass("disabled"), Disabled);
        ApplyBaseAttributes(output, css);

        output.Attributes.SetAttribute("data-rhx-details", "");

        if (Open)
            output.Attributes.SetAttribute("open", "open");

        if (Disabled)
            output.Attributes.SetAttribute("data-rhx-disabled", "");

        RenderHtmxAttributes(output);

        // Assemble inner HTML
        output.Content.Clear();

        // Summary
        output.Content.AppendHtml($"<summary class=\"{GetElementClass("summary")}\">");
        output.Content.AppendHtml($"<span class=\"{GetElementClass("summary-icon")}\" aria-hidden=\"true\"></span>");
        output.Content.AppendHtml($"<span class=\"{GetElementClass("summary-text")}\">{Enc(Summary)}</span>");
        output.Content.AppendHtml("</summary>");

        // Content
        output.Content.AppendHtml($"<div class=\"{GetElementClass("content")}\">");
        output.Content.AppendHtml(childContent);
        output.Content.AppendHtml("</div>");
    }

    private static string Enc(string? value) => WebUtility.HtmlEncode(value ?? "") ?? "";
}
