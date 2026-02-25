using htmxRazor.Components.DataDisplay;
using Xunit;

namespace htmxRazor.Tests;

public class SparklineTagHelperTests : TagHelperTestBase
{
    private SparklineTagHelper CreateHelper()
    {
        return new SparklineTagHelper();
    }

    // ══════════════════════════════════════════════
    //  Structure
    // ══════════════════════════════════════════════

    [Fact]
    public void Renders_Svg_Element()
    {
        var helper = CreateHelper();
        helper.Values = [1, 2, 3];
        var context = CreateContext("rhx-sparkline");
        var output = CreateOutput("rhx-sparkline");

        helper.Process(context, output);

        Assert.Equal("svg", output.TagName);
    }

    [Fact]
    public void Has_Block_Class()
    {
        var helper = CreateHelper();
        helper.Values = [1, 2, 3];
        var context = CreateContext("rhx-sparkline");
        var output = CreateOutput("rhx-sparkline");

        helper.Process(context, output);

        Assert.True(HasClass(output, "rhx-sparkline"));
    }

    [Fact]
    public void Has_ViewBox()
    {
        var helper = CreateHelper();
        helper.Values = [1, 2, 3];
        var context = CreateContext("rhx-sparkline");
        var output = CreateOutput("rhx-sparkline");

        helper.Process(context, output);

        AssertAttribute(output, "viewBox", "0 0 200 40");
    }

    [Fact]
    public void Has_PreserveAspectRatio_None()
    {
        var helper = CreateHelper();
        helper.Values = [1, 2, 3];
        var context = CreateContext("rhx-sparkline");
        var output = CreateOutput("rhx-sparkline");

        helper.Process(context, output);

        AssertAttribute(output, "preserveAspectRatio", "none");
    }

    [Fact]
    public void Has_Role_Img()
    {
        var helper = CreateHelper();
        helper.Values = [1, 2, 3];
        var context = CreateContext("rhx-sparkline");
        var output = CreateOutput("rhx-sparkline");

        helper.Process(context, output);

        AssertAttribute(output, "role", "img");
    }

    [Fact]
    public void Custom_Class_Merged()
    {
        var helper = CreateHelper();
        helper.Values = [1, 2, 3];
        helper.CssClass = "cpu-chart";
        var context = CreateContext("rhx-sparkline");
        var output = CreateOutput("rhx-sparkline");

        helper.Process(context, output);

        Assert.True(HasClass(output, "rhx-sparkline"));
        Assert.True(HasClass(output, "cpu-chart"));
    }

    // ══════════════════════════════════════════════
    //  Accessibility
    // ══════════════════════════════════════════════

    [Fact]
    public void Label_Sets_Aria_Label()
    {
        var helper = CreateHelper();
        helper.Values = [1, 2, 3];
        helper.Label = "CPU usage trend";
        var context = CreateContext("rhx-sparkline");
        var output = CreateOutput("rhx-sparkline");

        helper.Process(context, output);

        AssertAttribute(output, "aria-label", "CPU usage trend");
        AssertNoAttribute(output, "aria-hidden");
    }

    [Fact]
    public void No_Label_Sets_Aria_Hidden()
    {
        var helper = CreateHelper();
        helper.Values = [1, 2, 3];
        var context = CreateContext("rhx-sparkline");
        var output = CreateOutput("rhx-sparkline");

        helper.Process(context, output);

        AssertAttribute(output, "aria-hidden", "true");
        AssertNoAttribute(output, "aria-label");
    }

    // ══════════════════════════════════════════════
    //  Sizing
    // ══════════════════════════════════════════════

    [Fact]
    public void Default_Size_Style()
    {
        var helper = CreateHelper();
        helper.Values = [1, 2, 3];
        var context = CreateContext("rhx-sparkline");
        var output = CreateOutput("rhx-sparkline");

        helper.Process(context, output);

        var style = GetAttribute(output, "style");
        Assert.Contains("width:100%", style);
        Assert.Contains("height:2rem", style);
    }

    [Fact]
    public void Custom_Size()
    {
        var helper = CreateHelper();
        helper.Values = [1, 2, 3];
        helper.Width = "200px";
        helper.Height = "40px";
        var context = CreateContext("rhx-sparkline");
        var output = CreateOutput("rhx-sparkline");

        helper.Process(context, output);

        var style = GetAttribute(output, "style");
        Assert.Contains("width:200px", style);
        Assert.Contains("height:40px", style);
    }

    // ══════════════════════════════════════════════
    //  Empty / null values
    // ══════════════════════════════════════════════

    [Fact]
    public void Null_Values_Renders_Empty_Svg()
    {
        var helper = CreateHelper();
        helper.Values = null;
        var context = CreateContext("rhx-sparkline");
        var output = CreateOutput("rhx-sparkline");

        helper.Process(context, output);

        Assert.Equal("svg", output.TagName);
        Assert.Equal("", output.Content.GetContent());
    }

