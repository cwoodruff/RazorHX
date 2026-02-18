using RazorHX.Components.Utilities;
using Xunit;

namespace RazorHX.Tests;

public class PopupTagHelperTests : TagHelperTestBase
{
    private PopupTagHelper CreateHelper()
    {
        return new PopupTagHelper { ViewContext = CreateViewContext() };
    }

    // ══════════════════════════════════════════════
    //  Structure
    // ══════════════════════════════════════════════

    [Fact]
    public void Renders_Div_Element()
    {
        var helper = CreateHelper();
        var context = CreateContext("rhx-popup");
        var output = CreateOutput("rhx-popup");

        helper.Process(context, output);

        Assert.Equal("div", output.TagName);
    }

    [Fact]
    public void Has_Block_Class()
    {
        var helper = CreateHelper();
        var context = CreateContext("rhx-popup");
        var output = CreateOutput("rhx-popup");

        helper.Process(context, output);

        Assert.True(HasClass(output, "rhx-popup"));
    }

    [Fact]
    public void Has_Data_Popup_Attribute()
    {
        var helper = CreateHelper();
        var context = CreateContext("rhx-popup");
        var output = CreateOutput("rhx-popup");

        helper.Process(context, output);

        Assert.True(output.Attributes.TryGetAttribute("data-rhx-popup", out _));
    }

    [Fact]
    public void Custom_Class_Merged()
    {
        var helper = CreateHelper();
        helper.CssClass = "my-popup";
        var context = CreateContext("rhx-popup");
        var output = CreateOutput("rhx-popup");

        helper.Process(context, output);

        Assert.True(HasClass(output, "rhx-popup"));
        Assert.True(HasClass(output, "my-popup"));
    }

    // ══════════════════════════════════════════════
    //  Default state (inactive)
    // ══════════════════════════════════════════════

    [Fact]
    public void Default_Hidden()
    {
        var helper = CreateHelper();
        var context = CreateContext("rhx-popup");
        var output = CreateOutput("rhx-popup");

        helper.Process(context, output);

        Assert.True(output.Attributes.TryGetAttribute("hidden", out _));
    }

    [Fact]
    public void Default_Aria_Hidden()
    {
        var helper = CreateHelper();
        var context = CreateContext("rhx-popup");
        var output = CreateOutput("rhx-popup");

        helper.Process(context, output);

        AssertAttribute(output, "aria-hidden", "true");
    }

    [Fact]
    public void Default_No_Active_Modifier()
    {
        var helper = CreateHelper();
        var context = CreateContext("rhx-popup");
        var output = CreateOutput("rhx-popup");

        helper.Process(context, output);

        Assert.False(HasClass(output, "rhx-popup--active"));
    }

    // ══════════════════════════════════════════════
    //  Active state
    // ══════════════════════════════════════════════

    [Fact]
    public void Active_Has_Modifier()
    {
        var helper = CreateHelper();
        helper.Active = true;
        var context = CreateContext("rhx-popup");
        var output = CreateOutput("rhx-popup");

        helper.Process(context, output);

        Assert.True(HasClass(output, "rhx-popup--active"));
    }

    [Fact]
    public void Active_Not_Hidden()
    {
        var helper = CreateHelper();
        helper.Active = true;
        var context = CreateContext("rhx-popup");
        var output = CreateOutput("rhx-popup");

        helper.Process(context, output);

        AssertNoAttribute(output, "hidden");
        AssertNoAttribute(output, "aria-hidden");
    }

    // ══════════════════════════════════════════════
    //  Anchor
    // ══════════════════════════════════════════════

    [Fact]
    public void Anchor_Selector()
    {
        var helper = CreateHelper();
        helper.Anchor = "#my-trigger";
        var context = CreateContext("rhx-popup");
        var output = CreateOutput("rhx-popup");

        helper.Process(context, output);

        AssertAttribute(output, "data-rhx-anchor", "#my-trigger");
    }

    [Fact]
    public void No_Anchor_No_Attribute()
    {
        var helper = CreateHelper();
        var context = CreateContext("rhx-popup");
        var output = CreateOutput("rhx-popup");

        helper.Process(context, output);

        AssertNoAttribute(output, "data-rhx-anchor");
    }

    // ══════════════════════════════════════════════
    //  Placement
    // ══════════════════════════════════════════════

    [Fact]
    public void Default_Placement()
    {
        var helper = CreateHelper();
        var context = CreateContext("rhx-popup");
        var output = CreateOutput("rhx-popup");

        helper.Process(context, output);

        AssertAttribute(output, "data-rhx-placement", "bottom-start");
    }

    [Fact]
    public void Custom_Placement()
    {
        var helper = CreateHelper();
        helper.Placement = "top-end";
        var context = CreateContext("rhx-popup");
        var output = CreateOutput("rhx-popup");

        helper.Process(context, output);

        AssertAttribute(output, "data-rhx-placement", "top-end");
    }

    // ══════════════════════════════════════════════
    //  Distance and skidding
    // ══════════════════════════════════════════════

    [Fact]
    public void Default_Distance_Not_Rendered()
    {
        var helper = CreateHelper();
        var context = CreateContext("rhx-popup");
        var output = CreateOutput("rhx-popup");

        helper.Process(context, output);

        AssertNoAttribute(output, "data-rhx-distance");
    }

    [Fact]
    public void Custom_Distance()
    {
        var helper = CreateHelper();
        helper.Distance = 12;
        var context = CreateContext("rhx-popup");
        var output = CreateOutput("rhx-popup");

        helper.Process(context, output);

        AssertAttribute(output, "data-rhx-distance", "12");
    }

