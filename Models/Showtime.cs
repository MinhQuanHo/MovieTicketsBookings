using System;
using System.Collections.Generic;

namespace MovieTicketsBookings.Models;

public partial class Showtime
{
    public int Id { get; set; }

    public int? MovieId { get; set; }

    public int? RoomId { get; set; }

    public TimeOnly? StartTime { get; set; }

    public TimeOnly? EndTime { get; set; }

    public DateTime? Date { get; set; }

    public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();

    public virtual Movie? Movie { get; set; }

    public virtual Room? Room { get; set; }
}
