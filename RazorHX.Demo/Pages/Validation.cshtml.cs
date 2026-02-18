using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace RazorHX.Demo.Pages;

public class ValidationModel : PageModel
{
    [BindProperty]
    public ContactForm Contact { get; set; } = new();

    public bool Submitted { get; set; }

    public void OnGet()
    {
    }

    public class ContactForm
    {
        [Required(ErrorMessage = "Name is required.")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Name must be between 2 and 100 characters.")]
        [Display(Name = "Full Name")]
        public string Name { get; set; } = "";

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Please enter a valid email address.")]
        public string Email { get; set; } = "";

        [Required(ErrorMessage = "Please select a subject.")]
        public string Subject { get; set; } = "";

        [Required(ErrorMessage = "Message is required.")]
        [StringLength(500, MinimumLength = 10, ErrorMessage = "Message must be between 10 and 500 characters.")]
        public string Message { get; set; } = "";

        [Range(1, 5, ErrorMessage = "Rating must be between 1 and 5.")]
        public int? Rating { get; set; }
    }
}
