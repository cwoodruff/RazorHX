using htmxRazor.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace htmxRazor.Components.Navigation;

/// <summary>
/// Renders a semantic HTML landmark element based on the specified role.
/// Maps landmark roles to their native HTML elements (e.g., "navigation" → &lt;nav&gt;,
/// "main" → &lt;main&gt;). Addresses WCAG 2.4.1 Bypass Blocks by providing
/// programmatically determinable page regions.
/// </summary>
/// <example>
/// <code>
/// &lt;rhx-landmark rhx-role="navigation" rhx-label="Primary navigation"&gt;
///     &lt;!-- nav links --&gt;
/// &lt;/rhx-landmark&gt;
///
/// &lt;rhx-landmark rhx-role="main"&gt;
///     &lt;!-- page content --&gt;
/// &lt;/rhx-landmark&gt;
///
/// &lt;rhx-landmark rhx-role="region" rhx-label="Filters"&gt;
///     &lt;!-- filter controls --&gt;
/// &lt;/rhx-landmark&gt;
/// </code>
/// </example>
[HtmlTargetElement("rhx-landmark")]
public class LandmarkTagHelper : htmxRazorTagHelperBase
{
    /// <inheritdoc/>
    protected override string BlockName => "landmark";

    /// <summary>
    /// The landmark role. Determines which HTML element is rendered.
    /// Options: banner, navigation, main, complementary, contentinfo, region, search, form.
    /// Default: region.
    /// </summary>
    [HtmlAttributeName("rhx-role")]
    public string Role { get; set; } = "region";

    /// <summary>
    /// Accessible label for the landmark region. Required for "region" role
    /// to be exposed as a landmark by assistive technologies.
    /// </summary>
    [HtmlAttributeName("rhx-label")]
    public string? Label { get; set; }

    /// <summary>
    /// Creates a new LandmarkTagHelper with URL generation support.
    /// </summary>
    public LandmarkTagHelper(IUrlHelperFactory urlHelperFactory) : base(urlHelperFactory) { }

    /// <inheritdoc/>
    public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
    {
        var role = Role.ToLowerInvariant();
        output.TagName = MapRoleToElement(role);
        output.TagMode = TagMode.StartTagAndEndTag;

        var css = CreateCssBuilder();
        ApplyBaseAttributes(output, css);

        if (!string.IsNullOrWhiteSpace(Label))
        {
            output.Attributes.SetAttribute("aria-label", Label);
        }

        // For elements that don't have implicit landmark roles, add explicit role
        if (role == "search" && output.TagName == "div")
        {
            output.Attributes.SetAttribute("role", "search");
        }

        RenderHtmxAttributes(output);

        var childContent = await output.GetChildContentAsync();
        output.Content.SetHtmlContent(childContent);
    }

    private static string MapRoleToElement(string role) => role switch
    {
        "banner" => "header",
        "navigation" => "nav",
        "main" => "main",
        "complementary" => "aside",
        "contentinfo" => "footer",
        "region" => "section",
        "search" => "search",
        "form" => "form",
        _ => "section"
    };
}
