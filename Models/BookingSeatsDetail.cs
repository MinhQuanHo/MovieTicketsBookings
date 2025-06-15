using System;
using System.Collections.Generic;

namespace MovieTicketsBookings.Models;

public partial class BookingSeatsDetail
{
    public int Id { get; set; }

    public int? SeatId { get; set; }

    public int? Price { get; set; }

    public int? BookingId { get; set; }

    public virtual Booking? Booking { get; set; }

    public virtual Seat? Seat { get; set; }
}