    [Fact]
    public void Empty_Values_Renders_Empty_Svg()
    {
        var helper = CreateHelper();
        helper.Values = [];
        var context = CreateContext("rhx-sparkline");
        var output = CreateOutput("rhx-sparkline");

        helper.Process(context, output);

        Assert.Equal("svg", output.TagName);
        Assert.Equal("", output.Content.GetContent());
    }

    // ══════════════════════════════════════════════
    //  Line type
    // ══════════════════════════════════════════════

    [Fact]
    public void Line_Contains_Polyline()
    {
        var helper = CreateHelper();
        helper.Values = [1, 2, 3];
        var context = CreateContext("rhx-sparkline");
        var output = CreateOutput("rhx-sparkline");

        helper.Process(context, output);

        var content = output.Content.GetContent();
        Assert.Contains("<polyline", content);
        Assert.Contains("rhx-sparkline__line", content);
    }

    [Fact]
    public void Line_Has_Stroke_Color()
    {
        var helper = CreateHelper();
        helper.Values = [1, 2, 3];
        helper.StrokeColor = "#3b82f6";
        var context = CreateContext("rhx-sparkline");
        var output = CreateOutput("rhx-sparkline");

        helper.Process(context, output);

        var content = output.Content.GetContent();
        Assert.Contains("stroke=\"#3b82f6\"", content);
    }

    [Fact]
    public void Line_Has_Stroke_Width()
    {
        var helper = CreateHelper();
        helper.Values = [1, 2, 3];
        helper.StrokeWidth = 3;
        var context = CreateContext("rhx-sparkline");
        var output = CreateOutput("rhx-sparkline");

        helper.Process(context, output);

        var content = output.Content.GetContent();
        Assert.Contains("stroke-width=\"3\"", content);
    }

    [Fact]
    public void Line_Has_Fill_None()
    {
        var helper = CreateHelper();
        helper.Values = [1, 2, 3];
        var context = CreateContext("rhx-sparkline");
        var output = CreateOutput("rhx-sparkline");

        helper.Process(context, output);

        var content = output.Content.GetContent();
        Assert.Contains("fill=\"none\"", content);
    }

    [Fact]
    public void Line_No_Polygon()
    {
        var helper = CreateHelper();
        helper.Values = [1, 2, 3];
        var context = CreateContext("rhx-sparkline");
        var output = CreateOutput("rhx-sparkline");

        helper.Process(context, output);

        var content = output.Content.GetContent();
        Assert.DoesNotContain("<polygon", content);
    }

    [Fact]
    public void Line_No_Rect()
    {
        var helper = CreateHelper();
        helper.Values = [1, 2, 3];
        var context = CreateContext("rhx-sparkline");
        var output = CreateOutput("rhx-sparkline");

        helper.Process(context, output);

        var content = output.Content.GetContent();
        Assert.DoesNotContain("<rect", content);
    }

    // ══════════════════════════════════════════════
    //  Area type
    // ══════════════════════════════════════════════

    [Fact]
    public void Area_Contains_Polygon_And_Polyline()
    {
        var helper = CreateHelper();
        helper.Values = [1, 2, 3];
        helper.Type = "area";
        var context = CreateContext("rhx-sparkline");
        var output = CreateOutput("rhx-sparkline");

        helper.Process(context, output);

        var content = output.Content.GetContent();
        Assert.Contains("<polygon", content);
        Assert.Contains("rhx-sparkline__area", content);
        Assert.Contains("<polyline", content);
        Assert.Contains("rhx-sparkline__line", content);
    }

    [Fact]
    public void Area_Polygon_Closes_To_Bottom()
    {
        var helper = CreateHelper();
        helper.Values = [0, 10];
        helper.Min = 0;
        helper.Max = 10;
        helper.Type = "area";
        var context = CreateContext("rhx-sparkline");
        var output = CreateOutput("rhx-sparkline");

        helper.Process(context, output);

        var content = output.Content.GetContent();
        // The polygon should start at (x0, 40) and end at (xN, 40) — bottom edge
        Assert.Contains("0,40", content); // bottom-left corner
        Assert.Contains("200,40", content); // bottom-right corner
    }

    [Fact]
    public void Area_Custom_Fill_Color()
    {
        var helper = CreateHelper();
        helper.Values = [1, 2, 3];
        helper.Type = "area";
        helper.FillColor = "#10b98133";
        var context = CreateContext("rhx-sparkline");
        var output = CreateOutput("rhx-sparkline");

        helper.Process(context, output);

        var content = output.Content.GetContent();
        Assert.Contains("fill=\"#10b98133\"", content);
    }

