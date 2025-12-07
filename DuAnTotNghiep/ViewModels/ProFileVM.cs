using System.ComponentModel.DataAnnotations;

namespace WebApplication1.ViewModels
{
    public class ProFileVM
    {
        // MaKH, MatKhau, HoTen, GioiTinh, NgaySinh?, DiaChi, DienThoai, Email, Hinh?
        [Key]
        [Display(Name = "Tên đăng nhập")]
        [Required(ErrorMessage = "Bạn chưa nhập tên đăng nhập")]
        [MaxLength(20, ErrorMessage = "Tối đa 20 kí tự")]
        public string MaKh { get; set; }

        [Display(Name = "Họ và Tên")]
        [Required(ErrorMessage = "Bạn chưa nhập tên của bạn")]
        [MaxLength(50, ErrorMessage = "Tối đa 50 kí tự")]
        public string HoTen { get; set; }

        [Display(Name = "Giới Tính")]
        public bool GioiTinh { get; set; } = true;

        [Display(Name = "Ngày Sinh")]
        public DateTime? NgaySinh { get; set; }

        [Display(Name = "Địa Chỉ")]
        [MaxLength(60, ErrorMessage = "Tối đa 60 kí tự")]
        [Required(ErrorMessage = "Bạn chưa nhập địa chỉ")]
        public string DiaChi { get; set; }

        [Display(Name = "Số Điện Thoại")]
        [MaxLength(24, ErrorMessage = "Tối đa 24 kí tự")]
        [RegularExpression(@"0[9875]\d{8}", ErrorMessage = "Chưa đúng định dạng số điện thoại")]
        [Required(ErrorMessage = "Bạn chưa nhập số điện thoại")]
        public string DienThoai { get; set; }

        [Display(Name = "Email")]
        [EmailAddress(ErrorMessage = "Chưa đúng định dạng Email")]
        [Required(ErrorMessage = "Bạn chưa nhập email")]
        public string Email { get; set; }

        [Display(Name = "Ảnh đại diện")]
        public string? FileHinh { get; set; }
        public IFormFile? Hinh { get; set; }
    }
}
