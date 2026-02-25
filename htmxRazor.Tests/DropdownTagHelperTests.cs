using htmxRazor.Components.Actions;
using Xunit;

namespace htmxRazor.Tests;

public class DropdownTagHelperTests : TagHelperTestBase
{
    // ──────────────────────────────────────────────
    //  Helpers
    // ──────────────────────────────────────────────

    private DropdownTagHelper CreateDropdownHelper()
    {
        var helper = new DropdownTagHelper(CreateUrlHelperFactory());
        helper.ViewContext = CreateViewContext();
        return helper;
    }

    private DropdownItemTagHelper CreateItemHelper()
    {
        var helper = new DropdownItemTagHelper(CreateUrlHelperFactory());
        helper.ViewContext = CreateViewContext();
        return helper;
    }

    // ══════════════════════════════════════════════
    //  DropdownTagHelper
    // ══════════════════════════════════════════════

    [Fact]
    public async Task Dropdown_Renders_Div_With_Block_Class()
    {
        var helper = CreateDropdownHelper();
        var context = CreateContext("rhx-dropdown");
        var output = CreateOutput("rhx-dropdown", childContent: "");

        await helper.ProcessAsync(context, output);

        Assert.Equal("div", output.TagName);
        Assert.True(HasClass(output, "rhx-dropdown"));
    }

    [Fact]
    public async Task Dropdown_Has_DataAttribute()
    {
        var helper = CreateDropdownHelper();
        var context = CreateContext("rhx-dropdown");
        var output = CreateOutput("rhx-dropdown", childContent: "");

        await helper.ProcessAsync(context, output);

        Assert.NotNull(output.Attributes["data-rhx-dropdown"]);
    }

    [Fact]
    public async Task Dropdown_Default_Placement_Is_BottomStart()
    {
        var helper = CreateDropdownHelper();
        var context = CreateContext("rhx-dropdown");
        var output = CreateOutput("rhx-dropdown", childContent: "");

        await helper.ProcessAsync(context, output);

        AssertAttribute(output, "data-rhx-placement", "bottom-start");
    }

    [Fact]
    public async Task Dropdown_Custom_Placement_Renders()
    {
        var helper = CreateDropdownHelper();
        helper.Placement = "top-end";
        var context = CreateContext("rhx-dropdown");
        var output = CreateOutput("rhx-dropdown", childContent: "");

        await helper.ProcessAsync(context, output);

        AssertAttribute(output, "data-rhx-placement", "top-end");
    }

    [Fact]
    public async Task Dropdown_Open_Adds_Modifier_Class()
    {
        var helper = CreateDropdownHelper();
        helper.Open = true;
        var context = CreateContext("rhx-dropdown");
        var output = CreateOutput("rhx-dropdown", childContent: "");

        await helper.ProcessAsync(context, output);

        Assert.True(HasClass(output, "rhx-dropdown--open"));
    }

    [Fact]
    public async Task Dropdown_Closed_Has_No_Open_Modifier()
    {
        var helper = CreateDropdownHelper();
        var context = CreateContext("rhx-dropdown");
        var output = CreateOutput("rhx-dropdown", childContent: "");

        await helper.ProcessAsync(context, output);

        Assert.False(HasClass(output, "rhx-dropdown--open"));
    }

    [Fact]
    public async Task Dropdown_Disabled_Adds_Modifier_Class()
    {
        var helper = CreateDropdownHelper();
        helper.Disabled = true;
        var context = CreateContext("rhx-dropdown");
        var output = CreateOutput("rhx-dropdown", childContent: "");

        await helper.ProcessAsync(context, output);

        Assert.True(HasClass(output, "rhx-dropdown--disabled"));
    }

    [Fact]
    public async Task Dropdown_StayOpen_Sets_DataAttribute()
    {
        var helper = CreateDropdownHelper();
        helper.StayOpenOnSelect = true;
        var context = CreateContext("rhx-dropdown");
        var output = CreateOutput("rhx-dropdown", childContent: "");

        await helper.ProcessAsync(context, output);

        Assert.NotNull(output.Attributes["data-rhx-stay-open"]);
    }

    [Fact]
    public async Task Dropdown_No_StayOpen_No_DataAttribute()
    {
        var helper = CreateDropdownHelper();
        var context = CreateContext("rhx-dropdown");
        var output = CreateOutput("rhx-dropdown", childContent: "");

        await helper.ProcessAsync(context, output);

        Assert.Null(output.Attributes["data-rhx-stay-open"]);
    }

