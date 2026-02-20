using Microsoft.AspNetCore.Mvc.RazorPages;
using RazorHX.Components.Navigation;
using RazorHX.Demo.Models;

namespace RazorHX.Demo.Pages.Docs.Components;

public class CheckboxModel : PageModel
{
    public bool AgreeToTerms { get; set; } = false;

    public List<ComponentProperty> Properties { get; } = new()
    {
        new("rhx-for", "ModelExpression", "-", "ASP.NET Core model expression for two-way binding"),
        new("name", "string", "-", "The form field name"),
        new("rhx-label", "string", "-", "Label text displayed next to the checkbox"),
        new("rhx-hint", "string", "-", "Hint text displayed below the label"),
        new("rhx-checked", "bool", "false", "Whether the checkbox is checked"),
        new("rhx-indeterminate", "bool", "false", "Shows an indeterminate (mixed) state"),
        new("rhx-size", "string", "medium", "Checkbox size: small, medium, large"),
        new("rhx-disabled", "bool", "false", "Whether the checkbox is disabled"),
    };

    public string BasicCode => @"<rhx-checkbox name=""agree""
               rhx-label=""I agree to the terms and conditions"" />";

    public string PreCheckedCode => @"<rhx-checkbox name=""newsletter""
               rhx-label=""Subscribe to newsletter""
               rhx-checked=""true"" />";

    public string HintCode => @"<rhx-checkbox name=""marketing""
               rhx-label=""Receive marketing emails""
               rhx-hint=""We'll only send relevant updates"" />";

    public string IndeterminateCode => @"<rhx-checkbox name=""select-all""
               rhx-label=""Select all items""
               rhx-indeterminate=""true"" />";

    public string SizesCode => @"<rhx-checkbox name=""cb-sm"" rhx-label=""Small checkbox"" rhx-size=""small"" />
<rhx-checkbox name=""cb-md"" rhx-label=""Medium checkbox (default)"" />
<rhx-checkbox name=""cb-lg"" rhx-label=""Large checkbox"" rhx-size=""large"" />";

    public string StatesCode => @"<rhx-checkbox name=""cb-dis"" rhx-label=""Disabled checkbox""
               rhx-disabled=""true"" />
<rhx-checkbox name=""cb-dis-checked"" rhx-label=""Disabled checked""
               rhx-disabled=""true"" rhx-checked=""true"" />";

    public string BindingCode => @"<rhx-checkbox rhx-for=""AgreeToTerms""
               rhx-label=""Agree to Terms"" />";

    public void OnGet()
    {
        ViewData["Breadcrumbs"] = new List<BreadcrumbItem>
        {
            new("Home", "/"),
            new("Components", "/Docs/Components/Checkbox"),
            new("Checkbox")
        };
    }
}
