using htmxRazor.Components.Actions;
using Xunit;

namespace htmxRazor.Tests;

public class ButtonGroupTagHelperTests : TagHelperTestBase
{
    private ButtonGroupTagHelper CreateHelper()
    {
        var helper = new ButtonGroupTagHelper(CreateUrlHelperFactory());
        helper.ViewContext = CreateViewContext();
        return helper;
    }

    [Fact]
    public void Renders_Div_Element()
    {
        var helper = CreateHelper();
        var context = CreateContext("rhx-button-group");
        var output = CreateOutput("rhx-button-group");

        helper.Process(context, output);

        Assert.Equal("div", output.TagName);
    }

    [Fact]
    public void Has_Block_Class()
    {
        var helper = CreateHelper();
        var context = CreateContext("rhx-button-group");
        var output = CreateOutput("rhx-button-group");

        helper.Process(context, output);

        Assert.True(HasClass(output, "rhx-button-group"));
    }

    [Fact]
    public void Sets_Role_Group()
    {
        var helper = CreateHelper();
        var context = CreateContext("rhx-button-group");
        var output = CreateOutput("rhx-button-group");

        helper.Process(context, output);

        AssertAttribute(output, "role", "group");
    }

    [Fact]
    public void Sets_AriaLabel_When_Label_Provided()
    {
        var helper = CreateHelper();
        helper.Label = "Actions";
        var context = CreateContext("rhx-button-group");
        var output = CreateOutput("rhx-button-group");

        helper.Process(context, output);

        AssertAttribute(output, "aria-label", "Actions");
    }

    [Fact]
    public void No_AriaLabel_When_Label_Null()
    {
        var helper = CreateHelper();
        var context = CreateContext("rhx-button-group");
        var output = CreateOutput("rhx-button-group");

        helper.Process(context, output);

        AssertNoAttribute(output, "aria-label");
    }

    [Fact]
    public void No_AriaLabel_When_Label_Empty()
    {
        var helper = CreateHelper();
        helper.Label = "";
        var context = CreateContext("rhx-button-group");
        var output = CreateOutput("rhx-button-group");

        helper.Process(context, output);

        AssertNoAttribute(output, "aria-label");
    }

    [Fact]
    public void No_AriaLabel_When_Label_Whitespace()
    {
        var helper = CreateHelper();
        helper.Label = "   ";
        var context = CreateContext("rhx-button-group");
        var output = CreateOutput("rhx-button-group");

        helper.Process(context, output);

        AssertNoAttribute(output, "aria-label");
    }

    [Fact]
    public void Appends_Custom_CssClass()
    {
        var helper = CreateHelper();
        helper.CssClass = "toolbar-actions";
        var context = CreateContext("rhx-button-group");
        var output = CreateOutput("rhx-button-group");

        helper.Process(context, output);

        Assert.True(HasClass(output, "toolbar-actions"));
        Assert.True(HasClass(output, "rhx-button-group"));
    }

    [Fact]
    public void Uses_StartTagAndEndTag_Mode()
    {
        var helper = CreateHelper();
        var context = CreateContext("rhx-button-group");
        var output = CreateOutput("rhx-button-group");

        helper.Process(context, output);

        Assert.Equal(Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, output.TagMode);
    }
}
