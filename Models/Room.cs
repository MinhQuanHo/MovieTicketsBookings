using System;
using System.Collections.Generic;

namespace MovieTicketsBookings.Models;

public partial class Room
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public int? CinemaId { get; set; }

    public int? NumberOfRows { get; set; }

    public virtual Cinema? Cinema { get; set; }

    public virtual ICollection<Row> Rows { get; set; } = new List<Row>();

    public virtual ICollection<Showtime> Showtimes { get; set; } = new List<Showtime>();
}
