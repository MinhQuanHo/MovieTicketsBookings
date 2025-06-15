using System;
using System.Collections.Generic;

namespace MovieTicketsBookings.Models;

public partial class MovieCategory
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public virtual ICollection<Movie> Movies { get; set; } = new List<Movie>();
}
