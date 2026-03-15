using htmxRazor.Components.DataDisplay;
using htmxRazor.Rendering;
using Microsoft.AspNetCore.Html;
using Xunit;

namespace htmxRazor.Tests;

public class DataTablePaginationTagHelperTests : TagHelperTestBase
{
    private static string RenderPagination(int page, int pageSize, int totalItems, string url = "/data?handler=Table", string? target = null)
    {
        var helper = new DataTablePaginationTagHelper
        {
            Page = page,
            PageSize = pageSize,
            TotalItems = totalItems,
            Url = url,
            Target = target
        };
        var context = CreateContext("rhx-data-table-pagination");
        var slots = SlotRenderer.CreateForContext(context);
        var output = CreateOutput("rhx-data-table-pagination");

        helper.Process(context, output);

        if (!slots.Has("pagination")) return "";
        using var writer = new System.IO.StringWriter();
        slots.Get("pagination")!.WriteTo(writer, System.Text.Encodings.Web.HtmlEncoder.Default);
        return writer.ToString();
    }

    [Fact]
    public void Renders_Page_Info()
    {
        var html = RenderPagination(1, 10, 142);

        Assert.Contains("Showing 1\u201310 of 142", html);
    }

    [Fact]
    public void Second_Page_Shows_Correct_Range()
    {
        var html = RenderPagination(2, 10, 142);

        Assert.Contains("Showing 11\u201320 of 142", html);
    }

    [Fact]
    public void Last_Page_Shows_Correct_Range()
    {
        var html = RenderPagination(15, 10, 142);

        Assert.Contains("Showing 141\u2013142 of 142", html);
    }

    [Fact]
    public void Zero_Items_Shows_No_Items()
    {
        var html = RenderPagination(1, 10, 0);

        Assert.Contains("No items", html);
    }

    [Fact]
    public void First_Page_Disables_Prev_Buttons()
    {
        var html = RenderPagination(1, 10, 50);

        // First and Previous buttons should be disabled
        Assert.Contains("aria-label=\"First page\" disabled", html);
        Assert.Contains("aria-label=\"Previous page\" disabled", html);
        // Next and Last should NOT be disabled
        Assert.DoesNotContain("aria-label=\"Next page\" disabled", html);
    }

    [Fact]
    public void Last_Page_Disables_Next_Buttons()
    {
        var html = RenderPagination(5, 10, 50);

        Assert.Contains("aria-label=\"Last page\" disabled", html);
        Assert.Contains("aria-label=\"Next page\" disabled", html);
        Assert.DoesNotContain("aria-label=\"First page\" disabled", html);
    }

    [Fact]
    public void Page_Indicator_Shows_Current_And_Total()
    {
        var html = RenderPagination(3, 10, 50);

        Assert.Contains("Page 3 of 5", html);
    }

    [Fact]
    public void Navigation_Buttons_Have_HxGet()
    {
        var html = RenderPagination(2, 10, 50, "/items?handler=Table");

        Assert.Contains("hx-get=\"/items?handler=Table&amp;page=1\"", html);
        Assert.Contains("hx-get=\"/items?handler=Table&amp;page=3\"", html);
    }

    [Fact]
    public void Target_Applied_To_Buttons()
    {
        var html = RenderPagination(1, 10, 50, "/data", "#my-body");

        Assert.Contains("hx-target=\"#my-body\"", html);
    }

    [Fact]
    public void Has_Aria_Navigation_Label()
    {
        var html = RenderPagination(1, 10, 50);

        Assert.Contains("aria-label=\"Table pagination\"", html);
    }

    [Fact]
    public void Suppresses_Own_Output()
    {
        var helper = new DataTablePaginationTagHelper
        {
            Page = 1, PageSize = 10, TotalItems = 50, Url = "/data"
        };
        var context = CreateContext("rhx-data-table-pagination");
        SlotRenderer.CreateForContext(context);
        var output = CreateOutput("rhx-data-table-pagination");

        helper.Process(context, output);

        // Output should be suppressed (rendered in slot instead)
        Assert.Null(output.TagName);
    }
}
