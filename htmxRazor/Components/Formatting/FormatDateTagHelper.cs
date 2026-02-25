using System.Globalization;
using System.Text;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace htmxRazor.Components.Formatting;

/// <summary>
/// Formats a date/time value using .NET formatting with culture support.
/// Renders a semantic <c>&lt;time&gt;</c> element with an ISO <c>datetime</c> attribute.
/// </summary>
/// <example>
/// <code>
/// &lt;rhx-format-date rhx-date="@DateTime.Now" /&gt;
/// &lt;rhx-format-date rhx-date="@Model.Created" rhx-month="long" rhx-day="numeric" rhx-year="numeric" /&gt;
/// &lt;rhx-format-date rhx-date="@Model.Created" rhx-format="yyyy-MM-dd" /&gt;
/// </code>
/// </example>
[HtmlTargetElement("rhx-format-date")]
public class FormatDateTagHelper : TagHelper
{
    /// <summary>The date to format. Defaults to current UTC time if not set.</summary>
    [HtmlAttributeName("rhx-date")]
    public DateTimeOffset? Date { get; set; }

    /// <summary>Custom .NET date/time format string. When set, overrides individual part properties.</summary>
    [HtmlAttributeName("rhx-format")]
    public string? Format { get; set; }

    /// <summary>Weekday display: "narrow" (M), "short" (Mon), "long" (Monday).</summary>
    [HtmlAttributeName("rhx-weekday")]
    public string? Weekday { get; set; }

    /// <summary>Year display: "numeric" (2025), "2-digit" (25).</summary>
    [HtmlAttributeName("rhx-year")]
    public string? Year { get; set; }

    /// <summary>Month display: "numeric" (3), "2-digit" (03), "short" (Mar), "long" (March).</summary>
    [HtmlAttributeName("rhx-month")]
    public string? Month { get; set; }

    /// <summary>Day display: "numeric" (5), "2-digit" (05).</summary>
    [HtmlAttributeName("rhx-day")]
    public string? Day { get; set; }

    /// <summary>Hour display: "numeric" (3), "2-digit" (03).</summary>
    [HtmlAttributeName("rhx-hour")]
    public string? Hour { get; set; }

    /// <summary>Minute display: "numeric" (5), "2-digit" (05).</summary>
    [HtmlAttributeName("rhx-minute")]
    public string? Minute { get; set; }

    /// <summary>Second display: "numeric" (5), "2-digit" (05).</summary>
    [HtmlAttributeName("rhx-second")]
    public string? Second { get; set; }

    /// <summary>IANA or .NET timezone ID for conversion before formatting.</summary>
    [HtmlAttributeName("rhx-timezone")]
    public string? TimeZone { get; set; }

    /// <summary>Hour format: "12" or "24" (default).</summary>
    [HtmlAttributeName("rhx-hour-format")]
    public string HourFormat { get; set; } = "24";

    /// <summary>BCP 47 language tag for formatting (e.g., "fr-FR").</summary>
    [HtmlAttributeName("rhx-lang")]
    public string? Lang { get; set; }

    /// <summary>Additional CSS classes.</summary>
    [HtmlAttributeName("class")]
    public string? CssClass { get; set; }

    /// <inheritdoc/>
    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        var date = Date ?? DateTimeOffset.UtcNow;

        // Apply timezone conversion
        if (!string.IsNullOrWhiteSpace(TimeZone))
        {
            try
            {
                var tz = TimeZoneInfo.FindSystemTimeZoneById(TimeZone);
                date = TimeZoneInfo.ConvertTime(date, tz);
            }
            catch
            {
                // Ignore invalid timezone, use date as-is
            }
        }

        output.TagName = "time";
        output.TagMode = TagMode.StartTagAndEndTag;

        var classes = string.IsNullOrWhiteSpace(CssClass) ? "rhx-format-date" : $"rhx-format-date {CssClass}";
        output.Attributes.SetAttribute("class", classes);
        output.Attributes.SetAttribute("datetime", date.ToString("yyyy-MM-dd'T'HH:mm:sszzz"));