    // ══════════════════════════════════════════════
    //  Bar type
    // ══════════════════════════════════════════════

    [Fact]
    public void Bar_Contains_Rect_Elements()
    {
        var helper = CreateHelper();
        helper.Values = [1, 2, 3];
        helper.Type = "bar";
        var context = CreateContext("rhx-sparkline");
        var output = CreateOutput("rhx-sparkline");

        helper.Process(context, output);

        var content = output.Content.GetContent();
        Assert.Contains("<rect", content);
        Assert.Contains("rhx-sparkline__bar", content);
    }

    [Fact]
    public void Bar_Count_Matches_Values()
    {
        var helper = CreateHelper();
        helper.Values = [1, 2, 3, 4, 5];
        helper.Type = "bar";
        var context = CreateContext("rhx-sparkline");
        var output = CreateOutput("rhx-sparkline");

        helper.Process(context, output);

        var content = output.Content.GetContent();
        var count = content.Split("<rect").Length - 1;
        Assert.Equal(5, count);
    }

    [Fact]
    public void Bar_No_Polyline()
    {
        var helper = CreateHelper();
        helper.Values = [1, 2, 3];
        helper.Type = "bar";
        var context = CreateContext("rhx-sparkline");
        var output = CreateOutput("rhx-sparkline");

        helper.Process(context, output);

        var content = output.Content.GetContent();
        Assert.DoesNotContain("<polyline", content);
    }

    [Fact]
    public void Bar_Custom_Fill_Color()
    {
        var helper = CreateHelper();
        helper.Values = [1, 2, 3];
        helper.Type = "bar";
        helper.FillColor = "#ef4444";
        var context = CreateContext("rhx-sparkline");
        var output = CreateOutput("rhx-sparkline");

        helper.Process(context, output);

        var content = output.Content.GetContent();
        Assert.Contains("fill=\"#ef4444\"", content);
    }

    [Fact]
    public void Bar_Default_Fill_Is_Stroke_Color()
    {
        var helper = CreateHelper();
        helper.Values = [1, 2, 3];
        helper.Type = "bar";
        helper.StrokeColor = "#3b82f6";
        var context = CreateContext("rhx-sparkline");
        var output = CreateOutput("rhx-sparkline");

        helper.Process(context, output);

        var content = output.Content.GetContent();
        Assert.Contains("fill=\"#3b82f6\"", content);
    }

    // ══════════════════════════════════════════════
    //  ComputePoints — unit tests
    // ══════════════════════════════════════════════

    [Fact]
    public void ComputePoints_Count_Matches_Values()
    {
        var points = SparklineTagHelper.ComputePoints(
            [10, 20, 30, 40, 50], null, null, 200, 40, 2);

        Assert.Equal(5, points.Length);
    }

    [Fact]
    public void ComputePoints_X_Evenly_Distributed()
    {
        var points = SparklineTagHelper.ComputePoints(
            [1, 2, 3, 4, 5], null, null, 200, 40, 2);

        Assert.Equal(0, points[0].x);
        Assert.Equal(50, points[1].x);
        Assert.Equal(100, points[2].x);
        Assert.Equal(150, points[3].x);
        Assert.Equal(200, points[4].x);
    }

    [Fact]
    public void ComputePoints_Y_Inverted()
    {
        // Higher values should have lower Y (closer to top of SVG)
        var points = SparklineTagHelper.ComputePoints(
            [0, 100], 0, 100, 200, 40, 2);

        // Value 0 (minimum) should be at bottom: y = height - padding = 38
        Assert.Equal(38, points[0].y);
        // Value 100 (maximum) should be at top: y = padding = 2
        Assert.Equal(2, points[1].y);
    }

    [Fact]
    public void ComputePoints_Min_Max_Override()
    {
        var points = SparklineTagHelper.ComputePoints(
            [50], 0.0, 100.0, 200, 40, 2);

        // With min=0, max=100, value=50 should be at 50% height
        // y = 40 - 2 - 0.5 * 36 = 20
        Assert.Equal(20, points[0].y);
    }

    [Fact]
    public void ComputePoints_Auto_Min_Max()
    {
        var points = SparklineTagHelper.ComputePoints(
            [10, 30], null, null, 200, 40, 2);

        // Auto min=10, max=30
        // Value 10 (min) at bottom: y = 38
        // Value 30 (max) at top: y = 2
        Assert.Equal(38, points[0].y);
        Assert.Equal(2, points[1].y);
    }

    [Fact]
    public void ComputePoints_Single_Value()
    {
        var points = SparklineTagHelper.ComputePoints(
            [42], null, null, 200, 40, 2);

        // Single value: x should be centered at width/2
        Assert.Equal(100, points[0].x);
    }

