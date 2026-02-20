using Microsoft.AspNetCore.Mvc.RazorPages;
using RazorHX.Components.Navigation;
using RazorHX.Demo.Models;

namespace RazorHX.Demo.Pages.Docs.Components;

public class CardModel : PageModel
{
    public List<ComponentProperty> Properties { get; } = new()
    {
        new("(slot)", "rhx-card-header", "-", "Optional header section rendered at the top of the card"),
        new("(slot)", "rhx-card-footer", "-", "Optional footer section rendered at the bottom of the card"),
        new("(slot)", "rhx-card-image", "-", "Optional top image with rhx-src and rhx-alt attributes"),
    };

    public string BasicCode => @"<rhx-card>
    <rhx-card-header><h3 style=""margin:0;"">Product Name</h3></rhx-card-header>
    <p>A short description of the product. Great for showcasing items in a grid layout.</p>
    <rhx-card-footer>
        <rhx-button rhx-variant=""brand"" rhx-size=""small"">Buy Now</rhx-button>
        <rhx-button rhx-variant=""ghost"" rhx-size=""small"">Details</rhx-button>
    </rhx-card-footer>
</rhx-card>";

    public string ImageCode => @"<rhx-card>
    <rhx-card-image rhx-src=""https://picsum.photos/640/360"" rhx-alt=""Random photo"" />
    <rhx-card-header><h3 style=""margin:0;"">Featured Article</h3></rhx-card-header>
    <p>Lorem ipsum dolor sit amet, consectetur adipiscing elit. Vivamus lacinia odio vitae vestibulum.</p>
    <rhx-card-footer>
        <rhx-button rhx-variant=""ghost"" rhx-size=""small"">Read More</rhx-button>
    </rhx-card-footer>
</rhx-card>";

    public string MinimalCode => @"<rhx-card>
    <p>A simple card with just body content. No header, footer, or image required.</p>
</rhx-card>";

    public string GridCode => @"<div style=""display: grid; grid-template-columns: repeat(auto-fill, minmax(280px, 1fr)); gap: var(--rhx-space-lg);"">
    <rhx-card>
        <rhx-card-header><h3 style=""margin:0;"">Card 1</h3></rhx-card-header>
        <p>Card 1 content goes here with some placeholder text.</p>
        <rhx-card-footer>
            <rhx-button rhx-variant=""ghost"" rhx-size=""small"">Action</rhx-button>
        </rhx-card-footer>
    </rhx-card>
    <rhx-card>
        <rhx-card-header><h3 style=""margin:0;"">Card 2</h3></rhx-card-header>
        <p>Card 2 content goes here with some placeholder text.</p>
        <rhx-card-footer>
            <rhx-button rhx-variant=""ghost"" rhx-size=""small"">Action</rhx-button>
        </rhx-card-footer>
    </rhx-card>
    <rhx-card>
        <rhx-card-header><h3 style=""margin:0;"">Card 3</h3></rhx-card-header>
        <p>Card 3 content goes here with some placeholder text.</p>
        <rhx-card-footer>
            <rhx-button rhx-variant=""ghost"" rhx-size=""small"">Action</rhx-button>
        </rhx-card-footer>
    </rhx-card>
</div>";

    public void OnGet()
    {
        ViewData["Breadcrumbs"] = new List<BreadcrumbItem>
        {
            new("Home", "/"),
            new("Components", "/Docs/Components/Card"),
            new("Card")
        };
    }
}
