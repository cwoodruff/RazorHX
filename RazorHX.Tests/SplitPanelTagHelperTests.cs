using RazorHX.Components.Organization;
using RazorHX.Rendering;
using Xunit;

namespace RazorHX.Tests;

public class SplitPanelTagHelperTests : TagHelperTestBase
{
    // ──────────────────────────────────────────────
    //  Helpers
    // ──────────────────────────────────────────────

    private SplitPanelTagHelper CreateHelper()
    {
        var helper = new SplitPanelTagHelper(CreateUrlHelperFactory());
        helper.ViewContext = CreateViewContext();
        return helper;
    }

    // ══════════════════════════════════════════════
    //  SplitPanelTagHelper — Structure
    // ══════════════════════════════════════════════

    [Fact]
    public async Task Renders_Div_Element()
    {
        var helper = CreateHelper();
        var context = CreateContext("rhx-split-panel");
        var output = CreateOutput("rhx-split-panel", childContent: "");

        await helper.ProcessAsync(context, output);

        Assert.Equal("div", output.TagName);
    }

    [Fact]
    public async Task Has_Block_Class()
    {
        var helper = CreateHelper();
        var context = CreateContext("rhx-split-panel");
        var output = CreateOutput("rhx-split-panel", childContent: "");

        await helper.ProcessAsync(context, output);

        Assert.True(HasClass(output, "rhx-split-panel"));
    }

    [Fact]
    public async Task Has_Data_Attribute()
    {
        var helper = CreateHelper();
        var context = CreateContext("rhx-split-panel");
        var output = CreateOutput("rhx-split-panel", childContent: "");

        await helper.ProcessAsync(context, output);

        AssertAttribute(output, "data-rhx-split-panel", "");
    }

    [Fact]
    public async Task Default_Position_50()
    {
        var helper = CreateHelper();
        var context = CreateContext("rhx-split-panel");
        var output = CreateOutput("rhx-split-panel", childContent: "");

        await helper.ProcessAsync(context, output);

        AssertAttribute(output, "data-rhx-position", "50");
        var content = output.Content.GetContent();
        Assert.Contains("flex-basis: 50%", content);
    }

    [Fact]
    public async Task Custom_Position()
    {
        var helper = CreateHelper();
        helper.Position = 30;
        var context = CreateContext("rhx-split-panel");
        var output = CreateOutput("rhx-split-panel", childContent: "");

        await helper.ProcessAsync(context, output);

        AssertAttribute(output, "data-rhx-position", "30");
        var content = output.Content.GetContent();
        Assert.Contains("flex-basis: 30%", content);
    }

    [Fact]
    public async Task Contains_Start_Panel()
    {
        var helper = CreateHelper();
        var context = CreateContext("rhx-split-panel");
        var output = CreateOutput("rhx-split-panel", childContent: "");

        await helper.ProcessAsync(context, output);

        var content = output.Content.GetContent();
        Assert.Contains("rhx-split-panel__start", content);
    }

    [Fact]
    public async Task Contains_End_Panel()
    {
        var helper = CreateHelper();
        var context = CreateContext("rhx-split-panel");
        var output = CreateOutput("rhx-split-panel", childContent: "");

        await helper.ProcessAsync(context, output);

        var content = output.Content.GetContent();
        Assert.Contains("rhx-split-panel__end", content);
    }

    [Fact]
    public async Task Contains_Divider_With_Separator_Role()
    {
        var helper = CreateHelper();
        var context = CreateContext("rhx-split-panel");
        var output = CreateOutput("rhx-split-panel", childContent: "");

        await helper.ProcessAsync(context, output);

        var content = output.Content.GetContent();
        Assert.Contains("rhx-split-panel__divider", content);
        Assert.Contains("role=\"separator\"", content);
    }

