using htmxRazor.Components.Actions;
using Xunit;

namespace htmxRazor.Tests;

public class ButtonTagHelperTests : TagHelperTestBase
{
    private ButtonTagHelper CreateHelper()
    {
        var helper = new ButtonTagHelper(CreateUrlHelperFactory());
        helper.ViewContext = CreateViewContext();
        return helper;
    }

    // ──────────────────────────────────────────────
    //  Default rendering
    // ──────────────────────────────────────────────

    [Fact]
    public async Task Renders_Button_Element_By_Default()
    {
        var helper = CreateHelper();
        var context = CreateContext("rhx-button");
        var output = CreateOutput("rhx-button", childContent: "Click");

        await helper.ProcessAsync(context, output);

        Assert.Equal("button", output.TagName);
        AssertAttribute(output, "type", "button");
    }

    [Fact]
    public async Task Renders_Block_Class_And_Default_Modifiers()
    {
        var helper = CreateHelper();
        var context = CreateContext("rhx-button");
        var output = CreateOutput("rhx-button", childContent: "Click");

        await helper.ProcessAsync(context, output);

        Assert.True(HasClass(output, "rhx-button"));
        Assert.True(HasClass(output, "rhx-button--neutral"));
        Assert.True(HasClass(output, "rhx-button--filled"));
    }

    [Fact]
    public async Task Default_Medium_Size_Has_No_Size_Modifier()
    {
        var helper = CreateHelper();
        var context = CreateContext("rhx-button");
        var output = CreateOutput("rhx-button", childContent: "Click");

        await helper.ProcessAsync(context, output);

        Assert.False(HasClass(output, "rhx-button--medium"));
        Assert.False(HasClass(output, "rhx-button--small"));
        Assert.False(HasClass(output, "rhx-button--large"));
    }

    [Fact]
    public async Task Wraps_Child_Content_In_Label_Span()
    {
        var helper = CreateHelper();
        var context = CreateContext("rhx-button");
        var output = CreateOutput("rhx-button", childContent: "Save");

        await helper.ProcessAsync(context, output);

        var content = output.Content.GetContent();
        Assert.Contains("<span class=\"rhx-button__label\">", content);
        Assert.Contains("Save", content);
        Assert.Contains("</span>", content);
    }

    // ──────────────────────────────────────────────
    //  Variants
    // ──────────────────────────────────────────────

    [Theory]
    [InlineData("neutral", "rhx-button--neutral")]
    [InlineData("brand", "rhx-button--brand")]
    [InlineData("success", "rhx-button--success")]
    [InlineData("warning", "rhx-button--warning")]
    [InlineData("danger", "rhx-button--danger")]
    public async Task Renders_Correct_Variant_Class(string variant, string expectedClass)
    {
        var helper = CreateHelper();
        helper.Variant = variant;
        var context = CreateContext("rhx-button");
        var output = CreateOutput("rhx-button", childContent: "Click");

        await helper.ProcessAsync(context, output);

        Assert.True(HasClass(output, expectedClass));
    }

    // ──────────────────────────────────────────────
    //  Appearances
    // ──────────────────────────────────────────────

    [Theory]
    [InlineData("filled", "rhx-button--filled")]
    [InlineData("outlined", "rhx-button--outlined")]
    [InlineData("plain", "rhx-button--plain")]
    public async Task Renders_Correct_Appearance_Class(string appearance, string expectedClass)
    {
        var helper = CreateHelper();
        helper.Appearance = appearance;
        var context = CreateContext("rhx-button");
        var output = CreateOutput("rhx-button", childContent: "Click");

        await helper.ProcessAsync(context, output);

        Assert.True(HasClass(output, expectedClass));
    }

    // ──────────────────────────────────────────────
    //  Sizes
    // ──────────────────────────────────────────────

    [Fact]
    public async Task Renders_Small_Size_Class()
    {
        var helper = CreateHelper();
        helper.Size = "small";
        var context = CreateContext("rhx-button");
        var output = CreateOutput("rhx-button", childContent: "Click");

        await helper.ProcessAsync(context, output);

        Assert.True(HasClass(output, "rhx-button--small"));
    }

