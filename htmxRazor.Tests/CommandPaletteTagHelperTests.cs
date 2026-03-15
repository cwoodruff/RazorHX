using htmxRazor.Components.Overlays;
using Xunit;

namespace htmxRazor.Tests;

public class CommandPaletteTagHelperTests : TagHelperTestBase
{
    private CommandPaletteTagHelper CreateHelper()
    {
        var helper = new CommandPaletteTagHelper(CreateUrlHelperFactory());
        helper.ViewContext = CreateViewContext();
        return helper;
    }

    // ══════════════════════════════════════════════
    //  Structure
    // ══════════════════════════════════════════════

    [Fact]
    public async Task Renders_Div_Element()
    {
        var helper = CreateHelper();
        var context = CreateContext("rhx-command-palette");
        var output = CreateOutput("rhx-command-palette", childContent: "");

        await helper.ProcessAsync(context, output);

        Assert.Equal("div", output.TagName);
    }

    [Fact]
    public async Task Has_Block_Class()
    {
        var helper = CreateHelper();
        var context = CreateContext("rhx-command-palette");
        var output = CreateOutput("rhx-command-palette", childContent: "");

        await helper.ProcessAsync(context, output);

        Assert.True(HasClass(output, "rhx-command-palette"));
    }

    [Fact]
    public async Task Hidden_By_Default()
    {
        var helper = CreateHelper();
        var context = CreateContext("rhx-command-palette");
        var output = CreateOutput("rhx-command-palette", childContent: "");

        await helper.ProcessAsync(context, output);

        AssertAttribute(output, "hidden", "hidden");
    }

    [Fact]
    public async Task Has_Dialog_Role()
    {
        var helper = CreateHelper();
        var context = CreateContext("rhx-command-palette");
        var output = CreateOutput("rhx-command-palette", childContent: "");

        await helper.ProcessAsync(context, output);

        AssertAttribute(output, "role", "dialog");
        AssertAttribute(output, "aria-modal", "true");
    }

    [Fact]
    public async Task Has_AriaLabel()
    {
        var helper = CreateHelper();
        helper.Label = "Quick search";
        var context = CreateContext("rhx-command-palette");
        var output = CreateOutput("rhx-command-palette", childContent: "");

        await helper.ProcessAsync(context, output);

        AssertAttribute(output, "aria-label", "Quick search");
    }

    [Fact]
    public async Task Has_DataAttribute()
    {
        var helper = CreateHelper();
        var context = CreateContext("rhx-command-palette");
        var output = CreateOutput("rhx-command-palette", childContent: "");

        await helper.ProcessAsync(context, output);

        AssertAttribute(output, "data-rhx-command-palette", "");
    }

    [Fact]
    public async Task Shortcut_DataAttribute()
    {
        var helper = CreateHelper();
        helper.Shortcut = "mod+k";
        var context = CreateContext("rhx-command-palette");
        var output = CreateOutput("rhx-command-palette", childContent: "");

        await helper.ProcessAsync(context, output);

        AssertAttribute(output, "data-rhx-shortcut", "mod+k");
    }

    // ══════════════════════════════════════════════
    //  Inner content
    // ══════════════════════════════════════════════

    [Fact]
    public async Task Contains_Backdrop()
    {
        var helper = CreateHelper();
        var context = CreateContext("rhx-command-palette");
        var output = CreateOutput("rhx-command-palette", childContent: "");

        await helper.ProcessAsync(context, output);

        var content = output.Content.GetContent();
        Assert.Contains("rhx-command-palette__backdrop", content);
    }

    [Fact]
    public async Task Contains_Panel()
    {
        var helper = CreateHelper();
        var context = CreateContext("rhx-command-palette");
        var output = CreateOutput("rhx-command-palette", childContent: "");

        await helper.ProcessAsync(context, output);

        var content = output.Content.GetContent();
        Assert.Contains("rhx-command-palette__panel", content);
    }

    [Fact]
    public async Task Contains_Input_With_Combobox_Role()
    {
        var helper = CreateHelper();
        var context = CreateContext("rhx-command-palette");
        var output = CreateOutput("rhx-command-palette", childContent: "");

        await helper.ProcessAsync(context, output);

        var content = output.Content.GetContent();
        Assert.Contains("role=\"combobox\"", content);
        Assert.Contains("aria-expanded=\"false\"", content);
        Assert.Contains("aria-autocomplete=\"list\"", content);
        Assert.Contains("autocomplete=\"off\"", content);
    }

