using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using RazorHX.Components.Forms;
using Xunit;

namespace RazorHX.Tests;

public class ColorPickerTagHelperTests : TagHelperTestBase
{
    private ColorPickerTagHelper CreateHelper() => new(CreateUrlHelperFactory());

    // ── Test model ──

    private class TestModel
    {
        public string ThemeColor { get; set; } = "#3b82f6";
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
        var context = CreateContext("rhx-color-picker");
        var output = CreateOutput("rhx-color-picker");

        helper.ViewContext = CreateViewContext();
        helper.Process(context, output);

        Assert.Equal("div", output.TagName);
    }

    [Fact]
    public void Has_Block_Class()
    {
        var helper = CreateHelper();
        var context = CreateContext("rhx-color-picker");
        var output = CreateOutput("rhx-color-picker");

        helper.ViewContext = CreateViewContext();
        helper.Process(context, output);

        Assert.True(HasClass(output, "rhx-color-picker"));
    }

    [Fact]
    public void Has_Data_Attribute()
    {
        var helper = CreateHelper();
        var context = CreateContext("rhx-color-picker");
        var output = CreateOutput("rhx-color-picker");

        helper.ViewContext = CreateViewContext();
        helper.Process(context, output);

        AssertAttribute(output, "data-rhx-color-picker", "");
    }

    [Fact]
    public void Has_Format_Data_Attribute()
    {
        var helper = CreateHelper();
        helper.Format = "rgb";
        var context = CreateContext("rhx-color-picker");
        var output = CreateOutput("rhx-color-picker");

        helper.ViewContext = CreateViewContext();
        helper.Process(context, output);

        AssertAttribute(output, "data-rhx-format", "rgb");
    }

    // ── Trigger ──

    [Fact]
    public void Has_Trigger_Button()
    {
        var helper = CreateHelper();
        var context = CreateContext("rhx-color-picker");
        var output = CreateOutput("rhx-color-picker");

        helper.ViewContext = CreateViewContext();
        helper.Process(context, output);

        var content = output.Content.GetContent();
        Assert.Contains("rhx-color-picker__trigger", content);
        Assert.Contains("<button", content);
    }

    [Fact]
    public void Trigger_Has_Swatch()
    {
        var helper = CreateHelper();
        helper.Value = "#ff0000";
        var context = CreateContext("rhx-color-picker");
        var output = CreateOutput("rhx-color-picker");

        helper.ViewContext = CreateViewContext();
        helper.Process(context, output);

        var content = output.Content.GetContent();
        Assert.Contains("rhx-color-picker__swatch", content);
        Assert.Contains("background-color: #ff0000", content);
    }

    [Fact]
    public void Trigger_Has_Text()
    {
        var helper = CreateHelper();
        helper.Value = "#ff0000";
        var context = CreateContext("rhx-color-picker");
        var output = CreateOutput("rhx-color-picker");

        helper.ViewContext = CreateViewContext();
        helper.Process(context, output);

        var content = output.Content.GetContent();
        Assert.Contains("rhx-color-picker__text", content);
        Assert.Contains("#ff0000", content);
    }

    [Fact]
    public void Inline_No_Trigger()
    {
        var helper = CreateHelper();
        helper.Inline = true;
        var context = CreateContext("rhx-color-picker");
        var output = CreateOutput("rhx-color-picker");

        helper.ViewContext = CreateViewContext();
        helper.Process(context, output);

        var content = output.Content.GetContent();
        Assert.DoesNotContain("rhx-color-picker__trigger", content);
        Assert.True(HasClass(output, "rhx-color-picker--inline"));
        AssertAttribute(output, "data-rhx-inline", "");
    }

    // ── Panel ──

    [Fact]
    public void Has_Panel()
    {
        var helper = CreateHelper();
        var context = CreateContext("rhx-color-picker");
        var output = CreateOutput("rhx-color-picker");

        helper.ViewContext = CreateViewContext();
        helper.Process(context, output);

        var content = output.Content.GetContent();
        Assert.Contains("rhx-color-picker__panel", content);
    }

    [Fact]
    public void Panel_Hidden_By_Default()
    {
        var helper = CreateHelper();
        var context = CreateContext("rhx-color-picker");
        var output = CreateOutput("rhx-color-picker");

        helper.ViewContext = CreateViewContext();
        helper.Process(context, output);

        var content = output.Content.GetContent();
        // Panel div has hidden attribute
        Assert.Contains("rhx-color-picker__panel\" hidden", content);
    }

