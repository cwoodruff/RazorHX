using RazorHX.Components.Actions;
using Xunit;

namespace RazorHX.Tests;

public class ButtonTagHelperTests : TagHelperTestBase
{
    [Fact]
    public void Renders_Button_With_Default_Variant()
    {
        var tagHelper = new ButtonTagHelper();
        var context = CreateContext("rhx-button");
        var output = CreateOutput("rhx-button");

        tagHelper.Process(context, output);

        Assert.Equal("button", output.TagName);
        Assert.True(HasClass(output, "rhx-button"));
        Assert.True(HasClass(output, "rhx-button--default"));
        Assert.Equal("button", GetAttribute(output, "type"));
    }

    [Theory]
    [InlineData(ButtonVariant.Brand, "rhx-button--brand")]
    [InlineData(ButtonVariant.Success, "rhx-button--success")]
    [InlineData(ButtonVariant.Warning, "rhx-button--warning")]
    [InlineData(ButtonVariant.Danger, "rhx-button--danger")]
    [InlineData(ButtonVariant.Ghost, "rhx-button--ghost")]
    [InlineData(ButtonVariant.Text, "rhx-button--text")]
    public void Renders_Correct_Variant_Class(ButtonVariant variant, string expectedClass)
    {
        var tagHelper = new ButtonTagHelper { Variant = variant };
        var context = CreateContext("rhx-button");
        var output = CreateOutput("rhx-button");

        tagHelper.Process(context, output);

        Assert.True(HasClass(output, expectedClass));
    }

    [Fact]
    public void Renders_Small_Size_Class()
    {
        var tagHelper = new ButtonTagHelper { Size = ButtonSize.Small };
        var context = CreateContext("rhx-button");
        var output = CreateOutput("rhx-button");

        tagHelper.Process(context, output);

        Assert.True(HasClass(output, "rhx-button--small"));
    }

    [Fact]
    public void Renders_Large_Size_Class()
    {
        var tagHelper = new ButtonTagHelper { Size = ButtonSize.Large };
        var context = CreateContext("rhx-button");
        var output = CreateOutput("rhx-button");

        tagHelper.Process(context, output);

        Assert.True(HasClass(output, "rhx-button--large"));
    }

    [Fact]
    public void Sets_Disabled_Attribute_When_Disabled()
    {
        var tagHelper = new ButtonTagHelper { Disabled = true };
        var context = CreateContext("rhx-button");
        var output = CreateOutput("rhx-button");

        tagHelper.Process(context, output);

        Assert.Equal("disabled", GetAttribute(output, "disabled"));
        Assert.True(HasClass(output, "rhx-button--disabled"));
    }

    [Fact]
    public void Sets_Disabled_Attribute_When_Loading()
    {
        var tagHelper = new ButtonTagHelper { Loading = true };
        var context = CreateContext("rhx-button");
        var output = CreateOutput("rhx-button");

        tagHelper.Process(context, output);

        Assert.Equal("disabled", GetAttribute(output, "disabled"));
        Assert.True(HasClass(output, "rhx-button--loading"));
    }

    [Fact]
    public void Renders_FullWidth_Class()
    {
        var tagHelper = new ButtonTagHelper { FullWidth = true };
        var context = CreateContext("rhx-button");
        var output = CreateOutput("rhx-button");

        tagHelper.Process(context, output);

        Assert.True(HasClass(output, "rhx-button--full"));
    }

    [Fact]
    public void Applies_HxGet_Attribute()
    {
        var tagHelper = new ButtonTagHelper { HxGet = "/api/data" };
        var context = CreateContext("rhx-button");
        var output = CreateOutput("rhx-button");

        tagHelper.Process(context, output);

        Assert.Equal("/api/data", GetAttribute(output, "hx-get"));
    }

    [Fact]
    public void Applies_HxPost_And_Target()
    {
        var tagHelper = new ButtonTagHelper
        {
            HxPost = "/api/submit",
            HxTarget = "#result",
            HxSwap = "innerHTML"
        };
        var context = CreateContext("rhx-button");
        var output = CreateOutput("rhx-button");

        tagHelper.Process(context, output);

        Assert.Equal("/api/submit", GetAttribute(output, "hx-post"));
        Assert.Equal("#result", GetAttribute(output, "hx-target"));
        Assert.Equal("innerHTML", GetAttribute(output, "hx-swap"));
    }

    [Fact]
    public void Sets_AriaLabel_When_Provided()
    {
        var tagHelper = new ButtonTagHelper { AriaLabel = "Close dialog" };
        var context = CreateContext("rhx-button");
        var output = CreateOutput("rhx-button");

        tagHelper.Process(context, output);

        Assert.Equal("Close dialog", GetAttribute(output, "aria-label"));
    }

    [Fact]
    public void Appends_Custom_CssClass()
    {
        var tagHelper = new ButtonTagHelper { CssClass = "my-custom-class" };
        var context = CreateContext("rhx-button");
        var output = CreateOutput("rhx-button");

        tagHelper.Process(context, output);

        Assert.True(HasClass(output, "my-custom-class"));
        Assert.True(HasClass(output, "rhx-button"));
    }
}
