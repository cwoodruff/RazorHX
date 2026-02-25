using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using htmxRazor.Components.Forms;
using Xunit;

namespace htmxRazor.Tests;

public class RatingTagHelperTests : TagHelperTestBase
{
    private RatingTagHelper CreateHelper() => new(CreateUrlHelperFactory());

    // ── Test model ──

    private class TestModel
    {
        public int Score { get; set; }
        public double Rating { get; set; }
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
        var context = CreateContext("rhx-rating");
        var output = CreateOutput("rhx-rating");

        helper.ViewContext = CreateViewContext();
        helper.Process(context, output);

        Assert.Equal("div", output.TagName);
    }

    [Fact]
    public void Has_Block_Class()
    {
        var helper = CreateHelper();
        var context = CreateContext("rhx-rating");
        var output = CreateOutput("rhx-rating");

        helper.ViewContext = CreateViewContext();
        helper.Process(context, output);

        Assert.True(HasClass(output, "rhx-rating"));
    }

    [Fact]
    public void Has_Data_Attribute()
    {
        var helper = CreateHelper();
        var context = CreateContext("rhx-rating");
        var output = CreateOutput("rhx-rating");

        helper.ViewContext = CreateViewContext();
        helper.Process(context, output);

        AssertAttribute(output, "data-rhx-rating", "");
    }

    [Fact]
    public void Has_Precision_Data_Attribute()
    {
        var helper = CreateHelper();
        helper.Precision = 0.5;
        var context = CreateContext("rhx-rating");
        var output = CreateOutput("rhx-rating");

        helper.ViewContext = CreateViewContext();
        helper.Process(context, output);

        AssertAttribute(output, "data-rhx-precision", "0.5");
    }

    [Fact]
    public void Has_Max_Data_Attribute()
    {
        var helper = CreateHelper();
        helper.Max = 10;
        var context = CreateContext("rhx-rating");
        var output = CreateOutput("rhx-rating");

        helper.ViewContext = CreateViewContext();
        helper.Process(context, output);

        AssertAttribute(output, "data-rhx-max", "10");
    }

    // ── Stars ──

    [Fact]
    public void Renders_Default_5_Stars()
    {
        var helper = CreateHelper();
        var context = CreateContext("rhx-rating");
        var output = CreateOutput("rhx-rating");

        helper.ViewContext = CreateViewContext();
        helper.Process(context, output);

        var content = output.Content.GetContent();
        Assert.Contains("rhx-rating__stars", content);
        // Count star spans
        var count = content.Split("rhx-rating__star\"").Length - 1
                  + content.Split("rhx-rating__star--filled").Length - 1
                  + content.Split("rhx-rating__star--half").Length - 1;
        // Should have star elements with data-value
        Assert.Contains("data-value=\"1\"", content);
        Assert.Contains("data-value=\"5\"", content);
    }

    [Fact]
    public void Custom_Max_Stars()
    {
        var helper = CreateHelper();
        helper.Max = 10;
        var context = CreateContext("rhx-rating");
        var output = CreateOutput("rhx-rating");

        helper.ViewContext = CreateViewContext();
        helper.Process(context, output);

        var content = output.Content.GetContent();
        Assert.Contains("data-value=\"10\"", content);
    }

    [Fact]
    public void Stars_Have_SVG()
    {
        var helper = CreateHelper();
        var context = CreateContext("rhx-rating");
        var output = CreateOutput("rhx-rating");

        helper.ViewContext = CreateViewContext();
        helper.Process(context, output);

        var content = output.Content.GetContent();
        Assert.Contains("<polygon", content);
    }

    [Fact]
    public void Filled_Stars_Have_Modifier()
    {
        var helper = CreateHelper();
        helper.Value = "3";
        var context = CreateContext("rhx-rating");
        var output = CreateOutput("rhx-rating");

        helper.ViewContext = CreateViewContext();
        helper.Process(context, output);

        var content = output.Content.GetContent();
        Assert.Contains("rhx-rating__star--filled", content);
    }

    [Fact]
    public void Half_Star_With_Precision()
    {
        var helper = CreateHelper();
        helper.Value = "2.5";
        helper.Precision = 0.5;
        var context = CreateContext("rhx-rating");
        var output = CreateOutput("rhx-rating");

        helper.ViewContext = CreateViewContext();
        helper.Process(context, output);

        var content = output.Content.GetContent();
        Assert.Contains("rhx-rating__star--half", content);
    }

    // ── ARIA (slider role) ──

    [Fact]
    public void Has_Slider_Role()
    {
        var helper = CreateHelper();
        var context = CreateContext("rhx-rating");
        var output = CreateOutput("rhx-rating");

        helper.ViewContext = CreateViewContext();
        helper.Process(context, output);

        AssertAttribute(output, "role", "slider");
    }

    [Fact]
    public void Has_Aria_Valuemin_Max_Now()
    {
        var helper = CreateHelper();
        helper.Value = "3";
        helper.Max = 5;
        var context = CreateContext("rhx-rating");
        var output = CreateOutput("rhx-rating");

        helper.ViewContext = CreateViewContext();
        helper.Process(context, output);

        AssertAttribute(output, "aria-valuemin", "0");
        AssertAttribute(output, "aria-valuemax", "5");
        AssertAttribute(output, "aria-valuenow", "3");
    }

