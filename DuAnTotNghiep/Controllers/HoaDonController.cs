using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WebApplication1.Data;
using WebApplication1.Helpers;
using WebApplication1.ViewModels;

namespace WebApplication1.Controllers
{
    [Authorize(AuthenticationSchemes = "UserCookies", Policy = "RequireUserRole")]
    public class HoaDonController : Controller
    {
        private readonly EcommerceContext _context;

        public HoaDonController(EcommerceContext context) 
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Index()
        {
            ViewBag.CurrentTab = "Index";
            var UserID = HttpContext.User.Claims.SingleOrDefault(p => p.Type == MySetting.CART_CustomerID).Value;
            var query = _context.HoaDons.Where(hd => hd.MaKh == UserID);

            var hoaDons = query
                .OrderByDescending(hd => hd.MaHd)
                .Select(hd => new LichSuHoaDonVM
                {
                    MaHd = hd.MaHd,
                    ChiTietHds = hd.ChiTietHds.Select(ct => new ChiTietHdVM
                    {
                        TenHh = ct.MaHhNavigation.TenHh,
                        SoLuong = ct.SoLuong,
                        DonGia = ct.DonGia,
                        GiamGia = ct.GiamGia,
                        Hinh = ct.MaHhNavigation.Hinh,
                        ThanhTien = ct.SoLuong * (decimal)ct.DonGia,
                    }).ToList()
                }).ToList();
            return View(hoaDons);
        }

        public ActionResult LichSuHoaDon() 
        {
            ViewBag.CurrentTab = "LichSuHoaDon";
            var UserID = HttpContext.User.Claims.SingleOrDefault(p => p.Type == MySetting.CART_CustomerID).Value;
            var query = _context.HoaDons.Where(hd => hd.MaKh == UserID);

            var hoaDons = query
                .Where(hd => hd.MaTrangThai == 4)   
                .OrderByDescending(hd => hd.MaHd)
                .Select(hd => new LichSuHoaDonVM
                {
                    MaHd = hd.MaHd,
                    ChiTietHds = hd.ChiTietHds.Select(ct => new ChiTietHdVM
                    {
                        TenHh = ct.MaHhNavigation.TenHh,
                        SoLuong = ct.SoLuong,
                        DonGia = ct.DonGia,
                        GiamGia = ct.GiamGia,
                        Hinh = ct.MaHhNavigation.Hinh,
                        ThanhTien = ct.SoLuong * (decimal)ct.DonGia,
                    }).ToList()
                }).ToList();
            
            return View(hoaDons);
        }

        public ActionResult NewOrder()
        {
            ViewBag.CurrentTab = "NewOrder";
            var UserID = HttpContext.User.Claims.SingleOrDefault(p => p.Type == MySetting.CART_CustomerID).Value;
            var query = _context.HoaDons.Where(hd => hd.MaKh == UserID);

            var hoaDons = query
                .Where(hd => hd.MaTrangThai == 1)
                .OrderByDescending(hd => hd.MaHd)
                .Select(hd => new LichSuHoaDonVM
                {
                    MaHd = hd.MaHd,
                    ChiTietHds = hd.ChiTietHds
                    .Select(ct => new ChiTietHdVM
                    {
                        TenHh = ct.MaHhNavigation.TenHh,
                        SoLuong = ct.SoLuong,
                        DonGia = ct.DonGia,
                        GiamGia = ct.GiamGia,
                        Hinh = ct.MaHhNavigation.Hinh,
                        ThanhTien = ct.SoLuong * (decimal)ct.DonGia,
                    }).ToList(),
                    DiscountTotal = hd.InvoiceVouchers.Sum(iv => iv.DiscountAmount)
                })
                .ToList();
            // Sau đó khi render
            foreach (var hd in hoaDons)
            {
                foreach (var ct in hd.ChiTietHds)
                {
                    // Nếu muốn áp dụng discount theo % của toàn hóa đơn:
                    ct.ThanhTien -= (ct.ThanhTien * hd.DiscountTotal) / 100;
                }
            }
            return View(hoaDons);
        }

