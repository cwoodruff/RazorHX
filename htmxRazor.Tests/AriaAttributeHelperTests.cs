using Microsoft.AspNetCore.Razor.TagHelpers;
using htmxRazor.Infrastructure;
using Xunit;

namespace htmxRazor.Tests;

public class AriaAttributeHelperTests : TagHelperTestBase
{
    private static TagHelperOutput Output() => CreateOutput();

    // ── Roles ──

    [Fact]
    public void RoleButton_Sets_Role_Button()
    {
        var output = Output();
        AriaAttributeHelper.RoleButton(output);
        AssertAttribute(output, "role", "button");
    }

    [Fact]
    public void RoleDialog_Sets_Role_Dialog()
    {
        var output = Output();
        AriaAttributeHelper.RoleDialog(output);
        AssertAttribute(output, "role", "dialog");
    }

    [Fact]
    public void RoleAlertDialog_Sets_Role_AlertDialog()
    {
        var output = Output();
        AriaAttributeHelper.RoleAlertDialog(output);
        AssertAttribute(output, "role", "alertdialog");
    }

    [Fact]
    public void RoleTab_Sets_Role_Tab()
    {
        var output = Output();
        AriaAttributeHelper.RoleTab(output);
        AssertAttribute(output, "role", "tab");
    }

    [Fact]
    public void RoleTabList_Sets_Role_TabList()
    {
        var output = Output();
        AriaAttributeHelper.RoleTabList(output);
        AssertAttribute(output, "role", "tablist");
    }

    [Fact]
    public void RoleMenu_Sets_Role_Menu()
    {
        var output = Output();
        AriaAttributeHelper.RoleMenu(output);
        AssertAttribute(output, "role", "menu");
    }

    [Fact]
    public void RoleNavigation_Sets_Role_Navigation()
    {
        var output = Output();
        AriaAttributeHelper.RoleNavigation(output);
        AssertAttribute(output, "role", "navigation");
    }

    [Fact]
    public void RoleAlert_Sets_Role_Alert()
    {
        var output = Output();
        AriaAttributeHelper.RoleAlert(output);
        AssertAttribute(output, "role", "alert");
    }

    [Fact]
    public void RoleGroup_Sets_Role_Group()
    {
        var output = Output();
        AriaAttributeHelper.RoleGroup(output);
        AssertAttribute(output, "role", "group");
    }

    [Fact]
    public void SetRole_Sets_Custom_Role()
    {
        var output = Output();
        AriaAttributeHelper.SetRole(output, "presentation");
        AssertAttribute(output, "role", "presentation");
    }

    // ── Labels ──

    [Fact]
    public void AriaLabel_Sets_Attribute()
    {
        var output = Output();
        AriaAttributeHelper.AriaLabel(output, "Close");
        AssertAttribute(output, "aria-label", "Close");
    }

    [Fact]
    public void AriaLabelledBy_Sets_Attribute()
    {
        var output = Output();
        AriaAttributeHelper.AriaLabelledBy(output, "title-id");
        AssertAttribute(output, "aria-labelledby", "title-id");
    }

    [Fact]
    public void AriaDescribedBy_Sets_Attribute()
    {
        var output = Output();
        AriaAttributeHelper.AriaDescribedBy(output, "desc-id");
        AssertAttribute(output, "aria-describedby", "desc-id");
    }

    // ── State ──

    [Fact]
    public void AriaExpanded_True_Sets_True()
    {
        var output = Output();
        AriaAttributeHelper.AriaExpanded(output, true);
        AssertAttribute(output, "aria-expanded", "true");
    }

    [Fact]
    public void AriaExpanded_False_Sets_False()
    {
        var output = Output();
        AriaAttributeHelper.AriaExpanded(output, false);
        AssertAttribute(output, "aria-expanded", "false");
    }

    [Fact]
    public void AriaSelected_Sets_Attribute()
    {
        var output = Output();
        AriaAttributeHelper.AriaSelected(output, true);
        AssertAttribute(output, "aria-selected", "true");
    }

