using System;
using System.Collections.Generic;

namespace MovieTicketsBookings.Models;

public partial class Row
{
    public int Id { get; set; }

    public string? RowName { get; set; }

    public int? RoomId { get; set; }

    public int? NumberOfColumns { get; set; }

    public string? Type { get; set; }

    public virtual Room? Room { get; set; }

    public virtual ICollection<Seat> Seats { get; set; } = new List<Seat>();
}
