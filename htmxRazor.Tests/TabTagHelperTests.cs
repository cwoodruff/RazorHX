using Microsoft.AspNetCore.Razor.TagHelpers;
using htmxRazor.Components.Navigation;
using Xunit;

namespace htmxRazor.Tests;

public class TabTagHelperTests : TagHelperTestBase
{
    // ──────────────────────────────────────────────
    //  Helpers
    // ──────────────────────────────────────────────

    private TabGroupTagHelper CreateGroupHelper()
    {
        var helper = new TabGroupTagHelper(CreateUrlHelperFactory());
        helper.ViewContext = CreateViewContext();
        return helper;
    }

    private TabTagHelper CreateTabHelper()
    {
        var helper = new TabTagHelper(CreateUrlHelperFactory());
        helper.ViewContext = CreateViewContext();
        return helper;
    }

    private TabPanelTagHelper CreatePanelHelper()
    {
        var helper = new TabPanelTagHelper(CreateUrlHelperFactory());
        helper.ViewContext = CreateViewContext();
        return helper;
    }

    /// <summary>
    /// Creates a context with the RhxTabsNav list pre-registered (simulating parent).
    /// Returns both the context and the list for inspection.
    /// </summary>
    private (TagHelperContext context, List<string> navList) CreateTabContext(string tagName = "rhx-tab")
    {
        var context = CreateContext(tagName);
        var navList = new List<string>();
        context.Items["RhxTabsNav"] = navList;
        return (context, navList);
    }

    // ══════════════════════════════════════════════
    //  TabGroupTagHelper
    // ══════════════════════════════════════════════

    [Fact]
    public async Task Group_Renders_Div_With_Block_Class()
    {
        var helper = CreateGroupHelper();
        var context = CreateContext("rhx-tab-group");
        var output = CreateOutput("rhx-tab-group", childContent: "");

        await helper.ProcessAsync(context, output);

        Assert.Equal("div", output.TagName);
        Assert.True(HasClass(output, "rhx-tab-group"));
    }

    [Fact]
    public async Task Group_Has_Data_Tabs_Attribute()
    {
        var helper = CreateGroupHelper();
        var context = CreateContext("rhx-tab-group");
        var output = CreateOutput("rhx-tab-group", childContent: "");

        await helper.ProcessAsync(context, output);

        Assert.NotNull(output.Attributes["data-rhx-tabs"]);
    }

    [Fact]
    public async Task Group_Default_Placement_Is_Top()
    {
        var helper = CreateGroupHelper();
        var context = CreateContext("rhx-tab-group");
        var output = CreateOutput("rhx-tab-group", childContent: "");

        await helper.ProcessAsync(context, output);

        AssertAttribute(output, "data-rhx-placement", "top");
        // No modifier class for default top placement
        Assert.False(HasClass(output, "rhx-tab-group--top"));
    }

    [Fact]
    public async Task Group_Bottom_Placement_Adds_Modifier()
    {
        var helper = CreateGroupHelper();
        helper.Placement = "bottom";
        var context = CreateContext("rhx-tab-group");
        var output = CreateOutput("rhx-tab-group", childContent: "");

        await helper.ProcessAsync(context, output);

        Assert.True(HasClass(output, "rhx-tab-group--bottom"));
        AssertAttribute(output, "data-rhx-placement", "bottom");
    }

    [Fact]
    public async Task Group_Start_Placement_Adds_Modifier()
    {
        var helper = CreateGroupHelper();
        helper.Placement = "start";
        var context = CreateContext("rhx-tab-group");
        var output = CreateOutput("rhx-tab-group", childContent: "");

        await helper.ProcessAsync(context, output);

        Assert.True(HasClass(output, "rhx-tab-group--start"));
    }

    [Fact]
    public async Task Group_End_Placement_Adds_Modifier()
    {
        var helper = CreateGroupHelper();
        helper.Placement = "end";
        var context = CreateContext("rhx-tab-group");
        var output = CreateOutput("rhx-tab-group", childContent: "");

        await helper.ProcessAsync(context, output);

        Assert.True(HasClass(output, "rhx-tab-group--end"));
    }

    [Fact]
    public async Task Group_Default_Activation_No_Data_Attribute()
    {
        var helper = CreateGroupHelper();
        var context = CreateContext("rhx-tab-group");
        var output = CreateOutput("rhx-tab-group", childContent: "");

        await helper.ProcessAsync(context, output);

        Assert.Null(output.Attributes["data-rhx-activation"]);
    }

