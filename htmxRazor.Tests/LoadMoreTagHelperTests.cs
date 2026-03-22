using Microsoft.AspNetCore.Razor.TagHelpers;
using htmxRazor.Components.Patterns;
using Xunit;

namespace htmxRazor.Tests;

public class LoadMoreTagHelperTests : TagHelperTestBase
{
    private LoadMoreTagHelper CreateHelper(string? generatedUrl = "/generated-url")
    {
        var helper = new LoadMoreTagHelper(CreateUrlHelperFactory(generatedUrl));
        helper.ViewContext = CreateViewContext();
        return helper;
    }

    // ── Element ──

    [Fact]
    public async Task Renders_Div_Element()
    {
        var helper = CreateHelper();
        helper.Page = "/Items";

        var context = CreateContext("rhx-load-more");
        var output = CreateOutput("rhx-load-more");

        await helper.ProcessAsync(context, output);

        Assert.Equal("div", output.TagName);
    }

    [Fact]
    public async Task Has_Block_Class()
    {
        var helper = CreateHelper();
        helper.Page = "/Items";

        var context = CreateContext("rhx-load-more");
        var output = CreateOutput("rhx-load-more");

        await helper.ProcessAsync(context, output);

        Assert.True(HasClass(output, "rhx-load-more"));
    }

    // ── Button rendering ──

    [Fact]
    public async Task Renders_Button_Inside()
    {
        var helper = CreateHelper();
        helper.Page = "/Items";

        var context = CreateContext("rhx-load-more");
        var output = CreateOutput("rhx-load-more");

        await helper.ProcessAsync(context, output);

        var content = output.Content.GetContent();
        Assert.Contains("<button", content);
        Assert.Contains("rhx-load-more__button", content);
    }

    // ── URL generation ──

    [Fact]
    public async Task Generates_Url_From_Page_And_Handler()
    {
        var helper = CreateHelper("/Items?handler=LoadMore");
        helper.Page = "/Items";
        helper.PageHandler = "LoadMore";

        var context = CreateContext("rhx-load-more");
        var output = CreateOutput("rhx-load-more");

        await helper.ProcessAsync(context, output);

        var content = output.Content.GetContent();
        Assert.Contains("hx-get=\"/Items?handler=LoadMore\"", content);
    }

    [Fact]
    public async Task Generates_Url_With_Route_Values()
    {
        var helper = CreateHelper("/generated-url");
        helper.Page = "/Items";
        helper.PageHandler = "LoadMore";
        helper.RouteValues["page"] = "3";

        var context = CreateContext("rhx-load-more");
        var output = CreateOutput("rhx-load-more");

        await helper.ProcessAsync(context, output);

        var content = output.Content.GetContent();
        Assert.Contains("hx-get=\"/generated-url\"", content);
    }

    // ── htmx attributes ──

    [Fact]
    public async Task Sets_Default_Swap_Beforeend()
    {
        var helper = CreateHelper();
        helper.Page = "/Items";

        var context = CreateContext("rhx-load-more");
        var output = CreateOutput("rhx-load-more");

        await helper.ProcessAsync(context, output);

        var content = output.Content.GetContent();
        Assert.Contains("hx-swap=\"beforeend\"", content);
    }

    [Fact]
    public async Task Sets_Custom_Target()
    {
        var helper = CreateHelper();
        helper.Page = "/Items";
        helper.Target = "#item-list";

        var context = CreateContext("rhx-load-more");
        var output = CreateOutput("rhx-load-more");

        await helper.ProcessAsync(context, output);

        var content = output.Content.GetContent();
        Assert.Contains("hx-target=\"#item-list\"", content);
    }

    [Fact]
    public async Task Sets_Custom_Swap()
    {
        var helper = CreateHelper();
        helper.Page = "/Items";
        helper.Swap = "innerHTML";

        var context = CreateContext("rhx-load-more");
        var output = CreateOutput("rhx-load-more");

        await helper.ProcessAsync(context, output);

        var content = output.Content.GetContent();
        Assert.Contains("hx-swap=\"innerHTML\"", content);
    }

    // ── Child content ──

    [Fact]
    public async Task Passes_Through_Child_Content()
    {
        var helper = CreateHelper();
        helper.Page = "/Items";

        var context = CreateContext("rhx-load-more");
        var output = CreateOutput("rhx-load-more", childContent: "Load more items");

        await helper.ProcessAsync(context, output);

        var content = output.Content.GetContent();
        Assert.Contains("Load more items", content);
    }

    // ── Common attributes ──

    [Fact]
    public async Task Custom_CssClass_Appended()
    {
        var helper = CreateHelper();
        helper.Page = "/Items";
        helper.CssClass = "my-load-more";

        var context = CreateContext("rhx-load-more");
        var output = CreateOutput("rhx-load-more");

        await helper.ProcessAsync(context, output);

        Assert.True(HasClass(output, "rhx-load-more"));
        Assert.True(HasClass(output, "my-load-more"));
    }

    [Fact]
    public async Task Id_Sets_Attribute()
    {
        var helper = CreateHelper();
        helper.Page = "/Items";
        helper.Id = "load-more-trigger";

        var context = CreateContext("rhx-load-more");
        var output = CreateOutput("rhx-load-more");

        await helper.ProcessAsync(context, output);

        AssertAttribute(output, "id", "load-more-trigger");
    }

    [Fact]
    public async Task No_HxGet_When_Page_Not_Set()
    {
        var helper = CreateHelper();
        // Page not set

        var context = CreateContext("rhx-load-more");
        var output = CreateOutput("rhx-load-more");

        await helper.ProcessAsync(context, output);

        var content = output.Content.GetContent();
        Assert.DoesNotContain("hx-get=", content);
    }

    // ── Disabled state ──

    [Fact]
    public async Task Disabled_Adds_Attribute()
    {
        var helper = CreateHelper();
        helper.Page = "/Items";
        helper.Disabled = true;

        var context = CreateContext("rhx-load-more");
        var output = CreateOutput("rhx-load-more");

        await helper.ProcessAsync(context, output);

        var content = output.Content.GetContent();
        Assert.Contains("disabled", content);
        Assert.Contains("aria-disabled=\"true\"", content);
    }

    // ── Loading text ──

    [Fact]
    public async Task Loading_Text_Used_In_Indicator()
    {
        var helper = CreateHelper();
        helper.Page = "/Items";
        helper.LoadingText = "Fetching...";

        var context = CreateContext("rhx-load-more");
        var output = CreateOutput("rhx-load-more");

        await helper.ProcessAsync(context, output);

        var content = output.Content.GetContent();
        Assert.Contains("Fetching...", content);
    }

    // ── Variant ──

    [Fact]
    public async Task Variant_Modifier_Applied()
    {
        var helper = CreateHelper();
        helper.Page = "/Items";
        helper.Variant = "brand";

        var context = CreateContext("rhx-load-more");
        var output = CreateOutput("rhx-load-more");

        await helper.ProcessAsync(context, output);

        var content = output.Content.GetContent();
        Assert.Contains("rhx-button--brand", content);
    }
}
