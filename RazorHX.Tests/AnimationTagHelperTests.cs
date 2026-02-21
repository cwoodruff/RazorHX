using RazorHX.Components.Utilities;
using Xunit;

namespace RazorHX.Tests;

public class AnimationTagHelperTests : TagHelperTestBase
{
    private AnimationTagHelper CreateHelper()
    {
        return new AnimationTagHelper { ViewContext = CreateViewContext() };
    }

    // ══════════════════════════════════════════════
    //  Structure
    // ══════════════════════════════════════════════

    [Fact]
    public void Renders_Div_Element()
    {
        var helper = CreateHelper();
        var context = CreateContext("rhx-animation");
        var output = CreateOutput("rhx-animation");

        helper.Process(context, output);

        Assert.Equal("div", output.TagName);
    }

    [Fact]
    public void Has_Block_Class()
    {
        var helper = CreateHelper();
        var context = CreateContext("rhx-animation");
        var output = CreateOutput("rhx-animation");

        helper.Process(context, output);

        Assert.True(HasClass(output, "rhx-animation"));
    }

    [Fact]
    public void Has_Data_Attribute()
    {
        var helper = CreateHelper();
        var context = CreateContext("rhx-animation");
        var output = CreateOutput("rhx-animation");

        helper.Process(context, output);

        Assert.NotNull(GetAttribute(output, "data-rhx-animation"));
    }

    [Fact]
    public void Custom_Class_Merged()
    {
        var helper = CreateHelper();
        helper.CssClass = "my-anim";
        var context = CreateContext("rhx-animation");
        var output = CreateOutput("rhx-animation");

        helper.Process(context, output);

        Assert.True(HasClass(output, "rhx-animation"));
        Assert.True(HasClass(output, "my-anim"));
    }

    // ══════════════════════════════════════════════
    //  Default data attributes
    // ══════════════════════════════════════════════

    [Fact]
    public void Default_Animation_Name()
    {
        var helper = CreateHelper();
        var context = CreateContext("rhx-animation");
        var output = CreateOutput("rhx-animation");

        helper.Process(context, output);

        AssertAttribute(output, "data-rhx-animation", "fadeIn");
    }

    [Fact]
    public void Default_Duration()
    {
        var helper = CreateHelper();
        var context = CreateContext("rhx-animation");
        var output = CreateOutput("rhx-animation");

        helper.Process(context, output);

        AssertAttribute(output, "data-rhx-duration", "300");
    }

    [Fact]
    public void Default_Delay()
    {
        var helper = CreateHelper();
        var context = CreateContext("rhx-animation");
        var output = CreateOutput("rhx-animation");

        helper.Process(context, output);

        AssertAttribute(output, "data-rhx-delay", "0");
    }

    [Fact]
    public void Default_No_Direction_Attribute()
    {
        var helper = CreateHelper();
        var context = CreateContext("rhx-animation");
        var output = CreateOutput("rhx-animation");

        helper.Process(context, output);

        AssertNoAttribute(output, "data-rhx-direction");
    }

    [Fact]
    public void Default_No_Easing_Attribute()
    {
        var helper = CreateHelper();
        var context = CreateContext("rhx-animation");
        var output = CreateOutput("rhx-animation");

        helper.Process(context, output);

        AssertNoAttribute(output, "data-rhx-easing");
    }

    [Fact]
    public void Default_No_Iterations_Attribute()
    {
        var helper = CreateHelper();
        var context = CreateContext("rhx-animation");
        var output = CreateOutput("rhx-animation");

        helper.Process(context, output);

        AssertNoAttribute(output, "data-rhx-iterations");
    }

    [Fact]
    public void Default_No_Fill_Attribute()
    {
        var helper = CreateHelper();
        var context = CreateContext("rhx-animation");
        var output = CreateOutput("rhx-animation");

        helper.Process(context, output);

        // Default is "both", not rendered
        AssertNoAttribute(output, "data-rhx-fill");
    }

    [Fact]
    public void Default_Play_Adds_Playing_Modifier()
    {
        var helper = CreateHelper();
        var context = CreateContext("rhx-animation");
        var output = CreateOutput("rhx-animation");

        helper.Process(context, output);

        Assert.True(HasClass(output, "rhx-animation--playing"));
    }

    [Fact]
    public void Default_No_Paused_Attribute()
    {
        var helper = CreateHelper();
        var context = CreateContext("rhx-animation");
        var output = CreateOutput("rhx-animation");

        helper.Process(context, output);

        AssertNoAttribute(output, "data-rhx-paused");
    }

    // ══════════════════════════════════════════════
    //  Custom properties
    // ══════════════════════════════════════════════

    [Fact]
    public void Custom_Animation_Name()
    {
        var helper = CreateHelper();
        helper.Name = "slideInLeft";
        var context = CreateContext("rhx-animation");
        var output = CreateOutput("rhx-animation");

        helper.Process(context, output);

        AssertAttribute(output, "data-rhx-animation", "slideInLeft");
    }

