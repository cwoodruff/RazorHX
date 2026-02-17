using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Razor.TagHelpers;
using RazorHX.Infrastructure;
using RazorHX.Rendering;

namespace RazorHX.Components.Actions;

/// <summary>
/// Renders a dropdown menu container. Uses a slot-based trigger mechanism:
/// the <c>&lt;rhx-dropdown-trigger&gt;</c> child provides the trigger button,
/// and <c>&lt;rhx-dropdown-item&gt;</c> children populate the menu panel.
/// </summary>
/// <remarks>
/// <para>
/// JavaScript (<c>rhx-dropdown.js</c>) handles open/close toggling, keyboard
/// navigation, click-outside dismissal, viewport flip detection, and focus management.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// &lt;rhx-dropdown&gt;
///     &lt;rhx-dropdown-trigger&gt;
///         &lt;rhx-button rhx-variant="brand"&gt;Actions &amp;#9662;&lt;/rhx-button&gt;
///     &lt;/rhx-dropdown-trigger&gt;
///     &lt;rhx-dropdown-item&gt;Edit&lt;/rhx-dropdown-item&gt;
///     &lt;rhx-dropdown-divider /&gt;
///     &lt;rhx-dropdown-item&gt;Delete&lt;/rhx-dropdown-item&gt;
/// &lt;/rhx-dropdown&gt;
/// </code>
/// </example>
[HtmlTargetElement("rhx-dropdown")]
public class DropdownTagHelper : RazorHXTagHelperBase
{
    /// <inheritdoc/>
    protected override string BlockName => "dropdown";

    /// <summary>
    /// Whether the dropdown panel is initially open.
    /// </summary>
    [HtmlAttributeName("rhx-open")]
    public bool Open { get; set; }

    /// <summary>
    /// The preferred placement of the dropdown panel relative to the trigger.
    /// Options: bottom-start (default), bottom-end, top-start, top-end.
    /// CSS and JS handle actual positioning; JS flips when the panel would overflow the viewport.
    /// </summary>
    [HtmlAttributeName("rhx-placement")]
    public string Placement { get; set; } = "bottom-start";

    /// <summary>
    /// Whether the dropdown is disabled. Prevents opening and dims the trigger.
    /// </summary>
    [HtmlAttributeName("rhx-disabled")]
    public bool Disabled { get; set; }

    /// <summary>
    /// When true, the dropdown stays open after an item is selected.
    /// Useful for checkbox menus or multi-action panels.
    /// </summary>
    [HtmlAttributeName("rhx-stay-open")]
    public bool StayOpenOnSelect { get; set; }

    /// <summary>
    /// Accessible label for the dropdown menu panel.
    /// </summary>
    [HtmlAttributeName("aria-label")]
    public string? AriaLabel { get; set; }

    /// <summary>
    /// Creates a new DropdownTagHelper with URL generation support.
    /// </summary>
    public DropdownTagHelper(IUrlHelperFactory urlHelperFactory) : base(urlHelperFactory) { }

    /// <inheritdoc/>
    public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
    {
        // ── Set up slot renderer and context ──
        var slots = SlotRenderer.CreateForContext(context);
        context.Items[typeof(DropdownTagHelper)] = this;

        // ── Generate unique panel ID ──
        var panelId = !string.IsNullOrWhiteSpace(Id)
            ? $"{Id}-panel"
            : $"rhx-dropdown-{context.UniqueId}";
        context.Items["DropdownPanelId"] = panelId;

        // ── Process children ──
        var childContent = await output.GetChildContentAsync();

        // ── Render outer container ──
        output.TagName = "div";
        output.TagMode = TagMode.StartTagAndEndTag;

        var css = CreateCssBuilder()
            .AddIf(GetModifierClass("open"), Open)
            .AddIf(GetModifierClass("disabled"), Disabled);
        ApplyBaseAttributes(output, css);

        // ── Data attributes for JS ──
        output.Attributes.SetAttribute("data-rhx-dropdown", "");
        output.Attributes.SetAttribute("data-rhx-placement", Placement.ToLowerInvariant());

        if (StayOpenOnSelect)
        {
            output.Attributes.SetAttribute("data-rhx-stay-open", "");
        }

        // ── ARIA ──
        if (!string.IsNullOrWhiteSpace(AriaLabel))
        {
            // aria-label goes on the panel, not the container
        }

        // ── htmx (rare on container but supported) ──
        RenderHtmxAttributes(output);

        // ── Assemble inner HTML ──
        output.Content.Clear();

        // Trigger from slot
        if (slots.Has("trigger"))
        {
            output.Content.AppendHtml(slots.Get("trigger")!);
        }

        // Panel wrapper around child content (items/dividers)
        var ariaHiddenValue = Open ? "false" : "true";
        var hiddenAttr = Open ? "" : " hidden";

        output.Content.AppendHtml(
            $"<div class=\"{GetElementClass("panel")}\" " +
            $"id=\"{panelId}\" " +
            $"role=\"menu\"" +
            (!string.IsNullOrWhiteSpace(AriaLabel) ? $" aria-label=\"{AriaLabel}\"" : "") +
            $" aria-hidden=\"{ariaHiddenValue}\"{hiddenAttr}>");
        output.Content.AppendHtml(childContent);
        output.Content.AppendHtml("</div>");
    }
}
