using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using htmxRazor.Components.Forms;
using Xunit;

namespace htmxRazor.Tests;

public class ComboboxTagHelperTests : TagHelperTestBase
{
    private ComboboxTagHelper CreateHelper() => new(CreateUrlHelperFactory());

    // ── Test model ──

    private enum Status
    {
        [Display(Name = "To Do")]
        Todo,
        [Display(Name = "In Progress")]
        InProgress,
        Done
    }

    private class TestModel
    {
        public Status Status { get; set; }
        public string? City { get; set; }
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
    public async Task Renders_Div_Element()
    {
        var helper = CreateHelper();
        var context = CreateContext("rhx-combobox");
        var output = CreateOutput("rhx-combobox");

        helper.ViewContext = CreateViewContext();
        await helper.ProcessAsync(context, output);

        Assert.Equal("div", output.TagName);
    }

    [Fact]
    public async Task Has_Block_Class()
    {
        var helper = CreateHelper();
        var context = CreateContext("rhx-combobox");
        var output = CreateOutput("rhx-combobox");

        helper.ViewContext = CreateViewContext();
        await helper.ProcessAsync(context, output);

        Assert.True(HasClass(output, "rhx-combobox"));
    }

    [Fact]
    public async Task Has_Data_Attribute()
    {
        var helper = CreateHelper();
        var context = CreateContext("rhx-combobox");
        var output = CreateOutput("rhx-combobox");

        helper.ViewContext = CreateViewContext();
        await helper.ProcessAsync(context, output);

        AssertAttribute(output, "data-rhx-combobox", "");
    }

    // ── Text input ──

    [Fact]
    public async Task Has_Text_Input()
    {
        var helper = CreateHelper();
        var context = CreateContext("rhx-combobox");
        var output = CreateOutput("rhx-combobox");

        helper.ViewContext = CreateViewContext();
        await helper.ProcessAsync(context, output);

        var content = output.Content.GetContent();
        Assert.Contains("rhx-combobox__input", content);
        Assert.Contains("type=\"text\"", content);
    }

    [Fact]
    public async Task Input_Has_Combobox_Role()
    {
        var helper = CreateHelper();
        var context = CreateContext("rhx-combobox");
        var output = CreateOutput("rhx-combobox");

        helper.ViewContext = CreateViewContext();
        await helper.ProcessAsync(context, output);

        var content = output.Content.GetContent();
        Assert.Contains("role=\"combobox\"", content);
    }

    [Fact]
    public async Task Input_Has_Aria_Expanded_False()
    {
        var helper = CreateHelper();
        var context = CreateContext("rhx-combobox");
        var output = CreateOutput("rhx-combobox");

        helper.ViewContext = CreateViewContext();
        await helper.ProcessAsync(context, output);

        var content = output.Content.GetContent();
        Assert.Contains("aria-expanded=\"false\"", content);
    }

    [Fact]
    public async Task Input_Has_Aria_Autocomplete_List()
    {
        var helper = CreateHelper();
        var context = CreateContext("rhx-combobox");
        var output = CreateOutput("rhx-combobox");

        helper.ViewContext = CreateViewContext();
        await helper.ProcessAsync(context, output);

        var content = output.Content.GetContent();
        Assert.Contains("aria-autocomplete=\"list\"", content);
    }

    [Fact]
    public async Task Input_Has_Aria_Controls()
    {
        var helper = CreateHelper();
        helper.Name = "city";
        var context = CreateContext("rhx-combobox");
        var output = CreateOutput("rhx-combobox");

        helper.ViewContext = CreateViewContext();
        await helper.ProcessAsync(context, output);

        var content = output.Content.GetContent();
        Assert.Contains("aria-controls=\"city-listbox\"", content);
    }

    [Fact]
    public async Task Input_Has_Autocomplete_Off()
    {
        var helper = CreateHelper();
        var context = CreateContext("rhx-combobox");
        var output = CreateOutput("rhx-combobox");

        helper.ViewContext = CreateViewContext();
        await helper.ProcessAsync(context, output);

        var content = output.Content.GetContent();
        Assert.Contains("autocomplete=\"off\"", content);
    }