    [Fact]
    public async Task Divider_Has_Aria_Values()
    {
        var helper = CreateHelper();
        helper.Position = 40;
        var context = CreateContext("rhx-split-panel");
        var output = CreateOutput("rhx-split-panel", childContent: "");

        await helper.ProcessAsync(context, output);

        var content = output.Content.GetContent();
        Assert.Contains("aria-valuenow=\"40\"", content);
        Assert.Contains("aria-valuemin=\"0\"", content);
        Assert.Contains("aria-valuemax=\"100\"", content);
    }

    [Fact]
    public async Task Divider_Has_Handle()
    {
        var helper = CreateHelper();
        var context = CreateContext("rhx-split-panel");
        var output = CreateOutput("rhx-split-panel", childContent: "");

        await helper.ProcessAsync(context, output);

        var content = output.Content.GetContent();
        Assert.Contains("rhx-split-panel__divider-handle", content);
    }

    [Fact]
    public async Task Divider_Has_Tabindex_0()
    {
        var helper = CreateHelper();
        var context = CreateContext("rhx-split-panel");
        var output = CreateOutput("rhx-split-panel", childContent: "");

        await helper.ProcessAsync(context, output);

        var content = output.Content.GetContent();
        Assert.Contains("tabindex=\"0\"", content);
    }

    [Fact]
    public async Task Default_Horizontal_Orientation()
    {
        var helper = CreateHelper();
        var context = CreateContext("rhx-split-panel");
        var output = CreateOutput("rhx-split-panel", childContent: "");

        await helper.ProcessAsync(context, output);

        var content = output.Content.GetContent();
        Assert.Contains("aria-orientation=\"vertical\"", content);
        Assert.False(HasClass(output, "rhx-split-panel--vertical"));
    }

    // ══════════════════════════════════════════════
    //  Vertical
    // ══════════════════════════════════════════════

    [Fact]
    public async Task Vertical_Modifier()
    {
        var helper = CreateHelper();
        helper.Vertical = true;
        var context = CreateContext("rhx-split-panel");
        var output = CreateOutput("rhx-split-panel", childContent: "");

        await helper.ProcessAsync(context, output);

        Assert.True(HasClass(output, "rhx-split-panel--vertical"));
    }

    [Fact]
    public async Task Vertical_Data_Attribute()
    {
        var helper = CreateHelper();
        helper.Vertical = true;
        var context = CreateContext("rhx-split-panel");
        var output = CreateOutput("rhx-split-panel", childContent: "");

        await helper.ProcessAsync(context, output);

        AssertAttribute(output, "data-rhx-vertical", "");
    }

    [Fact]
    public async Task Vertical_Divider_Has_Horizontal_Orientation()
    {
        var helper = CreateHelper();
        helper.Vertical = true;
        var context = CreateContext("rhx-split-panel");
        var output = CreateOutput("rhx-split-panel", childContent: "");

        await helper.ProcessAsync(context, output);

        var content = output.Content.GetContent();
        Assert.Contains("aria-orientation=\"horizontal\"", content);
    }

    // ══════════════════════════════════════════════
    //  Disabled
    // ══════════════════════════════════════════════

    [Fact]
    public async Task Disabled_Modifier()
    {
        var helper = CreateHelper();
        helper.Disabled = true;
        var context = CreateContext("rhx-split-panel");
        var output = CreateOutput("rhx-split-panel", childContent: "");

        await helper.ProcessAsync(context, output);

        Assert.True(HasClass(output, "rhx-split-panel--disabled"));
    }

    [Fact]
    public async Task Disabled_Data_Attribute()
    {
        var helper = CreateHelper();
        helper.Disabled = true;
        var context = CreateContext("rhx-split-panel");
        var output = CreateOutput("rhx-split-panel", childContent: "");

        await helper.ProcessAsync(context, output);

        AssertAttribute(output, "data-rhx-disabled", "");
    }

    [Fact]
    public async Task Disabled_Divider_Has_Tabindex_Minus1()
    {
        var helper = CreateHelper();
        helper.Disabled = true;
        var context = CreateContext("rhx-split-panel");
        var output = CreateOutput("rhx-split-panel", childContent: "");

        await helper.ProcessAsync(context, output);

        var content = output.Content.GetContent();
        Assert.Contains("tabindex=\"-1\"", content);
    }