    [Fact]
    public void Inline_Panel_Not_Hidden()
    {
        var helper = CreateHelper();
        helper.Inline = true;
        var context = CreateContext("rhx-color-picker");
        var output = CreateOutput("rhx-color-picker");

        helper.ViewContext = CreateViewContext();
        helper.Process(context, output);

        var content = output.Content.GetContent();
        Assert.Contains("rhx-color-picker__panel\">", content);
    }

    // ── Saturation area ──

    [Fact]
    public void Has_Saturation_Area()
    {
        var helper = CreateHelper();
        var context = CreateContext("rhx-color-picker");
        var output = CreateOutput("rhx-color-picker");

        helper.ViewContext = CreateViewContext();
        helper.Process(context, output);

        var content = output.Content.GetContent();
        Assert.Contains("rhx-color-picker__saturation", content);
        Assert.Contains("rhx-color-picker__saturation-cursor", content);
    }

    // ── Hue slider ──

    [Fact]
    public void Has_Hue_Slider()
    {
        var helper = CreateHelper();
        var context = CreateContext("rhx-color-picker");
        var output = CreateOutput("rhx-color-picker");

        helper.ViewContext = CreateViewContext();
        helper.Process(context, output);

        var content = output.Content.GetContent();
        Assert.Contains("rhx-color-picker__hue", content);
        Assert.Contains("rhx-color-picker__hue-input", content);
        Assert.Contains("aria-label=\"Hue\"", content);
    }

    // ── Opacity slider ──

    [Fact]
    public void No_Opacity_Slider_By_Default()
    {
        var helper = CreateHelper();
        var context = CreateContext("rhx-color-picker");
        var output = CreateOutput("rhx-color-picker");

        helper.ViewContext = CreateViewContext();
        helper.Process(context, output);

        var content = output.Content.GetContent();
        Assert.DoesNotContain("rhx-color-picker__opacity", content);
    }

    [Fact]
    public void Opacity_Slider_When_Enabled()
    {
        var helper = CreateHelper();
        helper.Opacity = true;
        var context = CreateContext("rhx-color-picker");
        var output = CreateOutput("rhx-color-picker");

        helper.ViewContext = CreateViewContext();
        helper.Process(context, output);

        var content = output.Content.GetContent();
        Assert.Contains("rhx-color-picker__opacity", content);
        Assert.Contains("rhx-color-picker__opacity-input", content);
        Assert.Contains("aria-label=\"Opacity\"", content);
        AssertAttribute(output, "data-rhx-opacity", "");
    }

    // ── Color text input ──

    [Fact]
    public void Has_Text_Input()
    {
        var helper = CreateHelper();
        helper.Value = "#3b82f6";
        var context = CreateContext("rhx-color-picker");
        var output = CreateOutput("rhx-color-picker");

        helper.ViewContext = CreateViewContext();
        helper.Process(context, output);

        var content = output.Content.GetContent();
        Assert.Contains("rhx-color-picker__input", content);
        Assert.Contains("type=\"text\"", content);
        Assert.Contains("value=\"#3b82f6\"", content);
    }

    // ── Swatches ──

    [Fact]
    public void No_Swatches_By_Default()
    {
        var helper = CreateHelper();
        var context = CreateContext("rhx-color-picker");
        var output = CreateOutput("rhx-color-picker");

        helper.ViewContext = CreateViewContext();
        helper.Process(context, output);

        var content = output.Content.GetContent();
        Assert.DoesNotContain("rhx-color-picker__swatches", content);
    }

    [Fact]
    public void Swatches_Render_Presets()
    {
        var helper = CreateHelper();
        helper.Swatches = "#ff0000,#00ff00,#0000ff";
        var context = CreateContext("rhx-color-picker");
        var output = CreateOutput("rhx-color-picker");

        helper.ViewContext = CreateViewContext();
        helper.Process(context, output);

        var content = output.Content.GetContent();
        Assert.Contains("rhx-color-picker__swatches", content);
        Assert.Contains("data-color=\"#ff0000\"", content);
        Assert.Contains("data-color=\"#00ff00\"", content);
        Assert.Contains("data-color=\"#0000ff\"", content);
    }

    // ── Hidden input ──

