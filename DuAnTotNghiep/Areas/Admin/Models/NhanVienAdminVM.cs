namespace WebApplication1.Areas.Admin.Models
{
    public class NhanVienAdminVM
    {
        public string MaNv { get; set; } = null!;

        public string HoTen { get; set; } = null!;

        public string Email { get; set; } = null!;

        public string? MatKhau { get; set; }

        public int MaPq { get; set; }

        public bool XacNhan { get; set; }
    }
}
