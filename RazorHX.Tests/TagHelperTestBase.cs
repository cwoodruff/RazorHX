using Microsoft.AspNetCore.Razor.TagHelpers;

namespace RazorHX.Tests;

/// <summary>
/// Base class for tag helper unit tests.
/// Provides helpers to create TagHelperContext and TagHelperOutput for testing.
/// </summary>
public abstract class TagHelperTestBase
{
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

    protected static string? GetAttribute(TagHelperOutput output, string name)
    {
        return output.Attributes.TryGetAttribute(name, out var attr)
            ? attr.Value?.ToString()
            : null;
    }

    protected static bool HasClass(TagHelperOutput output, string className)
    {
        var classes = GetAttribute(output, "class") ?? "";
        return classes.Split(' ', StringSplitOptions.RemoveEmptyEntries).Contains(className);
    }
}
