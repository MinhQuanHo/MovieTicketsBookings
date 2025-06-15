using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using MovieTicketsBookings.Models;

namespace MovieTicketsBookings.Pages.Seat
{
    public class AddSeatModel : PageModel
    {

        private readonly Prn222FinalProjectContext _context;
        public AddSeatModel(Prn222FinalProjectContext context)
        {
            _context = context;
        }
        [BindProperty]
        public List<Models.Row> Rows { get; set; }
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
            Rows = await _context.Rows.Include(r => r.Seats).Where(r => r.RoomId == roomID).ToListAsync();
            return Page();
        }
        public async Task<IActionResult> OnPostAsync()
        {
            var roomId = HttpContext.Session.GetInt32("RoomID");
            var seatNumber = Request.Form["SeatNumber"];
            var selectedRow = await _context.Rows.FindAsync(Seat.RowId);
            Seat.SeatName = selectedRow.RowName + seatNumber;

            var seat = await _context.Seats.FirstOrDefaultAsync(r => r.SeatName == Seat.SeatName);
            if (seat == null)
            {
                await _context.Seats.AddAsync(Seat);
                await _context.SaveChangesAsync();
            }
            else
            {
                return NotFound();
            }
            return RedirectToPage("/Seat/ManageSeats");
        }
    }
}
