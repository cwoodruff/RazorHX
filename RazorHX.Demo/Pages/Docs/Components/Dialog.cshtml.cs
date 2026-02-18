using Microsoft.AspNetCore.Mvc.RazorPages;
using RazorHX.Components.Navigation;
using RazorHX.Demo.Models;

namespace RazorHX.Demo.Pages.Docs.Components;

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

    public string HtmxCode => @"<!-- Trigger: load content into dialog via htmx -->
<rhx-button rhx-variant=""brand""
            hx-get=""/api/edit-form/42""
            hx-target=""#edit-dialog .rhx-dialog__body""
            data-rhx-dialog-open=""edit-dialog"">
    Edit Item
</rhx-button>

<rhx-dialog id=""edit-dialog"" rhx-label=""Edit Item"">
    <!-- Content loaded via htmx -->
</rhx-dialog>";

    public string HtmxServerCode => @"// Server-side: open dialog after htmx swap
// Use HX-Trigger header to open a dialog from the server

[HttpPost(""/api/items"")]
public IActionResult CreateItem(ItemModel model)
{
    // ... save item ...

    // Tell the client to open a success dialog
    Response.Headers[""HX-Trigger""] =
        ""{\""rhx:dialog:open\"": \""success-dialog\""}"";
    return Ok();
}";

    public void OnGet()
    {
        ViewData["Breadcrumbs"] = new List<BreadcrumbItem>
        {
            new("Home", "/"),
            new("Components", "/Docs/Components/Dialog"),
            new("Dialog")
        };
    }
}
