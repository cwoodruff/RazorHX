using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using htmxRazor.Components.Navigation;
using htmxRazor.Demo.Models;

namespace htmxRazor.Demo.Pages.Docs.Components;

public class OptimisticModel : PageModel
{
    public List<ComponentProperty> Properties { get; } =
    [
        new("rhx-optimistic", "bool", "false", "Enable optimistic UI on rhx-switch, rhx-rating, or rhx-button"),
    ];

    public string SwitchCode => @"<rhx-switch name=""darkMode"" rhx-label=""Dark mode""
            rhx-optimistic=""true""
            hx-post=""/api/settings/darkMode""
            hx-trigger=""change"" hx-swap=""none"" />";

    public string RatingCode => @"<rhx-rating name=""score"" rhx-label=""Rate this""
             rhx-optimistic=""true""
             hx-post=""/api/rating""
             hx-trigger=""change"" hx-swap=""none"" />";

    public string ButtonCode => @"<rhx-button rhx-variant=""brand"" rhx-optimistic=""true""
            hx-post=""/api/like""
            hx-swap=""none"">
    Like
</rhx-button>";

    public void OnGet()
    {
        ViewData["Breadcrumbs"] = new List<BreadcrumbItem>
        {
            new("Home", "/"),
            new("Patterns", "/Patterns"),
            new("Optimistic UI")
        };
    }

    public IActionResult OnPostToggle()
    {
        return new StatusCodeResult(200);
    }

    public IActionResult OnPostToggleError()
    {
        return new StatusCodeResult(500);
    }

    public IActionResult OnPostRate()
    {
        return new StatusCodeResult(200);
    }

    public IActionResult OnPostLike()
    {
        return new StatusCodeResult(200);
    }
}
