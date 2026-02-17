using RazorHX.Components.Utilities;
using Xunit;

namespace RazorHX.Tests;

public class QrCodeTagHelperTests : TagHelperTestBase
{
    private QrCodeTagHelper CreateHelper() => new();

    // ── Element rendering ──

    [Fact]
    public void Renders_Canvas_Element()
    {
        var helper = CreateHelper();
        var context = CreateContext("rhx-qr-code");
        var output = CreateOutput("rhx-qr-code");

        helper.ViewContext = CreateViewContext();
        helper.Process(context, output);

        Assert.Equal("canvas", output.TagName);
    }

    [Fact]
    public void Has_Block_Class()
    {
        var helper = CreateHelper();
        var context = CreateContext("rhx-qr-code");
        var output = CreateOutput("rhx-qr-code");

        helper.ViewContext = CreateViewContext();
        helper.Process(context, output);

        Assert.True(HasClass(output, "rhx-qr-code"));
    }

    [Fact]
    public void Has_Data_Attribute()
    {
        var helper = CreateHelper();
        var context = CreateContext("rhx-qr-code");
        var output = CreateOutput("rhx-qr-code");

        helper.ViewContext = CreateViewContext();
        helper.Process(context, output);

        AssertAttribute(output, "data-rhx-qr-code", "");
    }

    [Fact]
    public void Has_Role_Img()
    {
        var helper = CreateHelper();
        var context = CreateContext("rhx-qr-code");
        var output = CreateOutput("rhx-qr-code");

        helper.ViewContext = CreateViewContext();
        helper.Process(context, output);

        AssertAttribute(output, "role", "img");
    }

    // ── Value ──

    [Fact]
    public void Value_Sets_Data_Attribute()
    {
        var helper = CreateHelper();
        helper.Value = "https://example.com";
        var context = CreateContext("rhx-qr-code");
        var output = CreateOutput("rhx-qr-code");

        helper.ViewContext = CreateViewContext();
        helper.Process(context, output);

        AssertAttribute(output, "data-rhx-qr-value", "https://example.com");
    }

    [Fact]
    public void Empty_Value_Sets_Empty_Data_Attribute()
    {
        var helper = CreateHelper();
        var context = CreateContext("rhx-qr-code");
        var output = CreateOutput("rhx-qr-code");

        helper.ViewContext = CreateViewContext();
        helper.Process(context, output);

        AssertAttribute(output, "data-rhx-qr-value", "");
    }

    // ── Size ──

    [Fact]
    public void Default_Size_128()
    {
        var helper = CreateHelper();
        var context = CreateContext("rhx-qr-code");
        var output = CreateOutput("rhx-qr-code");

        helper.ViewContext = CreateViewContext();
        helper.Process(context, output);

        AssertAttribute(output, "width", "128");
        AssertAttribute(output, "height", "128");
        AssertAttribute(output, "data-rhx-qr-size", "128");
    }

    [Fact]
    public void Custom_Size()
    {
        var helper = CreateHelper();
        helper.Size = 256;
        var context = CreateContext("rhx-qr-code");
        var output = CreateOutput("rhx-qr-code");

        helper.ViewContext = CreateViewContext();
        helper.Process(context, output);

        AssertAttribute(output, "width", "256");
        AssertAttribute(output, "height", "256");
        AssertAttribute(output, "data-rhx-qr-size", "256");
    }

    // ── Colors ──

    [Fact]
    public void Default_Fill_And_Background()
    {
        var helper = CreateHelper();
        var context = CreateContext("rhx-qr-code");
        var output = CreateOutput("rhx-qr-code");

        helper.ViewContext = CreateViewContext();
        helper.Process(context, output);

        AssertAttribute(output, "data-rhx-qr-fill", "#000000");
        AssertAttribute(output, "data-rhx-qr-background", "#ffffff");
    }

