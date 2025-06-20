using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MovieTicketsBooking.Pages
{
    public class VerifyCodeModel : PageModel
    {
        [BindProperty]
        public string Code { get; set; }

        public IActionResult OnGet()
        {
            if (HttpContext.User.Identity != null && HttpContext.User.Identity.IsAuthenticated)
            {
                return RedirectToPage("/Index");
            }

            return Page();
        }

        public IActionResult OnPost()
        {
            var storedCode = HttpContext.Session.GetString("VerificationCode");
            var email = HttpContext.Session.GetString("Email");
            var storedTimeStr = HttpContext.Session.GetString("VerificationCodeTime");

            if (string.IsNullOrEmpty(storedTimeStr))
            {
                ModelState.AddModelError("", "Verification code has expired.");
                return Page();
            }

            var storedTime = DateTime.Parse(storedTimeStr);
            var currentTime = DateTime.UtcNow;

            if (currentTime.Subtract(storedTime).TotalSeconds > 60)
            {
                ModelState.AddModelError("", "Verification code has expired.");
                return Page();
            }

            if (storedCode == Code)
            {
                return RedirectToPage("ResetPassword");
            }
            else
            {
                ModelState.AddModelError("", "Invalid verification code.");
                return Page();
            }

        }
    }
}
