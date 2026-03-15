using htmxRazor.Components.DataDisplay;
using htmxRazor.Rendering;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Xunit;

namespace htmxRazor.Tests;

public class DataTableTagHelperTests : TagHelperTestBase
{
    private DataTableTagHelper CreateHelper()
    {
        var helper = new DataTableTagHelper(CreateUrlHelperFactory());
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
        var context = CreateContext("rhx-data-table");
        var output = CreateOutput("rhx-data-table", childContent: "");

        await helper.ProcessAsync(context, output);

        Assert.Equal("div", output.TagName);
    }

    [Fact]
    public async Task Has_Block_Class()
    {
        var helper = CreateHelper();
        var context = CreateContext("rhx-data-table");
        var output = CreateOutput("rhx-data-table", childContent: "");

        await helper.ProcessAsync(context, output);

        Assert.True(HasClass(output, "rhx-data-table"));
    }

    [Fact]
    public async Task Has_Data_Attribute()
    {
        var helper = CreateHelper();
        var context = CreateContext("rhx-data-table");
        var output = CreateOutput("rhx-data-table", childContent: "");

        await helper.ProcessAsync(context, output);

        AssertAttribute(output, "data-rhx-data-table", "");
    }

    [Fact]
    public async Task Contains_Table_With_Grid_Role()
    {
        var helper = CreateHelper();
        var context = CreateContext("rhx-data-table");
        var output = CreateOutput("rhx-data-table", childContent: "");

        await helper.ProcessAsync(context, output);

        var content = output.Content.GetContent();
        Assert.Contains("role=\"grid\"", content);
        Assert.Contains("rhx-data-table__table", content);
    }

    [Fact]
    public async Task Contains_Scroll_Container()
    {
        var helper = CreateHelper();
        var context = CreateContext("rhx-data-table");
        var output = CreateOutput("rhx-data-table", childContent: "");

        await helper.ProcessAsync(context, output);

        var content = output.Content.GetContent();
        Assert.Contains("rhx-data-table__scroll-container", content);
    }

    [Fact]
    public async Task Contains_Tbody_With_Id()
    {
        var helper = CreateHelper();
        helper.Id = "my-table";
        var context = CreateContext("rhx-data-table");
        var output = CreateOutput("rhx-data-table", childContent: "<tr><td>Row</td></tr>");

        await helper.ProcessAsync(context, output);

        var content = output.Content.GetContent();
        Assert.Contains("id=\"my-table-body\"", content);
        Assert.Contains("rhx-data-table__body", content);
    }

    // ══════════════════════════════════════════════
    //  Caption & Label
    // ══════════════════════════════════════════════

    [Fact]
    public async Task Caption_Rendered()
    {
        var helper = CreateHelper();
        helper.Caption = "Product list";
        var context = CreateContext("rhx-data-table");
        var output = CreateOutput("rhx-data-table", childContent: "");

        await helper.ProcessAsync(context, output);

        var content = output.Content.GetContent();
        Assert.Contains("Product list", content);
        Assert.Contains("rhx-data-table__caption", content);
    }

    [Fact]
    public async Task Label_Sets_AriaLabel()
    {
        var helper = CreateHelper();
        helper.Label = "Products";
        var context = CreateContext("rhx-data-table");
        var output = CreateOutput("rhx-data-table", childContent: "");

        await helper.ProcessAsync(context, output);

        var content = output.Content.GetContent();
        Assert.Contains("aria-label=\"Products\"", content);
    }

    // ══════════════════════════════════════════════
    //  Modifiers
    // ══════════════════════════════════════════════

    [Theory]
    [InlineData("striped")]
    [InlineData("hoverable")]
    [InlineData("bordered")]
    [InlineData("compact")]
    [InlineData("sticky-header")]
    public async Task Modifier_Adds_Class(string modifier)
    {
        var helper = CreateHelper();
        switch (modifier)
        {
            case "striped": helper.Striped = true; break;
            case "hoverable": helper.Hoverable = true; break;
            case "bordered": helper.Bordered = true; break;
            case "compact": helper.Compact = true; break;
            case "sticky-header": helper.StickyHeader = true; break;
        }
        var context = CreateContext("rhx-data-table");
        var output = CreateOutput("rhx-data-table", childContent: "");

        await helper.ProcessAsync(context, output);

        Assert.True(HasClass(output, $"rhx-data-table--{modifier}"));
    }

    // ══════════════════════════════════════════════
    //  Loading state
    // ══════════════════════════════════════════════

    [Fact]
    public async Task Loading_Adds_Class_And_AriaBusy()
    {
        var helper = CreateHelper();
        helper.Loading = true;
        var context = CreateContext("rhx-data-table");
        var output = CreateOutput("rhx-data-table", childContent: "");

        await helper.ProcessAsync(context, output);

        Assert.True(HasClass(output, "rhx-data-table--loading"));
        AssertAttribute(output, "aria-busy", "true");
        var content = output.Content.GetContent();
        Assert.Contains("rhx-data-table__loading-overlay", content);
    }

    // ══════════════════════════════════════════════
    //  Column headers
    // ══════════════════════════════════════════════

