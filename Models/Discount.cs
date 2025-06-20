using System;
using System.Collections.Generic;

namespace MovieTicketsBookings.Models
{

    public partial class Discount
    {
        public int Id { get; set; }

        public string? Code { get; set; }

        public int? DiscountValue { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();
    }
}