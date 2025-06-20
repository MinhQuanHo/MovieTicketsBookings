using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using MovieTicketsOnlineBooking.Models;
using System.Security.Claims;

namespace MovieTicketsOnlineBooking.Pages.Movie
{
    public class ManageMoviesModel : PageModel
    {
        public List<MovieCategory> categories { get; set; }
        public List<Models.Movie> movies = new List<Models.Movie>();

        [BindProperty(SupportsGet = true)]
        public string CurrentFilter { get; set; }

        [BindProperty(SupportsGet = true)]
        public int? CategoryFilter { get; set; }

        public int PageSize { get; set; } = 5;
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }

        private readonly Prn222FinalProjectContext _context;

        public ManageMoviesModel(Prn222FinalProjectContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> OnGetAsync(string searchString, int? categoryId, int pageNumber = 1)
        {
            var roleIdClaim = User.FindFirst(ClaimTypes.Role)?.Value;

            if (string.IsNullOrEmpty(roleIdClaim) || roleIdClaim != "1")
            {
                return RedirectToPage("/AccessDenied");
            }

            if (searchString != null)
            {
                CurrentFilter = searchString;
            }

            if (categoryId != null)
            {
                CategoryFilter = categoryId;
            }

            IQueryable<Models.Movie> movieQuery = _context.Movies
        .Include(m => m.Category)
        .Where(m => m.Status == "Active" || m.Status == "Inactive");

            if (!string.IsNullOrEmpty(CurrentFilter))
            {
                movieQuery = movieQuery.Where(f => f.Title.Contains(CurrentFilter));
            }

            if (CategoryFilter.HasValue)
            {
                movieQuery = movieQuery.Where(m => m.CategoryId == CategoryFilter.Value);
            }

            var totalItems = await movieQuery.CountAsync();
            TotalPages = (int)Math.Ceiling(totalItems / (double)PageSize);
            CurrentPage = pageNumber;

            movies = await movieQuery
                .OrderBy(c => c.Title)
                .Skip((CurrentPage - 1) * PageSize)
                .Take(PageSize)
                .ToListAsync();

            categories = await _context.MovieCategories.ToListAsync();

            return Page();
        }
    }
}
