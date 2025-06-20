using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MovieTicketsBookings.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MMovieTicketsBookings.Pages.Room
{
    public class CreateRoomModel : PageModel
    {
        private readonly Prn222FinalProjectContext _context;

        public CreateRoomModel(Prn222FinalProjectContext context)
        {
            _context = context;
        }

        [BindProperty]
        public MovieTicketsBookings.Models.Room Room { get; set; } = new MovieTicketsBookings.Models.Room();

        [BindProperty]
        public List<RowInput> RowsInput { get; set; } = new List<RowInput>();

        public void OnGet()
        {
            // Initialize with at least one row input
            if (RowsInput.Count == 0)
            {
                RowsInput.Add(new RowInput());
            }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var cinemaId = HttpContext.Session.GetInt32("CinemaID");
            if (cinemaId == null)
            {
                return RedirectToPage("/Room/ManageRooms");
            }

            // Set CinemaId for the room
            Room.CinemaId = cinemaId.Value;

            // Create Rows collection from input
            Room.Rows = new List<Row>();
            foreach (var rowInput in RowsInput)
            {
                if (!string.IsNullOrEmpty(rowInput.RowName))
                {
                    Room.Rows.Add(new Row
                    {
                        RowName = rowInput.RowName,
                        Type = rowInput.Type,
                        NumberOfColumns = rowInput.NumberOfColumns
                    });
                }
            }

            // Update NumberOfRows based on actual rows added
            Room.NumberOfRows = Room.Rows.Count;

            _context.Rooms.Add(Room);
            await _context.SaveChangesAsync();

            return RedirectToPage("/Room/ManageRooms", new { id = cinemaId });
        }
    }

    // Helper class for row input binding
    public class RowInput
    {
        public string RowName { get; set; }
        public string Type { get; set; }
        public int NumberOfColumns { get; set; }
    }
}