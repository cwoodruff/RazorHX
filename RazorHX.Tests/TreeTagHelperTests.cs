using RazorHX.Components.Navigation;
using Xunit;

namespace RazorHX.Tests;

public class TreeTagHelperTests : TagHelperTestBase
{
    // ──────────────────────────────────────────────
    //  Helpers
    // ──────────────────────────────────────────────

    private TreeTagHelper CreateTreeHelper()
    {
        var helper = new TreeTagHelper(CreateUrlHelperFactory());
        helper.ViewContext = CreateViewContext();
        return helper;
    }

    private TreeItemTagHelper CreateItemHelper()
    {
        var helper = new TreeItemTagHelper(CreateUrlHelperFactory());
        helper.ViewContext = CreateViewContext();
        return helper;
    }

    // ══════════════════════════════════════════════
    //  TreeTagHelper
    // ══════════════════════════════════════════════

    [Fact]
    public void Tree_Renders_Div()
    {
        var helper = CreateTreeHelper();
        var context = CreateContext("rhx-tree");
        var output = CreateOutput("rhx-tree");

        helper.Process(context, output);

        Assert.Equal("div", output.TagName);
    }

    [Fact]
    public void Tree_Has_Block_Class()
    {
        var helper = CreateTreeHelper();
        var context = CreateContext("rhx-tree");
        var output = CreateOutput("rhx-tree");

        helper.Process(context, output);

        Assert.True(HasClass(output, "rhx-tree"));
    }

    [Fact]
    public void Tree_Has_Role_Tree()
    {
        var helper = CreateTreeHelper();
        var context = CreateContext("rhx-tree");
        var output = CreateOutput("rhx-tree");

        helper.Process(context, output);

        AssertAttribute(output, "role", "tree");
    }

    [Fact]
    public void Tree_Has_Data_Attribute()
    {
        var helper = CreateTreeHelper();
        var context = CreateContext("rhx-tree");
        var output = CreateOutput("rhx-tree");

        helper.Process(context, output);

        Assert.NotNull(output.Attributes["data-rhx-tree"]);
    }

    [Fact]
    public void Tree_Default_Selection_Single()
    {
        var helper = CreateTreeHelper();
        var context = CreateContext("rhx-tree");
        var output = CreateOutput("rhx-tree");

        helper.Process(context, output);

        AssertAttribute(output, "data-rhx-selection", "single");
    }

    [Fact]
    public void Tree_Multiple_Selection()
    {
        var helper = CreateTreeHelper();
        helper.Selection = "multiple";
        var context = CreateContext("rhx-tree");
        var output = CreateOutput("rhx-tree");

        helper.Process(context, output);

        AssertAttribute(output, "data-rhx-selection", "multiple");
    }

    [Fact]
    public void Tree_Leaf_Selection()
    {
        var helper = CreateTreeHelper();
        helper.Selection = "leaf";
        var context = CreateContext("rhx-tree");
        var output = CreateOutput("rhx-tree");

        helper.Process(context, output);

        AssertAttribute(output, "data-rhx-selection", "leaf");
    }

    [Fact]
    public void Tree_AriaLabel()
    {
        var helper = CreateTreeHelper();
        helper.AriaLabel = "File explorer";
        var context = CreateContext("rhx-tree");
        var output = CreateOutput("rhx-tree");

        helper.Process(context, output);

        AssertAttribute(output, "aria-label", "File explorer");
    }

    [Fact]
    public void Tree_No_AriaLabel_When_Not_Set()
    {
        var helper = CreateTreeHelper();
        var context = CreateContext("rhx-tree");
        var output = CreateOutput("rhx-tree");

        helper.Process(context, output);

        AssertNoAttribute(output, "aria-label");
    }

    [Fact]
    public void Tree_Custom_CssClass()
    {
        var helper = CreateTreeHelper();
        helper.CssClass = "my-tree";
        var context = CreateContext("rhx-tree");
        var output = CreateOutput("rhx-tree");

        helper.Process(context, output);

        Assert.True(HasClass(output, "my-tree"));
        Assert.True(HasClass(output, "rhx-tree"));
    }

    // ══════════════════════════════════════════════
    //  TreeItemTagHelper — Structure
    // ══════════════════════════════════════════════

    [Fact]
    public async Task Item_Renders_Div()
    {
        var helper = CreateItemHelper();
        helper.Label = "Documents";
        var context = CreateContext("rhx-tree-item");
        var output = CreateOutput("rhx-tree-item", childContent: "");

        await helper.ProcessAsync(context, output);

        Assert.Equal("div", output.TagName);
    }

