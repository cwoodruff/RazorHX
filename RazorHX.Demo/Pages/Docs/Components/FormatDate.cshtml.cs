using Microsoft.AspNetCore.Mvc.RazorPages;
using RazorHX.Components.Navigation;
using RazorHX.Demo.Models;

namespace RazorHX.Demo.Pages.Docs.Components;

public class FormatDateModel : PageModel
{
    public List<ComponentProperty> Properties { get; } = new()
    {
        new("rhx-date", "DateTimeOffset", "-", "The date/time value to format"),
        new("rhx-format", "string", "-", ".NET format string (e.g. yyyy-MM-dd)"),
        new("rhx-weekday", "string", "-", "Weekday display: long, short, narrow"),
        new("rhx-year", "string", "-", "Year display: numeric, 2-digit"),
        new("rhx-month", "string", "-", "Month display: numeric, 2-digit, long, short, narrow"),
        new("rhx-day", "string", "-", "Day display: numeric, 2-digit"),
        new("rhx-hour", "string", "-", "Hour display: numeric, 2-digit"),
        new("rhx-minute", "string", "-", "Minute display: numeric, 2-digit"),
        new("rhx-second", "string", "-", "Second display: numeric, 2-digit"),
        new("rhx-hour-format", "string", "-", "Clock format: 12, 24"),
        new("rhx-timezone", "string", "-", "IANA timezone (e.g. America/New_York)"),
    };

    public string CustomFormatCode => @"<rhx-format-date rhx-date=""@sampleDate"" rhx-format=""yyyy-MM-dd"" />
<rhx-format-date rhx-date=""@sampleDate"" rhx-format=""HH:mm:ss"" />
<rhx-format-date rhx-date=""@sampleDate""
                 rhx-format=""MMMM d, yyyy h:mm tt"" />";

    public string IndividualPartsCode => @"<rhx-format-date rhx-date=""@sampleDate""
                 rhx-month=""long"" rhx-day=""numeric"" rhx-year=""numeric"" />
<rhx-format-date rhx-date=""@sampleDate""
                 rhx-month=""short"" rhx-day=""numeric"" />
<rhx-format-date rhx-date=""@sampleDate""
                 rhx-month=""numeric"" rhx-day=""numeric"" rhx-year=""numeric"" />
<rhx-format-date rhx-date=""@sampleDate""
                 rhx-weekday=""long"" rhx-month=""long"" rhx-day=""numeric"" />
<rhx-format-date rhx-date=""@sampleDate"" rhx-year=""2-digit"" />";

    public string TimeCode => @"<rhx-format-date rhx-date=""@sampleDate""
                 rhx-hour=""2-digit"" rhx-minute=""2-digit"" rhx-hour-format=""24"" />
<rhx-format-date rhx-date=""@sampleDate""
                 rhx-hour=""numeric"" rhx-minute=""2-digit""
                 rhx-second=""2-digit"" rhx-hour-format=""12"" />
<rhx-format-date rhx-date=""@sampleDate""
                 rhx-month=""long"" rhx-day=""numeric"" rhx-year=""numeric""
                 rhx-hour=""numeric"" rhx-minute=""2-digit"" rhx-hour-format=""12"" />";

    public string TimezoneCode => @"<rhx-format-date rhx-date=""@sampleDate""
                 rhx-timezone=""America/New_York""
                 rhx-hour=""numeric"" rhx-minute=""2-digit"" rhx-hour-format=""24"" />
<rhx-format-date rhx-date=""@sampleDate""
                 rhx-timezone=""Asia/Tokyo""
                 rhx-hour=""numeric"" rhx-minute=""2-digit"" rhx-hour-format=""24"" />";

    public void OnGet()
    {
        ViewData["Breadcrumbs"] = new List<BreadcrumbItem>
        {
            new("Home", "/"),
            new("Components", "/Docs/Components/FormatDate"),
            new("Format Date")
        };
    }
}
