using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using htmxRazor.Components.Navigation;
using htmxRazor.Demo.Models;

namespace htmxRazor.Demo.Pages.Docs.Components;

public class RadioGroupModel : PageModel
{
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

    public List<ComponentProperty> Properties { get; } = new()
    {
        new("rhx-for", "ModelExpression", "-", "ASP.NET Core model expression for two-way binding"),
        new("name", "string", "-", "The form field name for all radios in the group"),
        new("value", "string", "-", "The currently selected value"),
        new("rhx-label", "string", "-", "Group label text"),
        new("rhx-hint", "string", "-", "Hint text displayed below the group label"),
        new("rhx-items", "List<SelectListItem>", "-", "Server-side items to render as radio buttons"),
        new("rhx-inline", "bool", "false", "Display radio buttons in a horizontal row"),
        new("rhx-size", "string", "medium", "Radio size: small, medium, large"),
        new("rhx-disabled", "bool", "false", "Whether the entire group is disabled"),
    };

    public string BasicCode => @"<rhx-radio-group rhx-label=""Notification Frequency""
                   name=""frequency""
                   rhx-items=""Model.FrequencyOptions"" />";

    public string PreSelectedCode => @"<rhx-radio-group rhx-label=""Notification Frequency""
                   name=""frequency-pre""
                   value=""weekly""
                   rhx-items=""Model.FrequencyOptions"" />";

    public string EnumBindingCode => @"<rhx-radio-group rhx-label=""Priority""
                   name=""priority""
                   rhx-for=""SelectedPriority"" />";

    public string InlineCode => @"<rhx-radio-group rhx-label=""Size"" name=""size""
                   rhx-inline=""true"" value=""md"">
    <rhx-radio value=""sm"">Small</rhx-radio>
    <rhx-radio value=""md"">Medium</rhx-radio>
    <rhx-radio value=""lg"">Large</rhx-radio>
    <rhx-radio value=""xl"">Extra Large</rhx-radio>
</rhx-radio-group>";

    public string HintCode => @"<rhx-radio-group rhx-label=""Plan"" name=""plan""
                   rhx-hint=""You can change your plan at any time""
                   rhx-items=""Model.PlanOptions"" />";

    public string ChildOptionsCode => @"<rhx-radio-group rhx-label=""Color"" name=""color"">
    <rhx-radio value=""red"">Red</rhx-radio>
    <rhx-radio value=""green"">Green</rhx-radio>
    <rhx-radio value=""blue"">Blue</rhx-radio>
    <rhx-radio value=""purple"" rhx-disabled=""true"">Purple (disabled)</rhx-radio>
</rhx-radio-group>";

    public string StatesCode => @"<!-- Disabled group -->
<rhx-radio-group rhx-label=""Disabled Group"" name=""dis-group""
                   rhx-disabled=""true"" value=""weekly""
                   rhx-items=""Model.FrequencyOptions"" />

<!-- Small size -->
<rhx-radio-group rhx-label=""Priority"" name=""priority-sm""
                   rhx-size=""small""
                   rhx-for=""SelectedPriority"" />";

    public string HtmxCode => @"<rhx-radio-group rhx-label=""Pricing Plan"" name=""plan""
                   rhx-inline=""true""
                   hx-get=""/Docs/Components/RadioGroup?handler=PlanDetails""
                   hx-trigger=""change""
                   hx-target=""#plan-details""
                   hx-include=""this"">
    <rhx-radio value=""free"">Free</rhx-radio>
    <rhx-radio value=""pro"">Pro</rhx-radio>
    <rhx-radio value=""enterprise"">Enterprise</rhx-radio>
</rhx-radio-group>
<div id=""plan-details"">Select a plan to see details...</div>";

    public void OnGet()
    {
        ViewData["Breadcrumbs"] = new List<BreadcrumbItem>
        {
            new("Home", "/"),
            new("Components", "/Docs/Components/RadioGroup"),
            new("Radio Group")
        };
    }

    public IActionResult OnGetPlanDetails(string? plan)
    {
        var (name, price, features) = plan switch
        {
            "free" => ("Free", "$0/mo", new[] { "5 projects", "1 GB storage", "Community support" }),
            "pro" => ("Pro", "$19/mo", new[] { "Unlimited projects", "100 GB storage", "Priority support", "Advanced analytics" }),
            "enterprise" => ("Enterprise", "$99/mo", new[] { "Unlimited everything", "1 TB storage", "24/7 dedicated support", "SSO & SAML", "Custom integrations" }),
            _ => ("", "", Array.Empty<string>())
        };

        if (string.IsNullOrEmpty(name))
        {
            return Content("<span style=\"color: var(--rhx-color-text-muted);\">Select a plan to see details...</span>", "text/html");
        }

        var featureItems = string.Join("", features.Select(f => $"<li style=\"padding: var(--rhx-space-xs) 0;\">{f}</li>"));
        var html = $"""
            <div style="color: var(--rhx-color-text-muted);">
                <strong>{name}</strong> &mdash; {price}
                <ul style="list-style: disc; padding-left: var(--rhx-space-lg); margin: var(--rhx-space-xs) 0 0 0;">{featureItems}</ul>
            </div>
            """;

        return Content(html, "text/html");
    }
}