    [Fact]
    public void Has_Hidden_Input()
    {
        var helper = CreateHelper();
        helper.Name = "color";
        helper.Value = "#3b82f6";
        var context = CreateContext("rhx-color-picker");
        var output = CreateOutput("rhx-color-picker");

        helper.ViewContext = CreateViewContext();
        helper.Process(context, output);

        var content = output.Content.GetContent();
        Assert.Contains("type=\"hidden\"", content);
        Assert.Contains("rhx-color-picker__value", content);
        Assert.Contains("name=\"color\"", content);
        Assert.Contains("value=\"#3b82f6\"", content);
    }

    // ── Label ──

    [Fact]
    public void Label_Renders()
    {
        var helper = CreateHelper();
        helper.Label = "Brand Color";
        helper.Name = "color";
        var context = CreateContext("rhx-color-picker");
        var output = CreateOutput("rhx-color-picker");

        helper.ViewContext = CreateViewContext();
        helper.Process(context, output);

        var content = output.Content.GetContent();
        Assert.Contains("<label", content);
        Assert.Contains("Brand Color", content);
    }

    // ── Error ──

    [Fact]
    public void Error_Adds_Modifier()
    {
        var helper = CreateHelper();
        helper.Name = "color";
        var context = CreateContext("rhx-color-picker");
        var output = CreateOutput("rhx-color-picker");

        var viewContext = CreateViewContext();
        viewContext.ModelState.AddModelError("color", "Invalid color");
        helper.ViewContext = viewContext;
        helper.Process(context, output);

        Assert.True(HasClass(output, "rhx-color-picker--error"));
    }

    // ── Disabled ──

    [Fact]
    public void Disabled_Adds_Modifier()
    {
        var helper = CreateHelper();
        helper.Disabled = true;
        var context = CreateContext("rhx-color-picker");
        var output = CreateOutput("rhx-color-picker");

        helper.ViewContext = CreateViewContext();
        helper.Process(context, output);

        Assert.True(HasClass(output, "rhx-color-picker--disabled"));
        var content = output.Content.GetContent();
        Assert.Contains(" disabled", content);
    }

    // ── Size ──

    [Fact]
    public void Small_Size_Adds_Modifier()
    {
        var helper = CreateHelper();
        helper.Size = "small";
        var context = CreateContext("rhx-color-picker");
        var output = CreateOutput("rhx-color-picker");

        helper.ViewContext = CreateViewContext();
        helper.Process(context, output);

        Assert.True(HasClass(output, "rhx-color-picker--small"));
    }

    // ── Model binding ──

    [Fact]
    public void For_Resolves_Name_And_Value()
    {
        var helper = CreateHelper();
        helper.For = CreateModelExpressionFor("ThemeColor", "#3b82f6");
        var context = CreateContext("rhx-color-picker");
        var output = CreateOutput("rhx-color-picker");

        helper.ViewContext = CreateViewContext();
        helper.Process(context, output);

        var content = output.Content.GetContent();
        Assert.Contains("name=\"ThemeColor\"", content);
        Assert.Contains("value=\"#3b82f6\"", content);
    }

    // ── htmx ──

    [Fact]
    public void Htmx_On_Hidden_Input()
    {
        var helper = CreateHelper();
        helper.HxPost = "/color";
        helper.HxTrigger = "change";
        var context = CreateContext("rhx-color-picker");
        var output = CreateOutput("rhx-color-picker");

        helper.ViewContext = CreateViewContext();
        helper.Process(context, output);

        var content = output.Content.GetContent();
        Assert.Contains("hx-post=\"/color\"", content);
        Assert.Contains("hx-trigger=\"change\"", content);
    }

    // ── CSS class merging ──

    [Fact]
    public void Custom_Css_Class_Merged()
    {
        var helper = CreateHelper();
        helper.CssClass = "my-picker";
        var context = CreateContext("rhx-color-picker");
        var output = CreateOutput("rhx-color-picker");

        helper.ViewContext = CreateViewContext();
        helper.Process(context, output);

        Assert.True(HasClass(output, "rhx-color-picker"));
        Assert.True(HasClass(output, "my-picker"));
    }

    // ── Default value ──

    [Fact]
    public void Default_Value_Is_Black()
    {
        var helper = CreateHelper();
        helper.Name = "color";
        var context = CreateContext("rhx-color-picker");
        var output = CreateOutput("rhx-color-picker");

        helper.ViewContext = CreateViewContext();
        helper.Process(context, output);

        var content = output.Content.GetContent();
        Assert.Contains("value=\"#000000\"", content);
    }
}
