using htmxRazor.Components.Overlays;
using Xunit;

namespace htmxRazor.Tests;

public class DetailsTagHelperTests : TagHelperTestBase
{
    // ──────────────────────────────────────────────
    //  Helpers
    // ──────────────────────────────────────────────

    private DetailsTagHelper CreateHelper()
    {
        var helper = new DetailsTagHelper(CreateUrlHelperFactory());
        helper.ViewContext = CreateViewContext();
        return helper;
    }

    // ══════════════════════════════════════════════
    //  DetailsTagHelper — Structure
    // ══════════════════════════════════════════════

    [Fact]
    public async Task Renders_Details_Element()
    {
        var helper = CreateHelper();
        var context = CreateContext("rhx-details");
        var output = CreateOutput("rhx-details", childContent: "");

        await helper.ProcessAsync(context, output);

        Assert.Equal("details", output.TagName);
    }

    [Fact]
    public async Task Has_Block_Class()
    {
        var helper = CreateHelper();
        var context = CreateContext("rhx-details");
        var output = CreateOutput("rhx-details", childContent: "");

        await helper.ProcessAsync(context, output);

        Assert.True(HasClass(output, "rhx-details"));
    }

    [Fact]
    public async Task Has_Data_Attribute()
    {
        var helper = CreateHelper();
        var context = CreateContext("rhx-details");
        var output = CreateOutput("rhx-details", childContent: "");

        await helper.ProcessAsync(context, output);

        AssertAttribute(output, "data-rhx-details", "");
    }

    [Fact]
    public async Task Contains_Summary()
    {
        var helper = CreateHelper();
        var context = CreateContext("rhx-details");
        var output = CreateOutput("rhx-details", childContent: "");

        await helper.ProcessAsync(context, output);

        var content = output.Content.GetContent();
        Assert.Contains("<summary class=\"rhx-details__summary\">", content);
    }

    [Fact]
    public async Task Summary_Has_Icon()
    {
        var helper = CreateHelper();
        var context = CreateContext("rhx-details");
        var output = CreateOutput("rhx-details", childContent: "");

        await helper.ProcessAsync(context, output);

        var content = output.Content.GetContent();
        Assert.Contains("rhx-details__summary-icon", content);
        Assert.Contains("aria-hidden=\"true\"", content);
    }

    [Fact]
    public async Task Summary_Has_Text()
    {
        var helper = CreateHelper();
        helper.Summary = "Order History";
        var context = CreateContext("rhx-details");
        var output = CreateOutput("rhx-details", childContent: "");

        await helper.ProcessAsync(context, output);

        var content = output.Content.GetContent();
        Assert.Contains("rhx-details__summary-text", content);
        Assert.Contains("Order History", content);
    }

    [Fact]
    public async Task Default_Summary_Text()
    {
        var helper = CreateHelper();
        var context = CreateContext("rhx-details");
        var output = CreateOutput("rhx-details", childContent: "");

        await helper.ProcessAsync(context, output);

        var content = output.Content.GetContent();
        Assert.Contains(">Details</span>", content);
    }

    [Fact]
    public async Task Summary_Is_HtmlEncoded()
    {
        var helper = CreateHelper();
        helper.Summary = "A & B";
        var context = CreateContext("rhx-details");
        var output = CreateOutput("rhx-details", childContent: "");

        await helper.ProcessAsync(context, output);

        var content = output.Content.GetContent();
        Assert.Contains("A &amp; B", content);
    }

    [Fact]
    public async Task Contains_Content_Div()
    {
        var helper = CreateHelper();
        var context = CreateContext("rhx-details");
        var output = CreateOutput("rhx-details", childContent: "");

        await helper.ProcessAsync(context, output);

        var content = output.Content.GetContent();
        Assert.Contains("rhx-details__content", content);
    }

    // ══════════════════════════════════════════════
    //  Open state
    // ══════════════════════════════════════════════

    [Fact]
    public async Task Open_Sets_Attribute()
    {
        var helper = CreateHelper();
        helper.Open = true;
        var context = CreateContext("rhx-details");
        var output = CreateOutput("rhx-details", childContent: "");

        await helper.ProcessAsync(context, output);

        AssertAttribute(output, "open", "open");
    }

    [Fact]
    public async Task Not_Open_No_Attribute()
    {
        var helper = CreateHelper();
        var context = CreateContext("rhx-details");
        var output = CreateOutput("rhx-details", childContent: "");

        await helper.ProcessAsync(context, output);

        AssertNoAttribute(output, "open");
    }

    // ══════════════════════════════════════════════
    //  Disabled state
    // ══════════════════════════════════════════════

    [Fact]
    public async Task Disabled_Has_Modifier()
    {
        var helper = CreateHelper();
        helper.Disabled = true;
        var context = CreateContext("rhx-details");
        var output = CreateOutput("rhx-details", childContent: "");

        await helper.ProcessAsync(context, output);

        Assert.True(HasClass(output, "rhx-details--disabled"));
    }

    [Fact]
    public async Task Disabled_Has_Data_Attribute()
    {
        var helper = CreateHelper();
        helper.Disabled = true;
        var context = CreateContext("rhx-details");
        var output = CreateOutput("rhx-details", childContent: "");

        await helper.ProcessAsync(context, output);

        AssertAttribute(output, "data-rhx-disabled", "");
    }

    [Fact]
    public async Task Not_Disabled_No_Modifier()
    {
        var helper = CreateHelper();
        var context = CreateContext("rhx-details");
        var output = CreateOutput("rhx-details", childContent: "");

        await helper.ProcessAsync(context, output);

        Assert.False(HasClass(output, "rhx-details--disabled"));
    }

    // ══════════════════════════════════════════════
    //  htmx support
    // ══════════════════════════════════════════════

    [Fact]
    public async Task Renders_Htmx_Attributes()
    {
        var helper = CreateHelper();
        helper.HxGet = "/api/orders";
        helper.HxTrigger = "toggle once";
        helper.HxTarget = "find .rhx-details__content";
        var context = CreateContext("rhx-details");
        var output = CreateOutput("rhx-details", childContent: "");

        await helper.ProcessAsync(context, output);

        AssertAttribute(output, "hx-get", "/api/orders");
        AssertAttribute(output, "hx-trigger", "toggle once");
        AssertAttribute(output, "hx-target", "find .rhx-details__content");
    }

    // ══════════════════════════════════════════════
    //  Custom CSS
    // ══════════════════════════════════════════════

    [Fact]
    public async Task Custom_CssClass_Appended()
    {
        var helper = CreateHelper();
        helper.CssClass = "my-details";
        var context = CreateContext("rhx-details");
        var output = CreateOutput("rhx-details", childContent: "");

        await helper.ProcessAsync(context, output);

        Assert.True(HasClass(output, "my-details"));
        Assert.True(HasClass(output, "rhx-details"));
    }

    // ══════════════════════════════════════════════
    //  Content ordering
    // ══════════════════════════════════════════════

    [Fact]
    public async Task Summary_Before_Content()
    {
        var helper = CreateHelper();
        var context = CreateContext("rhx-details");
        var output = CreateOutput("rhx-details", childContent: "");

        await helper.ProcessAsync(context, output);

        var html = output.Content.GetContent();
        var summaryIdx = html.IndexOf("rhx-details__summary");
        var contentIdx = html.IndexOf("rhx-details__content");
        Assert.True(summaryIdx < contentIdx);
    }
}
