using System;
using System.Collections.Generic;

namespace MovieTicketsBookings.Models;

public partial class Payment
{
    public int Id { get; set; }

    public int? BookingId { get; set; }

    public int? Amount { get; set; }

    public int? DiscountId { get; set; }

    public virtual Booking? Booking { get; set; }

    public virtual Discount? Discount { get; set; }
}
