using htmxRazor.Infrastructure;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace htmxRazor.Components.Feedback;

/// <summary>
/// Renders a fixed-position container that holds and stacks toast notifications.
/// Place one <c>&lt;rhx-toast-container&gt;</c> in your layout — toasts are added
/// dynamically via the <c>rhx:toast</c> event or OOB swaps.
/// </summary>
/// <example>
/// <code>
/// &lt;!-- In _Layout.cshtml --&gt;
/// &lt;rhx-toast-container rhx-position="top-end" /&gt;
/// </code>
/// </example>
[HtmlTargetElement("rhx-toast-container")]
public class ToastContainerTagHelper : htmxRazorTagHelperBase
{
    /// <inheritdoc/>
    protected override string BlockName => "toast-container";

    /// <summary>
    /// The position of the container on screen.
    /// Options: top-start, top-center, top-end, bottom-start, bottom-center, bottom-end.
    /// Default: top-end.
    /// </summary>
    [HtmlAttributeName("rhx-position")]
    public string Position { get; set; } = "top-end";

    /// <summary>
    /// Maximum number of toasts to display simultaneously. Oldest are removed first. Default: 5.
    /// </summary>
    [HtmlAttributeName("rhx-max")]
    public int MaxToasts { get; set; } = 5;

    /// <summary>
    /// Default auto-dismiss duration in milliseconds for toasts without an explicit duration.
    /// 0 = no auto-dismiss. Default: 5000.
    /// </summary>
    [HtmlAttributeName("rhx-duration")]
    public int DefaultDuration { get; set; } = 5000;

    /// <inheritdoc/>
    public override Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
    {
        output.TagName = "div";
        output.TagMode = TagMode.StartTagAndEndTag;

        var css = CreateCssBuilder()
            .Add(GetModifierClass(Position));

        ApplyBaseAttributes(output, css);

        // Ensure a stable id for OOB swap targeting
        if (string.IsNullOrWhiteSpace(Id))
            output.Attributes.SetAttribute("id", "rhx-toasts");

        output.Attributes.SetAttribute("role", "status");
        output.Attributes.SetAttribute("aria-live", "polite");
        output.Attributes.SetAttribute("aria-relevant", "additions");
        output.Attributes.SetAttribute("data-rhx-toast-container", "");
        output.Attributes.SetAttribute("data-rhx-max", MaxToasts.ToString());
        output.Attributes.SetAttribute("data-rhx-duration", DefaultDuration.ToString());

        return Task.CompletedTask;
    }
}