    [Fact]
    public void AriaDisabled_Sets_Attribute()
    {
        var output = Output();
        AriaAttributeHelper.AriaDisabled(output, true);
        AssertAttribute(output, "aria-disabled", "true");
    }

    [Fact]
    public void AriaHidden_Sets_Attribute()
    {
        var output = Output();
        AriaAttributeHelper.AriaHidden(output, true);
        AssertAttribute(output, "aria-hidden", "true");
    }

    [Fact]
    public void AriaChecked_Sets_Attribute()
    {
        var output = Output();
        AriaAttributeHelper.AriaChecked(output, true);
        AssertAttribute(output, "aria-checked", "true");
    }

    [Fact]
    public void AriaPressed_Sets_Attribute()
    {
        var output = Output();
        AriaAttributeHelper.AriaPressed(output, false);
        AssertAttribute(output, "aria-pressed", "false");
    }

    [Fact]
    public void AriaRequired_Sets_Attribute()
    {
        var output = Output();
        AriaAttributeHelper.AriaRequired(output, true);
        AssertAttribute(output, "aria-required", "true");
    }

    [Fact]
    public void AriaInvalid_Sets_Attribute()
    {
        var output = Output();
        AriaAttributeHelper.AriaInvalid(output, true);
        AssertAttribute(output, "aria-invalid", "true");
    }

    // ── Relationships ──

    [Fact]
    public void AriaCurrent_Default_Sets_Page()
    {
        var output = Output();
        AriaAttributeHelper.AriaCurrent(output);
        AssertAttribute(output, "aria-current", "page");
    }

    [Fact]
    public void AriaCurrent_Custom_Sets_Value()
    {
        var output = Output();
        AriaAttributeHelper.AriaCurrent(output, "step");
        AssertAttribute(output, "aria-current", "step");
    }

    [Fact]
    public void AriaLive_Default_Sets_Polite()
    {
        var output = Output();
        AriaAttributeHelper.AriaLive(output);
        AssertAttribute(output, "aria-live", "polite");
    }

    [Fact]
    public void AriaLive_Assertive_Sets_Assertive()
    {
        var output = Output();
        AriaAttributeHelper.AriaLive(output, "assertive");
        AssertAttribute(output, "aria-live", "assertive");
    }

    [Fact]
    public void AriaControls_Sets_Attribute()
    {
        var output = Output();
        AriaAttributeHelper.AriaControls(output, "panel-1");
        AssertAttribute(output, "aria-controls", "panel-1");
    }

    [Fact]
    public void AriaHasPopup_Default_Sets_True()
    {
        var output = Output();
        AriaAttributeHelper.AriaHasPopup(output);
        AssertAttribute(output, "aria-haspopup", "true");
    }

    [Fact]
    public void AriaHasPopup_Menu_Sets_Menu()
    {
        var output = Output();
        AriaAttributeHelper.AriaHasPopup(output, "menu");
        AssertAttribute(output, "aria-haspopup", "menu");
    }

    // ── Values ──

    [Fact]
    public void AriaValueNow_Sets_Numeric()
    {
        var output = Output();
        AriaAttributeHelper.AriaValueNow(output, 42);
        AssertAttribute(output, "aria-valuenow", "42");
    }

    [Fact]
    public void AriaValueMin_Sets_Numeric()
    {
        var output = Output();
        AriaAttributeHelper.AriaValueMin(output, 0);
        AssertAttribute(output, "aria-valuemin", "0");
    }

    [Fact]
    public void AriaValueMax_Sets_Numeric()
    {
        var output = Output();
        AriaAttributeHelper.AriaValueMax(output, 100);
        AssertAttribute(output, "aria-valuemax", "100");
    }

    // ── Batch ──

    [Fact]
    public void SetAttributes_Sets_Multiple_With_AutoPrefix()
    {
        var output = Output();
        AriaAttributeHelper.SetAttributes(output, new Dictionary<string, string>
        {
            ["label"] = "My Label",
            ["aria-expanded"] = "true"
        });

        AssertAttribute(output, "aria-label", "My Label");
        AssertAttribute(output, "aria-expanded", "true");
    }
}
