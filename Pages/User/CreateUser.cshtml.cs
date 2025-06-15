using MovieTicketsBookings.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BCrypt.Net;
using MovieTicketsBookings;


namespace MovieTicketsBookings.Pages.User
{
    public class CreateUserModel : PageModel
    {
        private readonly Prn222FinalProjectContext _context;
        private readonly EmailService _emailService;

        public CreateUserModel(Prn222FinalProjectContext context, EmailService emailService)
        {
            _context = context;
            _emailService = emailService;
        }
        [BindProperty]
        public Models.User NewUser { get; set; } = new Models.User();
        [BindProperty]
        public bool IsActivated { get; set; }
        [BindProperty]
        public string Msg { get; set; }
        public List<Role> Roles { get; set; } = new List<Role>();

        public async Task OnGetAsync()
        {
            Roles = await _context.Roles.ToListAsync();
        }
        public async Task<ActionResult> OnPostAsync()
        {
            var existingUserByEmail = await _context.Users
            .FirstOrDefaultAsync(u => u.Email == NewUser.Email);
            if (existingUserByEmail != null)
            {
                TempData["error"] = "Email đã tồn tại trong hệ thống!";
                Roles = await _context.Roles.ToListAsync();
                return Page();
            }

            var existingUserByUsername = await _context.Users
                .FirstOrDefaultAsync(u => u.Username == NewUser.Username);
            if (existingUserByUsername != null)
            {
                TempData["error"] = "Username đã tồn tại trong hệ thống!";
                Roles = await _context.Roles.ToListAsync();
                return Page();
            }

            NewUser.Status = IsActivated ? "Active" : "Inactive";
            string password = GeneratedPassword();
            SendEmail(NewUser.Username, password, NewUser.Email);
            string encryptPassword = BCrypt.Net.BCrypt.HashPassword(password);
            NewUser.Password = encryptPassword;
            NewUser.FullName = "New User";
            NewUser.Dob = DateTime.Now;
            NewUser.PhoneNumber = "0123456789";
            _context.Users.Add(NewUser);
            await _context.SaveChangesAsync();
            TempData["success"] = "Tạo tài khoản thành công!";
            return RedirectToPage("/User/ManageUsers");
        }

        private string GeneratedPassword()
        {
            var random = new Random();
            var password = string.Empty;
            for (int i = 0; i <= 6; i++)
            {
                int asciiValue = random.Next(32, 127);
                char randomChar = Convert.ToChar(asciiValue);
                password += randomChar;
            }
            return password;
        }
        private async void SendEmail(string username, string password, string email)
        {
            var subject = "Your account has been created.";
            var message = $"Username is: {username}, Password is: {password}.";
            await _emailService.SendEmailAsync(email, subject, message);
        }
    }
}
