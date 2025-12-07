using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;

namespace WebApplication1.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(AuthenticationSchemes = "AdminCookies", Policy = "RequireStaffAccess")]
    public class GopYAdminController : Controller
    {
        private readonly EcommerceContext _context;

        public GopYAdminController(EcommerceContext context)
        {
            _context = context;
        }

        // GET: Admin/Gopies
        public async Task<IActionResult> Index()
        {
            return View(await _context.Gopies.ToListAsync());
        }

        // GET: Admin/Gopies/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var gopY = await _context.Gopies
                .FirstOrDefaultAsync(m => m.MaGy == id);
            if (gopY == null)
            {
                return NotFound();
            }

            return View(gopY);
        }

        // GET: Admin/Gopies/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/Gopies/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MaGy,MaKh,NoiDung,NgayGy,HoTen,Email,DienThoai,CanTraLoi,TraLoi,NgayTl")] GopY gopY)
        {
            if (ModelState.IsValid)
            {
                gopY.MaGy = Guid.NewGuid();
                _context.Add(gopY);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(gopY);
        }

        // GET: Admin/Gopies/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var gopY = await _context.Gopies.FindAsync(id);
            if (gopY == null)
            {
                return NotFound();
            }
            return View(gopY);
        }

        // POST: Admin/Gopies/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("MaGy,MaKh,NoiDung,NgayGy,HoTen,Email,DienThoai,CanTraLoi,TraLoi,NgayTl")] GopY gopY)
        {
            if (id != gopY.MaGy)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(gopY);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GopYExists(gopY.MaGy))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(gopY);
        }

        // GET: Admin/Gopies/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var gopY = await _context.Gopies
                .FirstOrDefaultAsync(m => m.MaGy == id);
            if (gopY == null)
            {
                return NotFound();
            }

            return View(gopY);
        }

        // POST: Admin/Gopies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var gopY = await _context.Gopies.FindAsync(id);
            if (gopY != null)
            {
                _context.Gopies.Remove(gopY);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool GopYExists(Guid id)
        {
            return _context.Gopies.Any(e => e.MaGy == id);
        }
    }
}
