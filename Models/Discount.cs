using System;
using System.Collections.Generic;

namespace MovieTicketsBookings.Models;

public partial class Discount
{
    public int Id { get; set; }

    public string? Code { get; set; }

    public int? DiscountValue { get; set; }

    public DateOnly? StartDate { get; set; }

    public DateOnly? EndDate { get; set; }

    public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();
}
