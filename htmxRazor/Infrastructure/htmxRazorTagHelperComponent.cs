using htmxRazor.Configuration;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace htmxRazor.Infrastructure;

/// <summary>
/// Tag helper component that auto-injects htmxRazor stylesheet and script references into the page head.
/// </summary>
public sealed class htmxRazorTagHelperComponent : TagHelperComponent
{
    private readonly htmxRazorOptions _options;

    public htmxRazorTagHelperComponent(htmxRazorOptions options)
    {
        _options = options;
    }

    public override int Order => 1;

    public override Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
    {
        if (!string.Equals(output.TagName, "head", StringComparison.OrdinalIgnoreCase))
        {
            return Task.CompletedTask;
        }

        // Inject htmxRazor stylesheet
        output.PostContent.AppendHtml(
            "\n    <link rel=\"stylesheet\" href=\"/_rhx/css/rhx-tokens.css\">" +
            "\n    <link rel=\"stylesheet\" href=\"/_rhx/css/rhx-reset.css\">" +
            "\n    <link rel=\"stylesheet\" href=\"/_rhx/css/rhx-core.css\">" +
            "\n    <link rel=\"stylesheet\" href=\"/_rhx/css/rhx-utilities.css\">");

        // Inject theme stylesheet
        var theme = _options.DefaultTheme.ToLowerInvariant();
        output.PostContent.AppendHtml(
            $"\n    <link rel=\"stylesheet\" href=\"/_rhx/css/themes/rhx-{theme}.css\">");

        // Inject htmx script if configured
        if (_options.IncludeHtmxScript)
        {
            var htmxUrl = string.IsNullOrWhiteSpace(_options.CdnBaseUrl)
                ? "https://unpkg.com/htmx.org@2.0.4"
                : $"{_options.CdnBaseUrl.TrimEnd('/')}/htmx.org@2.0.4";

            output.PostContent.AppendHtml(
                $"\n    <script src=\"{htmxUrl}\"></script>");
        }

        // Inject htmxRazor core script
        output.PostContent.AppendHtml(
            "\n    <script src=\"/_rhx/js/rhx-core.js\" defer></script>\n");

        return Task.CompletedTask;
    }
}