    // ══════════════════════════════════════════════
    //  Snap & Primary
    // ══════════════════════════════════════════════

    [Fact]
    public async Task Snap_Data_Attribute()
    {
        var helper = CreateHelper();
        helper.Snap = "25,50,75";
        var context = CreateContext("rhx-split-panel");
        var output = CreateOutput("rhx-split-panel", childContent: "");

        await helper.ProcessAsync(context, output);

        AssertAttribute(output, "data-rhx-snap", "25,50,75");
    }

    [Fact]
    public async Task Custom_SnapThreshold()
    {
        var helper = CreateHelper();
        helper.SnapThreshold = 20;
        var context = CreateContext("rhx-split-panel");
        var output = CreateOutput("rhx-split-panel", childContent: "");

        await helper.ProcessAsync(context, output);

        AssertAttribute(output, "data-rhx-snap-threshold", "20");
    }

    [Fact]
    public async Task Default_SnapThreshold_Not_Rendered()
    {
        var helper = CreateHelper();
        var context = CreateContext("rhx-split-panel");
        var output = CreateOutput("rhx-split-panel", childContent: "");

        await helper.ProcessAsync(context, output);

        AssertNoAttribute(output, "data-rhx-snap-threshold");
    }

    [Fact]
    public async Task Primary_Data_Attribute()
    {
        var helper = CreateHelper();
        helper.Primary = "start";
        var context = CreateContext("rhx-split-panel");
        var output = CreateOutput("rhx-split-panel", childContent: "");

        await helper.ProcessAsync(context, output);

        AssertAttribute(output, "data-rhx-primary", "start");
    }

    [Fact]
    public async Task No_Primary_No_Attribute()
    {
        var helper = CreateHelper();
        var context = CreateContext("rhx-split-panel");
        var output = CreateOutput("rhx-split-panel", childContent: "");

        await helper.ProcessAsync(context, output);

        AssertNoAttribute(output, "data-rhx-primary");
    }

    // ══════════════════════════════════════════════
    //  Custom CSS & htmx
    // ══════════════════════════════════════════════

    [Fact]
    public async Task Custom_CssClass_Appended()
    {
        var helper = CreateHelper();
        helper.CssClass = "my-split";
        var context = CreateContext("rhx-split-panel");
        var output = CreateOutput("rhx-split-panel", childContent: "");

        await helper.ProcessAsync(context, output);

        Assert.True(HasClass(output, "my-split"));
        Assert.True(HasClass(output, "rhx-split-panel"));
    }

    [Fact]
    public async Task Renders_Htmx_Attributes()
    {
        var helper = CreateHelper();
        helper.HxGet = "/api/panel";
        var context = CreateContext("rhx-split-panel");
        var output = CreateOutput("rhx-split-panel", childContent: "");

        await helper.ProcessAsync(context, output);

        AssertAttribute(output, "hx-get", "/api/panel");
    }

    // ══════════════════════════════════════════════
    //  Slot children
    // ══════════════════════════════════════════════

    [Fact]
    public async Task Start_Slot_Content_Rendered()
    {
        var helper = CreateHelper();
        var context = CreateContext("rhx-split-panel");

        var output = new Microsoft.AspNetCore.Razor.TagHelpers.TagHelperOutput(
            tagName: "rhx-split-panel",
            attributes: new Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttributeList(),
            getChildContentAsync: (useCachedResult, encoder) =>
            {
                var slots = SlotRenderer.FromContext(context)!;
                slots.SetHtml("start", "<p>Sidebar</p>");
                slots.SetHtml("end", "<p>Main</p>");
                var content = new Microsoft.AspNetCore.Razor.TagHelpers.DefaultTagHelperContent();
                return Task.FromResult<Microsoft.AspNetCore.Razor.TagHelpers.TagHelperContent>(content);
            });

        await helper.ProcessAsync(context, output);

        var html = output.Content.GetContent();
        Assert.Contains("<p>Sidebar</p>", html);
        Assert.Contains("<p>Main</p>", html);
    }

