using htmxRazor.Components.Navigation;
using Xunit;

namespace htmxRazor.Tests;

/// <summary>
/// Tests for the PaginationTagHelper component.
/// </summary>
public class PaginationTagHelperTests : TagHelperTestBase
{
    private PaginationTagHelper CreateHelper()
    {
        var helper = new PaginationTagHelper(CreateUrlHelperFactory());
        helper.ViewContext = CreateViewContext();
        return helper;
    }

    // ──────────────────────────────────────────────
    //  Basic rendering
    // ──────────────────────────────────────────────

    [Fact]
    public async Task Renders_Nav_Element()
    {
        var helper = CreateHelper();
        helper.TotalPages = 5;
        var context = CreateContext("rhx-pagination");
        var output = CreateOutput("rhx-pagination");

        await helper.ProcessAsync(context, output);

        Assert.Equal("nav", output.TagName);
    }

    [Fact]
    public async Task Has_AriaLabel_Pagination()
    {
        var helper = CreateHelper();
        helper.TotalPages = 5;
        var context = CreateContext("rhx-pagination");
        var output = CreateOutput("rhx-pagination");

        await helper.ProcessAsync(context, output);

        AssertAttribute(output, "aria-label", "Pagination");
    }

    [Fact]
    public async Task Has_Block_Class()
    {
        var helper = CreateHelper();
        helper.TotalPages = 5;
        var context = CreateContext("rhx-pagination");
        var output = CreateOutput("rhx-pagination");

        await helper.ProcessAsync(context, output);

        Assert.True(HasClass(output, "rhx-pagination"));
    }

    // ──────────────────────────────────────────────
    //  Suppression
    // ──────────────────────────────────────────────

    [Fact]
    public async Task Suppresses_Output_When_Single_Page()
    {
        var helper = CreateHelper();
        helper.TotalPages = 1;
        var context = CreateContext("rhx-pagination");
        var output = CreateOutput("rhx-pagination");

        await helper.ProcessAsync(context, output);

        Assert.True(output.IsContentModified == false || output.Content.IsEmptyOrWhiteSpace);
    }

    [Fact]
    public async Task Suppresses_Output_When_Zero_Pages()
    {
        var helper = CreateHelper();
        helper.TotalPages = 0;
        var context = CreateContext("rhx-pagination");
        var output = CreateOutput("rhx-pagination");

        await helper.ProcessAsync(context, output);

        Assert.True(output.IsContentModified == false || output.Content.IsEmptyOrWhiteSpace);
    }

    // ──────────────────────────────────────────────
    //  Total items calculation
    // ──────────────────────────────────────────────

    [Fact]
    public async Task Calculates_TotalPages_From_TotalItems()
    {
        var helper = CreateHelper();
        helper.TotalItems = 95;
        helper.PageSize = 10;
        helper.CurrentPage = 1;
        var context = CreateContext("rhx-pagination");
        var output = CreateOutput("rhx-pagination");

        await helper.ProcessAsync(context, output);

        // 95 / 10 = 10 pages (ceil)
        Assert.Equal("nav", output.TagName);
        var content = output.Content.GetContent();
        Assert.Contains("10", content);
    }

    // ──────────────────────────────────────────────
    //  Current page
    // ──────────────────────────────────────────────

    [Fact]
    public async Task Marks_Current_Page_With_AriaCurrent()
    {
        var helper = CreateHelper();
        helper.TotalPages = 5;
        helper.CurrentPage = 3;
        var context = CreateContext("rhx-pagination");
        var output = CreateOutput("rhx-pagination");

        await helper.ProcessAsync(context, output);

        var content = output.Content.GetContent();
        Assert.Contains("aria-current=\"page\"", content);
        Assert.Contains(">3<", content);
    }

    [Fact]
    public async Task Current_Page_Button_Is_Disabled()
    {
        var helper = CreateHelper();
        helper.TotalPages = 5;
        helper.CurrentPage = 2;
        var context = CreateContext("rhx-pagination");
        var output = CreateOutput("rhx-pagination");

        await helper.ProcessAsync(context, output);

        var content = output.Content.GetContent();
        Assert.Contains("aria-current=\"page\"", content);
        // Current button should be disabled
        Assert.Contains("disabled>2<", content);
    }

    // ──────────────────────────────────────────────
    //  Navigation buttons
    // ──────────────────────────────────────────────

    [Fact]
    public async Task Prev_Disabled_On_First_Page()
    {
        var helper = CreateHelper();
        helper.TotalPages = 5;
        helper.CurrentPage = 1;
        var context = CreateContext("rhx-pagination");
        var output = CreateOutput("rhx-pagination");

        await helper.ProcessAsync(context, output);

        var content = output.Content.GetContent();
        Assert.Contains("aria-label=\"Previous page\" disabled>", content);
    }

    [Fact]
    public async Task Next_Disabled_On_Last_Page()
    {
        var helper = CreateHelper();
        helper.TotalPages = 5;
        helper.CurrentPage = 5;
        var context = CreateContext("rhx-pagination");
        var output = CreateOutput("rhx-pagination");

        await helper.ProcessAsync(context, output);

        var content = output.Content.GetContent();
        Assert.Contains("aria-label=\"Next page\" disabled>", content);
    }