    [Fact]
    public async Task Columns_Render_Headers()
    {
        var helper = CreateHelper();
        var context = CreateContext("rhx-data-table");
        // Simulate column registration
        var columns = new List<ColumnDefinition>
        {
            new() { Field = "name", Header = "Name" },
            new() { Field = "email", Header = "Email" }
        };
        context.Items["RhxColumns"] = columns;
        // Bypass child processing with pre-populated columns
        var output = CreateOutput("rhx-data-table", childContent: "");

        await helper.ProcessAsync(context, output);

        var content = output.Content.GetContent();
        Assert.Contains("Name", content);
        Assert.Contains("Email", content);
        Assert.Contains("rhx-data-table__head", content);
        Assert.Contains("scope=\"col\"", content);
    }

    [Fact]
    public async Task Sortable_Column_Renders_Button()
    {
        var helper = CreateHelper();
        helper.SortUrl = "/data";
        helper.Id = "t1";
        var context = CreateContext("rhx-data-table");
        context.Items["RhxColumns"] = new List<ColumnDefinition>
        {
            new() { Field = "name", Header = "Name", Sortable = true }
        };
        var output = CreateOutput("rhx-data-table", childContent: "");

        await helper.ProcessAsync(context, output);

        var content = output.Content.GetContent();
        Assert.Contains("rhx-data-table__sort-button", content);
        Assert.Contains("hx-get=", content);
        Assert.Contains("sort=name", content);
        Assert.Contains("hx-target=\"#t1-body\"", content);
    }

    [Fact]
    public async Task Sortable_Column_Asc_Shows_AriaSortAscending()
    {
        var helper = CreateHelper();
        helper.SortUrl = "/data";
        var context = CreateContext("rhx-data-table");
        context.Items["RhxColumns"] = new List<ColumnDefinition>
        {
            new() { Field = "name", Header = "Name", Sortable = true, SortDirection = "asc" }
        };
        var output = CreateOutput("rhx-data-table", childContent: "");

        await helper.ProcessAsync(context, output);

        var content = output.Content.GetContent();
        Assert.Contains("aria-sort=\"ascending\"", content);
        Assert.Contains("rhx-data-table__header--asc", content);
        // Next click should toggle to desc
        Assert.Contains("dir=desc", content);
    }

    [Fact]
    public async Task NonSortable_Column_Renders_Span()
    {
        var helper = CreateHelper();
        var context = CreateContext("rhx-data-table");
        context.Items["RhxColumns"] = new List<ColumnDefinition>
        {
            new() { Field = "name", Header = "Name", Sortable = false }
        };
        var output = CreateOutput("rhx-data-table", childContent: "");

        await helper.ProcessAsync(context, output);

        var content = output.Content.GetContent();
        Assert.Contains("rhx-data-table__header-text", content);
        Assert.DoesNotContain("rhx-data-table__sort-button", content);
    }

    [Fact]
    public async Task Filterable_Column_Renders_Input()
    {
        var helper = CreateHelper();
        helper.SortUrl = "/data";
        helper.Id = "t1";
        var context = CreateContext("rhx-data-table");
        context.Items["RhxColumns"] = new List<ColumnDefinition>
        {
            new() { Field = "email", Header = "Email", Filterable = true }
        };
        var output = CreateOutput("rhx-data-table", childContent: "");

        await helper.ProcessAsync(context, output);

        var content = output.Content.GetContent();
        Assert.Contains("rhx-data-table__filter-input", content);
        Assert.Contains("name=\"filter_email\"", content);
        Assert.Contains("hx-trigger=\"input changed delay:300ms\"", content);
        Assert.Contains("aria-label=\"Filter Email\"", content);
    }

    // ══════════════════════════════════════════════
    //  Pagination slot
    // ══════════════════════════════════════════════

    [Fact]
    public async Task Pagination_Slot_Rendered()
    {
        var helper = CreateHelper();
        var context = CreateContext("rhx-data-table");

        var output = new TagHelperOutput(
            tagName: "rhx-data-table",
            attributes: new TagHelperAttributeList(),
            getChildContentAsync: (useCachedResult, encoder) =>
            {
                var slots = SlotRenderer.FromContext(context)!;
                slots.SetHtml("pagination", "<div class=\"rhx-data-table__pagination\">Pagination</div>");
                var content = new DefaultTagHelperContent();
                return Task.FromResult<TagHelperContent>(content);
            });

        await helper.ProcessAsync(context, output);

        var html = output.Content.GetContent();
        Assert.Contains("rhx-data-table__pagination", html);
    }

    // ══════════════════════════════════════════════
    //  htmx support
    // ══════════════════════════════════════════════

    [Fact]
    public async Task Renders_Htmx_Attributes()
    {
        var helper = CreateHelper();
        helper.HxGet = "/api/data";
        helper.HxTarget = "#target";
        var context = CreateContext("rhx-data-table");
        var output = CreateOutput("rhx-data-table", childContent: "");

        await helper.ProcessAsync(context, output);

        AssertAttribute(output, "hx-get", "/api/data");
        AssertAttribute(output, "hx-target", "#target");
    }
}
