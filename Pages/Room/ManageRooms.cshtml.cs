using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using MovieTicketsBookings.Models;

namespace MovieTicketsBookings.Pages.Room
{
    public class ManageRoomsModel : PageModel
    {
        private readonly Prn222FinalProjectContext _context;

        public ManageRoomsModel(Prn222FinalProjectContext context)
        {
            _context = context;
        }

        [BindProperty]
        public List<Models.Room> Rooms { get; set; } = new List<Models.Room>();

        public async Task OnGetAsync(int id)
        {
            // L?u CinemaID v�o Session ?? d�ng sau (v� d? khi t?o Room m?i)
            HttpContext.Session.SetInt32("CinemaID", id);

            // Truy v?n danh s�ch ph�ng chi?u c?a r?p c� ID = id
            Rooms = await _context.Rooms
                .Include(r => r.Cinema)
                .Where(r => r.CinemaId == id)
                .ToListAsync();
        }

        public async Task<IActionResult> OnPostManageSeatAsync(int id)
        {
            // L?u RoomID v�o Session v� chuy?n trang qu?n l� gh?
            HttpContext.Session.SetInt32("RoomID", id);
            return RedirectToPage("/Seat/ManageSeats");
        }
    }
}
