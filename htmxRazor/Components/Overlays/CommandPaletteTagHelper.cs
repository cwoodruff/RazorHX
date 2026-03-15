using System.Net;
using htmxRazor.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace htmxRazor.Components.Overlays;

/// <summary>
/// Renders a modal search overlay activated via a keyboard shortcut (Cmd+K / Ctrl+K).
/// Fires a debounced <c>hx-get</c> to a configurable search endpoint. Results are
/// entirely server-rendered using <c>&lt;rhx-command-group&gt;</c> and <c>&lt;rhx-command-item&gt;</c>.
/// </summary>
/// <example>
/// <code>
/// &lt;rhx-command-palette id="search" rhx-placeholder="Search components..."
///     hx-get="/Search" hx-target="#search-results" hx-swap="innerHTML"&gt;
/// &lt;/rhx-command-palette&gt;
/// </code>
/// </example>
[HtmlTargetElement("rhx-command-palette")]
public class CommandPaletteTagHelper : htmxRazorTagHelperBase
{
    /// <inheritdoc/>
    protected override string BlockName => "command-palette";

    /// <summary>Input placeholder text. Default: "Search...".</summary>
    [HtmlAttributeName("rhx-placeholder")]
    public string Placeholder { get; set; } = "Search...";

    /// <summary>Keyboard shortcut to open. Default: "mod+k".</summary>
    [HtmlAttributeName("rhx-shortcut")]
    public string Shortcut { get; set; } = "mod+k";

    /// <summary>Debounce delay in ms. Default: 300.</summary>
    [HtmlAttributeName("rhx-debounce")]
    public int Debounce { get; set; } = 300;

    /// <summary>Minimum characters before search fires. Default: 1.</summary>
    [HtmlAttributeName("rhx-min-chars")]
    public int MinChars { get; set; } = 1;

    /// <summary>Message shown when results are empty. Default: "No results found".</summary>
    [HtmlAttributeName("rhx-empty-message")]
    public string EmptyMessage { get; set; } = "No results found";

    /// <summary>Accessible label. Default: "Command palette".</summary>
    [HtmlAttributeName("rhx-label")]
    public string Label { get; set; } = "Command palette";

    /// <summary>
    /// Creates a new CommandPaletteTagHelper with URL generation support.
    /// </summary>
    public CommandPaletteTagHelper(IUrlHelperFactory urlHelperFactory) : base(urlHelperFactory) { }

    /// <inheritdoc/>
    public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
    {
        var childContent = await output.GetChildContentAsync();

        output.TagName = "div";
        output.TagMode = TagMode.StartTagAndEndTag;

        var css = CreateCssBuilder();
        ApplyBaseAttributes(output, css);

        var paletteId = Id ?? $"rhx-cp-{context.UniqueId}";
        var resultsId = $"{paletteId}-results";
        var inputId = $"{paletteId}-input";

        output.Attributes.SetAttribute("data-rhx-command-palette", "");
        output.Attributes.SetAttribute("data-rhx-shortcut", Shortcut);
        output.Attributes.SetAttribute("role", "dialog");
        output.Attributes.SetAttribute("aria-modal", "true");
        output.Attributes.SetAttribute("aria-label", Label);
        output.Attributes.SetAttribute("hidden", "hidden");

        // Build inner HTML
        output.Content.Clear();

        // Backdrop
        output.Content.AppendHtml($"<div class=\"{GetElementClass("backdrop")}\"></div>");

        // Panel
        output.Content.AppendHtml($"<div class=\"{GetElementClass("panel")}\">");

        // Search area
        output.Content.AppendHtml($"<div class=\"{GetElementClass("search")}\">");

        // Search icon
        output.Content.AppendHtml(
            $"<span class=\"{GetElementClass("search-icon")}\" aria-hidden=\"true\">" +
            "<svg width=\"16\" height=\"16\" viewBox=\"0 0 24 24\" fill=\"none\" stroke=\"currentColor\" stroke-width=\"2\" stroke-linecap=\"round\" stroke-linejoin=\"round\">" +
            "<circle cx=\"11\" cy=\"11\" r=\"8\" /><path d=\"M21 21l-4.35-4.35\" />" +
            "</svg></span>");

        // Input with htmx
        var htmxTrigger = $"input changed delay:{Debounce}ms";
        output.Content.AppendHtml(
            $"<input class=\"{GetElementClass("input")}\"" +
            $" id=\"{Enc(inputId)}\"" +
            " type=\"text\"" +
            " role=\"combobox\"" +
            " aria-expanded=\"false\"" +
            $" aria-controls=\"{Enc(resultsId)}\"" +
            " aria-autocomplete=\"list\"" +
            " autocomplete=\"off\"" +
            $" placeholder=\"{Enc(Placeholder)}\"");

        // Forward htmx attributes to the input
        AppendHtmxAttr(output, "hx-get", HxGet);
        AppendHtmxAttr(output, "hx-post", HxPost);
        output.Content.AppendHtml($" hx-target=\"#{Enc(resultsId)}\"");
        output.Content.AppendHtml(" hx-swap=\"innerHTML\"");
        output.Content.AppendHtml($" hx-trigger=\"{Enc(htmxTrigger)}\"");

        if (!string.IsNullOrWhiteSpace(HxIndicator))
            output.Content.AppendHtml($" hx-indicator=\"{Enc(HxIndicator)}\"");
        if (!string.IsNullOrWhiteSpace(HxInclude))
            output.Content.AppendHtml($" hx-include=\"{Enc(HxInclude)}\"");
        if (!string.IsNullOrWhiteSpace(HxHeaders))
            output.Content.AppendHtml($" hx-headers=\"{Enc(HxHeaders)}\"");
        if (!string.IsNullOrWhiteSpace(HxParams))
            output.Content.AppendHtml($" hx-params=\"{Enc(HxParams)}\"");

        output.Content.AppendHtml($" name=\"q\"");
        output.Content.AppendHtml(" />");

        // Shortcut hint
        var shortcutDisplay = Shortcut.Replace("mod+", "\u2318").ToUpperInvariant();
        output.Content.AppendHtml(
            $"<kbd class=\"{GetElementClass("shortcut")}\">{Enc(shortcutDisplay)}</kbd>");

        output.Content.AppendHtml("</div>"); // close search

        // Results
        output.Content.AppendHtml(
            $"<div class=\"{GetElementClass("results")}\" id=\"{Enc(resultsId)}\"" +
            " role=\"listbox\" aria-label=\"Search results\">");
        output.Content.AppendHtml(childContent);
        output.Content.AppendHtml("</div>");

        // Empty message
        output.Content.AppendHtml(
            $"<div class=\"{GetElementClass("empty")}\" hidden>{Enc(EmptyMessage)}</div>");

        output.Content.AppendHtml("</div>"); // close panel
    }

    private void AppendHtmxAttr(TagHelperOutput output, string name, string? value)
    {
        if (value == null) return;
        if (value.Length == 0)
        {
            var url = GenerateRouteUrl();
            if (!string.IsNullOrWhiteSpace(url))
                output.Content.AppendHtml($" {name}=\"{Enc(url)}\"");
        }
        else
        {
            output.Content.AppendHtml($" {name}=\"{Enc(value)}\"");
        }
    }

    private static string Enc(string? value) => WebUtility.HtmlEncode(value ?? "") ?? "";
}
