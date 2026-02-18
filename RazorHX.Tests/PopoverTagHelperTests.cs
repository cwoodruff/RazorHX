using RazorHX.Components.Utilities;
using Xunit;

namespace RazorHX.Tests;

public class PopoverTagHelperTests : TagHelperTestBase
{
    private PopoverTagHelper CreateHelper()
    {
        return new PopoverTagHelper { ViewContext = CreateViewContext() };
    }

    // ══════════════════════════════════════════════
    //  Structure
    // ══════════════════════════════════════════════

    [Fact]
    public async Task Renders_Div_Element()
    {
        var helper = CreateHelper();
        var context = CreateContext("rhx-popover");
        var output = CreateOutput("rhx-popover", childContent: "<p>Content</p>");

        await helper.ProcessAsync(context, output);

        Assert.Equal("div", output.TagName);
    }

    [Fact]
    public async Task Has_Block_Class()
    {
        var helper = CreateHelper();
        var context = CreateContext("rhx-popover");
        var output = CreateOutput("rhx-popover", childContent: "Content");

        await helper.ProcessAsync(context, output);

        Assert.True(HasClass(output, "rhx-popover"));
    }

    [Fact]
    public async Task Has_Data_Popover_Attribute()
    {
        var helper = CreateHelper();
        var context = CreateContext("rhx-popover");
        var output = CreateOutput("rhx-popover", childContent: "Content");

        await helper.ProcessAsync(context, output);

        Assert.True(output.Attributes.TryGetAttribute("data-rhx-popover", out _));
    }

    [Fact]
    public async Task Has_Generated_Id()
    {
        var helper = CreateHelper();
        var context = CreateContext("rhx-popover");
        var output = CreateOutput("rhx-popover", childContent: "Content");

        await helper.ProcessAsync(context, output);

        var id = GetAttribute(output, "id");
        Assert.NotNull(id);
        Assert.StartsWith("rhx-popover-", id);
    }

    [Fact]
    public async Task Custom_Id_Overrides_Generated()
    {
        var helper = CreateHelper();
        helper.Id = "my-popover";
        var context = CreateContext("rhx-popover");
        var output = CreateOutput("rhx-popover", childContent: "Content");

        await helper.ProcessAsync(context, output);

        AssertAttribute(output, "id", "my-popover");
    }

    [Fact]
    public async Task Custom_Class_Merged()
    {
        var helper = CreateHelper();
        helper.CssClass = "custom-pop";
        var context = CreateContext("rhx-popover");
        var output = CreateOutput("rhx-popover", childContent: "Content");

        await helper.ProcessAsync(context, output);

        Assert.True(HasClass(output, "rhx-popover"));
        Assert.True(HasClass(output, "custom-pop"));
    }

    // ══════════════════════════════════════════════
    //  Content wrapper
    // ══════════════════════════════════════════════

    [Fact]
    public async Task Content_Wrapped_In_Content_Div()
    {
        var helper = CreateHelper();
        var context = CreateContext("rhx-popover");
        var output = CreateOutput("rhx-popover", childContent: "Hello");

        await helper.ProcessAsync(context, output);

        var content = output.Content.GetContent();
        Assert.Contains("rhx-popover__content", content);
    }

    [Fact]
    public async Task Arrow_Rendered_By_Default()
    {
        var helper = CreateHelper();
        var context = CreateContext("rhx-popover");
        var output = CreateOutput("rhx-popover", childContent: "Content");

        await helper.ProcessAsync(context, output);

        var content = output.Content.GetContent();
        Assert.Contains("rhx-popover__arrow", content);
    }

    [Fact]
    public async Task Arrow_Disabled()
    {
        var helper = CreateHelper();
        helper.Arrow = false;
        var context = CreateContext("rhx-popover");
        var output = CreateOutput("rhx-popover", childContent: "Content");

        await helper.ProcessAsync(context, output);

        var content = output.Content.GetContent();
        Assert.DoesNotContain("rhx-popover__arrow", content);
    }

    // ══════════════════════════════════════════════
    //  Default state (closed)
    // ══════════════════════════════════════════════

    [Fact]
    public async Task Default_Hidden()
    {
        var helper = CreateHelper();
        var context = CreateContext("rhx-popover");
        var output = CreateOutput("rhx-popover", childContent: "Content");

        await helper.ProcessAsync(context, output);

        Assert.True(output.Attributes.TryGetAttribute("hidden", out _));
    }

    [Fact]
    public async Task Default_Aria_Hidden()
    {
        var helper = CreateHelper();
        var context = CreateContext("rhx-popover");
        var output = CreateOutput("rhx-popover", childContent: "Content");

        await helper.ProcessAsync(context, output);

        AssertAttribute(output, "aria-hidden", "true");
    }

    [Fact]
    public async Task Default_No_Open_Modifier()
    {
        var helper = CreateHelper();
        var context = CreateContext("rhx-popover");
        var output = CreateOutput("rhx-popover", childContent: "Content");

        await helper.ProcessAsync(context, output);

        Assert.False(HasClass(output, "rhx-popover--open"));
    }

