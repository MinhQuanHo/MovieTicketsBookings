using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using MovieTicketsBookings.Models;
using System.Security.Claims;

namespace MovieTicketsBookings.Pages.Movie
{
    public class EditMovieModel : PageModel
    {
        private readonly Prn222FinalProjectContext _context;
        private readonly IWebHostEnvironment _environment;

        [BindProperty]
        public Models.Movie movie { get; set; }

        [BindProperty]
        public IFormFile? ImageFile { get; set; }

        [BindProperty]
        public int DurationHours { get; set; }

        [BindProperty]
        public int DurationMinutes { get; set; }

        public List<MovieCategory> categories { get; set; }

        public EditMovieModel(Prn222FinalProjectContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var roleIdClaim = User.FindFirst(ClaimTypes.Role)?.Value;

            if (string.IsNullOrEmpty(roleIdClaim) || roleIdClaim != "1")
            {
                return RedirectToPage("/AccessDenied");
            }

            movie = await _context.Movies.AsNoTracking().FirstOrDefaultAsync(m => m.Id == id);
            categories = _context.MovieCategories.ToList();

            if (movie == null)
            {
                return NotFound();
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                categories = _context.MovieCategories.ToList();
                return Page();
            }

            var movieToUpdate = await _context.Movies.AsNoTracking().FirstOrDefaultAsync(m => m.Id == movie.Id);

            if (movieToUpdate == null)
            {
                return NotFound();
            }

            movie.Status = movieToUpdate.Status;
            movie.Poster = movieToUpdate.Poster;

            movieToUpdate.Title = movie.Title;
            movieToUpdate.ReleaseDate = movie.ReleaseDate;
            movieToUpdate.Actor = movie.Actor;
            movieToUpdate.Director = movie.Director;
            movieToUpdate.Description = movie.Description;
            movieToUpdate.TrailerUrl = movie.TrailerUrl;
            movieToUpdate.Status = movie.Status;
            movieToUpdate.CategoryId = movie.CategoryId;
            movieToUpdate.Duration = movie.Duration;

            if (ImageFile != null)
            {
                var originalFileName = Path.GetFileNameWithoutExtension(ImageFile.FileName);
                var fileExtension = Path.GetExtension(ImageFile.FileName);
                var fileName = $"{Guid.NewGuid()}_{originalFileName}{fileExtension}";
                var filePath = Path.Combine(_environment.WebRootPath, "img", fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await ImageFile.CopyToAsync(stream);
                }

                if (!string.IsNullOrEmpty(movieToUpdate.Poster))
                {
                    var oldImagePath = Path.Combine(_environment.WebRootPath, "img", movieToUpdate.Poster);
                    if (System.IO.File.Exists(oldImagePath))
                    {
                        System.IO.File.Delete(oldImagePath);
                    }
                }

                movieToUpdate.Poster = fileName;
            }

            _context.Attach(movieToUpdate).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
                TempData["success"] = "Movie updated successfully";
                return RedirectToPage("./ManageMovies");
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MovieExists(movie.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
        }

        private bool MovieExists(int id)
        {
            return _context.Movies.Any(m => m.Id == id);
        }
    }
}