    // ── Trigger button ──

    [Fact]
    public async Task Has_Trigger_Button()
    {
        var helper = CreateHelper();
        var context = CreateContext("rhx-combobox");
        var output = CreateOutput("rhx-combobox");

        helper.ViewContext = CreateViewContext();
        await helper.ProcessAsync(context, output);

        var content = output.Content.GetContent();
        Assert.Contains("rhx-combobox__trigger", content);
        Assert.Contains("aria-label=\"Toggle\"", content);
    }

    // ── Listbox ──

    [Fact]
    public async Task Has_Listbox()
    {
        var helper = CreateHelper();
        helper.Name = "city";
        var context = CreateContext("rhx-combobox");
        var output = CreateOutput("rhx-combobox");

        helper.ViewContext = CreateViewContext();
        await helper.ProcessAsync(context, output);

        var content = output.Content.GetContent();
        Assert.Contains("role=\"listbox\"", content);
        Assert.Contains("id=\"city-listbox\"", content);
        Assert.Contains(" hidden>", content);
    }

    // ── Hidden input ──

    [Fact]
    public async Task Has_Hidden_Input()
    {
        var helper = CreateHelper();
        var context = CreateContext("rhx-combobox");
        var output = CreateOutput("rhx-combobox");

        helper.ViewContext = CreateViewContext();
        await helper.ProcessAsync(context, output);

        var content = output.Content.GetContent();
        Assert.Contains("type=\"hidden\"", content);
        Assert.Contains("data-rhx-combobox-value", content);
    }

    [Fact]
    public async Task Name_On_Hidden_Input()
    {
        var helper = CreateHelper();
        helper.Name = "city";
        var context = CreateContext("rhx-combobox");
        var output = CreateOutput("rhx-combobox");

        helper.ViewContext = CreateViewContext();
        await helper.ProcessAsync(context, output);

        var content = output.Content.GetContent();
        // Hidden input should have name attribute
        Assert.Contains("data-rhx-combobox-value", content);
        Assert.Contains("name=\"city\"", content);
    }

    [Fact]
    public async Task Value_On_Hidden_Input()
    {
        var helper = CreateHelper();
        helper.Name = "city";
        helper.Value = "NYC";
        var context = CreateContext("rhx-combobox");
        var output = CreateOutput("rhx-combobox");

        helper.ViewContext = CreateViewContext();
        await helper.ProcessAsync(context, output);

        var content = output.Content.GetContent();
        Assert.Contains("value=\"NYC\"", content);
    }

    // ── Placeholder ──

    [Fact]
    public async Task Placeholder_On_Input()
    {
        var helper = CreateHelper();
        helper.Placeholder = "Search cities...";
        var context = CreateContext("rhx-combobox");
        var output = CreateOutput("rhx-combobox");

        helper.ViewContext = CreateViewContext();
        await helper.ProcessAsync(context, output);

        var content = output.Content.GetContent();
        Assert.Contains("placeholder=\"Search cities...\"", content);
    }

    // ── Label ──

    [Fact]
    public async Task Label_With_For_And_LabelledBy()
    {
        var helper = CreateHelper();
        helper.Label = "City";
        helper.Name = "city";
        var context = CreateContext("rhx-combobox");
        var output = CreateOutput("rhx-combobox");

        helper.ViewContext = CreateViewContext();
        await helper.ProcessAsync(context, output);

        var content = output.Content.GetContent();
        Assert.Contains("rhx-combobox__label", content);
        Assert.Contains("id=\"city-label\"", content);
        Assert.Contains("for=\"city-input\"", content);
        Assert.Contains("City", content);
        Assert.Contains("aria-labelledby=\"city-label\"", content);
    }

    // ── Hint ──

    [Fact]
    public async Task Hint_Renders()
    {
        var helper = CreateHelper();
        helper.Hint = "Type to filter";
        helper.Name = "city";
        var context = CreateContext("rhx-combobox");
        var output = CreateOutput("rhx-combobox");

        helper.ViewContext = CreateViewContext();
        await helper.ProcessAsync(context, output);

        var content = output.Content.GetContent();
        Assert.Contains("rhx-combobox__hint", content);
        Assert.Contains("Type to filter", content);
    }