    [Fact]
    public async Task Group_Manual_Activation_Sets_Data_Attribute()
    {
        var helper = CreateGroupHelper();
        helper.Activation = "manual";
        var context = CreateContext("rhx-tab-group");
        var output = CreateOutput("rhx-tab-group", childContent: "");

        await helper.ProcessAsync(context, output);

        AssertAttribute(output, "data-rhx-activation", "manual");
    }

    [Fact]
    public async Task Group_Nav_Has_Tablist_Role()
    {
        var helper = CreateGroupHelper();
        var context = CreateContext("rhx-tab-group");
        var output = CreateOutput("rhx-tab-group", childContent: "");

        await helper.ProcessAsync(context, output);

        var content = output.Content.GetContent();
        Assert.Contains("role=\"tablist\"", content);
    }

    [Fact]
    public async Task Group_Nav_Has_Element_Class()
    {
        var helper = CreateGroupHelper();
        var context = CreateContext("rhx-tab-group");
        var output = CreateOutput("rhx-tab-group", childContent: "");

        await helper.ProcessAsync(context, output);

        var content = output.Content.GetContent();
        Assert.Contains("class=\"rhx-tab-group__nav\"", content);
    }

    [Fact]
    public async Task Group_Body_Has_Element_Class()
    {
        var helper = CreateGroupHelper();
        var context = CreateContext("rhx-tab-group");
        var output = CreateOutput("rhx-tab-group", childContent: "panel content");

        await helper.ProcessAsync(context, output);

        var content = output.Content.GetContent();
        Assert.Contains("class=\"rhx-tab-group__body\"", content);
        Assert.Contains("panel content", content);
    }

    [Fact]
    public async Task Group_AriaLabel_On_Nav()
    {
        var helper = CreateGroupHelper();
        helper.AriaLabel = "Settings tabs";
        var context = CreateContext("rhx-tab-group");
        var output = CreateOutput("rhx-tab-group", childContent: "");

        await helper.ProcessAsync(context, output);

        var content = output.Content.GetContent();
        Assert.Contains("aria-label=\"Settings tabs\"", content);
    }

    [Fact]
    public async Task Group_Vertical_Placement_Has_Orientation()
    {
        var helper = CreateGroupHelper();
        helper.Placement = "start";
        var context = CreateContext("rhx-tab-group");
        var output = CreateOutput("rhx-tab-group", childContent: "");

        await helper.ProcessAsync(context, output);

        var content = output.Content.GetContent();
        Assert.Contains("aria-orientation=\"vertical\"", content);
    }

    [Fact]
    public async Task Group_Horizontal_Placement_No_Orientation()
    {
        var helper = CreateGroupHelper();
        var context = CreateContext("rhx-tab-group");
        var output = CreateOutput("rhx-tab-group", childContent: "");

        await helper.ProcessAsync(context, output);

        var content = output.Content.GetContent();
        Assert.DoesNotContain("aria-orientation", content);
    }

    [Fact]
    public async Task Group_Custom_CssClass_Appended()
    {
        var helper = CreateGroupHelper();
        helper.CssClass = "my-tabs";
        var context = CreateContext("rhx-tab-group");
        var output = CreateOutput("rhx-tab-group", childContent: "");

        await helper.ProcessAsync(context, output);

        Assert.True(HasClass(output, "my-tabs"));
        Assert.True(HasClass(output, "rhx-tab-group"));
    }

    // ══════════════════════════════════════════════
    //  TabTagHelper
    // ══════════════════════════════════════════════

    [Fact]
    public async Task Tab_Suppresses_Output()
    {
        var helper = CreateTabHelper();
        helper.Panel = "general";
        var (context, _) = CreateTabContext();
        var output = CreateOutput("rhx-tab", childContent: "General");

        await helper.ProcessAsync(context, output);

        Assert.Null(output.TagName);
    }

    [Fact]
    public async Task Tab_Registers_Button_In_Nav_List()
    {
        var helper = CreateTabHelper();
        helper.Panel = "general";
        var (context, navList) = CreateTabContext();
        var output = CreateOutput("rhx-tab", childContent: "General");

        await helper.ProcessAsync(context, output);

        Assert.Single(navList);
        Assert.Contains("<button", navList[0]);
        Assert.Contains("</button>", navList[0]);
    }

    [Fact]
    public async Task Tab_Has_Block_Class()
    {
        var helper = CreateTabHelper();
        helper.Panel = "general";
        var (context, navList) = CreateTabContext();
        var output = CreateOutput("rhx-tab", childContent: "General");

        await helper.ProcessAsync(context, output);

        Assert.Contains("class=\"rhx-tab\"", navList[0]);
    }

