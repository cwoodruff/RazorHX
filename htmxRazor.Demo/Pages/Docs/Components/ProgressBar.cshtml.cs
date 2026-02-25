using System.Collections.Concurrent;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using htmxRazor.Components.Navigation;
using htmxRazor.Demo.Models;

namespace htmxRazor.Demo.Pages.Docs.Components;

public class ProgressBarModel : PageModel
{
    public List<ComponentProperty> Properties { get; } = new()
    {
        new("rhx-value", "int", "0", "Current progress value (0-100)"),
        new("rhx-label", "string", "-", "Accessible label for the progress bar"),
        new("rhx-indeterminate", "bool", "false", "Shows an indeterminate animation"),
    };

    public string DeterminateCode => @"<rhx-progress-bar rhx-value=""0"" rhx-label=""Not started"" />
<rhx-progress-bar rhx-value=""25"" rhx-label=""Quarter complete"" />
<rhx-progress-bar rhx-value=""65"" rhx-label=""Upload progress"" />
<rhx-progress-bar rhx-value=""100"" rhx-label=""Complete"" />";

    public string IndeterminateCode => @"<rhx-progress-bar rhx-indeterminate=""true"" rhx-label=""Loading"" />";

    private static readonly ConcurrentDictionary<string, int> TaskProgress = new();

    public string HtmxCode => @"<!-- Start button -->
<rhx-button rhx-variant=""brand""
            hx-post=""/Docs/Components/ProgressBar?handler=StartTask""
            hx-target=""#progress-container""
            hx-swap=""innerHTML"">
    Start Task
</rhx-button>

<!-- Server returns a polling element: -->
<div hx-get=""/Docs/Components/ProgressBar?handler=PollProgress&amp;taskId=abc123""
     hx-trigger=""every 500ms""
     hx-target=""#progress-container""
     hx-swap=""innerHTML"">
    <rhx-progress-bar rhx-value=""0"" rhx-label=""Processing"" />
</div>";

    public void OnGet()
    {
        ViewData["Breadcrumbs"] = new List<BreadcrumbItem>
        {
            new("Home", "/"),
            new("Components", "/Docs/Components/ProgressBar"),
            new("Progress Bar")
        };
    }

    public IActionResult OnPostStartTask()
    {
        var taskId = Guid.NewGuid().ToString("N")[..8];
        TaskProgress[taskId] = 0;

        return Content($"""
            <div hx-get="/Docs/Components/ProgressBar?handler=PollProgress&taskId={taskId}"
                 hx-trigger="every 500ms" hx-target="#progress-container" hx-swap="innerHTML">
                <rhx-progress-bar rhx-value="0" rhx-label="Processing" />
                <span style="color: var(--rhx-color-text-muted); font-size: var(--rhx-font-size-sm);">0% complete</span>
            </div>
            """, "text/html");
    }

    public IActionResult OnGetPollProgress(string taskId)
    {
        if (!TaskProgress.TryGetValue(taskId, out var progress))
        {
            return Content("<span style=\"color: var(--rhx-color-text-muted);\">Task not found.</span>", "text/html");
        }

        progress = Math.Min(progress + Random.Shared.Next(8, 20), 100);
        TaskProgress[taskId] = progress;

        if (progress >= 100)
        {
            TaskProgress.TryRemove(taskId, out _);
            return Content("""
                <div>
                    <rhx-progress-bar rhx-value="100" rhx-label="Complete" />
                    <span style="color: var(--rhx-color-success-500); font-size: var(--rhx-font-size-sm); font-weight: var(--rhx-font-weight-medium);">Task complete!</span>
                </div>
                """, "text/html");
        }

        return Content($"""
            <div hx-get="/Docs/Components/ProgressBar?handler=PollProgress&taskId={taskId}"
                 hx-trigger="every 500ms" hx-target="#progress-container" hx-swap="innerHTML">
                <rhx-progress-bar rhx-value="{progress}" rhx-label="Processing" />
                <span style="color: var(--rhx-color-text-muted); font-size: var(--rhx-font-size-sm);">{progress}% complete</span>
            </div>
            """, "text/html");
    }
}
