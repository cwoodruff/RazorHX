using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using RazorHX.Components.Forms;
using Xunit;

namespace RazorHX.Tests;

public class CheckboxTagHelperTests : TagHelperTestBase
{
    private CheckboxTagHelper CreateHelper() => new(CreateUrlHelperFactory());

    // ── Test model ──

    private class TestModel
    {
        public bool AgreeToTerms { get; set; }
        public bool DarkMode { get; set; }
    }

    private static ModelExpression CreateModelExpressionFor(string propertyName, object? value = null)
    {
        var provider = new EmptyModelMetadataProvider();
        var metadata = provider.GetMetadataForProperty(typeof(TestModel), propertyName);
        var explorer = new ModelExplorer(provider, metadata, value);
        return new ModelExpression(propertyName, explorer);
    }

    // ── Element rendering ──

    [Fact]
    public void Renders_Div_Element()
    {
        var helper = CreateHelper();
        var context = CreateContext("rhx-checkbox");
        var output = CreateOutput("rhx-checkbox");

        helper.ViewContext = CreateViewContext();
        helper.Process(context, output);

        Assert.Equal("div", output.TagName);
    }

    [Fact]
    public void Has_Block_Class()
    {
        var helper = CreateHelper();
        var context = CreateContext("rhx-checkbox");
        var output = CreateOutput("rhx-checkbox");

        helper.ViewContext = CreateViewContext();
        helper.Process(context, output);

        Assert.True(HasClass(output, "rhx-checkbox"));
    }

    [Fact]
    public void Has_Data_Attribute()
    {
        var helper = CreateHelper();
        var context = CreateContext("rhx-checkbox");
        var output = CreateOutput("rhx-checkbox");

        helper.ViewContext = CreateViewContext();
        helper.Process(context, output);

        AssertAttribute(output, "data-rhx-checkbox", "");
    }

    // ── Label wrapper ──

    [Fact]
    public void Has_Label_Wrapper()
    {
        var helper = CreateHelper();
        var context = CreateContext("rhx-checkbox");
        var output = CreateOutput("rhx-checkbox");

        helper.ViewContext = CreateViewContext();
        helper.Process(context, output);

        var content = output.Content.GetContent();
        Assert.Contains("<label", content);
        Assert.Contains("rhx-checkbox__label", content);
    }

    // ── Native checkbox ──

    [Fact]
    public void Has_Native_Checkbox_Input()
    {
        var helper = CreateHelper();
        var context = CreateContext("rhx-checkbox");
        var output = CreateOutput("rhx-checkbox");

        helper.ViewContext = CreateViewContext();
        helper.Process(context, output);

        var content = output.Content.GetContent();
        Assert.Contains("type=\"checkbox\"", content);
        Assert.Contains("rhx-checkbox__native", content);
    }

    [Fact]
    public void Native_Input_Is_Sr_Only()
    {
        var helper = CreateHelper();
        var context = CreateContext("rhx-checkbox");
        var output = CreateOutput("rhx-checkbox");

        helper.ViewContext = CreateViewContext();
        helper.Process(context, output);

        var content = output.Content.GetContent();
        Assert.Contains("rhx-sr-only", content);
    }

    [Fact]
    public void Value_Is_True()
    {
        var helper = CreateHelper();
        var context = CreateContext("rhx-checkbox");
        var output = CreateOutput("rhx-checkbox");

        helper.ViewContext = CreateViewContext();
        helper.Process(context, output);

        var content = output.Content.GetContent();
        Assert.Contains("value=\"true\"", content);
    }

    // ── Hidden false input ──

    [Fact]
    public void Has_Hidden_False_Input()
    {
        var helper = CreateHelper();
        helper.Name = "agree";
        var context = CreateContext("rhx-checkbox");
        var output = CreateOutput("rhx-checkbox");

        helper.ViewContext = CreateViewContext();
        helper.Process(context, output);

        var content = output.Content.GetContent();
        Assert.Contains("type=\"hidden\"", content);
        Assert.Contains("value=\"false\"", content);
    }

    [Fact]
    public void Name_On_Native_And_Hidden_Inputs()
    {
        var helper = CreateHelper();
        helper.Name = "agree";
        var context = CreateContext("rhx-checkbox");
        var output = CreateOutput("rhx-checkbox");

        helper.ViewContext = CreateViewContext();
        helper.Process(context, output);

        var content = output.Content.GetContent();
        // Both hidden and checkbox inputs have name
        Assert.Contains("name=\"agree\"", content);
    }