    [Fact]
    public async Task Tab_Has_Role_Tab()
    {
        var helper = CreateTabHelper();
        helper.Panel = "general";
        var (context, navList) = CreateTabContext();
        var output = CreateOutput("rhx-tab", childContent: "General");

        await helper.ProcessAsync(context, output);

        Assert.Contains("role=\"tab\"", navList[0]);
    }

    [Fact]
    public async Task Tab_Has_Correct_Id()
    {
        var helper = CreateTabHelper();
        helper.Panel = "general";
        var (context, navList) = CreateTabContext();
        var output = CreateOutput("rhx-tab", childContent: "General");

        await helper.ProcessAsync(context, output);

        Assert.Contains("id=\"tab-general\"", navList[0]);
    }

    [Fact]
    public async Task Tab_Has_Aria_Controls()
    {
        var helper = CreateTabHelper();
        helper.Panel = "general";
        var (context, navList) = CreateTabContext();
        var output = CreateOutput("rhx-tab", childContent: "General");

        await helper.ProcessAsync(context, output);

        Assert.Contains("aria-controls=\"panel-general\"", navList[0]);
    }

    [Fact]
    public async Task Active_Tab_Has_Aria_Selected_True()
    {
        var helper = CreateTabHelper();
        helper.Panel = "general";
        helper.Active = true;
        var (context, navList) = CreateTabContext();
        var output = CreateOutput("rhx-tab", childContent: "General");

        await helper.ProcessAsync(context, output);

        Assert.Contains("aria-selected=\"true\"", navList[0]);
    }

    [Fact]
    public async Task Inactive_Tab_Has_Aria_Selected_False()
    {
        var helper = CreateTabHelper();
        helper.Panel = "general";
        var (context, navList) = CreateTabContext();
        var output = CreateOutput("rhx-tab", childContent: "General");

        await helper.ProcessAsync(context, output);

        Assert.Contains("aria-selected=\"false\"", navList[0]);
    }

    [Fact]
    public async Task Active_Tab_Has_Tabindex_Zero()
    {
        var helper = CreateTabHelper();
        helper.Panel = "general";
        helper.Active = true;
        var (context, navList) = CreateTabContext();
        var output = CreateOutput("rhx-tab", childContent: "General");

        await helper.ProcessAsync(context, output);

        Assert.Contains("tabindex=\"0\"", navList[0]);
    }

    [Fact]
    public async Task Inactive_Tab_Has_Tabindex_Minus_One()
    {
        var helper = CreateTabHelper();
        helper.Panel = "general";
        var (context, navList) = CreateTabContext();
        var output = CreateOutput("rhx-tab", childContent: "General");

        await helper.ProcessAsync(context, output);

        Assert.Contains("tabindex=\"-1\"", navList[0]);
    }

    [Fact]
    public async Task Active_Tab_Has_Active_Modifier()
    {
        var helper = CreateTabHelper();
        helper.Panel = "general";
        helper.Active = true;
        var (context, navList) = CreateTabContext();
        var output = CreateOutput("rhx-tab", childContent: "General");

        await helper.ProcessAsync(context, output);

        Assert.Contains("rhx-tab--active", navList[0]);
    }

    [Fact]
    public async Task Disabled_Tab_Has_Disabled_Attribute()
    {
        var helper = CreateTabHelper();
        helper.Panel = "general";
        helper.Disabled = true;
        var (context, navList) = CreateTabContext();
        var output = CreateOutput("rhx-tab", childContent: "General");

        await helper.ProcessAsync(context, output);

        Assert.Contains(" disabled", navList[0]);
        Assert.Contains("aria-disabled=\"true\"", navList[0]);
        Assert.Contains("rhx-tab--disabled", navList[0]);
    }

    [Fact]
    public async Task Tab_Label_Wrapper_Present()
    {
        var helper = CreateTabHelper();
        helper.Panel = "general";
        var (context, navList) = CreateTabContext();
        var output = CreateOutput("rhx-tab", childContent: "General");

        await helper.ProcessAsync(context, output);

        Assert.Contains("<span class=\"rhx-tab__label\">", navList[0]);
        Assert.Contains("General", navList[0]);
    }

    [Fact]
    public async Task Closable_Tab_Has_Close_Button()
    {
        var helper = CreateTabHelper();
        helper.Panel = "general";
        helper.Closable = true;
        var (context, navList) = CreateTabContext();
        var output = CreateOutput("rhx-tab", childContent: "General");

        await helper.ProcessAsync(context, output);

        Assert.Contains("rhx-tab__close", navList[0]);
        Assert.Contains("aria-hidden=\"true\"", navList[0]);
        Assert.Contains("&times;", navList[0]);
        Assert.Contains("rhx-tab--closable", navList[0]);
    }

