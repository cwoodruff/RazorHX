using Microsoft.AspNetCore.Mvc.RazorPages;
using RazorHX.Components.Navigation;
using RazorHX.Demo.Models;

namespace RazorHX.Demo.Pages.Docs.Components;

public class SwitchModel : PageModel
{
    public bool DarkMode { get; set; } = true;

    public List<ComponentProperty> Properties { get; } = new()
    {
        new("rhx-for", "ModelExpression", "-", "ASP.NET Core model expression for two-way binding"),
        new("name", "string", "-", "The form field name"),
        new("rhx-label", "string", "-", "Label text displayed next to the switch"),
        new("rhx-hint", "string", "-", "Hint text displayed below the label"),
        new("rhx-checked", "bool", "false", "Whether the switch is on"),
        new("rhx-size", "string", "medium", "Switch size: small, medium, large"),
        new("rhx-disabled", "bool", "false", "Whether the switch is disabled"),
    };

    public string BasicCode => @"<rhx-switch name=""darkMode""
             rhx-label=""Dark mode"" />";

    public string PreCheckedCode => @"<rhx-switch name=""notifications""
             rhx-label=""Enable notifications""
             rhx-checked=""true"" />";

    public string HintCode => @"<rhx-switch name=""autoSave""
             rhx-label=""Auto-save""
             rhx-hint=""Automatically save changes every 30 seconds"" />";

    public string SizesCode => @"<rhx-switch name=""sw-sm"" rhx-label=""Small switch"" rhx-size=""small"" />
<rhx-switch name=""sw-md"" rhx-label=""Medium switch (default)"" />
<rhx-switch name=""sw-lg"" rhx-label=""Large switch"" rhx-size=""large"" />";

    public string StatesCode => @"<rhx-switch name=""sw-dis"" rhx-label=""Disabled switch""
             rhx-disabled=""true"" />
<rhx-switch name=""sw-dis-on"" rhx-label=""Disabled on""
             rhx-disabled=""true"" rhx-checked=""true"" />";

    public string BindingCode => @"<rhx-switch rhx-for=""DarkMode""
             rhx-label=""Dark Mode"" />";

    public void OnGet()
    {
        ViewData["Breadcrumbs"] = new List<BreadcrumbItem>
        {
            new("Home", "/"),
            new("Components", "/Docs/Components/Switch"),
            new("Switch")
        };
    }
}
