using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using MovieTicketsBookings.Models;
using System.Security.Claims;

namespace MovieTicketsBookings.Pages.Cinema
{
    public class ManageCinemasModel : PageModel
    {
        public List<Models.Cinema> cinemas = new List<Models.Cinema>();
        private readonly Prn222FinalProjectContext _context;

        [BindProperty(SupportsGet = true)]
        public string CurrentFilter { get; set; }

        public int PageSize { get; set; } = 5;
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }

        public ManageCinemasModel(Prn222FinalProjectContext context)
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

            IQueryable<Models.Cinema> cinemasQuery = _context.Cinemas;

            if (searchString != null)
            {
                CurrentFilter = searchString;
            }

            if (!string.IsNullOrEmpty(CurrentFilter))
            {
                cinemasQuery = cinemasQuery.Where(c => c.Name.Contains(CurrentFilter) || c.City.Contains(CurrentFilter));
            }

            var totalItems = await cinemasQuery.CountAsync();
            TotalPages = (int)Math.Ceiling(totalItems / (double)PageSize);

            CurrentPage = pageNumber > TotalPages ? 1 : pageNumber;

            cinemas = await cinemasQuery
                .OrderBy(c => c.Name)
                .Skip((CurrentPage - 1) * PageSize)
                .Take(PageSize)
                .ToListAsync();

            return Page();
        }
    }
}
