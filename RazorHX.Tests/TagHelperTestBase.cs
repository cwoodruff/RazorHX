using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.AspNetCore.Routing;
using Moq;
using Xunit;

namespace RazorHX.Tests;

/// <summary>
/// Base class for tag helper unit tests.
/// Provides helpers to create TagHelperContext, TagHelperOutput,
/// mock IUrlHelperFactory, and assertion utilities.
/// </summary>
public abstract class TagHelperTestBase
{
    /// <summary>
    /// Creates a mock IUrlHelperFactory. The returned IUrlHelper will
    /// return predictable generated URLs for Page and Action calls.
    /// </summary>
    protected static IUrlHelperFactory CreateUrlHelperFactory(string? generatedUrl = "/generated-url")
    {
        var urlHelper = new Mock<IUrlHelper>();

        // Page() extension needs ActionContext to not be null
        urlHelper
            .SetupGet(h => h.ActionContext)
            .Returns(new ActionContext(
                new DefaultHttpContext(),
                new RouteData(),
                new ActionDescriptor()));

        // Page URL generation uses RouteUrl under the hood (Page() is an extension method)
        urlHelper
            .Setup(h => h.RouteUrl(It.IsAny<UrlRouteContext>()))
            .Returns(generatedUrl);

        // Action URL generation
        urlHelper
            .Setup(h => h.Action(It.IsAny<UrlActionContext>()))
            .Returns(generatedUrl);

        var factory = new Mock<IUrlHelperFactory>();
        factory
            .Setup(f => f.GetUrlHelper(It.IsAny<ActionContext>()))
            .Returns(urlHelper.Object);

        return factory.Object;
    }

    /// <summary>
    /// Creates a minimal ViewContext for testing tag helpers that need it.
    /// </summary>
    protected static ViewContext CreateViewContext()
    {
        var httpContext = new DefaultHttpContext();
        var actionContext = new ActionContext(
            httpContext,
            new RouteData(),
            new ActionDescriptor());

        return new ViewContext
        {
            HttpContext = httpContext,
            RouteData = actionContext.RouteData
        };
    }

    /// <summary>
    /// Creates a TagHelperContext with the given tag name and attributes.
    /// </summary>
    protected static TagHelperContext CreateContext(
        string tagName = "div",
        TagHelperAttributeList? attributes = null)
    {
        return new TagHelperContext(
            tagName: tagName,
            allAttributes: attributes ?? [],
            items: new Dictionary<object, object>(),
            uniqueId: Guid.NewGuid().ToString());
    }

    /// <summary>
    /// Creates a TagHelperOutput with the given tag name, attributes, and optional child content.
    /// </summary>
    protected static TagHelperOutput CreateOutput(
        string tagName = "div",
        TagHelperAttributeList? attributes = null,
        string? childContent = null)
    {
        return new TagHelperOutput(
            tagName: tagName,
            attributes: attributes ?? [],
            getChildContentAsync: (useCachedResult, encoder) =>
            {
                var content = new DefaultTagHelperContent();
                if (childContent != null)
                {
                    content.SetContent(childContent);
                }
                return Task.FromResult<TagHelperContent>(content);
            });
    }

    /// <summary>
    /// Gets the value of a named attribute from the output, or null if absent.
    /// </summary>
    protected static string? GetAttribute(TagHelperOutput output, string name)
    {
        return output.Attributes.TryGetAttribute(name, out var attr)
            ? attr.Value?.ToString()
            : null;
    }

    /// <summary>
    /// Returns true if the output's "class" attribute contains the given class name.
    /// </summary>
    protected static bool HasClass(TagHelperOutput output, string className)
    {
        var classes = GetAttribute(output, "class") ?? "";
        return classes.Split(' ', StringSplitOptions.RemoveEmptyEntries).Contains(className);
    }

    /// <summary>
    /// Asserts that the output has a specific attribute with a specific value.
    /// </summary>
    protected static void AssertAttribute(TagHelperOutput output, string name, string expectedValue)
    {
        var actual = GetAttribute(output, name);
        Assert.Equal(expectedValue, actual);
    }

    /// <summary>
    /// Asserts that the output does NOT have the specified attribute.
    /// </summary>
    protected static void AssertNoAttribute(TagHelperOutput output, string name)
    {
        Assert.False(
            output.Attributes.TryGetAttribute(name, out _),
            $"Expected attribute '{name}' to be absent, but it was present.");
    }
}
