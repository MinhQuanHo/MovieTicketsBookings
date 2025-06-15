using System;
using System.Collections.Generic;

namespace MovieTicketsBookings.Models;

public partial class Cinema
{
    public int Id { get; set; }

    public string? Location { get; set; }

    public string? City { get; set; }

    public string? Name { get; set; }

    public string? Status { get; set; }

    public virtual ICollection<Revenue> Revenues { get; set; } = new List<Revenue>();

    public virtual ICollection<Room> Rooms { get; set; } = new List<Room>();
}
