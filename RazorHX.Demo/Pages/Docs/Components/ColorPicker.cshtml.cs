using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RazorHX.Components.Navigation;
using RazorHX.Demo.Models;

namespace RazorHX.Demo.Pages.Docs.Components;

public class ColorPickerModel : PageModel
{
    public string BrandColor { get; set; } = "#3b82f6";

    public List<ComponentProperty> Properties { get; } = new()
    {
        new("rhx-for", "ModelExpression", "-", "ASP.NET Core model expression for two-way binding"),
        new("name", "string", "-", "The form field name"),
        new("value", "string", "-", "The current color value (hex)"),
        new("rhx-label", "string", "-", "Label text"),
        new("rhx-format", "string", "hex", "Color format: hex, rgb, hsl"),
        new("rhx-swatches", "string", "-", "Comma-separated preset color values"),
        new("rhx-opacity", "bool", "false", "Show opacity/alpha slider"),
        new("rhx-inline", "bool", "false", "Always show the picker (no trigger button)"),
        new("rhx-disabled", "bool", "false", "Whether the color picker is disabled"),
    };

    public string BasicCode => @"<rhx-color-picker name=""color"" rhx-label=""Color"" value=""#3b82f6"" />";

    public string SwatchesCode => @"<rhx-color-picker name=""color-swatches"" rhx-label=""Brand Color""
           value=""#ef4444""
           rhx-swatches=""#ef4444,#f97316,#eab308,#22c55e,#3b82f6,#8b5cf6,#ec4899,#000000"" />";

    public string OpacityCode => @"<rhx-color-picker name=""color-alpha"" rhx-label=""Overlay Color""
           value=""#3b82f6"" rhx-opacity=""true"" />";

    public string FormatsCode => @"<rhx-color-picker name=""color-rgb"" rhx-label=""RGB Color""
           value=""#22c55e"" rhx-format=""rgb"" />
<rhx-color-picker name=""color-hsl"" rhx-label=""HSL Color""
           value=""#8b5cf6"" rhx-format=""hsl"" />";

    public string InlineCode => @"<rhx-color-picker name=""color-inline"" rhx-label=""Theme Color""
           value=""#f97316"" rhx-inline=""true""
           rhx-swatches=""#ef4444,#f97316,#eab308,#22c55e,#3b82f6,#8b5cf6"" />";

    public string StatesCode => @"<rhx-color-picker name=""color-dis"" rhx-label=""Disabled""
           value=""#6b7280"" rhx-disabled=""true"" />";

    public string ModelBindingCode => @"<rhx-color-picker rhx-for=""BrandColor"" rhx-label=""Brand Color"" />";

    public string HtmxCode => @"<rhx-color-picker name=""themeColor"" rhx-label=""Theme Color""
           value=""#3b82f6""
           rhx-swatches=""#ef4444,#f97316,#eab308,#22c55e,#3b82f6,#8b5cf6""
           hx-post=""/Docs/Components/ColorPicker?handler=SaveColor""
           hx-trigger=""change""
           hx-target=""#color-result""
           hx-include=""this"" />
<div id=""color-result"">Pick a color to save...</div>";

    public void OnGet()
    {
        ViewData["Breadcrumbs"] = new List<BreadcrumbItem>
        {
            new("Home", "/"),
            new("Components", "/Docs/Components/ColorPicker"),
            new("Color Picker")
        };
    }

    public IActionResult OnPostSaveColor(string? themeColor)
    {
        var color = string.IsNullOrWhiteSpace(themeColor) ? "#000000" : System.Net.WebUtility.HtmlEncode(themeColor);
        return Content($"""
            <span style="color: var(--rhx-color-text-muted);">
                Theme color saved:
                <span style="display: inline-block; width: 1em; height: 1em; background: {color}; border-radius: var(--rhx-radius-sm); vertical-align: middle; border: 1px solid var(--rhx-color-border);"></span>
                <strong>{color}</strong>
            </span>
            """, "text/html");
    }
}