    [Fact]
    public async Task Start_Suppresses_Output()
    {
        var startHelper = new SplitPanelStartTagHelper();
        var context = CreateContext("rhx-split-start");
        SlotRenderer.CreateForContext(context);
        var output = CreateOutput("rhx-split-start", childContent: "Sidebar");

        await startHelper.ProcessAsync(context, output);

        Assert.True(output.IsContentModified == false || output.Content.GetContent() == "");
    }

    [Fact]
    public async Task Start_Registers_Slot()
    {
        var startHelper = new SplitPanelStartTagHelper();
        var context = CreateContext("rhx-split-start");
        var slots = SlotRenderer.CreateForContext(context);
        var output = CreateOutput("rhx-split-start", childContent: "Sidebar");

        await startHelper.ProcessAsync(context, output);

        Assert.True(slots.Has("start"));
    }

    [Fact]
    public async Task End_Suppresses_Output()
    {
        var endHelper = new SplitPanelEndTagHelper();
        var context = CreateContext("rhx-split-end");
        SlotRenderer.CreateForContext(context);
        var output = CreateOutput("rhx-split-end", childContent: "Main");

        await endHelper.ProcessAsync(context, output);

        Assert.True(output.IsContentModified == false || output.Content.GetContent() == "");
    }

    [Fact]
    public async Task End_Registers_Slot()
    {
        var endHelper = new SplitPanelEndTagHelper();
        var context = CreateContext("rhx-split-end");
        var slots = SlotRenderer.CreateForContext(context);
        var output = CreateOutput("rhx-split-end", childContent: "Main");

        await endHelper.ProcessAsync(context, output);

        Assert.True(slots.Has("end"));
    }

    [Fact]
    public async Task Panels_Render_In_Order()
    {
        var helper = CreateHelper();
        var context = CreateContext("rhx-split-panel");

        var output = new Microsoft.AspNetCore.Razor.TagHelpers.TagHelperOutput(
            tagName: "rhx-split-panel",
            attributes: new Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttributeList(),
            getChildContentAsync: (useCachedResult, encoder) =>
            {
                var slots = SlotRenderer.FromContext(context)!;
                slots.SetHtml("start", "<p>Start</p>");
                slots.SetHtml("end", "<p>End</p>");
                var content = new Microsoft.AspNetCore.Razor.TagHelpers.DefaultTagHelperContent();
                return Task.FromResult<Microsoft.AspNetCore.Razor.TagHelpers.TagHelperContent>(content);
            });

        await helper.ProcessAsync(context, output);

        var html = output.Content.GetContent();
        var startIdx = html.IndexOf("rhx-split-panel__start");
        var dividerIdx = html.IndexOf("rhx-split-panel__divider");
        var endIdx = html.IndexOf("rhx-split-panel__end");

        Assert.True(startIdx < dividerIdx, "Start should appear before divider");
        Assert.True(dividerIdx < endIdx, "Divider should appear before end");
    }

    // ══════════════════════════════════════════════
    //  Position clamping
    // ══════════════════════════════════════════════

    [Fact]
    public async Task Position_Clamped_To_0()
    {
        var helper = CreateHelper();
        helper.Position = -10;
        var context = CreateContext("rhx-split-panel");
        var output = CreateOutput("rhx-split-panel", childContent: "");

        await helper.ProcessAsync(context, output);

        var content = output.Content.GetContent();
        Assert.Contains("flex-basis: 0%", content);
        Assert.Contains("aria-valuenow=\"0\"", content);
    }

    [Fact]
    public async Task Position_Clamped_To_100()
    {
        var helper = CreateHelper();
        helper.Position = 150;
        var context = CreateContext("rhx-split-panel");
        var output = CreateOutput("rhx-split-panel", childContent: "");

        await helper.ProcessAsync(context, output);

        var content = output.Content.GetContent();
        Assert.Contains("flex-basis: 100%", content);
        Assert.Contains("aria-valuenow=\"100\"", content);
    }
}
