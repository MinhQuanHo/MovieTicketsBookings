using System;
using System.Collections.Generic;

namespace MovieTicketsOnlineBooking.Models;

public partial class Seat
{
    public int Id { get; set; }

    public string? SeatName { get; set; }

    public int? RowId { get; set; }

    public string? Status { get; set; }

    public virtual ICollection<BookingSeatsDetail> BookingSeatsDetails { get; set; } = new List<BookingSeatsDetail>();

    public virtual Row? Row { get; set; }
}
