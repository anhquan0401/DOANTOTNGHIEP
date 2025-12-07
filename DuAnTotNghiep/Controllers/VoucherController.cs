using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WebApplication1.Data;
using WebApplication1.Helpers;

namespace WebApplication1.Controllers
{
    [Authorize(AuthenticationSchemes = "UserCookies", Policy = "RequireUserRole")]
    public class VoucherController : Controller
    {
        private readonly EcommerceContext _context;

        public VoucherController(EcommerceContext context)
        {
            _context = context;
        }
        [HttpGet]
        //public IActionResult Index()
        //{
        //    var vouchers = _context.Vouchers.ToList();
        //    return View(vouchers);
        //}

        //[HttpPost]
        //public async Task<IActionResult> GetVoucher(int VoucherId)
        //{
        //    var customerId = HttpContext.User.Claims.SingleOrDefault(p => p.Type == MySetting.CART_CustomerID).Value;
        //    var voucherUser = new VoucherUser
        //    {
        //        MaKh = customerId,
        //        VoucherId = VoucherId,
        //        AcquiredDate = DateTime.Now,
        //        IsUsed = false,
        //        UsedDate = null
        //    };
        //    _context.VoucherUsers.Add(voucherUser);
        //    await _context.SaveChangesAsync();
        //    return RedirectToAction("Index", "Voucher");
        //}

        public IActionResult Index()
        {
            // 1. Lấy danh sách tất cả voucher hiển thị
            var vouchers = _context.Vouchers.ToList();

            // 2. Lấy ID người dùng hiện tại (Giả sử bạn dùng Identity)
            // Nếu chưa đăng nhập thì danh sách đã lấy là rỗng
            var userId = HttpContext.User.Claims
                .SingleOrDefault(p => p.Type == MySetting.CART_CustomerID)?.Value;
            var collectedVoucherIds = new List<int>();

            if (userId != null)
            {
                // Truy vấn bảng lưu trữ lịch sử lấy voucher (Ví dụ: UserVouchers)
                collectedVoucherIds = _context.VoucherUsers
                                        .Where(uv => uv.MaKh == userId)
                                        .Select(uv => uv.VoucherId)
                                        .ToList();
            }

            // 3. Truyền danh sách ID đã lấy sang View
            ViewBag.CollectedVoucherIds = collectedVoucherIds;

            return View(vouchers);
        }

        [HttpPost]
        public IActionResult GetVoucher(int voucherId)
        {
            // 1. Lấy User ID
            var userId = HttpContext.User.Claims
                .SingleOrDefault(p => p.Type == MySetting.CART_CustomerID)?.Value;

            // 2. Lưu vào database (Bảng UserVouchers)
            // Kiểm tra xem đã lấy chưa để tránh trùng lặp
            var exists = _context.VoucherUsers.Any(x => x.MaKh == userId && x.VoucherId == voucherId);
            if (!exists)
            {
                var userVoucher = new VoucherUser
                {
                    MaKh = userId,
                    VoucherId = voucherId,
                    AcquiredDate = DateTime.Now,
                    IsUsed = false,
                    UsedDate = null
                };
                _context.VoucherUsers.Add(userVoucher);
                _context.SaveChanges();
            }

            // 3. Quan trọng: Redirect lại trang Index để load lại giao diện
            return RedirectToAction("Index");
        }
    }
}