    // ── Error ──

    [Fact]
    public async Task Error_Adds_Modifier_And_Message()
    {
        var helper = CreateHelper();
        helper.Name = "city";
        var context = CreateContext("rhx-combobox");
        var output = CreateOutput("rhx-combobox");

        var viewContext = CreateViewContext();
        viewContext.ModelState.AddModelError("city", "City is required");
        helper.ViewContext = viewContext;
        await helper.ProcessAsync(context, output);

        Assert.True(HasClass(output, "rhx-combobox--error"));
        var content = output.Content.GetContent();
        Assert.Contains("City is required", content);
        Assert.Contains("aria-invalid=\"true\"", content);
    }

    // ── States ──

    [Fact]
    public async Task Disabled_Adds_Modifier_And_Attributes()
    {
        var helper = CreateHelper();
        helper.Disabled = true;
        var context = CreateContext("rhx-combobox");
        var output = CreateOutput("rhx-combobox");

        helper.ViewContext = CreateViewContext();
        await helper.ProcessAsync(context, output);

        Assert.True(HasClass(output, "rhx-combobox--disabled"));
        var content = output.Content.GetContent();
        Assert.Contains(" disabled", content);
    }

    [Fact]
    public async Task Readonly_Adds_Modifier_And_Attribute()
    {
        var helper = CreateHelper();
        helper.Readonly = true;
        var context = CreateContext("rhx-combobox");
        var output = CreateOutput("rhx-combobox");

        helper.ViewContext = CreateViewContext();
        await helper.ProcessAsync(context, output);

        Assert.True(HasClass(output, "rhx-combobox--readonly"));
        var content = output.Content.GetContent();
        Assert.Contains(" readonly", content);
    }

    [Fact]
    public async Task Required_Sets_Aria()
    {
        var helper = CreateHelper();
        helper.Required = true;
        var context = CreateContext("rhx-combobox");
        var output = CreateOutput("rhx-combobox");

        helper.ViewContext = CreateViewContext();
        await helper.ProcessAsync(context, output);

        var content = output.Content.GetContent();
        Assert.Contains("aria-required=\"true\"", content);
    }

    // ── Size ──

    [Fact]
    public async Task Small_Size_Adds_Modifier()
    {
        var helper = CreateHelper();
        helper.Size = "small";
        var context = CreateContext("rhx-combobox");
        var output = CreateOutput("rhx-combobox");

        helper.ViewContext = CreateViewContext();
        await helper.ProcessAsync(context, output);

        Assert.True(HasClass(output, "rhx-combobox--small"));
    }

    [Fact]
    public async Task Large_Size_Adds_Modifier()
    {
        var helper = CreateHelper();
        helper.Size = "large";
        var context = CreateContext("rhx-combobox");
        var output = CreateOutput("rhx-combobox");

        helper.ViewContext = CreateViewContext();
        await helper.ProcessAsync(context, output);

        Assert.True(HasClass(output, "rhx-combobox--large"));
    }

    // ── Filled ──

    [Fact]
    public async Task Filled_Adds_Modifier()
    {
        var helper = CreateHelper();
        helper.Filled = true;
        var context = CreateContext("rhx-combobox");
        var output = CreateOutput("rhx-combobox");

        helper.ViewContext = CreateViewContext();
        await helper.ProcessAsync(context, output);

        Assert.True(HasClass(output, "rhx-combobox--filled"));
    }

    // ── Server filter ──

    [Fact]
    public async Task ServerFilter_Sets_Data_Attribute()
    {
        var helper = CreateHelper();
        helper.ServerFilter = true;
        var context = CreateContext("rhx-combobox");
        var output = CreateOutput("rhx-combobox");

        helper.ViewContext = CreateViewContext();
        await helper.ProcessAsync(context, output);

        AssertAttribute(output, "data-rhx-server-filter", "");
    }

    // ── Items binding ──

