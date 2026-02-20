using Microsoft.AspNetCore.Mvc.RazorPages;
using RazorHX.Components.Navigation;
using RazorHX.Demo.Models;

namespace RazorHX.Demo.Pages.Docs.Components;

public class FileInputModel : PageModel
{
    public List<ComponentProperty> Properties { get; } = new()
    {
        new("name", "string", "-", "The form field name"),
        new("rhx-label", "string", "-", "Label text"),
        new("rhx-hint", "string", "-", "Hint text"),
        new("rhx-accept", "string", "-", "File type filter (e.g. image/*, .pdf)"),
        new("rhx-multiple", "bool", "false", "Allow multiple file selection"),
        new("rhx-max-file-size", "long?", "-", "Maximum file size in bytes"),
        new("rhx-size", "string", "medium", "Input size: small, medium, large"),
        new("rhx-disabled", "bool", "false", "Whether the file input is disabled"),
    };

    public string BasicCode => @"<rhx-file-input name=""file"" rhx-label=""Upload File"" />";

    public string ImagesOnlyCode => @"<rhx-file-input name=""avatar"" rhx-label=""Profile Photo""
           rhx-accept=""image/*""
           rhx-hint=""JPG, PNG, or GIF. Max 2MB."" />";

    public string MultipleCode => @"<rhx-file-input name=""documents"" rhx-label=""Documents""
           rhx-multiple=""true""
           rhx-accept="".pdf,.doc,.docx"" />";

    public string MaxSizeCode => @"<rhx-file-input name=""upload"" rhx-label=""Upload""
           rhx-multiple=""true""
           rhx-max-file-size=""5242880""
           rhx-hint=""Maximum file size: 5MB per file"" />";

    public string SizesCode => @"<rhx-file-input name=""fi-sm"" rhx-label=""Small"" rhx-size=""small"" />
<rhx-file-input name=""fi-md"" rhx-label=""Medium (default)"" />
<rhx-file-input name=""fi-lg"" rhx-label=""Large"" rhx-size=""large"" />";

    public string StatesCode => @"<rhx-file-input name=""fi-dis"" rhx-label=""Disabled""
           rhx-disabled=""true"" />";

    public void OnGet()
    {
        ViewData["Breadcrumbs"] = new List<BreadcrumbItem>
        {
            new("Home", "/"),
            new("Components", "/Docs/Components/FileInput"),
            new("File Input")
        };
    }
}
