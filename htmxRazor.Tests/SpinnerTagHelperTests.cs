using htmxRazor.Components.Feedback;
using Xunit;

namespace htmxRazor.Tests;

public class SpinnerTagHelperTests : TagHelperTestBase
{
    private SpinnerTagHelper CreateHelper()
    {
        var helper = new SpinnerTagHelper(CreateUrlHelperFactory());
        helper.ViewContext = CreateViewContext();
        return helper;
    }

    // ──────────────────────────────────────────────
    //  Default rendering
    // ──────────────────────────────────────────────

    [Fact]
    public void Renders_Span_Element()
    {
        var helper = CreateHelper();
        var context = CreateContext("rhx-spinner");
        var output = CreateOutput("rhx-spinner");

        helper.Process(context, output);

        Assert.Equal("span", output.TagName);
    }

    [Fact]
    public void Renders_Block_Class()
    {
        var helper = CreateHelper();
        var context = CreateContext("rhx-spinner");
        var output = CreateOutput("rhx-spinner");

        helper.Process(context, output);

        Assert.True(HasClass(output, "rhx-spinner"));
    }

    [Fact]
    public void Renders_Status_Role()
    {
        var helper = CreateHelper();
        var context = CreateContext("rhx-spinner");
        var output = CreateOutput("rhx-spinner");

        helper.Process(context, output);

        AssertAttribute(output, "role", "status");
    }

    [Fact]
    public void Renders_Data_Rhx_Spinner_Attribute()
    {
        var helper = CreateHelper();
        var context = CreateContext("rhx-spinner");
        var output = CreateOutput("rhx-spinner");

        helper.Process(context, output);

        Assert.True(output.Attributes.TryGetAttribute("data-rhx-spinner", out _));
    }

    [Fact]
    public void Renders_Default_Aria_Label()
    {
        var helper = CreateHelper();
        var context = CreateContext("rhx-spinner");
        var output = CreateOutput("rhx-spinner");

        helper.Process(context, output);

        AssertAttribute(output, "aria-label", "Loading");
    }

    [Fact]
    public void Renders_Svg_Content()
    {
        var helper = CreateHelper();
        var context = CreateContext("rhx-spinner");
        var output = CreateOutput("rhx-spinner");

        helper.Process(context, output);

        var content = output.Content.GetContent();
        Assert.Contains("<svg", content);
        Assert.Contains("<circle", content);
        Assert.Contains("cx=\"12\"", content);
        Assert.Contains("cy=\"12\"", content);
        Assert.Contains("r=\"10\"", content);
    }

    // ──────────────────────────────────────────────
    //  Sizes
    // ──────────────────────────────────────────────

    [Fact]
    public void Default_Medium_No_Size_Modifier()
    {
        var helper = CreateHelper();
        var context = CreateContext("rhx-spinner");
        var output = CreateOutput("rhx-spinner");

        helper.Process(context, output);

        Assert.False(HasClass(output, "rhx-spinner--medium"));
        Assert.False(HasClass(output, "rhx-spinner--small"));
        Assert.False(HasClass(output, "rhx-spinner--large"));
    }

    [Fact]
    public void Small_Size_Adds_Modifier()
    {
        var helper = CreateHelper();
        helper.Size = "small";
        var context = CreateContext("rhx-spinner");
        var output = CreateOutput("rhx-spinner");

        helper.Process(context, output);

        Assert.True(HasClass(output, "rhx-spinner--small"));
    }

    [Fact]
    public void Large_Size_Adds_Modifier()
    {
        var helper = CreateHelper();
        helper.Size = "large";
        var context = CreateContext("rhx-spinner");
        var output = CreateOutput("rhx-spinner");

        helper.Process(context, output);

        Assert.True(HasClass(output, "rhx-spinner--large"));
    }

    [Fact]
    public void Custom_Size_Sets_Inline_Style()
    {
        var helper = CreateHelper();
        helper.Size = "3rem";
        var context = CreateContext("rhx-spinner");
        var output = CreateOutput("rhx-spinner");

        helper.Process(context, output);

        AssertAttribute(output, "style", "width: 3rem; height: 3rem;");
        Assert.False(HasClass(output, "rhx-spinner--3rem"));
    }

    [Fact]
    public void Size_Is_Case_Insensitive()
    {
        var helper = CreateHelper();
        helper.Size = "Large";
        var context = CreateContext("rhx-spinner");
        var output = CreateOutput("rhx-spinner");

        helper.Process(context, output);

        Assert.True(HasClass(output, "rhx-spinner--large"));
    }

    // ──────────────────────────────────────────────
    //  Custom label
    // ──────────────────────────────────────────────

    [Fact]
    public void Custom_Label()
    {
        var helper = CreateHelper();
        helper.Label = "Processing";
        var context = CreateContext("rhx-spinner");
        var output = CreateOutput("rhx-spinner");

        helper.Process(context, output);

        AssertAttribute(output, "aria-label", "Processing");
    }

    // ──────────────────────────────────────────────
    //  Custom CSS class (htmx-indicator)
    // ──────────────────────────────────────────────

    [Fact]
    public void Custom_CssClass_Appended()
    {
        var helper = CreateHelper();
        helper.CssClass = "htmx-indicator";
        var context = CreateContext("rhx-spinner");
        var output = CreateOutput("rhx-spinner");

        helper.Process(context, output);

        Assert.True(HasClass(output, "htmx-indicator"));
        Assert.True(HasClass(output, "rhx-spinner"));
    }
}
