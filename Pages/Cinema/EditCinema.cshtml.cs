
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using MovieTicketsBookings.Models;
using System.Security.Claims;

namespace MovieTicketsBookings.Pages.Cinema
{
    public class EditCinemaModel : PageModel
    {
        [BindProperty]
        public Models.Cinema cinema { get; set; }

        private readonly Prn222FinalProjectContext _context;

        public EditCinemaModel(Prn222FinalProjectContext context)
        {
            _context = context;
        }

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

            cinema = await _context.Cinemas.FirstOrDefaultAsync(m => m.Id == id);

            if (cinema == null)
            {
                return NotFound();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var existingCinema = await _context.Cinemas.AsNoTracking().FirstOrDefaultAsync(c => c.Id == cinema.Id);

            if (existingCinema == null)
            {
                return NotFound();
            }

            cinema.Status = existingCinema.Status;

            _context.Attach(cinema).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
                TempData["success"] = "Cinema updated successfully";
                return RedirectToPage("./ManageCinemas");
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CinemaExists(cinema.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
        }

        private bool CinemaExists(int id)
        {
            return _context.Cinemas.Any(e => e.Id == id);
        }
    }
}
