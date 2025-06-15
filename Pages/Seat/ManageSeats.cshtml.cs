using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using MovieTicketsBookings.Models;

namespace MovieTicketsBookings.Pages.Seat
{
    public class ManageSeatsModel : PageModel
    {
        private readonly Prn222FinalProjectContext _context;

        public ManageSeatsModel(Prn222FinalProjectContext context)
        {
            _context = context;
        }
        [BindProperty]
        public Models.Seat Seat { get; set; } = new Models.Seat();
        [BindProperty]
        public List<Models.Row> Rows { get; set; } = new List<Models.Row>();

        public async Task<ActionResult> OnGetAsync()
        {
            if (!User.Identity.IsAuthenticated)
            {
                var returnURl = Url.Page("/HomeOwner");
                return RedirectToPage("/Login", new { returnURl = returnURl });
            }
            var Id = HttpContext.Session.GetInt32("RoomID");
            if (Id == null)
            {
                return RedirectToPage("/Cinema/ManageCinemas");
            }
            Rows = await _context.Rows.Include(row => row.Seats).Where(row => row.RoomId == Id).ToListAsync();
            return Page();
        }
    }
}
