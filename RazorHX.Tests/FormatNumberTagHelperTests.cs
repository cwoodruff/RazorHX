using RazorHX.Components.Formatting;
using Xunit;

namespace RazorHX.Tests;

public class FormatNumberTagHelperTests : TagHelperTestBase
{
    private FormatNumberTagHelper CreateHelper()
    {
        return new FormatNumberTagHelper();
    }

    // ══════════════════════════════════════════════
    //  Structure
    // ══════════════════════════════════════════════

    [Fact]
    public void Renders_Span_Element()
    {
        var helper = CreateHelper();
        helper.Value = 42;
        var context = CreateContext("rhx-format-number");
        var output = CreateOutput("rhx-format-number");

        helper.Process(context, output);

        Assert.Equal("span", output.TagName);
    }

    [Fact]
    public void Has_Block_Class()
    {
        var helper = CreateHelper();
        helper.Value = 42;
        var context = CreateContext("rhx-format-number");
        var output = CreateOutput("rhx-format-number");

        helper.Process(context, output);

        Assert.True(HasClass(output, "rhx-format-number"));
    }

    [Fact]
    public void Custom_Class_Merged()
    {
        var helper = CreateHelper();
        helper.Value = 42;
        helper.CssClass = "price";
        var context = CreateContext("rhx-format-number");
        var output = CreateOutput("rhx-format-number");

        helper.Process(context, output);

        Assert.True(HasClass(output, "price"));
    }

    // ══════════════════════════════════════════════
    //  Decimal
    // ══════════════════════════════════════════════

    [Fact]
    public void Decimal_Default()
    {
        var result = FormatNumberTagHelper.FormatValue(1234.56, "decimal", null, "symbol", false, null, null, null);
        Assert.Equal("1,234.56", result);
    }

    [Fact]
    public void Decimal_No_Grouping()
    {
        var result = FormatNumberTagHelper.FormatValue(1234.56, "decimal", null, "symbol", true, null, null, null);
        Assert.Equal("1234.56", result);
    }

    [Fact]
    public void Decimal_Max_Fraction_Zero()
    {
        var result = FormatNumberTagHelper.FormatValue(1234.56, "decimal", null, "symbol", false, null, 0, null);
        Assert.Equal("1,235", result);
    }

    [Fact]
    public void Decimal_Min_Max_Fraction()
    {
        var result = FormatNumberTagHelper.FormatValue(1.5, "decimal", null, "symbol", false, 0, 3, null);
        Assert.Equal("1.5", result);
    }

    [Fact]
    public void Decimal_Min_Fraction_Pads()
    {
        var result = FormatNumberTagHelper.FormatValue(1.0, "decimal", null, "symbol", false, 2, 2, null);
        Assert.Equal("1.00", result);
    }

    // ══════════════════════════════════════════════
    //  Currency
    // ══════════════════════════════════════════════

    [Fact]
    public void Currency_USD_Symbol()
    {
        var result = FormatNumberTagHelper.FormatValue(1234.56, "currency", "USD", "symbol", false, null, null, null);
        Assert.Contains("1,234.56", result);
        Assert.Contains("$", result);
    }

    [Fact]
    public void Currency_USD_Code()
    {
        var result = FormatNumberTagHelper.FormatValue(1234.56, "currency", "USD", "code", false, null, null, null);
        Assert.Contains("USD", result);
        Assert.Contains("1,234.56", result);
    }

    [Fact]
    public void Currency_USD_Name()
    {
        var result = FormatNumberTagHelper.FormatValue(1234.56, "currency", "USD", "name", false, null, null, null);
        Assert.Contains("US dollars", result);
        Assert.Contains("1,234.56", result);
    }

    [Fact]
    public void Currency_JPY_Zero_Decimals()
    {
        var result = FormatNumberTagHelper.FormatValue(1234.0, "currency", "JPY", "symbol", false, null, null, null);
        Assert.Contains("¥", result);
        Assert.DoesNotContain(".", result);
    }

    [Fact]
    public void Currency_EUR_Symbol()
    {
        var result = FormatNumberTagHelper.FormatValue(1234.56, "currency", "EUR", "symbol", false, null, null, null);
        Assert.Contains("€", result);
    }

    [Fact]
    public void Currency_Custom_Fraction()
    {
        var result = FormatNumberTagHelper.FormatValue(9.9, "currency", "USD", "symbol", false, null, 0, null);
        Assert.Contains("$", result);
        Assert.Contains("10", result);
        Assert.DoesNotContain(".9", result);
    }

    // ══════════════════════════════════════════════
    //  Percent
    // ══════════════════════════════════════════════

    [Fact]
    public void Percent_Default()
    {
        var result = FormatNumberTagHelper.FormatValue(0.75, "percent", null, "symbol", false, null, null, null);
        Assert.Contains("75", result);
        Assert.Contains("%", result);
    }

    [Fact]
    public void Percent_Custom_Fraction()
    {
        var result = FormatNumberTagHelper.FormatValue(0.756, "percent", null, "symbol", false, null, 1, null);
        Assert.Contains("75.6", result);
    }

    // ══════════════════════════════════════════════
    //  Culture
    // ══════════════════════════════════════════════

    [Fact]
    public void German_Culture()
    {
        var result = FormatNumberTagHelper.FormatValue(1234.56, "decimal", null, "symbol", false, null, null, "de-DE");
        Assert.Contains("1.234,56", result);
    }

    // ══════════════════════════════════════════════
    //  Helpers
    // ══════════════════════════════════════════════

    [Fact]
    public void BuildFormat_Min0_Max2()
    {
        Assert.Equal("#,##0.##", FormatNumberTagHelper.BuildFormat(0, 2, true));
    }

    [Fact]
    public void BuildFormat_Min2_Max2()
    {
        Assert.Equal("#,##0.00", FormatNumberTagHelper.BuildFormat(2, 2, true));
    }

    [Fact]
    public void BuildFormat_No_Grouping()
    {
        Assert.Equal("0.0##", FormatNumberTagHelper.BuildFormat(1, 3, false));
    }

    [Fact]
    public void EnsureLabelSpacing_Prefix()
    {
        Assert.Equal("USD 1234", FormatNumberTagHelper.EnsureLabelSpacing("USD1234"));
    }

    [Fact]
    public void EnsureLabelSpacing_Suffix()
    {
        Assert.Equal("1234 EUR", FormatNumberTagHelper.EnsureLabelSpacing("1234EUR"));
    }

    [Fact]
    public void EnsureLabelSpacing_Already_Spaced()
    {
        Assert.Equal("1234 EUR", FormatNumberTagHelper.EnsureLabelSpacing("1234 EUR"));
    }

    [Fact]
    public void GetCurrencyInfo_Known()
    {
        var (symbol, decimals) = FormatNumberTagHelper.GetCurrencyInfo("JPY");
        Assert.Equal("¥", symbol);
        Assert.Equal(0, decimals);
    }

    [Fact]
    public void GetCurrencyInfo_Unknown()
    {
        var (symbol, decimals) = FormatNumberTagHelper.GetCurrencyInfo("XYZ");
        Assert.Equal("XYZ", symbol);
        Assert.Equal(2, decimals);
    }
}
