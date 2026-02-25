using htmxRazor.Components.Organization;
using Xunit;

namespace htmxRazor.Tests;

public class ScrollerTagHelperTests : TagHelperTestBase
{
    // ──────────────────────────────────────────────
    //  Helpers
    // ──────────────────────────────────────────────

    private ScrollerTagHelper CreateHelper()
    {
        var helper = new ScrollerTagHelper(CreateUrlHelperFactory());
        helper.ViewContext = CreateViewContext();
        return helper;
    }

    // ══════════════════════════════════════════════
    //  Structure
    // ══════════════════════════════════════════════

    [Fact]
    public async Task Renders_Div_Element()
    {
        var helper = CreateHelper();
        var context = CreateContext("rhx-scroller");
        var output = CreateOutput("rhx-scroller", childContent: "");

        await helper.ProcessAsync(context, output);

        Assert.Equal("div", output.TagName);
    }

    [Fact]
    public async Task Has_Block_Class()
    {
        var helper = CreateHelper();
        var context = CreateContext("rhx-scroller");
        var output = CreateOutput("rhx-scroller", childContent: "");

        await helper.ProcessAsync(context, output);

        Assert.True(HasClass(output, "rhx-scroller"));
    }

    [Fact]
    public async Task Has_Data_Attribute()
    {
        var helper = CreateHelper();
        var context = CreateContext("rhx-scroller");
        var output = CreateOutput("rhx-scroller", childContent: "");

        await helper.ProcessAsync(context, output);

        AssertAttribute(output, "data-rhx-scroller", "");
    }

    [Fact]
    public async Task Contains_Content_Wrapper()
    {
        var helper = CreateHelper();
        var context = CreateContext("rhx-scroller");
        var output = CreateOutput("rhx-scroller", childContent: "");

        await helper.ProcessAsync(context, output);

        var content = output.Content.GetContent();
        Assert.Contains("rhx-scroller__content", content);
    }

    [Fact]
    public async Task Contains_Start_Shadow()
    {
        var helper = CreateHelper();
        var context = CreateContext("rhx-scroller");
        var output = CreateOutput("rhx-scroller", childContent: "");

        await helper.ProcessAsync(context, output);

        var content = output.Content.GetContent();
        Assert.Contains("rhx-scroller__shadow--start", content);
    }

    [Fact]
    public async Task Contains_End_Shadow()
    {
        var helper = CreateHelper();
        var context = CreateContext("rhx-scroller");
        var output = CreateOutput("rhx-scroller", childContent: "");

        await helper.ProcessAsync(context, output);

        var content = output.Content.GetContent();
        Assert.Contains("rhx-scroller__shadow--end", content);
    }

    [Fact]
    public async Task Shadows_Have_AriaHidden()
    {
        var helper = CreateHelper();
        var context = CreateContext("rhx-scroller");
        var output = CreateOutput("rhx-scroller", childContent: "");

        await helper.ProcessAsync(context, output);

        var content = output.Content.GetContent();
        // Both shadows should have aria-hidden
        var count = content.Split("aria-hidden=\"true\"").Length - 1;
        Assert.Equal(2, count);
    }

    [Fact]
    public async Task Shadows_Have_Base_And_Direction_Classes()
    {
        var helper = CreateHelper();
        var context = CreateContext("rhx-scroller");
        var output = CreateOutput("rhx-scroller", childContent: "");

        await helper.ProcessAsync(context, output);

        var content = output.Content.GetContent();
        Assert.Contains("rhx-scroller__shadow rhx-scroller__shadow--start", content);
        Assert.Contains("rhx-scroller__shadow rhx-scroller__shadow--end", content);
    }

    // ══════════════════════════════════════════════
    //  Orientation
    // ══════════════════════════════════════════════

