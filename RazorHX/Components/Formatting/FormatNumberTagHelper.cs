using System.Globalization;
using System.Text;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace RazorHX.Components.Formatting;

/// <summary>
/// Formats a numeric value as decimal, currency, or percentage with culture support.
/// </summary>
/// <example>
/// <code>
/// &lt;rhx-format-number rhx-value="1234.5" /&gt;
/// &lt;rhx-format-number rhx-value="1234.56" rhx-type="currency" rhx-currency="USD" /&gt;
/// &lt;rhx-format-number rhx-value="0.75" rhx-type="percent" /&gt;
/// </code>
/// </example>
[HtmlTargetElement("rhx-format-number")]
public class FormatNumberTagHelper : TagHelper
{
    /// <summary>The numeric value to format.</summary>
    [HtmlAttributeName("rhx-value")]
    public double Value { get; set; }

    /// <summary>Format type: "decimal" (default), "currency", or "percent".</summary>
    [HtmlAttributeName("rhx-type")]
    public string Type { get; set; } = "decimal";

    /// <summary>ISO 4217 currency code (e.g., "USD", "EUR", "JPY").</summary>
    [HtmlAttributeName("rhx-currency")]
    public string? Currency { get; set; }

    /// <summary>How to display the currency: "symbol" ($), "code" (USD), or "name" (US dollars).</summary>
    [HtmlAttributeName("rhx-currency-display")]
    public string CurrencyDisplay { get; set; } = "symbol";

    /// <summary>Disables thousands grouping separators.</summary>
    [HtmlAttributeName("rhx-no-grouping")]
    public bool NoGrouping { get; set; }

    /// <summary>Minimum number of fraction digits.</summary>
    [HtmlAttributeName("rhx-minimum-fraction-digits")]
    public int? MinimumFractionDigits { get; set; }

    /// <summary>Maximum number of fraction digits.</summary>
    [HtmlAttributeName("rhx-maximum-fraction-digits")]
    public int? MaximumFractionDigits { get; set; }

    /// <summary>BCP 47 language tag for formatting (e.g., "de-DE").</summary>
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

        var classes = string.IsNullOrWhiteSpace(CssClass) ? "rhx-format-number" : $"rhx-format-number {CssClass}";
        output.Attributes.SetAttribute("class", classes);

