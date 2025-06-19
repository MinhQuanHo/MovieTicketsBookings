using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MovieTicketsBookings.Models;
using System.Security.Claims;


namespace MovieTicketsBookings.Pages.Movie
{
    public class CreateMovieModel : PageModel
    {
        public List<MovieCategory> categories { get; set; }
        private readonly Prn222FinalProjectContext _context;
        private readonly IWebHostEnvironment _environment;

        public CreateMovieModel(Prn222FinalProjectContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        [BindProperty]
        public Models.Movie movie { get; set; }

        [BindProperty]
        public IFormFile ImageFile { get; set; }

        [BindProperty]
        public int DurationHours { get; set; }

        [BindProperty]
        public int DurationMinutes { get; set; }

        public IActionResult OnGet()
        {
            var roleIdClaim = User.FindFirst(ClaimTypes.Role)?.Value;

            if (string.IsNullOrEmpty(roleIdClaim) || roleIdClaim != "1")
            {
                return RedirectToPage("/AccessDenied");
            }

            categories = _context.MovieCategories.ToList();
            return Page();
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                categories = _context.MovieCategories.ToList();
                return Page();
            }

            if (ImageFile != null)
            {
                var originalFileName = Path.GetFileNameWithoutExtension(ImageFile.FileName);
                var fileExtension = Path.GetExtension(ImageFile.FileName);
                var fileName = $"{Guid.NewGuid()}_{originalFileName}{fileExtension}";
                var filePath = Path.Combine(_environment.WebRootPath, "img", fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    ImageFile.CopyTo(stream);
                }

                movie.Poster = fileName;
            }

            movie.Status = "Active";

            movie.Duration = new TimeSpan(DurationHours, DurationMinutes, 0);

            _context.Movies.Add(movie);
            _context.SaveChanges();
            TempData["success"] = "Movie created successfully";

            return RedirectToPage("./ManageMovies");
        }
    }
}
