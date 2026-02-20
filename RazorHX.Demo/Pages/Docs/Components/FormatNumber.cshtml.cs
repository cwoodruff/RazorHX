using Microsoft.AspNetCore.Mvc.RazorPages;
using RazorHX.Components.Navigation;
using RazorHX.Demo.Models;

namespace RazorHX.Demo.Pages.Docs.Components;

public class FormatNumberModel : PageModel
{
    public List<ComponentProperty> Properties { get; } = new()
    {
        new("rhx-value", "double", "0", "The number to format"),
        new("rhx-type", "string", "decimal", "Format type: decimal, currency, percent"),
        new("rhx-currency", "string", "-", "ISO currency code (e.g. USD, EUR)"),
        new("rhx-currency-display", "string", "symbol", "Currency display: symbol, code, name"),
        new("rhx-lang", "string", "-", "Culture/locale (e.g. de-DE)"),
        new("rhx-no-grouping", "bool", "false", "Disable thousands grouping"),
        new("rhx-minimum-fraction-digits", "int?", "-", "Minimum decimal places"),
        new("rhx-maximum-fraction-digits", "int?", "-", "Maximum decimal places"),
    };

    public string DecimalCode => @"<rhx-format-number rhx-value=""1234.56"" />
<rhx-format-number rhx-value=""1234.56"" rhx-no-grouping=""true"" />
<rhx-format-number rhx-value=""1.5"" rhx-minimum-fraction-digits=""0""
                   rhx-maximum-fraction-digits=""3"" />
<rhx-format-number rhx-value=""1.0"" rhx-minimum-fraction-digits=""2""
                   rhx-maximum-fraction-digits=""2"" />";

    public string CurrencyCode => @"<rhx-format-number rhx-value=""1234.56"" rhx-type=""currency"" rhx-currency=""USD"" />
<rhx-format-number rhx-value=""1234.56"" rhx-type=""currency"" rhx-currency=""USD""
                   rhx-currency-display=""code"" />
<rhx-format-number rhx-value=""1234.56"" rhx-type=""currency"" rhx-currency=""USD""
                   rhx-currency-display=""name"" />
<rhx-format-number rhx-value=""1234"" rhx-type=""currency"" rhx-currency=""JPY"" />
<rhx-format-number rhx-value=""1234.56"" rhx-type=""currency"" rhx-currency=""EUR"" />
<rhx-format-number rhx-value=""1234.56"" rhx-type=""currency"" rhx-currency=""GBP"" />";

    public string PercentCode => @"<rhx-format-number rhx-value=""0.75"" rhx-type=""percent"" />
<rhx-format-number rhx-value=""0.756"" rhx-type=""percent""
                   rhx-maximum-fraction-digits=""1"" />";

    public string CultureCode => @"<rhx-format-number rhx-value=""1234.56"" rhx-lang=""de-DE"" />";

    public void OnGet()
    {
        ViewData["Breadcrumbs"] = new List<BreadcrumbItem>
        {
            new("Home", "/"),
            new("Components", "/Docs/Components/FormatNumber"),
            new("Format Number")
        };
    }
}
