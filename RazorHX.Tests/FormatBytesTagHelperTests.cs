using System.Globalization;
using RazorHX.Components.Formatting;
using Xunit;

namespace RazorHX.Tests;

public class FormatBytesTagHelperTests : TagHelperTestBase
{
    private FormatBytesTagHelper CreateHelper()
    {
        return new FormatBytesTagHelper();
    }

    // ══════════════════════════════════════════════
    //  Structure
    // ══════════════════════════════════════════════

    [Fact]
    public void Renders_Span_Element()
    {
        var helper = CreateHelper();
        helper.Value = 1024;
        var context = CreateContext("rhx-format-bytes");
        var output = CreateOutput("rhx-format-bytes");

        helper.Process(context, output);

        Assert.Equal("span", output.TagName);
    }

    [Fact]
    public void Has_Block_Class()
    {
        var helper = CreateHelper();
        helper.Value = 0;
        var context = CreateContext("rhx-format-bytes");
        var output = CreateOutput("rhx-format-bytes");

        helper.Process(context, output);

        Assert.True(HasClass(output, "rhx-format-bytes"));
    }

    [Fact]
    public void Custom_Class_Merged()
    {
        var helper = CreateHelper();
        helper.Value = 0;
        helper.CssClass = "highlight";
        var context = CreateContext("rhx-format-bytes");
        var output = CreateOutput("rhx-format-bytes");

        helper.Process(context, output);

        Assert.True(HasClass(output, "rhx-format-bytes"));
        Assert.True(HasClass(output, "highlight"));
    }

    // ══════════════════════════════════════════════
    //  Byte formatting (1024-based)
    // ══════════════════════════════════════════════

    [Fact]
    public void Zero_Bytes()
    {
        Assert.Equal("0 B", FormatBytesTagHelper.FormatValue(0, "byte", "short"));
    }

    [Fact]
    public void One_Byte()
    {
        Assert.Equal("1 B", FormatBytesTagHelper.FormatValue(1, "byte", "short"));
    }

    [Fact]
    public void Sub_Kilobyte()
    {
        Assert.Equal("500 B", FormatBytesTagHelper.FormatValue(500, "byte", "short"));
    }

    [Fact]
    public void Exact_Kilobyte()
    {
        Assert.Equal("1 KB", FormatBytesTagHelper.FormatValue(1024, "byte", "short"));
    }

    [Fact]
    public void Fractional_Megabyte()
    {
        // 1.5 MB = 1.5 * 1024 * 1024 = 1572864
        Assert.Equal("1.5 MB", FormatBytesTagHelper.FormatValue(1572864, "byte", "short"));
    }

    [Fact]
    public void Exact_Gigabyte()
    {
        Assert.Equal("1 GB", FormatBytesTagHelper.FormatValue(1073741824, "byte", "short"));
    }

    [Fact]
    public void Terabyte()
    {
        Assert.Equal("1 TB", FormatBytesTagHelper.FormatValue(1099511627776, "byte", "short"));
    }

    // ══════════════════════════════════════════════
    //  Bit formatting (1000-based)
    // ══════════════════════════════════════════════

    [Fact]
    public void Bits_Short()
    {
        Assert.Equal("1 kb", FormatBytesTagHelper.FormatValue(1000, "bit", "short"));
    }

    [Fact]
    public void Kilobits()
    {
        Assert.Equal("1 kb", FormatBytesTagHelper.FormatValue(1000, "bit", "short",
            CultureInfo.InvariantCulture));
    }

    [Fact]
    public void Megabits()
    {
        Assert.Equal("1 Mb", FormatBytesTagHelper.FormatValue(1000000, "bit", "short",
            CultureInfo.InvariantCulture));
    }

    // ══════════════════════════════════════════════
    //  Display styles
    // ══════════════════════════════════════════════

    [Fact]
    public void Long_Singular()
    {
        Assert.Equal("1 byte", FormatBytesTagHelper.FormatValue(1, "byte", "long"));
    }

    [Fact]
    public void Long_Plural()
    {
        Assert.Equal("0 bytes", FormatBytesTagHelper.FormatValue(0, "byte", "long"));
    }

    [Fact]
    public void Long_Kilobytes()
    {
        Assert.Equal("1.5 megabytes", FormatBytesTagHelper.FormatValue(1572864, "byte", "long"));
    }

    [Fact]
    public void Narrow_No_Space()
    {
        Assert.Equal("1KB", FormatBytesTagHelper.FormatValue(1024, "byte", "narrow"));
    }

    [Fact]
    public void Narrow_Fractional()
    {
        Assert.Equal("1.5MB", FormatBytesTagHelper.FormatValue(1572864, "byte", "narrow"));
    }

    // ══════════════════════════════════════════════
    //  Culture
    // ══════════════════════════════════════════════

    [Fact]
    public void German_Culture_Decimal_Separator()
    {
        var de = CultureInfo.GetCultureInfo("de-DE");
        Assert.Equal("1,5 MB", FormatBytesTagHelper.FormatValue(1572864, "byte", "short", de));
    }
}
