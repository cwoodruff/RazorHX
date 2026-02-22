using Microsoft.AspNetCore.Mvc;
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

    public string HtmxCode => @"<rhx-number-input rhx-label=""Quantity"" name=""quantity""
           value=""1"" rhx-min=""1"" rhx-max=""20"" rhx-step=""1""
           hx-get=""/Docs/Components/NumberInput?handler=Calculate""
           hx-trigger=""change""
           hx-target=""#calc-result""
           hx-include=""this"" />
<div id=""calc-result"">Total: $29.99</div>";

    public void OnGet()
    {
        ViewData["Breadcrumbs"] = new List<BreadcrumbItem>
        {
            new("Home", "/"),
            new("Components", "/Docs/Components/NumberInput"),
            new("Number Input")
        };
    }

    public IActionResult OnGetCalculate(int quantity)
    {
        if (quantity < 1) quantity = 1;
        var unitPrice = 29.99m;
        var discount = quantity >= 5 ? 0.10m : 0m;
        var total = unitPrice * quantity * (1 - discount);
        var html = discount > 0
            ? $"<strong>Total: ${total:F2}</strong> (${unitPrice}/ea &times; {quantity}, <span style=\"color: var(--rhx-color-success-500);\">10% bulk discount!</span>)"
            : $"<strong>Total: ${total:F2}</strong> (${unitPrice}/ea &times; {quantity})";
        return Content($"<span style=\"color: var(--rhx-color-text-muted);\">{html}</span>", "text/html");
    }
}
