using RazorHX.Components.Feedback;
using Xunit;

namespace RazorHX.Tests;

public class TooltipTagHelperTests : TagHelperTestBase
{
    private TooltipTagHelper CreateHelper()
    {
        return new TooltipTagHelper();
    }

    // ──────────────────────────────────────────────
    //  Default rendering
    // ──────────────────────────────────────────────

    [Fact]
    public async Task Renders_Span_Element()
    {
        var helper = CreateHelper();
        var context = CreateContext("rhx-tooltip");
        var output = CreateOutput("rhx-tooltip", childContent: "<button>Save</button>");

        await helper.ProcessAsync(context, output);

        Assert.Equal("span", output.TagName);
    }

    [Fact]
    public async Task Renders_Data_Rhx_Tooltip_Attribute()
    {
        var helper = CreateHelper();
        helper.Content = "Save your changes";
        var context = CreateContext("rhx-tooltip");
        var output = CreateOutput("rhx-tooltip", childContent: "<button>Save</button>");

        await helper.ProcessAsync(context, output);

        AssertAttribute(output, "data-rhx-tooltip", "Save your changes");
    }

    [Fact]
    public async Task Default_Placement_Is_Top()
    {
        var helper = CreateHelper();
        helper.Content = "Tip";
        var context = CreateContext("rhx-tooltip");
        var output = CreateOutput("rhx-tooltip", childContent: "text");

        await helper.ProcessAsync(context, output);

        AssertAttribute(output, "data-rhx-tooltip-placement", "top");
    }

    [Fact]
    public async Task Default_Not_Disabled()
    {
        var helper = CreateHelper();
        helper.Content = "Tip";
        var context = CreateContext("rhx-tooltip");
        var output = CreateOutput("rhx-tooltip", childContent: "text");

        await helper.ProcessAsync(context, output);

        AssertNoAttribute(output, "data-rhx-tooltip-disabled");
    }

    [Fact]
    public async Task Default_Trigger_Not_Rendered()
    {
        var helper = CreateHelper();
        helper.Content = "Tip";
        var context = CreateContext("rhx-tooltip");
        var output = CreateOutput("rhx-tooltip", childContent: "text");

        await helper.ProcessAsync(context, output);

        // Default "hover focus" is not rendered as attribute
        AssertNoAttribute(output, "data-rhx-tooltip-trigger");
    }

    [Fact]
    public async Task Preserves_Child_Content()
    {
        var helper = CreateHelper();
        helper.Content = "Tip";
        var context = CreateContext("rhx-tooltip");
        var output = CreateOutput("rhx-tooltip", childContent: "Save");

        await helper.ProcessAsync(context, output);

        var content = output.Content.GetContent();
        Assert.Contains("Save", content);
    }

    // ──────────────────────────────────────────────
    //  Placement
    // ──────────────────────────────────────────────

    [Theory]
    [InlineData("top")]
    [InlineData("bottom")]
    [InlineData("left")]
    [InlineData("right")]
    public async Task Custom_Placement(string placement)
    {
        var helper = CreateHelper();
        helper.Content = "Tip";
        helper.Placement = placement;
        var context = CreateContext("rhx-tooltip");
        var output = CreateOutput("rhx-tooltip", childContent: "text");

        await helper.ProcessAsync(context, output);

        AssertAttribute(output, "data-rhx-tooltip-placement", placement);
    }

    [Fact]
    public async Task Placement_Is_Case_Insensitive()
    {
        var helper = CreateHelper();
        helper.Content = "Tip";
        helper.Placement = "Bottom";
        var context = CreateContext("rhx-tooltip");
        var output = CreateOutput("rhx-tooltip", childContent: "text");

        await helper.ProcessAsync(context, output);

        AssertAttribute(output, "data-rhx-tooltip-placement", "bottom");
    }

    // ──────────────────────────────────────────────
    //  Disabled
    // ──────────────────────────────────────────────

    [Fact]
    public async Task Disabled_Sets_Attribute()
    {
        var helper = CreateHelper();
        helper.Content = "Tip";
        helper.Disabled = true;
        var context = CreateContext("rhx-tooltip");
        var output = CreateOutput("rhx-tooltip", childContent: "text");

        await helper.ProcessAsync(context, output);

        Assert.True(output.Attributes.TryGetAttribute("data-rhx-tooltip-disabled", out _));
    }

    // ──────────────────────────────────────────────
    //  Trigger
    // ──────────────────────────────────────────────

    [Fact]
    public async Task Custom_Trigger_Sets_Attribute()
    {
        var helper = CreateHelper();
        helper.Content = "Tip";
        helper.Trigger = "click";
        var context = CreateContext("rhx-tooltip");
        var output = CreateOutput("rhx-tooltip", childContent: "text");

        await helper.ProcessAsync(context, output);

        AssertAttribute(output, "data-rhx-tooltip-trigger", "click");
    }

    [Fact]
    public async Task Hover_Only_Trigger()
    {
        var helper = CreateHelper();
        helper.Content = "Tip";
        helper.Trigger = "hover";
        var context = CreateContext("rhx-tooltip");
        var output = CreateOutput("rhx-tooltip", childContent: "text");

        await helper.ProcessAsync(context, output);

        AssertAttribute(output, "data-rhx-tooltip-trigger", "hover");
    }

    // ──────────────────────────────────────────────
    //  Content encoding
    // ──────────────────────────────────────────────

    [Fact]
    public async Task Content_Is_Html_Encoded()
    {
        var helper = CreateHelper();
        helper.Content = "Use <b>bold</b>";
        var context = CreateContext("rhx-tooltip");
        var output = CreateOutput("rhx-tooltip", childContent: "text");

        await helper.ProcessAsync(context, output);

        var attr = GetAttribute(output, "data-rhx-tooltip") ?? "";
        Assert.DoesNotContain("<b>", attr);
        Assert.Contains("&lt;b&gt;", attr);
    }

    // ──────────────────────────────────────────────
    //  Tag mode
    // ──────────────────────────────────────────────

    [Fact]
    public async Task Uses_StartTagAndEndTag_Mode()
    {
        var helper = CreateHelper();
        helper.Content = "Tip";
        var context = CreateContext("rhx-tooltip");
        var output = CreateOutput("rhx-tooltip", childContent: "text");

        await helper.ProcessAsync(context, output);

        Assert.Equal(Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, output.TagMode);
    }
}
