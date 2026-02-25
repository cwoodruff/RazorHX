using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using htmxRazor.Components.Forms;
using Xunit;

namespace htmxRazor.Tests;

public class SwitchTagHelperTests : TagHelperTestBase
{
    private SwitchTagHelper CreateHelper() => new(CreateUrlHelperFactory());

    // ── Test model ──

    private class TestModel
    {
        public bool DarkMode { get; set; }
        public bool Notifications { get; set; }
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
        var context = CreateContext("rhx-switch");
        var output = CreateOutput("rhx-switch");

        helper.ViewContext = CreateViewContext();
        helper.Process(context, output);

        Assert.Equal("div", output.TagName);
    }

    [Fact]
    public void Has_Block_Class()
    {
        var helper = CreateHelper();
        var context = CreateContext("rhx-switch");
        var output = CreateOutput("rhx-switch");

        helper.ViewContext = CreateViewContext();
        helper.Process(context, output);

        Assert.True(HasClass(output, "rhx-switch"));
    }

    [Fact]
    public void Has_Data_Attribute()
    {
        var helper = CreateHelper();
        var context = CreateContext("rhx-switch");
        var output = CreateOutput("rhx-switch");

        helper.ViewContext = CreateViewContext();
        helper.Process(context, output);

        AssertAttribute(output, "data-rhx-switch", "");
    }

    // ── Label wrapper ──

    [Fact]
    public void Has_Label_Wrapper()
    {
        var helper = CreateHelper();
        var context = CreateContext("rhx-switch");
        var output = CreateOutput("rhx-switch");

        helper.ViewContext = CreateViewContext();
        helper.Process(context, output);

        var content = output.Content.GetContent();
        Assert.Contains("<label", content);
        Assert.Contains("rhx-switch__label", content);
    }

    // ── Native input with switch role ──

    [Fact]
    public void Has_Native_Checkbox_With_Switch_Role()
    {
        var helper = CreateHelper();
        var context = CreateContext("rhx-switch");
        var output = CreateOutput("rhx-switch");

        helper.ViewContext = CreateViewContext();
        helper.Process(context, output);

        var content = output.Content.GetContent();
        Assert.Contains("type=\"checkbox\"", content);
        Assert.Contains("role=\"switch\"", content);
        Assert.Contains("rhx-switch__native", content);
        Assert.Contains("rhx-sr-only", content);
    }

    // ── Hidden false input ──

    [Fact]
    public void Has_Hidden_False_Input()
    {
        var helper = CreateHelper();
        helper.Name = "darkMode";
        var context = CreateContext("rhx-switch");
        var output = CreateOutput("rhx-switch");

        helper.ViewContext = CreateViewContext();
        helper.Process(context, output);

        var content = output.Content.GetContent();
        Assert.Contains("type=\"hidden\"", content);
        Assert.Contains("value=\"false\"", content);
    }

    // ── Track and thumb ──

    [Fact]
    public void Has_Track_And_Thumb()
    {
        var helper = CreateHelper();
        var context = CreateContext("rhx-switch");
        var output = CreateOutput("rhx-switch");

        helper.ViewContext = CreateViewContext();
        helper.Process(context, output);

        var content = output.Content.GetContent();
        Assert.Contains("rhx-switch__track", content);
        Assert.Contains("rhx-switch__thumb", content);
    }

    // ── Checked state ──

    [Fact]
    public void Checked_Sets_Checked_And_Aria_Checked_True()
    {
        var helper = CreateHelper();
        helper.Checked = true;
        var context = CreateContext("rhx-switch");
        var output = CreateOutput("rhx-switch");

        helper.ViewContext = CreateViewContext();
        helper.Process(context, output);

        var content = output.Content.GetContent();
        Assert.Contains(" checked", content);
        Assert.Contains("aria-checked=\"true\"", content);
    }

    [Fact]
    public void Not_Checked_Aria_Checked_False()
    {
        var helper = CreateHelper();
        var context = CreateContext("rhx-switch");
        var output = CreateOutput("rhx-switch");

        helper.ViewContext = CreateViewContext();
        helper.Process(context, output);

        var content = output.Content.GetContent();
        Assert.Contains("aria-checked=\"false\"", content);
        Assert.DoesNotContain(" checked ", content);
    }

    // ── Label text ──

    [Fact]
    public void Label_Text_Renders()
    {
        var helper = CreateHelper();
        helper.Label = "Dark mode";
        var context = CreateContext("rhx-switch");
        var output = CreateOutput("rhx-switch");

        helper.ViewContext = CreateViewContext();
        helper.Process(context, output);

        var content = output.Content.GetContent();
        Assert.Contains("rhx-switch__text", content);
        Assert.Contains("Dark mode", content);
    }

    // ── Hint ──

