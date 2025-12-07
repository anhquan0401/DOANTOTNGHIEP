using WebApplication1.Data;

namespace WebApplication1.ViewModels
{
    public class ChiTietHangHoaVM
    {
        public int MaHH { get; set; }
        public string? TenHH { get; set; }
        public int Soluong { get; set; }
        public string? TenLoai { get; set; }
        public string? Img { get; set; }
        public double DonGia { get; set; }
        public string? MoTa { get; set; }
        public string? ChiTiet { get; set; }
        public double Rating { get; set; }
        public virtual ICollection<Comments> Commentss { get; set; } = new List<Comments>();
    }
}

/* MaHH TenHH TenLoai DonGia Sao MoTa Hinh*/