    [Fact]
    public async Task Dropdown_Panel_Has_Role_Menu()
    {
        var helper = CreateDropdownHelper();
        var context = CreateContext("rhx-dropdown");
        var output = CreateOutput("rhx-dropdown", childContent: "<button>Item</button>");

        await helper.ProcessAsync(context, output);

        var content = output.Content.GetContent();
        Assert.Contains("role=\"menu\"", content);
    }

    [Fact]
    public async Task Dropdown_Closed_Panel_Has_AriaHidden_True()
    {
        var helper = CreateDropdownHelper();
        var context = CreateContext("rhx-dropdown");
        var output = CreateOutput("rhx-dropdown", childContent: "");

        await helper.ProcessAsync(context, output);

        var content = output.Content.GetContent();
        Assert.Contains("aria-hidden=\"true\"", content);
        Assert.Contains("hidden", content);
    }

    [Fact]
    public async Task Dropdown_Open_Panel_Has_AriaHidden_False()
    {
        var helper = CreateDropdownHelper();
        helper.Open = true;
        var context = CreateContext("rhx-dropdown");
        var output = CreateOutput("rhx-dropdown", childContent: "");

        await helper.ProcessAsync(context, output);

        var content = output.Content.GetContent();
        Assert.Contains("aria-hidden=\"false\"", content);
        Assert.DoesNotContain(" hidden>", content);
    }

    [Fact]
    public async Task Dropdown_Panel_Has_Unique_Id()
    {
        var helper = CreateDropdownHelper();
        var context = CreateContext("rhx-dropdown");
        var output = CreateOutput("rhx-dropdown", childContent: "");

        await helper.ProcessAsync(context, output);

        var content = output.Content.GetContent();
        Assert.Contains("id=\"rhx-dropdown-", content);
    }

    [Fact]
    public async Task Dropdown_Panel_Has_Element_Class()
    {
        var helper = CreateDropdownHelper();
        var context = CreateContext("rhx-dropdown");
        var output = CreateOutput("rhx-dropdown", childContent: "");

        await helper.ProcessAsync(context, output);

        var content = output.Content.GetContent();
        Assert.Contains("class=\"rhx-dropdown__panel\"", content);
    }

    [Fact]
    public async Task Dropdown_AriaLabel_Applied_To_Panel()
    {
        var helper = CreateDropdownHelper();
        helper.AriaLabel = "Actions menu";
        var context = CreateContext("rhx-dropdown");
        var output = CreateOutput("rhx-dropdown", childContent: "");

        await helper.ProcessAsync(context, output);

        var content = output.Content.GetContent();
        Assert.Contains("aria-label=\"Actions menu\"", content);
    }

    [Fact]
    public async Task Dropdown_Custom_CssClass_Appended()
    {
        var helper = CreateDropdownHelper();
        helper.CssClass = "my-dropdown";
        var context = CreateContext("rhx-dropdown");
        var output = CreateOutput("rhx-dropdown", childContent: "");

        await helper.ProcessAsync(context, output);

        Assert.True(HasClass(output, "my-dropdown"));
        Assert.True(HasClass(output, "rhx-dropdown"));
    }

    // ══════════════════════════════════════════════
    //  DropdownItemTagHelper
    // ══════════════════════════════════════════════

    [Fact]
    public async Task Item_Renders_Button_By_Default()
    {
        var helper = CreateItemHelper();
        var context = CreateContext("rhx-dropdown-item");
        var output = CreateOutput("rhx-dropdown-item", childContent: "Edit");

        await helper.ProcessAsync(context, output);

        Assert.Equal("button", output.TagName);
        AssertAttribute(output, "type", "button");
    }

    [Fact]
    public async Task Item_Has_Element_Class()
    {
        var helper = CreateItemHelper();
        var context = CreateContext("rhx-dropdown-item");
        var output = CreateOutput("rhx-dropdown-item", childContent: "Edit");

        await helper.ProcessAsync(context, output);

        Assert.True(HasClass(output, "rhx-dropdown__item"));
    }

    [Fact]
    public async Task Item_Has_Menuitem_Role()
    {
        var helper = CreateItemHelper();
        var context = CreateContext("rhx-dropdown-item");
        var output = CreateOutput("rhx-dropdown-item", childContent: "Edit");

        await helper.ProcessAsync(context, output);

        AssertAttribute(output, "role", "menuitem");
    }

    [Fact]
    public async Task Item_Wraps_Content_In_Label()
    {
        var helper = CreateItemHelper();
        var context = CreateContext("rhx-dropdown-item");
        var output = CreateOutput("rhx-dropdown-item", childContent: "Edit");

        await helper.ProcessAsync(context, output);

        var content = output.Content.GetContent();
        Assert.Contains("<span class=\"rhx-dropdown__item-label\">", content);
        Assert.Contains("Edit", content);
    }

