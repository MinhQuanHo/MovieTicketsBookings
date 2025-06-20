using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MovieTicketsBookings.Models
{

    public partial class News
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Tiêu đề không được để trống")]
        [StringLength(200, ErrorMessage = "Tiêu đề không được vượt quá 200 ký tự")]
        public string? Title { get; set; }

        [Required(ErrorMessage = "Nội dung không được để trống")]
        public string? Content { get; set; }

        [Url(ErrorMessage = "Đường dẫn ảnh không hợp lệ")]
        [StringLength(300, ErrorMessage = "Đường dẫn ảnh không được vượt quá 300 ký tự")]
        public string? Image { get; set; }
        public DateTime? CreateAt { get; set; }
        public DateTime? UpdateAt { get; set; }
        public int? CreateBy { get; set; }

        public virtual User? CreateByNavigation { get; set; }
    }
}