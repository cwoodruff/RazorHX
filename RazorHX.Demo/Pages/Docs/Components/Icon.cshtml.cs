using Microsoft.AspNetCore.Mvc.RazorPages;
using RazorHX.Components.Navigation;
using RazorHX.Demo.Models;

namespace RazorHX.Demo.Pages.Docs.Components;

public class IconModel : PageModel
{
    public List<ComponentProperty> Properties { get; } = new()
    {
        new("rhx-name", "string", "-", "Icon name from the built-in registry"),
        new("rhx-size", "string", "medium", "Icon size: small (16px), medium (24px), large (32px)"),
        new("rhx-label", "string", "-", "Accessible label (sets aria-label and role=\"img\")"),
    };

    public string GalleryCode => @"@foreach (var name in IconRegistry.GetNames().OrderBy(n => n).Take(24))
{
    <rhx-icon rhx-name=""@name"" />
}";

    public string SizesCode => @"<rhx-icon rhx-name=""star"" rhx-size=""small"" />
<rhx-icon rhx-name=""star"" />
<rhx-icon rhx-name=""star"" rhx-size=""large"" />";

    public string AccessibleCode => @"<rhx-icon rhx-name=""settings"" rhx-label=""Settings"" />
<rhx-icon rhx-name=""bell"" rhx-label=""Notifications"" />
<rhx-icon rhx-name=""user"" rhx-label=""User profile"" />";

    public void OnGet()
    {
        ViewData["Breadcrumbs"] = new List<BreadcrumbItem>
        {
            new("Home", "/"),
            new("Components", "/Docs/Components/Icon"),
            new("Icon")
        };
    }
}
