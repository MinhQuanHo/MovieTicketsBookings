using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using MovieTicketsBookings.Models;
using System.Security.Claims;

namespace MovieTicketsBookings.Pages.Cinema
{
    public class CinemaDetailsModel : PageModel
    {
        private readonly Prn222FinalProjectContext _context;

        public CinemaDetailsModel(Prn222FinalProjectContext context)
        {
            _context = context;
        }

        public Models.Cinema Cinema { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            var roleIdClaim = User.FindFirst(ClaimTypes.Role)?.Value;

            if (string.IsNullOrEmpty(roleIdClaim) || roleIdClaim != "1")
            {
                return RedirectToPage("/AccessDenied");
            }

            if (id == null)
            {
                return NotFound();
            }

            Cinema = await _context.Cinemas.FirstOrDefaultAsync(c => c.Id == id);

            if (Cinema == null)
            {
                return NotFound();
            }

            return Page();
        }
    }
}
