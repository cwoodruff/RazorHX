using Microsoft.AspNetCore.Razor.TagHelpers;
using htmxRazor.Components.Organization;
using htmxRazor.Rendering;
using Xunit;

namespace htmxRazor.Tests;

public class TimelineItemTagHelperTests : TagHelperTestBase
{
    private TimelineItemTagHelper CreateHelper(string? generatedUrl = "/generated-url")
    {
        var helper = new TimelineItemTagHelper(CreateUrlHelperFactory(generatedUrl));
        helper.ViewContext = CreateViewContext();
        return helper;
    }

    // ── Element ──

    [Fact]
    public async Task Renders_Div_Element()
    {
        var helper = CreateHelper();

        var context = CreateContext("rhx-timeline-item");
        var output = CreateOutput("rhx-timeline-item", childContent: "Event happened");

        await helper.ProcessAsync(context, output);

        Assert.Equal("div", output.TagName);
    }

    [Fact]
    public async Task Has_Block_Class()
    {
        var helper = CreateHelper();

        var context = CreateContext("rhx-timeline-item");
        var output = CreateOutput("rhx-timeline-item", childContent: "Event");

        await helper.ProcessAsync(context, output);

        Assert.True(HasClass(output, "rhx-timeline-item"));
    }

    // ── Variants ──

    [Fact]
    public async Task Default_Variant_Neutral()
    {
        var helper = CreateHelper();

        var context = CreateContext("rhx-timeline-item");
        var output = CreateOutput("rhx-timeline-item", childContent: "Event");

        await helper.ProcessAsync(context, output);

        Assert.False(HasClass(output, "rhx-timeline-item--neutral"));
    }

    [Fact]
    public async Task Brand_Variant()
    {
        var helper = CreateHelper();
        helper.Variant = "brand";

        var context = CreateContext("rhx-timeline-item");
        var output = CreateOutput("rhx-timeline-item", childContent: "Event");

        await helper.ProcessAsync(context, output);

        Assert.True(HasClass(output, "rhx-timeline-item--brand"));
    }

    [Fact]
    public async Task Success_Variant()
    {
        var helper = CreateHelper();
        helper.Variant = "success";

        var context = CreateContext("rhx-timeline-item");
        var output = CreateOutput("rhx-timeline-item", childContent: "Event");

        await helper.ProcessAsync(context, output);

        Assert.True(HasClass(output, "rhx-timeline-item--success"));
    }

    [Fact]
    public async Task Warning_Variant()
    {
        var helper = CreateHelper();
        helper.Variant = "warning";

        var context = CreateContext("rhx-timeline-item");
        var output = CreateOutput("rhx-timeline-item", childContent: "Event");

        await helper.ProcessAsync(context, output);

        Assert.True(HasClass(output, "rhx-timeline-item--warning"));
    }

    [Fact]
    public async Task Danger_Variant()
    {
        var helper = CreateHelper();
        helper.Variant = "danger";

        var context = CreateContext("rhx-timeline-item");
        var output = CreateOutput("rhx-timeline-item", childContent: "Event");

        await helper.ProcessAsync(context, output);

        Assert.True(HasClass(output, "rhx-timeline-item--danger"));
    }

    // ── Active state ──

    [Fact]
    public async Task Active_Modifier()
    {
        var helper = CreateHelper();
        helper.Active = true;

        var context = CreateContext("rhx-timeline-item");
        var output = CreateOutput("rhx-timeline-item", childContent: "Event");

        await helper.ProcessAsync(context, output);

        Assert.True(HasClass(output, "rhx-timeline-item--active"));
    }

    // ── ARIA ──

    [Fact]
    public async Task Has_Role_Listitem()
    {
        var helper = CreateHelper();

        var context = CreateContext("rhx-timeline-item");
        var output = CreateOutput("rhx-timeline-item", childContent: "Event");

        await helper.ProcessAsync(context, output);

        AssertAttribute(output, "role", "listitem");
    }