    [Fact]
    public void Default_Skidding_Not_Rendered()
    {
        var helper = CreateHelper();
        var context = CreateContext("rhx-popup");
        var output = CreateOutput("rhx-popup");

        helper.Process(context, output);

        AssertNoAttribute(output, "data-rhx-skidding");
    }

    [Fact]
    public void Custom_Skidding()
    {
        var helper = CreateHelper();
        helper.Skidding = 10;
        var context = CreateContext("rhx-popup");
        var output = CreateOutput("rhx-popup");

        helper.Process(context, output);

        AssertAttribute(output, "data-rhx-skidding", "10");
    }

    // ══════════════════════════════════════════════
    //  Strategy
    // ══════════════════════════════════════════════

    [Fact]
    public void Default_Strategy_Not_Rendered()
    {
        var helper = CreateHelper();
        var context = CreateContext("rhx-popup");
        var output = CreateOutput("rhx-popup");

        helper.Process(context, output);

        AssertNoAttribute(output, "data-rhx-strategy");
    }

    [Fact]
    public void Fixed_Strategy()
    {
        var helper = CreateHelper();
        helper.Strategy = "fixed";
        var context = CreateContext("rhx-popup");
        var output = CreateOutput("rhx-popup");

        helper.Process(context, output);

        AssertAttribute(output, "data-rhx-strategy", "fixed");
    }

    // ══════════════════════════════════════════════
    //  Flip and shift
    // ══════════════════════════════════════════════

    [Fact]
    public void Default_Flip_No_Attribute()
    {
        var helper = CreateHelper();
        var context = CreateContext("rhx-popup");
        var output = CreateOutput("rhx-popup");

        helper.Process(context, output);

        AssertNoAttribute(output, "data-rhx-no-flip");
    }

    [Fact]
    public void No_Flip()
    {
        var helper = CreateHelper();
        helper.Flip = false;
        var context = CreateContext("rhx-popup");
        var output = CreateOutput("rhx-popup");

        helper.Process(context, output);

        Assert.True(output.Attributes.TryGetAttribute("data-rhx-no-flip", out _));
    }

    [Fact]
    public void Default_Shift_No_Attribute()
    {
        var helper = CreateHelper();
        var context = CreateContext("rhx-popup");
        var output = CreateOutput("rhx-popup");

        helper.Process(context, output);

        AssertNoAttribute(output, "data-rhx-no-shift");
    }

    [Fact]
    public void No_Shift()
    {
        var helper = CreateHelper();
        helper.Shift = false;
        var context = CreateContext("rhx-popup");
        var output = CreateOutput("rhx-popup");

        helper.Process(context, output);

        Assert.True(output.Attributes.TryGetAttribute("data-rhx-no-shift", out _));
    }

    // ══════════════════════════════════════════════
    //  Arrow
    // ══════════════════════════════════════════════

    [Fact]
    public void Default_No_Arrow()
    {
        var helper = CreateHelper();
        var context = CreateContext("rhx-popup");
        var output = CreateOutput("rhx-popup");

        helper.Process(context, output);

        AssertNoAttribute(output, "data-rhx-arrow");
    }

    [Fact]
    public void Arrow_Adds_Attribute()
    {
        var helper = CreateHelper();
        helper.Arrow = true;
        var context = CreateContext("rhx-popup");
        var output = CreateOutput("rhx-popup");

        helper.Process(context, output);

        Assert.True(output.Attributes.TryGetAttribute("data-rhx-arrow", out _));
    }

    [Fact]
    public void Arrow_Adds_Element()
    {
        var helper = CreateHelper();
        helper.Arrow = true;
        var context = CreateContext("rhx-popup");
        var output = CreateOutput("rhx-popup");

        helper.Process(context, output);

        var postContent = output.PostContent.GetContent();
        Assert.Contains("rhx-popup__arrow", postContent);
        Assert.Contains("data-rhx-popup-arrow", postContent);
    }

    [Fact]
    public void Arrow_Default_Padding_Not_Rendered()
    {
        var helper = CreateHelper();
        helper.Arrow = true;
        var context = CreateContext("rhx-popup");
        var output = CreateOutput("rhx-popup");

        helper.Process(context, output);

        AssertNoAttribute(output, "data-rhx-arrow-padding");
    }

    [Fact]
    public void Arrow_Custom_Padding()
    {
        var helper = CreateHelper();
        helper.Arrow = true;
        helper.ArrowPadding = 16;
        var context = CreateContext("rhx-popup");
        var output = CreateOutput("rhx-popup");

        helper.Process(context, output);

        AssertAttribute(output, "data-rhx-arrow-padding", "16");
    }

    // ══════════════════════════════════════════════
    //  htmx
    // ══════════════════════════════════════════════

    [Fact]
    public void Htmx_Attributes_Rendered()
    {
        var helper = CreateHelper();
        helper.HxGet = "/api/data";
        var context = CreateContext("rhx-popup");
        var output = CreateOutput("rhx-popup");

        helper.Process(context, output);

        AssertAttribute(output, "hx-get", "/api/data");
    }

    // ══════════════════════════════════════════════
    //  Id
    // ══════════════════════════════════════════════

    [Fact]
    public void Custom_Id()
    {
        var helper = CreateHelper();
        helper.Id = "my-popup";
        var context = CreateContext("rhx-popup");
        var output = CreateOutput("rhx-popup");

        helper.Process(context, output);

        AssertAttribute(output, "id", "my-popup");
    }
}
