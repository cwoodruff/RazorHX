using System.Globalization;
using htmxRazor.Components.Formatting;
using Xunit;

namespace htmxRazor.Tests;

public class FormatDateTagHelperTests : TagHelperTestBase
{
    private static readonly DateTimeOffset TestDate = new(2025, 3, 15, 10, 30, 45, TimeSpan.Zero);

    private FormatDateTagHelper CreateHelper()
    {
        return new FormatDateTagHelper { Date = TestDate };
    }

    // ══════════════════════════════════════════════
    //  Structure
    // ══════════════════════════════════════════════

    [Fact]
    public void Renders_Time_Element()
    {
        var helper = CreateHelper();
        var context = CreateContext("rhx-format-date");
        var output = CreateOutput("rhx-format-date");

        helper.Process(context, output);

        Assert.Equal("time", output.TagName);
    }

    [Fact]
    public void Has_Block_Class()
    {
        var helper = CreateHelper();
        var context = CreateContext("rhx-format-date");
        var output = CreateOutput("rhx-format-date");

        helper.Process(context, output);

        Assert.True(HasClass(output, "rhx-format-date"));
    }

    [Fact]
    public void Has_Datetime_Attribute()
    {
        var helper = CreateHelper();
        var context = CreateContext("rhx-format-date");
        var output = CreateOutput("rhx-format-date");

        helper.Process(context, output);

        var dt = GetAttribute(output, "datetime");
        Assert.NotNull(dt);
        Assert.Contains("2025-03-15", dt);
    }

    [Fact]
    public void Custom_Class_Merged()
    {
        var helper = CreateHelper();
        helper.CssClass = "created-at";
        var context = CreateContext("rhx-format-date");
        var output = CreateOutput("rhx-format-date");

        helper.Process(context, output);

        Assert.True(HasClass(output, "rhx-format-date"));
        Assert.True(HasClass(output, "created-at"));
    }

    // ══════════════════════════════════════════════
    //  Custom format string
    // ══════════════════════════════════════════════

    [Fact]
    public void Custom_Format_String()
    {
        var helper = CreateHelper();
        helper.Format = "yyyy-MM-dd";
        var context = CreateContext("rhx-format-date");
        var output = CreateOutput("rhx-format-date");

        helper.Process(context, output);

        Assert.Equal("2025-03-15", output.Content.GetContent());
    }

    [Fact]
    public void Custom_Format_With_Time()
    {
        var helper = CreateHelper();
        helper.Format = "HH:mm:ss";
        var context = CreateContext("rhx-format-date");
        var output = CreateOutput("rhx-format-date");

        helper.Process(context, output);

        Assert.Equal("10:30:45", output.Content.GetContent());
    }

    // ══════════════════════════════════════════════
    //  Individual parts
    // ══════════════════════════════════════════════

    [Fact]
    public void Month_Long_Day_Year()
    {
        var helper = CreateHelper();
        helper.Month = "long";
        helper.Day = "numeric";
        helper.Year = "numeric";
        var context = CreateContext("rhx-format-date");
        var output = CreateOutput("rhx-format-date");

        helper.Process(context, output);

        Assert.Equal("March 15, 2025", output.Content.GetContent());
    }

    [Fact]
    public void Month_Short_Day()
    {
        var helper = CreateHelper();
        helper.Month = "short";
        helper.Day = "numeric";
        var context = CreateContext("rhx-format-date");
        var output = CreateOutput("rhx-format-date");

        helper.Process(context, output);

        Assert.Equal("Mar 15", output.Content.GetContent());
    }

    [Fact]
    public void Month_Numeric_Day_Year()
    {
        var helper = CreateHelper();
        helper.Month = "numeric";
        helper.Day = "numeric";
        helper.Year = "numeric";
        var context = CreateContext("rhx-format-date");
        var output = CreateOutput("rhx-format-date");

        helper.Process(context, output);

        Assert.Equal("3/15/2025", output.Content.GetContent());
    }

