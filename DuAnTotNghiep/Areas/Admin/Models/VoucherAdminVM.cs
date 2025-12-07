namespace WebApplication1.Areas.Admin.Models
{
    public class VoucherAdminVM
    {
        public int VoucherId { get; set; }

        public string VoucherName { get; set; }

        public string VoucherCode { get; set; }

        public int DiscountVoucher { get; set; }

        public bool IsActive { get; set; } // Trạng thái của voucher (còn hiệu lực hay không)

        public DateTime CreatedDate { get; set; } // Ngày tạo voucher

        public DateTime? UpdatedDate { get; set; } // Ngày cập nhật voucher (nullable)

        public string Description { get; set; } // Mô tả về voucher (nếu cần)

        public DateTime ExpirationDate { get; set; } // Ngày hết hạn của voucher

        public int? UserLimit { get; set; } // Giới hạn số lần sử dụng trên mỗi người dùng (nullable)

        public decimal? OrderMinimum { get; set; } // Giá trị đơn hàng tối thiểu để áp dụng voucher
    }
}
