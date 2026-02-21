using Microsoft.AspNetCore.Mvc.RazorPages;
using RazorHX.Components.Navigation;
using RazorHX.Demo.Models;

namespace RazorHX.Demo.Pages.Docs.Components;

public class AnimationModel : PageModel
{
    public List<ComponentProperty> Properties { get; } = new()
    {
        new("rhx-name", "string", "fadeIn", "Animation name: fadeIn, fadeInUp, slideInLeft, slideInRight, slideInUp, bounceIn, zoomIn, pulse, bounce, spin"),
        new("rhx-duration", "int", "300", "Animation duration in milliseconds"),
        new("rhx-delay", "int", "0", "Delay before animation starts in milliseconds"),
        new("rhx-easing", "string", "ease", "CSS easing function"),
        new("rhx-iterations", "string", "1", "Number of iterations or \"infinite\""),
        new("rhx-play", "bool", "true", "Whether the animation starts automatically"),
    };

    public string FadeInCode => @"<rhx-animation rhx-name=""fadeIn"">
    <div>Faded in content</div>
</rhx-animation>";

    public string SlideInCode => @"<rhx-animation rhx-name=""slideInLeft"" rhx-duration=""500"">
    <div>Slide in from left</div>
</rhx-animation>

<rhx-animation rhx-name=""slideInRight"" rhx-duration=""500"" rhx-delay=""200"">
    <div>Slide in from right (200ms delay)</div>
</rhx-animation>

<rhx-animation rhx-name=""slideInUp"" rhx-duration=""500"" rhx-delay=""400"">
    <div>Slide in from below (400ms delay)</div>
</rhx-animation>";

    public string BounceZoomCode => @"<rhx-animation rhx-name=""bounceIn"" rhx-duration=""600"">
    <div>Bounce in</div>
</rhx-animation>

<rhx-animation rhx-name=""zoomIn"" rhx-duration=""600"">
    <div>Zoom in</div>
</rhx-animation>";

    public string ContinuousCode => @"<rhx-animation rhx-name=""pulse"" rhx-iterations=""infinite"">
    <div>Pulse</div>
</rhx-animation>

<rhx-animation rhx-name=""bounce"" rhx-iterations=""infinite"">
    <div>Bounce</div>
</rhx-animation>

<rhx-animation rhx-name=""spin"" rhx-iterations=""infinite"" rhx-duration=""1000"">
    <div>Spinning element</div>
</rhx-animation>";

    public string PausedCode => @"<rhx-animation rhx-name=""spin"" rhx-iterations=""infinite""
               rhx-duration=""1000"" rhx-play=""false"" id=""paused-anim"">
    <div>Paused until played</div>
</rhx-animation>

<button onclick=""
    var el = document.getElementById('paused-anim');
    var state = el.style.animationPlayState;
    if (state === 'running') {
        el.style.animationPlayState = 'paused';
        this.textContent = 'Play';
    } else {
        el.style.animationPlayState = 'running';
        this.textContent = 'Pause';
    }
"">Play</button>";

    public void OnGet()
    {
        ViewData["Breadcrumbs"] = new List<BreadcrumbItem>
        {
            new("Home", "/"),
            new("Components", "/Docs/Components/Animation"),
            new("Animation")
        };
    }
}
