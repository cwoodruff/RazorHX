using htmxRazor.Components.Navigation;
using Xunit;

namespace htmxRazor.Tests;

public class SkipNavTagHelperTests : TagHelperTestBase
{
    // ──────────────────────────────────────────────
    //  Helpers
    // ──────────────────────────────────────────────

    private static SkipNavTagHelper CreateHelper()
    {
        var helper = new SkipNavTagHelper();
        helper.ViewContext = CreateViewContext();
        return helper;
    }

    // ══════════════════════════════════════════════
    //  Structure
    // ══════════════════════════════════════════════

    [Fact]
    public async Task Renders_Anchor_Element()
    {
        var helper = CreateHelper();
        var context = CreateContext("rhx-skip-nav");
        var output = CreateOutput("rhx-skip-nav");

        await helper.ProcessAsync(context, output);

        Assert.Equal("a", output.TagName);
    }

    [Fact]
    public async Task Has_Block_Class()
    {
        var helper = CreateHelper();
        var context = CreateContext("rhx-skip-nav");
        var output = CreateOutput("rhx-skip-nav");

        await helper.ProcessAsync(context, output);

        Assert.True(HasClass(output, "rhx-skip-nav"));
    }

    [Fact]
    public async Task Default_Href_Is_MainContent()
    {
        var helper = CreateHelper();
        var context = CreateContext("rhx-skip-nav");
        var output = CreateOutput("rhx-skip-nav");

        await helper.ProcessAsync(context, output);

        AssertAttribute(output, "href", "#main-content");
    }

    [Fact]
    public async Task Default_Label_Text()
    {
        var helper = CreateHelper();
        var context = CreateContext("rhx-skip-nav");
        var output = CreateOutput("rhx-skip-nav");

        await helper.ProcessAsync(context, output);

        var content = output.Content.GetContent();
        Assert.Equal("Skip to main content", content);
    }

    // ══════════════════════════════════════════════
    //  Custom target & label
    // ══════════════════════════════════════════════

    [Fact]
    public async Task Custom_Target()
    {
        var helper = CreateHelper();
        helper.Target = "#content";
        var context = CreateContext("rhx-skip-nav");
        var output = CreateOutput("rhx-skip-nav");

        await helper.ProcessAsync(context, output);

        AssertAttribute(output, "href", "#content");
    }

    [Fact]
    public async Task Custom_Label()
    {
        var helper = CreateHelper();
        helper.Label = "Skip to content";
        var context = CreateContext("rhx-skip-nav");
        var output = CreateOutput("rhx-skip-nav");

        await helper.ProcessAsync(context, output);

        var content = output.Content.GetContent();
        Assert.Equal("Skip to content", content);
    }

    // ══════════════════════════════════════════════
    //  CSS class
    // ══════════════════════════════════════════════

    [Fact]
    public async Task Custom_CssClass_Appended()
    {
        var helper = CreateHelper();
        helper.CssClass = "my-skip";
        var context = CreateContext("rhx-skip-nav");
        var output = CreateOutput("rhx-skip-nav");

        await helper.ProcessAsync(context, output);

        Assert.True(HasClass(output, "my-skip"));
        Assert.True(HasClass(output, "rhx-skip-nav"));
    }

    // ══════════════════════════════════════════════
    //  Id
    // ══════════════════════════════════════════════

    [Fact]
    public async Task Id_Rendered()
    {
        var helper = CreateHelper();
        helper.Id = "skip-link";
        var context = CreateContext("rhx-skip-nav");
        var output = CreateOutput("rhx-skip-nav");

        await helper.ProcessAsync(context, output);

        AssertAttribute(output, "id", "skip-link");
    }
}
