using System.ComponentModel.DataAnnotations;
using WebApplication1.ViewModels;

namespace WebApplication1.Areas.Admin.Models
{
    public class HoaDonAdminVM
    {
        public int MaHd { get; set; }

        public string MaKh { get; set; } = null!;

        [Required]
        public DateTime NgayDat { get; set; }

        public DateTime? NgayGiao { get; set; }

        [Required, MaxLength(100)]
        public string? HoTen { get; set; }

        [Required, MaxLength(100)]
        public string DiaChi { get; set; } = null!;

        [Required]
        public string? DienThoai { get; set; }

        [Required]
        public string CachThanhToan { get; set; } = null!;

        [Required]
        public string CachVanChuyen { get; set; } = null!;

        [Required]
        public double PhiVanChuyen { get; set; }

        [Required]
        public int MaTrangThai { get; set; }

        [Required]
        public DateTime NgayCapNhat { get; set; }

        public string? GhiChu { get; set; }

        public int TongOrder { get; set; }

        public double Tong {  get; set; }

        public List<ChiTietHdVM> ChiTietHds { get; set; }
    }
}