    [Fact]
    public void Month_2Digit_Day_2Digit()
    {
        var helper = CreateHelper();
        helper.Month = "2-digit";
        helper.Day = "2-digit";
        var context = CreateContext("rhx-format-date");
        var output = CreateOutput("rhx-format-date");

        helper.Process(context, output);

        Assert.Equal("03/15", output.Content.GetContent());
    }

    [Fact]
    public void Year_Only()
    {
        var helper = CreateHelper();
        helper.Year = "numeric";
        var context = CreateContext("rhx-format-date");
        var output = CreateOutput("rhx-format-date");

        helper.Process(context, output);

        Assert.Equal("2025", output.Content.GetContent());
    }

    [Fact]
    public void Year_2Digit()
    {
        var helper = CreateHelper();
        helper.Year = "2-digit";
        var context = CreateContext("rhx-format-date");
        var output = CreateOutput("rhx-format-date");

        helper.Process(context, output);

        Assert.Equal("25", output.Content.GetContent());
    }

    [Fact]
    public void Weekday_Long()
    {
        var helper = CreateHelper();
        helper.Weekday = "long";
        helper.Month = "long";
        helper.Day = "numeric";
        var context = CreateContext("rhx-format-date");
        var output = CreateOutput("rhx-format-date");

        helper.Process(context, output);

        Assert.Equal("Saturday, March 15", output.Content.GetContent());
    }

    [Fact]
    public void Weekday_Short()
    {
        var helper = CreateHelper();
        helper.Weekday = "short";
        var context = CreateContext("rhx-format-date");
        var output = CreateOutput("rhx-format-date");

        helper.Process(context, output);

        Assert.Equal("Sat", output.Content.GetContent());
    }

    [Fact]
    public void Weekday_Narrow()
    {
        var helper = CreateHelper();
        helper.Weekday = "narrow";
        var context = CreateContext("rhx-format-date");
        var output = CreateOutput("rhx-format-date");

        helper.Process(context, output);

        Assert.Equal("S", output.Content.GetContent());
    }

    // ══════════════════════════════════════════════
    //  Time parts
    // ══════════════════════════════════════════════

    [Fact]
    public void Hour_Minute_24h()
    {
        var helper = CreateHelper();
        helper.Hour = "2-digit";
        helper.Minute = "2-digit";
        helper.HourFormat = "24";
        var context = CreateContext("rhx-format-date");
        var output = CreateOutput("rhx-format-date");

        helper.Process(context, output);

        Assert.Equal("10:30", output.Content.GetContent());
    }

    [Fact]
    public void Hour_Minute_Second_12h()
    {
        var helper = CreateHelper();
        helper.Hour = "numeric";
        helper.Minute = "2-digit";
        helper.Second = "2-digit";
        helper.HourFormat = "12";
        var context = CreateContext("rhx-format-date");
        var output = CreateOutput("rhx-format-date");

        helper.Process(context, output);

        Assert.Equal("10:30:45 AM", output.Content.GetContent());
    }

    [Fact]
    public void Date_And_Time()
    {
        var helper = CreateHelper();
        helper.Month = "long";
        helper.Day = "numeric";
        helper.Year = "numeric";
        helper.Hour = "numeric";
        helper.Minute = "2-digit";
        helper.HourFormat = "12";
        var context = CreateContext("rhx-format-date");
        var output = CreateOutput("rhx-format-date");

        helper.Process(context, output);

        Assert.Equal("March 15, 2025 10:30 AM", output.Content.GetContent());
    }

    // ══════════════════════════════════════════════
    //  Timezone
    // ══════════════════════════════════════════════

    [Fact]
    public void Timezone_Conversion()
    {
        var helper = CreateHelper();
        helper.TimeZone = "America/New_York";
        helper.Hour = "numeric";
        helper.Minute = "2-digit";
        helper.HourFormat = "24";
        var context = CreateContext("rhx-format-date");
        var output = CreateOutput("rhx-format-date");

        helper.Process(context, output);

        // March 15, 2025 UTC -> EDT (UTC-4)
        var content = output.Content.GetContent();
        Assert.Equal("6:30", content);
    }
}
