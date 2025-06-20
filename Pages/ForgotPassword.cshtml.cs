using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using MovieTicketsBooking;
using MovieTicketsBookings.Models;

namespace MovieTicketsBooking.Pages
{
    public class ForgotPasswordModel : PageModel
    {
        private readonly EmailService _emailService;
        private readonly Prn222FinalProjectContext _context;

        public ForgotPasswordModel(EmailService emailService, Prn222FinalProjectContext context)
        {
            _emailService = emailService;
            _context = context;
        }

        [BindProperty]
        public string Email { get; set; }

        public string VerificationCode { get; set; }

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
            if (string.IsNullOrEmpty(Email))
            {
                ModelState.AddModelError("", "Email is required.");
                return Page();
            }

            var emaillExists = await _context.Users.AnyAsync(u => u.Email == Email);
            if (!emaillExists)
            {
                TempData["error"] = "Email không tồn tại.";
                return Page();
            }

            VerificationCode = GenerateVerificationCode();

            HttpContext.Session.SetString("VerificationCode", VerificationCode);
            HttpContext.Session.SetString("Email", Email);
            HttpContext.Session.SetString("VerificationCodeTime", DateTime.UtcNow.ToString());

            var subject = "Password Reset Verification Code";
            var message = $"Your verification code is: {VerificationCode}";
            await _emailService.SendEmailAsync(Email, subject, message);

            return RedirectToPage("VerifyCode", new { email = Email });

        }

        private string GenerateVerificationCode()
        {
            var random = new Random();
            return random.Next(100000, 999999).ToString(); // Tạo mã xác thực 6 chữ số
        }

    }
}
