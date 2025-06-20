using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using MovieTicketsBookings.Models;

namespace MovieTicketsBooking.Pages
{
    public class ResetPasswordModel : PageModel
    {
        private readonly Prn222FinalProjectContext _context; // Truy cập trực tiếp vào DbContext

        public ResetPasswordModel(Prn222FinalProjectContext context)
        {
            _context = context;
        }

        [BindProperty]
        public string Password { get; set; }

        [BindProperty]
        public string ConfirmPassword { get; set; }

        public IActionResult OnGet()
        {
            if (HttpContext.User.Identity != null && HttpContext.User.Identity.IsAuthenticated)
            {
                return RedirectToPage("/Index");
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            // Kiểm tra nếu mật khẩu và xác nhận mật khẩu không khớp
            if (Password != ConfirmPassword)
            {
                ModelState.AddModelError("", "Passwords do not match.");
                return Page();
            }
            // Lấy email từ session
            var email = HttpContext.Session.GetString("Email");

            // Tìm người dùng trong cơ sở dữ liệu dựa trên email
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
            if (user == null)
            {
                ModelState.AddModelError("", "User not found.");
                return Page();
            }

            // Cập nhật mật khẩu mới cho người dùng
            user.Password = BCrypt.Net.BCrypt.HashPassword(Password);

            // Lưu thay đổi vào cơ sở dữ liệu
            await _context.SaveChangesAsync();

            // Điều hướng người dùng đến trang login sau khi cập nhật thành công
            return RedirectToPage("Login");
        }
    }
}
