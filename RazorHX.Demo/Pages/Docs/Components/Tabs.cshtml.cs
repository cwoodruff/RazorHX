using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RazorHX.Components.Navigation;
using RazorHX.Demo.Models;

namespace RazorHX.Demo.Pages.Docs.Components;

public class TabsModel : PageModel
{
    public List<ComponentProperty> Properties { get; } = new()
    {
        new("rhx-placement", "string", "top", "Tab bar position: top, bottom, start, end"),
        new("rhx-activation", "string", "auto", "Activation mode: auto (arrow keys activate), manual (Enter/Space to activate)"),
        new("aria-label", "string", "-", "Accessible label for the tab group"),
    };

    public List<ComponentProperty> TabProperties { get; } = new()
    {
        new("rhx-panel", "string", "-", "Name of the associated tab panel"),
        new("rhx-active", "bool", "false", "Whether this tab is initially active"),
        new("rhx-closable", "bool", "false", "Shows a close button on the tab"),
        new("rhx-disabled", "bool", "false", "Disables the tab"),
    };

    public List<ComponentProperty> PanelProperties { get; } = new()
    {
        new("rhx-name", "string", "-", "Panel name matching the tab's rhx-panel value"),
        new("rhx-active", "bool", "false", "Whether this panel is initially visible"),
    };

    public string BasicCode => @"<rhx-tab-group aria-label=""Account settings"">
    <rhx-tab rhx-panel=""general"" rhx-active=""true"">General</rhx-tab>
    <rhx-tab rhx-panel=""profile"">Profile</rhx-tab>
    <rhx-tab rhx-panel=""notifications"">Notifications</rhx-tab>
    <rhx-tab-panel rhx-name=""general"" rhx-active=""true"">
        <p>General account settings.</p>
    </rhx-tab-panel>
    <rhx-tab-panel rhx-name=""profile"">
        <p>Profile settings.</p>
    </rhx-tab-panel>
    <rhx-tab-panel rhx-name=""notifications"">
        <p>Notification preferences.</p>
    </rhx-tab-panel>
</rhx-tab-group>";

    public string PlacementsCode => @"<!-- Bottom -->
<rhx-tab-group rhx-placement=""bottom"">
    <rhx-tab rhx-panel=""b-one"" rhx-active=""true"">Tab One</rhx-tab>
    <rhx-tab rhx-panel=""b-two"">Tab Two</rhx-tab>
    <rhx-tab-panel rhx-name=""b-one"" rhx-active=""true"">
        <p>Bottom placement content.</p>
    </rhx-tab-panel>
    <rhx-tab-panel rhx-name=""b-two"">
        <p>Tab two content.</p>
    </rhx-tab-panel>
</rhx-tab-group>

<!-- Start (vertical) -->
<rhx-tab-group rhx-placement=""start"">
    <rhx-tab rhx-panel=""s-one"" rhx-active=""true"">Dashboard</rhx-tab>
    <rhx-tab rhx-panel=""s-two"">Analytics</rhx-tab>
    <rhx-tab rhx-panel=""s-three"">Reports</rhx-tab>
    <rhx-tab-panel rhx-name=""s-one"" rhx-active=""true"">
        <p>Dashboard overview.</p>
    </rhx-tab-panel>
    <rhx-tab-panel rhx-name=""s-two"">
        <p>Analytics panel.</p>
    </rhx-tab-panel>
    <rhx-tab-panel rhx-name=""s-three"">
        <p>Reports section.</p>
    </rhx-tab-panel>
</rhx-tab-group>

<!-- End (vertical) -->
<rhx-tab-group rhx-placement=""end"">
    <rhx-tab rhx-panel=""e-one"" rhx-active=""true"">Info</rhx-tab>
    <rhx-tab rhx-panel=""e-two"">Details</rhx-tab>
    <rhx-tab-panel rhx-name=""e-one"" rhx-active=""true"">
        <p>Info panel content.</p>
    </rhx-tab-panel>
    <rhx-tab-panel rhx-name=""e-two"">
        <p>Details panel content.</p>
    </rhx-tab-panel>
</rhx-tab-group>";

