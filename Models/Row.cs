using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MovieTicketsBookings.Models
{

    public partial class Row
    {
        public Row()
        {
            Seats = new HashSet<Seat>();
        }
        public int Id { get; set; }

        [Required(ErrorMessage = "Tên hàng không được để trống")]
        [MinLength(1, ErrorMessage = "Tên phải có tối thiểu 1 ký tự")]
        public string? RowName { get; set; }

        public int? RoomId { get; set; }

        public int? NumberOfColumns { get; set; }

        public string? Type { get; set; }

        public virtual Room? Room { get; set; }

        public virtual ICollection<Seat> Seats { get; set; } = new List<Seat>();
    }
}