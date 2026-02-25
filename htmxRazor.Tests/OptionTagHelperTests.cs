using htmxRazor.Components.Forms;
using Xunit;

namespace htmxRazor.Tests;

public class OptionTagHelperTests : TagHelperTestBase
{
    // ── Element rendering ──

    [Fact]
    public async Task Renders_Div_Element()
    {
        var helper = new OptionTagHelper();
        helper.Value = "us";
        var context = CreateContext("rhx-option");
        var output = CreateOutput("rhx-option", childContent: "United States");

        context.Items["OptionClassPrefix"] = "select";
        context.Items["SelectedValues"] = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        await helper.ProcessAsync(context, output);

        Assert.Equal("div", output.TagName);
    }

    [Fact]
    public async Task Has_Option_Class_With_Select_Prefix()
    {
        var helper = new OptionTagHelper();
        helper.Value = "us";
        var context = CreateContext("rhx-option");
        var output = CreateOutput("rhx-option", childContent: "United States");

        context.Items["OptionClassPrefix"] = "select";
        context.Items["SelectedValues"] = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        await helper.ProcessAsync(context, output);

        Assert.True(HasClass(output, "rhx-select__option"));
    }

    [Fact]
    public async Task Has_Option_Class_With_Combobox_Prefix()
    {
        var helper = new OptionTagHelper();
        helper.Value = "us";
        var context = CreateContext("rhx-option");
        var output = CreateOutput("rhx-option", childContent: "United States");

        context.Items["OptionClassPrefix"] = "combobox";
        context.Items["SelectedValues"] = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        await helper.ProcessAsync(context, output);

        Assert.True(HasClass(output, "rhx-combobox__option"));
    }

    [Fact]
    public async Task Has_Role_Option()
    {
        var helper = new OptionTagHelper();
        helper.Value = "us";
        var context = CreateContext("rhx-option");
        var output = CreateOutput("rhx-option", childContent: "United States");

        context.Items["OptionClassPrefix"] = "select";
        context.Items["SelectedValues"] = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        await helper.ProcessAsync(context, output);

        AssertAttribute(output, "role", "option");
    }

    [Fact]
    public async Task Has_Data_Value()
    {
        var helper = new OptionTagHelper();
        helper.Value = "us";
        var context = CreateContext("rhx-option");
        var output = CreateOutput("rhx-option", childContent: "United States");

        context.Items["OptionClassPrefix"] = "select";
        context.Items["SelectedValues"] = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        await helper.ProcessAsync(context, output);

        AssertAttribute(output, "data-value", "us");
    }

    [Fact]
    public async Task Has_Tabindex_Minus_One()
    {
        var helper = new OptionTagHelper();
        helper.Value = "us";
        var context = CreateContext("rhx-option");
        var output = CreateOutput("rhx-option", childContent: "United States");

        context.Items["OptionClassPrefix"] = "select";
        context.Items["SelectedValues"] = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        await helper.ProcessAsync(context, output);

        AssertAttribute(output, "tabindex", "-1");
    }

    // ── Value resolution ──

    [Fact]
    public async Task Value_From_Attribute()
    {
        var helper = new OptionTagHelper();
        helper.Value = "custom-value";
        var context = CreateContext("rhx-option");
        var output = CreateOutput("rhx-option", childContent: "Display Text");

        context.Items["OptionClassPrefix"] = "select";
        context.Items["SelectedValues"] = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        await helper.ProcessAsync(context, output);

        AssertAttribute(output, "data-value", "custom-value");
    }

    [Fact]
    public async Task Value_From_Content_When_No_Attribute()
    {
        var helper = new OptionTagHelper();
        // No Value set
        var context = CreateContext("rhx-option");
        var output = CreateOutput("rhx-option", childContent: "United States");

        context.Items["OptionClassPrefix"] = "select";
        context.Items["SelectedValues"] = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        await helper.ProcessAsync(context, output);

        AssertAttribute(output, "data-value", "United States");
    }

    // ── Selected state ──

    [Fact]
    public async Task Selected_Adds_Class_And_Aria()
    {
        var helper = new OptionTagHelper();
        helper.Value = "us";
        var context = CreateContext("rhx-option");
        var output = CreateOutput("rhx-option", childContent: "United States");

        context.Items["OptionClassPrefix"] = "select";
        context.Items["SelectedValues"] = new HashSet<string>(StringComparer.OrdinalIgnoreCase) { "us" };

        await helper.ProcessAsync(context, output);

        Assert.True(HasClass(output, "rhx-select__option--selected"));
        AssertAttribute(output, "aria-selected", "true");
    }

    [Fact]
    public async Task Not_Selected_Has_Aria_False()
    {
        var helper = new OptionTagHelper();
        helper.Value = "ca";
        var context = CreateContext("rhx-option");
        var output = CreateOutput("rhx-option", childContent: "Canada");

        context.Items["OptionClassPrefix"] = "select";
        context.Items["SelectedValues"] = new HashSet<string>(StringComparer.OrdinalIgnoreCase) { "us" };

        await helper.ProcessAsync(context, output);

        Assert.False(HasClass(output, "rhx-select__option--selected"));
        AssertAttribute(output, "aria-selected", "false");
    }

    // ── Disabled ──

    [Fact]
    public async Task Disabled_Adds_Class_And_Aria()
    {
        var helper = new OptionTagHelper();
        helper.Value = "mx";
        helper.Disabled = true;
        var context = CreateContext("rhx-option");
        var output = CreateOutput("rhx-option", childContent: "Mexico");

        context.Items["OptionClassPrefix"] = "select";
        context.Items["SelectedValues"] = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        await helper.ProcessAsync(context, output);

        Assert.True(HasClass(output, "rhx-select__option--disabled"));
        AssertAttribute(output, "aria-disabled", "true");
    }

    // ── Content ──

    [Fact]
    public async Task Content_Renders_As_Encoded_Text()
    {
        var helper = new OptionTagHelper();
        helper.Value = "test";
        var context = CreateContext("rhx-option");
        var output = CreateOutput("rhx-option", childContent: "Option & Text");

        context.Items["OptionClassPrefix"] = "select";
        context.Items["SelectedValues"] = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        await helper.ProcessAsync(context, output);

        var content = output.Content.GetContent();
        Assert.Contains("Option &amp; Text", content);
    }

    // ── Default prefix ──

    [Fact]
    public async Task Defaults_To_Select_Prefix_Without_Context()
    {
        var helper = new OptionTagHelper();
        helper.Value = "test";
        var context = CreateContext("rhx-option");
        var output = CreateOutput("rhx-option", childContent: "Test");

        // No context items set
        await helper.ProcessAsync(context, output);

        Assert.True(HasClass(output, "rhx-select__option"));
    }
}