    [Fact]
    public async Task Item_Has_Item_Class()
    {
        var helper = CreateItemHelper();
        helper.Label = "Documents";
        var context = CreateContext("rhx-tree-item");
        var output = CreateOutput("rhx-tree-item", childContent: "");

        await helper.ProcessAsync(context, output);

        Assert.True(HasClass(output, "rhx-tree__item"));
    }

    [Fact]
    public async Task Item_Has_Role_Treeitem()
    {
        var helper = CreateItemHelper();
        helper.Label = "Documents";
        var context = CreateContext("rhx-tree-item");
        var output = CreateOutput("rhx-tree-item", childContent: "");

        await helper.ProcessAsync(context, output);

        AssertAttribute(output, "role", "treeitem");
    }

    [Fact]
    public async Task Item_Has_Tabindex_Minus_One()
    {
        var helper = CreateItemHelper();
        helper.Label = "Documents";
        var context = CreateContext("rhx-tree-item");
        var output = CreateOutput("rhx-tree-item", childContent: "");

        await helper.ProcessAsync(context, output);

        AssertAttribute(output, "tabindex", "-1");
    }

    // ══════════════════════════════════════════════
    //  Label resolution
    // ══════════════════════════════════════════════

    [Fact]
    public async Task Label_From_Property()
    {
        var helper = CreateItemHelper();
        helper.Label = "Documents";
        var context = CreateContext("rhx-tree-item");
        var output = CreateOutput("rhx-tree-item", childContent: "");

        await helper.ProcessAsync(context, output);

        var content = output.Content.GetContent();
        Assert.Contains("rhx-tree__item-label", content);
        Assert.Contains("Documents", content);
    }

    [Fact]
    public async Task Label_From_Child_Content()
    {
        var helper = CreateItemHelper();
        var context = CreateContext("rhx-tree-item");
        var output = CreateOutput("rhx-tree-item", childContent: "readme.txt");

        await helper.ProcessAsync(context, output);

        var content = output.Content.GetContent();
        Assert.Contains("rhx-tree__item-label", content);
        Assert.Contains("readme.txt", content);
    }

    [Fact]
    public async Task Label_Property_Is_HtmlEncoded()
    {
        var helper = CreateItemHelper();
        helper.Label = "Tom & Jerry";
        var context = CreateContext("rhx-tree-item");
        var output = CreateOutput("rhx-tree-item", childContent: "");

        await helper.ProcessAsync(context, output);

        var content = output.Content.GetContent();
        Assert.Contains("Tom &amp; Jerry", content);
    }

    // ══════════════════════════════════════════════
    //  Leaf vs Branch
    // ══════════════════════════════════════════════

    [Fact]
    public async Task Leaf_Item_No_Expand_Icon()
    {
        var helper = CreateItemHelper();
        var context = CreateContext("rhx-tree-item");
        var output = CreateOutput("rhx-tree-item", childContent: "file.txt");

        await helper.ProcessAsync(context, output);

        var content = output.Content.GetContent();
        Assert.DoesNotContain("rhx-tree__expand-icon", content);
        Assert.True(HasClass(output, "rhx-tree__item--leaf"));
    }

    [Fact]
    public async Task Leaf_No_AriaExpanded()
    {
        var helper = CreateItemHelper();
        var context = CreateContext("rhx-tree-item");
        var output = CreateOutput("rhx-tree-item", childContent: "file.txt");

        await helper.ProcessAsync(context, output);

        AssertNoAttribute(output, "aria-expanded");
    }

    [Fact]
    public async Task Leaf_No_Children_Div()
    {
        var helper = CreateItemHelper();
        var context = CreateContext("rhx-tree-item");
        var output = CreateOutput("rhx-tree-item", childContent: "file.txt");

        await helper.ProcessAsync(context, output);

        var content = output.Content.GetContent();
        Assert.DoesNotContain("rhx-tree__children", content);
    }

    [Fact]
    public async Task Branch_Has_Expand_Icon()
    {
        var helper = CreateItemHelper();
        helper.Label = "Documents";
        var context = CreateContext("rhx-tree-item");
        var output = CreateOutput("rhx-tree-item", childContent: "<div>nested child</div>");

        await helper.ProcessAsync(context, output);

        var content = output.Content.GetContent();
        Assert.Contains("rhx-tree__expand-icon", content);
        Assert.Contains("aria-hidden=\"true\"", content);
    }

