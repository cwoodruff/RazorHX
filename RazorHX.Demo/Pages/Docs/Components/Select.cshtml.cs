using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using RazorHX.Components.Navigation;
using RazorHX.Demo.Models;

namespace RazorHX.Demo.Pages.Docs.Components;

public class SelectModel : PageModel
{
    public List<SelectListItem> Countries { get; } = new()
    {
        new("United States", "US"),
        new("Canada", "CA"),
        new("United Kingdom", "UK"),
        new("Germany", "DE"),
        new("France", "FR"),
        new("Japan", "JP"),
        new("Australia", "AU"),
        new SelectListItem { Text = "Antarctica", Value = "AQ", Disabled = true }
    };

    public List<SelectListItem> Cities { get; } = new()
    {
        new("New York", "NYC"),
        new("Los Angeles", "LA"),
        new("Chicago", "CHI"),
        new("Houston", "HOU"),
        new("Phoenix", "PHX")
    };

    public enum Priority
    {
        [Display(Name = "Low Priority")]
        Low,
        [Display(Name = "Medium Priority")]
        Medium,
        [Display(Name = "High Priority")]
        High,
        [Display(Name = "Critical")]
        Critical
    }

    public Priority SelectedPriority { get; set; } = Priority.Medium;

    public List<ComponentProperty> Properties { get; } = new()
    {
        new("rhx-for", "ModelExpression", "-", "ASP.NET Core model expression for two-way binding"),
        new("name", "string", "-", "The form field name"),
        new("value", "string", "-", "The current value"),
        new("rhx-label", "string", "-", "Label text displayed above the select"),
        new("rhx-hint", "string", "-", "Hint text displayed below the select"),
        new("rhx-size", "string", "medium", "Select size: small, medium, large"),
        new("rhx-disabled", "bool", "false", "Whether the select is disabled"),
        new("rhx-required", "bool", "false", "Whether the field is required"),
        new("rhx-placeholder", "string", "-", "Placeholder text when no selection"),
        new("rhx-items", "List<SelectListItem>", "-", "Server-side items to render as options"),
        new("rhx-multiple", "bool", "false", "Allow multiple selections"),
        new("rhx-with-clear", "bool", "false", "Show a clear button when a value is selected"),
        new("rhx-filled", "bool", "false", "Use filled appearance"),
    };

    public string BasicCode => @"<rhx-select rhx-label=""Country"" name=""country""
           rhx-placeholder=""Choose a country""
           rhx-items=""Model.Countries"" />";

    public string EnumBindingCode => @"<rhx-select rhx-label=""Priority"" name=""priority""
           value=""Medium"" rhx-for=""SelectedPriority"" />";

    public string WithClearCode => @"<rhx-select rhx-label=""Country"" name=""country-clear""
           value=""US"" rhx-with-clear=""true""
           rhx-items=""Model.Countries"" />";

    public string ChildOptionsCode => @"<rhx-select rhx-label=""Color"" name=""color""
           rhx-placeholder=""Pick a color"">
    <rhx-option value=""red"">Red</rhx-option>
    <rhx-option value=""green"">Green</rhx-option>
    <rhx-option value=""blue"">Blue</rhx-option>
    <rhx-option value=""purple"" rhx-disabled=""true"">Purple (disabled)</rhx-option>
</rhx-select>";

    public string MultiSelectCode => @"<rhx-select rhx-label=""Tags"" name=""tags""
           rhx-multiple=""true"" rhx-placeholder=""Select tags"">
    <rhx-option value=""bug"">Bug</rhx-option>
    <rhx-option value=""feature"">Feature</rhx-option>
    <rhx-option value=""docs"">Documentation</rhx-option>
    <rhx-option value=""perf"">Performance</rhx-option>
</rhx-select>";

    public string SizesCode => @"<rhx-select rhx-label=""Small"" rhx-size=""small""
           rhx-placeholder=""Small"" rhx-items=""Model.Cities"" name=""sel-sm"" />
<rhx-select rhx-label=""Medium (default)""
           rhx-placeholder=""Medium"" rhx-items=""Model.Cities"" name=""sel-md"" />
<rhx-select rhx-label=""Large"" rhx-size=""large""
           rhx-placeholder=""Large"" rhx-items=""Model.Cities"" name=""sel-lg"" />";

    public void OnGet()
    {
        ViewData["Breadcrumbs"] = new List<BreadcrumbItem>
        {
            new("Home", "/"),
            new("Components", "/Docs/Components/Select"),
            new("Select")
        };
    }
}
