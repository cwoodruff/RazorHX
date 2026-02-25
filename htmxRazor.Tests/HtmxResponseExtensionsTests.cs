using Microsoft.AspNetCore.Http;
using htmxRazor.Infrastructure;
using Xunit;

namespace htmxRazor.Tests;

public class HtmxResponseExtensionsTests
{
    private static HttpResponse CreateResponse()
    {
        var context = new DefaultHttpContext();
        return context.Response;
    }

    [Fact]
    public void HxRedirect_Sets_Header()
    {
        var response = CreateResponse();
        response.HxRedirect("/new-page");
        Assert.Equal("/new-page", response.Headers["HX-Redirect"].ToString());
    }

    [Fact]
    public void HxRefresh_Sets_Header_To_True()
    {
        var response = CreateResponse();
        response.HxRefresh();
        Assert.Equal("true", response.Headers["HX-Refresh"].ToString());
    }

    [Fact]
    public void HxRetarget_Sets_Header()
    {
        var response = CreateResponse();
        response.HxRetarget("#new-target");
        Assert.Equal("#new-target", response.Headers["HX-Retarget"].ToString());
    }

    [Fact]
    public void HxReswap_Sets_Header()
    {
        var response = CreateResponse();
        response.HxReswap("outerHTML");
        Assert.Equal("outerHTML", response.Headers["HX-Reswap"].ToString());
    }

    [Fact]
    public void HxPushUrl_Sets_Header()
    {
        var response = CreateResponse();
        response.HxPushUrl("/items/42");
        Assert.Equal("/items/42", response.Headers["HX-Push-Url"].ToString());
    }

    [Fact]
    public void HxReplaceUrl_Sets_Header()
    {
        var response = CreateResponse();
        response.HxReplaceUrl("/items/42");
        Assert.Equal("/items/42", response.Headers["HX-Replace-Url"].ToString());
    }

    [Fact]
    public void HxLocation_Sets_Header()
    {
        var response = CreateResponse();
        response.HxLocation("/items");
        Assert.Equal("/items", response.Headers["HX-Location"].ToString());
    }

    [Fact]
    public void HxTrigger_Simple_Sets_EventName()
    {
        var response = CreateResponse();
        response.HxTrigger("itemDeleted");
        Assert.Equal("itemDeleted", response.Headers["HX-Trigger"].ToString());
    }

    [Fact]
    public void HxTrigger_WithDetail_Sets_Json()
    {
        var response = CreateResponse();
        response.HxTrigger("showMessage", new { message = "Saved!" });
        var header = response.Headers["HX-Trigger"].ToString();
        Assert.Contains("showMessage", header);
        Assert.Contains("Saved!", header);
    }

    [Fact]
    public void HxTriggerAfterSettle_Simple_Sets_EventName()
    {
        var response = CreateResponse();
        response.HxTriggerAfterSettle("animateIn");
        Assert.Equal("animateIn", response.Headers["HX-Trigger-After-Settle"].ToString());
    }

    [Fact]
    public void HxTriggerAfterSettle_WithDetail_Sets_Json()
    {
        var response = CreateResponse();
        response.HxTriggerAfterSettle("notify", new { level = "success" });
        var header = response.Headers["HX-Trigger-After-Settle"].ToString();
        Assert.Contains("notify", header);
        Assert.Contains("success", header);
    }

    [Fact]
    public void HxTriggerAfterSwap_Simple_Sets_EventName()
    {
        var response = CreateResponse();
        response.HxTriggerAfterSwap("scrollToTop");
        Assert.Equal("scrollToTop", response.Headers["HX-Trigger-After-Swap"].ToString());
    }

    [Fact]
    public void HxTriggerAfterSwap_WithDetail_Sets_Json()
    {
        var response = CreateResponse();
        response.HxTriggerAfterSwap("highlight", new { id = 42 });
        var header = response.Headers["HX-Trigger-After-Swap"].ToString();
        Assert.Contains("highlight", header);
        Assert.Contains("42", header);
    }
}
