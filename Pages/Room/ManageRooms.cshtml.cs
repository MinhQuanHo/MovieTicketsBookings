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
            // L?u CinemaID vào Session ?? dùng sau (ví d? khi t?o Room m?i)
            HttpContext.Session.SetInt32("CinemaID", id);

            // Truy v?n danh sách phòng chi?u c?a r?p có ID = id
            Rooms = await _context.Rooms
                .Include(r => r.Cinema)
                .Where(r => r.CinemaId == id)
                .ToListAsync();
        }

        public async Task<IActionResult> OnPostManageSeatAsync(int id)
        {
            // L?u RoomID vào Session và chuy?n trang qu?n lý gh?
            HttpContext.Session.SetInt32("RoomID", id);
            return RedirectToPage("/Seat/ManageSeats");
        }
    }
}
