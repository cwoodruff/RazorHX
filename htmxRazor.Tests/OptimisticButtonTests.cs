using htmxRazor.Components.Actions;
using Xunit;

namespace htmxRazor.Tests;

public class OptimisticButtonTests : TagHelperTestBase
{
    private ButtonTagHelper CreateHelper()
    {
        var helper = new ButtonTagHelper(CreateUrlHelperFactory());
        helper.ViewContext = CreateViewContext();
        return helper;
    }

    [Fact]
    public async Task Optimistic_True_Adds_Data_Attribute()
    {
        var helper = CreateHelper();
        helper.Optimistic = true;

        var context = CreateContext("rhx-button");
        var output = CreateOutput("rhx-button", childContent: "Save");

        await helper.ProcessAsync(context, output);

        Assert.True(output.Attributes.TryGetAttribute("data-rhx-optimistic", out _));
    }

    [Fact]
    public async Task Optimistic_False_No_Data_Attribute()
    {
        var helper = CreateHelper();
        helper.Optimistic = false;

        var context = CreateContext("rhx-button");
        var output = CreateOutput("rhx-button", childContent: "Save");

        await helper.ProcessAsync(context, output);

        Assert.False(output.Attributes.TryGetAttribute("data-rhx-optimistic", out _));
    }

    [Fact]
    public async Task Optimistic_Does_Not_Add_Loading_Class()
    {
        var helper = CreateHelper();
        helper.Optimistic = true;

        var context = CreateContext("rhx-button");
        var output = CreateOutput("rhx-button", childContent: "Save");

        await helper.ProcessAsync(context, output);

        // Loading class should NOT be added server-side; JS handles it
        Assert.False(HasClass(output, "rhx-button--loading"));
    }

    [Fact]
    public async Task Optimistic_Preserves_Button_Type()
    {
        var helper = CreateHelper();
        helper.Optimistic = true;
        helper.ButtonType = "submit";

        var context = CreateContext("rhx-button");
        var output = CreateOutput("rhx-button", childContent: "Submit");

        await helper.ProcessAsync(context, output);

        AssertAttribute(output, "type", "submit");
    }
}
