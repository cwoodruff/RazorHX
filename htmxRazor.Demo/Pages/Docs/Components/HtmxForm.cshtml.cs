using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using htmxRazor.Components.Navigation;
using htmxRazor.Demo.Models;
using htmxRazor.Infrastructure;

namespace htmxRazor.Demo.Pages.Docs.Components;

public class HtmxFormModel : PageModel
{
    public List<ComponentProperty> Properties { get; } =
    [
        new("page", "string", "-", "Razor Page path for form action URL"),
        new("page-handler", "string", "-", "Page handler name for form submission"),
        new("route-*", "string", "-", "Route parameter values for URL generation"),
        new("rhx-method", "string", "post", "HTTP method: post, put, patch, delete"),
        new("rhx-target-422", "string", "-", "CSS selector for 422 validation error responses"),
        new("rhx-target-4xx", "string", "-", "CSS selector for 4xx client error responses"),
        new("rhx-target-5xx", "string", "-", "CSS selector for 5xx server error responses"),
        new("rhx-error-target", "string", "-", "Shorthand: sets all error targets to the same selector"),
        new("rhx-reset-on-success", "bool", "false", "Reset the form on successful submission"),
        new("rhx-disable-on-submit", "bool", "true", "Disable submit buttons during submission"),
        new("rhx-indicator", "string", "-", "CSS selector for the loading indicator element"),
    ];

    [BindProperty]
    public string? Name { get; set; }

    [BindProperty]
    public string? Email { get; set; }

    public string BasicCode => @"<rhx-htmx-form page=""/Docs/Components/HtmxForm"" page-handler=""Contact""
                rhx-error-target=""#form-errors"">
    <rhx-input name=""Name"" rhx-label=""Name"" rhx-required=""true"" />
    <rhx-input name=""Email"" rhx-label=""Email"" type=""email"" />
    <rhx-button type=""submit"" rhx-variant=""brand"">Submit</rhx-button>
</rhx-htmx-form>";

    public string ResetCode => @"<rhx-htmx-form page=""/Docs/Components/HtmxForm"" page-handler=""Message""
                rhx-reset-on-success=""true"">
    <rhx-textarea name=""Message"" rhx-label=""Message"" />
    <rhx-button type=""submit"" rhx-variant=""brand"">Send</rhx-button>
</rhx-htmx-form>";

    public void OnGet()
    {
        ViewData["Breadcrumbs"] = new List<BreadcrumbItem>
        {
            new("Home", "/"),
            new("Components", "/Docs/Components/HtmxForm"),
            new("htmx Form")
        };
    }

    public IActionResult OnPostContact()
    {
        if (string.IsNullOrWhiteSpace(Name))
        {
            return this.HtmxValidationFailure("_ContactFormErrors", this);
        }

        return Content($"<rhx-callout rhx-variant=\"success\">Thanks, {Name}! Form submitted.</rhx-callout>", "text/html");
    }

    public IActionResult OnPostMessage()
    {
        return Content("<rhx-callout rhx-variant=\"success\">Message sent!</rhx-callout>", "text/html");
    }
}
