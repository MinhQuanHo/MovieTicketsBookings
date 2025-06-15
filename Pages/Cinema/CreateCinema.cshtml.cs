
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MovieTicketsBookings.Models;
using System.Security.Claims;

namespace MovieTicketsBookings.Pages.Cinema
{
    public class CreateCinemaModel : PageModel
    {
        private readonly Prn222FinalProjectContext _context;

        public CreateCinemaModel(Prn222FinalProjectContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Models.Cinema cinema { get; set; }

        public IActionResult OnGet()
        {
            var roleIdClaim = User.FindFirst(ClaimTypes.Role)?.Value;

            if (string.IsNullOrEmpty(roleIdClaim) || roleIdClaim != "1")
            {
                return RedirectToPage("/AccessDenied");
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            cinema.Status = "Active";

            _context.Cinemas.Add(cinema);
            await _context.SaveChangesAsync();
            TempData["success"] = "Cinema created successfully";

            return RedirectToPage("./ManageCinemas");
        }
    }
}
