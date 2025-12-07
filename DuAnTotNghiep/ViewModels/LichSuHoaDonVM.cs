using WebApplication1.Data;

namespace WebApplication1.ViewModels
{
    public class LichSuHoaDonVM
    {
        public int MaHd { get; set; }
        public string? HoTen { get; set; }
        public string? DienThoai { get; set; }
        public DateTime NgayDat { get; set; }
        public DateTime? NgayGiao { get; set; }
        public double TongTien { get; set; }
        public string CachThanhToan { get; set; } = null!;
        public string CachVanChuyen { get; set; } = null!;
        public double PhiVanChuyen { get; set; }
        public string DiaChi { get; set; }
        public string TrangThai { get; set; }

        public decimal DiscountTotal { get; set; }

        public DateTime NgayCapNhat { get; set; }

        public List<ChiTietHdVM> ChiTietHds { get; set; }

        public virtual ICollection<InvoiceVoucher> InvoiceVouchers { get; set; } = new List<InvoiceVoucher>();
    }
}