    [Fact]
    public async Task Default_Orientation_Horizontal()
    {
        var helper = CreateHelper();
        var context = CreateContext("rhx-scroller");
        var output = CreateOutput("rhx-scroller", childContent: "");

        await helper.ProcessAsync(context, output);

        Assert.True(HasClass(output, "rhx-scroller--horizontal"));
        AssertAttribute(output, "data-rhx-orientation", "horizontal");
    }

    [Fact]
    public async Task Orientation_Vertical()
    {
        var helper = CreateHelper();
        helper.Orientation = "vertical";
        var context = CreateContext("rhx-scroller");
        var output = CreateOutput("rhx-scroller", childContent: "");

        await helper.ProcessAsync(context, output);

        Assert.True(HasClass(output, "rhx-scroller--vertical"));
        AssertAttribute(output, "data-rhx-orientation", "vertical");
    }

    [Fact]
    public async Task Orientation_Both()
    {
        var helper = CreateHelper();
        helper.Orientation = "both";
        var context = CreateContext("rhx-scroller");
        var output = CreateOutput("rhx-scroller", childContent: "");

        await helper.ProcessAsync(context, output);

        Assert.True(HasClass(output, "rhx-scroller--both"));
        AssertAttribute(output, "data-rhx-orientation", "both");
    }

    [Fact]
    public async Task Orientation_Case_Insensitive()
    {
        var helper = CreateHelper();
        helper.Orientation = "Vertical";
        var context = CreateContext("rhx-scroller");
        var output = CreateOutput("rhx-scroller", childContent: "");

        await helper.ProcessAsync(context, output);

        Assert.True(HasClass(output, "rhx-scroller--vertical"));
        AssertAttribute(output, "data-rhx-orientation", "vertical");
    }

    // ══════════════════════════════════════════════
    //  Content ordering
    // ══════════════════════════════════════════════

    [Fact]
    public async Task Content_Before_Shadows()
    {
        var helper = CreateHelper();
        var context = CreateContext("rhx-scroller");
        var output = CreateOutput("rhx-scroller", childContent: "");

        await helper.ProcessAsync(context, output);

        var html = output.Content.GetContent();
        var contentIdx = html.IndexOf("rhx-scroller__content");
        var shadowIdx = html.IndexOf("rhx-scroller__shadow");
        Assert.True(contentIdx < shadowIdx);
    }

    [Fact]
    public async Task Start_Shadow_Before_End_Shadow()
    {
        var helper = CreateHelper();
        var context = CreateContext("rhx-scroller");
        var output = CreateOutput("rhx-scroller", childContent: "");

        await helper.ProcessAsync(context, output);

        var html = output.Content.GetContent();
        var startIdx = html.IndexOf("rhx-scroller__shadow--start");
        var endIdx = html.IndexOf("rhx-scroller__shadow--end");
        Assert.True(startIdx < endIdx);
    }

    // ══════════════════════════════════════════════
    //  Custom CSS
    // ══════════════════════════════════════════════

    [Fact]
    public async Task Custom_CssClass_Appended()
    {
        var helper = CreateHelper();
        helper.CssClass = "my-scroller";
        var context = CreateContext("rhx-scroller");
        var output = CreateOutput("rhx-scroller", childContent: "");

        await helper.ProcessAsync(context, output);

        Assert.True(HasClass(output, "my-scroller"));
        Assert.True(HasClass(output, "rhx-scroller"));
    }

    // ══════════════════════════════════════════════
    //  htmx support
    // ══════════════════════════════════════════════

    [Fact]
    public async Task Renders_Htmx_Attributes()
    {
        var helper = CreateHelper();
        helper.HxGet = "/api/items";
        helper.HxTrigger = "revealed";
        helper.HxTarget = "find .rhx-scroller__content";
        var context = CreateContext("rhx-scroller");
        var output = CreateOutput("rhx-scroller", childContent: "");

        await helper.ProcessAsync(context, output);

        AssertAttribute(output, "hx-get", "/api/items");
        AssertAttribute(output, "hx-trigger", "revealed");
        AssertAttribute(output, "hx-target", "find .rhx-scroller__content");
    }
}
