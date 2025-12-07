using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using WebApplication1.Areas.Admin.Models;
using WebApplication1.Data;
using WebApplication1.Helpers;
using WebApplication1.ViewModels;

namespace WebApplication1.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(AuthenticationSchemes = "AdminCookies", Policy = "RequireAdminRole")]
    public class KhachHangAdminController : Controller
    {
        private readonly EcommerceContext _context;
        private readonly IWebHostEnvironment _environment;

        public KhachHangAdminController(EcommerceContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        // GET: Admin/KhachHangAdmin
        public async Task<IActionResult> Index()
        {
            var khachHangs = _context.KhachHangs.AsQueryable();

            var results = khachHangs.Select(kh => new KhachHangAdminVM 
            {
                MaKh = kh.MaKh,
                HoTen = kh.HoTen,
                GioiTinh = kh.GioiTinh,
                DienThoai = kh.DienThoai,
                Email = kh.Email,
                FileHinh = kh.Hinh,
            }).ToList();
            return View(results);
        }

        // GET: Admin/KhachHangAdmin/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var khachHang = await _context.KhachHangs
                .FirstOrDefaultAsync(m => m.MaKh == id);
            if (khachHang == null)
            {
                return NotFound();
            }

            return View(khachHang);
        }

        // GET: Admin/KhachHangAdmin/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/KhachHangAdmin/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(KhachHangAdminVM model)
        {
            if(model.Hinh == null)
            {
                ModelState.AddModelError("Hinh", "Bat Buoc");
            }
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            string newFileName = DateTime.Now.ToString("yyyyMMddHHmmssfff");
            newFileName += Path.GetExtension(model.Hinh!.FileName);

            string imageFullPath = _environment.WebRootPath + "/Hinh/KhachHang/" + newFileName;
            using (var stream = System.IO.File.Create(imageFullPath))
            {
                model.Hinh.CopyTo(stream);
            }
           
            var checkMakh = _context.KhachHangs.SingleOrDefault(c => c.MaKh == model.MaKh);
            if (checkMakh == null)
            {
                string randomKey = MyUtil.GenerateRandomKey();
                KhachHang khachHangs = new KhachHang()
                {
                    MaKh = model.MaKh,
                    HoTen = model.HoTen,
                    HashMatKhau = model.MatKhau.ToMd5Hash(randomKey),
                    GioiTinh = model.GioiTinh,
                    NgaySinh = model.NgaySinh ?? DateTime.Now,
                    DiaChi = model.DiaChi,
                    DienThoai = model.DienThoai,
                    Email = model.Email,
                    Hinh = newFileName,
                    RandomKey = randomKey,
                    HieuLuc = model.HieuLuc,
                    VaiTro = model.VaiTro
            };

                _context.Add(khachHangs);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            else
            {
                ModelState.AddModelError("Loi", "Tên đăng nhập này đã được đăng kí");
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: Admin/KhachHangAdmin/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if(id == null)
            {
                return NotFound();
            }

            var khachHangs = await _context.KhachHangs.FindAsync(id);
            var khachHangAdminVM = new KhachHangAdminVM()
            {
                MaKh = khachHangs.MaKh,
                HoTen = khachHangs.HoTen,
                GioiTinh = khachHangs.GioiTinh,
                NgaySinh = khachHangs.NgaySinh,
                DiaChi = khachHangs.DiaChi,
                DienThoai = khachHangs.DienThoai,
                Email = khachHangs.Email,
                HieuLuc = khachHangs.HieuLuc,
                VaiTro = khachHangs.VaiTro,
            };
            ViewData["ImgaeFileName"] = khachHangs.Hinh;

            return View(khachHangAdminVM);
        }

        // POST: Admin/KhachHangAdmin/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, KhachHangAdminVM model)
        {
            if (id == null)
            {
                return NotFound();
            }

            var khachHang = await _context.KhachHangs.FindAsync(id);
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
                khachHang.Email = model.Email;
                khachHang.Hinh = newFileName;
                khachHang.HieuLuc = model.HieuLuc;
                khachHang.VaiTro = model.VaiTro;

                _context.Update(khachHang);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            else
            {
                ViewData["ImageFileName"] = khachHang.Hinh;
                return View(model);
            }

        }

        // GET: Admin/KhachHangAdmin/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var khachHang = await _context.KhachHangs
                .FirstOrDefaultAsync(m => m.MaKh == id);
            if (khachHang == null)
            {
                return NotFound();
            }

            return View(khachHang);
        }

        // POST: Admin/KhachHangAdmin/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var khachHang = await _context.KhachHangs.FindAsync(id);
            var comment = await _context.Commentss.Where(c => c.MaKh == id).ToListAsync();
            var hoadon = await _context.HoaDons.Where(hd => hd.MaKh == id).ToListAsync();
            if (khachHang != null)
            {
                if(comment != null)
                {
                    foreach (var item in comment)
                    {
                        _context.Commentss.Remove(item);
                    }
                }
                if (hoadon != null)
                {
                    foreach (var hd in hoadon)
                    {
                        var chiTietHds = await _context.ChiTietHds.Where(ct => ct.MaHd == hd.MaHd).ToListAsync();
                        _context.ChiTietHds.RemoveRange(chiTietHds);
                        _context.HoaDons.Remove(hd);
                    }
                }
                _context.KhachHangs.Remove(khachHang);
                
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
       
    }
}
