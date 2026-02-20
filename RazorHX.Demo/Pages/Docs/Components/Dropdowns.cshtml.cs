using Microsoft.AspNetCore.Mvc.RazorPages;
using RazorHX.Components.Navigation;
using RazorHX.Demo.Models;

namespace RazorHX.Demo.Pages.Docs.Components;

public class DropdownsModel : PageModel
{
    public List<ComponentProperty> Properties { get; } = new()
    {
        new("rhx-open", "bool", "false", "Whether the dropdown is initially open"),
        new("rhx-placement", "string", "bottom-start", "Menu position: bottom-start, bottom-end, top-start, top-end"),
        new("rhx-disabled", "bool", "false", "Disables the dropdown trigger"),
        new("rhx-stay-open", "bool", "false", "Keep menu open after item selection (useful for checkboxes)"),
        new("aria-label", "string", "-", "Accessible label for the dropdown menu"),
    };

    public List<ComponentProperty> ItemProperties { get; } = new()
    {
        new("rhx-type", "string", "normal", "Item type: normal, checkbox"),
        new("rhx-checked", "bool", "false", "Checked state for checkbox items"),
        new("rhx-value", "string", "-", "Value associated with the item"),
        new("rhx-disabled", "bool", "false", "Disables the item"),
        new("rhx-href", "string", "-", "Renders the item as a navigation link"),
        new("aria-label", "string", "-", "Accessible label for the item"),
    };

    public string BasicCode => @"<rhx-dropdown>
    <rhx-dropdown-trigger>
        <rhx-button rhx-variant=""neutral"" rhx-appearance=""outlined"">
            Actions &#9662;
        </rhx-button>
    </rhx-dropdown-trigger>
    <rhx-dropdown-item>Edit</rhx-dropdown-item>
    <rhx-dropdown-item>Duplicate</rhx-dropdown-item>
    <rhx-dropdown-divider />
    <rhx-dropdown-item>Archive</rhx-dropdown-item>
    <rhx-dropdown-item rhx-disabled=""true"">Delete</rhx-dropdown-item>
</rhx-dropdown>";

    public string PlacementsCode => @"<rhx-dropdown rhx-placement=""bottom-start"">
    <rhx-dropdown-trigger>
        <rhx-button rhx-variant=""brand"">Bottom Start &#9662;</rhx-button>
    </rhx-dropdown-trigger>
    <rhx-dropdown-item>Item A</rhx-dropdown-item>
    <rhx-dropdown-item>Item B</rhx-dropdown-item>
</rhx-dropdown>

<rhx-dropdown rhx-placement=""bottom-end"">
    <rhx-dropdown-trigger>
        <rhx-button rhx-variant=""brand"">Bottom End &#9662;</rhx-button>
    </rhx-dropdown-trigger>
    <rhx-dropdown-item>Item A</rhx-dropdown-item>
    <rhx-dropdown-item>Item B</rhx-dropdown-item>
</rhx-dropdown>

<rhx-dropdown rhx-placement=""top-start"">
    <rhx-dropdown-trigger>
        <rhx-button rhx-variant=""brand"">Top Start &#9652;</rhx-button>
    </rhx-dropdown-trigger>
    <rhx-dropdown-item>Item A</rhx-dropdown-item>
    <rhx-dropdown-item>Item B</rhx-dropdown-item>
</rhx-dropdown>";

    public string CheckboxCode => @"<rhx-dropdown rhx-stay-open=""true"">
    <rhx-dropdown-trigger>
        <rhx-button rhx-variant=""neutral"" rhx-appearance=""outlined"">
            Columns &#9662;
        </rhx-button>
    </rhx-dropdown-trigger>
    <rhx-dropdown-item rhx-type=""checkbox"" rhx-checked=""true"" rhx-value=""name"">
        Name
    </rhx-dropdown-item>
    <rhx-dropdown-item rhx-type=""checkbox"" rhx-checked=""true"" rhx-value=""email"">
        Email
    </rhx-dropdown-item>
    <rhx-dropdown-item rhx-type=""checkbox"" rhx-value=""phone"">
        Phone
    </rhx-dropdown-item>
    <rhx-dropdown-item rhx-type=""checkbox"" rhx-value=""role"">
        Role
    </rhx-dropdown-item>
</rhx-dropdown>";

    public string LinksCode => @"<rhx-dropdown>
    <rhx-dropdown-trigger>
        <rhx-button rhx-variant=""neutral"" rhx-appearance=""outlined"">
            Navigate &#9662;
        </rhx-button>
    </rhx-dropdown-trigger>
    <rhx-dropdown-item rhx-href=""/"">Home</rhx-dropdown-item>
    <rhx-dropdown-item rhx-href=""/Docs/Components/Dropdowns"">Dropdowns</rhx-dropdown-item>
    <rhx-dropdown-divider />
    <rhx-dropdown-item rhx-href=""/Error"">Error Page</rhx-dropdown-item>
</rhx-dropdown>";

    public string HtmxCode => @"<rhx-dropdown>
    <rhx-dropdown-trigger>
        <rhx-button rhx-variant=""brand"" rhx-appearance=""outlined"">
            Actions &#9662;
        </rhx-button>
    </rhx-dropdown-trigger>
    <rhx-dropdown-item hx-post=""/api/edit""
                       hx-target=""#dropdown-result"" hx-swap=""innerHTML"">
        Edit Item
    </rhx-dropdown-item>
    <rhx-dropdown-item hx-post=""/api/duplicate""
                       hx-target=""#dropdown-result"" hx-swap=""innerHTML"">
        Duplicate Item
    </rhx-dropdown-item>
    <rhx-dropdown-divider />
    <rhx-dropdown-item hx-delete=""/api/remove""
                       hx-target=""#dropdown-result"" hx-swap=""innerHTML""
                       hx-confirm=""Are you sure?"">
        Delete Item
    </rhx-dropdown-item>
</rhx-dropdown>
<span id=""dropdown-result"">Select an action...</span>";

    public string StatesCode => @"<rhx-dropdown rhx-disabled=""true"">
    <rhx-dropdown-trigger>
        <rhx-button rhx-variant=""neutral"" rhx-appearance=""outlined""
                    rhx-disabled=""true"">
            Disabled &#9662;
        </rhx-button>
    </rhx-dropdown-trigger>
    <rhx-dropdown-item>Unreachable</rhx-dropdown-item>
</rhx-dropdown>

<rhx-dropdown rhx-open=""true"">
    <rhx-dropdown-trigger>
        <rhx-button rhx-variant=""success"" rhx-appearance=""outlined"">
            Open &#9662;
        </rhx-button>
    </rhx-dropdown-trigger>
    <rhx-dropdown-item>Already visible</rhx-dropdown-item>
    <rhx-dropdown-item>Second item</rhx-dropdown-item>
</rhx-dropdown>";

    public void OnGet()
    {
        ViewData["Breadcrumbs"] = new List<BreadcrumbItem>
        {
            new("Home", "/"),
            new("Components", "/Docs/Components/Dropdowns"),
            new("Dropdown")
        };
    }
}
