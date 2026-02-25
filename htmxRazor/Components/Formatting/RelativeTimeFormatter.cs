namespace htmxRazor.Components.Formatting;

/// <summary>
/// Computes human-readable relative time strings (e.g., "5 days ago", "in 3 hours", "yesterday").
/// </summary>
public static class RelativeTimeFormatter
{
    /// <summary>
    /// Formats the difference between <paramref name="date"/> and <paramref name="now"/>
    /// as a human-readable relative time string.
    /// </summary>
    /// <param name="date">The target date.</param>
    /// <param name="now">The reference date (typically now).</param>
    /// <param name="format">"long" (default), "short", or "narrow".</param>
    /// <param name="numeric">"always" (default) or "auto" (uses "yesterday" instead of "1 day ago").</param>
    public static string Format(DateTimeOffset date, DateTimeOffset now, string format = "long", string numeric = "always")
    {
        var diff = now - date;
        var isFuture = diff < TimeSpan.Zero;
        var abs = isFuture ? diff.Negate() : diff;
        var totalSeconds = abs.TotalSeconds;

        // "just now" / "in a moment" threshold
        if (totalSeconds < 10)
        {
            return format switch
            {
                "narrow" => "now",
                "short" => "now",
                _ => isFuture ? "in a moment" : "just now"
            };
        }

        var (value, unit) = GetUnitAndValue(abs);

        // Auto: use natural language for single-unit values
        if (numeric?.Equals("auto", StringComparison.OrdinalIgnoreCase) == true && value == 1)
        {
            var auto = GetAutoText(unit, isFuture, format);
            if (auto != null) return auto;
        }

        return FormatRelative(value, unit, isFuture, format);
    }

    internal static (long value, string unit) GetUnitAndValue(TimeSpan abs)
    {
        if (abs.TotalSeconds < 60) return (Math.Max(1, (long)abs.TotalSeconds), "second");
        if (abs.TotalMinutes < 60) return ((long)abs.TotalMinutes, "minute");
        if (abs.TotalHours < 24) return ((long)abs.TotalHours, "hour");
        if (abs.TotalDays < 7) return ((long)abs.TotalDays, "day");
        if (abs.TotalDays < 30) return ((long)(abs.TotalDays / 7), "week");
        if (abs.TotalDays < 365) return ((long)(abs.TotalDays / 30), "month");
        return ((long)(abs.TotalDays / 365), "year");
    }

    internal static string FormatRelative(long value, string unit, bool isFuture, string format)
    {
        var shortUnit = unit switch
        {
            "second" => "s",
            "minute" => "m",
            "hour" => "h",
            "day" => "d",
            "week" => "w",
            "month" => "mo",
            "year" => "y",
            _ => unit
        };

        if (format == "narrow")
            return $"{value}{shortUnit}";

        if (format == "short")
            return isFuture ? $"in {value}{shortUnit}" : $"{value}{shortUnit} ago";

        // Long format
        var plural = value == 1 ? "" : "s";
        return isFuture
            ? $"in {value} {unit}{plural}"
            : $"{value} {unit}{plural} ago";
    }

    private static string? GetAutoText(string unit, bool isFuture, string format)
    {
        if (format == "narrow" || format == "short")
        {
            return unit switch
            {
                "day" => isFuture ? "tomorrow" : "yesterday",
                _ => null
            };
        }

        return unit switch
        {
            "day" => isFuture ? "tomorrow" : "yesterday",
            "week" => isFuture ? "next week" : "last week",
            "month" => isFuture ? "next month" : "last month",
            "year" => isFuture ? "next year" : "last year",
            _ => null
        };
    }
}