    // ── Checked state ──

    [Fact]
    public void Checked_Sets_Checked_Attribute()
    {
        var helper = CreateHelper();
        helper.Checked = true;
        var context = CreateContext("rhx-checkbox");
        var output = CreateOutput("rhx-checkbox");

        helper.ViewContext = CreateViewContext();
        helper.Process(context, output);

        var content = output.Content.GetContent();
        Assert.Contains(" checked", content);
    }

    [Fact]
    public void Not_Checked_By_Default()
    {
        var helper = CreateHelper();
        var context = CreateContext("rhx-checkbox");
        var output = CreateOutput("rhx-checkbox");

        helper.ViewContext = CreateViewContext();
        helper.Process(context, output);

        var content = output.Content.GetContent();
        Assert.DoesNotContain(" checked", content);
    }

    // ── Custom visual control ──

    [Fact]
    public void Has_Custom_Visual_Control()
    {
        var helper = CreateHelper();
        var context = CreateContext("rhx-checkbox");
        var output = CreateOutput("rhx-checkbox");

        helper.ViewContext = CreateViewContext();
        helper.Process(context, output);

        var content = output.Content.GetContent();
        Assert.Contains("rhx-checkbox__control", content);
        Assert.Contains("aria-hidden=\"true\"", content);
    }

    [Fact]
    public void Has_Checkmark_SVG()
    {
        var helper = CreateHelper();
        var context = CreateContext("rhx-checkbox");
        var output = CreateOutput("rhx-checkbox");

        helper.ViewContext = CreateViewContext();
        helper.Process(context, output);

        var content = output.Content.GetContent();
        Assert.Contains("rhx-checkbox__check", content);
        Assert.Contains("<polyline", content);
    }

    [Fact]
    public void Has_Indeterminate_Dash_SVG()
    {
        var helper = CreateHelper();
        var context = CreateContext("rhx-checkbox");
        var output = CreateOutput("rhx-checkbox");

        helper.ViewContext = CreateViewContext();
        helper.Process(context, output);

        var content = output.Content.GetContent();
        Assert.Contains("rhx-checkbox__dash", content);
    }

    // ── Indeterminate ──

    [Fact]
    public void Indeterminate_Sets_Data_Attribute()
    {
        var helper = CreateHelper();
        helper.Indeterminate = true;
        var context = CreateContext("rhx-checkbox");
        var output = CreateOutput("rhx-checkbox");

        helper.ViewContext = CreateViewContext();
        helper.Process(context, output);

        AssertAttribute(output, "data-rhx-indeterminate", "");
    }

    // ── Label text ──

    [Fact]
    public void Label_Text_Renders()
    {
        var helper = CreateHelper();
        helper.Label = "I agree";
        var context = CreateContext("rhx-checkbox");
        var output = CreateOutput("rhx-checkbox");

        helper.ViewContext = CreateViewContext();
        helper.Process(context, output);

        var content = output.Content.GetContent();
        Assert.Contains("rhx-checkbox__text", content);
        Assert.Contains("I agree", content);
    }

    // ── Hint ──

    [Fact]
    public void Hint_Renders()
    {
        var helper = CreateHelper();
        helper.Hint = "Required to proceed";
        helper.Name = "agree";
        var context = CreateContext("rhx-checkbox");
        var output = CreateOutput("rhx-checkbox");

        helper.ViewContext = CreateViewContext();
        helper.Process(context, output);

        var content = output.Content.GetContent();
        Assert.Contains("rhx-checkbox__hint", content);
        Assert.Contains("Required to proceed", content);
    }

    // ── Error ──

    [Fact]
    public void Error_Adds_Modifier_And_Message()
    {
        var helper = CreateHelper();
        helper.Name = "agree";
        var context = CreateContext("rhx-checkbox");
        var output = CreateOutput("rhx-checkbox");

        var viewContext = CreateViewContext();
        viewContext.ModelState.AddModelError("agree", "You must agree");
        helper.ViewContext = viewContext;
        helper.Process(context, output);

        Assert.True(HasClass(output, "rhx-checkbox--error"));
        var content = output.Content.GetContent();
        Assert.Contains("You must agree", content);
        Assert.Contains("aria-invalid=\"true\"", content);
    }

