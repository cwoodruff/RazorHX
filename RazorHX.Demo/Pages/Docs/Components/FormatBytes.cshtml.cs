using Microsoft.AspNetCore.Mvc.RazorPages;
using RazorHX.Components.Navigation;
using RazorHX.Demo.Models;

namespace RazorHX.Demo.Pages.Docs.Components;

public class FormatBytesModel : PageModel
{
    public List<ComponentProperty> Properties { get; } = new()
    {
        new("rhx-value", "long", "0", "The byte value to format"),
        new("rhx-display", "string", "short", "Display style: short, long, narrow"),
        new("rhx-unit", "string", "byte", "Unit type: byte (binary 1024) or bit (decimal 1000)"),
    };

    public string ShortCode => @"<rhx-format-bytes rhx-value=""0"" />
<rhx-format-bytes rhx-value=""512"" />
<rhx-format-bytes rhx-value=""1024"" />
<rhx-format-bytes rhx-value=""1572864"" />
<rhx-format-bytes rhx-value=""1073741824"" />
<rhx-format-bytes rhx-value=""1099511627776"" />";

    public string LongCode => @"<rhx-format-bytes rhx-value=""1"" rhx-display=""long"" />
<rhx-format-bytes rhx-value=""0"" rhx-display=""long"" />
<rhx-format-bytes rhx-value=""1572864"" rhx-display=""long"" />";

    public string NarrowCode => @"<rhx-format-bytes rhx-value=""1024"" rhx-display=""narrow"" />
<rhx-format-bytes rhx-value=""1572864"" rhx-display=""narrow"" />";

    public string BitsCode => @"<rhx-format-bytes rhx-value=""500"" rhx-unit=""bit"" />
<rhx-format-bytes rhx-value=""1000"" rhx-unit=""bit"" />
<rhx-format-bytes rhx-value=""1000000"" rhx-unit=""bit"" />";

    public void OnGet()
    {
        ViewData["Breadcrumbs"] = new List<BreadcrumbItem>
        {
            new("Home", "/"),
            new("Components", "/Docs/Components/FormatBytes"),
            new("Format Bytes")
        };
    }
}
