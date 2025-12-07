using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Areas.Admin.Models;
using WebApplication1.Data;
using WebApplication1.ViewModels;

namespace WebApplication1.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(AuthenticationSchemes = "AdminCookies", Policy = "RequireStaffAccess")]
    public class HoaDonAdminController : Controller
    {
        private readonly EcommerceContext _context;

        public HoaDonAdminController(EcommerceContext context)
        {
            _context = context;
        }

        // GET: Admin/HoaDonAdmin
        public async Task<IActionResult> Index(int TongOrder = 0, int TongThanhToanCOD = 0, int TongThanhToanCK = 0)
        {
            var hoaDons = _context.HoaDons.AsQueryable();
            TongOrder = _context.HoaDons.Where(r => r.NgayDat.Date == DateTime.Today).Count();
            ViewBag.TongOrder = TongOrder;

            TongThanhToanCOD = _context.HoaDons.Where(r => r.CachThanhToan == "COD").Count();
            ViewBag.COD = TongThanhToanCOD;

            TongThanhToanCK = _context.HoaDons.Where(r => r.CachThanhToan == "Chuyển khoản").Count();
            ViewBag.CK = TongThanhToanCK;


            var results = hoaDons.Select(r => new HoaDonAdminVM
            {
                MaHd = r.MaHd,
                MaKh = r.MaKh,
                NgayDat = r.NgayDat,
                NgayGiao = r.NgayGiao,
                HoTen = r.HoTen,
                DiaChi = r.DiaChi,
                DienThoai = r.DienThoai,
                CachThanhToan = r.CachThanhToan,
                CachVanChuyen = r.CachVanChuyen,
                PhiVanChuyen = r.PhiVanChuyen,
                MaTrangThai = r.MaTrangThai,
                NgayCapNhat = r.NgayCapNhat,
                GhiChu = r.GhiChu,
                Tong = r.ChiTietHds.Sum(ct => ct.DonGia * (ct.SoLuong - ct.GiamGia)),
            }).ToList();
            return View(results);
        }

        public IActionResult TongOrder() 
        {
            var result = _context.HoaDons.Where(r => r.NgayDat.Date == DateTime.Today).Select(r => new HoaDonAdminVM
            {
                MaHd = r.MaHd,
                MaKh = r.MaKh,
                NgayDat = r.NgayDat,
                NgayGiao = r.NgayGiao,
                HoTen = r.HoTen,
                DiaChi = r.DiaChi,
                DienThoai = r.DienThoai,
                CachThanhToan = r.CachThanhToan,
                CachVanChuyen = r.CachVanChuyen,
                PhiVanChuyen = r.PhiVanChuyen,
                MaTrangThai = r.MaTrangThai,
                NgayCapNhat = r.NgayCapNhat,
                GhiChu = r.GhiChu,
                Tong = r.ChiTietHds.Sum(ct => ct.DonGia * (ct.SoLuong - ct.GiamGia)),
            }).ToList();
            return View(result); 
        }

        public IActionResult TongCOD()
        {
            var result = _context.HoaDons.Where(r => r.CachThanhToan == "COD").Select(r => new HoaDonAdminVM
            {
                MaHd = r.MaHd,
                MaKh = r.MaKh,
                NgayDat = r.NgayDat,
                NgayGiao = r.NgayGiao,
                HoTen = r.HoTen,
                DiaChi = r.DiaChi,
                DienThoai = r.DienThoai,
                CachThanhToan = r.CachThanhToan,
                CachVanChuyen = r.CachVanChuyen,
                PhiVanChuyen = r.PhiVanChuyen,
                MaTrangThai = r.MaTrangThai,
                NgayCapNhat = r.NgayCapNhat,
                GhiChu = r.GhiChu,
                Tong = r.ChiTietHds.Sum(ct => ct.DonGia * (ct.SoLuong - ct.GiamGia)),
            }).ToList();
            return View(result);
        }

        public IActionResult TongCK()
        {
            var result = _context.HoaDons.Where(r => r.CachThanhToan == "Chuyển khoản").Select(r => new HoaDonAdminVM
            {
                MaHd = r.MaHd,
                MaKh = r.MaKh,
                NgayDat = r.NgayDat,
                NgayGiao = r.NgayGiao,
                HoTen = r.HoTen,
                DiaChi = r.DiaChi,
                DienThoai = r.DienThoai,
                CachThanhToan = r.CachThanhToan,
                CachVanChuyen = r.CachVanChuyen,
                PhiVanChuyen = r.PhiVanChuyen,
                MaTrangThai = r.MaTrangThai,
                NgayCapNhat = r.NgayCapNhat,
                GhiChu = r.GhiChu,
                Tong = r.ChiTietHds.Sum(ct => ct.DonGia * (ct.SoLuong - ct.GiamGia)),
            }).ToList();
            return View(result);
        }

        // GET: Admin/HoaDonAdmin/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var hoaDon = await _context.HoaDons
                .Include(h => h.MaKhNavigation)
                .Include(h => h.MaNvNavigation)
                .Include(h => h.MaTrangThaiNavigation)
                .FirstOrDefaultAsync(m => m.MaHd == id);
            if (hoaDon == null)
            {
                return NotFound();
            }

            return View(hoaDon);
        }

        // GET: Admin/HoaDonAdmin/Create
        public IActionResult Create()
        {
            ViewData["MaKh"] = new SelectList(_context.KhachHangs, "MaKh", "MaKh");
            ViewData["MaNv"] = new SelectList(_context.NhanViens, "MaNv", "MaNv");
            ViewData["MaTrangThai"] = new SelectList(_context.TrangThais, "MaTrangThai", "MaTrangThai");
            return View();
        }

        // POST: Admin/HoaDonAdmin/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public async Task<IActionResult> Create([Bind("MaHd,MaKh,NgayDat,NgayCan,NgayGiao,HoTen,DiaChi,DienThoai,CachThanhToan,CachVanChuyen,PhiVanChuyen,MaTrangThai,MaNv,GhiChu")] HoaDon hoaDon)
        {
            if (ModelState.IsValid)
            {
                _context.Add(hoaDon);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["MaKh"] = new SelectList(_context.KhachHangs, "MaKh", "MaKh", hoaDon.MaKh);
            ViewData["MaNv"] = new SelectList(_context.NhanViens, "MaNv", "MaNv", hoaDon.MaNv);
            ViewData["MaTrangThai"] = new SelectList(_context.TrangThais, "MaTrangThai", "MaTrangThai", hoaDon.MaTrangThai);
            return View(hoaDon);
        }

        // GET: Admin/HoaDonAdmin/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var hoaDon = await _context.HoaDons.FindAsync(id);
            if (hoaDon == null)
            {
                return NotFound();
            }

            var hoaDonAdminVM = new HoaDonAdminVM ()
            {
                NgayDat = hoaDon.NgayDat,
                NgayGiao = hoaDon.NgayGiao,
                HoTen = hoaDon.HoTen,
                DiaChi = hoaDon.DiaChi,
                DienThoai = hoaDon.DienThoai,
                CachThanhToan = hoaDon.CachThanhToan,
                CachVanChuyen = hoaDon.CachVanChuyen,
                PhiVanChuyen = hoaDon.PhiVanChuyen,
                MaTrangThai = hoaDon.MaTrangThai,
                NgayCapNhat = hoaDon.NgayCapNhat,
                GhiChu = hoaDon.GhiChu,
            };
            ViewData["MaHd"] = hoaDon.MaHd;
            ViewData["MaKh"] = hoaDon.MaKh;
            return View(hoaDonAdminVM);
        }

        // POST: Admin/HoaDonAdmin/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, HoaDonAdminVM model)
        {
            if(id == null)
            {
                return NotFound();
            }   

            var hoaDon = await _context.HoaDons.FindAsync(id);

            if(!ModelState.IsValid)
            {
                ViewData["MaHd"] = hoaDon.MaHd;
                ViewData["MaKh"] = hoaDon.MaKh;
            }
            hoaDon.NgayDat = model.NgayDat;
            hoaDon.NgayGiao = model.NgayGiao;
            hoaDon.HoTen = model.HoTen;
            hoaDon.DiaChi = model.DiaChi;
            hoaDon.DienThoai = model.DienThoai;
            hoaDon.CachThanhToan = model.CachThanhToan;
            hoaDon.CachVanChuyen = model.CachVanChuyen;
            hoaDon.PhiVanChuyen = model.PhiVanChuyen;
            hoaDon.MaTrangThai = model.MaTrangThai;
            hoaDon.NgayCapNhat = model.NgayCapNhat;
            hoaDon.GhiChu = model.GhiChu;

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // GET: Admin/HoaDonAdmin/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var hoaDon = await _context.HoaDons
                .Include(h => h.MaKhNavigation)
                .Include(h => h.MaNvNavigation)
                .Include(h => h.MaTrangThaiNavigation)
                .FirstOrDefaultAsync(m => m.MaHd == id);
            if (hoaDon == null)
            {
                return NotFound();
            }

            return View(hoaDon);
        }

        // POST: Admin/HoaDonAdmin/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var chiTietHds = _context.ChiTietHds.Where(ct => ct.MaHd == id);
            _context.ChiTietHds.RemoveRange(chiTietHds);

            var hoaDon = await _context.HoaDons.FindAsync(id);
            if (hoaDon != null)
            {
                _context.HoaDons.Remove(hoaDon);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool HoaDonExists(int id)
        {
            return _context.HoaDons.Any(e => e.MaHd == id);
        }

        //public IActionResult TotalOderNew()
        //{
        //    var oderNew = _context.HoaDons.SingleOrDefault(o => o.NgayDat == DateTime.Today);
            

        //    return View(oderNew);
            
        //}
    }
}
