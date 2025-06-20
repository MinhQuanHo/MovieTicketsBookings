using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using MovieTicketsBookings.Models;

namespace MovieTicketsBookings.Pages.Category
{
    public class ManageCategoryModel : PageModel
    {
        public List<Models.MovieCategory> categories = new List<Models.MovieCategory>();
        private readonly Prn222FinalProjectContext _context;
         
        [BindProperty(SupportsGet = true)]
        public string CurrentFilter { get; set; }

        public int PageSize { get; set; } = 5;
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }

        public ManageCategoryModel(Prn222FinalProjectContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> OnGetAsync(string searchString, int pageNumber = 1)
        {
            var roleIdClaim = User.FindFirst(ClaimTypes.Role)?.Value;

            if (string.IsNullOrEmpty(roleIdClaim) || roleIdClaim != "1")
            {
                return RedirectToPage("/AccessDenied");
            }

            IQueryable<MovieCategory> categoriesQuery = _context.MovieCategories;

            if (searchString != null)
            {
                CurrentFilter = searchString;
            }

            if (!string.IsNullOrEmpty(CurrentFilter))
            {
                categoriesQuery = categoriesQuery.Where(c => c.Name.Contains(CurrentFilter));
            }

            var totalItems = await categoriesQuery.CountAsync();
            TotalPages = (int)Math.Ceiling(totalItems / (double)PageSize);

            CurrentPage = pageNumber > TotalPages ? 1 : pageNumber;

            categories = await categoriesQuery
                .OrderBy(c => c.Id)
                .Skip((CurrentPage - 1) * PageSize)
                .Take(PageSize)
                .ToListAsync();

            return Page();
        }
    }
}