    [Fact]
    public void Custom_Duration()
    {
        var helper = CreateHelper();
        helper.Duration = 500;
        var context = CreateContext("rhx-animation");
        var output = CreateOutput("rhx-animation");

        helper.Process(context, output);

        AssertAttribute(output, "data-rhx-duration", "500");
    }

    [Fact]
    public void Custom_Delay()
    {
        var helper = CreateHelper();
        helper.Delay = 200;
        var context = CreateContext("rhx-animation");
        var output = CreateOutput("rhx-animation");

        helper.Process(context, output);

        AssertAttribute(output, "data-rhx-delay", "200");
    }

    [Fact]
    public void Custom_Direction()
    {
        var helper = CreateHelper();
        helper.Direction = "reverse";
        var context = CreateContext("rhx-animation");
        var output = CreateOutput("rhx-animation");

        helper.Process(context, output);

        AssertAttribute(output, "data-rhx-direction", "reverse");
    }

    [Fact]
    public void Normal_Direction_Not_Rendered()
    {
        var helper = CreateHelper();
        helper.Direction = "normal";
        var context = CreateContext("rhx-animation");
        var output = CreateOutput("rhx-animation");

        helper.Process(context, output);

        AssertNoAttribute(output, "data-rhx-direction");
    }

    [Fact]
    public void Custom_Easing()
    {
        var helper = CreateHelper();
        helper.Easing = "ease-in-out";
        var context = CreateContext("rhx-animation");
        var output = CreateOutput("rhx-animation");

        helper.Process(context, output);

        AssertAttribute(output, "data-rhx-easing", "ease-in-out");
    }

    [Fact]
    public void Custom_Iterations()
    {
        var helper = CreateHelper();
        helper.Iterations = "infinite";
        var context = CreateContext("rhx-animation");
        var output = CreateOutput("rhx-animation");

        helper.Process(context, output);

        AssertAttribute(output, "data-rhx-iterations", "infinite");
    }

    [Fact]
    public void Iterations_One_Not_Rendered()
    {
        var helper = CreateHelper();
        helper.Iterations = "1";
        var context = CreateContext("rhx-animation");
        var output = CreateOutput("rhx-animation");

        helper.Process(context, output);

        AssertNoAttribute(output, "data-rhx-iterations");
    }

    [Fact]
    public void Custom_Fill()
    {
        var helper = CreateHelper();
        helper.Fill = "forwards";
        var context = CreateContext("rhx-animation");
        var output = CreateOutput("rhx-animation");

        helper.Process(context, output);

        AssertAttribute(output, "data-rhx-fill", "forwards");
    }

    [Fact]
    public void Paused_Adds_Data_Attribute()
    {
        var helper = CreateHelper();
        helper.Play = false;
        var context = CreateContext("rhx-animation");
        var output = CreateOutput("rhx-animation");

        helper.Process(context, output);

        Assert.True(output.Attributes.TryGetAttribute("data-rhx-paused", out _));
    }

    [Fact]
    public void Paused_No_Playing_Modifier()
    {
        var helper = CreateHelper();
        helper.Play = false;
        var context = CreateContext("rhx-animation");
        var output = CreateOutput("rhx-animation");

        helper.Process(context, output);

        Assert.False(HasClass(output, "rhx-animation--playing"));
    }

    // ══════════════════════════════════════════════
    //  htmx
    // ══════════════════════════════════════════════

    [Fact]
    public void Htmx_Attributes_Rendered()
    {
        var helper = CreateHelper();
        helper.HxGet = "/api/content";
        helper.HxTarget = "#result";
        helper.HxSwap = "innerHTML";
        var context = CreateContext("rhx-animation");
        var output = CreateOutput("rhx-animation");

        helper.Process(context, output);

        AssertAttribute(output, "hx-get", "/api/content");
        AssertAttribute(output, "hx-target", "#result");
        AssertAttribute(output, "hx-swap", "innerHTML");
    }

    // ══════════════════════════════════════════════
    //  Id and hidden
    // ══════════════════════════════════════════════

    [Fact]
    public void Custom_Id()
    {
        var helper = CreateHelper();
        helper.Id = "my-animation";
        var context = CreateContext("rhx-animation");
        var output = CreateOutput("rhx-animation");

        helper.Process(context, output);

        AssertAttribute(output, "id", "my-animation");
    }

    [Fact]
    public void Hidden_Attribute()
    {
        var helper = CreateHelper();
        helper.Hidden = true;
        var context = CreateContext("rhx-animation");
        var output = CreateOutput("rhx-animation");

        helper.Process(context, output);

        Assert.True(output.Attributes.TryGetAttribute("hidden", out _));
    }
}