    [Fact]
    public async Task Renders_Large_Size_Class()
    {
        var helper = CreateHelper();
        helper.Size = "large";
        var context = CreateContext("rhx-button");
        var output = CreateOutput("rhx-button", childContent: "Click");

        await helper.ProcessAsync(context, output);

        Assert.True(HasClass(output, "rhx-button--large"));
    }

    // ──────────────────────────────────────────────
    //  Shape modifiers
    // ──────────────────────────────────────────────

    [Fact]
    public async Task Pill_Adds_Pill_Modifier()
    {
        var helper = CreateHelper();
        helper.Pill = true;
        var context = CreateContext("rhx-button");
        var output = CreateOutput("rhx-button", childContent: "Click");

        await helper.ProcessAsync(context, output);

        Assert.True(HasClass(output, "rhx-button--pill"));
    }

    [Fact]
    public async Task Circle_Adds_Circle_Modifier()
    {
        var helper = CreateHelper();
        helper.Circle = true;
        var context = CreateContext("rhx-button");
        var output = CreateOutput("rhx-button", childContent: "+");

        await helper.ProcessAsync(context, output);

        Assert.True(HasClass(output, "rhx-button--circle"));
    }

    // ──────────────────────────────────────────────
    //  Disabled state
    // ──────────────────────────────────────────────

    [Fact]
    public async Task Disabled_Button_Sets_Disabled_Attribute()
    {
        var helper = CreateHelper();
        helper.Disabled = true;
        var context = CreateContext("rhx-button");
        var output = CreateOutput("rhx-button", childContent: "Click");

        await helper.ProcessAsync(context, output);

        Assert.Equal("disabled", GetAttribute(output, "disabled"));
    }

    [Fact]
    public async Task Disabled_Button_Does_Not_Add_Disabled_Modifier_Class()
    {
        var helper = CreateHelper();
        helper.Disabled = true;
        var context = CreateContext("rhx-button");
        var output = CreateOutput("rhx-button", childContent: "Click");

        await helper.ProcessAsync(context, output);

        // For native <button>, disabled attribute is sufficient; modifier class is only for <a>
        Assert.False(HasClass(output, "rhx-button--disabled"));
    }

    // ──────────────────────────────────────────────
    //  Loading state
    // ──────────────────────────────────────────────

    [Fact]
    public async Task Loading_Sets_Disabled_And_AriaBusy()
    {
        var helper = CreateHelper();
        helper.Loading = true;
        var context = CreateContext("rhx-button");
        var output = CreateOutput("rhx-button", childContent: "Click");

        await helper.ProcessAsync(context, output);

        Assert.Equal("disabled", GetAttribute(output, "disabled"));
        AssertAttribute(output, "aria-busy", "true");
        Assert.True(HasClass(output, "rhx-button--loading"));
    }

    [Fact]
    public async Task Loading_Prepends_Spinner()
    {
        var helper = CreateHelper();
        helper.Loading = true;
        var context = CreateContext("rhx-button");
        var output = CreateOutput("rhx-button", childContent: "Saving");

        await helper.ProcessAsync(context, output);

        var content = output.Content.GetContent();
        Assert.Contains("rhx-button__spinner", content);
        Assert.Contains("rhx-spinner", content);
        Assert.Contains("role=\"status\"", content);
        // Spinner should appear before the label
        var spinnerPos = content.IndexOf("rhx-button__spinner");
        var labelPos = content.IndexOf("rhx-button__label");
        Assert.True(spinnerPos < labelPos);
    }

    [Fact]
    public async Task Not_Loading_Has_No_Spinner()
    {
        var helper = CreateHelper();
        var context = CreateContext("rhx-button");
        var output = CreateOutput("rhx-button", childContent: "Click");

        await helper.ProcessAsync(context, output);

        var content = output.Content.GetContent();
        Assert.DoesNotContain("rhx-button__spinner", content);
        Assert.DoesNotContain("rhx-spinner", content);
    }

    // ──────────────────────────────────────────────
    //  Link rendering (<a> tag)
    // ──────────────────────────────────────────────

    [Fact]
    public async Task Href_Renders_As_Anchor_Tag()
    {
        var helper = CreateHelper();
        helper.Href = "/about";
        var context = CreateContext("rhx-button");
        var output = CreateOutput("rhx-button", childContent: "About");

        await helper.ProcessAsync(context, output);

        Assert.Equal("a", output.TagName);
        AssertAttribute(output, "href", "/about");
        AssertAttribute(output, "role", "button");
        AssertNoAttribute(output, "type");
    }

