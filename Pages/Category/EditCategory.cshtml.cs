using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using MovieTicketsBookings.Models;
using System.Security.Claims;

namespace MovieTicketsBookings.Pages.Category
{
    public class EditCategoryModel : PageModel
    {
        [BindProperty]
        public MovieCategory category { get; set; }

        private readonly Prn222FinalProjectContext _context;

        public EditCategoryModel(Prn222FinalProjectContext context) 
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

            category = await _context.MovieCategories.FirstOrDefaultAsync(m => m.Id == id);

            if (category == null)
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

            _context.Attach(category).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            TempData["success"] = "Thể loại được chỉnh sửa thành công.";
            return RedirectToPage("./ManageCategories");
        }
    }
}

