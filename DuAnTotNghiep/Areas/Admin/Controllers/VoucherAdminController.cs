using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Areas.Admin.Models;
using WebApplication1.Data;

namespace WebApplication1.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(AuthenticationSchemes = "AdminCookies", Policy = "RequireStaffAccess")]
    public class VoucherAdminController : Controller
    {
        private readonly EcommerceContext _context;

        public VoucherAdminController(EcommerceContext context)
        {
            _context = context;
        }

        // GET: Admin/VoucherAdmin
        public IActionResult Index()
        {
            var vouchers = _context.Vouchers.ToList();
            return View(vouchers);
        }

        // GET: Admin/VoucherAdmin/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/VoucherAdmin/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(VoucherAdminVM voucherVM)
        {
            if (ModelState.IsValid)
            {
                var voucher = new Voucher
                {
                    VoucherName = voucherVM.VoucherName,
                    VoucherCode = voucherVM.VoucherCode,
                    DiscountVoucher = voucherVM.DiscountVoucher,
                    IsActive = voucherVM.IsActive,
                    CreatedDate = DateTime.Now,
                    UpdatedDate = DateTime.Now,
                    Description = voucherVM.Description,
                    ExpirationDate = voucherVM.ExpirationDate,
                    UserLimit = voucherVM.UserLimit,
                    OrderMinimum = voucherVM.OrderMinimum
                };
                _context.Vouchers.Add(voucher);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(voucherVM);
        }

    }
}
