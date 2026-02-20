using Microsoft.AspNetCore.Mvc.RazorPages;
using RazorHX.Components.Navigation;
using RazorHX.Demo.Models;

namespace RazorHX.Demo.Pages.Docs.Components;

public class NumberInputModel : PageModel
{
    public List<ComponentProperty> Properties { get; } = new()
    {
        new("rhx-for", "ModelExpression", "-", "ASP.NET Core model expression for two-way binding"),
        new("name", "string", "-", "The form field name"),
        new("value", "string", "-", "The current value"),
        new("rhx-label", "string", "-", "Label text displayed above the input"),
        new("rhx-hint", "string", "-", "Hint text displayed below the input"),
        new("rhx-size", "string", "medium", "Input size: small, medium, large"),
        new("rhx-disabled", "bool", "false", "Whether the input is disabled"),
        new("rhx-min", "string", "-", "Minimum allowed value"),
        new("rhx-max", "string", "-", "Maximum allowed value"),
        new("rhx-step", "string", "1", "Step increment for the stepper buttons"),
        new("rhx-no-steppers", "bool", "false", "Hide the increment/decrement buttons"),
    };

    public string BasicCode => @"<rhx-number-input rhx-label=""Quantity"" name=""quantity""
           value=""1"" rhx-min=""0"" rhx-max=""99"" rhx-step=""1"" />";

    public string DecimalStepCode => @"<rhx-number-input rhx-label=""Price"" name=""price""
           value=""9.99"" rhx-min=""0"" rhx-step=""0.01"" />";

    public string NoSteppersCode => @"<rhx-number-input rhx-label=""Amount"" name=""amount""
           value=""100"" rhx-no-steppers=""true"" />";

    public string SizesCode => @"<rhx-number-input rhx-label=""Small"" rhx-size=""small""
           name=""num-sm"" value=""5"" rhx-min=""0"" rhx-max=""10"" />
<rhx-number-input rhx-label=""Medium"" name=""num-md""
           value=""5"" rhx-min=""0"" rhx-max=""10"" />
<rhx-number-input rhx-label=""Large"" rhx-size=""large""
           name=""num-lg"" value=""5"" rhx-min=""0"" rhx-max=""10"" />";

    public string StatesCode => @"<rhx-number-input rhx-label=""Disabled"" name=""num-dis""
           value=""3"" rhx-disabled=""true"" />";

    public void OnGet()
    {
        ViewData["Breadcrumbs"] = new List<BreadcrumbItem>
        {
            new("Home", "/"),
            new("Components", "/Docs/Components/NumberInput"),
            new("Number Input")
        };
    }
}
