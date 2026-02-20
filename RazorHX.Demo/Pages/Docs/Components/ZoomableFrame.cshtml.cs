using Microsoft.AspNetCore.Mvc.RazorPages;
using RazorHX.Components.Navigation;
using RazorHX.Demo.Models;

namespace RazorHX.Demo.Pages.Docs.Components;

public class ZoomableFrameModel : PageModel
{
    public List<ComponentProperty> Properties { get; } = new()
    {
        new("rhx-src", "string", "-", "URL of the image to display"),
        new("rhx-alt", "string", "-", "Alt text for the image"),
        new("rhx-scale", "double", "1", "Initial zoom level"),
        new("rhx-min-scale", "double", "1", "Minimum zoom level"),
        new("rhx-max-scale", "double", "3", "Maximum zoom level"),
    };

    public string ImageCode => @"<div style=""max-width: 600px; height: 400px; overflow: hidden;"">
    <rhx-zoomable-frame
        rhx-src=""https://picsum.photos/id/1015/1200/800""
        rhx-alt=""Mountain landscape""
        rhx-min-scale=""0.5""
        rhx-max-scale=""4"" />
</div>";

    public string PreZoomedCode => @"<div style=""max-width: 600px; height: 400px; overflow: hidden;"">
    <rhx-zoomable-frame
        rhx-src=""https://picsum.photos/id/1015/1200/800""
        rhx-alt=""Mountain landscape zoomed""
        rhx-scale=""2"" />
</div>";

    public void OnGet()
    {
        ViewData["Breadcrumbs"] = new List<BreadcrumbItem>
        {
            new("Home", "/"),
            new("Components", "/Docs/Components/ZoomableFrame"),
            new("Zoomable Frame")
        };
    }
}
