using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Data;
using WebApplication1.Helpers;
using WebApplication1.ViewModels;

namespace WebApplication1.Controllers
{
    [Authorize(AuthenticationSchemes = "UserCookies", Policy = "RequireUserRole")]
    public class ProfileUserController : Controller
    {
        private readonly EcommerceContext db;
        private readonly IWebHostEnvironment _environment;

        public ProfileUserController(EcommerceContext context, IWebHostEnvironment environment)
        {
            db = context;
            _environment = environment;
        }

        [HttpGet]
        public IActionResult proFile()
        {
            var customerId = HttpContext.User.Claims.SingleOrDefault(p => p.Type == MySetting.CART_CustomerID).Value;
            var proFile = db.KhachHangs.SingleOrDefault(p => p.MaKh == customerId);
            var resutls = new ProFileVM()
            {
                MaKh = proFile.MaKh,
                HoTen = proFile.HoTen,
                GioiTinh = proFile.GioiTinh,
                NgaySinh = proFile.NgaySinh,
                DiaChi = proFile.DiaChi,
                DienThoai = proFile.DienThoai,
                Email = proFile.Email,
                FileHinh = proFile.Hinh,
            };

            return View(resutls);
        }

        [HttpGet]
        public async Task<IActionResult> editFrofile(string id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var proFile = await db.KhachHangs.FindAsync(id);
            var resutls = new ProFileVM()
            {
                MaKh = proFile.MaKh,
                HoTen = proFile.HoTen,
                GioiTinh = proFile.GioiTinh,
                NgaySinh = proFile.NgaySinh,
                DiaChi = proFile.DiaChi,
                DienThoai = proFile.DienThoai,
                Email = proFile.Email,
            };
            ViewData["ImgaeFileName"] = proFile.Hinh;

            return View(resutls);
        }

        [HttpPost]
        public async Task<IActionResult> editFrofile(string id, ProFileVM model)
        {
            if (id == null)
            {
                return NotFound();
            }

            var khachHang = await db.KhachHangs.FindAsync(id);
            if (khachHang == null)
            {
                return NotFound();
            }

            string newFileName = khachHang.Hinh;
            if (model.Hinh != null)
            {
                newFileName = DateTime.Now.ToString("yyyyMMddHHmmssfff") + Path.GetExtension(model.Hinh.FileName);

                string imageFullPath = Path.Combine(_environment.WebRootPath, "Hinh/KhachHang", newFileName);
                using (var stream = new FileStream(imageFullPath, FileMode.Create))
                {
                    await model.Hinh.CopyToAsync(stream);
                }

                // delete the old image
                string oldImageFullPath = Path.Combine(_environment.WebRootPath, "Hinh/KhachHang", khachHang.Hinh);
                if (System.IO.File.Exists(oldImageFullPath))
                {
                    System.IO.File.Delete(oldImageFullPath);
                }
            }

            if (ModelState.IsValid)
            {
                khachHang.MaKh = model.MaKh;
                khachHang.HoTen = model.HoTen;
                khachHang.GioiTinh = model.GioiTinh;
                khachHang.NgaySinh = model.NgaySinh ?? DateTime.MinValue; // Chú ý: Sử dụng giá trị mặc định phù hợp nếu null
                khachHang.DiaChi = model.DiaChi;
                khachHang.DienThoai = model.DienThoai;
                khachHang.Hinh = newFileName;
                khachHang.Email = model.Email;

                db.Update(khachHang);
                await db.SaveChangesAsync();
                return RedirectToAction("proFile", "ProfileUser");
            }
            else
            {
                ViewData["ImgaeFileName"] = khachHang.Hinh;
                return View(model);
            }
        }

        [HttpGet]
        public IActionResult changePassword()
        {
            return View();
        }

        [HttpPost]
        public IActionResult changePassword(ChangePasswordVM model)
        {
            var customerId = HttpContext.User.Claims
                               .SingleOrDefault(p => p.Type == MySetting.CART_CustomerID)?.Value;

            var user = db.KhachHangs.SingleOrDefault(p => p.MaKh == customerId);

            if (ModelState.IsValid)
            {
                var hasher = new PasswordHasher<KhachHang>();

                // 1. Kiểm tra mật khẩu cũ
                var result = hasher.VerifyHashedPassword(user, user.HashMatKhau, model.oldMatKhau);

                if (result == PasswordVerificationResult.Failed)
                {
                    ModelState.AddModelError("Loi", "Mật khẩu cũ không đúng");
                    return View(model);
                }

                // 2. Kiểm tra mật khẩu mới nhập lại
                if (model.newMatKhau != model.renewMatKhau)
                {
                    ModelState.AddModelError("Loi", "Nhập lại mật khẩu mới không trùng khớp");
                    return View(model);
                }

                // 3. Hash mật khẩu mới bằng PBKDF2
                user.HashMatKhau = hasher.HashPassword(user, model.newMatKhau);

                db.Update(user);
                db.SaveChanges();

                return RedirectToAction("Recommend", "Home");
            }

            return View(model);
        }
    }
}
