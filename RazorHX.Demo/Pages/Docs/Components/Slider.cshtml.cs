using Microsoft.AspNetCore.Mvc.RazorPages;
using RazorHX.Components.Navigation;
using RazorHX.Demo.Models;

namespace RazorHX.Demo.Pages.Docs.Components;

public class SliderModel : PageModel
{
    public int Volume { get; set; } = 50;

    public List<ComponentProperty> Properties { get; } = new()
    {
        new("rhx-for", "ModelExpression", "-", "ASP.NET Core model expression for two-way binding"),
        new("name", "string", "-", "The form field name"),
        new("value", "string", "-", "The current value"),
        new("rhx-label", "string", "-", "Label text displayed above the slider"),
        new("rhx-hint", "string", "-", "Hint text displayed below the slider"),
        new("rhx-min", "string", "0", "Minimum value"),
        new("rhx-max", "string", "100", "Maximum value"),
        new("rhx-step", "string", "1", "Step increment"),
        new("rhx-tooltip", "string", "-", "Tooltip position: top, bottom"),
        new("rhx-size", "string", "medium", "Slider size: small, medium, large"),
        new("rhx-disabled", "bool", "false", "Whether the slider is disabled"),
    };

    public string BasicCode => @"<rhx-slider name=""volume"" rhx-label=""Volume"" value=""50"" />";

    public string CustomRangeCode => @"<rhx-slider name=""custom"" rhx-label=""Custom Range""
           rhx-min=""10"" rhx-max=""200"" rhx-step=""10""
           value=""80"" />";

    public string TooltipCode => @"<rhx-slider name=""brightness"" rhx-label=""Brightness""
           value=""75"" rhx-tooltip=""top"" />
<rhx-slider name=""contrast"" rhx-label=""Contrast""
           value=""60"" rhx-tooltip=""bottom"" />";

    public string HintCode => @"<rhx-slider name=""opacity"" rhx-label=""Opacity""
           value=""100""
           rhx-hint=""0 = fully transparent, 100 = fully opaque"" />";

    public string SizesCode => @"<rhx-slider name=""sl-sm"" rhx-label=""Small slider"" rhx-size=""small"" value=""30"" />
<rhx-slider name=""sl-md"" rhx-label=""Medium slider (default)"" value=""50"" />
<rhx-slider name=""sl-lg"" rhx-label=""Large slider"" rhx-size=""large"" value=""70"" />";

    public string StatesCode => @"<rhx-slider name=""sl-dis"" rhx-label=""Disabled slider""
           rhx-disabled=""true"" value=""40"" />";

    public string ModelBindingCode => @"<rhx-slider rhx-for=""Volume"" rhx-label=""Volume"" />";

    public void OnGet()
    {
        ViewData["Breadcrumbs"] = new List<BreadcrumbItem>
        {
            new("Home", "/"),
            new("Components", "/Docs/Components/Slider"),
            new("Slider")
        };
    }
}
