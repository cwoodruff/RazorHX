using htmxRazor.Components.Forms;
using Xunit;

namespace htmxRazor.Tests;

public class RadioTagHelperTests : TagHelperTestBase
{
    private static RadioTagHelper CreateHelper() => new();

    private static Dictionary<object, object> CreateGroupContext(
        string name = "color",
        string? value = null,
        bool disabled = false)
    {
        var items = new Dictionary<object, object>
        {
            ["RadioGroupName"] = name
        };
        if (value != null)
            items["RadioGroupValue"] = value;
        if (disabled)
            items["RadioGroupDisabled"] = true;
        return items;
    }

    // ── Element rendering ──

    [Fact]
    public async Task Renders_Label_Element()
    {
        var helper = CreateHelper();
        helper.Value = "red";
        var context = CreateContext("rhx-radio");
        context.Items["RadioGroupName"] = "color";
        var output = CreateOutput("rhx-radio", childContent: "Red");

        await helper.ProcessAsync(context, output);

        Assert.Equal("label", output.TagName);
    }

    [Fact]
    public async Task Has_Radio_Class()
    {
        var helper = CreateHelper();
        helper.Value = "red";
        var context = CreateContext("rhx-radio");
        context.Items["RadioGroupName"] = "color";
        var output = CreateOutput("rhx-radio", childContent: "Red");

        await helper.ProcessAsync(context, output);

        Assert.True(HasClass(output, "rhx-radio"));
    }

    // ── Native radio input ──

    [Fact]
    public async Task Has_Native_Radio_Input()
    {
        var helper = CreateHelper();
        helper.Value = "red";
        var context = CreateContext("rhx-radio");
        context.Items["RadioGroupName"] = "color";
        var output = CreateOutput("rhx-radio", childContent: "Red");

        await helper.ProcessAsync(context, output);

        var content = output.Content.GetContent();
        Assert.Contains("type=\"radio\"", content);
        Assert.Contains("rhx-radio__native", content);
    }

    [Fact]
    public async Task Native_Input_Is_Sr_Only()
    {
        var helper = CreateHelper();
        helper.Value = "red";
        var context = CreateContext("rhx-radio");
        context.Items["RadioGroupName"] = "color";
        var output = CreateOutput("rhx-radio", childContent: "Red");

        await helper.ProcessAsync(context, output);

        var content = output.Content.GetContent();
        Assert.Contains("rhx-sr-only", content);
    }

    // ── Custom control ──

    [Fact]
    public async Task Has_Custom_Control()
    {
        var helper = CreateHelper();
        helper.Value = "red";
        var context = CreateContext("rhx-radio");
        context.Items["RadioGroupName"] = "color";
        var output = CreateOutput("rhx-radio", childContent: "Red");

        await helper.ProcessAsync(context, output);

        var content = output.Content.GetContent();
        Assert.Contains("rhx-radio__control", content);
        Assert.Contains("aria-hidden=\"true\"", content);
    }

    // ── Value ──

    [Fact]
    public async Task Value_Sets_Value_Attribute()
    {
        var helper = CreateHelper();
        helper.Value = "red";
        var context = CreateContext("rhx-radio");
        context.Items["RadioGroupName"] = "color";
        var output = CreateOutput("rhx-radio", childContent: "Red");

        await helper.ProcessAsync(context, output);

        var content = output.Content.GetContent();
        Assert.Contains("value=\"red\"", content);
        Assert.Contains("name=\"color\"", content);
    }

    [Fact]
    public async Task Value_From_Content_When_No_Value_Attribute()
    {
        var helper = CreateHelper();
        // No Value set — should fall back to content text
        var context = CreateContext("rhx-radio");
        context.Items["RadioGroupName"] = "color";
        var output = CreateOutput("rhx-radio", childContent: "Red");

        await helper.ProcessAsync(context, output);

        var content = output.Content.GetContent();
        Assert.Contains("value=\"Red\"", content);
    }

    // ── Checked state ──

    [Fact]
    public async Task Checked_When_Matches_Group_Value()
    {
        var helper = CreateHelper();
        helper.Value = "red";
        var context = CreateContext("rhx-radio");
        foreach (var kv in CreateGroupContext("color", "red"))
            context.Items[kv.Key] = kv.Value;
        var output = CreateOutput("rhx-radio", childContent: "Red");

        await helper.ProcessAsync(context, output);

        var content = output.Content.GetContent();
        Assert.Contains(" checked", content);
    }

    [Fact]
    public async Task Not_Checked_When_Different_Value()
    {
        var helper = CreateHelper();
        helper.Value = "red";
        var context = CreateContext("rhx-radio");
        foreach (var kv in CreateGroupContext("color", "blue"))
            context.Items[kv.Key] = kv.Value;
        var output = CreateOutput("rhx-radio", childContent: "Red");

        await helper.ProcessAsync(context, output);

        var content = output.Content.GetContent();
        Assert.DoesNotContain(" checked", content);
    }

    // ── Disabled ──

    [Fact]
    public async Task Disabled_Adds_Class_And_Attribute()
    {
        var helper = CreateHelper();
        helper.Value = "red";
        helper.Disabled = true;
        var context = CreateContext("rhx-radio");
        context.Items["RadioGroupName"] = "color";
        var output = CreateOutput("rhx-radio", childContent: "Red");

        await helper.ProcessAsync(context, output);

        Assert.True(HasClass(output, "rhx-radio--disabled"));
        var content = output.Content.GetContent();
        Assert.Contains(" disabled", content);
    }

    [Fact]
    public async Task Group_Disabled_Propagates()
    {
        var helper = CreateHelper();
        helper.Value = "red";
        var context = CreateContext("rhx-radio");
        foreach (var kv in CreateGroupContext("color", disabled: true))
            context.Items[kv.Key] = kv.Value;
        var output = CreateOutput("rhx-radio", childContent: "Red");

        await helper.ProcessAsync(context, output);

        Assert.True(HasClass(output, "rhx-radio--disabled"));
        var content = output.Content.GetContent();
        Assert.Contains(" disabled", content);
    }

    // ── Label text ──

    [Fact]
    public async Task Label_Text_Renders()
    {
        var helper = CreateHelper();
        helper.Value = "red";
        helper.Label = "Custom Label";
        var context = CreateContext("rhx-radio");
        context.Items["RadioGroupName"] = "color";
        var output = CreateOutput("rhx-radio", childContent: "Red");

        await helper.ProcessAsync(context, output);

        var content = output.Content.GetContent();
        Assert.Contains("rhx-radio__text", content);
        // Label prop overrides child content
        Assert.Contains("Custom Label", content);
    }
}
