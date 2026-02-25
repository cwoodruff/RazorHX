using htmxRazor.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace htmxRazor.Components.Actions;

/// <summary>
/// Renders a single item within a dropdown menu. Supports normal items, checkbox items,
/// link items, and disabled items. Each item can independently carry htmx attributes.
/// </summary>
/// <remarks>
/// <para>
/// Normal items render as <c>&lt;button role="menuitem"&gt;</c>. Checkbox items use
/// <c>role="menuitemcheckbox"</c> with <c>aria-checked</c>. When <see cref="Href"/>
/// is set, the item renders as an <c>&lt;a&gt;</c> tag.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// &lt;rhx-dropdown-item&gt;Edit&lt;/rhx-dropdown-item&gt;
///
/// &lt;rhx-dropdown-item rhx-type="checkbox" rhx-checked="true" rhx-value="col-name"&gt;
///     Name
/// &lt;/rhx-dropdown-item&gt;
///
/// &lt;rhx-dropdown-item rhx-href="/settings"&gt;Settings&lt;/rhx-dropdown-item&gt;
///
/// &lt;rhx-dropdown-item hx-post="/api/action" hx-target="#result"&gt;
///     Run Action
/// &lt;/rhx-dropdown-item&gt;
/// </code>
/// </example>
[HtmlTargetElement("rhx-dropdown-item")]
public class DropdownItemTagHelper : htmxRazorTagHelperBase
{
    /// <inheritdoc/>
    protected override string BlockName => "dropdown";

    /// <summary>
    /// The type of dropdown item.
    /// Options: normal (default), checkbox.
    /// Checkbox items toggle a check mark and use <c>role="menuitemcheckbox"</c>.
    /// </summary>
    [HtmlAttributeName("rhx-type")]
    public string Type { get; set; } = "normal";

    /// <summary>
    /// Whether a checkbox item is checked. Only used when <see cref="Type"/> is "checkbox".
    /// </summary>
    [HtmlAttributeName("rhx-checked")]
    public bool Checked { get; set; }

    /// <summary>
    /// A data value associated with this item. Rendered as <c>data-value</c> for JS access.
    /// </summary>
    [HtmlAttributeName("rhx-value")]
    public string? Value { get; set; }

    /// <summary>
    /// Whether the item is disabled. Prevents interaction and dims the item.
    /// </summary>
    [HtmlAttributeName("rhx-disabled")]
    public bool Disabled { get; set; }

    /// <summary>
    /// When set, the item renders as an <c>&lt;a&gt;</c> tag with this URL.
    /// </summary>
    [HtmlAttributeName("rhx-href")]
    public string? Href { get; set; }

    /// <summary>
    /// Accessible label for the item. Useful when the visible text is insufficient.
    /// </summary>
    [HtmlAttributeName("aria-label")]
    public string? AriaLabel { get; set; }

    /// <summary>
    /// Creates a new DropdownItemTagHelper with URL generation support.
    /// </summary>
    public DropdownItemTagHelper(IUrlHelperFactory urlHelperFactory) : base(urlHelperFactory) { }

    /// <inheritdoc/>
    public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
    {
        var isLink = !string.IsNullOrWhiteSpace(Href);
        var isCheckbox = Type.Equals("checkbox", StringComparison.OrdinalIgnoreCase);

        // ── Tag name ──
        output.TagName = isLink ? "a" : "button";
        output.TagMode = TagMode.StartTagAndEndTag;

        // ── CSS classes ──
        // Item is a BEM element of "dropdown", not the block itself
        var itemClass = GetElementClass("item");
        var css = new CssClassBuilder(itemClass)
            .AddIf($"{itemClass}--checkbox", isCheckbox)
            .AddIf($"{itemClass}--checked", isCheckbox && Checked)
            .AddIf($"{itemClass}--disabled", Disabled);

        if (!string.IsNullOrWhiteSpace(CssClass))
        {
            css.Add(CssClass);
        }

        output.Attributes.SetAttribute("class", css.Build());

        // ── Common attributes ──
        if (!string.IsNullOrWhiteSpace(Id))
        {
            output.Attributes.SetAttribute("id", Id);
        }

        if (Hidden)
        {
            output.Attributes.SetAttribute("hidden", "hidden");
        }

        // ── Role ──
        if (isCheckbox)
        {
            AriaAttributeHelper.SetRole(output, "menuitemcheckbox");
            AriaAttributeHelper.AriaChecked(output, Checked);
        }
        else
        {
            AriaAttributeHelper.RoleMenuItem(output);
        }

        // ── Element-specific attributes ──
        if (isLink)
        {
            output.Attributes.SetAttribute("href", Href);
        }
        else
        {
            output.Attributes.SetAttribute("type", "button");
        }

        // ── Disabled state ──
        if (Disabled)
        {
            if (isLink)
            {
                AriaAttributeHelper.AriaDisabled(output, true);
                output.Attributes.SetAttribute("tabindex", "-1");
            }
            else
            {
                output.Attributes.SetAttribute("disabled", "disabled");
            }
        }

        // ── Data value ──
        if (!string.IsNullOrWhiteSpace(Value))
        {
            output.Attributes.SetAttribute("data-value", Value);
        }

        // ── Accessible label ──
        if (!string.IsNullOrWhiteSpace(AriaLabel))
        {
            AriaAttributeHelper.AriaLabel(output, AriaLabel);
        }

        // ── htmx attributes ──
        RenderHtmxAttributes(output);

        // ── Inner content structure ──
        var childContent = await output.GetChildContentAsync();
        output.Content.Clear();

        // Checkbox check mark (prepended)
        if (isCheckbox)
        {
            output.Content.AppendHtml(
                $"<span class=\"{GetElementClass("item-check")}\" aria-hidden=\"true\">" +
                (Checked ? "&#10003;" : "") +
                "</span>");
        }

        // Label wrapper
        output.Content.AppendHtml($"<span class=\"{GetElementClass("item-label")}\">");
        output.Content.AppendHtml(childContent);
        output.Content.AppendHtml("</span>");
    }
}