    public string ManualCode => @"<rhx-tab-group rhx-activation=""manual"">
    <rhx-tab rhx-panel=""m-one"" rhx-active=""true"">First</rhx-tab>
    <rhx-tab rhx-panel=""m-two"">Second</rhx-tab>
    <rhx-tab rhx-panel=""m-three"">Third</rhx-tab>
    <rhx-tab-panel rhx-name=""m-one"" rhx-active=""true"">
        <p>Use arrow keys to move focus, then Enter/Space to select.</p>
    </rhx-tab-panel>
    <rhx-tab-panel rhx-name=""m-two"">
        <p>Second tab content.</p>
    </rhx-tab-panel>
    <rhx-tab-panel rhx-name=""m-three"">
        <p>Third tab content.</p>
    </rhx-tab-panel>
</rhx-tab-group>";

    public string ClosableCode => @"<rhx-tab-group>
    <rhx-tab rhx-panel=""c-one"" rhx-active=""true"" rhx-closable=""true"">Document 1</rhx-tab>
    <rhx-tab rhx-panel=""c-two"" rhx-closable=""true"">Document 2</rhx-tab>
    <rhx-tab rhx-panel=""c-three"" rhx-closable=""true"">Document 3</rhx-tab>
    <rhx-tab-panel rhx-name=""c-one"" rhx-active=""true"">
        <p>Document 1 content. Close this tab to see the next one activate.</p>
    </rhx-tab-panel>
    <rhx-tab-panel rhx-name=""c-two"">
        <p>Document 2 content.</p>
    </rhx-tab-panel>
    <rhx-tab-panel rhx-name=""c-three"">
        <p>Document 3 content.</p>
    </rhx-tab-panel>
</rhx-tab-group>";

    public string DisabledCode => @"<rhx-tab-group>
    <rhx-tab rhx-panel=""d-one"" rhx-active=""true"">Active</rhx-tab>
    <rhx-tab rhx-panel=""d-two"" rhx-disabled=""true"">Disabled</rhx-tab>
    <rhx-tab rhx-panel=""d-three"">Available</rhx-tab>
    <rhx-tab-panel rhx-name=""d-one"" rhx-active=""true"">
        <p>The ""Disabled"" tab cannot be clicked or focused via keyboard.</p>
    </rhx-tab-panel>
    <rhx-tab-panel rhx-name=""d-two"">
        <p>This content is unreachable via the disabled tab.</p>
    </rhx-tab-panel>
    <rhx-tab-panel rhx-name=""d-three"">
        <p>Available tab content.</p>
    </rhx-tab-panel>
</rhx-tab-group>";

    public string LazyCode => @"<rhx-tab-group>
    <rhx-tab rhx-panel=""lazy-pre"" rhx-active=""true"">Pre-rendered</rhx-tab>
    <rhx-tab rhx-panel=""lazy-one""
             hx-get=""/Docs/Components/Tabs?handler=LazyContent&tab=one""
             hx-target=""#panel-lazy-one""
             hx-swap=""innerHTML""
             hx-trigger=""click once"">Lazy Tab 1</rhx-tab>
    <rhx-tab rhx-panel=""lazy-two""
             hx-get=""/Docs/Components/Tabs?handler=LazyContent&tab=two""
             hx-target=""#panel-lazy-two""
             hx-swap=""innerHTML""
             hx-trigger=""click once"">Lazy Tab 2</rhx-tab>
    <rhx-tab-panel rhx-name=""lazy-pre"" rhx-active=""true"">
        <p>This panel was rendered with the page.</p>
    </rhx-tab-panel>
    <rhx-tab-panel rhx-name=""lazy-one"">
        <rhx-spinner rhx-size=""small"" /> Loading...
    </rhx-tab-panel>
    <rhx-tab-panel rhx-name=""lazy-two"">
        <rhx-spinner rhx-size=""small"" /> Loading...
    </rhx-tab-panel>
</rhx-tab-group>";

    public void OnGet()
    {
        ViewData["Breadcrumbs"] = new List<BreadcrumbItem>
        {
            new("Home", "/"),
            new("Components", "/Docs/Components/Tabs"),
            new("Tabs")
        };
    }

    public ContentResult OnGetLazyContent(string tab)
    {
        var html = tab switch
        {
            "one" => "<p><strong>Lazy Tab 1</strong> — This content was loaded via htmx when the tab was first clicked.</p>",
            "two" => "<p><strong>Lazy Tab 2</strong> — Another lazy-loaded panel. The spinner was replaced with this content.</p>",
            _ => "<p>Unknown tab content.</p>"
        };

        return Content(html, "text/html");
    }
}
