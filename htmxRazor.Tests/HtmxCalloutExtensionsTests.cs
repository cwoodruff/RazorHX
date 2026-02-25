using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Routing;
using Moq;
using htmxRazor.Infrastructure;
using Xunit;

namespace htmxRazor.Tests;

public class HtmxCalloutExtensionsTests
{
    private static PageModel CreatePageModel()
    {
        var httpContext = new DefaultHttpContext();
        var modelState = new Microsoft.AspNetCore.Mvc.ModelBinding.ModelStateDictionary();
        var actionContext = new ActionContext(httpContext, new RouteData(), new Microsoft.AspNetCore.Mvc.Abstractions.ActionDescriptor(), modelState);
        var pageContext = new PageContext(actionContext);
        var page = new Mock<PageModel>();
        page.Object.PageContext = pageContext;
        return page.Object;
    }

    // ──────────────────────────────────────────────
    //  Basic rendering
    // ──────────────────────────────────────────────

    [Fact]
    public void Returns_ContentResult()
    {
        var page = CreatePageModel();

        var result = page.HtmxCallout("Hello");

        Assert.IsType<ContentResult>(result);
    }

    [Fact]
    public void ContentType_Is_TextHtml()
    {
        var page = CreatePageModel();

        var result = page.HtmxCallout("Hello");

        Assert.Equal("text/html", result.ContentType);
    }

    [Fact]
    public void StatusCode_Is_200()
    {
        var page = CreatePageModel();

        var result = page.HtmxCallout("Hello");

        Assert.Equal(200, result.StatusCode);
    }

    [Fact]
    public void Contains_Rhx_Callout_Tag()
    {
        var page = CreatePageModel();

        var result = page.HtmxCallout("Hello");

        Assert.Contains("<rhx-callout", result.Content);
        Assert.Contains("</rhx-callout>", result.Content);
    }

    [Fact]
    public void Contains_Message_Text()
    {
        var page = CreatePageModel();

        var result = page.HtmxCallout("Item saved!");

        Assert.Contains("Item saved!", result.Content);
    }

    // ──────────────────────────────────────────────
    //  Default variant
    // ──────────────────────────────────────────────

    [Fact]
    public void Default_Variant_Is_Neutral()
    {
        var page = CreatePageModel();

        var result = page.HtmxCallout("Hello");

        Assert.Contains("rhx-variant=\"neutral\"", result.Content);
    }

    // ──────────────────────────────────────────────
    //  Custom variant
    // ──────────────────────────────────────────────

    [Theory]
    [InlineData("success")]
    [InlineData("danger")]
    [InlineData("warning")]
    [InlineData("brand")]
    public void Custom_Variant_Renders(string variant)
    {
        var page = CreatePageModel();

        var result = page.HtmxCallout("Hello", variant: variant);

        Assert.Contains($"rhx-variant=\"{variant}\"", result.Content);
    }

    // ──────────────────────────────────────────────
    //  Duration
    // ──────────────────────────────────────────────

    [Fact]
    public void Default_Duration_No_Attribute()
    {
        var page = CreatePageModel();

        var result = page.HtmxCallout("Hello");

        Assert.DoesNotContain("rhx-duration", result.Content);
    }

    [Fact]
    public void Custom_Duration_Renders()
    {
        var page = CreatePageModel();

        var result = page.HtmxCallout("Hello", duration: 3000);

        Assert.Contains("rhx-duration=\"3000\"", result.Content);
    }

    // ──────────────────────────────────────────────
    //  Closable
    // ──────────────────────────────────────────────

    [Fact]
    public void Default_Closable_True()
    {
        var page = CreatePageModel();

        var result = page.HtmxCallout("Hello");

        Assert.Contains("rhx-closable=\"true\"", result.Content);
    }

    [Fact]
    public void Closable_False_No_Attribute()
    {
        var page = CreatePageModel();

        var result = page.HtmxCallout("Hello", closable: false);

        Assert.DoesNotContain("rhx-closable", result.Content);
    }

    // ──────────────────────────────────────────────
    //  HTML encoding
    // ──────────────────────────────────────────────

    [Fact]
    public void Message_Is_Html_Encoded()
    {
        var page = CreatePageModel();

        var result = page.HtmxCallout("<script>alert('xss')</script>");

        Assert.DoesNotContain("<script>", result.Content);
        Assert.Contains("&lt;script&gt;", result.Content);
    }

    [Fact]
    public void Variant_Is_Html_Encoded()
    {
        var page = CreatePageModel();

        var result = page.HtmxCallout("Hello", variant: "bad\"value");

        Assert.DoesNotContain("bad\"value", result.Content);
        Assert.Contains("bad&quot;value", result.Content);
    }

    // ──────────────────────────────────────────────
    //  All parameters combined
    // ──────────────────────────────────────────────

    [Fact]
    public void All_Parameters()
    {
        var page = CreatePageModel();

        var result = page.HtmxCallout(
            "Saved!",
            variant: "success",
            duration: 5000,
            closable: true);

        Assert.Contains("rhx-variant=\"success\"", result.Content);
        Assert.Contains("rhx-closable=\"true\"", result.Content);
        Assert.Contains("rhx-duration=\"5000\"", result.Content);
        Assert.Contains("Saved!", result.Content);
    }
}