    [Fact]
    public async Task Branch_Has_Children_Div_With_Role_Group()
    {
        var helper = CreateItemHelper();
        helper.Label = "Documents";
        var context = CreateContext("rhx-tree-item");
        var output = CreateOutput("rhx-tree-item", childContent: "<div>child</div>");

        await helper.ProcessAsync(context, output);

        var content = output.Content.GetContent();
        Assert.Contains("class=\"rhx-tree__children\"", content);
        Assert.Contains("role=\"group\"", content);
    }

    [Fact]
    public async Task Branch_Not_Leaf()
    {
        var helper = CreateItemHelper();
        helper.Label = "Documents";
        var context = CreateContext("rhx-tree-item");
        var output = CreateOutput("rhx-tree-item", childContent: "<div>child</div>");

        await helper.ProcessAsync(context, output);

        Assert.False(HasClass(output, "rhx-tree__item--leaf"));
    }

    // ══════════════════════════════════════════════
    //  Expanded / Collapsed
    // ══════════════════════════════════════════════

    [Fact]
    public async Task Expanded_Has_AriaExpanded_True()
    {
        var helper = CreateItemHelper();
        helper.Label = "Documents";
        helper.Expanded = true;
        var context = CreateContext("rhx-tree-item");
        var output = CreateOutput("rhx-tree-item", childContent: "<div>child</div>");

        await helper.ProcessAsync(context, output);

        AssertAttribute(output, "aria-expanded", "true");
        Assert.True(HasClass(output, "rhx-tree__item--expanded"));
    }

    [Fact]
    public async Task Collapsed_Has_AriaExpanded_False()
    {
        var helper = CreateItemHelper();
        helper.Label = "Documents";
        var context = CreateContext("rhx-tree-item");
        var output = CreateOutput("rhx-tree-item", childContent: "<div>child</div>");

        await helper.ProcessAsync(context, output);

        AssertAttribute(output, "aria-expanded", "false");
        Assert.False(HasClass(output, "rhx-tree__item--expanded"));
    }

    [Fact]
    public async Task Collapsed_Children_Hidden()
    {
        var helper = CreateItemHelper();
        helper.Label = "Documents";
        var context = CreateContext("rhx-tree-item");
        var output = CreateOutput("rhx-tree-item", childContent: "<div>child</div>");

        await helper.ProcessAsync(context, output);

        var content = output.Content.GetContent();
        Assert.Contains("hidden", content);
    }

    [Fact]
    public async Task Expanded_Children_Not_Hidden()
    {
        var helper = CreateItemHelper();
        helper.Label = "Documents";
        helper.Expanded = true;
        var context = CreateContext("rhx-tree-item");
        var output = CreateOutput("rhx-tree-item", childContent: "<div>child</div>");

        await helper.ProcessAsync(context, output);

        var content = output.Content.GetContent();
        // Children div should NOT have hidden attribute
        Assert.Contains("role=\"group\">", content);
    }

    // ══════════════════════════════════════════════
    //  Selected / Disabled
    // ══════════════════════════════════════════════

    [Fact]
    public async Task Selected_Has_AriaSelected()
    {
        var helper = CreateItemHelper();
        helper.Selected = true;
        var context = CreateContext("rhx-tree-item");
        var output = CreateOutput("rhx-tree-item", childContent: "item");

        await helper.ProcessAsync(context, output);

        AssertAttribute(output, "aria-selected", "true");
        Assert.True(HasClass(output, "rhx-tree__item--selected"));
    }

    [Fact]
    public async Task Not_Selected_No_AriaSelected()
    {
        var helper = CreateItemHelper();
        var context = CreateContext("rhx-tree-item");
        var output = CreateOutput("rhx-tree-item", childContent: "item");

        await helper.ProcessAsync(context, output);

        AssertNoAttribute(output, "aria-selected");
        Assert.False(HasClass(output, "rhx-tree__item--selected"));
    }

    [Fact]
    public async Task Disabled_Has_AriaDisabled()
    {
        var helper = CreateItemHelper();
        helper.Disabled = true;
        var context = CreateContext("rhx-tree-item");
        var output = CreateOutput("rhx-tree-item", childContent: "item");

        await helper.ProcessAsync(context, output);

        AssertAttribute(output, "aria-disabled", "true");
        Assert.True(HasClass(output, "rhx-tree__item--disabled"));
    }

    // ══════════════════════════════════════════════
    //  Lazy loading
    // ══════════════════════════════════════════════

