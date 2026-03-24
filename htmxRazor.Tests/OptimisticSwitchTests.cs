using htmxRazor.Components.Forms;
using Xunit;

namespace htmxRazor.Tests;

public class OptimisticSwitchTests : TagHelperTestBase
{
    private SwitchTagHelper CreateHelper()
    {
        var helper = new SwitchTagHelper(CreateUrlHelperFactory());
        helper.ViewContext = CreateViewContext();
        return helper;
    }

    [Fact]
    public void Optimistic_True_Adds_Data_Attribute()
    {
        var helper = CreateHelper();
        helper.Optimistic = true;

        var context = CreateContext("rhx-switch");
        var output = CreateOutput("rhx-switch");

        helper.Process(context, output);

        Assert.True(output.Attributes.TryGetAttribute("data-rhx-optimistic", out _));
    }

    [Fact]
    public void Optimistic_False_No_Data_Attribute()
    {
        var helper = CreateHelper();
        helper.Optimistic = false;

        var context = CreateContext("rhx-switch");
        var output = CreateOutput("rhx-switch");

        helper.Process(context, output);

        Assert.False(output.Attributes.TryGetAttribute("data-rhx-optimistic", out _));
    }

    [Fact]
    public void Optimistic_Preserves_Other_Attributes()
    {
        var helper = CreateHelper();
        helper.Optimistic = true;
        helper.Checked = true;

        var context = CreateContext("rhx-switch");
        var output = CreateOutput("rhx-switch");

        helper.Process(context, output);

        // Should still have data-rhx-switch
        Assert.True(output.Attributes.TryGetAttribute("data-rhx-switch", out _));
        Assert.True(HasClass(output, "rhx-switch"));
    }

    [Fact]
    public void Optimistic_Works_With_HxPost()
    {
        var helper = CreateHelper();
        helper.Optimistic = true;
        helper.HxPost = "/api/toggle";

        var context = CreateContext("rhx-switch");
        var output = CreateOutput("rhx-switch");

        helper.Process(context, output);

        Assert.True(output.Attributes.TryGetAttribute("data-rhx-optimistic", out _));
        // htmx attributes are rendered in the inner native input via BuildHtmxAttributeString
        var content = output.Content.GetContent();
        Assert.Contains("hx-post", content);
    }
}
