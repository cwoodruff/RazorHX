using Microsoft.AspNetCore.Razor.TagHelpers;
using RazorHX.Components.Patterns;
using Xunit;

namespace RazorHX.Tests;

public class LazyLoadTagHelperTests : TagHelperTestBase
{
    private LazyLoadTagHelper CreateHelper(string? generatedUrl = "/generated-url")
    {
        var helper = new LazyLoadTagHelper(CreateUrlHelperFactory(generatedUrl));
        helper.ViewContext = CreateViewContext();
        return helper;
    }

    // ── Element ──

    [Fact]
    public async Task Renders_Div_Element()
    {
        var helper = CreateHelper();
        helper.Page = "/Dashboard";

        var context = CreateContext("rhx-lazy-load");
        var output = CreateOutput("rhx-lazy-load");

        await helper.ProcessAsync(context, output);

        Assert.Equal("div", output.TagName);
    }

    [Fact]
    public async Task Has_Block_Class()
    {
        var helper = CreateHelper();
        helper.Page = "/Dashboard";

        var context = CreateContext("rhx-lazy-load");
        var output = CreateOutput("rhx-lazy-load");

        await helper.ProcessAsync(context, output);

        Assert.True(HasClass(output, "rhx-lazy-load"));
    }

    // ── htmx attributes ──

    [Fact]
    public async Task Sets_Default_Trigger_Load()
    {
        var helper = CreateHelper();
        helper.Page = "/Dashboard";

        var context = CreateContext("rhx-lazy-load");
        var output = CreateOutput("rhx-lazy-load");

        await helper.ProcessAsync(context, output);

        AssertAttribute(output, "hx-trigger", "load");
    }

    [Fact]
    public async Task Revealed_Trigger()
    {
        var helper = CreateHelper();
        helper.Page = "/Dashboard";
        helper.Trigger = "revealed";

        var context = CreateContext("rhx-lazy-load");
        var output = CreateOutput("rhx-lazy-load");

        await helper.ProcessAsync(context, output);

        AssertAttribute(output, "hx-trigger", "revealed");
    }

    [Fact]
    public async Task Sets_Default_Target_This()
    {
        var helper = CreateHelper();
        helper.Page = "/Dashboard";

        var context = CreateContext("rhx-lazy-load");
        var output = CreateOutput("rhx-lazy-load");

        await helper.ProcessAsync(context, output);

        AssertAttribute(output, "hx-target", "this");
    }

    [Fact]
    public async Task Sets_Default_Swap_OuterHTML()
    {
        var helper = CreateHelper();
        helper.Page = "/Dashboard";

        var context = CreateContext("rhx-lazy-load");
        var output = CreateOutput("rhx-lazy-load");

        await helper.ProcessAsync(context, output);

        AssertAttribute(output, "hx-swap", "outerHTML");
    }

    [Fact]
    public async Task Generates_Url_From_Page_And_Handler()
    {
        var helper = CreateHelper("/Dashboard?handler=Chart");
        helper.Page = "/Dashboard";
        helper.PageHandler = "Chart";

        var context = CreateContext("rhx-lazy-load");
        var output = CreateOutput("rhx-lazy-load");

        await helper.ProcessAsync(context, output);

        AssertAttribute(output, "hx-get", "/Dashboard?handler=Chart");
    }

    [Fact]
    public async Task Generates_Url_With_Route_Values()
    {
        var helper = CreateHelper("/generated-url");
        helper.Page = "/Dashboard";
        helper.RouteValues["id"] = "42";

        var context = CreateContext("rhx-lazy-load");
        var output = CreateOutput("rhx-lazy-load");

        await helper.ProcessAsync(context, output);

        AssertAttribute(output, "hx-get", "/generated-url");
    }

    [Fact]
    public async Task Custom_Target_And_Swap()
    {
        var helper = CreateHelper();
        helper.Page = "/Dashboard";
        helper.Target = "#chart-area";
        helper.Swap = "innerHTML";

        var context = CreateContext("rhx-lazy-load");
        var output = CreateOutput("rhx-lazy-load");

        await helper.ProcessAsync(context, output);

        AssertAttribute(output, "hx-target", "#chart-area");
        AssertAttribute(output, "hx-swap", "innerHTML");
    }

    [Fact]
    public async Task Passes_Through_Child_Content()
    {
        var helper = CreateHelper();
        helper.Page = "/Dashboard";

        var context = CreateContext("rhx-lazy-load");
        var output = CreateOutput("rhx-lazy-load", childContent: "<div>Placeholder</div>");

        await helper.ProcessAsync(context, output);

        var content = output.Content.GetContent();
        Assert.Contains("Placeholder", content);
    }

    [Fact]
    public async Task Custom_CssClass_Appended()
    {
        var helper = CreateHelper();
        helper.Page = "/Dashboard";
        helper.CssClass = "my-lazy";

        var context = CreateContext("rhx-lazy-load");
        var output = CreateOutput("rhx-lazy-load");

        await helper.ProcessAsync(context, output);

        Assert.True(HasClass(output, "rhx-lazy-load"));
        Assert.True(HasClass(output, "my-lazy"));
    }

    [Fact]
    public async Task No_HxGet_When_Page_Not_Set()
    {
        var helper = CreateHelper();

        var context = CreateContext("rhx-lazy-load");
        var output = CreateOutput("rhx-lazy-load");

        await helper.ProcessAsync(context, output);

        AssertNoAttribute(output, "hx-get");
    }

    [Fact]
    public async Task Id_Sets_Attribute()
    {
        var helper = CreateHelper();
        helper.Page = "/Dashboard";
        helper.Id = "lazy-chart";

        var context = CreateContext("rhx-lazy-load");
        var output = CreateOutput("rhx-lazy-load");

        await helper.ProcessAsync(context, output);

        AssertAttribute(output, "id", "lazy-chart");
    }
}
