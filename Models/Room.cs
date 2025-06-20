using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MovieTicketsBookings.Models
{

    public partial class Room
    {
        public Room()
        {
            Rows = new HashSet<Row>();
            Showtimes = new HashSet<Showtime>();
        }
        public int Id { get; set; }
        [Required(ErrorMessage = "Tên phòng không được để trống")]
        [MinLength(4, ErrorMessage = "Tên phải có tối thiểu 4 ký tự")]
        public string? Name { get; set; }

        public int? CinemaId { get; set; }

        public int? NumberOfRows { get; set; }

        public virtual Cinema? Cinema { get; set; }

        public virtual ICollection<Row> Rows { get; set; } = new List<Row>();

        public virtual ICollection<Showtime> Showtimes { get; set; } = new List<Showtime>();
    }
}