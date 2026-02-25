using System.Globalization;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace htmxRazor.Components.Formatting;

/// <summary>
/// Formats a byte or bit value into a human-readable string with appropriate units.
/// </summary>
/// <example>
/// <code>
/// &lt;rhx-format-bytes rhx-value="1536" /&gt;              → 1.5 KB
/// &lt;rhx-format-bytes rhx-value="1000" rhx-unit="bit" /&gt; → 1 kb
/// &lt;rhx-format-bytes rhx-value="2048" rhx-display="long" /&gt; → 2 kilobytes
/// </code>
/// </example>
[HtmlTargetElement("rhx-format-bytes")]
public class FormatBytesTagHelper : TagHelper
{
    /// <summary>The raw value in bytes (or bits if unit="bit").</summary>
    [HtmlAttributeName("rhx-value")]
    public long Value { get; set; }

    /// <summary>Unit type: "byte" (default, 1024-based) or "bit" (1000-based).</summary>
    [HtmlAttributeName("rhx-unit")]
    public string Unit { get; set; } = "byte";

    /// <summary>Display style: "short" (1.5 MB), "long" (1.5 megabytes), "narrow" (1.5MB).</summary>
    [HtmlAttributeName("rhx-display")]
    public string Display { get; set; } = "short";

    /// <summary>BCP 47 language tag for number formatting (e.g., "de-DE").</summary>
    [HtmlAttributeName("rhx-lang")]
    public string? Lang { get; set; }

    /// <summary>Additional CSS classes.</summary>
    [HtmlAttributeName("class")]
    public string? CssClass { get; set; }

    /// <inheritdoc/>
    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        output.TagName = "span";
        output.TagMode = TagMode.StartTagAndEndTag;

        var classes = string.IsNullOrWhiteSpace(CssClass) ? "rhx-format-bytes" : $"rhx-format-bytes {CssClass}";
        output.Attributes.SetAttribute("class", classes);

        var culture = ResolveCulture(Lang);
        output.Content.SetContent(FormatValue(Value, Unit, Display, culture));
    }

    /// <summary>
    /// Formats a byte/bit value with the given display style.
    /// </summary>
    public static string FormatValue(long value, string unit, string display, CultureInfo? culture = null)
    {
        culture ??= CultureInfo.InvariantCulture;
        var isBit = unit.Equals("bit", StringComparison.OrdinalIgnoreCase);
        var divisor = isBit ? 1000.0 : 1024.0;

        var shortLabels = isBit
            ? new[] { "b", "kb", "Mb", "Gb", "Tb", "Pb" }
            : new[] { "B", "KB", "MB", "GB", "TB", "PB" };
        var longSingular = isBit
            ? new[] { "bit", "kilobit", "megabit", "gigabit", "terabit", "petabit" }
            : new[] { "byte", "kilobyte", "megabyte", "gigabyte", "terabyte", "petabyte" };
        var longPlural = isBit
            ? new[] { "bits", "kilobits", "megabits", "gigabits", "terabits", "petabits" }
            : new[] { "bytes", "kilobytes", "megabytes", "gigabytes", "terabytes", "petabytes" };

        var abs = Math.Abs((double)value);
        var scaled = (double)value;
        var idx = 0;

        while (abs >= divisor && idx < shortLabels.Length - 1)
        {
            abs /= divisor;
            scaled /= divisor;
            idx++;
        }

        // Format number: no decimals for base unit, 1 decimal for larger; omit ".0"
        string number;
        if (idx == 0)
        {
            number = ((long)scaled).ToString("N0", culture);
        }
        else
        {
            var isWhole = Math.Abs(scaled - Math.Round(scaled)) < 0.05;
            number = isWhole
                ? Math.Round(scaled).ToString("N0", culture)
                : scaled.ToString("N1", culture);
        }

        var d = display?.ToLowerInvariant() ?? "short";
        var label = d switch
        {
            "long" => Math.Abs(scaled - 1.0) < 0.05 && !isBit ? longSingular[idx] : longPlural[idx],
            _ => shortLabels[idx]
        };

        // For "long" singular: exactly "1 byte", "1 kilobyte", etc.
        if (d == "long" && Math.Abs(scaled - 1.0) < 0.05)
            label = longSingular[idx];

        var sep = d == "narrow" ? "" : " ";
        return $"{number}{sep}{label}";
    }

    internal static CultureInfo ResolveCulture(string? lang)
    {
        if (string.IsNullOrWhiteSpace(lang)) return CultureInfo.InvariantCulture;
        try { return CultureInfo.GetCultureInfo(lang); }
        catch { return CultureInfo.InvariantCulture; }
    }
}
