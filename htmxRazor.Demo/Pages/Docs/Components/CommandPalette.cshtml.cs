using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using htmxRazor.Components.Navigation;
using htmxRazor.Demo.Models;

namespace htmxRazor.Demo.Pages.Docs.Components;

public class CommandPaletteModel : PageModel
{
    public List<ComponentProperty> Properties { get; } =
    [
        new("rhx-placeholder", "string", "\"Search...\"", "Input placeholder text"),
        new("rhx-shortcut", "string", "\"mod+k\"", "Keyboard shortcut to open (mod = Cmd on Mac, Ctrl on Win/Linux)"),
        new("rhx-debounce", "int", "300", "Debounce delay in milliseconds"),
        new("rhx-min-chars", "int", "1", "Minimum characters before search fires"),
        new("rhx-empty-message", "string", "\"No results found\"", "Message when results are empty"),
        new("rhx-label", "string", "\"Command palette\"", "Accessible label for the dialog"),
    ];

    public List<ComponentProperty> GroupProperties { get; } =
    [
        new("rhx-heading", "string", "-", "Group heading text"),
    ];

    public List<ComponentProperty> ItemProperties { get; } =
    [
        new("rhx-value", "string", "-", "Item value passed on selection"),
        new("rhx-href", "string?", "-", "Navigation URL on select"),
        new("rhx-icon", "string?", "-", "Leading icon name"),
        new("rhx-description", "string?", "-", "Secondary description text"),
        new("rhx-shortcut", "string?", "-", "Display keyboard shortcut hint"),
        new("rhx-disabled", "bool", "false", "Disabled state"),
    ];

    public string BasicCode => @"<rhx-button rhx-variant=""neutral"" rhx-appearance=""outlined""
            onclick=""document.getElementById('basic-cp').removeAttribute('hidden');
                     document.getElementById('basic-cp').querySelector('input').focus();"">
    Open Palette
</rhx-button>

<rhx-command-palette id=""basic-cp"" rhx-label=""Quick search"">
    <rhx-command-group rhx-heading=""Pages"">
        <rhx-command-item rhx-value=""home"" rhx-href=""/"" rhx-icon=""home"">
            Home
        </rhx-command-item>
        <rhx-command-item rhx-value=""getting-started"" rhx-href=""/Docs/GettingStarted"" rhx-icon=""file"">
            Getting Started
        </rhx-command-item>
    </rhx-command-group>
    <rhx-command-group rhx-heading=""Components"">
        <rhx-command-item rhx-value=""button"" rhx-href=""/Docs/Components/Button"" rhx-icon=""cursor"">
            Button
        </rhx-command-item>
        <rhx-command-item rhx-value=""dialog"" rhx-href=""/Docs/Components/Dialog"" rhx-icon=""layers"">
            Dialog
        </rhx-command-item>
    </rhx-command-group>
</rhx-command-palette>";

    public string HtmxCode => @"<!-- Press Cmd+K / Ctrl+K to open -->
<rhx-command-palette id=""htmx-cp""
    rhx-placeholder=""Search components...""
    hx-get=""/Docs/Components/CommandPalette?handler=Search""
    rhx-debounce=""200""
    rhx-label=""Search components"">
</rhx-command-palette>

<!-- Server handler returns grouped results: -->
public IActionResult OnGetSearch(string q) { ... }";

    public string ItemCode => @"<rhx-command-item rhx-value=""settings""
    rhx-href=""/settings""
    rhx-icon=""settings""
    rhx-description=""Manage your preferences""
    rhx-shortcut=""⌘,"">
    Settings
</rhx-command-item>

<rhx-command-item rhx-value=""locked"" rhx-disabled=""true"">
    Locked Feature
</rhx-command-item>";

