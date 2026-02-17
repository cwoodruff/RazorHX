using RazorHX.Components.Feedback;
using Xunit;

namespace RazorHX.Tests;

public class BadgeTagHelperTests : TagHelperTestBase
{
    private BadgeTagHelper CreateHelper()
    {
        var helper = new BadgeTagHelper(CreateUrlHelperFactory());
        helper.ViewContext = CreateViewContext();
        return helper;
    }

    // ──────────────────────────────────────────────
    //  Default rendering
    // ──────────────────────────────────────────────

    [Fact]
    public void Renders_Span_Element()
    {
        var helper = CreateHelper();
        var context = CreateContext("rhx-badge");
        var output = CreateOutput("rhx-badge");

        helper.Process(context, output);

        Assert.Equal("span", output.TagName);
    }

    [Fact]
    public void Renders_Block_Class_And_Default_Variant()
    {
        var helper = CreateHelper();
        var context = CreateContext("rhx-badge");
        var output = CreateOutput("rhx-badge");

        helper.Process(context, output);

        Assert.True(HasClass(output, "rhx-badge"));
        Assert.True(HasClass(output, "rhx-badge--neutral"));
    }

    [Fact]
    public void Default_No_Pill_Modifier()
    {
        var helper = CreateHelper();
        var context = CreateContext("rhx-badge");
        var output = CreateOutput("rhx-badge");

        helper.Process(context, output);

        Assert.False(HasClass(output, "rhx-badge--pill"));
    }

    [Fact]
    public void Default_No_Pulse_Modifier()
    {
        var helper = CreateHelper();
        var context = CreateContext("rhx-badge");
        var output = CreateOutput("rhx-badge");

        helper.Process(context, output);

        Assert.False(HasClass(output, "rhx-badge--pulse"));
    }

    // ──────────────────────────────────────────────
    //  Variants
    // ──────────────────────────────────────────────

    [Theory]
    [InlineData("neutral", "rhx-badge--neutral")]
    [InlineData("brand", "rhx-badge--brand")]
    [InlineData("success", "rhx-badge--success")]
    [InlineData("warning", "rhx-badge--warning")]
    [InlineData("danger", "rhx-badge--danger")]
    public void Renders_Correct_Variant_Class(string variant, string expectedClass)
    {
        var helper = CreateHelper();
        helper.Variant = variant;
        var context = CreateContext("rhx-badge");
        var output = CreateOutput("rhx-badge");

        helper.Process(context, output);

        Assert.True(HasClass(output, expectedClass));
    }

    [Fact]
    public void Variant_Is_Case_Insensitive()
    {
        var helper = CreateHelper();
        helper.Variant = "Brand";
        var context = CreateContext("rhx-badge");
        var output = CreateOutput("rhx-badge");

        helper.Process(context, output);

        Assert.True(HasClass(output, "rhx-badge--brand"));
    }

    // ──────────────────────────────────────────────
    //  Pill modifier
    // ──────────────────────────────────────────────

    [Fact]
    public void Pill_Adds_Pill_Modifier()
    {
        var helper = CreateHelper();
        helper.Pill = true;
        var context = CreateContext("rhx-badge");
        var output = CreateOutput("rhx-badge");

        helper.Process(context, output);

        Assert.True(HasClass(output, "rhx-badge--pill"));
    }

    // ──────────────────────────────────────────────
    //  Pulse modifier
    // ──────────────────────────────────────────────

    [Fact]
    public void Pulse_Adds_Pulse_Modifier()
    {
        var helper = CreateHelper();
        helper.Pulse = true;
        var context = CreateContext("rhx-badge");
        var output = CreateOutput("rhx-badge");

        helper.Process(context, output);

        Assert.True(HasClass(output, "rhx-badge--pulse"));
    }

    // ──────────────────────────────────────────────
    //  Combined modifiers
    // ──────────────────────────────────────────────

    [Fact]
    public void Pill_And_Pulse_Together()
    {
        var helper = CreateHelper();
        helper.Pill = true;
        helper.Pulse = true;
        var context = CreateContext("rhx-badge");
        var output = CreateOutput("rhx-badge");

        helper.Process(context, output);

        Assert.True(HasClass(output, "rhx-badge--pill"));
        Assert.True(HasClass(output, "rhx-badge--pulse"));
    }

    [Fact]
    public void Variant_And_Pill_And_Pulse()
    {
        var helper = CreateHelper();
        helper.Variant = "danger";
        helper.Pill = true;
        helper.Pulse = true;
        var context = CreateContext("rhx-badge");
        var output = CreateOutput("rhx-badge");

        helper.Process(context, output);

        Assert.True(HasClass(output, "rhx-badge--danger"));
        Assert.True(HasClass(output, "rhx-badge--pill"));
        Assert.True(HasClass(output, "rhx-badge--pulse"));
    }

    // ──────────────────────────────────────────────
    //  Custom CSS class
    // ──────────────────────────────────────────────

    [Fact]
    public void Custom_CssClass_Appended()
    {
        var helper = CreateHelper();
        helper.CssClass = "my-badge";
        var context = CreateContext("rhx-badge");
        var output = CreateOutput("rhx-badge");

        helper.Process(context, output);

        Assert.True(HasClass(output, "my-badge"));
        Assert.True(HasClass(output, "rhx-badge"));
    }

    // ──────────────────────────────────────────────
    //  Tag mode
    // ──────────────────────────────────────────────

    [Fact]
    public void Uses_StartTagAndEndTag_Mode()
    {
        var helper = CreateHelper();
        var context = CreateContext("rhx-badge");
        var output = CreateOutput("rhx-badge");

        helper.Process(context, output);

        Assert.Equal(Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, output.TagMode);
    }
}
