using Microsoft.AspNetCore.Mvc.RazorPages;
using RazorHX.Components.Navigation;
using RazorHX.Demo.Models;

namespace RazorHX.Demo.Pages.Docs.Components;

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

    public void OnGet()
    {
        ViewData["Breadcrumbs"] = new List<BreadcrumbItem>
        {
            new("Home", "/"),
            new("Components", "/Docs/Components/Textarea"),
            new("Textarea")
        };
    }
}
