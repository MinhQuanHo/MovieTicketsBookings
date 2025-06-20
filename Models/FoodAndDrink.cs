using System;
using System.Collections.Generic;

namespace MovieTicketsOnlineBooking.Models;

public partial class FoodAndDrink
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public int? Price { get; set; }

    public int? Quantity { get; set; }

    public string? Image { get; set; }

    public string? Status { get; set; }

    public virtual ICollection<BookingItem> BookingItems { get; set; } = new List<BookingItem>();
}
