using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RazorHX.Components.Navigation;
using RazorHX.Demo.Models;

namespace RazorHX.Demo.Pages.Docs.Components;

public class InputModel : PageModel
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
        new("rhx-readonly", "bool", "false", "Whether the input is read-only"),
        new("rhx-required", "bool", "false", "Whether the field is required"),
        new("rhx-placeholder", "string", "-", "Placeholder text"),
        new("rhx-type", "string", "text", "Input type: text, email, password, number, tel, url, search, date"),
        new("rhx-with-clear", "bool", "false", "Show a clear button when the input has a value"),
        new("rhx-password-toggle", "bool", "false", "Show a password visibility toggle"),
        new("rhx-filled", "bool", "false", "Use filled appearance (background instead of border)"),
        new("rhx-pattern", "string", "-", "Regex pattern for validation"),
        new("rhx-minlength", "int?", "-", "Minimum text length"),
        new("rhx-maxlength", "int?", "-", "Maximum text length"),
        new("rhx-min", "string", "-", "Minimum value for number/date types"),
        new("rhx-max", "string", "-", "Maximum value for number/date types"),
        new("rhx-step", "string", "-", "Step increment for number types"),
        new("rhx-autocomplete", "string", "-", "Autocomplete hint (e.g. email, off)"),
        new("rhx-autofocus", "bool", "false", "Whether the input receives focus on page load"),
        new("aria-label", "string", "-", "Accessible label for the native input"),
    };

    public string BasicCode => @"<rhx-input rhx-label=""Full Name""
           rhx-placeholder=""Enter your name""
           name=""name"" />";

    public string TypesCode => @"<rhx-input rhx-label=""Email"" rhx-type=""email""
           rhx-placeholder=""name@example.com"" name=""email"" />
<rhx-input rhx-label=""Password"" rhx-type=""password""
           rhx-placeholder=""Enter password"" name=""password"" />
<rhx-input rhx-label=""Number"" rhx-type=""number""
           rhx-placeholder=""0"" name=""number"" />";

    public string ClearCode => @"<rhx-input rhx-label=""Search""
           rhx-placeholder=""Type to search...""
           rhx-with-clear=""true""
           name=""search"" />";

    public string PasswordToggleCode => @"<rhx-input rhx-label=""Password""
           rhx-type=""password""
           rhx-password-toggle=""true""
           name=""pw"" />";

    public string SizesCode => @"<rhx-input rhx-label=""Small"" rhx-size=""small"" name=""sm"" />
<rhx-input rhx-label=""Medium"" name=""md"" />
<rhx-input rhx-label=""Large"" rhx-size=""large"" name=""lg"" />";

    public string StatesCode => @"<rhx-input rhx-label=""Disabled"" rhx-disabled=""true""
           value=""Cannot edit"" name=""disabled"" />
<rhx-input rhx-label=""Readonly"" rhx-readonly=""true""
           value=""Read only value"" name=""readonly"" />";

    public string HtmxSearchCode => @"<rhx-input rhx-label=""Live Search""
           rhx-placeholder=""Type to search...""
           rhx-with-clear=""true""
           name=""q""
           hx-get=""/Docs/Components/Input?handler=Search""
           hx-trigger=""input changed delay:300ms""
           hx-target=""#search-results"" />
<div id=""search-results"">Results will appear here...</div>";

    public void OnGet()
    {
        ViewData["Breadcrumbs"] = new List<BreadcrumbItem>
        {
            new("Home", "/"),
            new("Components", "/Docs/Components/Input"),
            new("Input")
        };
    }

    public IActionResult OnGetSearch(string? q)
    {
        if (string.IsNullOrWhiteSpace(q))
        {
            return Content("<span style=\"color: var(--rhx-color-text-muted);\">Results will appear here...</span>", "text/html");
        }

        var html = $"""
            <ul style="list-style: none; padding: 0; margin: 0;">
                <li style="padding: var(--rhx-space-xs) 0;">Found: <strong>{System.Net.WebUtility.HtmlEncode(q)}</strong> component</li>
                <li style="padding: var(--rhx-space-xs) 0;">Found: <strong>{System.Net.WebUtility.HtmlEncode(q)}-input</strong> variant</li>
                <li style="padding: var(--rhx-space-xs) 0;">Found: <strong>{System.Net.WebUtility.HtmlEncode(q)}-demo</strong> example</li>
            </ul>
            """;

        return Content(html, "text/html");
    }
}
