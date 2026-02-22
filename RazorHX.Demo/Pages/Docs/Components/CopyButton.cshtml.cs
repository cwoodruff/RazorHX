using Microsoft.AspNetCore.Mvc.RazorPages;
using RazorHX.Components.Navigation;
using RazorHX.Demo.Models;

namespace RazorHX.Demo.Pages.Docs.Components;

public class CopyButtonModel : PageModel
{
    public List<ComponentProperty> Properties { get; } = new()
    {
        new("rhx-value", "string", "-", "Static text to copy to clipboard"),
        new("rhx-from", "string", "-", "CSS selector of element whose text content to copy"),
        new("rhx-copy-label", "string", "Copy", "Button label before copy"),
        new("rhx-success-label", "string", "Copied!", "Button label after successful copy"),
        new("rhx-feedback-duration", "int", "2000", "Milliseconds to show success label"),
        new("rhx-disabled", "bool", "false", "Whether the button is disabled"),
    };

    public string StaticValueCode => @"<code>dotnet add package htmxRazor</code>
<rhx-copy-button rhx-value=""dotnet add package htmxRazor"" />";

    public string FromElementCode => @"<pre id=""install-cmd""><code>npm install razorhx-client</code></pre>
<rhx-copy-button rhx-from=""#install-cmd code"" />";

    public string CustomLabelsCode => @"<rhx-copy-button rhx-value=""https://example.com""
               rhx-copy-label=""Copy URL""
               rhx-success-label=""URL Copied!"" />";

    public string LongFeedbackCode => @"<rhx-copy-button rhx-value=""sk-1234567890abcdef""
               rhx-feedback-duration=""5000"" />";

    public string DisabledCode => @"<rhx-copy-button rhx-value=""No copying allowed""
               rhx-disabled=""true"" />";

    public void OnGet()
    {
        ViewData["Breadcrumbs"] = new List<BreadcrumbItem>
        {
            new("Home", "/"),
            new("Components", "/Docs/Components/Button"),
            new("Copy Button")
        };
    }
}
