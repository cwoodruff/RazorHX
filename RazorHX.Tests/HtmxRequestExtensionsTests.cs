using Microsoft.AspNetCore.Http;
using RazorHX.Infrastructure;
using Xunit;

namespace RazorHX.Tests;

public class HtmxRequestExtensionsTests
{
    private static HttpRequest CreateRequest(params (string key, string value)[] headers)
    {
        var context = new DefaultHttpContext();
        foreach (var (key, value) in headers)
        {
            context.Request.Headers[key] = value;
        }
        return context.Request;
    }

    [Fact]
    public void IsHtmxRequest_True_When_Header_Present()
    {
        var request = CreateRequest(("HX-Request", "true"));
        Assert.True(request.IsHtmxRequest());
    }

    [Fact]
    public void IsHtmxRequest_False_When_Header_Absent()
    {
        var request = CreateRequest();
        Assert.False(request.IsHtmxRequest());
    }

    [Fact]
    public void IsHtmxBoosted_True_When_Header_Is_True()
    {
        var request = CreateRequest(("HX-Boosted", "true"));
        Assert.True(request.IsHtmxBoosted());
    }

    [Fact]
    public void IsHtmxBoosted_False_When_Header_Absent()
    {
        var request = CreateRequest();
        Assert.False(request.IsHtmxBoosted());
    }

    [Fact]
    public void IsHtmxHistoryRestore_True_When_Header_Is_True()
    {
        var request = CreateRequest(("HX-History-Restore-Request", "true"));
        Assert.True(request.IsHtmxHistoryRestore());
    }

    [Fact]
    public void GetHxTarget_Returns_Header_Value()
    {
        var request = CreateRequest(("HX-Target", "my-div"));
        Assert.Equal("my-div", request.GetHxTarget());
    }

    [Fact]
    public void GetHxTarget_Returns_Null_When_Absent()
    {
        var request = CreateRequest();
        Assert.Null(request.GetHxTarget());
    }

    [Fact]
    public void GetHxTriggerId_Returns_Header_Value()
    {
        var request = CreateRequest(("HX-Trigger", "btn-submit"));
        Assert.Equal("btn-submit", request.GetHxTriggerId());
    }

    [Fact]
    public void GetHxTriggerName_Returns_Header_Value()
    {
        var request = CreateRequest(("HX-Trigger-Name", "submit-btn"));
        Assert.Equal("submit-btn", request.GetHxTriggerName());
    }

    [Fact]
    public void GetHxPrompt_Returns_Header_Value()
    {
        var request = CreateRequest(("HX-Prompt", "user input"));
        Assert.Equal("user input", request.GetHxPrompt());
    }

    [Fact]
    public void GetCurrentUrl_Returns_Header_Value()
    {
        var request = CreateRequest(("HX-Current-URL", "https://example.com/page"));
        Assert.Equal("https://example.com/page", request.GetCurrentUrl());
    }

    [Fact]
    public void GetCurrentUrl_Returns_Null_When_Absent()
    {
        var request = CreateRequest();
        Assert.Null(request.GetCurrentUrl());
    }
}