    // ── Sample data for search ──
    private static readonly List<(string Name, string Href, string Icon, string Category)> AllItems =
    [
        ("Button", "/Docs/Components/Button", "cursor", "Actions"),
        ("Dropdown", "/Docs/Components/Dropdowns", "chevron-down", "Actions"),
        ("Input", "/Docs/Components/Input", "edit", "Forms"),
        ("Select", "/Docs/Components/Select", "chevron-down", "Forms"),
        ("Combobox", "/Docs/Components/Combobox", "search", "Forms"),
        ("Checkbox", "/Docs/Components/Checkbox", "check", "Forms"),
        ("Switch", "/Docs/Components/Switch", "check", "Forms"),
        ("Dialog", "/Docs/Components/Dialog", "layers", "Overlays"),
        ("Drawer", "/Docs/Components/Drawer", "layers", "Overlays"),
        ("Command Palette", "/Docs/Components/CommandPalette", "search", "Overlays"),
        ("Callout", "/Docs/Components/Callout", "bell", "Feedback"),
        ("Toast", "/Docs/Components/Toast", "bell", "Feedback"),
        ("Badge", "/Docs/Components/Badge", "bell", "Feedback"),
        ("Tabs", "/Docs/Components/Tabs", "arrow-right", "Navigation"),
        ("Breadcrumb", "/Docs/Components/Breadcrumb", "arrow-right", "Navigation"),
        ("Tree", "/Docs/Components/Tree", "arrow-right", "Navigation"),
        ("Card", "/Docs/Components/Card", "grid", "Organization"),
        ("Data Table", "/Docs/Components/DataTable", "table", "Data Display"),
        ("Icon", "/Docs/Components/Icon", "image", "Imagery"),
        ("Avatar", "/Docs/Components/Avatar", "user", "Imagery"),
        ("Sparkline", "/Docs/Components/Sparkline", "table", "Data Display"),
    ];

    public void OnGet()
    {
        ViewData["Breadcrumbs"] = new List<BreadcrumbItem>
        {
            new("Home", "/"),
            new("Components", "/Docs/Components/CommandPalette"),
            new("Command Palette")
        };
    }

    public IActionResult OnGetSearch(string? q)
    {
        if (string.IsNullOrWhiteSpace(q))
            return Content("", "text/html");

        var matches = AllItems
            .Where(i => i.Name.Contains(q, StringComparison.OrdinalIgnoreCase) ||
                        i.Category.Contains(q, StringComparison.OrdinalIgnoreCase))
            .ToList();

        if (matches.Count == 0)
            return Content("", "text/html");

        var grouped = matches.GroupBy(m => m.Category).OrderBy(g => g.Key);
        var html = "";
        foreach (var group in grouped)
        {
            var headingId = $"srg-{Guid.NewGuid():N}";
            html += $"<div class=\"rhx-command-palette__group\" role=\"group\" aria-labelledby=\"{headingId}\">";
            html += $"<div class=\"rhx-command-palette__group-heading\" id=\"{headingId}\" role=\"presentation\">{Enc(group.Key)}</div>";
            foreach (var item in group)
            {
                var iconSvg = htmxRazor.Components.Imagery.IconRegistry.Get(item.Icon) ?? "";
                html += "<div class=\"rhx-command-palette__item\" role=\"option\" aria-selected=\"false\" tabindex=\"-1\"" +
                    $" data-rhx-value=\"{Enc(item.Name)}\" data-rhx-href=\"{Enc(item.Href)}\">" +
                    "<span class=\"rhx-command-palette__item-icon\" aria-hidden=\"true\">" +
                    $"<svg class=\"rhx-icon rhx-icon--small\" viewBox=\"0 0 24 24\" fill=\"none\" stroke=\"currentColor\" stroke-width=\"2\" stroke-linecap=\"round\" stroke-linejoin=\"round\">{iconSvg}</svg>" +
                    "</span>" +
                    "<div class=\"rhx-command-palette__item-content\">" +
                    $"<span class=\"rhx-command-palette__item-label\">{Enc(item.Name)}</span>" +
                    "</div></div>";
            }
            html += "</div>";
        }

        return Content(html, "text/html");
    }

    private static string Enc(string? value)
        => System.Net.WebUtility.HtmlEncode(value ?? "") ?? "";
}