    [Fact]
    public async Task Non_Closable_Tab_No_Close_Button()
    {
        var helper = CreateTabHelper();
        helper.Panel = "general";
        var (context, navList) = CreateTabContext();
        var output = CreateOutput("rhx-tab", childContent: "General");

        await helper.ProcessAsync(context, output);

        Assert.DoesNotContain("rhx-tab__close", navList[0]);
    }

    [Fact]
    public async Task Disabled_Closable_Tab_No_Close_Button()
    {
        var helper = CreateTabHelper();
        helper.Panel = "general";
        helper.Closable = true;
        helper.Disabled = true;
        var (context, navList) = CreateTabContext();
        var output = CreateOutput("rhx-tab", childContent: "General");

        await helper.ProcessAsync(context, output);

        Assert.DoesNotContain("rhx-tab__close", navList[0]);
    }

    [Fact]
    public async Task Tab_Htmx_Get_Renders_On_Button()
    {
        var helper = CreateTabHelper();
        helper.Panel = "settings";
        helper.HxGet = "/Settings/Content";
        helper.HxTarget = "#panel-settings";
        helper.HxSwap = "innerHTML";
        var (context, navList) = CreateTabContext();
        var output = CreateOutput("rhx-tab", childContent: "Settings");

        await helper.ProcessAsync(context, output);

        Assert.Contains("hx-get=\"/Settings/Content\"", navList[0]);
        Assert.Contains("hx-target=\"#panel-settings\"", navList[0]);
        Assert.Contains("hx-swap=\"innerHTML\"", navList[0]);
    }

    [Fact]
    public async Task Tab_Custom_CssClass_Appended()
    {
        var helper = CreateTabHelper();
        helper.Panel = "general";
        helper.CssClass = "my-tab";
        var (context, navList) = CreateTabContext();
        var output = CreateOutput("rhx-tab", childContent: "General");

        await helper.ProcessAsync(context, output);

        Assert.Contains("my-tab", navList[0]);
        Assert.Contains("rhx-tab", navList[0]);
    }

    [Fact]
    public async Task Multiple_Tabs_Register_In_Order()
    {
        var (context, navList) = CreateTabContext();

        var tab1 = CreateTabHelper();
        tab1.Panel = "first";
        tab1.Active = true;
        var output1 = CreateOutput("rhx-tab", childContent: "First");
        await tab1.ProcessAsync(context, output1);

        var tab2 = CreateTabHelper();
        tab2.Panel = "second";
        var output2 = CreateOutput("rhx-tab", childContent: "Second");
        await tab2.ProcessAsync(context, output2);

        Assert.Equal(2, navList.Count);
        Assert.Contains("First", navList[0]);
        Assert.Contains("Second", navList[1]);
    }

    // ══════════════════════════════════════════════
    //  TabPanelTagHelper
    // ══════════════════════════════════════════════

    [Fact]
    public void Panel_Renders_Div()
    {
        var helper = CreatePanelHelper();
        helper.Name = "general";
        helper.Active = true;
        var context = CreateContext("rhx-tab-panel");
        var output = CreateOutput("rhx-tab-panel", childContent: "Content");

        helper.Process(context, output);

        Assert.Equal("div", output.TagName);
    }

    [Fact]
    public void Panel_Has_Block_Class()
    {
        var helper = CreatePanelHelper();
        helper.Name = "general";
        helper.Active = true;
        var context = CreateContext("rhx-tab-panel");
        var output = CreateOutput("rhx-tab-panel", childContent: "Content");

        helper.Process(context, output);

        Assert.True(HasClass(output, "rhx-tab-panel"));
    }

    [Fact]
    public void Panel_Has_Role_Tabpanel()
    {
        var helper = CreatePanelHelper();
        helper.Name = "general";
        helper.Active = true;
        var context = CreateContext("rhx-tab-panel");
        var output = CreateOutput("rhx-tab-panel", childContent: "Content");

        helper.Process(context, output);

        AssertAttribute(output, "role", "tabpanel");
    }

    [Fact]
    public void Panel_Has_Correct_Id()
    {
        var helper = CreatePanelHelper();
        helper.Name = "general";
        helper.Active = true;
        var context = CreateContext("rhx-tab-panel");
        var output = CreateOutput("rhx-tab-panel", childContent: "Content");

        helper.Process(context, output);

        AssertAttribute(output, "id", "panel-general");
    }

