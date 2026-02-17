using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace RazorHX.Rendering;

/// <summary>
/// Manages named slot/child content for composite components.
/// Allows components to define named content regions that consumers can populate.
/// </summary>
public sealed class SlotRenderer
{
    private readonly Dictionary<string, IHtmlContent> _slots = new(StringComparer.OrdinalIgnoreCase);

    /// <summary>
    /// Registers content for a named slot.
    /// </summary>
    public void Set(string name, IHtmlContent content)
    {
        _slots[name] = content;
    }

    /// <summary>
    /// Registers content for a named slot from a TagHelperContent.
    /// </summary>
    public void Set(string name, TagHelperContent content)
    {
        _slots[name] = content;
    }

    /// <summary>
    /// Gets the content for a named slot, or null if the slot is not populated.
    /// </summary>
    public IHtmlContent? Get(string name)
    {
        return _slots.GetValueOrDefault(name);
    }

    /// <summary>
    /// Checks whether a named slot has content.
    /// </summary>
    public bool Has(string name) => _slots.ContainsKey(name);

    /// <summary>
    /// Gets the content for a named slot, or returns the provided fallback content.
    /// </summary>
    public IHtmlContent GetOrDefault(string name, IHtmlContent fallback)
    {
        return _slots.GetValueOrDefault(name) ?? fallback;
    }

    /// <summary>
    /// Gets the content for a named slot, or returns the provided fallback HTML string.
    /// </summary>
    public IHtmlContent GetOrDefault(string name, string fallbackHtml)
    {
        return _slots.GetValueOrDefault(name) ?? new HtmlString(fallbackHtml);
    }
}
