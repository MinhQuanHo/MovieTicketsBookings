using System;
using System.Collections.Generic;

namespace MovieTicketsBookings.Models 
{ 

public partial class Booking
{
    public int Id { get; set; }

    public int? UserId { get; set; }

    public int? ShowtimeId { get; set; }

    public DateTime? BookingDate { get; set; }

    public string? Status { get; set; }

    public int? TotalPrice { get; set; }

    public string? TicketCode { get; set; }

    public virtual ICollection<BookingItem> BookingItems { get; set; } = new List<BookingItem>();

    public virtual ICollection<BookingSeatsDetail> BookingSeatsDetails { get; set; } = new List<BookingSeatsDetail>();

    public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();

    public virtual Showtime? Showtime { get; set; }

    public virtual User? User { get; set; }
}
}