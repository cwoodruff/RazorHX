using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using htmxRazor.Components.Forms;
using Xunit;

namespace htmxRazor.Tests;

public class RadioGroupTagHelperTests : TagHelperTestBase
{
    private RadioGroupTagHelper CreateHelper() => new(CreateUrlHelperFactory());

    // ── Test model ──

    private enum Priority
    {
        [Display(Name = "Low Priority")]
        Low,
        [Display(Name = "Medium Priority")]
        Medium,
        [Display(Name = "High Priority")]
        High,
        Critical
    }

    private class TestModel
    {
        public Priority SelectedPriority { get; set; }
        public string? Size { get; set; }
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
    public async Task Renders_Fieldset_Element()
    {
        var helper = CreateHelper();
        var context = CreateContext("rhx-radio-group");
        var output = CreateOutput("rhx-radio-group");

        helper.ViewContext = CreateViewContext();
        await helper.ProcessAsync(context, output);

        Assert.Equal("fieldset", output.TagName);
    }

    [Fact]
    public async Task Has_Block_Class()
    {
        var helper = CreateHelper();
        var context = CreateContext("rhx-radio-group");
        var output = CreateOutput("rhx-radio-group");

        helper.ViewContext = CreateViewContext();
        await helper.ProcessAsync(context, output);

        Assert.True(HasClass(output, "rhx-radio-group"));
    }

    [Fact]
    public async Task Has_Radiogroup_Role()
    {
        var helper = CreateHelper();
        var context = CreateContext("rhx-radio-group");
        var output = CreateOutput("rhx-radio-group");

        helper.ViewContext = CreateViewContext();
        await helper.ProcessAsync(context, output);

        AssertAttribute(output, "role", "radiogroup");
    }

    [Fact]
    public async Task Has_Data_Attribute()
    {
        var helper = CreateHelper();
        var context = CreateContext("rhx-radio-group");
        var output = CreateOutput("rhx-radio-group");

        helper.ViewContext = CreateViewContext();
        await helper.ProcessAsync(context, output);

        AssertAttribute(output, "data-rhx-radio-group", "");
    }

    // ── Legend ──

    [Fact]
    public async Task Has_Legend()
    {
        var helper = CreateHelper();
        helper.Label = "Priority";
        var context = CreateContext("rhx-radio-group");
        var output = CreateOutput("rhx-radio-group");

        helper.ViewContext = CreateViewContext();
        await helper.ProcessAsync(context, output);

        var content = output.Content.GetContent();
        Assert.Contains("<legend", content);
        Assert.Contains("rhx-radio-group__legend", content);
        Assert.Contains("Priority", content);
    }

    // ── Items container ──

    [Fact]
    public async Task Has_Items_Container()
    {
        var helper = CreateHelper();
        var context = CreateContext("rhx-radio-group");
        var output = CreateOutput("rhx-radio-group");

        helper.ViewContext = CreateViewContext();
        await helper.ProcessAsync(context, output);

        var content = output.Content.GetContent();
        Assert.Contains("rhx-radio-group__items", content);
    }

    // ── Items binding ──

    [Fact]
    public async Task Items_Generate_Radios()
    {
        var helper = CreateHelper();
        helper.Name = "size";
        helper.Items = new List<SelectListItem>
        {
            new("Small", "sm"),
            new("Medium", "md"),
            new("Large", "lg")
        };
        var context = CreateContext("rhx-radio-group");
        var output = CreateOutput("rhx-radio-group");

        helper.ViewContext = CreateViewContext();
        await helper.ProcessAsync(context, output);

        var content = output.Content.GetContent();
        Assert.Contains("value=\"sm\"", content);
        Assert.Contains("Small", content);
        Assert.Contains("value=\"md\"", content);
        Assert.Contains("Medium", content);
        Assert.Contains("value=\"lg\"", content);
        Assert.Contains("Large", content);
        Assert.Contains("type=\"radio\"", content);
    }

    [Fact]
    public async Task Items_Selected_Radio()
    {
        var helper = CreateHelper();
        helper.Name = "size";
        helper.Value = "md";
        helper.Items = new List<SelectListItem>
        {
            new("Small", "sm"),
            new("Medium", "md"),
            new("Large", "lg")
        };
        var context = CreateContext("rhx-radio-group");
        var output = CreateOutput("rhx-radio-group");

        helper.ViewContext = CreateViewContext();
        await helper.ProcessAsync(context, output);

        var content = output.Content.GetContent();
        Assert.Contains("value=\"md\" checked", content);
    }

    [Fact]
    public async Task Items_Disabled_Radio()
    {
        var helper = CreateHelper();
        helper.Name = "size";
        helper.Items = new List<SelectListItem>
        {
            new("Small", "sm"),
            new SelectListItem { Text = "Medium", Value = "md", Disabled = true }
        };
        var context = CreateContext("rhx-radio-group");
        var output = CreateOutput("rhx-radio-group");

        helper.ViewContext = CreateViewContext();
        await helper.ProcessAsync(context, output);

        var content = output.Content.GetContent();
        Assert.Contains("rhx-radio--disabled", content);
    }

    // ── Enum auto-generation ──

    [Fact]
    public async Task Enum_Auto_Generates_Radios()
    {
        var helper = CreateHelper();
        helper.For = CreateModelExpressionFor("SelectedPriority", Priority.Medium);
        var context = CreateContext("rhx-radio-group");
        var output = CreateOutput("rhx-radio-group");

        helper.ViewContext = CreateViewContext();
        await helper.ProcessAsync(context, output);

        var content = output.Content.GetContent();
        Assert.Contains("Low Priority", content);
        Assert.Contains("value=\"Low\"", content);
        Assert.Contains("Medium Priority", content);
        Assert.Contains("value=\"Medium\"", content);
        Assert.Contains("High Priority", content);
        Assert.Contains("value=\"High\"", content);
        Assert.Contains("Critical", content);
        Assert.Contains("value=\"Critical\"", content);
    }

