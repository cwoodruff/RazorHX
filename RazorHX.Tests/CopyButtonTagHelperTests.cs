using RazorHX.Components.Utilities;
using Xunit;

namespace RazorHX.Tests;

public class CopyButtonTagHelperTests : TagHelperTestBase
{
    private CopyButtonTagHelper CreateHelper() => new();

    // ── Element rendering ──

    [Fact]
    public void Renders_Button_Element()
    {
        var helper = CreateHelper();
        var context = CreateContext("rhx-copy-button");
        var output = CreateOutput("rhx-copy-button");

        helper.ViewContext = CreateViewContext();
        helper.Process(context, output);

        Assert.Equal("button", output.TagName);
    }

    [Fact]
    public void Has_Block_Class()
    {
        var helper = CreateHelper();
        var context = CreateContext("rhx-copy-button");
        var output = CreateOutput("rhx-copy-button");

        helper.ViewContext = CreateViewContext();
        helper.Process(context, output);

        Assert.True(HasClass(output, "rhx-copy-button"));
    }

    [Fact]
    public void Has_Data_Attribute()
    {
        var helper = CreateHelper();
        var context = CreateContext("rhx-copy-button");
        var output = CreateOutput("rhx-copy-button");

        helper.ViewContext = CreateViewContext();
        helper.Process(context, output);

        AssertAttribute(output, "data-rhx-copy-button", "");
    }

    [Fact]
    public void Has_Type_Button()
    {
        var helper = CreateHelper();
        var context = CreateContext("rhx-copy-button");
        var output = CreateOutput("rhx-copy-button");

        helper.ViewContext = CreateViewContext();
        helper.Process(context, output);

        AssertAttribute(output, "type", "button");
    }

    // ── Value and From ──

    [Fact]
    public void Value_Sets_Data_Attribute()
    {
        var helper = CreateHelper();
        helper.Value = "hello world";
        var context = CreateContext("rhx-copy-button");
        var output = CreateOutput("rhx-copy-button");

        helper.ViewContext = CreateViewContext();
        helper.Process(context, output);

        AssertAttribute(output, "data-rhx-copy-value", "hello world");
    }

    [Fact]
    public void From_Sets_Data_Attribute()
    {
        var helper = CreateHelper();
        helper.From = "#code pre";
        var context = CreateContext("rhx-copy-button");
        var output = CreateOutput("rhx-copy-button");

        helper.ViewContext = CreateViewContext();
        helper.Process(context, output);

        AssertAttribute(output, "data-rhx-copy-from", "#code pre");
    }

    [Fact]
    public void No_Value_Omits_Data_Attribute()
    {
        var helper = CreateHelper();
        var context = CreateContext("rhx-copy-button");
        var output = CreateOutput("rhx-copy-button");

        helper.ViewContext = CreateViewContext();
        helper.Process(context, output);

        AssertNoAttribute(output, "data-rhx-copy-value");
    }

    [Fact]
    public void No_From_Omits_Data_Attribute()
    {
        var helper = CreateHelper();
        var context = CreateContext("rhx-copy-button");
        var output = CreateOutput("rhx-copy-button");

        helper.ViewContext = CreateViewContext();
        helper.Process(context, output);

        AssertNoAttribute(output, "data-rhx-copy-from");
    }

    // ── Labels ──

    [Fact]
    public void Default_Aria_Label_Is_Copy()
    {
        var helper = CreateHelper();
        var context = CreateContext("rhx-copy-button");
        var output = CreateOutput("rhx-copy-button");

        helper.ViewContext = CreateViewContext();
        helper.Process(context, output);

        AssertAttribute(output, "aria-label", "Copy");
    }

    [Fact]
    public void Custom_Copy_Label_Sets_Aria_Label()
    {
        var helper = CreateHelper();
        helper.CopyLabel = "Copy command";
        var context = CreateContext("rhx-copy-button");
        var output = CreateOutput("rhx-copy-button");

        helper.ViewContext = CreateViewContext();
        helper.Process(context, output);

        AssertAttribute(output, "aria-label", "Copy command");
    }

    [Fact]
    public void Custom_Success_Label_Sets_Data_Attribute()
    {
        var helper = CreateHelper();
        helper.SuccessLabel = "Done!";
        var context = CreateContext("rhx-copy-button");
        var output = CreateOutput("rhx-copy-button");

        helper.ViewContext = CreateViewContext();
        helper.Process(context, output);

        AssertAttribute(output, "data-rhx-copy-success-label", "Done!");
    }