    [Fact]
    public async Task Link_Target_Blank_Sets_Rel_Noopener()
    {
        var helper = CreateHelper();
        helper.Href = "/external";
        helper.LinkTarget = "_blank";
        var context = CreateContext("rhx-button");
        var output = CreateOutput("rhx-button", childContent: "Open");

        await helper.ProcessAsync(context, output);

        AssertAttribute(output, "target", "_blank");
        AssertAttribute(output, "rel", "noopener noreferrer");
    }

    [Fact]
    public async Task Link_Target_Self_Has_No_Rel()
    {
        var helper = CreateHelper();
        helper.Href = "/page";
        helper.LinkTarget = "_self";
        var context = CreateContext("rhx-button");
        var output = CreateOutput("rhx-button", childContent: "Go");

        await helper.ProcessAsync(context, output);

        AssertAttribute(output, "target", "_self");
        AssertNoAttribute(output, "rel");
    }

    [Fact]
    public async Task Download_Sets_Download_Attribute()
    {
        var helper = CreateHelper();
        helper.Href = "/file.pdf";
        helper.Download = "report.pdf";
        var context = CreateContext("rhx-button");
        var output = CreateOutput("rhx-button", childContent: "Download");

        await helper.ProcessAsync(context, output);

        AssertAttribute(output, "download", "report.pdf");
    }

    [Fact]
    public async Task Disabled_Link_Uses_AriaDisabled_And_TabindexMinus1()
    {
        var helper = CreateHelper();
        helper.Href = "/about";
        helper.Disabled = true;
        var context = CreateContext("rhx-button");
        var output = CreateOutput("rhx-button", childContent: "About");

        await helper.ProcessAsync(context, output);

        Assert.Equal("a", output.TagName);
        AssertAttribute(output, "aria-disabled", "true");
        AssertAttribute(output, "tabindex", "-1");
        Assert.True(HasClass(output, "rhx-button--disabled"));
        AssertNoAttribute(output, "disabled");
    }

    // ──────────────────────────────────────────────
    //  Form attributes
    // ──────────────────────────────────────────────

    [Fact]
    public async Task Submit_Type_Renders()
    {
        var helper = CreateHelper();
        helper.ButtonType = "submit";
        var context = CreateContext("rhx-button");
        var output = CreateOutput("rhx-button", childContent: "Submit");

        await helper.ProcessAsync(context, output);

        AssertAttribute(output, "type", "submit");
    }

    [Fact]
    public async Task Name_And_Value_Render()
    {
        var helper = CreateHelper();
        helper.Name = "action";
        helper.FormValue = "save";
        var context = CreateContext("rhx-button");
        var output = CreateOutput("rhx-button", childContent: "Save");

        await helper.ProcessAsync(context, output);

        AssertAttribute(output, "name", "action");
        AssertAttribute(output, "value", "save");
    }

    [Fact]
    public async Task Name_And_Value_Not_Rendered_On_Links()
    {
        var helper = CreateHelper();
        helper.Href = "/about";
        helper.Name = "action";
        helper.FormValue = "save";
        var context = CreateContext("rhx-button");
        var output = CreateOutput("rhx-button", childContent: "About");

        await helper.ProcessAsync(context, output);

        AssertNoAttribute(output, "name");
        AssertNoAttribute(output, "value");
    }

    // ──────────────────────────────────────────────
    //  Accessibility
    // ──────────────────────────────────────────────

    [Fact]
    public async Task AriaLabel_Sets_Attribute()
    {
        var helper = CreateHelper();
        helper.AriaLabel = "Close dialog";
        var context = CreateContext("rhx-button");
        var output = CreateOutput("rhx-button", childContent: "X");

        await helper.ProcessAsync(context, output);

        AssertAttribute(output, "aria-label", "Close dialog");
    }

    // ──────────────────────────────────────────────
    //  Custom CSS class
    // ──────────────────────────────────────────────

