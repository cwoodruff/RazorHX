using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace htmxRazor.Project.Pages;

public class IndexModel : PageModel
{
    public void OnGet() { }

    public IActionResult OnGetHelloWorld()
    {
        return Content(
            "<rhx-callout rhx-variant='success'>Hello from the server! This was loaded via htmx.</rhx-callout>",
            "text/html");
    }
}