    [Fact]
    public async Task Placeholder_Rendered()
    {
        var helper = CreateHelper();
        helper.Placeholder = "Find anything...";
        var context = CreateContext("rhx-command-palette");
        var output = CreateOutput("rhx-command-palette", childContent: "");

        await helper.ProcessAsync(context, output);

        var content = output.Content.GetContent();
        Assert.Contains("placeholder=\"Find anything...\"", content);
    }

    [Fact]
    public async Task Contains_Results_With_Listbox_Role()
    {
        var helper = CreateHelper();
        helper.Id = "cp";
        var context = CreateContext("rhx-command-palette");
        var output = CreateOutput("rhx-command-palette", childContent: "");

        await helper.ProcessAsync(context, output);

        var content = output.Content.GetContent();
        Assert.Contains("role=\"listbox\"", content);
        Assert.Contains("id=\"cp-results\"", content);
    }

    [Fact]
    public async Task Input_AriaControls_Points_To_Results()
    {
        var helper = CreateHelper();
        helper.Id = "search";
        var context = CreateContext("rhx-command-palette");
        var output = CreateOutput("rhx-command-palette", childContent: "");

        await helper.ProcessAsync(context, output);

        var content = output.Content.GetContent();
        Assert.Contains("aria-controls=\"search-results\"", content);
    }

    [Fact]
    public async Task Contains_Empty_Message()
    {
        var helper = CreateHelper();
        helper.EmptyMessage = "Nothing here";
        var context = CreateContext("rhx-command-palette");
        var output = CreateOutput("rhx-command-palette", childContent: "");

        await helper.ProcessAsync(context, output);

        var content = output.Content.GetContent();
        Assert.Contains("Nothing here", content);
        Assert.Contains("rhx-command-palette__empty", content);
    }

    [Fact]
    public async Task Contains_Search_Icon()
    {
        var helper = CreateHelper();
        var context = CreateContext("rhx-command-palette");
        var output = CreateOutput("rhx-command-palette", childContent: "");

        await helper.ProcessAsync(context, output);

        var content = output.Content.GetContent();
        Assert.Contains("rhx-command-palette__search-icon", content);
    }

    [Fact]
    public async Task Contains_Shortcut_Kbd()
    {
        var helper = CreateHelper();
        var context = CreateContext("rhx-command-palette");
        var output = CreateOutput("rhx-command-palette", childContent: "");

        await helper.ProcessAsync(context, output);

        var content = output.Content.GetContent();
        Assert.Contains("rhx-command-palette__shortcut", content);
        Assert.Contains("<kbd", content);
    }

    // ══════════════════════════════════════════════
    //  htmx forwarding
    // ══════════════════════════════════════════════

    [Fact]
    public async Task HxGet_Forwarded_To_Input()
    {
        var helper = CreateHelper();
        helper.HxGet = "/api/search";
        var context = CreateContext("rhx-command-palette");
        var output = CreateOutput("rhx-command-palette", childContent: "");

        await helper.ProcessAsync(context, output);

        var content = output.Content.GetContent();
        Assert.Contains("hx-get=\"/api/search\"", content);
    }

    [Fact]
    public async Task Debounce_In_HxTrigger()
    {
        var helper = CreateHelper();
        helper.Debounce = 500;
        var context = CreateContext("rhx-command-palette");
        var output = CreateOutput("rhx-command-palette", childContent: "");

        await helper.ProcessAsync(context, output);

        var content = output.Content.GetContent();
        Assert.Contains("hx-trigger=\"input changed delay:500ms\"", content);
    }

    [Fact]
    public async Task HxTarget_Points_To_Results()
    {
        var helper = CreateHelper();
        helper.HxGet = "/search";
        helper.Id = "mycp";
        var context = CreateContext("rhx-command-palette");
        var output = CreateOutput("rhx-command-palette", childContent: "");

        await helper.ProcessAsync(context, output);

        var content = output.Content.GetContent();
        Assert.Contains("hx-target=\"#mycp-results\"", content);
    }
}