    [Fact]
    public async Task Item_Disabled_Sets_Disabled_Attribute()
    {
        var helper = CreateItemHelper();
        helper.Disabled = true;
        var context = CreateContext("rhx-dropdown-item");
        var output = CreateOutput("rhx-dropdown-item", childContent: "Edit");

        await helper.ProcessAsync(context, output);

        AssertAttribute(output, "disabled", "disabled");
        Assert.True(HasClass(output, "rhx-dropdown__item--disabled"));
    }

    [Fact]
    public async Task Item_Href_Renders_As_Anchor()
    {
        var helper = CreateItemHelper();
        helper.Href = "/settings";
        var context = CreateContext("rhx-dropdown-item");
        var output = CreateOutput("rhx-dropdown-item", childContent: "Settings");

        await helper.ProcessAsync(context, output);

        Assert.Equal("a", output.TagName);
        AssertAttribute(output, "href", "/settings");
        AssertNoAttribute(output, "type");
    }

    [Fact]
    public async Task Item_Disabled_Link_Uses_AriaDisabled()
    {
        var helper = CreateItemHelper();
        helper.Href = "/settings";
        helper.Disabled = true;
        var context = CreateContext("rhx-dropdown-item");
        var output = CreateOutput("rhx-dropdown-item", childContent: "Settings");

        await helper.ProcessAsync(context, output);

        Assert.Equal("a", output.TagName);
        AssertAttribute(output, "aria-disabled", "true");
        AssertAttribute(output, "tabindex", "-1");
        AssertNoAttribute(output, "disabled");
    }

    [Fact]
    public async Task Item_Value_Sets_DataValue()
    {
        var helper = CreateItemHelper();
        helper.Value = "edit-action";
        var context = CreateContext("rhx-dropdown-item");
        var output = CreateOutput("rhx-dropdown-item", childContent: "Edit");

        await helper.ProcessAsync(context, output);

        AssertAttribute(output, "data-value", "edit-action");
    }

    [Fact]
    public async Task Item_AriaLabel_Sets_Attribute()
    {
        var helper = CreateItemHelper();
        helper.AriaLabel = "Edit this item";
        var context = CreateContext("rhx-dropdown-item");
        var output = CreateOutput("rhx-dropdown-item", childContent: "Edit");

        await helper.ProcessAsync(context, output);

        AssertAttribute(output, "aria-label", "Edit this item");
    }

    // ── Checkbox item tests ──

    [Fact]
    public async Task Checkbox_Item_Has_MenuitemCheckbox_Role()
    {
        var helper = CreateItemHelper();
        helper.Type = "checkbox";
        var context = CreateContext("rhx-dropdown-item");
        var output = CreateOutput("rhx-dropdown-item", childContent: "Name");

        await helper.ProcessAsync(context, output);

        AssertAttribute(output, "role", "menuitemcheckbox");
    }

    [Fact]
    public async Task Checkbox_Checked_Has_AriaChecked_True()
    {
        var helper = CreateItemHelper();
        helper.Type = "checkbox";
        helper.Checked = true;
        var context = CreateContext("rhx-dropdown-item");
        var output = CreateOutput("rhx-dropdown-item", childContent: "Name");

        await helper.ProcessAsync(context, output);

        AssertAttribute(output, "aria-checked", "true");
    }

    [Fact]
    public async Task Checkbox_Unchecked_Has_AriaChecked_False()
    {
        var helper = CreateItemHelper();
        helper.Type = "checkbox";
        helper.Checked = false;
        var context = CreateContext("rhx-dropdown-item");
        var output = CreateOutput("rhx-dropdown-item", childContent: "Name");

        await helper.ProcessAsync(context, output);

        AssertAttribute(output, "aria-checked", "false");
    }

    [Fact]
    public async Task Checkbox_Has_Checkbox_Modifier_Class()
    {
        var helper = CreateItemHelper();
        helper.Type = "checkbox";
        var context = CreateContext("rhx-dropdown-item");
        var output = CreateOutput("rhx-dropdown-item", childContent: "Name");

        await helper.ProcessAsync(context, output);

        Assert.True(HasClass(output, "rhx-dropdown__item--checkbox"));
    }

    [Fact]
    public async Task Checkbox_Checked_Has_Checked_Modifier()
    {
        var helper = CreateItemHelper();
        helper.Type = "checkbox";
        helper.Checked = true;
        var context = CreateContext("rhx-dropdown-item");
        var output = CreateOutput("rhx-dropdown-item", childContent: "Name");

        await helper.ProcessAsync(context, output);

        Assert.True(HasClass(output, "rhx-dropdown__item--checked"));
    }

