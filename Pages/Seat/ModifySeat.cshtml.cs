using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using MovieTicketsBookings.Models;

namespace MovieTicketsBookings.Pages.Seat
{
    public class ModifySeatModel : PageModel
    {
        private readonly Prn222FinalProjectContext _context;
        public ModifySeatModel(Prn222FinalProjectContext context)
        {
            _context = context;
        }
        [BindProperty]
        public List<Models.Seat> Seats { get; set; }
        [BindProperty]
        public Models.Seat Seat { get; set; } = new Models.Seat();
        public async Task<IActionResult> OnGetAsync()
        {
            if (!User.Identity.IsAuthenticated)
            {
                var returnURl = Url.Page("/HomeOwner");
                return RedirectToPage("/Login", new { returnURl = returnURl });
            }
            var roomID = HttpContext.Session.GetInt32("RoomID");
            Seats = await _context.Seats.Include(s => s.Row).Where(s => s.Row.RoomId == roomID).ToListAsync();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var roomID = HttpContext.Session.GetInt32("RoomID");
            var seat = _context.Seats.Include(s => s.Row).FirstOrDefault(s => s.Row.RoomId == roomID && s.SeatName == Seat.SeatName);
            if (seat != null)
            {
                seat.Status = Seat.Status;
                _context.Seats.Update(seat);
            }
            await _context.SaveChangesAsync();
            return RedirectToPage("/Seat/ManageSeats");
        }
    }
}