    // ── States ──

    [Fact]
    public void Disabled_Adds_Modifier_And_Attribute()
    {
        var helper = CreateHelper();
        helper.Disabled = true;
        var context = CreateContext("rhx-checkbox");
        var output = CreateOutput("rhx-checkbox");

        helper.ViewContext = CreateViewContext();
        helper.Process(context, output);

        Assert.True(HasClass(output, "rhx-checkbox--disabled"));
        var content = output.Content.GetContent();
        Assert.Contains(" disabled", content);
    }

    [Fact]
    public void Required_Sets_Aria()
    {
        var helper = CreateHelper();
        helper.Required = true;
        var context = CreateContext("rhx-checkbox");
        var output = CreateOutput("rhx-checkbox");

        helper.ViewContext = CreateViewContext();
        helper.Process(context, output);

        var content = output.Content.GetContent();
        Assert.Contains("aria-required=\"true\"", content);
        Assert.Contains(" required", content);
    }

    // ── Size ──

    [Fact]
    public void Small_Size_Adds_Modifier()
    {
        var helper = CreateHelper();
        helper.Size = "small";
        var context = CreateContext("rhx-checkbox");
        var output = CreateOutput("rhx-checkbox");

        helper.ViewContext = CreateViewContext();
        helper.Process(context, output);

        Assert.True(HasClass(output, "rhx-checkbox--small"));
    }

    [Fact]
    public void Large_Size_Adds_Modifier()
    {
        var helper = CreateHelper();
        helper.Size = "large";
        var context = CreateContext("rhx-checkbox");
        var output = CreateOutput("rhx-checkbox");

        helper.ViewContext = CreateViewContext();
        helper.Process(context, output);

        Assert.True(HasClass(output, "rhx-checkbox--large"));
    }

    // ── Model binding ──

    [Fact]
    public void For_Bool_True_Sets_Checked()
    {
        var helper = CreateHelper();
        helper.For = CreateModelExpressionFor("DarkMode", true);
        var context = CreateContext("rhx-checkbox");
        var output = CreateOutput("rhx-checkbox");

        helper.ViewContext = CreateViewContext();
        helper.Process(context, output);

        var content = output.Content.GetContent();
        Assert.Contains(" checked", content);
        Assert.Contains("name=\"DarkMode\"", content);
    }

    [Fact]
    public void For_Bool_False_Not_Checked()
    {
        var helper = CreateHelper();
        helper.For = CreateModelExpressionFor("AgreeToTerms", false);
        var context = CreateContext("rhx-checkbox");
        var output = CreateOutput("rhx-checkbox");

        helper.ViewContext = CreateViewContext();
        helper.Process(context, output);

        var content = output.Content.GetContent();
        Assert.DoesNotContain(" checked", content);
    }

    // ── htmx ──

    [Fact]
    public void Htmx_On_Native_Input()
    {
        var helper = CreateHelper();
        helper.HxPost = "/toggle";
        helper.HxTrigger = "change";
        var context = CreateContext("rhx-checkbox");
        var output = CreateOutput("rhx-checkbox");

        helper.ViewContext = CreateViewContext();
        helper.Process(context, output);

        var content = output.Content.GetContent();
        Assert.Contains("hx-post=\"/toggle\"", content);
        Assert.Contains("hx-trigger=\"change\"", content);
    }

    // ── CSS class merging ──

    [Fact]
    public void Custom_Css_Class_Merged()
    {
        var helper = CreateHelper();
        helper.CssClass = "my-checkbox";
        var context = CreateContext("rhx-checkbox");
        var output = CreateOutput("rhx-checkbox");

        helper.ViewContext = CreateViewContext();
        helper.Process(context, output);

        Assert.True(HasClass(output, "rhx-checkbox"));
        Assert.True(HasClass(output, "my-checkbox"));
    }

    // ── ARIA label ──

    [Fact]
    public void Aria_Label_Sets_Attribute()
    {
        var helper = CreateHelper();
        helper.AriaLabel = "Accept terms";
        var context = CreateContext("rhx-checkbox");
        var output = CreateOutput("rhx-checkbox");

        helper.ViewContext = CreateViewContext();
        helper.Process(context, output);

        var content = output.Content.GetContent();
        Assert.Contains("aria-label=\"Accept terms\"", content);
    }
}
