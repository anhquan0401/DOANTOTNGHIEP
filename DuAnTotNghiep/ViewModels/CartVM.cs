using WebApplication1.Data;

namespace WebApplication1.ViewModels
{
    public class CartVM
    {
        public int MaHH { get; set; }
        public string? Img { get; set; }
        public string? TenHH { get; set; }
        public double DonGia { get; set; }
        public int Soluong { get; set; }
        public double ThanhTien => Soluong * DonGia;
    }
}
