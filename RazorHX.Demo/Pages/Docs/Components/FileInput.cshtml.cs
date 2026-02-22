using Microsoft.AspNetCore.Mvc;
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

    public string HtmxCode => @"<form id=""upload-form"" hx-encoding=""multipart/form-data"">
    <rhx-file-input name=""uploadFile"" rhx-label=""Upload a File""
                    rhx-accept="".pdf,.jpg,.png,.txt""
                    rhx-hint=""PDF, JPG, PNG, or TXT. Max 10MB."" />
</form>
<rhx-button rhx-variant=""brand""
            hx-post=""/Docs/Components/FileInput?handler=Upload""
            hx-include=""#upload-form""
            hx-target=""#upload-result""
            hx-encoding=""multipart/form-data"">
    Upload
</rhx-button>
<div id=""upload-result"">Select a file and click upload...</div>";

    public void OnGet()
    {
        ViewData["Breadcrumbs"] = new List<BreadcrumbItem>
        {
            new("Home", "/"),
            new("Components", "/Docs/Components/FileInput"),
            new("File Input")
        };
    }

    public IActionResult OnPostUpload(IFormFile? uploadFile)
    {
        if (uploadFile is null || uploadFile.Length == 0)
        {
            return Content("<span style=\"color: var(--rhx-color-text-muted);\">Please select a file to upload.</span>", "text/html");
        }

        var sizeMb = uploadFile.Length / (1024.0 * 1024.0);
        var sizeDisplay = sizeMb >= 1 ? $"{sizeMb:F2} MB" : $"{uploadFile.Length / 1024.0:F1} KB";
        var name = System.Net.WebUtility.HtmlEncode(uploadFile.FileName);
        var type = System.Net.WebUtility.HtmlEncode(uploadFile.ContentType);

        return Content($"""
            <div style="padding: var(--rhx-space-md); background: var(--rhx-color-surface-raised); border-radius: var(--rhx-radius-md); color: var(--rhx-color-text-muted);">
                <strong>File uploaded successfully!</strong>
                <ul style="list-style: none; padding: 0; margin: var(--rhx-space-xs) 0 0 0;">
                    <li>Name: <strong>{name}</strong></li>
                    <li>Size: <strong>{sizeDisplay}</strong></li>
                    <li>Type: <strong>{type}</strong></li>
                </ul>
            </div>
            """, "text/html");
    }
}
