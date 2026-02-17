using RazorHX.Components.Organization;
using Xunit;

namespace RazorHX.Tests;

public class DividerTagHelperTests : TagHelperTestBase
{
    // ──────────────────────────────────────────────
    //  Helpers
    // ──────────────────────────────────────────────

    private static DividerTagHelper CreateHelper()
    {
        var helper = new DividerTagHelper();
        helper.ViewContext = CreateViewContext();
        return helper;
    }

    // ══════════════════════════════════════════════
    //  Horizontal (default)
    // ══════════════════════════════════════════════

    [Fact]
    public void Renders_Hr_Element()
    {
        var helper = CreateHelper();
        var context = CreateContext("rhx-divider");
        var output = CreateOutput("rhx-divider");

        helper.Process(context, output);

        Assert.Equal("hr", output.TagName);
    }

    [Fact]
    public void Has_Block_Class()
    {
        var helper = CreateHelper();
        var context = CreateContext("rhx-divider");
        var output = CreateOutput("rhx-divider");

        helper.Process(context, output);

        Assert.True(HasClass(output, "rhx-divider"));
    }

    [Fact]
    public void Horizontal_Has_No_Role()
    {
        var helper = CreateHelper();
        var context = CreateContext("rhx-divider");
        var output = CreateOutput("rhx-divider");

        helper.Process(context, output);

        AssertNoAttribute(output, "role");
    }

    [Fact]
    public void Horizontal_Has_No_Vertical_Modifier()
    {
        var helper = CreateHelper();
        var context = CreateContext("rhx-divider");
        var output = CreateOutput("rhx-divider");

        helper.Process(context, output);

        Assert.False(HasClass(output, "rhx-divider--vertical"));
    }

    [Fact]
    public void Horizontal_Is_SelfClosing()
    {
        var helper = CreateHelper();
        var context = CreateContext("rhx-divider");
        var output = CreateOutput("rhx-divider");

        helper.Process(context, output);

        Assert.Equal(Microsoft.AspNetCore.Razor.TagHelpers.TagMode.SelfClosing, output.TagMode);
    }

    [Fact]
    public void Custom_CssClass_Appended()
    {
        var helper = CreateHelper();
        helper.CssClass = "my-divider";
        var context = CreateContext("rhx-divider");
        var output = CreateOutput("rhx-divider");

        helper.Process(context, output);

        Assert.True(HasClass(output, "my-divider"));
        Assert.True(HasClass(output, "rhx-divider"));
    }

    // ══════════════════════════════════════════════
    //  Vertical
    // ══════════════════════════════════════════════

    [Fact]
    public void Vertical_Renders_Div()
    {
        var helper = CreateHelper();
        helper.Vertical = true;
        var context = CreateContext("rhx-divider");
        var output = CreateOutput("rhx-divider");

        helper.Process(context, output);

        Assert.Equal("div", output.TagName);
    }

    [Fact]
    public void Vertical_Has_Modifier_Class()
    {
        var helper = CreateHelper();
        helper.Vertical = true;
        var context = CreateContext("rhx-divider");
        var output = CreateOutput("rhx-divider");

        helper.Process(context, output);

        Assert.True(HasClass(output, "rhx-divider--vertical"));
    }

    [Fact]
    public void Vertical_Has_Separator_Role()
    {
        var helper = CreateHelper();
        helper.Vertical = true;
        var context = CreateContext("rhx-divider");
        var output = CreateOutput("rhx-divider");

        helper.Process(context, output);

        AssertAttribute(output, "role", "separator");
    }

    [Fact]
    public void Vertical_Has_Aria_Orientation()
    {
        var helper = CreateHelper();
        helper.Vertical = true;
        var context = CreateContext("rhx-divider");
        var output = CreateOutput("rhx-divider");

        helper.Process(context, output);

        AssertAttribute(output, "aria-orientation", "vertical");
    }

    [Fact]
    public void Vertical_Is_StartTagAndEndTag()
    {
        var helper = CreateHelper();
        helper.Vertical = true;
        var context = CreateContext("rhx-divider");
        var output = CreateOutput("rhx-divider");

        helper.Process(context, output);

        Assert.Equal(Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, output.TagMode);
    }
}
