using Microsoft.AspNetCore.Razor.TagHelpers;
using htmxRazor.Components.Patterns;
using Xunit;

namespace htmxRazor.Tests;

public class SseStreamTagHelperTests : TagHelperTestBase
{
    private SseStreamTagHelper CreateHelper(string? generatedUrl = "/generated-url")
    {
        var helper = new SseStreamTagHelper(CreateUrlHelperFactory(generatedUrl));
        helper.ViewContext = CreateViewContext();
        return helper;
    }

    // ── Element ──

    [Fact]
    public async Task Renders_Div_Element()
    {
        var helper = CreateHelper();
        helper.Url = "/api/stream";

        var context = CreateContext("rhx-sse-stream");
        var output = CreateOutput("rhx-sse-stream");

        await helper.ProcessAsync(context, output);

        Assert.Equal("div", output.TagName);
    }

    [Fact]
    public async Task Has_Block_Class()
    {
        var helper = CreateHelper();
        helper.Url = "/api/stream";

        var context = CreateContext("rhx-sse-stream");
        var output = CreateOutput("rhx-sse-stream");

        await helper.ProcessAsync(context, output);

        Assert.True(HasClass(output, "rhx-sse-stream"));
    }

    // ── htmx SSE extension ──

    [Fact]
    public async Task Sets_HxExt_Sse()
    {
        var helper = CreateHelper();
        helper.Url = "/api/stream";

        var context = CreateContext("rhx-sse-stream");
        var output = CreateOutput("rhx-sse-stream");

        await helper.ProcessAsync(context, output);

        AssertAttribute(output, "hx-ext", "sse");
    }

    [Fact]
    public async Task Sets_SseConnect_From_Url()
    {
        var helper = CreateHelper();
        helper.Url = "/api/stream";

        var context = CreateContext("rhx-sse-stream");
        var output = CreateOutput("rhx-sse-stream");

        await helper.ProcessAsync(context, output);

        AssertAttribute(output, "sse-connect", "/api/stream");
    }

    [Fact]
    public async Task Sets_SseConnect_From_Page_Route()
    {
        var helper = CreateHelper("/Dashboard?handler=Stream");
        helper.Page = "/Dashboard";
        helper.PageHandler = "Stream";

        var context = CreateContext("rhx-sse-stream");
        var output = CreateOutput("rhx-sse-stream");

        await helper.ProcessAsync(context, output);

        AssertAttribute(output, "sse-connect", "/Dashboard?handler=Stream");
    }

    [Fact]
    public async Task Sets_Default_SseSwap_Message()
    {
        var helper = CreateHelper();
        helper.Url = "/api/stream";

        var context = CreateContext("rhx-sse-stream");
        var output = CreateOutput("rhx-sse-stream");

        await helper.ProcessAsync(context, output);

        AssertAttribute(output, "sse-swap", "message");
    }

    [Fact]
    public async Task Sets_Custom_SseSwap_Event()
    {
        var helper = CreateHelper();
        helper.Url = "/api/stream";
        helper.EventName = "status-update";

        var context = CreateContext("rhx-sse-stream");
        var output = CreateOutput("rhx-sse-stream");

        await helper.ProcessAsync(context, output);

        AssertAttribute(output, "sse-swap", "status-update");
    }

    [Fact]
    public async Task Sets_Default_HxSwap_InnerHTML()
    {
        var helper = CreateHelper();
        helper.Url = "/api/stream";

        var context = CreateContext("rhx-sse-stream");
        var output = CreateOutput("rhx-sse-stream");

        await helper.ProcessAsync(context, output);

        AssertAttribute(output, "hx-swap", "innerHTML");
    }

    [Fact]
    public async Task Sets_Custom_HxSwap()
    {
        var helper = CreateHelper();
        helper.Url = "/api/stream";
        helper.SseSwap = "beforeend";

        var context = CreateContext("rhx-sse-stream");
        var output = CreateOutput("rhx-sse-stream");

        await helper.ProcessAsync(context, output);

        AssertAttribute(output, "hx-swap", "beforeend");
    }

    // ── ARIA ──

    [Fact]
    public async Task Sets_AriaLive_Polite()
    {
        var helper = CreateHelper();
        helper.Url = "/api/stream";

        var context = CreateContext("rhx-sse-stream");
        var output = CreateOutput("rhx-sse-stream");

        await helper.ProcessAsync(context, output);

        AssertAttribute(output, "aria-live", "polite");
    }

    // ── Child content ──

    [Fact]
    public async Task Passes_Through_Child_Content()
    {
        var helper = CreateHelper();
        helper.Url = "/api/stream";

        var context = CreateContext("rhx-sse-stream");
        var output = CreateOutput("rhx-sse-stream", childContent: "<span>Connecting...</span>");

        await helper.ProcessAsync(context, output);

        var content = output.Content.GetContent();
        Assert.Contains("Connecting...", content);
    }

    // ── Common attributes ──

    [Fact]
    public async Task Custom_CssClass_Appended()
    {
        var helper = CreateHelper();
        helper.Url = "/api/stream";
        helper.CssClass = "my-stream";

        var context = CreateContext("rhx-sse-stream");
        var output = CreateOutput("rhx-sse-stream");

        await helper.ProcessAsync(context, output);

        Assert.True(HasClass(output, "rhx-sse-stream"));
        Assert.True(HasClass(output, "my-stream"));
    }

    [Fact]
    public async Task Id_Sets_Attribute()
    {
        var helper = CreateHelper();
        helper.Url = "/api/stream";
        helper.Id = "live-feed";

        var context = CreateContext("rhx-sse-stream");
        var output = CreateOutput("rhx-sse-stream");

        await helper.ProcessAsync(context, output);

        AssertAttribute(output, "id", "live-feed");
    }

    [Fact]
    public async Task Merges_HxExt_With_Existing()
    {
        var helper = CreateHelper();
        helper.Url = "/api/stream";
        helper.HxExt = "json-enc";

        var context = CreateContext("rhx-sse-stream");
        var output = CreateOutput("rhx-sse-stream");

        await helper.ProcessAsync(context, output);

        var ext = GetAttribute(output, "hx-ext");
        Assert.Contains("sse", ext);
        Assert.Contains("json-enc", ext);
    }

    [Fact]
    public async Task CloseOn_Sets_SseClose()
    {
        var helper = CreateHelper();
        helper.Url = "/api/stream";
        helper.CloseOnEvent = "done";

        var context = CreateContext("rhx-sse-stream");
        var output = CreateOutput("rhx-sse-stream");

        await helper.ProcessAsync(context, output);

        AssertAttribute(output, "sse-close", "done");
    }

    [Fact]
    public async Task No_SseConnect_When_No_Url_Or_Page()
    {
        var helper = CreateHelper();
        // Neither Url nor Page set

        var context = CreateContext("rhx-sse-stream");
        var output = CreateOutput("rhx-sse-stream");

        await helper.ProcessAsync(context, output);

        AssertNoAttribute(output, "sse-connect");
    }
}
