using htmxRazor.Components.Feedback;
using Xunit;

namespace htmxRazor.Tests;

public class ProgressBarTagHelperTests : TagHelperTestBase
{
    private ProgressBarTagHelper CreateHelper()
    {
        var helper = new ProgressBarTagHelper(CreateUrlHelperFactory());
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
        var context = CreateContext("rhx-progress-bar");
        var output = CreateOutput("rhx-progress-bar");

        helper.Process(context, output);

        Assert.Equal("div", output.TagName);
    }

    [Fact]
    public void Renders_Block_Class()
    {
        var helper = CreateHelper();
        var context = CreateContext("rhx-progress-bar");
        var output = CreateOutput("rhx-progress-bar");

        helper.Process(context, output);

        Assert.True(HasClass(output, "rhx-progress-bar"));
    }

    [Fact]
    public void Renders_Progressbar_Role()
    {
        var helper = CreateHelper();
        var context = CreateContext("rhx-progress-bar");
        var output = CreateOutput("rhx-progress-bar");

        helper.Process(context, output);

        AssertAttribute(output, "role", "progressbar");
    }

    [Fact]
    public void Renders_Aria_Value_Attributes()
    {
        var helper = CreateHelper();
        helper.Value = 65;
        var context = CreateContext("rhx-progress-bar");
        var output = CreateOutput("rhx-progress-bar");

        helper.Process(context, output);

        AssertAttribute(output, "aria-valuemin", "0");
        AssertAttribute(output, "aria-valuemax", "100");
        AssertAttribute(output, "aria-valuenow", "65");
    }

    [Fact]
    public void Renders_Track_And_Fill()
    {
        var helper = CreateHelper();
        helper.Value = 65;
        var context = CreateContext("rhx-progress-bar");
        var output = CreateOutput("rhx-progress-bar");

        helper.Process(context, output);

        var content = output.Content.GetContent();
        Assert.Contains("rhx-progress-bar__track", content);
        Assert.Contains("rhx-progress-bar__fill", content);
        Assert.Contains("width: 65%", content);
    }

    [Fact]
    public void Renders_Percentage_Label()
    {
        var helper = CreateHelper();
        helper.Value = 42;
        var context = CreateContext("rhx-progress-bar");
        var output = CreateOutput("rhx-progress-bar");

        helper.Process(context, output);

        var content = output.Content.GetContent();
        Assert.Contains("rhx-progress-bar__label", content);
        Assert.Contains("42%", content);
    }

    // ──────────────────────────────────────────────
    //  Value clamping
    // ──────────────────────────────────────────────

    [Fact]
    public void Clamps_Value_Above_100()
    {
        var helper = CreateHelper();
        helper.Value = 150;
        var context = CreateContext("rhx-progress-bar");
        var output = CreateOutput("rhx-progress-bar");

        helper.Process(context, output);

        AssertAttribute(output, "aria-valuenow", "100");
        var content = output.Content.GetContent();
        Assert.Contains("width: 100%", content);
        Assert.Contains("100%", content);
    }

    [Fact]
    public void Clamps_Value_Below_0()
    {
        var helper = CreateHelper();
        helper.Value = -10;
        var context = CreateContext("rhx-progress-bar");
        var output = CreateOutput("rhx-progress-bar");

        helper.Process(context, output);

        AssertAttribute(output, "aria-valuenow", "0");
        var content = output.Content.GetContent();
        Assert.Contains("width: 0%", content);
    }

    // ──────────────────────────────────────────────
    //  Indeterminate
    // ──────────────────────────────────────────────

    [Fact]
    public void Indeterminate_Adds_Modifier_Class()
    {
        var helper = CreateHelper();
        helper.Indeterminate = true;
        var context = CreateContext("rhx-progress-bar");
        var output = CreateOutput("rhx-progress-bar");

        helper.Process(context, output);

        Assert.True(HasClass(output, "rhx-progress-bar--indeterminate"));
    }

    [Fact]
    public void Indeterminate_Has_No_Aria_Valuenow()
    {
        var helper = CreateHelper();
        helper.Indeterminate = true;
        var context = CreateContext("rhx-progress-bar");
        var output = CreateOutput("rhx-progress-bar");

        helper.Process(context, output);

        AssertNoAttribute(output, "aria-valuenow");
    }

    [Fact]
    public void Indeterminate_Fill_Has_No_Width_Style()
    {
        var helper = CreateHelper();
        helper.Indeterminate = true;
        var context = CreateContext("rhx-progress-bar");
        var output = CreateOutput("rhx-progress-bar");

        helper.Process(context, output);

        var content = output.Content.GetContent();
        Assert.Contains("rhx-progress-bar__fill", content);
        Assert.DoesNotContain("style=", content);
    }

    [Fact]
    public void Indeterminate_Has_No_Label()
    {
        var helper = CreateHelper();
        helper.Indeterminate = true;
        var context = CreateContext("rhx-progress-bar");
        var output = CreateOutput("rhx-progress-bar");

        helper.Process(context, output);

        var content = output.Content.GetContent();
        Assert.DoesNotContain("rhx-progress-bar__label", content);
    }

    // ──────────────────────────────────────────────
    //  Label / aria-label
    // ──────────────────────────────────────────────

    [Fact]
    public void Label_Sets_Aria_Label()
    {
        var helper = CreateHelper();
        helper.Label = "Upload progress";
        var context = CreateContext("rhx-progress-bar");
        var output = CreateOutput("rhx-progress-bar");

        helper.Process(context, output);

        AssertAttribute(output, "aria-label", "Upload progress");
    }

    [Fact]
    public void No_Label_No_Aria_Label()
    {
        var helper = CreateHelper();
        var context = CreateContext("rhx-progress-bar");
        var output = CreateOutput("rhx-progress-bar");

        helper.Process(context, output);

        AssertNoAttribute(output, "aria-label");
    }

    // ──────────────────────────────────────────────
    //  Custom CSS class
    // ──────────────────────────────────────────────

    [Fact]
    public void Custom_CssClass_Appended()
    {
        var helper = CreateHelper();
        helper.CssClass = "my-progress";
        var context = CreateContext("rhx-progress-bar");
        var output = CreateOutput("rhx-progress-bar");

        helper.Process(context, output);

        Assert.True(HasClass(output, "my-progress"));
        Assert.True(HasClass(output, "rhx-progress-bar"));
    }

    // ──────────────────────────────────────────────
    //  htmx attributes
    // ──────────────────────────────────────────────

    [Fact]
    public void HxGet_Renders()
    {
        var helper = CreateHelper();
        helper.HxGet = "/api/progress";
        var context = CreateContext("rhx-progress-bar");
        var output = CreateOutput("rhx-progress-bar");

        helper.Process(context, output);

        AssertAttribute(output, "hx-get", "/api/progress");
    }
}
