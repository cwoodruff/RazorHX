using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace RazorHX.Demo.Pages;

public class TogglesModel : PageModel
{
    public bool AgreeToTerms { get; set; } = false;

    public bool DarkMode { get; set; } = true;

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

    public List<SelectListItem> FrequencyOptions { get; } = new()
    {
        new("Daily", "daily"),
        new("Weekly", "weekly"),
        new("Monthly", "monthly"),
        new("Never", "never")
    };

    public List<SelectListItem> PlanOptions { get; } = new()
    {
        new("Free", "free"),
        new("Pro", "pro"),
        new("Enterprise", "enterprise")
    };

    public void OnGet()
    {
    }
}
