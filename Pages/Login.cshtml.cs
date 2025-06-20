using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MovieTicketsBookings.Models; // Import user model
using System.Linq; // For LINQ queries
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace MovieTicketsBookings.Pages
{
    public class LoginModel : PageModel
    {
        private readonly Prn222FinalProjectContext _context;

        public Models.User User { get; set; }
        private IHttpContextAccessor Accessor;

        private IRequestCookieCollection Cookies
        {
            get
            {
                return Accessor.HttpContext.Request.Cookies;
            }
        }

        public LoginModel(Prn222FinalProjectContext context, IHttpContextAccessor accessor)
        {
            _context = context;
            Accessor = accessor;
        }

        [BindProperty]
        [Required(ErrorMessage = "Tên đăng nhập không được để trống")]
        [StringLength(50, ErrorMessage = "Tên đăng nhập không được vượt quá 50 ký tự")]
        [MinLength(6, ErrorMessage = "Tên đăng nhập phải có tối thiểu 6 ký tự")]
        public string Username { get; set; }


        [BindProperty]
        [Required(ErrorMessage = "Mật khẩu không được để trống")]
        [StringLength(100, ErrorMessage = "Mật khẩu phải có ít nhất 6 ký tự", MinimumLength = 6)]
        public string Password { get; set; }

        [BindProperty]
        public bool RememberMe { get; set; }

        public string ErrorMessage { get; set; }

        public IActionResult OnGet()
        {
            if (HttpContext.User.Identity != null && HttpContext.User.Identity.IsAuthenticated)
            {
                return RedirectToPage("/Index");
            }

            if (Cookies["UserName"] != null && Cookies["Password"] != null)
            {
                Username = Cookies["UserName"];
                Password = Cookies["Password"];
            }

            return Page();
        }


        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            if (string.IsNullOrEmpty(Username) || string.IsNullOrEmpty(Password))
            {
                TempData["error"] = "Tên đăng nhập và mật khẩu không được để trống.";
                return Page();
            }

            var user = _context.Users.FirstOrDefault(u => u.Username == Username);

            if (user == null || user.Password == null || !BCrypt.Net.BCrypt.Verify(Password, user.Password))
            {
                TempData["error"] = "Đăng nhập thất bại. Tên đăng nhập hoặc mật khẩu không đúng.";
                return Page();
            }

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

            if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }

            if (RememberMe)
            {
                CookieOptions options = new CookieOptions
                {
                    Expires = DateTime.Now.AddDays(30),
                    HttpOnly = true, // Để ngăn truy cập từ JavaScript
                    Secure = true // Bảo mật bằng HTTPS
                };

                Response.Cookies.Append("UserName", Username, options);
                Response.Cookies.Append("Password", Username, options);
            }
            else
            {
                Response.Cookies.Delete("UserName");
                Response.Cookies.Delete("Password");
            }

            switch (user.RoleId)
            {
                case 1:
                    return RedirectToPage("/HomeAdmin");
                case 2:
                    return RedirectToPage("/Index");
                case 3:
                    return RedirectToPage("/HomeOwner");
                case 4:
                    return RedirectToPage("/HomeStaff");
                default:
                    return Page();
            }
        }
    }
}