        output.Content.SetContent(FormatValue(
            Value, Type, Currency, CurrencyDisplay, NoGrouping,
            MinimumFractionDigits, MaximumFractionDigits, Lang));
    }

    /// <summary>
    /// Formats a number according to the specified type and options.
    /// </summary>
    public static string FormatValue(double value, string type, string? currency,
        string currencyDisplay, bool noGrouping, int? minFrac, int? maxFrac, string? lang)
    {
        var culture = ResolveCulture(lang, currency, type);
        var nfi = (NumberFormatInfo)culture.NumberFormat.Clone();

        if (noGrouping)
        {
            nfi.NumberGroupSeparator = "";
            nfi.CurrencyGroupSeparator = "";
            nfi.PercentGroupSeparator = "";
        }

        var t = type?.ToLowerInvariant() ?? "decimal";

        if (t == "percent")
            return FormatPercent(value, nfi, minFrac, maxFrac);

        if (t == "currency")
            return FormatCurrency(value, currency, currencyDisplay, nfi, minFrac, maxFrac);

        return FormatDecimal(value, nfi, minFrac, maxFrac);
    }

    private static string FormatDecimal(double value, NumberFormatInfo nfi, int? minFrac, int? maxFrac)
    {
        if (minFrac.HasValue || maxFrac.HasValue)
        {
            var min = minFrac ?? 0;
            var max = maxFrac ?? (minFrac ?? nfi.NumberDecimalDigits);
            if (max < min) max = min;
            return value.ToString(BuildFormat(min, max, nfi.NumberGroupSeparator != ""), nfi);
        }

        return value.ToString("N", nfi);
    }

    private static string FormatPercent(double value, NumberFormatInfo nfi, int? minFrac, int? maxFrac)
    {
        var digits = maxFrac ?? minFrac ?? nfi.PercentDecimalDigits;
        nfi.PercentDecimalDigits = digits;
        return value.ToString("P" + digits, nfi);
    }

    private static string FormatCurrency(double value, string? currency, string currencyDisplay,
        NumberFormatInfo nfi, int? minFrac, int? maxFrac)
    {
        // Apply currency info
        if (!string.IsNullOrWhiteSpace(currency))
        {
            var (symbol, defaultDecimals) = GetCurrencyInfo(currency);
            nfi.CurrencySymbol = symbol;
            if (!minFrac.HasValue && !maxFrac.HasValue)
                nfi.CurrencyDecimalDigits = defaultDecimals;
        }

        if (maxFrac.HasValue)
            nfi.CurrencyDecimalDigits = maxFrac.Value;
        else if (minFrac.HasValue)
            nfi.CurrencyDecimalDigits = minFrac.Value;

        var formatted = value.ToString("C", nfi);

        var cd = currencyDisplay?.ToLowerInvariant() ?? "symbol";
        if (cd != "symbol" && !string.IsNullOrWhiteSpace(currency))
        {
            var label = cd == "code"
                ? currency.ToUpperInvariant()
                : GetCurrencyName(currency);
            formatted = formatted.Replace(nfi.CurrencySymbol, label);
            formatted = EnsureLabelSpacing(formatted);
        }

        return formatted;
    }

    /// <summary>
    /// Builds a custom number format string with variable fraction digits.
    /// </summary>
    public static string BuildFormat(int minFrac, int maxFrac, bool grouped)
    {
        var intPart = grouped ? "#,##0" : "0";
        if (maxFrac == 0) return intPart;
        var frac = new string('0', minFrac) + new string('#', maxFrac - minFrac);
        return intPart + "." + frac;
    }

    /// <summary>
    /// Ensures a space between letter sequences and digit sequences in a formatted string.
    /// Handles cases like "USD1,234.56" → "USD 1,234.56".
    /// </summary>
    public static string EnsureLabelSpacing(string s)
    {
        var sb = new StringBuilder(s.Length + 2);
        for (var i = 0; i < s.Length; i++)
        {
            sb.Append(s[i]);
            if (i + 1 < s.Length)
            {
                if (char.IsLetter(s[i]) && (char.IsDigit(s[i + 1]) || s[i + 1] == '-'))
                    sb.Append(' ');
                else if (char.IsDigit(s[i]) && char.IsLetter(s[i + 1]))
                    sb.Append(' ');
            }
        }
        return sb.ToString();
    }

    // ── Currency data ──

    private static readonly Dictionary<string, (string symbol, int decimals)> CurrencyData =
        new(StringComparer.OrdinalIgnoreCase)
        {
            ["USD"] = ("$", 2), ["EUR"] = ("€", 2), ["GBP"] = ("£", 2),
            ["JPY"] = ("¥", 0), ["CNY"] = ("¥", 2), ["KRW"] = ("₩", 0),
            ["INR"] = ("₹", 2), ["CAD"] = ("CA$", 2), ["AUD"] = ("A$", 2),
            ["CHF"] = ("CHF", 2), ["BRL"] = ("R$", 2), ["MXN"] = ("MX$", 2),
            ["SEK"] = ("kr", 2), ["NOK"] = ("kr", 2), ["DKK"] = ("kr", 2),
            ["PLN"] = ("zł", 2), ["RUB"] = ("₽", 2), ["THB"] = ("฿", 2),
            ["TWD"] = ("NT$", 0), ["SGD"] = ("S$", 2), ["HKD"] = ("HK$", 2),
            ["NZD"] = ("NZ$", 2), ["ZAR"] = ("R", 2), ["TRY"] = ("₺", 2),
            ["ILS"] = ("₪", 2), ["CZK"] = ("Kč", 2), ["HUF"] = ("Ft", 0),
        };

    /// <summary>Gets the currency symbol and default decimal places for a currency code.</summary>
    public static (string symbol, int decimals) GetCurrencyInfo(string? code)
    {
        if (string.IsNullOrWhiteSpace(code)) return ("$", 2);
        return CurrencyData.TryGetValue(code, out var info) ? info : (code, 2);
    }

    private static readonly Dictionary<string, string> CurrencyNames =
        new(StringComparer.OrdinalIgnoreCase)
        {
            ["USD"] = "US dollars", ["EUR"] = "euros", ["GBP"] = "British pounds",
            ["JPY"] = "Japanese yen", ["CNY"] = "Chinese yuan", ["KRW"] = "South Korean won",
            ["INR"] = "Indian rupees", ["CAD"] = "Canadian dollars", ["AUD"] = "Australian dollars",
            ["CHF"] = "Swiss francs", ["BRL"] = "Brazilian reais", ["MXN"] = "Mexican pesos",
        };

    internal static string GetCurrencyName(string? code)
    {
        if (string.IsNullOrWhiteSpace(code)) return "dollars";
        return CurrencyNames.TryGetValue(code, out var name) ? name : code;
    }

    private static CultureInfo ResolveCulture(string? lang, string? currency, string? type)
    {
        if (!string.IsNullOrWhiteSpace(lang))
        {
            try { return CultureInfo.GetCultureInfo(lang); }
            catch { /* fall through */ }
        }

        // For currency, use a culture that matches the currency code
        if (type?.Equals("currency", StringComparison.OrdinalIgnoreCase) == true
            && !string.IsNullOrWhiteSpace(currency))
        {
            var cultureName = currency.ToUpperInvariant() switch
            {
                "USD" => "en-US", "EUR" => "fr-FR", "GBP" => "en-GB",
                "JPY" => "ja-JP", "CNY" => "zh-CN", "KRW" => "ko-KR",
                "INR" => "hi-IN", "CAD" => "en-CA", "AUD" => "en-AU",
                "CHF" => "de-CH", "BRL" => "pt-BR", "MXN" => "es-MX",
                _ => null
            };
            if (cultureName != null)
            {
                try { return CultureInfo.GetCultureInfo(cultureName); }
                catch { /* fall through */ }
            }
        }

        return CultureInfo.InvariantCulture;
    }
}
