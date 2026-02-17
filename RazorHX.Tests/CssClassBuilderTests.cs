using RazorHX.Infrastructure;
using Xunit;

namespace RazorHX.Tests;

public class CssClassBuilderTests
{
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
    public void Add_IgnoresNullAndWhitespace()
    {
        var result = new CssClassBuilder("a").Add("").Add(null!).Add("  ").Add("b").Build();
        Assert.Equal("a b", result);
    }

    [Fact]
    public void ToString_ReturnsBuild()
    {
        var builder = new CssClassBuilder("x").Add("y");
        Assert.Equal(builder.Build(), builder.ToString());
    }
}
