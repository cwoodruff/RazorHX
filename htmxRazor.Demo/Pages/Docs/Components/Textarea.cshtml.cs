using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using htmxRazor.Components.Navigation;
using htmxRazor.Demo.Models;

namespace htmxRazor.Demo.Pages.Docs.Components;

public class TextareaModel : PageModel
{
    public List<ComponentProperty> Properties { get; } = new()
    {
        new("rhx-for", "ModelExpression", "-", "ASP.NET Core model expression for two-way binding"),
        new("name", "string", "-", "The form field name"),
        new("value", "string", "-", "The current value"),
        new("rhx-label", "string", "-", "Label text displayed above the textarea"),
        new("rhx-hint", "string", "-", "Hint text displayed below the textarea"),
        new("rhx-size", "string", "medium", "Textarea size: small, medium, large"),
        new("rhx-disabled", "bool", "false", "Whether the textarea is disabled"),
        new("rhx-readonly", "bool", "false", "Whether the textarea is read-only"),
        new("rhx-required", "bool", "false", "Whether the field is required"),
        new("rhx-placeholder", "string", "-", "Placeholder text"),
        new("rhx-rows", "int?", "-", "Number of visible text rows"),
        new("rhx-resize", "string", "vertical", "Resize behavior: vertical, horizontal, auto, none"),
        new("rhx-maxlength", "int?", "-", "Maximum character length"),
        new("rhx-filled", "bool", "false", "Use filled appearance"),
    };

    public string BasicCode => @"<rhx-textarea rhx-label=""Notes""
           rhx-placeholder=""Enter your notes...""
           name=""notes"" />";

    public string HintMaxlengthCode => @"<rhx-textarea rhx-label=""Bio"" rhx-rows=""5""
           rhx-placeholder=""Tell us about yourself...""
           rhx-maxlength=""500""
           rhx-hint=""Max 500 characters""
           name=""bio"" />";

    public string AutoResizeCode => @"<rhx-textarea rhx-label=""Auto-resize""
           rhx-resize=""auto"" rhx-rows=""2""
           rhx-placeholder=""Type here and watch the textarea grow...""
           name=""auto"" />";

    public string SizesCode => @"<rhx-textarea rhx-label=""Small"" rhx-size=""small"" name=""ta-sm"" />
<rhx-textarea rhx-label=""Medium (default)"" name=""ta-md"" />
<rhx-textarea rhx-label=""Large"" rhx-size=""large"" name=""ta-lg"" />";

    public string StatesCode => @"<rhx-textarea rhx-label=""Disabled"" rhx-disabled=""true""
           value=""Cannot edit"" name=""ta-dis"" />
<rhx-textarea rhx-label=""Readonly"" rhx-readonly=""true""
           value=""This content is read-only"" name=""ta-ro"" />";

    public string HtmxCode => @"<rhx-textarea rhx-label=""Leave a Comment"" name=""comment""
           rhx-rows=""3""
           rhx-placeholder=""Write your comment here..."" />
<rhx-button rhx-variant=""brand""
            hx-post=""/Docs/Components/Textarea?handler=Comment""
            hx-include=""[name='comment']""
            hx-target=""#comment-result"">
    Submit Comment
</rhx-button>
<div id=""comment-result"">Write a comment and click submit...</div>";

    public void OnGet()
    {
        ViewData["Breadcrumbs"] = new List<BreadcrumbItem>
        {
            new("Home", "/"),
            new("Components", "/Docs/Components/Textarea"),
            new("Textarea")
        };
    }

    public IActionResult OnPostComment(string? comment)
    {
        if (string.IsNullOrWhiteSpace(comment))
        {
            return Content("<span style=\"color: var(--rhx-color-text-muted);\">Please enter a comment before submitting.</span>", "text/html");
        }

        var encoded = System.Net.WebUtility.HtmlEncode(comment);
        return Content($"""
            <div style="padding: var(--rhx-space-md); background: var(--rhx-color-surface-raised); border-radius: var(--rhx-radius-md); color: var(--rhx-color-text-muted);">
                <strong>Comment submitted!</strong>
                <p style="margin: var(--rhx-space-xs) 0 0 0; font-style: italic;">"{encoded}"</p>
            </div>
            """, "text/html");
    }
}
