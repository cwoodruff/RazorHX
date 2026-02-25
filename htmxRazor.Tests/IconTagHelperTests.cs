using htmxRazor.Components.Imagery;
using Xunit;

namespace htmxRazor.Tests;

public class IconTagHelperTests : TagHelperTestBase
{
    // ══════════════════════════════════════════════
    //  IconRegistry
    // ══════════════════════════════════════════════

    [Fact]
    public void Registry_Has_Builtin_Icons()
    {
        Assert.True(IconRegistry.Has("check"));
        Assert.True(IconRegistry.Has("x"));
        Assert.True(IconRegistry.Has("search"));
        Assert.True(IconRegistry.Has("home"));
    }

    [Fact]
    public void Registry_Case_Insensitive()
    {
        Assert.True(IconRegistry.Has("CHECK"));
        Assert.True(IconRegistry.Has("Check"));
    }

    [Fact]
    public void Registry_Get_Returns_Svg()
    {
        var svg = IconRegistry.Get("check");
        Assert.NotNull(svg);
        Assert.Contains("path", svg);
    }

    [Fact]
    public void Registry_Get_Unknown_Returns_Null()
    {
        Assert.Null(IconRegistry.Get("nonexistent-icon-xyz"));
    }

    [Fact]
    public void Registry_Custom_Icon()
    {
        IconRegistry.Register("test-custom", "<circle cx=\"12\" cy=\"12\" r=\"10\" />");
        Assert.True(IconRegistry.Has("test-custom"));
        Assert.Contains("circle", IconRegistry.Get("test-custom")!);
    }

    [Fact]
    public void Registry_GetNames_Returns_Icons()
    {
        var names = IconRegistry.GetNames().ToList();
        Assert.True(names.Count >= 40);
    }

    // ══════════════════════════════════════════════
    //  IconTagHelper — Structure
    // ══════════════════════════════════════════════

    [Fact]
    public void Renders_Svg_Element()
    {
        var helper = new IconTagHelper { Name = "check" };
        var context = CreateContext("rhx-icon");
        var output = CreateOutput("rhx-icon");

        helper.Process(context, output);

        Assert.Equal("svg", output.TagName);
    }

    [Fact]
    public void Has_Block_Class()
    {
        var helper = new IconTagHelper { Name = "check" };
        var context = CreateContext("rhx-icon");
        var output = CreateOutput("rhx-icon");

        helper.Process(context, output);

        Assert.True(HasClass(output, "rhx-icon"));
    }

    [Fact]
    public void Has_ViewBox()
    {
        var helper = new IconTagHelper { Name = "check" };
        var context = CreateContext("rhx-icon");
        var output = CreateOutput("rhx-icon");

        helper.Process(context, output);

        AssertAttribute(output, "viewBox", "0 0 24 24");
    }

    [Fact]
    public void Has_Stroke_Attributes()
    {
        var helper = new IconTagHelper { Name = "check" };
        var context = CreateContext("rhx-icon");
        var output = CreateOutput("rhx-icon");

        helper.Process(context, output);

        AssertAttribute(output, "stroke", "currentColor");
        AssertAttribute(output, "fill", "none");
        AssertAttribute(output, "stroke-width", "2");
    }

    [Fact]
    public void Contains_Svg_Content()
    {
        var helper = new IconTagHelper { Name = "check" };
        var context = CreateContext("rhx-icon");
        var output = CreateOutput("rhx-icon");

        helper.Process(context, output);

        var content = output.Content.GetContent();
        Assert.Contains("path", content);
    }

    // ── Decorative (default) ──

    [Fact]
    public void Default_AriaHidden()
    {
        var helper = new IconTagHelper { Name = "check" };
        var context = CreateContext("rhx-icon");
        var output = CreateOutput("rhx-icon");

        helper.Process(context, output);

        AssertAttribute(output, "aria-hidden", "true");
        AssertNoAttribute(output, "role");
    }

    // ── Accessible (with label) ──

    [Fact]
    public void Label_Sets_AriaLabel()
    {
        var helper = new IconTagHelper { Name = "check", Label = "Checkmark" };
        var context = CreateContext("rhx-icon");
        var output = CreateOutput("rhx-icon");

        helper.Process(context, output);

        AssertAttribute(output, "aria-label", "Checkmark");
        AssertAttribute(output, "role", "img");
        AssertNoAttribute(output, "aria-hidden");
    }

    // ── Size ──

    [Fact]
    public void Size_Small()
    {
        var helper = new IconTagHelper { Name = "check", Size = "small" };
        var context = CreateContext("rhx-icon");
        var output = CreateOutput("rhx-icon");

        helper.Process(context, output);

        Assert.True(HasClass(output, "rhx-icon--small"));
    }

    [Fact]
    public void Size_Large()
    {
        var helper = new IconTagHelper { Name = "check", Size = "large" };
        var context = CreateContext("rhx-icon");
        var output = CreateOutput("rhx-icon");

        helper.Process(context, output);

        Assert.True(HasClass(output, "rhx-icon--large"));
    }

    // ── Unknown icon ──

    [Fact]
    public void Unknown_Icon_Suppresses_Output()
    {
        var helper = new IconTagHelper { Name = "nonexistent-xyz" };
        var context = CreateContext("rhx-icon");
        var output = CreateOutput("rhx-icon");

        helper.Process(context, output);

        Assert.True(output.IsContentModified == false || output.Content.GetContent() == "");
    }

    // ── Custom CSS ──

    [Fact]
    public void Custom_CssClass()
    {
        var helper = new IconTagHelper { Name = "check", CssClass = "my-icon" };
        var context = CreateContext("rhx-icon");
        var output = CreateOutput("rhx-icon");

        helper.Process(context, output);

        Assert.True(HasClass(output, "my-icon"));
        Assert.True(HasClass(output, "rhx-icon"));
    }
}
