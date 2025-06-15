using System;
using System.Collections.Generic;

namespace MovieTicketsBookings.Models;

public partial class Revenue
{
    public int Id { get; set; }

    public int? CinemaId { get; set; }

    public int? Quarter { get; set; }

    public int? Year { get; set; }

    public int? TotalRevenue { get; set; }

    public virtual Cinema? Cinema { get; set; }
}
