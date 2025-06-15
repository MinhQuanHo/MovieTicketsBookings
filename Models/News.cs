using System;
using System.Collections.Generic;

namespace MovieTicketsBookings.Models;

public partial class News
{
    public int Id { get; set; }

    public string? Title { get; set; }

    public string? Content { get; set; }

    public string? Image { get; set; }

    public DateTime? CreateAt { get; set; }

    public DateTime? UpdateAt { get; set; }

    public int? CreateBy { get; set; }

    public virtual User? CreateByNavigation { get; set; }
}
