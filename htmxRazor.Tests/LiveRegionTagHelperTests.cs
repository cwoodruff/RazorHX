using htmxRazor.Components.Utilities;
using Xunit;

namespace htmxRazor.Tests;

/// <summary>
/// Tests for the LiveRegionTagHelper component.
/// </summary>
public class LiveRegionTagHelperTests : TagHelperTestBase
{
    private LiveRegionTagHelper CreateHelper()
    {
        var helper = new LiveRegionTagHelper(CreateUrlHelperFactory());
        helper.ViewContext = CreateViewContext();
        return helper;
    }

    [Fact]
    public async Task Renders_Div_Element()
    {
        var helper = CreateHelper();
        var context = CreateContext("rhx-live-region");
        var output = CreateOutput("rhx-live-region", childContent: "Content");

        await helper.ProcessAsync(context, output);

        Assert.Equal("div", output.TagName);
    }

    [Fact]
    public async Task Has_Block_Class()
    {
        var helper = CreateHelper();
        var context = CreateContext("rhx-live-region");
        var output = CreateOutput("rhx-live-region", childContent: "Content");

        await helper.ProcessAsync(context, output);

        Assert.True(HasClass(output, "rhx-live-region"));
    }

    [Fact]
    public async Task Has_Default_Role_Status()
    {
        var helper = CreateHelper();
        var context = CreateContext("rhx-live-region");
        var output = CreateOutput("rhx-live-region", childContent: "Content");

        await helper.ProcessAsync(context, output);

        AssertAttribute(output, "role", "status");
    }

    [Fact]
    public async Task Has_Default_AriaLive_Polite()
    {
        var helper = CreateHelper();
        var context = CreateContext("rhx-live-region");
        var output = CreateOutput("rhx-live-region", childContent: "Content");

        await helper.ProcessAsync(context, output);

        AssertAttribute(output, "aria-live", "polite");
    }

    [Fact]
    public async Task Has_AriaAtomic_True_By_Default()
    {
        var helper = CreateHelper();
        var context = CreateContext("rhx-live-region");
        var output = CreateOutput("rhx-live-region", childContent: "Content");

        await helper.ProcessAsync(context, output);

        AssertAttribute(output, "aria-atomic", "true");
    }

    [Theory]
    [InlineData("assertive")]
    [InlineData("off")]
    public async Task Respects_Politeness_Setting(string politeness)
    {
        var helper = CreateHelper();
        helper.Politeness = politeness;
        var context = CreateContext("rhx-live-region");
        var output = CreateOutput("rhx-live-region", childContent: "Content");

        await helper.ProcessAsync(context, output);

        AssertAttribute(output, "aria-live", politeness);
    }

    [Fact]
    public async Task Renders_AriaRelevant_When_Set()
    {
        var helper = CreateHelper();
        helper.Relevant = "additions removals";
        var context = CreateContext("rhx-live-region");
        var output = CreateOutput("rhx-live-region", childContent: "Content");

        await helper.ProcessAsync(context, output);

        AssertAttribute(output, "aria-relevant", "additions removals");
    }

    [Fact]
    public async Task No_AriaRelevant_By_Default()
    {
        var helper = CreateHelper();
        var context = CreateContext("rhx-live-region");
        var output = CreateOutput("rhx-live-region", childContent: "Content");

        await helper.ProcessAsync(context, output);

        AssertNoAttribute(output, "aria-relevant");
    }

    [Fact]
    public async Task Adds_VisuallyHidden_Modifier()
    {
        var helper = CreateHelper();
        helper.VisuallyHidden = true;
        var context = CreateContext("rhx-live-region");
        var output = CreateOutput("rhx-live-region", childContent: "Content");

        await helper.ProcessAsync(context, output);

        Assert.True(HasClass(output, "rhx-live-region--visually-hidden"));
    }

    [Fact]
    public async Task Custom_Role()
    {
        var helper = CreateHelper();
        helper.Role = "alert";
        var context = CreateContext("rhx-live-region");
        var output = CreateOutput("rhx-live-region", childContent: "Content");

        await helper.ProcessAsync(context, output);

        AssertAttribute(output, "role", "alert");
    }

    [Fact]
    public async Task Forwards_Htmx_Attributes()
    {
        var helper = CreateHelper();
        helper.HxGet = "/api/status";
        helper.HxTrigger = "every 5s";
        var context = CreateContext("rhx-live-region");
        var output = CreateOutput("rhx-live-region", childContent: "Content");

        await helper.ProcessAsync(context, output);

        AssertAttribute(output, "hx-get", "/api/status");
        AssertAttribute(output, "hx-trigger", "every 5s");
    }
}