    [Fact]
    public void Default_Success_Label_In_Data_Attribute()
    {
        var helper = CreateHelper();
        var context = CreateContext("rhx-copy-button");
        var output = CreateOutput("rhx-copy-button");

        helper.ViewContext = CreateViewContext();
        helper.Process(context, output);

        AssertAttribute(output, "data-rhx-copy-success-label", "Copied!");
    }

    // ── Feedback duration ──

    [Fact]
    public void Default_Duration_Omits_Data_Attribute()
    {
        var helper = CreateHelper();
        var context = CreateContext("rhx-copy-button");
        var output = CreateOutput("rhx-copy-button");

        helper.ViewContext = CreateViewContext();
        helper.Process(context, output);

        AssertNoAttribute(output, "data-rhx-copy-duration");
    }

    [Fact]
    public void Custom_Duration_Sets_Data_Attribute()
    {
        var helper = CreateHelper();
        helper.FeedbackDuration = 3000;
        var context = CreateContext("rhx-copy-button");
        var output = CreateOutput("rhx-copy-button");

        helper.ViewContext = CreateViewContext();
        helper.Process(context, output);

        AssertAttribute(output, "data-rhx-copy-duration", "3000");
    }

    // ── Disabled state ──

    [Fact]
    public void Disabled_Adds_Modifier_Class()
    {
        var helper = CreateHelper();
        helper.Disabled = true;
        var context = CreateContext("rhx-copy-button");
        var output = CreateOutput("rhx-copy-button");

        helper.ViewContext = CreateViewContext();
        helper.Process(context, output);

        Assert.True(HasClass(output, "rhx-copy-button--disabled"));
    }

    [Fact]
    public void Disabled_Sets_Html_Attribute()
    {
        var helper = CreateHelper();
        helper.Disabled = true;
        var context = CreateContext("rhx-copy-button");
        var output = CreateOutput("rhx-copy-button");

        helper.ViewContext = CreateViewContext();
        helper.Process(context, output);

        AssertAttribute(output, "disabled", "disabled");
    }

    [Fact]
    public void Disabled_Sets_Aria_Disabled()
    {
        var helper = CreateHelper();
        helper.Disabled = true;
        var context = CreateContext("rhx-copy-button");
        var output = CreateOutput("rhx-copy-button");

        helper.ViewContext = CreateViewContext();
        helper.Process(context, output);

        AssertAttribute(output, "aria-disabled", "true");
    }

    [Fact]
    public void Not_Disabled_Omits_Disabled_Attributes()
    {
        var helper = CreateHelper();
        var context = CreateContext("rhx-copy-button");
        var output = CreateOutput("rhx-copy-button");

        helper.ViewContext = CreateViewContext();
        helper.Process(context, output);

        Assert.False(HasClass(output, "rhx-copy-button--disabled"));
        AssertNoAttribute(output, "disabled");
        AssertNoAttribute(output, "aria-disabled");
    }

    // ── Content ──

    [Fact]
    public void Contains_Copy_Icon_Span()
    {
        var helper = CreateHelper();
        var context = CreateContext("rhx-copy-button");
        var output = CreateOutput("rhx-copy-button");

        helper.ViewContext = CreateViewContext();
        helper.Process(context, output);

        var content = output.Content.GetContent();
        Assert.Contains("rhx-copy-button__icon--copy", content);
        Assert.Contains("<svg", content);
    }

    [Fact]
    public void Contains_Success_Icon_Span()
    {
        var helper = CreateHelper();
        var context = CreateContext("rhx-copy-button");
        var output = CreateOutput("rhx-copy-button");

        helper.ViewContext = CreateViewContext();
        helper.Process(context, output);

        var content = output.Content.GetContent();
        Assert.Contains("rhx-copy-button__icon--success", content);
    }

    // ── CSS class merging ──

    [Fact]
    public void Custom_Css_Class_Is_Merged()
    {
        var helper = CreateHelper();
        helper.CssClass = "my-custom";
        var context = CreateContext("rhx-copy-button");
        var output = CreateOutput("rhx-copy-button");

        helper.ViewContext = CreateViewContext();
        helper.Process(context, output);

        Assert.True(HasClass(output, "rhx-copy-button"));
        Assert.True(HasClass(output, "my-custom"));
    }
}
