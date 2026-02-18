using Microsoft.AspNetCore.Razor.TagHelpers;
using RazorHX.Components.Patterns;
using Xunit;

namespace RazorHX.Tests;

public class PollTagHelperTests : TagHelperTestBase
{
    private PollTagHelper CreateHelper(string? generatedUrl = "/generated-url")
    {
        var helper = new PollTagHelper(CreateUrlHelperFactory(generatedUrl));
        helper.ViewContext = CreateViewContext();
        return helper;
    }

    // ── Element ──

    [Fact]
    public async Task Renders_Div_Element()
    {
        var helper = CreateHelper();
        helper.Page = "/Dashboard";

        var context = CreateContext("rhx-poll");
        var output = CreateOutput("rhx-poll");

        await helper.ProcessAsync(context, output);

        Assert.Equal("div", output.TagName);
    }

    [Fact]
    public async Task Has_Block_Class()
    {
        var helper = CreateHelper();
        helper.Page = "/Dashboard";

        var context = CreateContext("rhx-poll");
        var output = CreateOutput("rhx-poll");

        await helper.ProcessAsync(context, output);

        Assert.True(HasClass(output, "rhx-poll"));
    }

    // ── htmx attributes ──

    [Fact]
    public async Task Sets_Default_Interval_Trigger()
    {
        var helper = CreateHelper();
        helper.Page = "/Dashboard";

        var context = CreateContext("rhx-poll");
        var output = CreateOutput("rhx-poll");

        await helper.ProcessAsync(context, output);

        AssertAttribute(output, "hx-trigger", "every 5s");
    }

    [Fact]
    public async Task Custom_Interval()
    {
        var helper = CreateHelper();
        helper.Page = "/Dashboard";
        helper.Interval = "10s";

        var context = CreateContext("rhx-poll");
        var output = CreateOutput("rhx-poll");

        await helper.ProcessAsync(context, output);

        AssertAttribute(output, "hx-trigger", "every 10s");
    }

    [Fact]
    public async Task Sets_Default_Target_This()
    {
        var helper = CreateHelper();
        helper.Page = "/Dashboard";

        var context = CreateContext("rhx-poll");
        var output = CreateOutput("rhx-poll");

        await helper.ProcessAsync(context, output);

        AssertAttribute(output, "hx-target", "this");
    }

    [Fact]
    public async Task Sets_Default_Swap_OuterHTML()
    {
        var helper = CreateHelper();
        helper.Page = "/Dashboard";

        var context = CreateContext("rhx-poll");
        var output = CreateOutput("rhx-poll");

        await helper.ProcessAsync(context, output);

        AssertAttribute(output, "hx-swap", "outerHTML");
    }

    [Fact]
    public async Task Generates_Url_From_Page_And_Handler()
    {
        var helper = CreateHelper("/Dashboard?handler=Stats");
        helper.Page = "/Dashboard";
        helper.PageHandler = "Stats";

        var context = CreateContext("rhx-poll");
        var output = CreateOutput("rhx-poll");

        await helper.ProcessAsync(context, output);

        AssertAttribute(output, "hx-get", "/Dashboard?handler=Stats");
    }

    [Fact]
    public async Task Custom_Target_And_Swap()
    {
        var helper = CreateHelper();
        helper.Page = "/Dashboard";
        helper.Target = "#stats-panel";
        helper.Swap = "innerHTML";

        var context = CreateContext("rhx-poll");
        var output = CreateOutput("rhx-poll");

        await helper.ProcessAsync(context, output);

        AssertAttribute(output, "hx-target", "#stats-panel");
        AssertAttribute(output, "hx-swap", "innerHTML");
    }

    [Fact]
    public async Task Passes_Through_Child_Content()
    {
        var helper = CreateHelper();
        helper.Page = "/Dashboard";

        var context = CreateContext("rhx-poll");
        var output = CreateOutput("rhx-poll", childContent: "<span>Updating...</span>");

        await helper.ProcessAsync(context, output);

        var content = output.Content.GetContent();
        Assert.Contains("Updating...", content);
    }

    [Fact]
    public async Task Custom_CssClass_Appended()
    {
        var helper = CreateHelper();
        helper.Page = "/Dashboard";
        helper.CssClass = "my-poll";

        var context = CreateContext("rhx-poll");
        var output = CreateOutput("rhx-poll");

        await helper.ProcessAsync(context, output);

        Assert.True(HasClass(output, "rhx-poll"));
        Assert.True(HasClass(output, "my-poll"));
    }

    [Fact]
    public async Task No_HxGet_When_Page_Not_Set()
    {
        var helper = CreateHelper();

        var context = CreateContext("rhx-poll");
        var output = CreateOutput("rhx-poll");

        await helper.ProcessAsync(context, output);

        AssertNoAttribute(output, "hx-get");
    }
}
