using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MovieTicketsBookings.Models
{
    public partial class User
    {
        public User()
        {
            Bookings = new HashSet<Booking>();
            Feedbacks = new HashSet<Feedback>();
            News = new HashSet<News>();
        }

        public int Id { get; set; }
        public int? RoleId { get; set; }

        [Required(ErrorMessage = "Tên đăng nhập không được để trống")]
        [StringLength(50, ErrorMessage = "Tên đăng nhập không được vượt quá 50 ký tự")]
        [MinLength(4, ErrorMessage = "Tên đăng nhập phải có tối thiểu 6 ký tự")]
        public string Username { get; set; } = null!;

        [Required(ErrorMessage = "Mật khẩu không được để trống")]
        [StringLength(100, ErrorMessage = "Mật khẩu phải có ít nhất 6 ký tự", MinimumLength = 6)]
        public string Password { get; set; } = null!;

        [Required(ErrorMessage = "Email không được để trống")]
        [EmailAddress(ErrorMessage = "Email không hợp lệ")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Số điện thoại không được để trống")]
        [Phone(ErrorMessage = "Số điện thoại không hợp lệ")]
        public string? PhoneNumber { get; set; }

    public DateTime? Dob { get; set; }

        [Required(ErrorMessage = "Họ và tên không được để trống")]
        [StringLength(100, ErrorMessage = "Họ và tên không được vượt quá 100 ký tự")]
        [MinLength(4, ErrorMessage = "Tên đăng nhập phải có tối thiểu 4 ký tự")]
        public string? FullName { get; set; }
        public string? Status { get; set; }

        public virtual Role? Role { get; set; }
        public virtual ICollection<Booking> Bookings { get; set; }
        public virtual ICollection<Feedback> Feedbacks { get; set; }
        public virtual ICollection<News> News { get; set; }
    }
}