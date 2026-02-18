using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Routing;
using Moq;
using RazorHX.Infrastructure;
using Xunit;

namespace RazorHX.Tests;

public class HtmxValidationExtensionsTests
{
    private static Mock<PageModel> CreatePageModelMock()
    {
        var httpContext = new DefaultHttpContext();
        var pageModel = new Mock<PageModel>() { CallBase = true };

        var actionContext = new ActionContext(
            httpContext,
            new RouteData(),
            new Microsoft.AspNetCore.Mvc.Abstractions.ActionDescriptor());

        var pageContext = new PageContext(actionContext)
        {
            ViewData = new ViewDataDictionary(
                new EmptyModelMetadataProvider(),
                new ModelStateDictionary())
        };
        pageModel.Object.PageContext = pageContext;

        // Mock Partial to return a PartialViewResult without needing full infrastructure
        pageModel.Setup(p => p.Partial(It.IsAny<string>(), It.IsAny<object>()))
            .Returns((string name, object model) => new PartialViewResult
            {
                ViewName = name,
                ViewData = new ViewDataDictionary(
                    new EmptyModelMetadataProvider(),
                    new ModelStateDictionary())
                {
                    Model = model
                }
            });

        return pageModel;
    }

    [Fact]
    public void HtmxValidationFailure_Sets_422_Status()
    {
        var mock = CreatePageModelMock();
        var page = mock.Object;

        page.HtmxValidationFailure("_Form");

        Assert.Equal(422, page.Response.StatusCode);
    }

    [Fact]
    public void HtmxValidationFailure_Returns_PartialViewResult()
    {
        var mock = CreatePageModelMock();
        var page = mock.Object;

        var result = page.HtmxValidationFailure("_Form");

        Assert.IsType<PartialViewResult>(result);
    }

    [Fact]
    public void HtmxValidationFailure_Uses_Given_Partial_Name()
    {
        var mock = CreatePageModelMock();
        var page = mock.Object;

        var result = page.HtmxValidationFailure("_ContactForm");

        var partial = Assert.IsType<PartialViewResult>(result);
        Assert.Equal("_ContactForm", partial.ViewName);
    }

    [Fact]
    public void HtmxSuccess_Returns_PartialViewResult()
    {
        var mock = CreatePageModelMock();
        var page = mock.Object;

        var result = page.HtmxSuccess("_Form");

        Assert.IsType<PartialViewResult>(result);
    }

    [Fact]
    public void HtmxSuccess_With_Message_Sets_Trigger_Header()
    {
        var mock = CreatePageModelMock();
        var page = mock.Object;

        page.HtmxSuccess("_Form", message: "Saved!");

        var header = page.Response.Headers["HX-Trigger-After-Settle"].ToString();
        Assert.Contains("rhx:toast", header);
        Assert.Contains("Saved!", header);
    }

    [Fact]
    public void HtmxSuccess_Without_Message_No_Trigger_Header()
    {
        var mock = CreatePageModelMock();
        var page = mock.Object;

        page.HtmxSuccess("_Form");

        Assert.False(page.Response.Headers.ContainsKey("HX-Trigger-After-Settle"));
    }
}
