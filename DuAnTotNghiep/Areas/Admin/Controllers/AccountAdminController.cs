using AutoMapper;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WebApplication1.Areas.Admin.Models;
using WebApplication1.Data;
using WebApplication1.Helpers;
using WebApplication1.ViewModels;

namespace WebApplication1.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AccountAdminController : Controller
    {
        private readonly EcommerceContext db;
        
        public AccountAdminController(EcommerceContext context)
        {
            db = context;
 
        }
        #region register
        [HttpGet]
        public IActionResult RegisterAdmin()
        {
            return View();
        }

        [HttpPost]
        public IActionResult RegisterAdmin(RegisterAdminVM model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var check = db.NhanViens.SingleOrDefault(a => a.Email == model.Email);
            if (check != null)
            {
                ModelState.AddModelError("Loi", "Email đã được đăng ký");
                return View(model);
            }

            var nhanVien = new NhanVien()
            {
                MaNv = Guid.NewGuid().ToString(), // Convert Guid to string
                HoTen = model.HoTen,
                Email = model.Email,
                MaPq = 2,   // mặc định Nhân viên
                XacNhan = false,
            };

            // Băm PBKDF2
            var hasher = new PasswordHasher<NhanVien>();
            nhanVien.HashMatKhau = hasher.HashPassword(nhanVien, model.MatKhau);

            db.NhanViens.Add(nhanVien);
            db.SaveChanges();

            return RedirectToAction("LoginAdmin", "AccountAdmin");
        }
        #endregion

        #region Login
        [HttpGet]
        public IActionResult LoginAdmin(string? ReturnURL)
        {
            ViewBag.ReturnURL = ReturnURL;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> LoginAdmin(LoginAdminVM model, string? ReturnURL)
        {
            ViewBag.ReturnURL = ReturnURL;

            if (!ModelState.IsValid)
                return View(model);

            var admin = db.NhanViens.SingleOrDefault(a => a.Email == model.Email);
            if (admin == null)
            {
                ModelState.AddModelError("Loi", "Tài khoản này không tồn tại.");
                return View(model);
            }

            // Kiểm tra mật khẩu bằng PBKDF2
            var hasher = new PasswordHasher<NhanVien>();
            var result = hasher.VerifyHashedPassword(admin, admin.HashMatKhau, model.MatKhau);

            if (result == PasswordVerificationResult.Failed)
            {
                ModelState.AddModelError("Loi", "Sai thông tin đăng nhập");
                return View(model);
            }

            var waitConfirm = db.NhanViens.SingleOrDefault(a => a.Email == model.Email && a.XacNhan == true);
            if(waitConfirm == null)
            {
                return View("WaitConfirm");
            }

            // Xác định role dựa vào MaPq
            string role = admin.MaPq == 1 ? "QuanTriVien" : "NhanVien";

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, admin.Email),
                new Claim(ClaimTypes.Name, admin.HoTen),
                new Claim(MySetting.AdminID, admin.MaNv.ToString()),
                new Claim(ClaimTypes.Role, role)
            };

            var claimsIdentity = new ClaimsIdentity(claims, "AdminCookies");
            var claimPrincipal = new ClaimsPrincipal(claimsIdentity);

            var authProperties = new AuthenticationProperties
            {
                IsPersistent = true
            };

            await HttpContext.SignInAsync("AdminCookies", claimPrincipal, authProperties);

            return Url.IsLocalUrl(ReturnURL)
                ? Redirect(ReturnURL)
                : RedirectToAction("Index", "HomeAdmin");
        }
        #endregion


        public async Task<IActionResult> LogoutAdmin()
        {
            await HttpContext.SignOutAsync("AdminCookies");
            return RedirectToAction("LoginAdmin", "AccountAdmin");
        }
    }
}
