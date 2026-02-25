using htmxRazor.Components.Feedback;
using Xunit;

namespace htmxRazor.Tests;

public class SkeletonTagHelperTests : TagHelperTestBase
{
    private SkeletonTagHelper CreateHelper()
    {
        var helper = new SkeletonTagHelper(CreateUrlHelperFactory());
        helper.ViewContext = CreateViewContext();
        return helper;
    }

    // ──────────────────────────────────────────────
    //  Default rendering
    // ──────────────────────────────────────────────

    [Fact]
    public void Renders_Div_Element()
    {
        var helper = CreateHelper();
        var context = CreateContext("rhx-skeleton");
        var output = CreateOutput("rhx-skeleton");

        helper.Process(context, output);

        Assert.Equal("div", output.TagName);
    }

    [Fact]
    public void Renders_Block_Class()
    {
        var helper = CreateHelper();
        var context = CreateContext("rhx-skeleton");
        var output = CreateOutput("rhx-skeleton");

        helper.Process(context, output);

        Assert.True(HasClass(output, "rhx-skeleton"));
    }

    [Fact]
    public void Renders_Aria_Hidden()
    {
        var helper = CreateHelper();
        var context = CreateContext("rhx-skeleton");
        var output = CreateOutput("rhx-skeleton");

        helper.Process(context, output);

        AssertAttribute(output, "aria-hidden", "true");
    }

    [Fact]
    public void Default_Effect_Is_Sheen()
    {
        var helper = CreateHelper();
        var context = CreateContext("rhx-skeleton");
        var output = CreateOutput("rhx-skeleton");

        helper.Process(context, output);

        Assert.True(HasClass(output, "rhx-skeleton--sheen"));
    }

    [Fact]
    public void Default_Shape_Rounded_No_Modifier()
    {
        var helper = CreateHelper();
        var context = CreateContext("rhx-skeleton");
        var output = CreateOutput("rhx-skeleton");

        helper.Process(context, output);

        Assert.False(HasClass(output, "rhx-skeleton--rounded"));
        Assert.False(HasClass(output, "rhx-skeleton--circle"));
        Assert.False(HasClass(output, "rhx-skeleton--rectangle"));
    }

    [Fact]
    public void Default_Dimensions_In_Style()
    {
        var helper = CreateHelper();
        var context = CreateContext("rhx-skeleton");
        var output = CreateOutput("rhx-skeleton");

        helper.Process(context, output);

        var style = GetAttribute(output, "style") ?? "";
        Assert.Contains("width: 100%", style);
        Assert.Contains("height: 1rem", style);
        Assert.Contains("border-radius: var(--rhx-radius-md)", style);
    }

    // ──────────────────────────────────────────────
    //  Effects
    // ──────────────────────────────────────────────

    [Fact]
    public void Pulse_Effect_Adds_Modifier()
    {
        var helper = CreateHelper();
        helper.Effect = "pulse";
        var context = CreateContext("rhx-skeleton");
        var output = CreateOutput("rhx-skeleton");

        helper.Process(context, output);

        Assert.True(HasClass(output, "rhx-skeleton--pulse"));
        Assert.False(HasClass(output, "rhx-skeleton--sheen"));
    }

    [Fact]
    public void None_Effect_No_Modifier()
    {
        var helper = CreateHelper();
        helper.Effect = "none";
        var context = CreateContext("rhx-skeleton");
        var output = CreateOutput("rhx-skeleton");

        helper.Process(context, output);

        Assert.False(HasClass(output, "rhx-skeleton--none"));
        Assert.False(HasClass(output, "rhx-skeleton--sheen"));
        Assert.False(HasClass(output, "rhx-skeleton--pulse"));
    }

    [Fact]
    public void Effect_Is_Case_Insensitive()
    {
        var helper = CreateHelper();
        helper.Effect = "Pulse";
        var context = CreateContext("rhx-skeleton");
        var output = CreateOutput("rhx-skeleton");

        helper.Process(context, output);

        Assert.True(HasClass(output, "rhx-skeleton--pulse"));
    }

    // ──────────────────────────────────────────────
    //  Shapes
    // ──────────────────────────────────────────────

    [Fact]
    public void Circle_Shape_Adds_Modifier_And_BorderRadius()
    {
        var helper = CreateHelper();
        helper.Shape = "circle";
        var context = CreateContext("rhx-skeleton");
        var output = CreateOutput("rhx-skeleton");

        helper.Process(context, output);

        Assert.True(HasClass(output, "rhx-skeleton--circle"));
        var style = GetAttribute(output, "style") ?? "";
        Assert.Contains("border-radius: var(--rhx-radius-full)", style);
    }

    [Fact]
    public void Rectangle_Shape_Adds_Modifier_And_BorderRadius()
    {
        var helper = CreateHelper();
        helper.Shape = "rectangle";
        var context = CreateContext("rhx-skeleton");
        var output = CreateOutput("rhx-skeleton");

        helper.Process(context, output);

        Assert.True(HasClass(output, "rhx-skeleton--rectangle"));
        var style = GetAttribute(output, "style") ?? "";
        Assert.Contains("border-radius: 0", style);
    }

    // ──────────────────────────────────────────────
    //  Custom dimensions
    // ──────────────────────────────────────────────

    [Fact]
    public void Custom_Width_And_Height()
    {
        var helper = CreateHelper();
        helper.Width = "200px";
        helper.Height = "24px";
        var context = CreateContext("rhx-skeleton");
        var output = CreateOutput("rhx-skeleton");

        helper.Process(context, output);

        var style = GetAttribute(output, "style") ?? "";
        Assert.Contains("width: 200px", style);
        Assert.Contains("height: 24px", style);
    }

    // ──────────────────────────────────────────────
    //  Custom CSS class
    // ──────────────────────────────────────────────

    [Fact]
    public void Custom_CssClass_Appended()
    {
        var helper = CreateHelper();
        helper.CssClass = "my-skeleton";
        var context = CreateContext("rhx-skeleton");
        var output = CreateOutput("rhx-skeleton");

        helper.Process(context, output);

        Assert.True(HasClass(output, "my-skeleton"));
        Assert.True(HasClass(output, "rhx-skeleton"));
    }
}