    [Fact]
    public async Task Checkbox_Prepends_Check_Mark_Span()
    {
        var helper = CreateItemHelper();
        helper.Type = "checkbox";
        helper.Checked = true;
        var context = CreateContext("rhx-dropdown-item");
        var output = CreateOutput("rhx-dropdown-item", childContent: "Name");

        await helper.ProcessAsync(context, output);

        var content = output.Content.GetContent();
        Assert.Contains("rhx-dropdown__item-check", content);
        Assert.Contains("aria-hidden=\"true\"", content);
        // Check mark should appear before label
        var checkPos = content.IndexOf("rhx-dropdown__item-check");
        var labelPos = content.IndexOf("rhx-dropdown__item-label");
        Assert.True(checkPos < labelPos);
    }

    [Fact]
    public async Task Unchecked_Checkbox_Has_Empty_Check_Mark()
    {
        var helper = CreateItemHelper();
        helper.Type = "checkbox";
        helper.Checked = false;
        var context = CreateContext("rhx-dropdown-item");
        var output = CreateOutput("rhx-dropdown-item", childContent: "Name");

        await helper.ProcessAsync(context, output);

        var content = output.Content.GetContent();
        Assert.Contains("rhx-dropdown__item-check", content);
        // Should not contain the check mark character
        Assert.DoesNotContain("&#10003;", content);
    }

    // ── htmx on items ──

    [Fact]
    public async Task Item_HxPost_Renders()
    {
        var helper = CreateItemHelper();
        helper.HxPost = "/api/action";
        helper.HxTarget = "#result";
        var context = CreateContext("rhx-dropdown-item");
        var output = CreateOutput("rhx-dropdown-item", childContent: "Run");

        await helper.ProcessAsync(context, output);

        AssertAttribute(output, "hx-post", "/api/action");
        AssertAttribute(output, "hx-target", "#result");
    }

    [Fact]
    public async Task Item_Custom_CssClass_Appended()
    {
        var helper = CreateItemHelper();
        helper.CssClass = "danger-item";
        var context = CreateContext("rhx-dropdown-item");
        var output = CreateOutput("rhx-dropdown-item", childContent: "Delete");

        await helper.ProcessAsync(context, output);

        Assert.True(HasClass(output, "danger-item"));
        Assert.True(HasClass(output, "rhx-dropdown__item"));
    }

    // ══════════════════════════════════════════════
    //  DropdownDividerTagHelper
    // ══════════════════════════════════════════════

    [Fact]
    public void Divider_Renders_Div()
    {
        var helper = new DropdownDividerTagHelper();
        var context = CreateContext("rhx-dropdown-divider");
        var output = CreateOutput("rhx-dropdown-divider");

        helper.Process(context, output);

        Assert.Equal("div", output.TagName);
    }

    [Fact]
    public void Divider_Has_Divider_Class()
    {
        var helper = new DropdownDividerTagHelper();
        var context = CreateContext("rhx-dropdown-divider");
        var output = CreateOutput("rhx-dropdown-divider");

        helper.Process(context, output);

        Assert.True(HasClass(output, "rhx-dropdown__divider"));
    }

    [Fact]
    public void Divider_Has_Separator_Role()
    {
        var helper = new DropdownDividerTagHelper();
        var context = CreateContext("rhx-dropdown-divider");
        var output = CreateOutput("rhx-dropdown-divider");

        helper.Process(context, output);

        AssertAttribute(output, "role", "separator");
    }

    // ══════════════════════════════════════════════
    //  Keyboard Interaction Contract (documentation)
    // ══════════════════════════════════════════════
    //
    // The following keyboard behaviors are implemented in rhx-dropdown.js
    // and tested via browser/integration tests (out of scope for xUnit):
    //
    // Trigger:
    //   Click        → Toggle open/close
    //   ArrowDown    → Open dropdown, focus first item
    //   ArrowUp      → Open dropdown, focus first item
    //   Escape       → Close dropdown (when open)
    //
    // Menu items:
    //   ArrowDown    → Focus next item (wraps to first)
    //   ArrowUp      → Focus previous item (wraps to last)
    //   Home         → Focus first item
    //   End          → Focus last item
    //   Enter/Space  → Activate item (select or toggle checkbox)
    //   Escape       → Close dropdown, return focus to trigger
    //   Tab          → Close dropdown
    //
    // Checkbox items:
    //   Enter/Space  → Toggle aria-checked, update check mark, stay open
    //
    // Other:
    //   Click outside → Close dropdown
    //   Item click    → Activate item (close unless stay-open)
}
