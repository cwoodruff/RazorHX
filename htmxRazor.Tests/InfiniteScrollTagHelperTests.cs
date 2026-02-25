using Microsoft.AspNetCore.Razor.TagHelpers;
using htmxRazor.Components.Patterns;
using Xunit;

namespace htmxRazor.Tests;

public class InfiniteScrollTagHelperTests : TagHelperTestBase
{
    private InfiniteScrollTagHelper CreateHelper(string? generatedUrl = "/generated-url")
    {
        var helper = new InfiniteScrollTagHelper(CreateUrlHelperFactory(generatedUrl));
        helper.ViewContext = CreateViewContext();
        return helper;
    }

    // ── Element ──

    [Fact]
    public async Task Renders_Div_Element()
    {
        var helper = CreateHelper();
        helper.Page = "/Items";

        var context = CreateContext("rhx-infinite-scroll");
        var output = CreateOutput("rhx-infinite-scroll");

        await helper.ProcessAsync(context, output);

        Assert.Equal("div", output.TagName);
    }

    [Fact]
    public async Task Has_Block_Class()
    {
        var helper = CreateHelper();
        helper.Page = "/Items";

        var context = CreateContext("rhx-infinite-scroll");
        var output = CreateOutput("rhx-infinite-scroll");

        await helper.ProcessAsync(context, output);

        Assert.True(HasClass(output, "rhx-infinite-scroll"));
    }

    // ── htmx attributes ──

    [Fact]
    public async Task Sets_HxTrigger_To_Revealed()
    {
        var helper = CreateHelper();
        helper.Page = "/Items";

        var context = CreateContext("rhx-infinite-scroll");
        var output = CreateOutput("rhx-infinite-scroll");

        await helper.ProcessAsync(context, output);

        AssertAttribute(output, "hx-trigger", "revealed");
    }

    [Fact]
    public async Task Sets_Default_Swap_Beforeend()
    {
        var helper = CreateHelper();
        helper.Page = "/Items";

        var context = CreateContext("rhx-infinite-scroll");
        var output = CreateOutput("rhx-infinite-scroll");

        await helper.ProcessAsync(context, output);

        AssertAttribute(output, "hx-swap", "beforeend");
    }

    [Fact]
    public async Task Generates_Url_From_Page_And_Handler()
    {
        var helper = CreateHelper("/Items?handler=LoadMore");
        helper.Page = "/Items";
        helper.PageHandler = "LoadMore";

        var context = CreateContext("rhx-infinite-scroll");
        var output = CreateOutput("rhx-infinite-scroll");

        await helper.ProcessAsync(context, output);

        AssertAttribute(output, "hx-get", "/Items?handler=LoadMore");
    }

    [Fact]
    public async Task Generates_Url_With_Route_Values()
    {
        var helper = CreateHelper("/generated-url");
        helper.Page = "/Items";
        helper.PageHandler = "LoadMore";
        helper.RouteValues["page"] = "3";

        var context = CreateContext("rhx-infinite-scroll");
        var output = CreateOutput("rhx-infinite-scroll");

        await helper.ProcessAsync(context, output);

        AssertAttribute(output, "hx-get", "/generated-url");
    }

    [Fact]
    public async Task Sets_Custom_Target()
    {
        var helper = CreateHelper();
        helper.Page = "/Items";
        helper.Target = "#item-list";

        var context = CreateContext("rhx-infinite-scroll");
        var output = CreateOutput("rhx-infinite-scroll");

        await helper.ProcessAsync(context, output);

        AssertAttribute(output, "hx-target", "#item-list");
    }

    [Fact]
    public async Task Sets_Custom_Swap()
    {
        var helper = CreateHelper();
        helper.Page = "/Items";
        helper.Swap = "innerHTML";

        var context = CreateContext("rhx-infinite-scroll");
        var output = CreateOutput("rhx-infinite-scroll");

        await helper.ProcessAsync(context, output);

        AssertAttribute(output, "hx-swap", "innerHTML");
    }

    [Fact]
    public async Task Passes_Through_Child_Content()
    {
        var helper = CreateHelper();
        helper.Page = "/Items";

        var context = CreateContext("rhx-infinite-scroll");
        var output = CreateOutput("rhx-infinite-scroll", childContent: "<span>Loading...</span>");

        await helper.ProcessAsync(context, output);

        var content = output.Content.GetContent();
        Assert.Contains("Loading...", content);
    }

    [Fact]
    public async Task Custom_CssClass_Appended()
    {
        var helper = CreateHelper();
        helper.Page = "/Items";
        helper.CssClass = "my-scroll";

        var context = CreateContext("rhx-infinite-scroll");
        var output = CreateOutput("rhx-infinite-scroll");

        await helper.ProcessAsync(context, output);

        Assert.True(HasClass(output, "rhx-infinite-scroll"));
        Assert.True(HasClass(output, "my-scroll"));
    }

    [Fact]
    public async Task Id_Sets_Attribute()
    {
        var helper = CreateHelper();
        helper.Page = "/Items";
        helper.Id = "scroll-trigger";

        var context = CreateContext("rhx-infinite-scroll");
        var output = CreateOutput("rhx-infinite-scroll");

        await helper.ProcessAsync(context, output);

        AssertAttribute(output, "id", "scroll-trigger");
    }

    [Fact]
    public async Task No_HxGet_When_Page_Not_Set()
    {
        var helper = CreateHelper();
        // Page not set

        var context = CreateContext("rhx-infinite-scroll");
        var output = CreateOutput("rhx-infinite-scroll");

        await helper.ProcessAsync(context, output);

        AssertNoAttribute(output, "hx-get");
    }

    [Fact]
    public async Task No_HxTarget_When_Target_Not_Set()
    {
        var helper = CreateHelper();
        helper.Page = "/Items";
        // Target not set

        var context = CreateContext("rhx-infinite-scroll");
        var output = CreateOutput("rhx-infinite-scroll");

        await helper.ProcessAsync(context, output);

        AssertNoAttribute(output, "hx-target");
    }
}
