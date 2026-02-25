using htmxRazor.Components.Imagery;
using Xunit;

namespace htmxRazor.Tests;

public class AnimatedImageTagHelperTests : TagHelperTestBase
{
    private AnimatedImageTagHelper CreateHelper()
    {
        var helper = new AnimatedImageTagHelper(CreateUrlHelperFactory());
        helper.ViewContext = CreateViewContext();
        return helper;
    }

    // ══════════════════════════════════════════════
    //  Structure
    // ══════════════════════════════════════════════

    [Fact]
    public void Renders_Div_Element()
    {
        var helper = CreateHelper();
        helper.Src = "animation.gif";
        var context = CreateContext("rhx-animated-image");
        var output = CreateOutput("rhx-animated-image");

        helper.Process(context, output);

        Assert.Equal("div", output.TagName);
    }

    [Fact]
    public void Has_Block_Class()
    {
        var helper = CreateHelper();
        helper.Src = "animation.gif";
        var context = CreateContext("rhx-animated-image");
        var output = CreateOutput("rhx-animated-image");

        helper.Process(context, output);

        Assert.True(HasClass(output, "rhx-animated-image"));
    }

    [Fact]
    public void Has_Data_Attribute()
    {
        var helper = CreateHelper();
        helper.Src = "animation.gif";
        var context = CreateContext("rhx-animated-image");
        var output = CreateOutput("rhx-animated-image");

        helper.Process(context, output);

        AssertAttribute(output, "data-rhx-animated-image", "");
    }

    [Fact]
    public void Contains_Img()
    {
        var helper = CreateHelper();
        helper.Src = "animation.gif";
        helper.Alt = "Spinner";
        var context = CreateContext("rhx-animated-image");
        var output = CreateOutput("rhx-animated-image");

        helper.Process(context, output);

        var content = output.Content.GetContent();
        Assert.Contains("rhx-animated-image__img", content);
        Assert.Contains("src=\"animation.gif\"", content);
        Assert.Contains("alt=\"Spinner\"", content);
    }

    [Fact]
    public void Contains_Canvas()
    {
        var helper = CreateHelper();
        helper.Src = "animation.gif";
        var context = CreateContext("rhx-animated-image");
        var output = CreateOutput("rhx-animated-image");

        helper.Process(context, output);

        var content = output.Content.GetContent();
        Assert.Contains("rhx-animated-image__canvas", content);
        Assert.Contains("aria-hidden=\"true\"", content);
    }

    [Fact]
    public void Contains_Control_Button()
    {
        var helper = CreateHelper();
        helper.Src = "animation.gif";
        var context = CreateContext("rhx-animated-image");
        var output = CreateOutput("rhx-animated-image");

        helper.Process(context, output);

        var content = output.Content.GetContent();
        Assert.Contains("rhx-animated-image__control", content);
        Assert.Contains("type=\"button\"", content);
    }

    // ══════════════════════════════════════════════
    //  Play / Pause state
    // ══════════════════════════════════════════════

    [Fact]
    public void Default_Playing()
    {
        var helper = CreateHelper();
        helper.Src = "animation.gif";
        var context = CreateContext("rhx-animated-image");
        var output = CreateOutput("rhx-animated-image");

        helper.Process(context, output);

        Assert.False(HasClass(output, "rhx-animated-image--paused"));
        AssertNoAttribute(output, "data-rhx-paused");
        var content = output.Content.GetContent();
        Assert.Contains("Pause animation", content);
    }

    [Fact]
    public void Paused_State()
    {
        var helper = CreateHelper();
        helper.Src = "animation.gif";
        helper.Play = false;
        var context = CreateContext("rhx-animated-image");
        var output = CreateOutput("rhx-animated-image");

        helper.Process(context, output);

        Assert.True(HasClass(output, "rhx-animated-image--paused"));
        AssertAttribute(output, "data-rhx-paused", "");
        var content = output.Content.GetContent();
        Assert.Contains("Play animation", content);
    }

    // ══════════════════════════════════════════════
    //  htmx
    // ══════════════════════════════════════════════

    [Fact]
    public void Renders_Htmx_Attributes()
    {
        var helper = CreateHelper();
        helper.Src = "animation.gif";
        helper.HxGet = "/api/frame";
        var context = CreateContext("rhx-animated-image");
        var output = CreateOutput("rhx-animated-image");

        helper.Process(context, output);

        AssertAttribute(output, "hx-get", "/api/frame");
    }
}