    [Fact]
    public async Task Active_Sets_AriaCurrent()
    {
        var helper = CreateHelper();
        helper.Active = true;

        var context = CreateContext("rhx-timeline-item");
        var output = CreateOutput("rhx-timeline-item", childContent: "Event");

        await helper.ProcessAsync(context, output);

        AssertAttribute(output, "aria-current", "step");
    }

    // ── Label ──

    [Fact]
    public async Task Renders_Label()
    {
        var helper = CreateHelper();
        helper.Label = "March 15, 2026";

        var context = CreateContext("rhx-timeline-item");
        var output = CreateOutput("rhx-timeline-item", childContent: "Event");

        await helper.ProcessAsync(context, output);

        var content = output.Content.GetContent();
        Assert.Contains("rhx-timeline-item__label", content);
        Assert.Contains("March 15, 2026", content);
    }

    [Fact]
    public async Task No_Label_When_Not_Set()
    {
        var helper = CreateHelper();
        // Label not set

        var context = CreateContext("rhx-timeline-item");
        var output = CreateOutput("rhx-timeline-item", childContent: "Event");

        await helper.ProcessAsync(context, output);

        var content = output.Content.GetContent();
        Assert.DoesNotContain("rhx-timeline-item__label", content);
    }

    // ── Connector ──

    [Fact]
    public async Task Renders_Connector()
    {
        var helper = CreateHelper();

        var context = CreateContext("rhx-timeline-item");
        var output = CreateOutput("rhx-timeline-item", childContent: "Event");

        await helper.ProcessAsync(context, output);

        var content = output.Content.GetContent();
        Assert.Contains("rhx-timeline-item__connector", content);
        Assert.Contains("rhx-timeline-item__dot", content);
        Assert.Contains("rhx-timeline-item__line", content);
    }

    // ── Icon slot ──

    [Fact]
    public async Task Icon_Slot_Rendered_In_Dot()
    {
        var helper = CreateHelper();

        var context = CreateContext("rhx-timeline-item");
        // Pre-populate the icon slot to simulate TimelineIconTagHelper
        var slots = SlotRenderer.CreateForContext(context);
        var iconContent = new DefaultTagHelperContent();
        iconContent.SetHtmlContent("<svg>check</svg>");
        slots.Set("icon", iconContent);

        var output = CreateOutput("rhx-timeline-item", childContent: "Event");

        await helper.ProcessAsync(context, output);

        var content = output.Content.GetContent();
        Assert.Contains("<svg>check</svg>", content);
    }

    // ── Body content ──

    [Fact]
    public async Task Passes_Through_Child_Content_As_Body()
    {
        var helper = CreateHelper();

        var context = CreateContext("rhx-timeline-item");
        var output = CreateOutput("rhx-timeline-item", childContent: "Order shipped");

        await helper.ProcessAsync(context, output);

        var content = output.Content.GetContent();
        Assert.Contains("rhx-timeline-item__body", content);
        Assert.Contains("Order shipped", content);
    }

    // ── Common attributes ──

    [Fact]
    public async Task Custom_CssClass_Appended()
    {
        var helper = CreateHelper();
        helper.CssClass = "my-item";

        var context = CreateContext("rhx-timeline-item");
        var output = CreateOutput("rhx-timeline-item", childContent: "Event");

        await helper.ProcessAsync(context, output);

        Assert.True(HasClass(output, "rhx-timeline-item"));
        Assert.True(HasClass(output, "my-item"));
    }

    [Fact]
    public async Task HxGet_Forwarded()
    {
        var helper = CreateHelper();
        helper.HxGet = "/api/detail";

        var context = CreateContext("rhx-timeline-item");
        var output = CreateOutput("rhx-timeline-item", childContent: "Event");

        await helper.ProcessAsync(context, output);

        AssertAttribute(output, "hx-get", "/api/detail");
    }
}