    [Fact]
    public async Task Enum_Selected_Radio()
    {
        var helper = CreateHelper();
        helper.For = CreateModelExpressionFor("SelectedPriority", Priority.Medium);
        var context = CreateContext("rhx-radio-group");
        var output = CreateOutput("rhx-radio-group");

        helper.ViewContext = CreateViewContext();
        await helper.ProcessAsync(context, output);

        var content = output.Content.GetContent();
        Assert.Contains("value=\"Medium\" checked", content);
    }

    // ── Child content ──

    [Fact]
    public async Task Child_Content_Renders()
    {
        var helper = CreateHelper();
        helper.Name = "color";
        var context = CreateContext("rhx-radio-group");
        var output = CreateOutput("rhx-radio-group", childContent: "<span>child radios</span>");

        helper.ViewContext = CreateViewContext();
        await helper.ProcessAsync(context, output);

        var content = output.Content.GetContent();
        Assert.Contains("child radios", content);
    }

    // ── Inline ──

    [Fact]
    public async Task Inline_Adds_Modifier()
    {
        var helper = CreateHelper();
        helper.Inline = true;
        var context = CreateContext("rhx-radio-group");
        var output = CreateOutput("rhx-radio-group");

        helper.ViewContext = CreateViewContext();
        await helper.ProcessAsync(context, output);

        Assert.True(HasClass(output, "rhx-radio-group--inline"));
    }

    // ── States ──

    [Fact]
    public async Task Disabled_Adds_Modifier()
    {
        var helper = CreateHelper();
        helper.Disabled = true;
        var context = CreateContext("rhx-radio-group");
        var output = CreateOutput("rhx-radio-group");

        helper.ViewContext = CreateViewContext();
        await helper.ProcessAsync(context, output);

        Assert.True(HasClass(output, "rhx-radio-group--disabled"));
    }

    [Fact]
    public async Task Error_Adds_Modifier_And_Message()
    {
        var helper = CreateHelper();
        helper.Name = "priority";
        var context = CreateContext("rhx-radio-group");
        var output = CreateOutput("rhx-radio-group");

        var viewContext = CreateViewContext();
        viewContext.ModelState.AddModelError("priority", "Please select a priority");
        helper.ViewContext = viewContext;
        await helper.ProcessAsync(context, output);

        Assert.True(HasClass(output, "rhx-radio-group--error"));
        var content = output.Content.GetContent();
        Assert.Contains("Please select a priority", content);
        AssertAttribute(output, "aria-invalid", "true");
    }

    [Fact]
    public async Task Required_Sets_Aria()
    {
        var helper = CreateHelper();
        helper.Required = true;
        var context = CreateContext("rhx-radio-group");
        var output = CreateOutput("rhx-radio-group");

        helper.ViewContext = CreateViewContext();
        await helper.ProcessAsync(context, output);

        AssertAttribute(output, "aria-required", "true");
    }

    // ── Hint ──

    [Fact]
    public async Task Hint_Renders()
    {
        var helper = CreateHelper();
        helper.Hint = "Select one option";
        helper.Name = "priority";
        var context = CreateContext("rhx-radio-group");
        var output = CreateOutput("rhx-radio-group");

        helper.ViewContext = CreateViewContext();
        await helper.ProcessAsync(context, output);

        var content = output.Content.GetContent();
        Assert.Contains("rhx-radio-group__hint", content);
        Assert.Contains("Select one option", content);
    }

    // ── Size ──

    [Fact]
    public async Task Small_Size_Adds_Modifier()
    {
        var helper = CreateHelper();
        helper.Size = "small";
        var context = CreateContext("rhx-radio-group");
        var output = CreateOutput("rhx-radio-group");

        helper.ViewContext = CreateViewContext();
        await helper.ProcessAsync(context, output);

        Assert.True(HasClass(output, "rhx-radio-group--small"));
    }

    // ── htmx on fieldset ──

    [Fact]
    public async Task Htmx_On_Fieldset()
    {
        var helper = CreateHelper();
        helper.HxPost = "/update-priority";
        helper.HxTrigger = "change";
        var context = CreateContext("rhx-radio-group");
        var output = CreateOutput("rhx-radio-group");

        helper.ViewContext = CreateViewContext();
        await helper.ProcessAsync(context, output);

        AssertAttribute(output, "hx-post", "/update-priority");
        AssertAttribute(output, "hx-trigger", "change");
    }

    // ── CSS class merging ──

    [Fact]
    public async Task Custom_Css_Class_Merged()
    {
        var helper = CreateHelper();
        helper.CssClass = "my-radio-group";
        var context = CreateContext("rhx-radio-group");
        var output = CreateOutput("rhx-radio-group");

        helper.ViewContext = CreateViewContext();
        await helper.ProcessAsync(context, output);

        Assert.True(HasClass(output, "rhx-radio-group"));
        Assert.True(HasClass(output, "my-radio-group"));
    }

    // ── ARIA label ──

    [Fact]
    public async Task Aria_Label_Sets_Attribute()
    {
        var helper = CreateHelper();
        helper.AriaLabel = "Select priority";
        var context = CreateContext("rhx-radio-group");
        var output = CreateOutput("rhx-radio-group");

        helper.ViewContext = CreateViewContext();
        await helper.ProcessAsync(context, output);

        AssertAttribute(output, "aria-label", "Select priority");
    }
}
