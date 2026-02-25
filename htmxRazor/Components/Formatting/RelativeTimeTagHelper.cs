using Microsoft.AspNetCore.Razor.TagHelpers;

namespace htmxRazor.Components.Formatting;

/// <summary>
/// Renders a human-readable relative time string (e.g., "5 days ago", "in 3 hours").
/// Outputs a semantic <c>&lt;time&gt;</c> element with ISO datetime and data attributes
/// for optional JavaScript auto-updating.
/// </summary>
/// <example>
/// <code>
/// &lt;rhx-relative-time rhx-date="@Model.CreatedAt" /&gt;
/// &lt;rhx-relative-time rhx-date="@Model.CreatedAt" rhx-numeric="auto" /&gt;
/// &lt;rhx-relative-time rhx-date="@Model.CreatedAt" rhx-format="short" /&gt;
/// </code>
/// </example>
[HtmlTargetElement("rhx-relative-time")]
public class RelativeTimeTagHelper : TagHelper
{
    /// <summary>The target date. Defaults to current UTC time if not set.</summary>
    [HtmlAttributeName("rhx-date")]
    public DateTimeOffset? Date { get; set; }

    /// <summary>Display format: "long" (5 minutes ago), "short" (5m ago), "narrow" (5m).</summary>
    [HtmlAttributeName("rhx-format")]
    public string Format { get; set; } = "long";

    /// <summary>
    /// "always" (default): always use numeric (e.g., "1 day ago").
    /// "auto": use natural language when possible (e.g., "yesterday").
    /// </summary>
    [HtmlAttributeName("rhx-numeric")]
    public string Numeric { get; set; } = "always";

    /// <summary>BCP 47 language tag (reserved for future localization).</summary>
    [HtmlAttributeName("rhx-lang")]
    public string? Lang { get; set; }

    /// <summary>Additional CSS classes.</summary>
    [HtmlAttributeName("class")]
    public string? CssClass { get; set; }

    /// <inheritdoc/>
    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        var date = Date ?? DateTimeOffset.UtcNow;
        var now = DateTimeOffset.UtcNow;

        output.TagName = "time";
        output.TagMode = TagMode.StartTagAndEndTag;

        var classes = string.IsNullOrWhiteSpace(CssClass)
            ? "rhx-relative-time"
            : $"rhx-relative-time {CssClass}";
        output.Attributes.SetAttribute("class", classes);

        var iso = date.ToString("yyyy-MM-dd'T'HH:mm:sszzz");
        output.Attributes.SetAttribute("datetime", iso);

        // Data attributes for JS auto-updating
        output.Attributes.SetAttribute("data-rhx-relative-time", iso);
        output.Attributes.SetAttribute("data-rhx-relative-format", Format.ToLowerInvariant());
        output.Attributes.SetAttribute("data-rhx-relative-numeric", Numeric.ToLowerInvariant());

        var text = RelativeTimeFormatter.Format(date, now, Format.ToLowerInvariant(), Numeric.ToLowerInvariant());
        output.Content.SetContent(text);
    }
}