    // ══════════════════════════════════════════════
    //  Open state
    // ══════════════════════════════════════════════

    [Fact]
    public async Task Open_Has_Modifier()
    {
        var helper = CreateHelper();
        helper.Open = true;
        var context = CreateContext("rhx-popover");
        var output = CreateOutput("rhx-popover", childContent: "Content");

        await helper.ProcessAsync(context, output);

        Assert.True(HasClass(output, "rhx-popover--open"));
    }

    [Fact]
    public async Task Open_Not_Hidden()
    {
        var helper = CreateHelper();
        helper.Open = true;
        var context = CreateContext("rhx-popover");
        var output = CreateOutput("rhx-popover", childContent: "Content");

        await helper.ProcessAsync(context, output);

        AssertNoAttribute(output, "hidden");
        AssertNoAttribute(output, "aria-hidden");
    }

    // ══════════════════════════════════════════════
    //  Trigger
    // ══════════════════════════════════════════════

    [Fact]
    public async Task Trigger_Selector()
    {
        var helper = CreateHelper();
        helper.Trigger = "#my-button";
        var context = CreateContext("rhx-popover");
        var output = CreateOutput("rhx-popover", childContent: "Content");

        await helper.ProcessAsync(context, output);

        AssertAttribute(output, "data-rhx-trigger", "#my-button");
    }

    [Fact]
    public async Task Trigger_Previous()
    {
        var helper = CreateHelper();
        helper.Trigger = "previous";
        var context = CreateContext("rhx-popover");
        var output = CreateOutput("rhx-popover", childContent: "Content");

        await helper.ProcessAsync(context, output);

        AssertAttribute(output, "data-rhx-trigger", "previous");
    }

    // ══════════════════════════════════════════════
    //  Placement
    // ══════════════════════════════════════════════

    [Fact]
    public async Task Default_Placement()
    {
        var helper = CreateHelper();
        var context = CreateContext("rhx-popover");
        var output = CreateOutput("rhx-popover", childContent: "Content");

        await helper.ProcessAsync(context, output);

        AssertAttribute(output, "data-rhx-placement", "bottom");
    }

    [Fact]
    public async Task Custom_Placement()
    {
        var helper = CreateHelper();
        helper.Placement = "top-start";
        var context = CreateContext("rhx-popover");
        var output = CreateOutput("rhx-popover", childContent: "Content");

        await helper.ProcessAsync(context, output);

        AssertAttribute(output, "data-rhx-placement", "top-start");
    }

    // ══════════════════════════════════════════════
    //  Distance
    // ══════════════════════════════════════════════

    [Fact]
    public async Task Default_Distance_Not_Rendered()
    {
        var helper = CreateHelper();
        var context = CreateContext("rhx-popover");
        var output = CreateOutput("rhx-popover", childContent: "Content");

        await helper.ProcessAsync(context, output);

        AssertNoAttribute(output, "data-rhx-distance");
    }

    [Fact]
    public async Task Custom_Distance()
    {
        var helper = CreateHelper();
        helper.Distance = 16;
        var context = CreateContext("rhx-popover");
        var output = CreateOutput("rhx-popover", childContent: "Content");

        await helper.ProcessAsync(context, output);

        AssertAttribute(output, "data-rhx-distance", "16");
    }

    // ══════════════════════════════════════════════
    //  Trigger event
    // ══════════════════════════════════════════════

    [Fact]
    public async Task Default_Trigger_Event_Not_Rendered()
    {
        var helper = CreateHelper();
        var context = CreateContext("rhx-popover");
        var output = CreateOutput("rhx-popover", childContent: "Content");

        await helper.ProcessAsync(context, output);

        // Default "click" is not rendered
        AssertNoAttribute(output, "data-rhx-trigger-event");
    }

    [Fact]
    public async Task Hover_Trigger_Event()
    {
        var helper = CreateHelper();
        helper.TriggerEvent = "hover";
        var context = CreateContext("rhx-popover");
        var output = CreateOutput("rhx-popover", childContent: "Content");

        await helper.ProcessAsync(context, output);

        AssertAttribute(output, "data-rhx-trigger-event", "hover");
    }

    [Fact]
    public async Task Focus_Trigger_Event()
    {
        var helper = CreateHelper();
        helper.TriggerEvent = "focus";
        var context = CreateContext("rhx-popover");
        var output = CreateOutput("rhx-popover", childContent: "Content");

        await helper.ProcessAsync(context, output);

        AssertAttribute(output, "data-rhx-trigger-event", "focus");
    }

    // ══════════════════════════════════════════════
    //  htmx
    // ══════════════════════════════════════════════

    [Fact]
    public async Task Htmx_Attributes_Rendered()
    {
        var helper = CreateHelper();
        helper.HxGet = "/api/popover-content";
        helper.HxTarget = "this";
        var context = CreateContext("rhx-popover");
        var output = CreateOutput("rhx-popover", childContent: "Content");

        await helper.ProcessAsync(context, output);

        AssertAttribute(output, "hx-get", "/api/popover-content");
        AssertAttribute(output, "hx-target", "this");
    }
}