    [Fact]
    public async Task Lazy_Has_Data_Attribute()
    {
        var helper = CreateItemHelper();
        helper.Lazy = true;
        var context = CreateContext("rhx-tree-item");
        var output = CreateOutput("rhx-tree-item", childContent: "Projects");

        await helper.ProcessAsync(context, output);

        Assert.NotNull(output.Attributes["data-rhx-tree-lazy"]);
        Assert.True(HasClass(output, "rhx-tree__item--lazy"));
    }

    [Fact]
    public async Task Lazy_Has_Expand_Icon_Even_Without_Children()
    {
        var helper = CreateItemHelper();
        helper.Lazy = true;
        var context = CreateContext("rhx-tree-item");
        var output = CreateOutput("rhx-tree-item", childContent: "Projects");

        await helper.ProcessAsync(context, output);

        var content = output.Content.GetContent();
        Assert.Contains("rhx-tree__expand-icon", content);
        Assert.False(HasClass(output, "rhx-tree__item--leaf"));
    }

    [Fact]
    public async Task Lazy_Has_Children_Div()
    {
        var helper = CreateItemHelper();
        helper.Lazy = true;
        var context = CreateContext("rhx-tree-item");
        var output = CreateOutput("rhx-tree-item", childContent: "Projects");

        await helper.ProcessAsync(context, output);

        var content = output.Content.GetContent();
        Assert.Contains("rhx-tree__children", content);
        Assert.Contains("role=\"group\"", content);
    }

    [Fact]
    public async Task Lazy_Has_AriaExpanded()
    {
        var helper = CreateItemHelper();
        helper.Lazy = true;
        var context = CreateContext("rhx-tree-item");
        var output = CreateOutput("rhx-tree-item", childContent: "Projects");

        await helper.ProcessAsync(context, output);

        AssertAttribute(output, "aria-expanded", "false");
    }

    // ══════════════════════════════════════════════
    //  htmx & Custom CSS
    // ══════════════════════════════════════════════

    [Fact]
    public async Task Htmx_Attributes_Rendered()
    {
        var helper = CreateItemHelper();
        helper.Lazy = true;
        helper.HxGet = "/api/children/1";
        helper.HxTarget = "find .rhx-tree__children";
        helper.HxSwap = "innerHTML";
        helper.HxTrigger = "toggle once";
        var context = CreateContext("rhx-tree-item");
        var output = CreateOutput("rhx-tree-item", childContent: "Projects");

        await helper.ProcessAsync(context, output);

        AssertAttribute(output, "hx-get", "/api/children/1");
        AssertAttribute(output, "hx-target", "find .rhx-tree__children");
        AssertAttribute(output, "hx-swap", "innerHTML");
        AssertAttribute(output, "hx-trigger", "toggle once");
    }

    [Fact]
    public async Task Custom_CssClass()
    {
        var helper = CreateItemHelper();
        helper.CssClass = "my-item";
        var context = CreateContext("rhx-tree-item");
        var output = CreateOutput("rhx-tree-item", childContent: "item");

        await helper.ProcessAsync(context, output);

        Assert.True(HasClass(output, "my-item"));
        Assert.True(HasClass(output, "rhx-tree__item"));
    }

    [Fact]
    public async Task Item_Content_Div_Present()
    {
        var helper = CreateItemHelper();
        var context = CreateContext("rhx-tree-item");
        var output = CreateOutput("rhx-tree-item", childContent: "file.txt");

        await helper.ProcessAsync(context, output);

        var content = output.Content.GetContent();
        Assert.Contains("class=\"rhx-tree__item-content\"", content);
    }

    // ══════════════════════════════════════════════
    //  Keyboard Interaction Contract (documentation)
    // ══════════════════════════════════════════════
    //
    // The following keyboard behaviors are implemented in rhx-tree.js:
    //
    //   ArrowDown    → Focus next visible item
    //   ArrowUp      → Focus previous visible item
    //   ArrowRight   → Expand collapsed, or focus first child
    //   ArrowLeft    → Collapse expanded, or focus parent
    //   Home         → Focus first visible item
    //   End          → Focus last visible item
    //   Enter/Space  → Select item, toggle expand
    //
    // Selection modes:
    //   single   → One item at a time
    //   multiple → Toggle selection
    //   leaf     → Only leaf items selectable
    //
    // Lazy loading:
    //   Expanding a lazy item dispatches a 'toggle' event
    //   for htmx to intercept via hx-trigger="toggle once"
}
