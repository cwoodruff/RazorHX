using Microsoft.AspNetCore.Mvc.RazorPages;
using RazorHX.Components.Navigation;
using RazorHX.Demo.Models;

namespace RazorHX.Demo.Pages.Docs.Components;

public class QrCodeModel : PageModel
{
    public List<ComponentProperty> Properties { get; } = new()
    {
        new("rhx-value", "string", "-", "The text/URL to encode"),
        new("rhx-label", "string", "-", "Accessible label for the QR code"),
        new("rhx-size", "int", "128", "Width and height in pixels"),
        new("rhx-fill", "string", "#000000", "Color of the QR code modules"),
        new("rhx-background", "string", "#ffffff", "Background color"),
        new("rhx-error-correction", "string", "M", "Error correction level: L, M, Q, H"),
        new("rhx-radius", "double", "0", "Dot corner radius (0 = square, 0.5 = rounded)"),
    };

    public string BasicCode => @"<rhx-qr-code rhx-value=""https://example.com""
              rhx-label=""Example website"" />";

    public string LargeCode => @"<rhx-qr-code rhx-value=""https://example.com""
              rhx-label=""Example website""
              rhx-size=""256""
              rhx-error-correction=""H"" />";

    public string CustomColorsCode => @"<rhx-qr-code rhx-value=""https://example.com""
              rhx-label=""Branded QR code""
              rhx-fill=""#6366f1""
              rhx-background=""#f0f0ff"" />";

    public string RoundedCode => @"<rhx-qr-code rhx-value=""https://example.com""
              rhx-label=""Rounded QR code""
              rhx-radius=""0.5"" />";

    public string SmallCode => @"<rhx-qr-code rhx-value=""https://example.com""
              rhx-label=""Small QR code""
              rhx-size=""64"" />";

    public string ErrorCorrectionCode => @"<rhx-qr-code rhx-value=""https://example.com"" rhx-label=""Low EC""
              rhx-error-correction=""L"" rhx-size=""96"" />
<rhx-qr-code rhx-value=""https://example.com"" rhx-label=""Medium EC""
              rhx-error-correction=""M"" rhx-size=""96"" />
<rhx-qr-code rhx-value=""https://example.com"" rhx-label=""Quartile EC""
              rhx-error-correction=""Q"" rhx-size=""96"" />
<rhx-qr-code rhx-value=""https://example.com"" rhx-label=""High EC""
              rhx-error-correction=""H"" rhx-size=""96"" />";

    public void OnGet()
    {
        ViewData["Breadcrumbs"] = new List<BreadcrumbItem>
        {
            new("Home", "/"),
            new("Components", "/Docs/Components/Button"),
            new("QR Code")
        };
    }
}