    // ──────────────────────────────────────────────
    //  htmx integration
    // ──────────────────────────────────────────────

    [Fact]
    public async Task Generates_HxGet_With_Page_Param()
    {
        var helper = CreateHelper();
        helper.TotalPages = 5;
        helper.CurrentPage = 1;
        helper.HxGet = "/items";
        helper.HxTarget = "#results";
        var context = CreateContext("rhx-pagination");
        var output = CreateOutput("rhx-pagination");

        await helper.ProcessAsync(context, output);

        var content = output.Content.GetContent();
        Assert.Contains("hx-get=\"/items?p=2\"", content);
        Assert.Contains("hx-target=\"#results\"", content);
    }

    [Fact]
    public async Task Custom_PageParam()
    {
        var helper = CreateHelper();
        helper.TotalPages = 5;
        helper.CurrentPage = 1;
        helper.HxGet = "/items";
        helper.PageParam = "pg";
        var context = CreateContext("rhx-pagination");
        var output = CreateOutput("rhx-pagination");

        await helper.ProcessAsync(context, output);

        var content = output.Content.GetContent();
        Assert.Contains("hx-get=\"/items?pg=2\"", content);
    }

    // ──────────────────────────────────────────────
    //  Ellipsis
    // ──────────────────────────────────────────────

    [Fact]
    public async Task Shows_Ellipsis_For_Many_Pages()
    {
        var helper = CreateHelper();
        helper.TotalPages = 20;
        helper.CurrentPage = 10;
        helper.MaxVisible = 7;
        var context = CreateContext("rhx-pagination");
        var output = CreateOutput("rhx-pagination");

        await helper.ProcessAsync(context, output);

        var content = output.Content.GetContent();
        Assert.Contains("&hellip;", content);
    }

    [Fact]
    public async Task No_Ellipsis_When_Few_Pages()
    {
        var helper = CreateHelper();
        helper.TotalPages = 5;
        helper.CurrentPage = 3;
        helper.MaxVisible = 7;
        var context = CreateContext("rhx-pagination");
        var output = CreateOutput("rhx-pagination");

        await helper.ProcessAsync(context, output);

        var content = output.Content.GetContent();
        Assert.DoesNotContain("&hellip;", content);
    }

    // ──────────────────────────────────────────────
    //  Info text
    // ──────────────────────────────────────────────

    [Fact]
    public async Task Shows_Info_When_Enabled()
    {
        var helper = CreateHelper();
        helper.TotalPages = 10;
        helper.CurrentPage = 3;
        helper.ShowInfo = true;
        var context = CreateContext("rhx-pagination");
        var output = CreateOutput("rhx-pagination");

        await helper.ProcessAsync(context, output);

        var content = output.Content.GetContent();
        Assert.Contains("Page 3 of 10", content);
    }

    [Fact]
    public async Task No_Info_By_Default()
    {
        var helper = CreateHelper();
        helper.TotalPages = 10;
        helper.CurrentPage = 3;
        var context = CreateContext("rhx-pagination");
        var output = CreateOutput("rhx-pagination");

        await helper.ProcessAsync(context, output);

        var content = output.Content.GetContent();
        Assert.DoesNotContain("Page 3 of 10", content);
    }

    // ──────────────────────────────────────────────
    //  Size variant
    // ──────────────────────────────────────────────

    [Fact]
    public async Task Adds_Size_Modifier_Class()
    {
        var helper = CreateHelper();
        helper.TotalPages = 5;
        helper.Size = "small";
        var context = CreateContext("rhx-pagination");
        var output = CreateOutput("rhx-pagination");

        await helper.ProcessAsync(context, output);

        Assert.True(HasClass(output, "rhx-pagination--small"));
    }

    // ──────────────────────────────────────────────
    //  Edge buttons
    // ──────────────────────────────────────────────

    [Fact]
    public async Task Shows_First_Last_By_Default()
    {
        var helper = CreateHelper();
        helper.TotalPages = 5;
        helper.CurrentPage = 3;
        var context = CreateContext("rhx-pagination");
        var output = CreateOutput("rhx-pagination");

        await helper.ProcessAsync(context, output);

        var content = output.Content.GetContent();
        Assert.Contains("aria-label=\"First page\"", content);
        Assert.Contains("aria-label=\"Last page\"", content);
    }

    [Fact]
    public async Task Hides_First_Last_When_ShowEdges_False()
    {
        var helper = CreateHelper();
        helper.TotalPages = 5;
        helper.CurrentPage = 3;
        helper.ShowEdges = false;
        var context = CreateContext("rhx-pagination");
        var output = CreateOutput("rhx-pagination");

        await helper.ProcessAsync(context, output);

        var content = output.Content.GetContent();
        Assert.DoesNotContain("aria-label=\"First page\"", content);
        Assert.DoesNotContain("aria-label=\"Last page\"", content);
    }
}