    [Fact]
    public async Task Custom_CssClass_Appended()
    {
        var helper = CreateHelper();
        helper.CssClass = "my-custom";
        var context = CreateContext("rhx-button");
        var output = CreateOutput("rhx-button", childContent: "Click");

        await helper.ProcessAsync(context, output);

        Assert.True(HasClass(output, "my-custom"));
        Assert.True(HasClass(output, "rhx-button"));
    }

    // ──────────────────────────────────────────────
    //  htmx attributes
    // ──────────────────────────────────────────────

    [Fact]
    public async Task HxGet_Renders()
    {
        var helper = CreateHelper();
        helper.HxGet = "/api/data";
        var context = CreateContext("rhx-button");
        var output = CreateOutput("rhx-button", childContent: "Load");

        await helper.ProcessAsync(context, output);

        AssertAttribute(output, "hx-get", "/api/data");
    }

    [Fact]
    public async Task HxPost_With_Target_And_Swap_Renders()
    {
        var helper = CreateHelper();
        helper.HxPost = "/api/submit";
        helper.HxTarget = "#result";
        helper.HxSwap = "innerHTML";
        var context = CreateContext("rhx-button");
        var output = CreateOutput("rhx-button", childContent: "Submit");

        await helper.ProcessAsync(context, output);

        AssertAttribute(output, "hx-post", "/api/submit");
        AssertAttribute(output, "hx-target", "#result");
        AssertAttribute(output, "hx-swap", "innerHTML");
    }

    [Fact]
    public async Task Null_Htmx_Attributes_Not_Rendered()
    {
        var helper = CreateHelper();
        var context = CreateContext("rhx-button");
        var output = CreateOutput("rhx-button", childContent: "Click");

        await helper.ProcessAsync(context, output);

        AssertNoAttribute(output, "hx-get");
        AssertNoAttribute(output, "hx-post");
        AssertNoAttribute(output, "hx-target");
        AssertNoAttribute(output, "hx-swap");
    }

    [Fact]
    public async Task HxConfirm_Renders()
    {
        var helper = CreateHelper();
        helper.HxPost = "/api/delete";
        helper.HxConfirm = "Are you sure?";
        var context = CreateContext("rhx-button");
        var output = CreateOutput("rhx-button", childContent: "Delete");

        await helper.ProcessAsync(context, output);

        AssertAttribute(output, "hx-confirm", "Are you sure?");
    }

    // ──────────────────────────────────────────────
    //  Variant × Appearance combinations
    // ──────────────────────────────────────────────

    [Theory]
    [InlineData("brand", "outlined")]
    [InlineData("danger", "plain")]
    [InlineData("success", "filled")]
    [InlineData("warning", "outlined")]
    [InlineData("neutral", "plain")]
    public async Task Variant_And_Appearance_Combine(string variant, string appearance)
    {
        var helper = CreateHelper();
        helper.Variant = variant;
        helper.Appearance = appearance;
        var context = CreateContext("rhx-button");
        var output = CreateOutput("rhx-button", childContent: "Click");

        await helper.ProcessAsync(context, output);

        Assert.True(HasClass(output, $"rhx-button--{variant}"));
        Assert.True(HasClass(output, $"rhx-button--{appearance}"));
    }

    // ──────────────────────────────────────────────
    //  Case insensitivity
    // ──────────────────────────────────────────────

    [Fact]
    public async Task Variant_Is_Case_Insensitive()
    {
        var helper = CreateHelper();
        helper.Variant = "Brand";
        var context = CreateContext("rhx-button");
        var output = CreateOutput("rhx-button", childContent: "Click");

        await helper.ProcessAsync(context, output);

        Assert.True(HasClass(output, "rhx-button--brand"));
    }

    [Fact]
    public async Task Appearance_Is_Case_Insensitive()
    {
        var helper = CreateHelper();
        helper.Appearance = "Outlined";
        var context = CreateContext("rhx-button");
        var output = CreateOutput("rhx-button", childContent: "Click");

        await helper.ProcessAsync(context, output);

        Assert.True(HasClass(output, "rhx-button--outlined"));
    }

    [Fact]
    public async Task Size_Is_Case_Insensitive()
    {
        var helper = CreateHelper();
        helper.Size = "Large";
        var context = CreateContext("rhx-button");
        var output = CreateOutput("rhx-button", childContent: "Click");

        await helper.ProcessAsync(context, output);

        Assert.True(HasClass(output, "rhx-button--large"));
    }
}
