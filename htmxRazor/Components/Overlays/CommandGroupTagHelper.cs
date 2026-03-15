using System.Net;
using htmxRazor.Infrastructure;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace htmxRazor.Components.Overlays;

/// <summary>
/// Groups command palette results under a heading within <c>&lt;rhx-command-palette&gt;</c>.
/// </summary>
/// <example>
/// <code>
/// &lt;rhx-command-group rhx-heading="Pages"&gt;
///     &lt;rhx-command-item rhx-value="home" rhx-href="/"&gt;Home&lt;/rhx-command-item&gt;
///     &lt;rhx-command-item rhx-value="about" rhx-href="/about"&gt;About&lt;/rhx-command-item&gt;
/// &lt;/rhx-command-group&gt;
/// </code>
/// </example>
[HtmlTargetElement("rhx-command-group")]
public class CommandGroupTagHelper : TagHelper
{
    /// <summary>Group heading text.</summary>
    [HtmlAttributeName("rhx-heading")]
    public string Heading { get; set; } = "";

    /// <inheritdoc/>
    public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
    {
        var childContent = await output.GetChildContentAsync();

        output.TagName = "div";
        output.TagMode = TagMode.StartTagAndEndTag;

        var headingId = $"rhx-cg-{context.UniqueId}-heading";

        output.Attributes.SetAttribute("class", "rhx-command-palette__group");
        output.Attributes.SetAttribute("role", "group");
        output.Attributes.SetAttribute("aria-labelledby", headingId);

        output.Content.Clear();
        output.Content.AppendHtml(
            $"<div class=\"rhx-command-palette__group-heading\" id=\"{Enc(headingId)}\" role=\"presentation\">" +
            $"{Enc(Heading)}</div>");
        output.Content.AppendHtml(childContent);
    }

    private static string Enc(string? value) => WebUtility.HtmlEncode(value ?? "") ?? "";
}
