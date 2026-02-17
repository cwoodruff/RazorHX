using RazorHX.Components.Imagery;
using Xunit;

namespace RazorHX.Tests;

public class ZoomableFrameTagHelperTests : TagHelperTestBase
{
    private ZoomableFrameTagHelper CreateHelper()
    {
        var helper = new ZoomableFrameTagHelper(CreateUrlHelperFactory());
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
        helper.Src = "diagram.png";
        helper.Alt = "Diagram";
        var context = CreateContext("rhx-zoomable-frame");
        var output = CreateOutput("rhx-zoomable-frame");

        helper.Process(context, output);

        Assert.Equal("div", output.TagName);
    }

    [Fact]
    public void Has_Block_Class()
    {
        var helper = CreateHelper();
        helper.Src = "diagram.png";
        helper.Alt = "Diagram";
        var context = CreateContext("rhx-zoomable-frame");
        var output = CreateOutput("rhx-zoomable-frame");

        helper.Process(context, output);

        Assert.True(HasClass(output, "rhx-zoomable-frame"));
    }

    [Fact]
    public void Has_Data_Attribute()
    {
        var helper = CreateHelper();
        helper.Src = "diagram.png";
        helper.Alt = "Diagram";
        var context = CreateContext("rhx-zoomable-frame");
        var output = CreateOutput("rhx-zoomable-frame");

        helper.Process(context, output);

        AssertAttribute(output, "data-rhx-zoomable-frame", "");
    }

    [Fact]
    public void Has_Tabindex()
    {
        var helper = CreateHelper();
        helper.Src = "diagram.png";
        helper.Alt = "Diagram";
        var context = CreateContext("rhx-zoomable-frame");
        var output = CreateOutput("rhx-zoomable-frame");

        helper.Process(context, output);

        AssertAttribute(output, "tabindex", "0");
    }

    [Fact]
    public void Has_Application_Role()
    {
        var helper = CreateHelper();
        helper.Src = "diagram.png";
        helper.Alt = "Diagram";
        var context = CreateContext("rhx-zoomable-frame");
        var output = CreateOutput("rhx-zoomable-frame");

        helper.Process(context, output);

        AssertAttribute(output, "role", "application");
    }

    // ══════════════════════════════════════════════
    //  Image mode (Alt set)
    // ══════════════════════════════════════════════

    [Fact]
    public void Alt_Set_Renders_Img()
    {
        var helper = CreateHelper();
        helper.Src = "diagram.png";
        helper.Alt = "Diagram";
        var context = CreateContext("rhx-zoomable-frame");
        var output = CreateOutput("rhx-zoomable-frame");

        helper.Process(context, output);

        var content = output.Content.GetContent();
        Assert.Contains("rhx-zoomable-frame__content", content);
        Assert.Contains("<img", content);
        Assert.Contains("src=\"diagram.png\"", content);
        Assert.Contains("alt=\"Diagram\"", content);
    }

    [Fact]
    public void Img_Not_Draggable()
    {
        var helper = CreateHelper();
        helper.Src = "diagram.png";
        helper.Alt = "Diagram";
        var context = CreateContext("rhx-zoomable-frame");
        var output = CreateOutput("rhx-zoomable-frame");

        helper.Process(context, output);

        var content = output.Content.GetContent();
        Assert.Contains("draggable=\"false\"", content);
    }

    // ══════════════════════════════════════════════
    //  Iframe mode (no Alt)
    // ══════════════════════════════════════════════

    [Fact]
    public void No_Alt_Renders_Iframe()
    {
        var helper = CreateHelper();
        helper.Src = "https://example.com";
        var context = CreateContext("rhx-zoomable-frame");
        var output = CreateOutput("rhx-zoomable-frame");

        helper.Process(context, output);

        var content = output.Content.GetContent();
        Assert.Contains("<iframe", content);
        Assert.Contains("src=\"https://example.com\"", content);
        Assert.DoesNotContain("<img", content);
    }

    // ══════════════════════════════════════════════
    //  Scale configuration
    // ══════════════════════════════════════════════

    [Fact]
    public void Default_Scale_Values()
    {
        var helper = CreateHelper();
        helper.Src = "diagram.png";
        helper.Alt = "Diagram";
        var context = CreateContext("rhx-zoomable-frame");
        var output = CreateOutput("rhx-zoomable-frame");

        helper.Process(context, output);

        AssertAttribute(output, "data-rhx-scale", "1.00");
        AssertAttribute(output, "data-rhx-min-scale", "0.50");
        AssertAttribute(output, "data-rhx-max-scale", "5.00");
    }

    [Fact]
    public void Custom_Scale_Values()
    {
        var helper = CreateHelper();
        helper.Src = "diagram.png";
        helper.Alt = "Diagram";
        helper.Scale = 2;
        helper.MinScale = 0.25;
        helper.MaxScale = 10;
        var context = CreateContext("rhx-zoomable-frame");
        var output = CreateOutput("rhx-zoomable-frame");

        helper.Process(context, output);

        AssertAttribute(output, "data-rhx-scale", "2.00");
        AssertAttribute(output, "data-rhx-min-scale", "0.25");
        AssertAttribute(output, "data-rhx-max-scale", "10.00");
    }

    [Fact]
    public void Initial_Scale_Applied_To_Content()
    {
        var helper = CreateHelper();
        helper.Src = "diagram.png";
        helper.Alt = "Diagram";
        helper.Scale = 2;
        var context = CreateContext("rhx-zoomable-frame");
        var output = CreateOutput("rhx-zoomable-frame");

        helper.Process(context, output);

        var content = output.Content.GetContent();
        Assert.Contains("scale(2.00)", content);
    }

    [Fact]
    public void Default_Scale_No_Style()
    {
        var helper = CreateHelper();
        helper.Src = "diagram.png";
        helper.Alt = "Diagram";
        var context = CreateContext("rhx-zoomable-frame");
        var output = CreateOutput("rhx-zoomable-frame");

        helper.Process(context, output);

        var content = output.Content.GetContent();
        Assert.DoesNotContain("scale(", content);
    }

    // ══════════════════════════════════════════════
    //  htmx
    // ══════════════════════════════════════════════

    [Fact]
    public void Renders_Htmx_Attributes()
    {
        var helper = CreateHelper();
        helper.Src = "diagram.png";
        helper.Alt = "Diagram";
        helper.HxGet = "/api/image";
        var context = CreateContext("rhx-zoomable-frame");
        var output = CreateOutput("rhx-zoomable-frame");

        helper.Process(context, output);

        AssertAttribute(output, "hx-get", "/api/image");
    }
}
