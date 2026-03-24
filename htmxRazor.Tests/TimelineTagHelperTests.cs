using Microsoft.AspNetCore.Razor.TagHelpers;
using htmxRazor.Components.Organization;
using Xunit;

namespace htmxRazor.Tests;

public class TimelineTagHelperTests : TagHelperTestBase
{
    private TimelineTagHelper CreateHelper(string? generatedUrl = "/generated-url")
    {
        var helper = new TimelineTagHelper(CreateUrlHelperFactory(generatedUrl));
        helper.ViewContext = CreateViewContext();
        return helper;
    }

    // ── Element ──

    [Fact]
    public async Task Renders_Div_Element()
    {
        var helper = CreateHelper();

        var context = CreateContext("rhx-timeline");
        var output = CreateOutput("rhx-timeline");

        await helper.ProcessAsync(context, output);

        Assert.Equal("div", output.TagName);
    }

    [Fact]
    public async Task Has_Block_Class()
    {
        var helper = CreateHelper();

        var context = CreateContext("rhx-timeline");
        var output = CreateOutput("rhx-timeline");

        await helper.ProcessAsync(context, output);

        Assert.True(HasClass(output, "rhx-timeline"));
    }

    // ── Layout ──

    [Fact]
    public async Task Default_Layout_Vertical()
    {
        var helper = CreateHelper();

        var context = CreateContext("rhx-timeline");
        var output = CreateOutput("rhx-timeline");

        await helper.ProcessAsync(context, output);

        Assert.True(HasClass(output, "rhx-timeline--vertical"));
    }

    [Fact]
    public async Task Horizontal_Layout()
    {
        var helper = CreateHelper();
        helper.Layout = "horizontal";

        var context = CreateContext("rhx-timeline");
        var output = CreateOutput("rhx-timeline");

        await helper.ProcessAsync(context, output);

        Assert.True(HasClass(output, "rhx-timeline--horizontal"));
    }

    // ── Align ──

    [Fact]
    public async Task Align_Center()
    {
        var helper = CreateHelper();
        helper.Align = "center";

        var context = CreateContext("rhx-timeline");
        var output = CreateOutput("rhx-timeline");

        await helper.ProcessAsync(context, output);

        Assert.True(HasClass(output, "rhx-timeline--center"));
    }

    [Fact]
    public async Task Align_Alternate()
    {
        var helper = CreateHelper();
        helper.Align = "alternate";

        var context = CreateContext("rhx-timeline");
        var output = CreateOutput("rhx-timeline");

        await helper.ProcessAsync(context, output);

        Assert.True(HasClass(output, "rhx-timeline--alternate"));
    }

    // ── ARIA ──

    [Fact]
    public async Task Has_Role_List()
    {
        var helper = CreateHelper();

        var context = CreateContext("rhx-timeline");
        var output = CreateOutput("rhx-timeline");

        await helper.ProcessAsync(context, output);

        AssertAttribute(output, "role", "list");
    }

    // ── Child content ──

    [Fact]
    public async Task Passes_Through_Child_Content()
    {
        var helper = CreateHelper();

        var context = CreateContext("rhx-timeline");
        var output = CreateOutput("rhx-timeline", childContent: "<div>Item 1</div>");

        await helper.ProcessAsync(context, output);

        var content = output.Content.GetContent();
        Assert.Contains("Item 1", content);
    }

    // ── Common attributes ──

    [Fact]
    public async Task Custom_CssClass_Appended()
    {
        var helper = CreateHelper();
        helper.CssClass = "my-timeline";

        var context = CreateContext("rhx-timeline");
        var output = CreateOutput("rhx-timeline");

        await helper.ProcessAsync(context, output);

        Assert.True(HasClass(output, "rhx-timeline"));
        Assert.True(HasClass(output, "my-timeline"));
    }

    [Fact]
    public async Task Id_Sets_Attribute()
    {
        var helper = CreateHelper();
        helper.Id = "project-timeline";

        var context = CreateContext("rhx-timeline");
        var output = CreateOutput("rhx-timeline");

        await helper.ProcessAsync(context, output);

        AssertAttribute(output, "id", "project-timeline");
    }

    [Fact]
    public async Task HxGet_Forwarded()
    {
        var helper = CreateHelper();
        helper.HxGet = "/api/timeline";

        var context = CreateContext("rhx-timeline");
        var output = CreateOutput("rhx-timeline");

        await helper.ProcessAsync(context, output);

        AssertAttribute(output, "hx-get", "/api/timeline");
    }

    [Fact]
    public async Task Hidden_Attribute_Forwarded()
    {
        var helper = CreateHelper();
        helper.Hidden = true;

        var context = CreateContext("rhx-timeline");
        var output = CreateOutput("rhx-timeline");

        await helper.ProcessAsync(context, output);

        AssertAttribute(output, "hidden", "hidden");
    }
}
