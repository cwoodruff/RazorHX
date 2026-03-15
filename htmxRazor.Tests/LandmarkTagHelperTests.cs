using htmxRazor.Components.Navigation;
using Xunit;

namespace htmxRazor.Tests;

public class LandmarkTagHelperTests : TagHelperTestBase
{
    // ──────────────────────────────────────────────
    //  Helpers
    // ──────────────────────────────────────────────

    private LandmarkTagHelper CreateHelper()
    {
        var helper = new LandmarkTagHelper(CreateUrlHelperFactory());
        helper.ViewContext = CreateViewContext();
        return helper;
    }

    // ══════════════════════════════════════════════
    //  Default rendering
    // ══════════════════════════════════════════════

    [Fact]
    public async Task Default_Role_Renders_Section()
    {
        var helper = CreateHelper();
        var context = CreateContext("rhx-landmark");
        var output = CreateOutput("rhx-landmark", childContent: "content");

        await helper.ProcessAsync(context, output);

        Assert.Equal("section", output.TagName);
    }

    [Fact]
    public async Task Has_Block_Class()
    {
        var helper = CreateHelper();
        var context = CreateContext("rhx-landmark");
        var output = CreateOutput("rhx-landmark", childContent: "");

        await helper.ProcessAsync(context, output);

        Assert.True(HasClass(output, "rhx-landmark"));
    }

    // ══════════════════════════════════════════════
    //  Role → element mapping
    // ══════════════════════════════════════════════

    [Theory]
    [InlineData("banner", "header")]
    [InlineData("navigation", "nav")]
    [InlineData("main", "main")]
    [InlineData("complementary", "aside")]
    [InlineData("contentinfo", "footer")]
    [InlineData("region", "section")]
    [InlineData("search", "search")]
    [InlineData("form", "form")]
    public async Task Role_Maps_To_Correct_Element(string role, string expectedElement)
    {
        var helper = CreateHelper();
        helper.Role = role;
        var context = CreateContext("rhx-landmark");
        var output = CreateOutput("rhx-landmark", childContent: "");

        await helper.ProcessAsync(context, output);

        Assert.Equal(expectedElement, output.TagName);
    }

    [Fact]
    public async Task Unknown_Role_Defaults_To_Section()
    {
        var helper = CreateHelper();
        helper.Role = "unknown";
        var context = CreateContext("rhx-landmark");
        var output = CreateOutput("rhx-landmark", childContent: "");

        await helper.ProcessAsync(context, output);

        Assert.Equal("section", output.TagName);
    }

    // ══════════════════════════════════════════════
    //  Role case insensitivity
    // ══════════════════════════════════════════════

    [Fact]
    public async Task Role_Is_Case_Insensitive()
    {
        var helper = CreateHelper();
        helper.Role = "NAVIGATION";
        var context = CreateContext("rhx-landmark");
        var output = CreateOutput("rhx-landmark", childContent: "");

        await helper.ProcessAsync(context, output);

        Assert.Equal("nav", output.TagName);
    }

    // ══════════════════════════════════════════════
    //  Label
    // ══════════════════════════════════════════════

    [Fact]
    public async Task Label_Sets_AriaLabel()
    {
        var helper = CreateHelper();
        helper.Label = "Primary navigation";
        var context = CreateContext("rhx-landmark");
        var output = CreateOutput("rhx-landmark", childContent: "");

        await helper.ProcessAsync(context, output);

        AssertAttribute(output, "aria-label", "Primary navigation");
    }

    [Fact]
    public async Task No_Label_No_AriaLabel()
    {
        var helper = CreateHelper();
        var context = CreateContext("rhx-landmark");
        var output = CreateOutput("rhx-landmark", childContent: "");

        await helper.ProcessAsync(context, output);

        AssertNoAttribute(output, "aria-label");
    }

    // ══════════════════════════════════════════════
    //  CSS class
    // ══════════════════════════════════════════════

    [Fact]
    public async Task Custom_CssClass_Appended()
    {
        var helper = CreateHelper();
        helper.CssClass = "my-region";
        var context = CreateContext("rhx-landmark");
        var output = CreateOutput("rhx-landmark", childContent: "");

        await helper.ProcessAsync(context, output);

        Assert.True(HasClass(output, "my-region"));
        Assert.True(HasClass(output, "rhx-landmark"));
    }

    // ══════════════════════════════════════════════
    //  htmx support
    // ══════════════════════════════════════════════

    [Fact]
    public async Task Renders_Htmx_Attributes()
    {
        var helper = CreateHelper();
        helper.HxGet = "/api/content";
        helper.HxTarget = "#body";
        var context = CreateContext("rhx-landmark");
        var output = CreateOutput("rhx-landmark", childContent: "");

        await helper.ProcessAsync(context, output);

        AssertAttribute(output, "hx-get", "/api/content");
        AssertAttribute(output, "hx-target", "#body");
    }

    // ══════════════════════════════════════════════
    //  Id
    // ══════════════════════════════════════════════

    [Fact]
    public async Task Id_Rendered()
    {
        var helper = CreateHelper();
        helper.Id = "main-section";
        var context = CreateContext("rhx-landmark");
        var output = CreateOutput("rhx-landmark", childContent: "");

        await helper.ProcessAsync(context, output);

        AssertAttribute(output, "id", "main-section");
    }
}
