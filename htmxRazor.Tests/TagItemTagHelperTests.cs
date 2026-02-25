using htmxRazor.Components.Feedback;
using Xunit;

namespace htmxRazor.Tests;

public class TagItemTagHelperTests : TagHelperTestBase
{
    private TagItemTagHelper CreateHelper()
    {
        var helper = new TagItemTagHelper(CreateUrlHelperFactory());
        helper.ViewContext = CreateViewContext();
        return helper;
    }

    // ──────────────────────────────────────────────
    //  Default rendering
    // ──────────────────────────────────────────────

    [Fact]
    public async Task Renders_Span_Element()
    {
        var helper = CreateHelper();
        var context = CreateContext("rhx-tag");
        var output = CreateOutput("rhx-tag", childContent: "C#");

        await helper.ProcessAsync(context, output);

        Assert.Equal("span", output.TagName);
    }

    [Fact]
    public async Task Renders_Block_Class_And_Default_Variant()
    {
        var helper = CreateHelper();
        var context = CreateContext("rhx-tag");
        var output = CreateOutput("rhx-tag", childContent: "C#");

        await helper.ProcessAsync(context, output);

        Assert.True(HasClass(output, "rhx-tag"));
        Assert.True(HasClass(output, "rhx-tag--neutral"));
    }

    [Fact]
    public async Task Wraps_Child_Content_In_Label_Span()
    {
        var helper = CreateHelper();
        var context = CreateContext("rhx-tag");
        var output = CreateOutput("rhx-tag", childContent: "C#");

        await helper.ProcessAsync(context, output);

        var content = output.Content.GetContent();
        Assert.Contains("rhx-tag__label", content);
        Assert.Contains("C#", content);
    }

    [Fact]
    public async Task Default_No_Remove_Button()
    {
        var helper = CreateHelper();
        var context = CreateContext("rhx-tag");
        var output = CreateOutput("rhx-tag", childContent: "C#");

        await helper.ProcessAsync(context, output);

        var content = output.Content.GetContent();
        Assert.DoesNotContain("rhx-tag__remove", content);
    }

    [Fact]
    public async Task Default_No_Size_Modifier()
    {
        var helper = CreateHelper();
        var context = CreateContext("rhx-tag");
        var output = CreateOutput("rhx-tag", childContent: "C#");

        await helper.ProcessAsync(context, output);

        Assert.False(HasClass(output, "rhx-tag--medium"));
        Assert.False(HasClass(output, "rhx-tag--small"));
        Assert.False(HasClass(output, "rhx-tag--large"));
    }

    [Fact]
    public async Task Default_No_Pill_Modifier()
    {
        var helper = CreateHelper();
        var context = CreateContext("rhx-tag");
        var output = CreateOutput("rhx-tag", childContent: "C#");

        await helper.ProcessAsync(context, output);

        Assert.False(HasClass(output, "rhx-tag--pill"));
    }

    // ──────────────────────────────────────────────
    //  Variants
    // ──────────────────────────────────────────────

    [Theory]
    [InlineData("neutral", "rhx-tag--neutral")]
    [InlineData("brand", "rhx-tag--brand")]
    [InlineData("success", "rhx-tag--success")]
    [InlineData("warning", "rhx-tag--warning")]
    [InlineData("danger", "rhx-tag--danger")]
    public async Task Renders_Correct_Variant_Class(string variant, string expectedClass)
    {
        var helper = CreateHelper();
        helper.Variant = variant;
        var context = CreateContext("rhx-tag");
        var output = CreateOutput("rhx-tag", childContent: "Tag");

        await helper.ProcessAsync(context, output);

        Assert.True(HasClass(output, expectedClass));
    }

    [Fact]
    public async Task Variant_Is_Case_Insensitive()
    {
        var helper = CreateHelper();
        helper.Variant = "Brand";
        var context = CreateContext("rhx-tag");
        var output = CreateOutput("rhx-tag", childContent: "Tag");

        await helper.ProcessAsync(context, output);

        Assert.True(HasClass(output, "rhx-tag--brand"));
    }

    // ──────────────────────────────────────────────
    //  Sizes
    // ──────────────────────────────────────────────

    [Fact]
    public async Task Small_Size_Adds_Modifier()
    {
        var helper = CreateHelper();
        helper.Size = "small";
        var context = CreateContext("rhx-tag");
        var output = CreateOutput("rhx-tag", childContent: "Tag");

        await helper.ProcessAsync(context, output);

        Assert.True(HasClass(output, "rhx-tag--small"));
    }

    [Fact]
    public async Task Large_Size_Adds_Modifier()
    {
        var helper = CreateHelper();
        helper.Size = "large";
        var context = CreateContext("rhx-tag");
        var output = CreateOutput("rhx-tag", childContent: "Tag");

        await helper.ProcessAsync(context, output);

        Assert.True(HasClass(output, "rhx-tag--large"));
    }

    [Fact]
    public async Task Size_Is_Case_Insensitive()
    {
        var helper = CreateHelper();
        helper.Size = "Small";
        var context = CreateContext("rhx-tag");
        var output = CreateOutput("rhx-tag", childContent: "Tag");

        await helper.ProcessAsync(context, output);

        Assert.True(HasClass(output, "rhx-tag--small"));
    }

