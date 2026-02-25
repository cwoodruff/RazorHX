using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using htmxRazor.Components.Navigation;
using htmxRazor.Demo.Models;

namespace htmxRazor.Demo.Pages.Docs.Components;

public class DialogModel : PageModel
{
    public List<ComponentProperty> Properties { get; } = new()
    {
        new("rhx-open", "bool", "false", "Whether the dialog is initially open"),
        new("rhx-label", "string", "-", "Title text displayed in the dialog header"),
        new("rhx-no-header", "bool", "false", "Hides the default header with title and close button"),
        new("id", "string", "-", "Element ID used for dialog open triggers"),
    };

    public string BasicCode => @"<rhx-button rhx-variant=""brand""
            data-rhx-dialog-open=""demo-dialog"">
    Open Dialog
</rhx-button>

<rhx-dialog id=""demo-dialog"" rhx-label=""Example Dialog"">
    <p>This is a basic dialog with a title and close button.</p>
    <p>Press ESC or click the close button to dismiss.</p>
</rhx-dialog>";

    public string NoHeaderCode => @"<rhx-button data-rhx-dialog-open=""confirm-dialog"">
    Open Headerless Dialog
</rhx-button>

<rhx-dialog id=""confirm-dialog"" rhx-no-header=""true"">
    <p>Are you sure you want to proceed?</p>
    <rhx-button rhx-variant=""neutral""
                onclick=""this.closest('dialog').close()"">
        Cancel
    </rhx-button>
    <rhx-button rhx-variant=""brand""
                onclick=""this.closest('dialog').close()"">
        Confirm
    </rhx-button>
</rhx-dialog>";

    public string FooterCode => @"<rhx-button rhx-variant=""brand""
            data-rhx-dialog-open=""edit-dialog"">
    Open Dialog with Footer
</rhx-button>

<rhx-dialog id=""edit-dialog"" rhx-label=""Edit Profile"">
    <rhx-input rhx-label=""Display Name""
               rhx-placeholder=""Enter name""
               name=""displayName"" />

    <rhx-dialog-footer>
        <rhx-button rhx-variant=""neutral"" rhx-appearance=""outlined""
                    onclick=""this.closest('dialog').close()"">
            Cancel
        </rhx-button>
        <rhx-button rhx-variant=""brand""
                    onclick=""this.closest('dialog').close()"">
            Save Changes
        </rhx-button>
    </rhx-dialog-footer>
</rhx-dialog>";

    public string HtmxCode => @"<rhx-button rhx-variant=""brand""
            hx-get=""/Docs/Components/Dialog?handler=EditForm""
            hx-target=""#htmx-dialog .rhx-dialog__body""
            data-rhx-dialog-open=""htmx-dialog"">
    Edit Profile
</rhx-button>

<rhx-dialog id=""htmx-dialog"" rhx-label=""Edit Profile"">
    <p>Loading...</p>
</rhx-dialog>";

    public void OnGet()
    {
        ViewData["Breadcrumbs"] = new List<BreadcrumbItem>
        {
            new("Home", "/"),
            new("Components", "/Docs/Components/Dialog"),
            new("Dialog")
        };
    }

    public IActionResult OnGetEditForm()
    {
        return Content("""
            <div style="display: flex; flex-direction: column; gap: var(--rhx-space-md);">
                <div>
                    <label style="display: block; font-weight: var(--rhx-font-weight-medium); margin-bottom: var(--rhx-space-xs);">Name</label>
                    <input type="text" value="Jane Smith" style="width: 100%; padding: var(--rhx-space-sm); border: 1px solid var(--rhx-color-border); border-radius: var(--rhx-radius-md);" />
                </div>
                <div>
                    <label style="display: block; font-weight: var(--rhx-font-weight-medium); margin-bottom: var(--rhx-space-xs);">Email</label>
                    <input type="email" value="jane@example.com" style="width: 100%; padding: var(--rhx-space-sm); border: 1px solid var(--rhx-color-border); border-radius: var(--rhx-radius-md);" />
                </div>
            </div>
            """, "text/html");
    }
}