    [Fact]
    public void Hint_Renders()
    {
        var helper = CreateHelper();
        helper.Hint = "Toggle dark theme";
        helper.Name = "darkMode";
        var context = CreateContext("rhx-switch");
        var output = CreateOutput("rhx-switch");

        helper.ViewContext = CreateViewContext();
        helper.Process(context, output);

        var content = output.Content.GetContent();
        Assert.Contains("rhx-switch__hint", content);
        Assert.Contains("Toggle dark theme", content);
    }

    // ── Error ──

    [Fact]
    public void Error_Adds_Modifier_And_Message()
    {
        var helper = CreateHelper();
        helper.Name = "darkMode";
        var context = CreateContext("rhx-switch");
        var output = CreateOutput("rhx-switch");

        var viewContext = CreateViewContext();
        viewContext.ModelState.AddModelError("darkMode", "Selection required");
        helper.ViewContext = viewContext;
        helper.Process(context, output);

        Assert.True(HasClass(output, "rhx-switch--error"));
        var content = output.Content.GetContent();
        Assert.Contains("Selection required", content);
        Assert.Contains("aria-invalid=\"true\"", content);
    }

    // ── States ──

    [Fact]
    public void Disabled_Adds_Modifier_And_Attribute()
    {
        var helper = CreateHelper();
        helper.Disabled = true;
        var context = CreateContext("rhx-switch");
        var output = CreateOutput("rhx-switch");

        helper.ViewContext = CreateViewContext();
        helper.Process(context, output);

        Assert.True(HasClass(output, "rhx-switch--disabled"));
        var content = output.Content.GetContent();
        Assert.Contains(" disabled", content);
    }

    [Fact]
    public void Required_Sets_Aria()
    {
        var helper = CreateHelper();
        helper.Required = true;
        var context = CreateContext("rhx-switch");
        var output = CreateOutput("rhx-switch");

        helper.ViewContext = CreateViewContext();
        helper.Process(context, output);

        var content = output.Content.GetContent();
        Assert.Contains("aria-required=\"true\"", content);
    }

    // ── Size ──

    [Fact]
    public void Small_Size_Adds_Modifier()
    {
        var helper = CreateHelper();
        helper.Size = "small";
        var context = CreateContext("rhx-switch");
        var output = CreateOutput("rhx-switch");

        helper.ViewContext = CreateViewContext();
        helper.Process(context, output);

        Assert.True(HasClass(output, "rhx-switch--small"));
    }

    [Fact]
    public void Large_Size_Adds_Modifier()
    {
        var helper = CreateHelper();
        helper.Size = "large";
        var context = CreateContext("rhx-switch");
        var output = CreateOutput("rhx-switch");

        helper.ViewContext = CreateViewContext();
        helper.Process(context, output);

        Assert.True(HasClass(output, "rhx-switch--large"));
    }

    // ── Model binding ──

    [Fact]
    public void For_Bool_True_Sets_Checked()
    {
        var helper = CreateHelper();
        helper.For = CreateModelExpressionFor("DarkMode", true);
        var context = CreateContext("rhx-switch");
        var output = CreateOutput("rhx-switch");

        helper.ViewContext = CreateViewContext();
        helper.Process(context, output);

        var content = output.Content.GetContent();
        Assert.Contains(" checked", content);
        Assert.Contains("aria-checked=\"true\"", content);
        Assert.Contains("name=\"DarkMode\"", content);
    }

    // ── htmx ──

    [Fact]
    public void Htmx_On_Native_Input()
    {
        var helper = CreateHelper();
        helper.HxPost = "/settings";
        helper.HxTrigger = "change";
        var context = CreateContext("rhx-switch");
        var output = CreateOutput("rhx-switch");

        helper.ViewContext = CreateViewContext();
        helper.Process(context, output);

        var content = output.Content.GetContent();
        Assert.Contains("hx-post=\"/settings\"", content);
        Assert.Contains("hx-trigger=\"change\"", content);
    }

    // ── CSS class merging ──

    [Fact]
    public void Custom_Css_Class_Merged()
    {
        var helper = CreateHelper();
        helper.CssClass = "my-switch";
        var context = CreateContext("rhx-switch");
        var output = CreateOutput("rhx-switch");

        helper.ViewContext = CreateViewContext();
        helper.Process(context, output);

        Assert.True(HasClass(output, "rhx-switch"));
        Assert.True(HasClass(output, "my-switch"));
    }

    // ── ARIA label ──

    [Fact]
    public void Aria_Label_Sets_Attribute()
    {
        var helper = CreateHelper();
        helper.AriaLabel = "Enable dark mode";
        var context = CreateContext("rhx-switch");
        var output = CreateOutput("rhx-switch");

        helper.ViewContext = CreateViewContext();
        helper.Process(context, output);

        var content = output.Content.GetContent();
        Assert.Contains("aria-label=\"Enable dark mode\"", content);
    }
}
