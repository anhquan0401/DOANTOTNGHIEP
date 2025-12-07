using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Areas.Admin.Models
{
    public class HangHoaAdminVM
    {
        public int MaHH { get; set; }

        [Required, MaxLength(100)]
        public string TenHH { get; set; }

        [Required]
        public int SoLuong { get; set; }

        [Required]
        public string TenAlias { get; set; }

        [Required]
        public int MaLoai { get; set; }

        public string? MoTaDonVi { get; set; }

        [Required]
        public double DonGia { get; set; }

        public string? HinhFile {  get; set; } 
        public IFormFile? Hinh { get; set; }
        public DateTime? NgaySx { get; set; }

        [Required]
        public float GiamGia { get; set; }
        [Required]
        public int SoLanXem { get; set; }

        [Required]
        public string MoTa {  get; set; }
        [Required]
        public string MaNcc { get; set; }

    }
}
