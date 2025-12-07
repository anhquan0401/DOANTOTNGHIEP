using WebApplication1.Data;

namespace WebApplication1.ViewModels
{
    public class VoucherUserVM
    {
        public int VoucherUserId { get; set; }

        public string MaKh { get; set; }

        public int VoucherId { get; set; }

        public DateTime AcquiredDate { get; set; } // Ngày mà người dùng lấy voucher

        public bool IsUsed { get; set; }

        public DateTime? UsedDate { get; set; }

        public Voucher VoucherIdNavigation { get; set; }
    }
}
