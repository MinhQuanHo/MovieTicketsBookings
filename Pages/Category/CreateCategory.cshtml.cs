using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MovieTicketsBookings.Models;
using System.Security.Claims;

namespace MovieTicketsBookings.Pages.Category
{
    public class CreateCategoryModel : PageModel
    {
        private readonly Prn222FinalProjectContext _context;

        public CreateCategoryModel(Prn222FinalProjectContext context)
        {
            _context = context;
        }

        [BindProperty]
        public MovieCategory category { get; set; }

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

            _context.MovieCategories.Add(category);
            await _context.SaveChangesAsync();
            TempData["success"] = "Thể loại phim đã được thêm thành công.";

            return RedirectToPage("./ManageCategories");
        }
    }
}
