using AutoMapper;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Net.Mail;
using System.Security.Claims;
using WebApplication1.Data;
using WebApplication1.Helpers;
using WebApplication1.Repository.Interface;
using WebApplication1.Repository.Service;
using WebApplication1.ViewModels;

namespace WebApplication1.Controllers
{
    public class KhachHangController : Controller
    {
        private readonly EcommerceContext db;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _environment;
        private readonly IMyEmailSender _myEmailSender;

        public KhachHangController(EcommerceContext context, IMapper mapper, IWebHostEnvironment environment, IMyEmailSender myEmailSender)
        {
            db = context;
            _mapper = mapper;
            _environment = environment;
            _myEmailSender = myEmailSender;
        }

        #region Register
        [HttpGet]
        public IActionResult DangKy()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> DangKy(RegisterVM model, IFormFile Hinh)
        {
            if (ModelState.IsValid)
            {
                var checkMakh = db.KhachHangs.SingleOrDefault(c => c.Email == model.Email);
                if (checkMakh == null)
                {
                    TempData["SuccessMessage"] = "Đăng kí thành công!";
                    var khachHang = _mapper.Map<KhachHang>(model);

                    // Không cần RandomKey nữa
                    var hasher = new PasswordHasher<KhachHang>();
                    khachHang.MaKh = MyUtil.GenerateRandomKey();
                    khachHang.HashMatKhau = hasher.HashPassword(khachHang, model.MatKhau);
                    khachHang.HieuLuc = true;
                    khachHang.VaiTro = 0;

                    if (Hinh != null)
                    {
                        khachHang.Hinh = MyUtil.UpLoadHinh(Hinh, "KhachHang");
                    }

                    string path = Path.Combine(_environment.WebRootPath, "Template/Welcome.cshtml");
                    string htmtString = System.IO.File.ReadAllText(path);
                    htmtString = htmtString.Replace("{{Username}}", model.Email);
                    htmtString = htmtString.Replace("{{url}}", "https://localhost:7225/KhachHang/DangNhap");

                    bool status = await _myEmailSender.EmailSendAsync(model.Email, "Đăng kí người dùng mới", htmtString);

                    db.Add(khachHang);
                    await db.SaveChangesAsync();

                    return RedirectToAction("DangNhap", "KhachHang");
                }
                else
                {
                    TempData["ErrorMessage"] = "Email này đã được đăng kí";
                }
            }

            return View();
        }
        #endregion Register

        #region Login
        [HttpGet]
        public IActionResult DangNhap(string? ReturnURL)
        {
            ViewBag.ReturnURL = ReturnURL;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> DangNhap(LoginVM model, string? ReturnURL)
        {
            ViewBag.ReturnURL = ReturnURL;

            if (!ModelState.IsValid)
                return View();

            var khachHang = db.KhachHangs.SingleOrDefault(kh => kh.Email == model.Email);
            if (khachHang == null)
            {
                TempData["ErrorMessage"] = "Email này không tồn tại. Vui lòng đăng kí!";
                return View();
            }

            if (!khachHang.HieuLuc)
            {
                TempData["ErrorMessage"] = "Tài khoản này đã bị khóa. Vui lòng đăng kí lại!";
                return View();
            }

            // ⭐ Kiểm tra mật khẩu đúng cách
            var hasher = new PasswordHasher<KhachHang>();
            var result = hasher.VerifyHashedPassword(khachHang, khachHang.HashMatKhau, model.MatKhau);

            if (result == PasswordVerificationResult.Failed)
            {
                TempData["ErrorMessage"] = "Sai mật khẩu";
                return View();
            }

            // ⭐ Đăng nhập thành công
            TempData["SuccessMessage"] = "Đăng nhập thành công!";

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, khachHang.Email),
                new Claim(ClaimTypes.Name, khachHang.HoTen),
                new Claim(MySetting.CART_CustomerID, khachHang.MaKh.ToString()),
                new Claim(ClaimTypes.Role, "Customer"),
            };

            var claimsIdentity = new ClaimsIdentity(claims, "UserCookies");
            var claimPrincipal = new ClaimsPrincipal(claimsIdentity);

            await HttpContext.SignInAsync("UserCookies", claimPrincipal,
                new AuthenticationProperties { IsPersistent = true });

            ViewBag.RedirectUrl = Url.IsLocalUrl(ReturnURL) ? ReturnURL : "/";

            return View();
        }

        #endregion Login


        public async Task<IActionResult> DangXuat()
        {
            await HttpContext.SignOutAsync("UserCookies");
            return RedirectToAction("DangNhap", "KhachHang");
        }

        #region forgotPasswords
        [HttpGet]
        public IActionResult forgotPassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> forgotPassword(forgotPasswordVM model)
        {
            if(!ModelState.IsValid)
            {
                return View(model);
            }
            
            var user = db.KhachHangs.SingleOrDefault(kh => kh.Email == model.Email);
            if(user == null)
            {
                ModelState.AddModelError("Email", "Email này không tồn tại.");
            }

            var resetCode = MyUtil.GenerateRandomKey();
            user.ResetCode = resetCode;
            await db.SaveChangesAsync();

            var callBackUrl = Url.Action("ConfirmCode", "KhachHang", null, protocol: HttpContext.Request.Scheme);
            await _myEmailSender.EmailSendAsync(model.Email, "Quên mật khẩu", $"Mã xác nhận của bạn là: {resetCode}. Vui lòng xác nhận mã này tại <a href='{callBackUrl}'>đây</a>.");

            ViewBag.Message = "Mã xác nhận đã được gửi tới email của bạn!";
            return RedirectToAction("ConfirmCode", "Khachhang");
        }
        #endregion

        #region Confirm code
        [HttpGet]
        public IActionResult ConfirmCode()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ConfirmCode(ConfirmCodeVM model)
        {
            if (ModelState.IsValid)
            {
                var user = db.KhachHangs.SingleOrDefault(kh => kh.ResetCode == model.Code);
                if (user == null)
                {
                    ModelState.AddModelError("Code", "Mã xác nhận không chính xác");
                    return View(model); // PHẢI RETURN ngay
                }

                // Hash mật khẩu theo chuẩn ASP.NET Core
                var hasher = new PasswordHasher<KhachHang>();
                user.HashMatKhau = hasher.HashPassword(user, model.NewPassWord);

                // Xóa mã reset để tránh reuse
                user.ResetCode = null;
                await db.SaveChangesAsync();

                TempData["SuccessMessage"] = "Mật khẩu của bạn đã được thay đổi thành công.";
                return RedirectToAction("DangNhap", "KhachHang");
            }

            return View(model);
        }
        #endregion
    }
}
