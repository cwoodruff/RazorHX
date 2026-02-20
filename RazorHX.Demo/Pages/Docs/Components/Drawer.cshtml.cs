using Microsoft.AspNetCore.Mvc.RazorPages;
using RazorHX.Components.Navigation;
using RazorHX.Demo.Models;

namespace RazorHX.Demo.Pages.Docs.Components;

public class DrawerModel : PageModel
{
    public List<ComponentProperty> Properties { get; } = new()
    {
        new("id", "string", "-", "Element ID used for open/close triggers"),
        new("rhx-label", "string", "-", "Title text displayed in the drawer header"),
        new("rhx-placement", "string", "end", "Slide direction: start, end, top, bottom"),
        new("rhx-contained", "bool", "false", "Position relative to parent instead of viewport"),
        new("rhx-no-header", "bool", "false", "Hides the default header"),
    };

    public string EndCode => @"<rhx-button rhx-variant=""brand""
            data-rhx-drawer-open=""end-drawer"">
    Open End Drawer
</rhx-button>

<rhx-drawer id=""end-drawer"" rhx-label=""Notifications"">
    <p>You have no new notifications.</p>
    <p>Check back later for updates.</p>
    <rhx-drawer-footer>
        <rhx-button rhx-variant=""ghost""
                    data-rhx-drawer-close=""end-drawer"">
            Close
        </rhx-button>
    </rhx-drawer-footer>
</rhx-drawer>";

    public string StartCode => @"<rhx-button rhx-variant=""outline""
            data-rhx-drawer-open=""start-drawer"">
    Open Start Drawer
</rhx-button>

<rhx-drawer id=""start-drawer"" rhx-label=""Navigation""
            rhx-placement=""start"">
    <nav style=""display: flex; flex-direction: column; gap: var(--rhx-space-sm);"">
        <a href=""#"">Dashboard</a>
        <a href=""#"">Settings</a>
        <a href=""#"">Profile</a>
        <a href=""#"">Help</a>
    </nav>
</rhx-drawer>";

    public string TopBottomCode => @"<rhx-button rhx-variant=""outline""
            data-rhx-drawer-open=""top-drawer"">
    Open Top Drawer
</rhx-button>
<rhx-button rhx-variant=""outline""
            data-rhx-drawer-open=""bottom-drawer"">
    Open Bottom Drawer
</rhx-button>

<rhx-drawer id=""top-drawer"" rhx-label=""Alert Banner""
            rhx-placement=""top"">
    <p>This drawer slides down from the top of the viewport.</p>
</rhx-drawer>

<rhx-drawer id=""bottom-drawer"" rhx-label=""Cookie Consent""
            rhx-placement=""bottom"">
    <p>We use cookies to improve your experience.</p>
    <rhx-drawer-footer>
        <rhx-button rhx-variant=""ghost""
                    data-rhx-drawer-close=""bottom-drawer"">
            Decline
        </rhx-button>
        <rhx-button rhx-variant=""brand""
                    data-rhx-drawer-close=""bottom-drawer"">
            Accept
        </rhx-button>
    </rhx-drawer-footer>
</rhx-drawer>";

    public string ContainedCode => @"<div style=""position: relative; height: 300px;
     border: var(--rhx-border-width) solid var(--rhx-color-border);
     border-radius: var(--rhx-radius-md); overflow: hidden;"">
    <div style=""padding: var(--rhx-space-lg);"">
        <rhx-button rhx-variant=""outline"" rhx-size=""small""
                    data-rhx-drawer-open=""contained-drawer"">
            Toggle Sidebar
        </rhx-button>
        <p>Main content area. The drawer opens within this container.</p>
    </div>

    <rhx-drawer id=""contained-drawer"" rhx-label=""Filters""
                rhx-placement=""start"" rhx-contained>
        <label><input type=""checkbox"" /> Active</label>
        <label><input type=""checkbox"" /> Pending</label>
        <label><input type=""checkbox"" /> Archived</label>
    </rhx-drawer>
</div>";

    public void OnGet()
    {
        ViewData["Breadcrumbs"] = new List<BreadcrumbItem>
        {
            new("Home", "/"),
            new("Components", "/Docs/Components/Drawer"),
            new("Drawer")
        };
    }
}