    [Fact]
    public async Task Items_Generates_Options()
    {
        var helper = CreateHelper();
        helper.Items = new List<SelectListItem>
        {
            new("New York", "NYC"),
            new("Los Angeles", "LA")
        };
        var context = CreateContext("rhx-combobox");
        var output = CreateOutput("rhx-combobox");

        helper.ViewContext = CreateViewContext();
        await helper.ProcessAsync(context, output);

        var content = output.Content.GetContent();
        Assert.Contains("data-value=\"NYC\"", content);
        Assert.Contains("New York", content);
        Assert.Contains("data-value=\"LA\"", content);
        Assert.Contains("Los Angeles", content);
    }

    // ── Enum auto-generation ──

    [Fact]
    public async Task Enum_Auto_Generates_Options()
    {
        var helper = CreateHelper();
        helper.For = CreateModelExpressionFor("Status", Status.InProgress);
        var context = CreateContext("rhx-combobox");
        var output = CreateOutput("rhx-combobox");

        helper.ViewContext = CreateViewContext();
        await helper.ProcessAsync(context, output);

        var content = output.Content.GetContent();
        Assert.Contains("To Do", content);
        Assert.Contains("data-value=\"Todo\"", content);
        Assert.Contains("In Progress", content);
        Assert.Contains("data-value=\"InProgress\"", content);
        Assert.Contains("Done", content);
    }

    // ── htmx on text input ──

    [Fact]
    public async Task Htmx_On_Text_Input()
    {
        var helper = CreateHelper();
        helper.HxGet = "/api/cities";
        helper.HxTrigger = "input changed delay:200ms";
        helper.HxTarget = "find .rhx-combobox__listbox";
        var context = CreateContext("rhx-combobox");
        var output = CreateOutput("rhx-combobox");

        helper.ViewContext = CreateViewContext();
        await helper.ProcessAsync(context, output);

        var content = output.Content.GetContent();
        Assert.Contains("hx-get=\"/api/cities\"", content);
        Assert.Contains("hx-trigger=\"input changed delay:200ms\"", content);
    }

    // ── CSS class merging ──

    [Fact]
    public async Task Custom_Css_Class_Is_Merged()
    {
        var helper = CreateHelper();
        helper.CssClass = "my-combobox";
        var context = CreateContext("rhx-combobox");
        var output = CreateOutput("rhx-combobox");

        helper.ViewContext = CreateViewContext();
        await helper.ProcessAsync(context, output);

        Assert.True(HasClass(output, "rhx-combobox"));
        Assert.True(HasClass(output, "my-combobox"));
    }

    // ── Model binding ──

    [Fact]
    public async Task For_Resolves_Name_And_Value()
    {
        var helper = CreateHelper();
        helper.For = CreateModelExpressionFor("City", "NYC");
        var context = CreateContext("rhx-combobox");
        var output = CreateOutput("rhx-combobox");

        helper.ViewContext = CreateViewContext();
        await helper.ProcessAsync(context, output);

        var content = output.Content.GetContent();
        Assert.Contains("name=\"City\"", content);
        Assert.Contains("value=\"NYC\"", content);
    }

    // ── Control wrapper ──

    [Fact]
    public async Task Has_Control_Wrapper()
    {
        var helper = CreateHelper();
        var context = CreateContext("rhx-combobox");
        var output = CreateOutput("rhx-combobox");

        helper.ViewContext = CreateViewContext();
        await helper.ProcessAsync(context, output);

        var content = output.Content.GetContent();
        Assert.Contains("rhx-combobox__control", content);
    }

    // ── MaxOptionsVisible ──

    [Fact]
    public async Task MaxOptionsVisible_Sets_Data_Attribute()
    {
        var helper = CreateHelper();
        helper.MaxOptionsVisible = 10;
        var context = CreateContext("rhx-combobox");
        var output = CreateOutput("rhx-combobox");

        helper.ViewContext = CreateViewContext();
        await helper.ProcessAsync(context, output);

        var content = output.Content.GetContent();
        Assert.Contains("data-rhx-max-visible=\"10\"", content);
    }
}
