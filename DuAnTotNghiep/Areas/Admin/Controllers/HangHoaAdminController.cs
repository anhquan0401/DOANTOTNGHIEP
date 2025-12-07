using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Hosting;
using WebApplication1.Areas.Admin.Models;
using WebApplication1.Data;
using WebApplication1.Helpers;
using WebApplication1.ViewModels;

namespace WebApplication1.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(AuthenticationSchemes = "AdminCookies", Policy = "RequireStaffAccess")]
    public class HangHoaAdminController : Controller
    {
        private readonly EcommerceContext _context;
        private readonly IWebHostEnvironment _environment;

        public HangHoaAdminController(EcommerceContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        // GET: Admin/HangHoaAdmin
        public async Task<IActionResult> Index(int? loai, int page = 1, int Tong = 0)
        {
            var hangHoas = _context.HangHoas.AsQueryable();
            if(loai.HasValue)
            {
                hangHoas = hangHoas.Where(hh => hh.MaLoai == loai.Value);
            }
            int pageSize = 9;
            int skip = (page - 1) * pageSize;

            int totalItems = hangHoas.Count();
            int totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);
            
            ViewBag.TotalPages = totalPages;
            ViewBag.CurrentPage = page;
            ViewBag.Loais = loai;
            Tong = hangHoas.Where(r => r.NgaySx.Date == DateTime.Today).Count();
            ViewBag.Total = Tong;

            var result = hangHoas.Skip(skip).Take(pageSize).Select(r => new HangHoaAdminVM
            {
                MaHH = r.MaHh,
                TenHH = r.TenHh,
                SoLuong = r.SoLuong,
                TenAlias = r.TenAlias,
                MaLoai = r.MaLoai,
                MoTaDonVi = r.MoTaDonVi,
                DonGia = (double)r.DonGia,
                HinhFile = r.Hinh,
                NgaySx = r.NgaySx,
                SoLanXem = r.SoLanXem,
                MoTa = r.MoTa,
                MaNcc = r.MaNcc
            }).ToList();

            return View(result);
        }

        public IActionResult Total()
        {
            var result = _context.HangHoas.Where(r => r.NgaySx.Date == DateTime.Today).Select(r => new HangHoaAdminVM
            {
                MaHH = r.MaHh,
                TenHH = r.TenHh,
                SoLuong = r.SoLuong,
                TenAlias = r.TenAlias,
                MaLoai = r.MaLoai,
                MoTaDonVi = r.MoTaDonVi,
                DonGia = (double)r.DonGia,
                HinhFile = r.Hinh,
                NgaySx = r.NgaySx,
                SoLanXem = r.SoLanXem,
                MoTa = r.MoTa,
                MaNcc = r.MaNcc
            }).ToList();

            return View(result);
        }

        // GET: Admin/HangHoaAdmin/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var hangHoa = await _context.HangHoas
                .Include(h => h.MaLoaiNavigation)
                .Include(h => h.MaNccNavigation)
                .FirstOrDefaultAsync(m => m.MaHh == id);
            if (hangHoa == null)
            {
                return NotFound();
            }

            return View(hangHoa);
        }

        // GET: Admin/HangHoaAdmin/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/HangHoaAdmin/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public async Task<IActionResult> Create(HangHoaAdminVM model)
        {
            if (model.Hinh == null)
            {
                ModelState.AddModelError("Hinh", "Bat Buoc");
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            string newFileName = DateTime.Now.ToString("yyyyMMddHHmmssfff");
            newFileName += Path.GetExtension(model.Hinh!.FileName);

            string imageFullPath = _environment.WebRootPath + "/Hinh/HangHoa/" + newFileName;
            using (var stream = System.IO.File.Create(imageFullPath))
            {
                model.Hinh.CopyTo(stream);
            }

            HangHoa hangHoas = new HangHoa()
            {
                TenHh = model.TenHH,
                SoLuong = model.SoLuong,
                TenAlias = model.TenAlias,
                MaLoai = model.MaLoai,
                MoTaDonVi = model.MoTaDonVi,
                DonGia = model.DonGia,
                Hinh = newFileName,
                NgaySx = DateTime.Now,
                SoLanXem = model.SoLanXem,
                MoTa = model.MoTa,
                MaNcc = model.MaNcc,
            };
            _context.HangHoas.Add(hangHoas);

            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

        // GET: Admin/HangHoaAdmin/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var hangHoa = await _context.HangHoas.FindAsync(id);

            var hangHoaVMs = new HangHoaAdminVM()
            {
                TenHH = hangHoa.TenHh,
                SoLuong = hangHoa.SoLuong,
                TenAlias = hangHoa.TenAlias,
                MaLoai = hangHoa.MaLoai,
                MoTaDonVi = hangHoa.MoTaDonVi,
                DonGia = (double)hangHoa.DonGia,
                SoLanXem = hangHoa.SoLanXem,
                MoTa = hangHoa.MoTa,
                MaNcc = hangHoa.MaNcc
            };

            ViewData["MaHH"] = hangHoa.MaHh;
            ViewData["ImgaeFileName"] = hangHoa.Hinh;
            ViewData["NgaySX"] = hangHoa.NgaySx.ToString("MM/dd/yyyy");

            return View(hangHoaVMs);
        }

        // POST: Admin/HangHoaAdmin/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id, HangHoaAdminVM model)
        {
            if (id == null)
            {
                return NotFound();
            }
            var hangHoa = await _context.HangHoas.FindAsync(id);

            if (!ModelState.IsValid)
            {
                ViewData["MaHH"] = hangHoa.MaHh;
                ViewData["ImgaeFileName"] = hangHoa.Hinh;
                ViewData["NgaySX"] = hangHoa.NgaySx.ToString("MM/dd/yyyy");
                return View(model);
            }


            // update the image file if we have a new image file
            string newFileName = hangHoa.Hinh;
            if (model.Hinh != null)
            {
                newFileName = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                newFileName += Path.GetExtension(model.Hinh!.FileName);

                string imageFullPath = _environment.WebRootPath + "/Hinh/HangHoa/" + newFileName;
                using (var stream = System.IO.File.Create(imageFullPath))
                {
                    model.Hinh.CopyTo(stream);
                }

                // delete the old image
                string oldImageFullPath = _environment.WebRootPath + "/Hinh/HangHoa/" + hangHoa.Hinh;
                System.IO.File.Delete(oldImageFullPath);
            }

            // update the product in the database
            hangHoa.TenHh = model.TenHH;
            hangHoa.SoLuong = model.SoLuong;
            hangHoa.TenAlias = model.TenAlias;
            hangHoa.MaLoai = model.MaLoai;
            hangHoa.MoTaDonVi = model.MoTaDonVi;
            hangHoa.DonGia = model.DonGia;
            hangHoa.Hinh = newFileName;
            hangHoa.SoLanXem = model.SoLanXem;
            hangHoa.MoTa = model.MoTa;
            hangHoa.MaNcc = model.MaNcc;

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // POST: Admin/HangHoaAdmin/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var comment = _context.Commentss.Where(c => c.MaHh == id);
            _context.Commentss.RemoveRange(comment);

            var hangHoa = await _context.HangHoas.FindAsync(id);

            if (hangHoa == null)
            {
                return NotFound();
            }
            // delete the old image
            string oldImageFullPath = _environment.WebRootPath + "/Hinh/HangHoa/" + hangHoa.Hinh;
            System.IO.File.Delete(oldImageFullPath);

            _context.HangHoas.Remove(hangHoa);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
        private bool HangHoaExists(int id)
        {
            return _context.HangHoas.Any(e => e.MaHh == id);
        }
    }
}