    [Fact]
    public void Panel_Has_Aria_Labelledby()
    {
        var helper = CreatePanelHelper();
        helper.Name = "general";
        helper.Active = true;
        var context = CreateContext("rhx-tab-panel");
        var output = CreateOutput("rhx-tab-panel", childContent: "Content");

        helper.Process(context, output);

        AssertAttribute(output, "aria-labelledby", "tab-general");
    }

    [Fact]
    public void Panel_Has_Tabindex_Zero()
    {
        var helper = CreatePanelHelper();
        helper.Name = "general";
        helper.Active = true;
        var context = CreateContext("rhx-tab-panel");
        var output = CreateOutput("rhx-tab-panel", childContent: "Content");

        helper.Process(context, output);

        AssertAttribute(output, "tabindex", "0");
    }

    [Fact]
    public void Active_Panel_Not_Hidden()
    {
        var helper = CreatePanelHelper();
        helper.Name = "general";
        helper.Active = true;
        var context = CreateContext("rhx-tab-panel");
        var output = CreateOutput("rhx-tab-panel", childContent: "Content");

        helper.Process(context, output);

        AssertNoAttribute(output, "hidden");
    }

    [Fact]
    public void Inactive_Panel_Has_Hidden()
    {
        var helper = CreatePanelHelper();
        helper.Name = "general";
        var context = CreateContext("rhx-tab-panel");
        var output = CreateOutput("rhx-tab-panel", childContent: "Content");

        helper.Process(context, output);

        AssertAttribute(output, "hidden", "hidden");
    }

    [Fact]
    public void Active_Panel_Has_Active_Modifier()
    {
        var helper = CreatePanelHelper();
        helper.Name = "general";
        helper.Active = true;
        var context = CreateContext("rhx-tab-panel");
        var output = CreateOutput("rhx-tab-panel", childContent: "Content");

        helper.Process(context, output);

        Assert.True(HasClass(output, "rhx-tab-panel--active"));
    }

    [Fact]
    public void Inactive_Panel_No_Active_Modifier()
    {
        var helper = CreatePanelHelper();
        helper.Name = "general";
        var context = CreateContext("rhx-tab-panel");
        var output = CreateOutput("rhx-tab-panel", childContent: "Content");

        helper.Process(context, output);

        Assert.False(HasClass(output, "rhx-tab-panel--active"));
    }

    [Fact]
    public void Panel_Custom_CssClass_Appended()
    {
        var helper = CreatePanelHelper();
        helper.Name = "general";
        helper.Active = true;
        helper.CssClass = "my-panel";
        var context = CreateContext("rhx-tab-panel");
        var output = CreateOutput("rhx-tab-panel", childContent: "Content");

        helper.Process(context, output);

        Assert.True(HasClass(output, "my-panel"));
        Assert.True(HasClass(output, "rhx-tab-panel"));
    }

    [Fact]
    public void Panel_Htmx_Attributes_Rendered()
    {
        var helper = CreatePanelHelper();
        helper.Name = "lazy";
        helper.Active = true;
        helper.HxGet = "/Lazy/Content";
        helper.HxTrigger = "revealed";
        var context = CreateContext("rhx-tab-panel");
        var output = CreateOutput("rhx-tab-panel", childContent: "");

        helper.Process(context, output);

        AssertAttribute(output, "hx-get", "/Lazy/Content");
        AssertAttribute(output, "hx-trigger", "revealed");
    }

    // ══════════════════════════════════════════════
    //  Keyboard Interaction Contract (documentation)
    // ══════════════════════════════════════════════
    //
    // The following keyboard behaviors are implemented in rhx-tabs.js
    // and tested via browser/integration tests (out of scope for xUnit):
    //
    // Auto activation mode:
    //   ArrowRight/Down  → Focus and activate next tab (wraps)
    //   ArrowLeft/Up     → Focus and activate previous tab (wraps)
    //   Home             → Focus and activate first tab
    //   End              → Focus and activate last tab
    //
    // Manual activation mode:
    //   ArrowRight/Down  → Focus next tab (no activation)
    //   ArrowLeft/Up     → Focus previous tab (no activation)
    //   Home             → Focus first tab
    //   End              → Focus last tab
    //   Enter/Space      → Activate focused tab
    //
    // Horizontal (top/bottom): ArrowLeft/ArrowRight
    // Vertical (start/end):    ArrowUp/ArrowDown
    //
    // Close button:
    //   Click close (×)  → Dispatch rhx:tab:close, remove tab and panel
}
