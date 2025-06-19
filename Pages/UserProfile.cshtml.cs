using MovieTicketsBookings.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace MovieTicketsBookings.Pages
{
    public class UserProfileModel : PageModel
    {
        private readonly Prn222FinalProjectContext _context;
        public UserProfileModel(Prn222FinalProjectContext context)
        {
            _context = context;
        }

        [BindProperty]
        [Required(ErrorMessage = "Họ và tên không được để trống")]
        [StringLength(100, ErrorMessage = "Họ và tên không được vượt quá 100 ký tự")]
        [MinLength(4, ErrorMessage = "Tên đăng nhập phải có tối thiểu 4 ký tự")]
        public string? FullName { get; set; }

        public string? Username { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "Email không được để trống")]
        [EmailAddress(ErrorMessage = "Email không hợp lệ")]
        public string? Email { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "Số điện thoại không được để trống")]
        [Phone(ErrorMessage = "Số điện thoại không hợp lệ")]
        public string? PhoneNumber { get; set; }

        public string? Password { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "Ngày sinh không được để trống")]
        [Range(typeof(DateTime), "1920-01-01", "2020-12-31", ErrorMessage = "Ngày sinh không hợp lệ")]
        [DataType(DataType.Date)]
        public DateTime? Dob { get; set; }

        public string? Role { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "Mật khẩu không được để trống")]
        [StringLength(100, ErrorMessage = "Mật khẩu phải có ít nhất 6 ký tự", MinimumLength = 6)]
        public string? NewPassword { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "Mật khẩu xác nhận không được để trống")]
        [StringLength(100, ErrorMessage = "Mật khẩu phải có ít nhất 6 ký tự", MinimumLength = 6)]
        [Compare("NewPassword", ErrorMessage = "Mật khẩu xác nhận không khớp")]
        public string? ConfirmPassword { get; set; }

        public void OnGet()
        {
            var userClaims = User.Claims;
            var username = userClaims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;
            if (username != null)
            {
                var user = _context.Users.FirstOrDefault(u => u.Username == username);
                if (user != null)
                {
                    FullName = user.FullName;
                    Email = user.Email;
                    PhoneNumber = user.PhoneNumber;

                    // Chuyển đổi từ DateOnly? -> DateTime?
                    //   Dob = user.Dob.HasValue ? user.Dob.Value.ToDateTime(TimeOnly.MinValue) : null;
                    Dob = user.Dob;
                }
            }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var userClaims = User.Claims;
            var username = userClaims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;

            if (username == null) return NotFound();

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
            if (user == null) return NotFound();

            var emailExists = await _context.Users.AnyAsync(u => u.Email == Email && u.Username != username);
            if (emailExists)
            {
                ModelState.AddModelError("Email", "Email đã tồn tại.");
                return Page();
            }

            user.FullName = FullName;
            user.Email = Email;
            user.PhoneNumber = PhoneNumber;

            // Chuyển đổi từ DateTime? -> DateOnly?
            //  user.Dob = Dob.HasValue ? DateOnly.FromDateTime(Dob.Value) : null;
            user.Dob = Dob;
            if (!string.IsNullOrEmpty(NewPassword) && NewPassword == ConfirmPassword)
            {
                user.Password = BCrypt.Net.BCrypt.HashPassword(NewPassword);
            }

            await _context.SaveChangesAsync();

            TempData["success"] = "Thông tin của bạn đã được cập nhật thành công.";

            return RedirectToPage("/UserProfile");
        }
    }
}
