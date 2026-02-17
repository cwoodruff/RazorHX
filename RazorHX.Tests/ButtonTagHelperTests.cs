using RazorHX.Components.Actions;
using Xunit;

namespace RazorHX.Tests;

public class ButtonTagHelperTests : TagHelperTestBase
{
    private ButtonTagHelper CreateHelper()
    {
        var helper = new ButtonTagHelper(CreateUrlHelperFactory());
        helper.ViewContext = CreateViewContext();
        return helper;
    }

    [Fact]
    public void Renders_Button_Element_With_Block_Class()
    {
        var helper = CreateHelper();
        var context = CreateContext("rhx-button");
        var output = CreateOutput("rhx-button");

        helper.Process(context, output);

        Assert.Equal("button", output.TagName);
        Assert.True(HasClass(output, "rhx-button"));
        Assert.Equal("button", GetAttribute(output, "type"));
    }

    [Theory]
    [InlineData(ButtonVariant.Default, "rhx-button--default")]
    [InlineData(ButtonVariant.Brand, "rhx-button--brand")]
    [InlineData(ButtonVariant.Success, "rhx-button--success")]
    [InlineData(ButtonVariant.Warning, "rhx-button--warning")]
    [InlineData(ButtonVariant.Danger, "rhx-button--danger")]
    [InlineData(ButtonVariant.Ghost, "rhx-button--ghost")]
    [InlineData(ButtonVariant.Text, "rhx-button--text")]
    public void Renders_Correct_Variant_Class(ButtonVariant variant, string expectedClass)
    {
        var helper = CreateHelper();
        helper.Variant = variant;
        var context = CreateContext("rhx-button");
        var output = CreateOutput("rhx-button");

        helper.Process(context, output);

        Assert.True(HasClass(output, expectedClass));
    }

    [Fact]
    public void Renders_Small_Size_Class()
    {
        var helper = CreateHelper();
        helper.Size = ButtonSize.Small;
        var context = CreateContext("rhx-button");
        var output = CreateOutput("rhx-button");

        helper.Process(context, output);

        Assert.True(HasClass(output, "rhx-button--small"));
    }

    [Fact]
    public void Renders_Large_Size_Class()
    {
        var helper = CreateHelper();
        helper.Size = ButtonSize.Large;
        var context = CreateContext("rhx-button");
        var output = CreateOutput("rhx-button");

        helper.Process(context, output);

        Assert.True(HasClass(output, "rhx-button--large"));
    }

    [Fact]
    public void Default_Size_Has_No_Size_Modifier()
    {
        var helper = CreateHelper();
        helper.Size = ButtonSize.Default;
        var context = CreateContext("rhx-button");
        var output = CreateOutput("rhx-button");

        helper.Process(context, output);

        Assert.False(HasClass(output, "rhx-button--small"));
        Assert.False(HasClass(output, "rhx-button--large"));
    }

    [Fact]
    public void Sets_Disabled_Attribute_And_Class_When_Disabled()
    {
        var helper = CreateHelper();
        helper.Disabled = true;
        var context = CreateContext("rhx-button");
        var output = CreateOutput("rhx-button");

        helper.Process(context, output);

        Assert.Equal("disabled", GetAttribute(output, "disabled"));
        Assert.True(HasClass(output, "rhx-button--disabled"));
    }

    [Fact]
    public void Sets_Disabled_And_Loading_Class_When_Loading()
    {
        var helper = CreateHelper();
        helper.Loading = true;
        var context = CreateContext("rhx-button");
        var output = CreateOutput("rhx-button");

        helper.Process(context, output);

        Assert.Equal("disabled", GetAttribute(output, "disabled"));
        Assert.True(HasClass(output, "rhx-button--loading"));
    }

    [Fact]
    public void Renders_FullWidth_Class()
    {
        var helper = CreateHelper();
        helper.FullWidth = true;
        var context = CreateContext("rhx-button");
        var output = CreateOutput("rhx-button");

        helper.Process(context, output);

        Assert.True(HasClass(output, "rhx-button--full"));
    }

    [Fact]
    public void Renders_IconOnly_Class()
    {
        var helper = CreateHelper();
        helper.IconOnly = true;
        var context = CreateContext("rhx-button");
        var output = CreateOutput("rhx-button");

        helper.Process(context, output);

        Assert.True(HasClass(output, "rhx-button--icon-only"));
    }

    [Fact]
    public void Sets_AriaLabel_When_Provided()
    {
        var helper = CreateHelper();
        helper.AriaLabel = "Close dialog";
        var context = CreateContext("rhx-button");
        var output = CreateOutput("rhx-button");

        helper.Process(context, output);

        AssertAttribute(output, "aria-label", "Close dialog");
    }

    [Fact]
    public void Appends_Custom_CssClass()
    {
        var helper = CreateHelper();
        helper.CssClass = "my-custom-class";
        var context = CreateContext("rhx-button");
        var output = CreateOutput("rhx-button");

        helper.Process(context, output);

        Assert.True(HasClass(output, "my-custom-class"));
        Assert.True(HasClass(output, "rhx-button"));
    }

    [Fact]
    public void Renders_HxGet_Attribute()
    {
        var helper = CreateHelper();
        helper.HxGet = "/api/data";
        var context = CreateContext("rhx-button");
        var output = CreateOutput("rhx-button");

        helper.Process(context, output);

        AssertAttribute(output, "hx-get", "/api/data");
    }

    [Fact]
    public void Renders_HxPost_With_Target_And_Swap()
    {
        var helper = CreateHelper();
        helper.HxPost = "/api/submit";
        helper.HxTarget = "#result";
        helper.HxSwap = "innerHTML";
        var context = CreateContext("rhx-button");
        var output = CreateOutput("rhx-button");

        helper.Process(context, output);

        AssertAttribute(output, "hx-post", "/api/submit");
        AssertAttribute(output, "hx-target", "#result");
        AssertAttribute(output, "hx-swap", "innerHTML");
    }

    [Fact]
    public void Does_Not_Render_Null_Htmx_Attributes()
    {
        var helper = CreateHelper();
        var context = CreateContext("rhx-button");
        var output = CreateOutput("rhx-button");

        helper.Process(context, output);

        AssertNoAttribute(output, "hx-get");
        AssertNoAttribute(output, "hx-post");
        AssertNoAttribute(output, "hx-target");
        AssertNoAttribute(output, "hx-swap");
    }

    [Fact]
    public void Renders_HxConfirm()
    {
        var helper = CreateHelper();
        helper.HxPost = "/api/delete";
        helper.HxConfirm = "Are you sure?";
        var context = CreateContext("rhx-button");
        var output = CreateOutput("rhx-button");

        helper.Process(context, output);

        AssertAttribute(output, "hx-confirm", "Are you sure?");
    }

    [Fact]
    public void Renders_Submit_ButtonType()
    {
        var helper = CreateHelper();
        helper.ButtonType = "submit";
        var context = CreateContext("rhx-button");
        var output = CreateOutput("rhx-button");

        helper.Process(context, output);

        AssertAttribute(output, "type", "submit");
    }
}
