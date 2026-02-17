using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace RazorHX.Rendering;

/// <summary>
/// Manages named child content sections (slots) within a composite Tag Helper.
/// Components use slots to define structured content regions that consumers can populate
/// via child tag helpers (e.g., <c>&lt;rhx-card-header&gt;</c>, <c>&lt;rhx-card-footer&gt;</c>).
/// </summary>
/// <remarks>
/// <para>
/// The slot system works via the <see cref="TagHelperContext.Items"/> dictionary:
/// a parent tag helper registers a <see cref="SlotRenderer"/> in its context items,
/// and child tag helpers retrieve it to register their content into named slots.
/// </para>
/// <example>
/// Parent tag helper:
/// <code>
/// public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
/// {
///     var slots = new SlotRenderer();
///     context.Items[typeof(SlotRenderer)] = slots;
///     await output.GetChildContentAsync(); // processes child tag helpers
///
///     // Now use slots.Get("header"), slots.Get("footer"), etc.
/// }
/// </code>
/// Child tag helper:
/// <code>
/// public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
/// {
///     if (context.Items.TryGetValue(typeof(SlotRenderer), out var obj) &amp;&amp; obj is SlotRenderer slots)
///     {
///         var content = await output.GetChildContentAsync();
///         slots.Set("header", content);
///     }
///     output.SuppressOutput(); // consumed by parent
/// }
/// </code>
/// </example>
/// </remarks>
public sealed class SlotRenderer
{
    private readonly Dictionary<string, IHtmlContent> _slots = new(StringComparer.OrdinalIgnoreCase);

    /// <summary>
    /// Registers content for a named slot from an <see cref="IHtmlContent"/> instance.
    /// </summary>
    /// <param name="name">The slot name (case-insensitive).</param>
    /// <param name="content">The HTML content to register.</param>
    public void Set(string name, IHtmlContent content)
    {
        _slots[name] = content;
    }

    /// <summary>
    /// Registers content for a named slot from a <see cref="TagHelperContent"/> instance.
    /// The content is snapshot at the time of this call.
    /// </summary>
    /// <param name="name">The slot name (case-insensitive).</param>
    /// <param name="content">The tag helper content to register.</param>
    public void Set(string name, TagHelperContent content)
    {
        _slots[name] = content;
    }

    /// <summary>
    /// Registers raw HTML string content for a named slot.
    /// </summary>
    /// <param name="name">The slot name (case-insensitive).</param>
    /// <param name="html">The raw HTML string.</param>
    public void SetHtml(string name, string html)
    {
        _slots[name] = new HtmlString(html);
    }

    /// <summary>
    /// Gets the content for a named slot, or null if the slot has not been populated.
    /// </summary>
    /// <param name="name">The slot name (case-insensitive).</param>
    /// <returns>The slot content, or null if not set.</returns>
    public IHtmlContent? Get(string name)
    {
        return _slots.GetValueOrDefault(name);
    }

    /// <summary>
    /// Checks whether a named slot has been populated with content.
    /// </summary>
    /// <param name="name">The slot name (case-insensitive).</param>
    /// <returns>True if the slot has content.</returns>
    public bool Has(string name) => _slots.ContainsKey(name);

    /// <summary>
    /// Gets the content for a named slot, or returns fallback content if the slot is not populated.
    /// </summary>
    /// <param name="name">The slot name (case-insensitive).</param>
    /// <param name="fallback">Fallback content to return if the slot is empty.</param>
    /// <returns>The slot content or the fallback.</returns>
    public IHtmlContent GetOrDefault(string name, IHtmlContent fallback)
    {
        return _slots.GetValueOrDefault(name) ?? fallback;
    }

    /// <summary>
    /// Gets the content for a named slot, or returns fallback HTML if the slot is not populated.
    /// </summary>
    /// <param name="name">The slot name (case-insensitive).</param>
    /// <param name="fallbackHtml">Fallback HTML string to return if the slot is empty.</param>
    /// <returns>The slot content or the fallback as <see cref="HtmlString"/>.</returns>
    public IHtmlContent GetOrDefault(string name, string fallbackHtml)
    {
        return _slots.GetValueOrDefault(name) ?? new HtmlString(fallbackHtml);
    }

    /// <summary>
    /// Gets all slot names that have been populated.
    /// </summary>
    /// <returns>An enumerable of populated slot names.</returns>
    public IEnumerable<string> GetSlotNames() => _slots.Keys;

    /// <summary>
    /// Returns the number of populated slots.
    /// </summary>
    public int Count => _slots.Count;

    /// <summary>
    /// Retrieves a <see cref="SlotRenderer"/> from a <see cref="TagHelperContext"/>'s Items dictionary,
    /// or returns null if no slot renderer is registered.
    /// </summary>
    /// <param name="context">The tag helper context to search.</param>
    /// <returns>The slot renderer, or null if not found.</returns>
    public static SlotRenderer? FromContext(TagHelperContext context)
    {
        return context.Items.TryGetValue(typeof(SlotRenderer), out var obj) && obj is SlotRenderer slots
            ? slots
            : null;
    }

    /// <summary>
    /// Registers a new <see cref="SlotRenderer"/> in a <see cref="TagHelperContext"/>'s Items dictionary.
    /// Parent tag helpers call this before <c>GetChildContentAsync()</c> to allow
    /// child tag helpers to register their slot content.
    /// </summary>
    /// <param name="context">The tag helper context to register in.</param>
    /// <returns>The newly created slot renderer.</returns>
    public static SlotRenderer CreateForContext(TagHelperContext context)
    {
        var slots = new SlotRenderer();
        context.Items[typeof(SlotRenderer)] = slots;
        return slots;
    }
}
