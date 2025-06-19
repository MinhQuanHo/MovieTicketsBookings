using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using MovieTicketsOnlineBooking.Models;

namespace MovieTicketsOnlineBooking.Pages.Movie
{
    public class ChangeStatusModel : PageModel
    {
        private readonly Prn222FinalProjectContext _context;

        public ChangeStatusModel(Prn222FinalProjectContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Models.Movie Movie { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Movie = await _context.Movies.FirstOrDefaultAsync(m => m.Id == id);

            if (Movie == null)
            {
                return NotFound();
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (Movie == null || Movie.Id == 0)
            {
                return NotFound();
            }

            try
            {
                var movieToUpdate = await _context.Movies.FindAsync(Movie.Id);

                if (movieToUpdate != null)
                {
                    movieToUpdate.Status = movieToUpdate.Status == "Active" ? "Inactive" : "Active";
                    _context.Update(movieToUpdate);
                    await _context.SaveChangesAsync();

                    TempData["success"] = $"Movie status has been changed to {movieToUpdate.Status} successfully";
                    return RedirectToPage("./MovieDetailsAdmin", new { id = Movie.Id });
                }

                return NotFound();
            }
            catch (Exception ex)
            {
                TempData["error"] = "An error occurred while changing the movie status. Please try again.";
                return Page();
            }
        }
    }
}