    [Fact]
    public void Has_Tabindex()
    {
        var helper = CreateHelper();
        var context = CreateContext("rhx-rating");
        var output = CreateOutput("rhx-rating");

        helper.ViewContext = CreateViewContext();
        helper.Process(context, output);

        AssertAttribute(output, "tabindex", "0");
    }

    // ── Readonly ──

    [Fact]
    public void Readonly_No_Slider_Role()
    {
        var helper = CreateHelper();
        helper.Readonly = true;
        var context = CreateContext("rhx-rating");
        var output = CreateOutput("rhx-rating");

        helper.ViewContext = CreateViewContext();
        helper.Process(context, output);

        Assert.True(HasClass(output, "rhx-rating--readonly"));
        AssertNoAttribute(output, "role");
        AssertNoAttribute(output, "tabindex");
    }

    // ── Hidden input ──

    [Fact]
    public void Has_Hidden_Input()
    {
        var helper = CreateHelper();
        helper.Name = "rating";
        var context = CreateContext("rhx-rating");
        var output = CreateOutput("rhx-rating");

        helper.ViewContext = CreateViewContext();
        helper.Process(context, output);

        var content = output.Content.GetContent();
        Assert.Contains("type=\"hidden\"", content);
        Assert.Contains("rhx-rating__value", content);
        Assert.Contains("name=\"rating\"", content);
    }

    // ── Label ──

    [Fact]
    public void Label_Renders()
    {
        var helper = CreateHelper();
        helper.Label = "Rating";
        helper.Name = "rating";
        var context = CreateContext("rhx-rating");
        var output = CreateOutput("rhx-rating");

        helper.ViewContext = CreateViewContext();
        helper.Process(context, output);

        var content = output.Content.GetContent();
        Assert.Contains("<label", content);
        Assert.Contains("Rating", content);
    }

    // ── Error ──

    [Fact]
    public void Error_Adds_Modifier()
    {
        var helper = CreateHelper();
        helper.Name = "rating";
        var context = CreateContext("rhx-rating");
        var output = CreateOutput("rhx-rating");

        var viewContext = CreateViewContext();
        viewContext.ModelState.AddModelError("rating", "Rating is required");
        helper.ViewContext = viewContext;
        helper.Process(context, output);

        Assert.True(HasClass(output, "rhx-rating--error"));
    }

    // ── Disabled ──

    [Fact]
    public void Disabled_Adds_Modifier()
    {
        var helper = CreateHelper();
        helper.Disabled = true;
        var context = CreateContext("rhx-rating");
        var output = CreateOutput("rhx-rating");

        helper.ViewContext = CreateViewContext();
        helper.Process(context, output);

        Assert.True(HasClass(output, "rhx-rating--disabled"));
        AssertNoAttribute(output, "role");
    }

    // ── Size ──

    [Fact]
    public void Small_Size_Adds_Modifier()
    {
        var helper = CreateHelper();
        helper.Size = "small";
        var context = CreateContext("rhx-rating");
        var output = CreateOutput("rhx-rating");

        helper.ViewContext = CreateViewContext();
        helper.Process(context, output);

        Assert.True(HasClass(output, "rhx-rating--small"));
    }

    [Fact]
    public void Large_Size_Adds_Modifier()
    {
        var helper = CreateHelper();
        helper.Size = "large";
        var context = CreateContext("rhx-rating");
        var output = CreateOutput("rhx-rating");

        helper.ViewContext = CreateViewContext();
        helper.Process(context, output);

        Assert.True(HasClass(output, "rhx-rating--large"));
    }

    // ── Model binding ──

    [Fact]
    public void For_Resolves_Name_And_Value()
    {
        var helper = CreateHelper();
        helper.For = CreateModelExpressionFor("Score", 4);
        var context = CreateContext("rhx-rating");
        var output = CreateOutput("rhx-rating");

        helper.ViewContext = CreateViewContext();
        helper.Process(context, output);

        var content = output.Content.GetContent();
        Assert.Contains("name=\"Score\"", content);
        Assert.Contains("value=\"4\"", content);
    }

    // ── htmx ──

    [Fact]
    public void Htmx_On_Hidden_Input()
    {
        var helper = CreateHelper();
        helper.HxPost = "/rate";
        helper.HxTrigger = "change";
        var context = CreateContext("rhx-rating");
        var output = CreateOutput("rhx-rating");

        helper.ViewContext = CreateViewContext();
        helper.Process(context, output);

        var content = output.Content.GetContent();
        Assert.Contains("hx-post=\"/rate\"", content);
        Assert.Contains("hx-trigger=\"change\"", content);
    }

    // ── CSS class merging ──

    [Fact]
    public void Custom_Css_Class_Merged()
    {
        var helper = CreateHelper();
        helper.CssClass = "my-rating";
        var context = CreateContext("rhx-rating");
        var output = CreateOutput("rhx-rating");

        helper.ViewContext = CreateViewContext();
        helper.Process(context, output);

        Assert.True(HasClass(output, "rhx-rating"));
        Assert.True(HasClass(output, "my-rating"));
    }

    // ── ARIA label ──

    [Fact]
    public void Aria_Label_Sets_Attribute()
    {
        var helper = CreateHelper();
        helper.AriaLabel = "Product rating";
        var context = CreateContext("rhx-rating");
        var output = CreateOutput("rhx-rating");

        helper.ViewContext = CreateViewContext();
        helper.Process(context, output);

        var content = output.Content.GetContent();
        Assert.Contains("aria-label=\"Product rating\"", content);
    }
}
