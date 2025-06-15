using MovieTicketsBookings.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using MovieTicketsBookings.Models;

namespace MovieTicketsBookings.Pages.User
{
    public class EditUserModel : PageModel
    {
        private readonly Prn222FinalProjectContext _context;
        public EditUserModel(Prn222FinalProjectContext context)
        {
            _context = context;
        }
        [BindProperty]
        public Models.User User { get; set; }

        [BindProperty]
        public bool IsActived { get; set; }
        public async Task OnGetAsync(int id)
        {
            User = await _context.Users.Include(r => r.Role).FirstOrDefaultAsync(u => u.Id == id);
            IsActived = User.Status == "Active" ? true : false;
        }

        public async Task<IActionResult> OnPostAsync(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user != null)
            {
                user.Status = IsActived ? "Active" : "Inactive"; // Đánh dấu là đã sửa đổi
                _context.Users.Update(user);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("/User/ManageUsers");
        }
    }
}
