using System.Net;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace RazorHX.Components.Forms;

/// <summary>
/// Renders validation error messages for a specific model property.
/// When errors exist, displays the first error message in a visible span.
/// When no errors exist, renders a hidden placeholder span for client-side validation.
/// </summary>
/// <example>
/// <code>
/// &lt;rhx-validation-message rhx-for="Email" /&gt;
/// </code>
/// </example>
[HtmlTargetElement("rhx-validation-message")]
public class ValidationMessageTagHelper : TagHelper
{
    /// <summary>
    /// ASP.NET Core model expression identifying the property to display errors for.
    /// </summary>
    [HtmlAttributeName("rhx-for")]
    public ModelExpression? For { get; set; }

    /// <summary>
    /// Additional CSS classes.
    /// </summary>
    [HtmlAttributeName("class")]
    public string? CssClass { get; set; }

    /// <summary>
    /// The current ViewContext (auto-injected).
    /// </summary>
    [HtmlAttributeNotBound]
    [ViewContext]
    public ViewContext ViewContext { get; set; } = default!;

    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        output.TagName = "span";
        output.TagMode = TagMode.StartTagAndEndTag;

        var name = For?.Name ?? "";
        var errorId = $"{SanitizeId(name)}-error";

        // Read errors from ModelState
        string? errorMessage = null;
        if (!string.IsNullOrEmpty(name) &&
            ViewContext.ModelState.TryGetValue(name, out var entry) &&
            entry.Errors.Count > 0)
        {
            errorMessage = entry.Errors[0].ErrorMessage;
        }

        var hasError = !string.IsNullOrEmpty(errorMessage);

        // Build CSS classes
        var classes = hasError
            ? "rhx-validation-message rhx-validation-message--error"
            : "rhx-validation-message";

        if (!string.IsNullOrWhiteSpace(CssClass))
            classes = $"{classes} {CssClass}";

        output.Attributes.SetAttribute("class", classes);
        output.Attributes.SetAttribute("id", errorId);
        output.Attributes.SetAttribute("aria-live", "polite");

        if (hasError)
        {
            output.Attributes.SetAttribute("role", "alert");
            output.Content.SetContent(errorMessage!);
        }
        else
        {
            output.Attributes.SetAttribute("hidden", "hidden");
        }
    }

    private static string SanitizeId(string name)
    {
        return name.Replace('.', '_').Replace('[', '_').Replace(']', '_');
    }
}
