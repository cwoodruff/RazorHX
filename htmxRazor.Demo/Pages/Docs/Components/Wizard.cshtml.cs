using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using htmxRazor.Components.Navigation;
using htmxRazor.Demo.Models;

namespace htmxRazor.Demo.Pages.Docs.Components;

public class WizardModel : PageModel
{
    public List<ComponentProperty> Properties { get; } =
    [
        new("rhx-current-step", "int", "1", "The current step number (1-based)"),
        new("rhx-total-steps", "int", "0", "Total steps. When 0, auto-counted from children"),
        new("rhx-layout", "string", "horizontal", "Stepper layout: horizontal, vertical"),
        new("rhx-show-nav", "bool", "true", "Show prev/next navigation buttons"),
        new("rhx-linear", "bool", "true", "Steps must be completed in order"),
        new("page", "string", "-", "Razor Page path for wizard navigation URLs"),
        new("page-handler-prev", "string", "WizardPrev", "Page handler for the Previous button"),
        new("page-handler-next", "string", "WizardNext", "Page handler for the Next button"),
        new("page-handler-step", "string", "WizardStep", "Page handler for clicking a step indicator"),
    ];

    public List<ComponentProperty> StepProperties { get; } =
    [
        new("rhx-title", "string", "-", "Display title shown in the stepper indicator"),
        new("rhx-description", "string", "-", "Description text shown below the title"),
        new("rhx-status", "string", "incomplete", "Step status: incomplete, current, complete, error"),
        new("rhx-optional", "bool", "false", "Whether this step can be skipped"),
    ];

    public int CurrentStep { get; set; } = 1;

    public string BasicCode => @"<rhx-wizard rhx-current-step=""1"" page=""/Docs/Components/Wizard"">
    <rhx-wizard-step rhx-title=""Account"" rhx-status=""current"">
        <h3>Create Account</h3>
        <rhx-input name=""username"" rhx-label=""Username"" />
    </rhx-wizard-step>
    <rhx-wizard-step rhx-title=""Profile"" rhx-description=""Personal info"">
        <h3>Your Profile</h3>
        <rhx-input name=""fullName"" rhx-label=""Full Name"" />
    </rhx-wizard-step>
    <rhx-wizard-step rhx-title=""Confirm"">
        <h3>Review &amp; Confirm</h3>
        <p>Please review your information before submitting.</p>
    </rhx-wizard-step>
</rhx-wizard>";

    public string VerticalCode => @"<rhx-wizard rhx-current-step=""2"" rhx-layout=""vertical"" page=""/Docs/Components/Wizard"">
    <rhx-wizard-step rhx-title=""Basics"" rhx-status=""complete"">...</rhx-wizard-step>
    <rhx-wizard-step rhx-title=""Details"" rhx-status=""current"">...</rhx-wizard-step>
    <rhx-wizard-step rhx-title=""Review"">...</rhx-wizard-step>
</rhx-wizard>";

    public void OnGet(int step = 1)
    {
        CurrentStep = step;

        ViewData["Breadcrumbs"] = new List<BreadcrumbItem>
        {
            new("Home", "/"),
            new("Components", "/Docs/Components/Wizard"),
            new("Wizard")
        };
    }

    public IActionResult OnGetWizardPrev(int step = 1)
    {
        return Partial("_WizardPartial", step);
    }

    public IActionResult OnPostWizardNext(int step = 2)
    {
        return Partial("_WizardPartial", step);
    }

    public IActionResult OnGetWizardStep(int step = 1)
    {
        return Partial("_WizardPartial", step);
    }
}
