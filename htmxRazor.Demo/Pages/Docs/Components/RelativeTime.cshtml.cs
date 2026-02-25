using Microsoft.AspNetCore.Mvc.RazorPages;
using htmxRazor.Components.Navigation;
using htmxRazor.Demo.Models;

namespace htmxRazor.Demo.Pages.Docs.Components;

public class RelativeTimeModel : PageModel
{
    public List<ComponentProperty> Properties { get; } = new()
    {
        new("rhx-date", "DateTimeOffset", "-", "The date/time to display relative to now"),
        new("rhx-format", "string", "long", "Format style: long, short, narrow"),
        new("rhx-numeric", "string", "always", "Numeric mode: always, auto"),
    };

    public string LongCode => @"<rhx-relative-time rhx-date=""@DateTimeOffset.UtcNow.AddSeconds(-3)"" />
<rhx-relative-time rhx-date=""@DateTimeOffset.UtcNow.AddMinutes(-5)"" />
<rhx-relative-time rhx-date=""@DateTimeOffset.UtcNow.AddHours(-3)"" />
<rhx-relative-time rhx-date=""@DateTimeOffset.UtcNow.AddDays(-1)"" />
<rhx-relative-time rhx-date=""@DateTimeOffset.UtcNow.AddDays(-14)"" />
<rhx-relative-time rhx-date=""@DateTimeOffset.UtcNow.AddDays(-60)"" />";

    public string FutureCode => @"<rhx-relative-time rhx-date=""@DateTimeOffset.UtcNow.AddSeconds(3)"" />
<rhx-relative-time rhx-date=""@DateTimeOffset.UtcNow.AddMinutes(10)"" />
<rhx-relative-time rhx-date=""@DateTimeOffset.UtcNow.AddHours(2)"" />
<rhx-relative-time rhx-date=""@DateTimeOffset.UtcNow.AddDays(3)"" />";

    public string ShortCode => @"<rhx-relative-time rhx-date=""@DateTimeOffset.UtcNow.AddMinutes(-5)""
                   rhx-format=""short"" />
<rhx-relative-time rhx-date=""@DateTimeOffset.UtcNow.AddHours(2)""
                   rhx-format=""short"" />
<rhx-relative-time rhx-date=""@DateTimeOffset.UtcNow.AddDays(-3)""
                   rhx-format=""short"" />";

    public string NarrowCode => @"<rhx-relative-time rhx-date=""@DateTimeOffset.UtcNow.AddMinutes(-5)""
                   rhx-format=""narrow"" />
<rhx-relative-time rhx-date=""@DateTimeOffset.UtcNow.AddHours(2)""
                   rhx-format=""narrow"" />
<rhx-relative-time rhx-date=""@DateTimeOffset.UtcNow.AddDays(-45)""
                   rhx-format=""narrow"" />";

    public string AutoNumericCode => @"<rhx-relative-time rhx-date=""@DateTimeOffset.UtcNow.AddDays(-1)""
                   rhx-numeric=""auto"" />
<rhx-relative-time rhx-date=""@DateTimeOffset.UtcNow.AddDays(1)""
                   rhx-numeric=""auto"" />
<rhx-relative-time rhx-date=""@DateTimeOffset.UtcNow.AddDays(-7)""
                   rhx-numeric=""auto"" />
<rhx-relative-time rhx-date=""@DateTimeOffset.UtcNow.AddDays(-365)""
                   rhx-numeric=""auto"" />";

    public void OnGet()
    {
        ViewData["Breadcrumbs"] = new List<BreadcrumbItem>
        {
            new("Home", "/"),
            new("Components", "/Docs/Components/RelativeTime"),
            new("Relative Time")
        };
    }
}
