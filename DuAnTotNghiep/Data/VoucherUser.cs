namespace WebApplication1.Data
{
    public class VoucherUser
    {
        public int VoucherUserId { get; set; }

        public string MaKh { get; set; }

        public int VoucherId { get; set; }

        public DateTime AcquiredDate { get; set; } // Ngày mà người dùng lấy voucher

        public bool IsUsed { get; set; }

        public DateTime? UsedDate { get; set; }

        public KhachHang MaKhNavigation { get; set; }

        public Voucher VoucherIdNavigation { get; set; }

        public ICollection<InvoiceVoucher> InvoiceVouchers { get; set; } = new List<InvoiceVoucher>();
    }
}