    // ──────────────────────────────────────────────
    //  Pill modifier
    // ──────────────────────────────────────────────

    [Fact]
    public async Task Pill_Adds_Pill_Modifier()
    {
        var helper = CreateHelper();
        helper.Pill = true;
        var context = CreateContext("rhx-tag");
        var output = CreateOutput("rhx-tag", childContent: "Tag");

        await helper.ProcessAsync(context, output);

        Assert.True(HasClass(output, "rhx-tag--pill"));
    }

    // ──────────────────────────────────────────────
    //  Removable
    // ──────────────────────────────────────────────

    [Fact]
    public async Task Removable_Renders_Remove_Button()
    {
        var helper = CreateHelper();
        helper.Removable = true;
        var context = CreateContext("rhx-tag");
        var output = CreateOutput("rhx-tag", childContent: "C#");

        await helper.ProcessAsync(context, output);

        var content = output.Content.GetContent();
        Assert.Contains("rhx-tag__remove", content);
        Assert.Contains("aria-label=\"Remove\"", content);
        Assert.Contains("type=\"button\"", content);
    }

    [Fact]
    public async Task Removable_Adds_Removable_Modifier()
    {
        var helper = CreateHelper();
        helper.Removable = true;
        var context = CreateContext("rhx-tag");
        var output = CreateOutput("rhx-tag", childContent: "C#");

        await helper.ProcessAsync(context, output);

        Assert.True(HasClass(output, "rhx-tag--removable"));
    }

    [Fact]
    public async Task Removable_Button_Contains_Svg_Icon()
    {
        var helper = CreateHelper();
        helper.Removable = true;
        var context = CreateContext("rhx-tag");
        var output = CreateOutput("rhx-tag", childContent: "Tag");

        await helper.ProcessAsync(context, output);

        var content = output.Content.GetContent();
        Assert.Contains("<svg", content);
    }

    // ──────────────────────────────────────────────
    //  Combined modifiers
    // ──────────────────────────────────────────────

    [Fact]
    public async Task Variant_Size_Pill_Removable()
    {
        var helper = CreateHelper();
        helper.Variant = "danger";
        helper.Size = "large";
        helper.Pill = true;
        helper.Removable = true;
        var context = CreateContext("rhx-tag");
        var output = CreateOutput("rhx-tag", childContent: "Tag");

        await helper.ProcessAsync(context, output);

        Assert.True(HasClass(output, "rhx-tag--danger"));
        Assert.True(HasClass(output, "rhx-tag--large"));
        Assert.True(HasClass(output, "rhx-tag--pill"));
        Assert.True(HasClass(output, "rhx-tag--removable"));
    }

    // ──────────────────────────────────────────────
    //  Custom CSS class
    // ──────────────────────────────────────────────

    [Fact]
    public async Task Custom_CssClass_Appended()
    {
        var helper = CreateHelper();
        helper.CssClass = "my-tag";
        var context = CreateContext("rhx-tag");
        var output = CreateOutput("rhx-tag", childContent: "C#");

        await helper.ProcessAsync(context, output);

        Assert.True(HasClass(output, "my-tag"));
        Assert.True(HasClass(output, "rhx-tag"));
    }

    // ──────────────────────────────────────────────
    //  htmx attributes
    // ──────────────────────────────────────────────

    [Fact]
    public async Task HxDelete_Renders()
    {
        var helper = CreateHelper();
        helper.HxDelete = "/tags/42";
        var context = CreateContext("rhx-tag");
        var output = CreateOutput("rhx-tag", childContent: "Tag");

        await helper.ProcessAsync(context, output);

        AssertAttribute(output, "hx-delete", "/tags/42");
    }

    [Fact]
    public async Task HxPost_With_Target_And_Swap()
    {
        var helper = CreateHelper();
        helper.HxPost = "/tags";
        helper.HxTarget = "closest .rhx-tag";
        helper.HxSwap = "outerHTML swap:200ms";
        var context = CreateContext("rhx-tag");
        var output = CreateOutput("rhx-tag", childContent: "Tag");

        await helper.ProcessAsync(context, output);

        AssertAttribute(output, "hx-post", "/tags");
        AssertAttribute(output, "hx-target", "closest .rhx-tag");
        AssertAttribute(output, "hx-swap", "outerHTML swap:200ms");
    }

    [Fact]
    public async Task Null_Htmx_Attributes_Not_Rendered()
    {
        var helper = CreateHelper();
        var context = CreateContext("rhx-tag");
        var output = CreateOutput("rhx-tag", childContent: "Tag");

        await helper.ProcessAsync(context, output);

        AssertNoAttribute(output, "hx-get");
        AssertNoAttribute(output, "hx-post");
        AssertNoAttribute(output, "hx-delete");
    }

    // ──────────────────────────────────────────────
    //  Tag mode
    // ──────────────────────────────────────────────

    [Fact]
    public async Task Uses_StartTagAndEndTag_Mode()
    {
        var helper = CreateHelper();
        var context = CreateContext("rhx-tag");
        var output = CreateOutput("rhx-tag", childContent: "Tag");

        await helper.ProcessAsync(context, output);

        Assert.Equal(Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, output.TagMode);
    }
}
