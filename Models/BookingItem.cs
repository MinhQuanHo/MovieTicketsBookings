using System;
using System.Collections.Generic;

namespace MovieTicketsBookings.Models
{

    public partial class BookingItem
    {
        public int Id { get; set; }

        public int? FoodAndDrinksId { get; set; }

        public int? Quantity { get; set; }

        public int? Price { get; set; }

        public int? BookingId { get; set; }

        public virtual Booking? Booking { get; set; }

        public virtual FoodAndDrink? FoodAndDrinks { get; set; }
    }
}