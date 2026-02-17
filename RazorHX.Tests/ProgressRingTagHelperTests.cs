using RazorHX.Components.Feedback;
using Xunit;

namespace RazorHX.Tests;

public class ProgressRingTagHelperTests : TagHelperTestBase
{
    private ProgressRingTagHelper CreateHelper()
    {
        var helper = new ProgressRingTagHelper(CreateUrlHelperFactory());
        helper.ViewContext = CreateViewContext();
        return helper;
    }

    // ──────────────────────────────────────────────
    //  Default rendering
    // ──────────────────────────────────────────────

    [Fact]
    public void Renders_Svg_Element()
    {
        var helper = CreateHelper();
        var context = CreateContext("rhx-progress-ring");
        var output = CreateOutput("rhx-progress-ring");

        helper.Process(context, output);

        Assert.Equal("svg", output.TagName);
    }

    [Fact]
    public void Renders_Block_Class()
    {
        var helper = CreateHelper();
        var context = CreateContext("rhx-progress-ring");
        var output = CreateOutput("rhx-progress-ring");

        helper.Process(context, output);

        Assert.True(HasClass(output, "rhx-progress-ring"));
    }

    [Fact]
    public void Renders_ViewBox()
    {
        var helper = CreateHelper();
        var context = CreateContext("rhx-progress-ring");
        var output = CreateOutput("rhx-progress-ring");

        helper.Process(context, output);

        AssertAttribute(output, "viewBox", "0 0 36 36");
    }

    [Fact]
    public void Renders_Progressbar_Role()
    {
        var helper = CreateHelper();
        var context = CreateContext("rhx-progress-ring");
        var output = CreateOutput("rhx-progress-ring");

        helper.Process(context, output);

        AssertAttribute(output, "role", "progressbar");
    }

    [Fact]
    public void Renders_Aria_Value_Attributes()
    {
        var helper = CreateHelper();
        helper.Value = 75;
        var context = CreateContext("rhx-progress-ring");
        var output = CreateOutput("rhx-progress-ring");

        helper.Process(context, output);

        AssertAttribute(output, "aria-valuenow", "75");
        AssertAttribute(output, "aria-valuemin", "0");
        AssertAttribute(output, "aria-valuemax", "100");
    }

    // ──────────────────────────────────────────────
    //  SVG content
    // ──────────────────────────────────────────────

    [Fact]
    public void Renders_Track_Circle()
    {
        var helper = CreateHelper();
        var context = CreateContext("rhx-progress-ring");
        var output = CreateOutput("rhx-progress-ring");

        helper.Process(context, output);

        var content = output.Content.GetContent();
        Assert.Contains("rhx-progress-ring__track", content);
        Assert.Contains("r=\"15.9155\"", content);
        Assert.Contains("fill=\"none\"", content);
    }

    [Fact]
    public void Renders_Fill_Circle_With_Dasharray()
    {
        var helper = CreateHelper();
        helper.Value = 65;
        var context = CreateContext("rhx-progress-ring");
        var output = CreateOutput("rhx-progress-ring");

        helper.Process(context, output);

        var content = output.Content.GetContent();
        Assert.Contains("rhx-progress-ring__fill", content);
        Assert.Contains("stroke-dasharray=\"65 100\"", content);
        Assert.Contains("stroke-linecap=\"round\"", content);
        Assert.Contains("transform=\"rotate(-90 18 18)\"", content);
    }

    [Fact]
    public void Renders_Center_Label()
    {
        var helper = CreateHelper();
        helper.Value = 65;
        var context = CreateContext("rhx-progress-ring");
        var output = CreateOutput("rhx-progress-ring");

        helper.Process(context, output);

        var content = output.Content.GetContent();
        Assert.Contains("rhx-progress-ring__label", content);
        Assert.Contains("65%", content);
        Assert.Contains("text-anchor=\"middle\"", content);
    }

    // ──────────────────────────────────────────────
    //  Custom stroke widths
    // ──────────────────────────────────────────────

    [Fact]
    public void Custom_Track_Width()
    {
        var helper = CreateHelper();
        helper.TrackWidth = 5;
        var context = CreateContext("rhx-progress-ring");
        var output = CreateOutput("rhx-progress-ring");

        helper.Process(context, output);

        var content = output.Content.GetContent();
        // Track circle should have stroke-width="5"
        Assert.Contains("stroke-width=\"5\"", content);
    }

    [Fact]
    public void Custom_Indicator_Width()
    {
        var helper = CreateHelper();
        helper.IndicatorWidth = 6;
        var context = CreateContext("rhx-progress-ring");
        var output = CreateOutput("rhx-progress-ring");

        helper.Process(context, output);

        var content = output.Content.GetContent();
        // Fill circle should have stroke-width="6"
        Assert.Contains("stroke-width=\"6\"", content);
    }

    // ──────────────────────────────────────────────
    //  Label / aria-label
    // ──────────────────────────────────────────────

    [Fact]
    public void Label_Sets_Aria_Label()
    {
        var helper = CreateHelper();
        helper.Label = "Upload";
        var context = CreateContext("rhx-progress-ring");
        var output = CreateOutput("rhx-progress-ring");

        helper.Process(context, output);

        AssertAttribute(output, "aria-label", "Upload");
    }

    [Fact]
    public void No_Label_No_Aria_Label()
    {
        var helper = CreateHelper();
        var context = CreateContext("rhx-progress-ring");
        var output = CreateOutput("rhx-progress-ring");

        helper.Process(context, output);

        AssertNoAttribute(output, "aria-label");
    }

    // ──────────────────────────────────────────────
    //  Value clamping
    // ──────────────────────────────────────────────

    [Fact]
    public void Clamps_Value_Above_100()
    {
        var helper = CreateHelper();
        helper.Value = 200;
        var context = CreateContext("rhx-progress-ring");
        var output = CreateOutput("rhx-progress-ring");

        helper.Process(context, output);

        AssertAttribute(output, "aria-valuenow", "100");
        var content = output.Content.GetContent();
        Assert.Contains("stroke-dasharray=\"100 100\"", content);
    }

    // ──────────────────────────────────────────────
    //  Custom CSS class
    // ──────────────────────────────────────────────

    [Fact]
    public void Custom_CssClass_Appended()
    {
        var helper = CreateHelper();
        helper.CssClass = "my-ring";
        var context = CreateContext("rhx-progress-ring");
        var output = CreateOutput("rhx-progress-ring");

        helper.Process(context, output);

        Assert.True(HasClass(output, "my-ring"));
        Assert.True(HasClass(output, "rhx-progress-ring"));
    }
}