    [Fact]
    public void ComputePoints_All_Same_Values()
    {
        // All same values — should not crash (division by zero guard)
        var points = SparklineTagHelper.ComputePoints(
            [5, 5, 5], null, null, 200, 40, 2);

        Assert.Equal(3, points.Length);
        // All points at same height (mid-bottom since 5 maps to 0 in normalized range
        // when max - min = 0, we set max = min + 1, so normalized = (5-5)/1 = 0)
        // y = 40 - 2 - 0 * 36 = 38
        Assert.Equal(38, points[0].y);
        Assert.Equal(38, points[1].y);
        Assert.Equal(38, points[2].y);
    }

    [Fact]
    public void ComputePoints_Negative_Values()
    {
        var points = SparklineTagHelper.ComputePoints(
            [-10, 0, 10], null, null, 200, 40, 2);

        // min=-10, max=10, range=20
        // -10 normalized = 0 -> y = 38
        // 0 normalized = 0.5 -> y = 20
        // 10 normalized = 1 -> y = 2
        Assert.Equal(38, points[0].y);
        Assert.Equal(20, points[1].y);
        Assert.Equal(2, points[2].y);
    }

    [Fact]
    public void ComputePoints_Values_Clamped_To_Min_Max()
    {
        // Value 150 exceeds max of 100, should clamp to top
        var points = SparklineTagHelper.ComputePoints(
            [150], 0.0, 100.0, 200, 40, 2);

        Assert.Equal(2, points[0].y);
    }

    [Fact]
    public void ComputePoints_Empty_Returns_Empty()
    {
        var points = SparklineTagHelper.ComputePoints(
            [], null, null, 200, 40, 2);

        Assert.Empty(points);
    }

    // ══════════════════════════════════════════════
    //  FormatPoints
    // ══════════════════════════════════════════════

    [Fact]
    public void FormatPoints_Basic()
    {
        var points = new (double x, double y)[] { (0, 38), (100, 20), (200, 2) };
        var result = SparklineTagHelper.FormatPoints(points);

        Assert.Equal("0,38 100,20 200,2", result);
    }

    [Fact]
    public void FormatPoints_Decimals()
    {
        var points = new (double x, double y)[] { (33.33, 15.5) };
        var result = SparklineTagHelper.FormatPoints(points);

        Assert.Equal("33.33,15.5", result);
    }

    // ══════════════════════════════════════════════
    //  Default stroke properties
    // ══════════════════════════════════════════════

    [Fact]
    public void Default_Stroke_Color()
    {
        var helper = CreateHelper();
        helper.Values = [1, 2, 3];
        var context = CreateContext("rhx-sparkline");
        var output = CreateOutput("rhx-sparkline");

        helper.Process(context, output);

        var content = output.Content.GetContent();
        Assert.Contains("stroke=\"currentColor\"", content);
    }

    [Fact]
    public void Default_Stroke_Width()
    {
        var helper = CreateHelper();
        helper.Values = [1, 2, 3];
        var context = CreateContext("rhx-sparkline");
        var output = CreateOutput("rhx-sparkline");

        helper.Process(context, output);

        var content = output.Content.GetContent();
        Assert.Contains("stroke-width=\"2\"", content);
    }

    // ══════════════════════════════════════════════
    //  Type is case-insensitive
    // ══════════════════════════════════════════════

    [Fact]
    public void Type_Case_Insensitive_Area()
    {
        var helper = CreateHelper();
        helper.Values = [1, 2, 3];
        helper.Type = "Area";
        var context = CreateContext("rhx-sparkline");
        var output = CreateOutput("rhx-sparkline");

        helper.Process(context, output);

        var content = output.Content.GetContent();
        Assert.Contains("<polygon", content);
        Assert.Contains("<polyline", content);
    }

    [Fact]
    public void Type_Case_Insensitive_Bar()
    {
        var helper = CreateHelper();
        helper.Values = [1, 2, 3];
        helper.Type = "BAR";
        var context = CreateContext("rhx-sparkline");
        var output = CreateOutput("rhx-sparkline");

        helper.Process(context, output);

        var content = output.Content.GetContent();
        Assert.Contains("<rect", content);
    }

    // ══════════════════════════════════════════════
    //  Two-point line
    // ══════════════════════════════════════════════

    [Fact]
    public void Two_Points_Line()
    {
        var helper = CreateHelper();
        helper.Values = [0, 10];
        helper.Min = 0;
        helper.Max = 10;
        var context = CreateContext("rhx-sparkline");
        var output = CreateOutput("rhx-sparkline");

        helper.Process(context, output);

        var content = output.Content.GetContent();
        // First point at (0, 38), second at (200, 2)
        Assert.Contains("points=\"0,38 200,2\"", content);
    }
}