        public ActionResult Paid()
        {
            ViewBag.CurrentTab = "Paid";
            var UserID = HttpContext.User.Claims.SingleOrDefault(p => p.Type == MySetting.CART_CustomerID).Value;
            var query = _context.HoaDons.Where(hd => hd.MaKh == UserID);

            var hoaDons = query
                .Where(hd => hd.MaTrangThai == 2)
                .OrderByDescending(hd => hd.MaHd)
                .Select(hd => new LichSuHoaDonVM
                {
                    MaHd = hd.MaHd,
                    ChiTietHds = hd.ChiTietHds.Select(ct => new ChiTietHdVM
                    {
                        TenHh = ct.MaHhNavigation.TenHh,
                        SoLuong = ct.SoLuong,
                        DonGia = ct.DonGia,
                        GiamGia = ct.GiamGia,
                        Hinh = ct.MaHhNavigation.Hinh,
                        ThanhTien = ct.SoLuong * (decimal)ct.DonGia,
                    }).ToList()
                }).ToList();
            
            return View(hoaDons);
        }

        public ActionResult PendingShipment()
        {
            ViewBag.CurrentTab = "PendingShipment";
            var UserID = HttpContext.User.Claims.SingleOrDefault(p => p.Type == MySetting.CART_CustomerID).Value;
            var query = _context.HoaDons.Where(hd => hd.MaKh == UserID);

            var hoaDons = query
                .Where(hd => hd.MaTrangThai == 3)
                .OrderByDescending(hd => hd.MaHd)
                .Select(hd => new LichSuHoaDonVM
                {
                    MaHd = hd.MaHd,
                    ChiTietHds = hd.ChiTietHds.Select(ct => new ChiTietHdVM
                    {
                        TenHh = ct.MaHhNavigation.TenHh,
                        SoLuong = ct.SoLuong,
                        DonGia = ct.DonGia,
                        GiamGia = ct.GiamGia,
                        Hinh = ct.MaHhNavigation.Hinh,
                        ThanhTien = ct.SoLuong * (decimal)ct.DonGia,
                    }).ToList()
                }).ToList();
            
            return View(hoaDons);
        }

        public ActionResult Cancelled()
        {
            ViewBag.CurrentTab = "Cancelled";
            var UserID = HttpContext.User.Claims.SingleOrDefault(p => p.Type == MySetting.CART_CustomerID).Value;
            var query = _context.HoaDons.Where(hd => hd.MaKh == UserID);

            var hoaDons = query
                .Where(hd => hd.MaTrangThai == 5)
                .OrderByDescending(hd => hd.MaHd)
                .Select(hd => new LichSuHoaDonVM
                {
                    MaHd = hd.MaHd,
                    ChiTietHds = hd.ChiTietHds.Select(ct => new ChiTietHdVM
                    {
                        TenHh = ct.MaHhNavigation.TenHh,
                        SoLuong = ct.SoLuong,
                        DonGia = ct.DonGia,
                        GiamGia = ct.GiamGia,
                        Hinh = ct.MaHhNavigation.Hinh,
                        ThanhTien = ct.SoLuong * (decimal)ct.DonGia,
                    }).ToList()
                }).ToList();
            return View(hoaDons);
        }



        public ActionResult Details(int id)
        {
            var UserID = HttpContext.User.Claims.SingleOrDefault(p => p.Type == MySetting.CART_CustomerID).Value;
            var hoaDon = _context.HoaDons
                .Where(hd => hd.MaKh == UserID && hd.MaHd == id)
                .Select(hd => new LichSuHoaDonVM
                {
                    MaHd = hd.MaHd,
                    NgayDat = hd.NgayDat,
                    NgayGiao = hd.NgayGiao,
                    HoTen = hd.HoTen,
                    DienThoai = hd.DienThoai,
                    TongTien = hd.ChiTietHds.Sum(ct => ct.DonGia * (ct.SoLuong - ct.GiamGia)),
                    DiaChi = hd.DiaChi,
                    CachThanhToan = hd.CachThanhToan,
                    CachVanChuyen = hd.CachVanChuyen,
                    PhiVanChuyen = hd.PhiVanChuyen,
                    TrangThai = hd.MaTrangThaiNavigation.TenTrangThai,
                    NgayCapNhat = hd.NgayCapNhat,
                    ChiTietHds = hd.ChiTietHds.Select(ct => new ChiTietHdVM
                    {
                        TenHh = ct.MaHhNavigation.TenHh,
                        SoLuong = ct.SoLuong,
                        DonGia = ct.DonGia,
                        GiamGia = ct.GiamGia,
                        Hinh = ct.MaHhNavigation.Hinh,
                        ThanhTien = ct.SoLuong * (decimal)ct.DonGia,
                    }).ToList()
                }).FirstOrDefault();

            if (hoaDon == null)
            {
                return NotFound();
            }

            return View(hoaDon);
        }

    }
}
