using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using RazorHX.Components.Navigation;

namespace RazorHX.Demo.Pages.Docs.Components;

public class SelectsModel : PageModel
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

    public void OnGet()
    {
        ViewData["Breadcrumbs"] = new List<BreadcrumbItem>
        {
            new("Home", "/"),
            new("Components", "/Docs/Components/Selects"),
            new("Select")
        };
    }
}
