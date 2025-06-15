using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using MovieTicketsBookings.Models;

namespace MovieTicketsBookings.Pages.User
{
    public class ManageUsersModel : PageModel
    {
        private readonly Prn222FinalProjectContext _projectContext;

        public List<Models.User> UsersSource { get; set; }

        public ManageUsersModel(Prn222FinalProjectContext projectContext)
        {
            _projectContext = projectContext;
        }

        [BindProperty(SupportsGet = true)]
        public string SearchTerm {  get; set; }

        [BindProperty(SupportsGet = true)]
        public string SelectedStatus {  get; set; }

        [BindProperty(SupportsGet = true)]
        public string SelectedRole { get; set; }

        [BindProperty]
        public string Msg {  get; set; }

        [BindProperty(SupportsGet = true)]
        public int CurrentPage { get; set; } = 1;
        public int PageSize { get; set; } = 5;
        public int TotalPages { get; set; }
        public IList<Models.User> Users { get; set; }
        public bool IsSearchPerformed { get; set; } = false;

        public async Task OnGetAsync()
        {
            await LoadUsersAsync(false);
        }

        public async Task OnPostSearchAsync()
        {
            await LoadUsersAsync(true);
        }

        private async Task LoadUsersAsync(bool isSearch)
        {
            UsersSource = await _projectContext.Users.Include(r => r.Role).ToListAsync();

            var listUsers = UsersSource
                .Where(item => (string.IsNullOrWhiteSpace(SearchTerm) || item.FullName.Contains(SearchTerm, StringComparison.OrdinalIgnoreCase)
                || item.Email.Contains(SearchTerm, StringComparison.OrdinalIgnoreCase)) &&
                               (string.IsNullOrWhiteSpace(SelectedRole) || item.Role.RoleName.Equals(SelectedRole, StringComparison.OrdinalIgnoreCase)) &&
                               (string.IsNullOrWhiteSpace(SelectedStatus) || item.Status.Equals(SelectedStatus, StringComparison.OrdinalIgnoreCase)));

            int totalUsers = listUsers.Count();
            TotalPages = (int)Math.Ceiling(totalUsers / (double)PageSize);

            Users = listUsers
                .Skip((CurrentPage - 1) * PageSize)
                .Take(PageSize)
                .ToList();

            if (isSearch && (!string.IsNullOrWhiteSpace(SearchTerm) ||
                        !string.IsNullOrWhiteSpace(SelectedStatus) ||
                        !string.IsNullOrWhiteSpace(SelectedRole)))
            {
                Msg = totalUsers + " records found";
            }
            else
            {
                Msg = string.Empty;
            }
        }

        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            var user = await _projectContext.Users.FindAsync(id);

            if (user != null)
            {
                _projectContext.Users.Remove(user);
                await _projectContext.SaveChangesAsync();
                TempData["success"] = $"Ng??i dùng v?i ID {id} ?ã ???c xóa thành công.";
            }
            else
            {
                TempData["error"] = "User not found.";
            }

            return RedirectToPage();
        }


    }
}
