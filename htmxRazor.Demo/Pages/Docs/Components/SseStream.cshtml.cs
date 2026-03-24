using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using htmxRazor.Components.Navigation;
using htmxRazor.Demo.Models;
using htmxRazor.Infrastructure;

namespace htmxRazor.Demo.Pages.Docs.Components;

public class SseStreamModel : PageModel
{
    public List<ComponentProperty> Properties { get; } =
    [
        new("rhx-url", "string", "-", "Explicit URL for the SSE endpoint"),
        new("page", "string", "-", "Razor Page path for SSE endpoint URL generation"),
        new("page-handler", "string", "-", "Page handler name for URL generation"),
        new("route-*", "string", "-", "Route parameter values for URL generation"),
        new("rhx-event", "string", "message", "SSE event name to listen for"),
        new("rhx-swap", "string", "innerHTML", "How SSE data is swapped into the container"),
        new("rhx-close-on", "string", "-", "SSE event name that triggers closing the connection"),
        new("rhx-reconnect", "bool", "true", "Whether to auto-reconnect on connection loss"),
    ];

    public string BasicCode => @"<rhx-sse-stream rhx-url=""/Docs/Components/SseStream?handler=Clock""
                 rhx-event=""clock"">
    <rhx-spinner />
</rhx-sse-stream>";

    public string CloseCode => @"<rhx-sse-stream page=""/Docs/Components/SseStream"" page-handler=""Countdown""
                 rhx-event=""tick"" rhx-close-on=""done"">
    Waiting for countdown...
</rhx-sse-stream>";

    public void OnGet()
    {
        ViewData["Breadcrumbs"] = new List<BreadcrumbItem>
        {
            new("Home", "/"),
            new("Patterns", "/Patterns"),
            new("SSE Stream")
        };
    }

    public async Task<IActionResult> OnGetClock(CancellationToken cancellationToken)
    {
        Response.PrepareSseResponse();

        for (var i = 0; i < 10 && !cancellationToken.IsCancellationRequested; i++)
        {
            var time = DateTime.Now.ToString("HH:mm:ss");
            await Response.WriteSseEventAsync($"<strong>{time}</strong>", "clock");
            await Task.Delay(1000, cancellationToken);
        }

        return new EmptyResult();
    }

    public async Task<IActionResult> OnGetCountdown(CancellationToken cancellationToken)
    {
        Response.PrepareSseResponse();

        for (var i = 5; i >= 0 && !cancellationToken.IsCancellationRequested; i--)
        {
            await Response.WriteSseEventAsync($"<span>Countdown: {i}</span>", "tick");
            await Task.Delay(1000, cancellationToken);
        }

        if (!cancellationToken.IsCancellationRequested)
            await Response.WriteSseEventAsync("<span>Done!</span>", "done");

        return new EmptyResult();
    }
}