        var culture = ResolveCulture(Lang);
        output.Content.SetContent(FormatDate(date, culture));
    }

    private string FormatDate(DateTimeOffset date, CultureInfo culture)
    {
        // Explicit format string takes priority
        if (!string.IsNullOrWhiteSpace(Format))
            return date.ToString(Format, culture);

        var hasAnyPart = Weekday != null || Year != null || Month != null || Day != null
                         || Hour != null || Minute != null || Second != null;

        if (!hasAnyPart)
            return date.ToString("d", culture); // Culture's short date

        return FormatFromParts(date, culture);
    }

    /// <summary>
    /// Builds a formatted date string from individual part properties.
    /// </summary>
    internal string FormatFromParts(DateTimeOffset date, CultureInfo culture)
    {
        var sb = new StringBuilder();

        // ── Weekday prefix ──
        if (Weekday != null)
        {
            var wd = Weekday.ToLowerInvariant();
            var dtf = culture.DateTimeFormat;
            sb.Append(wd switch
            {
                "narrow" => dtf.GetAbbreviatedDayName(date.DayOfWeek).Length > 0
                    ? dtf.GetAbbreviatedDayName(date.DayOfWeek)[..1]
                    : "?",
                "short" => date.ToString("ddd", culture),
                "long" => date.ToString("dddd", culture),
                _ => date.ToString("ddd", culture)
            });
        }

        // ── Date parts ──
        var dateParts = new List<string>();
        var isWordMonth = Month?.ToLowerInvariant() is "short" or "long";

        if (Month != null)
        {
            dateParts.Add(Month.ToLowerInvariant() switch
            {
                "numeric" => date.Month.ToString(culture),
                "2-digit" => date.ToString("MM", culture),
                "short" => date.ToString("MMM", culture),
                "long" => date.ToString("MMMM", culture),
                _ => date.ToString("MMMM", culture)
            });
        }

        if (Day != null)
        {
            dateParts.Add(Day.ToLowerInvariant() switch
            {
                "numeric" => date.Day.ToString(culture),
                "2-digit" => date.ToString("dd", culture),
                _ => date.Day.ToString(culture)
            });
        }

        if (Year != null)
        {
            dateParts.Add(Year.ToLowerInvariant() switch
            {
                "numeric" => date.Year.ToString(culture),
                "2-digit" => date.ToString("yy", culture),
                _ => date.Year.ToString(culture)
            });
        }

        if (dateParts.Count > 0)
        {
            if (sb.Length > 0) sb.Append(", ");

            if (isWordMonth)
            {
                // "March 15, 2025" style
                sb.Append(dateParts[0]); // month
                if (dateParts.Count >= 2) sb.Append(' ').Append(dateParts[1]); // day
                if (dateParts.Count >= 3) sb.Append(", ").Append(dateParts[2]); // year
            }
            else
            {
                // "3/15/2025" numeric style
                sb.Append(string.Join("/", dateParts));
            }
        }

        // ── Time parts ──
        var timeParts = new List<string>();
        var use12 = HourFormat == "12";

        if (Hour != null)
        {
            var h = use12 ? (date.Hour % 12 == 0 ? 12 : date.Hour % 12) : date.Hour;
            timeParts.Add(Hour.ToLowerInvariant() switch
            {
                "2-digit" => h.ToString("D2"),
                _ => h.ToString()
            });
        }

        if (Minute != null)
        {
            timeParts.Add(Minute.ToLowerInvariant() switch
            {
                "2-digit" => date.Minute.ToString("D2"),
                _ => date.Minute.ToString()
            });
        }

        if (Second != null)
        {
            timeParts.Add(Second.ToLowerInvariant() switch
            {
                "2-digit" => date.Second.ToString("D2"),
                _ => date.Second.ToString()
            });
        }

        if (timeParts.Count > 0)
        {
            if (sb.Length > 0) sb.Append(' ');
            sb.Append(string.Join(":", timeParts));
            if (use12 && Hour != null)
            {
                sb.Append(' ');
                sb.Append(date.Hour >= 12 ? "PM" : "AM");
            }
        }

        return sb.ToString();
    }

    private static CultureInfo ResolveCulture(string? lang)
    {
        if (string.IsNullOrWhiteSpace(lang)) return CultureInfo.InvariantCulture;
        try { return CultureInfo.GetCultureInfo(lang); }
        catch { return CultureInfo.InvariantCulture; }
    }
}
