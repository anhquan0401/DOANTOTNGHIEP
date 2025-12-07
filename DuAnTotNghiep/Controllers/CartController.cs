using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Build.Framework;
using Microsoft.EntityFrameworkCore;
using System.Net.WebSockets;
using WebApplication1.Data;
using WebApplication1.Helpers;
using WebApplication1.ViewModels;

namespace WebApplication1.Controllers
{
    [Authorize(AuthenticationSchemes = "UserCookies", Policy = "RequireUserRole")]
    public class CartController : Controller
    {
        private readonly PaypalClient _paypalClient;
        private readonly EcommerceContext db;

        public CartController(EcommerceContext context, PaypalClient paypalClient)
        {
            _paypalClient = paypalClient;
            db = context;
        }
        public List<CartVM> Cart => HttpContext.Session.Get<List<CartVM>>(MySetting.CART_KEY) ?? new List<CartVM>();
        public IActionResult Index()
        {
            return View(Cart);
        }
        public IActionResult AddToCart(int? id, int quantity = 1)
        {
            var gioHang = Cart;
            var item = gioHang.SingleOrDefault(p => p.MaHH == id);

            if (item == null)
            {
                var hangHoa = db.HangHoas.SingleOrDefault(p => p.MaHh == id);
                if(hangHoa == null)
                {
                    // Cách 2: Sử dụng ViewBag
                    ViewBag.ErrorMessage = "Sản phẩm không tồn tại!";
                    return Redirect("/404");
                }
                // Cách 1: Sử dụng TempData
                TempData["SuccessMessage"] = "Đã thêm vào giỏ hàng!";
                

                item = new CartVM
                {
                    MaHH = hangHoa.MaHh,
                    Img = hangHoa.Hinh ?? string.Empty,
                    TenHH = hangHoa.TenHh,
                    DonGia = hangHoa.DonGia ?? 0,
                    Soluong = quantity
                };
                gioHang.Add(item);

            }
            else
            {
                item.Soluong += quantity;
            }
            HttpContext.Session.Set(MySetting.CART_KEY, gioHang);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult UpdateCart(int id, int quantity)
        {
            // Thực hiện cập nhật giỏ hàng trên server dựa trên id, quantity và price
            var gioHang = Cart;
            var item = gioHang.SingleOrDefault(p => p.MaHH == id);

            if (item != null)
            {
                item.Soluong = quantity;
            }

            HttpContext.Session.Set(MySetting.CART_KEY, gioHang);

            // Trả về phản hồi cho client nếu cần
            return RedirectToAction("Index", "Cart");
        }

        public IActionResult Remove(int id)
        {
            var gioHang = Cart;
            var item = gioHang.SingleOrDefault(p => p.MaHH == id);

            if (item != null)
            {  
                gioHang.Remove(item);
            }
            HttpContext.Session.Set(MySetting.CART_KEY, gioHang);
            return RedirectToAction("Index", "Cart");
        }

        [HttpGet]
        public IActionResult Checkout()
        {
            var carts = Cart;
            var voucherUsers = db.VoucherUsers
            .Include(v => v.VoucherIdNavigation)
            .Select(v => new VoucherUserVM
            {
                VoucherUserId = v.VoucherUserId,
                MaKh = v.MaKh,
                VoucherId = v.VoucherId,
                AcquiredDate = v.AcquiredDate,
                IsUsed = v.IsUsed,
                UsedDate = v.UsedDate,
                VoucherIdNavigation = v.VoucherIdNavigation // Gán luôn
            })
            .ToList();


            if (Cart.Count == 0)
            {
                return Redirect("/");
            }
            var model = new CheckoutPageVM
            {
                Carts = carts,
                VoucherUsers = voucherUsers
            };
            ViewBag.PaypalClientId = _paypalClient.ClientId;
            return View(model);
        }

        [HttpPost]
        public IActionResult ApplyVoucher([FromBody] CheckoutPageVM model)
        {
            double total = Cart.Sum(t => t.ThanhTien);

            if (model.SelectedVoucherUsers != null && model.SelectedVoucherUsers.Length > 0)
            {
                var vouchers = db.VoucherUsers
                    .Include(vu => vu.VoucherIdNavigation)
                    .Where(vu => model.SelectedVoucherUsers.Contains(vu.VoucherUserId))
                    .ToList();

                foreach (var voucher in vouchers)
                {
                    if (!voucher.IsUsed && total >= (double)(voucher.VoucherIdNavigation.OrderMinimum ?? 0))
                    {
                        double discountRate = (double)voucher.VoucherIdNavigation.DiscountVoucher / 100.0;
                        double discountAmount = total * discountRate;
                        total -= discountAmount;

                    }
                }
            }

            return Json(new { finalTotal = total });
        }

        [HttpPost]
        public IActionResult Checkout(CheckoutPageVM model)
        {
            if (!ModelState.IsValid)
            {
                // Trả lại view cùng với model nếu dữ liệu không hợp lệ
                return View(model);
            }

            var customerId = HttpContext.User.Claims
                .SingleOrDefault(p => p.Type == MySetting.CART_CustomerID)?.Value;

            if (string.IsNullOrEmpty(customerId))
            {
                ModelState.AddModelError("", "Không tìm thấy thông tin khách hàng.");
                return View(model);
            }

            var khachHang = db.KhachHangs.SingleOrDefault(kh => kh.MaKh == customerId);

            var hoadon = new HoaDon
            {
                MaKh = customerId,
                HoTen = model.CheckoutVM.HoTen ?? khachHang?.HoTen,
                DiaChi = model.CheckoutVM.DiaChi ?? khachHang?.DiaChi,
                DienThoai = model.CheckoutVM.DienThoai ?? khachHang?.DienThoai,
                NgayDat = DateTime.Now,
                NgayGiao = DateTime.Now.AddDays(3),
                CachThanhToan = "COD",
                CachVanChuyen = "GRAB",
                MaTrangThai = 0,
                NgayCapNhat = DateTime.Now,
                GhiChu = model.CheckoutVM.GhiChu,
            };

            using (var transaction = db.Database.BeginTransaction())
            {
                try
                {
                    // Lưu hóa đơn
                    db.Add(hoadon);
                    db.SaveChanges();

                    // Thêm voucher (nếu có)
                    if (model.SelectedVoucherUsers != null && model.SelectedVoucherUsers.Length > 0)
                    {

                        var invoiceVoucherList = new List<InvoiceVoucher>();
                        foreach (var voucherUserId in model.SelectedVoucherUsers)
                        {
                            var voucherUser = db.VoucherUsers
                                .Include(v => v.VoucherIdNavigation)
                                .SingleOrDefault(v => v.VoucherUserId == voucherUserId);

                            if (voucherUser != null && !voucherUser.IsUsed)
                            {
                                invoiceVoucherList.Add(new InvoiceVoucher
                                {
                                    MaHd = hoadon.MaHd,
                                    VoucherUserId = voucherUser.VoucherUserId,
                                    AppliedDate = DateTime.Now,
                                    DiscountAmount = voucherUser.VoucherIdNavigation.DiscountVoucher
                                });

                                // Đánh dấu voucher đã sử dụng
                                voucherUser.IsUsed = true;
                                voucherUser.UsedDate = DateTime.Now;
                            }
                        }

                        if (invoiceVoucherList.Any())
                        {
                            db.AddRange(invoiceVoucherList);
                        }
                    }

                    // Thêm chi tiết hóa đơn
                    var cart = HttpContext.Session.Get<List<CartVM>>(MySetting.CART_KEY) ?? new List<CartVM>();
                    var cthd = cart.Select(item => new ChiTietHd
                    {
                        MaHd = hoadon.MaHd,
                        SoLuong = item.Soluong,
                        DonGia = item.DonGia,
                        MaHh = item.MaHH,
                        GiamGia = 0
                    }).ToList();

                    if (cthd.Any())
                    {
                        db.AddRange(cthd);
                    }

                    db.SaveChanges();
                    transaction.Commit();

                    // Clear giỏ hàng sau khi thanh toán thành công
                    HttpContext.Session.Set(MySetting.CART_KEY, new List<CartVM>());

                    return View("CheckoutSuccess");
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    ModelState.AddModelError("", "Có lỗi xảy ra khi thanh toán: " + ex.Message);
                    return View(model);
                }
            }
        }

        public IActionResult PaymentSuccess()
        {
            return View("CheckoutSuccess");
        }

        #region Paypal payment
        [HttpPost("/Cart/create-paypal-order")]
        public async Task<IActionResult> CreatePaypalOrder(CancellationToken cancellationToken)
        {
            // Thông tin đơn hàng gửi qua Paypal
            var tongTien = Cart.Sum(p => p.ThanhTien).ToString();
            var donViTienTe = "USD";
            var maDonHangThamChieu = "DH" + DateTime.Now.Ticks.ToString();

            try
            {
                var response = await _paypalClient.CreateOrder(tongTien, donViTienTe, maDonHangThamChieu);

                // Lưu lại mã để sau này truy vấn đơn hàng
                HttpContext.Session.SetString("PaypalRef", maDonHangThamChieu);

                return Ok(response);
            }
            catch (Exception ex)
            {
                var error = new { ex.GetBaseException().Message };
                return BadRequest(error);
            }
        }

        [HttpPost("/Cart/capture-paypal-order")]
        public async Task<IActionResult> CapturePaypalOrder(string orderID, CancellationToken cancellationToken)
        {
            try
            {
                var response = await _paypalClient.CaptureOrder(orderID);

                // 🔥 LẤY MÃ THAM CHIẾU ĐƠN HÀNG
                string referenceId = HttpContext.Session.GetString("PaypalRef");

                if (!string.IsNullOrEmpty(referenceId))
                {
                    // 🔥 TÌM ĐƠN HÀNG TRONG DATABASE
                    var donHang = db.HoaDons.FirstOrDefault(x => x.MaThamChieu == referenceId);

                    if (donHang != null)
                    {
                        // ⭐ Cập nhật trạng thái thanh toán
                        donHang.CachThanhToan = "Chuyển Khoản";   // PayPal → xem như chuyển khoản
                        donHang.MaTrangThai = 4;                 // 4 = Đã thanh toán
                        donHang.NgayCapNhat = DateTime.Now;

                        db.HoaDons.Update(donHang);
                        await db.SaveChangesAsync();

                        // ⭐⭐⭐ XOÁ GIỎ HÀNG (Sau khi thanh toán thành công)

                        // Nếu bạn đang dùng Session để chứa giỏ hàng:
                        HttpContext.Session.Remove("Cart");

                        // Nếu bạn dùng danh sách tĩnh Cart:
                        // Cart.Clear();
                    }
                }

                return Ok(response);
            }
            catch (Exception ex)
            {
                Console.WriteLine("PAYPAL ERROR: " + ex.ToString());  // ⬅ LOG FULL LỖI
                return BadRequest(new
                {
                    message = "PAYPAL ERROR",
                    detail = ex.ToString()     // gửi chi tiết lỗi ra FE hoặc Postman
                });
            }
        }
        #endregion


    }
}
