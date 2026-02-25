using Microsoft.AspNetCore.Mvc.RazorPages;
using htmxRazor.Components.Navigation;
using htmxRazor.Demo.Models;

namespace htmxRazor.Demo.Pages.Docs.Components;

public class AvatarModel : PageModel
{
    public List<ComponentProperty> Properties { get; } = new()
    {
        new("rhx-image", "string", "-", "URL of the avatar image"),
        new("rhx-initials", "string", "-", "One or two letter initials (used when no image)"),
        new("rhx-label", "string", "-", "Accessible label (required)"),
        new("rhx-size", "string", "medium", "Size: small, medium, large, or any CSS value"),
        new("rhx-shape", "string", "circle", "Shape: circle, rounded, square"),
    };

    public string ImagesCode => @"<rhx-avatar rhx-image=""https://i.pravatar.cc/150?u=a""
             rhx-label=""Alice"" rhx-size=""small"" />
<rhx-avatar rhx-image=""https://i.pravatar.cc/150?u=b""
             rhx-label=""Bob"" />
<rhx-avatar rhx-image=""https://i.pravatar.cc/150?u=c""
             rhx-label=""Charlie"" rhx-size=""large"" />";

    public string InitialsCode => @"<rhx-avatar rhx-initials=""JD"" rhx-label=""John Doe"" />
<rhx-avatar rhx-initials=""AB"" rhx-label=""Alice Brown"" />
<rhx-avatar rhx-initials=""KS"" rhx-label=""Karen Smith"" />
<rhx-avatar rhx-initials=""MR"" rhx-label=""Mike Ross"" />
<rhx-avatar rhx-initials=""TN"" rhx-label=""Tina Nguyen"" />
<rhx-avatar rhx-initials=""LP"" rhx-label=""Liam Parker"" />";

    public string ShapesCode => @"<rhx-avatar rhx-initials=""JD"" rhx-label=""Circle"" rhx-shape=""circle"" />
<rhx-avatar rhx-initials=""JD"" rhx-label=""Rounded"" rhx-shape=""rounded"" />
<rhx-avatar rhx-initials=""JD"" rhx-label=""Square"" rhx-shape=""square"" />";

    public string CustomSizeCode => @"<rhx-avatar rhx-initials=""XL"" rhx-label=""Extra large avatar"" rhx-size=""5rem"" />";

    public void OnGet()
    {
        ViewData["Breadcrumbs"] = new List<BreadcrumbItem>
        {
            new("Home", "/"),
            new("Components", "/Docs/Components/Avatar"),
            new("Avatar")
        };
    }
}