    [Fact]
    public void Custom_Fill_And_Background()
    {
        var helper = CreateHelper();
        helper.Fill = "#1e40af";
        helper.Background = "#f0f9ff";
        var context = CreateContext("rhx-qr-code");
        var output = CreateOutput("rhx-qr-code");

        helper.ViewContext = CreateViewContext();
        helper.Process(context, output);

        AssertAttribute(output, "data-rhx-qr-fill", "#1e40af");
        AssertAttribute(output, "data-rhx-qr-background", "#f0f9ff");
    }

    // ── Label / Accessibility ──

    [Fact]
    public void Label_Sets_Aria_Label()
    {
        var helper = CreateHelper();
        helper.Label = "Share link QR code";
        var context = CreateContext("rhx-qr-code");
        var output = CreateOutput("rhx-qr-code");

        helper.ViewContext = CreateViewContext();
        helper.Process(context, output);

        AssertAttribute(output, "aria-label", "Share link QR code");
    }

    [Fact]
    public void No_Label_Omits_Aria_Label()
    {
        var helper = CreateHelper();
        var context = CreateContext("rhx-qr-code");
        var output = CreateOutput("rhx-qr-code");

        helper.ViewContext = CreateViewContext();
        helper.Process(context, output);

        AssertNoAttribute(output, "aria-label");
    }

    // ── Error correction ──

    [Fact]
    public void Default_Error_Correction_M()
    {
        var helper = CreateHelper();
        var context = CreateContext("rhx-qr-code");
        var output = CreateOutput("rhx-qr-code");

        helper.ViewContext = CreateViewContext();
        helper.Process(context, output);

        AssertAttribute(output, "data-rhx-qr-ec", "M");
    }

    [Fact]
    public void Custom_Error_Correction()
    {
        var helper = CreateHelper();
        helper.ErrorCorrection = "H";
        var context = CreateContext("rhx-qr-code");
        var output = CreateOutput("rhx-qr-code");

        helper.ViewContext = CreateViewContext();
        helper.Process(context, output);

        AssertAttribute(output, "data-rhx-qr-ec", "H");
    }

    [Fact]
    public void Error_Correction_Normalized_To_Uppercase()
    {
        var helper = CreateHelper();
        helper.ErrorCorrection = "h";
        var context = CreateContext("rhx-qr-code");
        var output = CreateOutput("rhx-qr-code");

        helper.ViewContext = CreateViewContext();
        helper.Process(context, output);

        AssertAttribute(output, "data-rhx-qr-ec", "H");
    }

    // ── Radius ──

    [Fact]
    public void Default_Radius_Omits_Data_Attribute()
    {
        var helper = CreateHelper();
        var context = CreateContext("rhx-qr-code");
        var output = CreateOutput("rhx-qr-code");

        helper.ViewContext = CreateViewContext();
        helper.Process(context, output);

        AssertNoAttribute(output, "data-rhx-qr-radius");
    }

    [Fact]
    public void Custom_Radius_Sets_Data_Attribute()
    {
        var helper = CreateHelper();
        helper.Radius = 0.5;
        var context = CreateContext("rhx-qr-code");
        var output = CreateOutput("rhx-qr-code");

        helper.ViewContext = CreateViewContext();
        helper.Process(context, output);

        AssertAttribute(output, "data-rhx-qr-radius", "0.50");
    }

    // ── CSS class merging ──

    [Fact]
    public void Custom_Css_Class_Is_Merged()
    {
        var helper = CreateHelper();
        helper.CssClass = "my-qr";
        var context = CreateContext("rhx-qr-code");
        var output = CreateOutput("rhx-qr-code");

        helper.ViewContext = CreateViewContext();
        helper.Process(context, output);

        Assert.True(HasClass(output, "rhx-qr-code"));
        Assert.True(HasClass(output, "my-qr"));
    }

    // ── Id attribute ──

    [Fact]
    public void Id_Sets_Html_Attribute()
    {
        var helper = CreateHelper();
        helper.Id = "share-qr";
        var context = CreateContext("rhx-qr-code");
        var output = CreateOutput("rhx-qr-code");

        helper.ViewContext = CreateViewContext();
        helper.Process(context, output);

        AssertAttribute(output, "id", "share-qr");
    }
}
