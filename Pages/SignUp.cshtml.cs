using BCrypt.Net;
using MovieTicketsBookings.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MovieTicketsBookings.Pages
{
    public class SignUpModel : PageModel
    {
        private readonly Prn222FinalProjectContext _context;

        public SignUpModel(Prn222FinalProjectContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Models.User InputUser { get; set; } = new Models.User();

        [BindProperty]
        public bool RememberMe { get; set; } // Thêm Remember Me

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            // Kiểm tra xem user đã tồn tại chưa
            bool userExists = _context.Users.Any(u => u.Username == InputUser.Username || u.Email == InputUser.Email);
            if (userExists)
            {
                TempData["error"] = "Tên đăng nhập hoặc email đã tồn tại.";
                return Page();
            }

            // Mã hóa mật khẩu
            string encryptPassword = BCrypt.Net.BCrypt.HashPassword(InputUser.Password);

            var user = new Models.User
            {
                Username = InputUser.Username,
                FullName = InputUser.FullName,
                Dob = InputUser.Dob,
                PhoneNumber = InputUser.PhoneNumber,
                Email = InputUser.Email,
                Password = encryptPassword,
                Status = "Active",
                RoleId = 2
            };

            // Lưu vào database
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            // Tạo Claims để đăng nhập ngay sau khi đăng ký
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Username),
                new Claim("UserId", user.Id.ToString()),
                new Claim("FullName", user.FullName),
                new Claim(ClaimTypes.Role, user.RoleId.ToString()),
                new Claim("Email", user.Email),
                new Claim("PhoneNumber", user.PhoneNumber),
                new Claim("Password", user.Password),
                new Claim(ClaimTypes.DateOfBirth, user.Dob.HasValue ? user.Dob.Value.ToString("yyyy-MM-dd") : string.Empty),
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity));

            // Lưu Cookies nếu người dùng chọn "Nhớ tài khoản"
            if (RememberMe)
            {
                CookieOptions options = new CookieOptions
                {
                    Expires = DateTime.Now.AddDays(30),
                    HttpOnly = true,
                    Secure = true
                };

                Response.Cookies.Append("UserName", user.Username, options);
                Response.Cookies.Append("Password", InputUser.Password, options);
            }

            TempData["success"] = "Bạn đã tạo tài khoản thành công";

            // Điều hướng sau khi đăng ký thành công
            return RedirectToPage("/Index");
        }
    }
}
