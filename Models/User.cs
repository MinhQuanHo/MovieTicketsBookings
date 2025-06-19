using System;
using System.Collections.Generic;

namespace MovieTicketsOnlineBooking.Models;

public partial class User
{
    public int Id { get; set; }

    public int? RoleId { get; set; }

    public string Username { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string? Email { get; set; }

    public string? PhoneNumber { get; set; }

    public DateOnly? Dob { get; set; }

    public string? FullName { get; set; }

    public string? Status { get; set; }

    public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();

    public virtual ICollection<Feedback> Feedbacks { get; set; } = new List<Feedback>();

    public virtual ICollection<News> News { get; set; } = new List<News>();

    public virtual Role? Role { get; set; }
}
