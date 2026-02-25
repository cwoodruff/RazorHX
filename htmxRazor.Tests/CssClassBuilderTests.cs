using htmxRazor.Infrastructure;
using Xunit;

namespace htmxRazor.Tests;

public class CssClassBuilderTests
{
    [Fact]
    public void Build_Empty_ReturnsEmptyString()
    {
        var builder = new CssClassBuilder();
        Assert.Equal("", builder.Build());
    }

    [Fact]
    public void Build_WithInitialClass_ReturnsClass()
    {
        var builder = new CssClassBuilder("rhx-button");
        Assert.Equal("rhx-button", builder.Build());
    }

    [Fact]
    public void Add_AppendsClass()
    {
        var result = new CssClassBuilder("a").Add("b").Build();
        Assert.Equal("a b", result);
    }

    [Fact]
    public void Add_IgnoresNull()
    {
        var result = new CssClassBuilder("a").Add(null).Build();
        Assert.Equal("a", result);
    }

    [Fact]
    public void Add_IgnoresEmpty()
    {
        var result = new CssClassBuilder("a").Add("").Build();
        Assert.Equal("a", result);
    }

    [Fact]
    public void Add_IgnoresWhitespace()
    {
        var result = new CssClassBuilder("a").Add("  ").Build();
        Assert.Equal("a", result);
    }

    [Fact]
    public void AddRange_AppendsMultipleClasses()
    {
        var result = new CssClassBuilder("a").AddRange("b", "c", "d").Build();
        Assert.Equal("a b c d", result);
    }

    [Fact]
    public void AddRange_FiltersNulls()
    {
        var result = new CssClassBuilder("a").AddRange("b", null, "c").Build();
        Assert.Equal("a b c", result);
    }

    [Fact]
    public void AddIf_True_AppendsClass()
    {
        var result = new CssClassBuilder("a").AddIf("b", true).Build();
        Assert.Equal("a b", result);
    }

    [Fact]
    public void AddIf_False_DoesNotAppendClass()
    {
        var result = new CssClassBuilder("a").AddIf("b", false).Build();
        Assert.Equal("a", result);
    }

    [Fact]
    public void AddChoice_True_AddsTrueClass()
    {
        var result = new CssClassBuilder().AddChoice(true, "yes", "no").Build();
        Assert.Equal("yes", result);
    }

    [Fact]
    public void AddChoice_False_AddsFalseClass()
    {
        var result = new CssClassBuilder().AddChoice(false, "yes", "no").Build();
        Assert.Equal("no", result);
    }

    [Fact]
    public void AddVariant_WithValue_AddsBemVariant()
    {
        var result = new CssClassBuilder("rhx-button")
            .AddVariant("button", "brand")
            .Build();
        Assert.Equal("rhx-button rhx-button--brand", result);
    }

    [Fact]
    public void AddVariant_NullVariant_SkipsClass()
    {
        var result = new CssClassBuilder("rhx-button")
            .AddVariant("button", null)
            .Build();
        Assert.Equal("rhx-button", result);
    }

    [Fact]
    public void AddVariant_EmptyVariant_SkipsClass()
    {
        var result = new CssClassBuilder("rhx-button")
            .AddVariant("button", "")
            .Build();
        Assert.Equal("rhx-button", result);
    }

    [Fact]
    public void AddSize_WithValue_AddsBemSize()
    {
        var result = new CssClassBuilder("rhx-button")
            .AddSize("button", "large")
            .Build();
        Assert.Equal("rhx-button rhx-button--large", result);
    }

    [Fact]
    public void AddSize_NullSize_SkipsClass()
    {
        var result = new CssClassBuilder("rhx-button")
            .AddSize("button", null)
            .Build();
        Assert.Equal("rhx-button", result);
    }

    [Fact]
    public void AddEnum_WithValue_AddsLowercaseClass()
    {
        var result = new CssClassBuilder("rhx-button")
            .AddEnum<TestVariant>("rhx-button--", TestVariant.Brand)
            .Build();
        Assert.Equal("rhx-button rhx-button--brand", result);
    }

    [Fact]
    public void AddEnum_NullValue_SkipsClass()
    {
        var result = new CssClassBuilder("rhx-button")
            .AddEnum<TestVariant>("rhx-button--", null)
            .Build();
        Assert.Equal("rhx-button", result);
    }

    [Fact]
    public void AddFrom_WithFactory_AddsResult()
    {
        var result = new CssClassBuilder("a")
            .AddFrom(() => "b")
            .Build();
        Assert.Equal("a b", result);
    }

    [Fact]
    public void AddFrom_NullFactory_SkipsClass()
    {
        var result = new CssClassBuilder("a")
            .AddFrom(() => null)
            .Build();
        Assert.Equal("a", result);
    }

    [Fact]
    public void IsEmpty_WithNoClasses_ReturnsTrue()
    {
        Assert.True(new CssClassBuilder().IsEmpty);
    }

    [Fact]
    public void IsEmpty_WithClasses_ReturnsFalse()
    {
        Assert.False(new CssClassBuilder("a").IsEmpty);
    }

    [Fact]
    public void Count_ReturnsClassCount()
    {
        var builder = new CssClassBuilder("a").Add("b").Add("c");
        Assert.Equal(3, builder.Count);
    }

    [Fact]
    public void ToString_ReturnsSameAsBuild()
    {
        var builder = new CssClassBuilder("x").Add("y");
        Assert.Equal(builder.Build(), builder.ToString());
    }

    [Fact]
    public void FluentChaining_ComplexExample()
    {
        var variant = "brand";
        var size = "large";
        var disabled = true;
        var loading = false;

        var css = new CssClassBuilder("rhx-button")
            .AddIf("rhx-button--brand", variant == "brand")
            .AddIf("rhx-button--large", size == "large")
            .AddIf("rhx-button--disabled", disabled)
            .AddIf("rhx-button--loading", loading)
            .Build();

        Assert.Equal("rhx-button rhx-button--brand rhx-button--large rhx-button--disabled", css);
    }

    private enum TestVariant
    {
        Default,
        Brand,
        Danger
    }
}
