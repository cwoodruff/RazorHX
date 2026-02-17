using System.Net;
using System.Text;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace RazorHX.Components.Forms;

/// <summary>
/// Renders an individual radio button for use inside <c>&lt;rhx-radio-group&gt;</c>.
/// Reads the parent group's name, selected value, and disabled state from the tag helper context
/// to render a label wrapping a visually-hidden native radio input and a custom control.
/// </summary>
/// <example>
/// <code>
/// &lt;rhx-radio-group rhx-label="Size" name="size"&gt;
///     &lt;rhx-radio value="sm"&gt;Small&lt;/rhx-radio&gt;
///     &lt;rhx-radio value="md"&gt;Medium&lt;/rhx-radio&gt;
///     &lt;rhx-radio value="lg"&gt;Large&lt;/rhx-radio&gt;
/// &lt;/rhx-radio-group&gt;
/// </code>
/// </example>
[HtmlTargetElement("rhx-radio")]
public class RadioTagHelper : TagHelper
{
    /// <summary>The value submitted with the form when this radio is selected.</summary>
    [HtmlAttributeName("value")]
    public string? Value { get; set; }

    /// <summary>The label text. Falls back to child content.</summary>
    [HtmlAttributeName("rhx-label")]
    public string? Label { get; set; }

    /// <summary>Whether this individual radio button is disabled.</summary>
    [HtmlAttributeName("rhx-disabled")]
    public bool Disabled { get; set; }

    /// <inheritdoc/>
    public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
    {
        var childContent = await output.GetChildContentAsync();
        var text = Label ?? childContent.GetContent().Trim();

        var name = context.Items.TryGetValue("RadioGroupName", out var n)
            ? n as string ?? ""
            : "";
        var groupValue = context.Items.TryGetValue("RadioGroupValue", out var gv)
            ? gv as string
            : null;
        var groupDisabled = context.Items.TryGetValue("RadioGroupDisabled", out var gd)
            && gd is bool gdBool && gdBool;

        var value = Value ?? text;
        var isSelected = string.Equals(value, groupValue, StringComparison.OrdinalIgnoreCase);
        var isDisabled = Disabled || groupDisabled;

        output.TagName = "label";
        output.TagMode = TagMode.StartTagAndEndTag;

        var cls = "rhx-radio";
        if (isDisabled) cls += " rhx-radio--disabled";
        output.Attributes.SetAttribute("class", cls);

        var sb = new StringBuilder();

        // Native radio input (visually hidden)
        sb.Append("<input type=\"radio\" class=\"rhx-radio__native rhx-sr-only\"");
        sb.Append($" name=\"{Enc(name)}\" value=\"{Enc(value)}\"");
        if (isSelected) sb.Append(" checked");
        if (isDisabled) sb.Append(" disabled");
        sb.Append(" />");

        // Custom visual control
        sb.Append("<span class=\"rhx-radio__control\" aria-hidden=\"true\"></span>");

        // Label text
        if (!string.IsNullOrEmpty(text))
            sb.Append($"<span class=\"rhx-radio__text\">{Enc(text)}</span>");

        output.Content.SetHtmlContent(sb.ToString());
    }

    private static string Enc(string? value) => WebUtility.HtmlEncode(value ?? "") ?? "";
}
