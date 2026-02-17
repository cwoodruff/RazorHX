using Microsoft.AspNetCore.Razor.TagHelpers;
using RazorHX.Rendering;

namespace RazorHX.Components.Actions;

/// <summary>
/// Captures trigger content for a dropdown menu. Wraps its child (typically an
/// <c>&lt;rhx-button&gt;</c>) in a trigger wrapper with ARIA attributes and
/// registers it to the parent dropdown's trigger slot.
/// </summary>
/// <remarks>
/// <para>
/// This tag helper suppresses its own output — the content is rendered by the
/// parent <see cref="DropdownTagHelper"/> in the correct position. ARIA attributes
/// (<c>aria-haspopup</c>, <c>aria-expanded</c>, <c>aria-controls</c>) are placed
/// on the wrapper div; JavaScript transfers them to the actual button element at init.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// &lt;rhx-dropdown&gt;
///     &lt;rhx-dropdown-trigger&gt;
///         &lt;rhx-button rhx-variant="brand"&gt;Actions&lt;/rhx-button&gt;
///     &lt;/rhx-dropdown-trigger&gt;
///     &lt;rhx-dropdown-item&gt;Edit&lt;/rhx-dropdown-item&gt;
/// &lt;/rhx-dropdown&gt;
/// </code>
/// </example>
[HtmlTargetElement("rhx-dropdown-trigger", ParentTag = "rhx-dropdown")]
public class DropdownTriggerTagHelper : TagHelper
{
    /// <inheritdoc/>
    public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
    {
        var slots = SlotRenderer.FromContext(context);
        if (slots == null)
        {
            // Not inside a dropdown — render as pass-through
            output.TagName = null;
            return;
        }

        var childContent = await output.GetChildContentAsync();

        // Read parent dropdown state
        var dropdown = context.Items.TryGetValue(typeof(DropdownTagHelper), out var dd)
            ? dd as DropdownTagHelper
            : null;
        var isOpen = dropdown?.Open ?? false;
        var isDisabled = dropdown?.Disabled ?? false;

        // Read panel ID for aria-controls
        var panelId = context.Items.TryGetValue("DropdownPanelId", out var id)
            ? id as string
            : null;

        // Build trigger wrapper HTML
        var html = new System.Text.StringBuilder();
        html.Append("<div data-rhx-dropdown-trigger");
        html.Append(" aria-haspopup=\"menu\"");
        html.Append($" aria-expanded=\"{isOpen.ToString().ToLowerInvariant()}\"");

        if (!string.IsNullOrEmpty(panelId))
        {
            html.Append($" aria-controls=\"{panelId}\"");
        }

        if (isDisabled)
        {
            html.Append(" aria-disabled=\"true\"");
        }

        html.Append('>');

        // Register assembled HTML to trigger slot
        slots.SetHtml("trigger", html.ToString() + childContent.GetContent() + "</div>");

        output.SuppressOutput();
    }
}
