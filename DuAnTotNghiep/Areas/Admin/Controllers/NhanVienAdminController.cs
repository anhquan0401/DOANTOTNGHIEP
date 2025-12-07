using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Areas.Admin.Models;
using WebApplication1.Data;

namespace WebApplication1.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(AuthenticationSchemes = "AdminCookies", Policy = "RequireAdminRole")]
    public class NhanVienAdminController : Controller
    {
        private readonly EcommerceContext _ecommerceContext;

        public NhanVienAdminController(EcommerceContext ecommerceContext)
        {
            _ecommerceContext = ecommerceContext;
        }

        public IActionResult Index()
        {
            var nhanViens = _ecommerceContext.NhanViens.ToList();
            var results = nhanViens.Select(nv => new NhanVienAdminVM
            {
                MaNv = nv.MaNv,
                HoTen = nv.HoTen,
                Email = nv.Email,
                MaPq = nv.MaPq,
                XacNhan = nv.XacNhan
            }).ToList();
            return View(results);
        }

        public IActionResult Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var nhanVien = _ecommerceContext.NhanViens
                .FirstOrDefault(nv => nv.MaNv == id);
            if (nhanVien == null)
            {
                return NotFound();
            }
            var nhanVienVM = new NhanVienAdminVM
            {
                MaNv = nhanVien.MaNv,
                HoTen = nhanVien.HoTen,
                Email = nhanVien.Email,
                MaPq = nhanVien.MaPq,
                XacNhan = nhanVien.XacNhan
            };
            return View(nhanVienVM);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(NhanVienAdminVM nhanVienVM)
        {
            if (ModelState.IsValid)
            {
                var nhanVien = new NhanVien
                {
                    MaNv = nhanVienVM.MaNv,
                    HoTen = nhanVienVM.HoTen,
                    Email = nhanVienVM.Email,
                    MaPq = nhanVienVM.MaPq,
                    XacNhan = nhanVienVM.XacNhan
                };
                _ecommerceContext.NhanViens.Add(nhanVien);
                _ecommerceContext.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(nhanVienVM);
        }

        [HttpGet]
        public IActionResult Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var nhanVien = _ecommerceContext.NhanViens
                .FirstOrDefault(nv => nv.MaNv == id);
            if (nhanVien == null)
            {
                return NotFound();
            }
            var nhanVienVM = new NhanVienAdminVM
            {
                MaNv = nhanVien.MaNv,
                HoTen = nhanVien.HoTen,
                Email = nhanVien.Email,
                MaPq = nhanVien.MaPq,
                XacNhan = nhanVien.XacNhan
            };
            return View(nhanVienVM);
        }

        [HttpPost]
        public IActionResult Edit(string id, NhanVienAdminVM nhanVienVM)
        {
            if (id != nhanVienVM.MaNv)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                var nhanVien = _ecommerceContext.NhanViens
                    .FirstOrDefault(nv => nv.MaNv == id);
                if (nhanVien == null)
                {
                    return NotFound();
                }
                nhanVien.HoTen = nhanVienVM.HoTen;
                nhanVien.Email = nhanVienVM.Email;
                nhanVien.MaPq = nhanVienVM.MaPq;
                nhanVien.XacNhan = nhanVienVM.XacNhan;
                _ecommerceContext.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(nhanVienVM);
        }

        [HttpGet]
        public IActionResult Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var nhanVien = _ecommerceContext.NhanViens
                .FirstOrDefault(nv => nv.MaNv == id);
            if (nhanVien == null)
            {
                return NotFound();
            }
            var nhanVienVM = new NhanVienAdminVM
            {
                MaNv = nhanVien.MaNv,
                HoTen = nhanVien.HoTen,
                Email = nhanVien.Email,
                MaPq = nhanVien.MaPq,
                XacNhan = nhanVien.XacNhan
            };
            return View(nhanVienVM);
        }

        [HttpPost]
        public IActionResult DeleteConfirmed(string id)
        {
            var nhanVien = _ecommerceContext.NhanViens
                .FirstOrDefault(nv => nv.MaNv == id);
            if (nhanVien == null)
            {
                return NotFound();
            }
            _ecommerceContext.NhanViens.Remove(nhanVien);
            _ecommerceContext.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
    }
}
