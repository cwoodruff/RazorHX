using htmxRazor.Components.Imagery;
using Xunit;

namespace htmxRazor.Tests;

public class ComparisonTagHelperTests : TagHelperTestBase
{
    private ComparisonTagHelper CreateHelper()
    {
        var helper = new ComparisonTagHelper(CreateUrlHelperFactory());
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
        helper.Before = "before.jpg";
        helper.After = "after.jpg";
        var context = CreateContext("rhx-comparison");
        var output = CreateOutput("rhx-comparison");

        helper.Process(context, output);

        Assert.Equal("div", output.TagName);
    }

    [Fact]
    public void Has_Block_Class()
    {
        var helper = CreateHelper();
        helper.Before = "before.jpg";
        helper.After = "after.jpg";
        var context = CreateContext("rhx-comparison");
        var output = CreateOutput("rhx-comparison");

        helper.Process(context, output);

        Assert.True(HasClass(output, "rhx-comparison"));
    }

    [Fact]
    public void Has_Data_Attribute()
    {
        var helper = CreateHelper();
        helper.Before = "before.jpg";
        helper.After = "after.jpg";
        var context = CreateContext("rhx-comparison");
        var output = CreateOutput("rhx-comparison");

        helper.Process(context, output);

        AssertAttribute(output, "data-rhx-comparison", "");
    }

    [Fact]
    public void Contains_Before_Image()
    {
        var helper = CreateHelper();
        helper.Before = "before.jpg";
        helper.BeforeAlt = "Before shot";
        helper.After = "after.jpg";
        var context = CreateContext("rhx-comparison");
        var output = CreateOutput("rhx-comparison");

        helper.Process(context, output);

        var content = output.Content.GetContent();
        Assert.Contains("rhx-comparison__before", content);
        Assert.Contains("src=\"before.jpg\"", content);
        Assert.Contains("alt=\"Before shot\"", content);
    }

    [Fact]
    public void Contains_After_Image()
    {
        var helper = CreateHelper();
        helper.Before = "before.jpg";
        helper.After = "after.jpg";
        helper.AfterAlt = "After shot";
        var context = CreateContext("rhx-comparison");
        var output = CreateOutput("rhx-comparison");

        helper.Process(context, output);

        var content = output.Content.GetContent();
        Assert.Contains("rhx-comparison__after", content);
        Assert.Contains("src=\"after.jpg\"", content);
        Assert.Contains("alt=\"After shot\"", content);
    }

    [Fact]
    public void Contains_Handle()
    {
        var helper = CreateHelper();
        helper.Before = "before.jpg";
        helper.After = "after.jpg";
        var context = CreateContext("rhx-comparison");
        var output = CreateOutput("rhx-comparison");

        helper.Process(context, output);

        var content = output.Content.GetContent();
        Assert.Contains("rhx-comparison__handle", content);
        Assert.Contains("role=\"slider\"", content);
        Assert.Contains("tabindex=\"0\"", content);
    }

    [Fact]
    public void Handle_Has_Grip()
    {
        var helper = CreateHelper();
        helper.Before = "before.jpg";
        helper.After = "after.jpg";
        var context = CreateContext("rhx-comparison");
        var output = CreateOutput("rhx-comparison");

        helper.Process(context, output);

        var content = output.Content.GetContent();
        Assert.Contains("rhx-comparison__handle-grip", content);
        Assert.Contains("rhx-comparison__handle-line", content);
    }

    // ══════════════════════════════════════════════
    //  Position
    // ══════════════════════════════════════════════

    [Fact]
    public void Default_Position_50()
    {
        var helper = CreateHelper();
        helper.Before = "before.jpg";
        helper.After = "after.jpg";
        var context = CreateContext("rhx-comparison");
        var output = CreateOutput("rhx-comparison");

        helper.Process(context, output);

        var content = output.Content.GetContent();
        Assert.Contains("aria-valuenow=\"50\"", content);
        Assert.Contains("left: 50%", content);
        Assert.Contains("inset(0 50% 0 0)", content);
    }

    [Fact]
    public void Custom_Position()
    {
        var helper = CreateHelper();
        helper.Before = "before.jpg";
        helper.After = "after.jpg";
        helper.Position = 75;
        var context = CreateContext("rhx-comparison");
        var output = CreateOutput("rhx-comparison");

        helper.Process(context, output);

        var content = output.Content.GetContent();
        Assert.Contains("aria-valuenow=\"75\"", content);
        Assert.Contains("left: 75%", content);
        Assert.Contains("inset(0 25% 0 0)", content);
    }

    [Fact]
    public void Position_Clamped_Min()
    {
        var helper = CreateHelper();
        helper.Before = "before.jpg";
        helper.After = "after.jpg";
        helper.Position = -10;
        var context = CreateContext("rhx-comparison");
        var output = CreateOutput("rhx-comparison");

        helper.Process(context, output);

        var content = output.Content.GetContent();
        Assert.Contains("aria-valuenow=\"0\"", content);
    }

    [Fact]
    public void Position_Clamped_Max()
    {
        var helper = CreateHelper();
        helper.Before = "before.jpg";
        helper.After = "after.jpg";
        helper.Position = 150;
        var context = CreateContext("rhx-comparison");
        var output = CreateOutput("rhx-comparison");

        helper.Process(context, output);

        var content = output.Content.GetContent();
        Assert.Contains("aria-valuenow=\"100\"", content);
    }

    [Fact]
    public void Handle_Has_AriaValueMin_Max()
    {
        var helper = CreateHelper();
        helper.Before = "before.jpg";
        helper.After = "after.jpg";
        var context = CreateContext("rhx-comparison");
        var output = CreateOutput("rhx-comparison");

        helper.Process(context, output);

        var content = output.Content.GetContent();
        Assert.Contains("aria-valuemin=\"0\"", content);
        Assert.Contains("aria-valuemax=\"100\"", content);
    }

    // ══════════════════════════════════════════════
    //  Ordering
    // ══════════════════════════════════════════════

    [Fact]
    public void Before_Then_After_Then_Handle()
    {
        var helper = CreateHelper();
        helper.Before = "before.jpg";
        helper.After = "after.jpg";
        var context = CreateContext("rhx-comparison");
        var output = CreateOutput("rhx-comparison");

        helper.Process(context, output);

        var html = output.Content.GetContent();
        var beforeIdx = html.IndexOf("rhx-comparison__before");
        var afterIdx = html.IndexOf("rhx-comparison__after");
        var handleIdx = html.IndexOf("rhx-comparison__handle");
        Assert.True(beforeIdx < afterIdx);
        Assert.True(afterIdx < handleIdx);
    }

    // ══════════════════════════════════════════════
    //  htmx
    // ══════════════════════════════════════════════

    [Fact]
    public void Renders_Htmx_Attributes()
    {
        var helper = CreateHelper();
        helper.Before = "before.jpg";
        helper.After = "after.jpg";
        helper.HxGet = "/api/compare";
        var context = CreateContext("rhx-comparison");
        var output = CreateOutput("rhx-comparison");

        helper.Process(context, output);

        AssertAttribute(output, "hx-get", "/api/compare");
    }
}
