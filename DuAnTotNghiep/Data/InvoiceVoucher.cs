namespace WebApplication1.Data
{
    public class InvoiceVoucher
    {
        public int InvoiceVoucherId { get; set; }

        public int MaHd { get; set; }

        public int? VoucherUserId { get; set; }

        // Thêm thuộc tính để lưu ngày voucher được áp dụng (nếu cần)
        public DateTime AppliedDate { get; set; }

        // Có thể thêm thuộc tính để xác định số tiền giảm giá do voucher áp dụng
        public decimal DiscountAmount { get; set; }

        public HoaDon MaHdNavigation { get; set; }

        public VoucherUser VoucherUserIdNavigation { get; set; }
    }
}
