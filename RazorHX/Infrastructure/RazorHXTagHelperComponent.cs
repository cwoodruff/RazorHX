using Microsoft.AspNetCore.Razor.TagHelpers;
using RazorHX.Configuration;

namespace RazorHX.Infrastructure;

/// <summary>
/// Tag helper component that auto-injects RazorHX stylesheet and script references into the page head.
/// </summary>
public sealed class RazorHXTagHelperComponent : TagHelperComponent
{
    private readonly RazorHXOptions _options;

    public RazorHXTagHelperComponent(RazorHXOptions options)
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

        // Inject RazorHX stylesheet
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

        // Inject RazorHX core script
        output.PostContent.AppendHtml(
            "\n    <script src=\"/_rhx/js/rhx-core.js\" defer></script>\n");

        return Task.CompletedTask;
    }
}
