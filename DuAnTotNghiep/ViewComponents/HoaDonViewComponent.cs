using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol.Plugins;
using WebApplication1.Data;
using WebApplication1.Helpers;

namespace WebApplication1.ViewComponents
{
    public class HoaDonViewComponent : ViewComponent
    {
        private readonly EcommerceContext _context;

        public HoaDonViewComponent(EcommerceContext context)
        {
            _context = context;
        }

        //public IViewComponentResult Invoke(string currentTab)
        //{
        //    var query = _context.HoaDons;
        //    var UserID = HttpContext.User.Claims.SingleOrDefault(p => p.Type == MySetting.CART_CustomerID).Value;

        //    ViewBag.NewOrderCount = query.Count(hd => hd.MaKh == UserID && hd.MaTrangThai == 0);
        //    ViewBag.PaidCount = query.Count(hd => hd.MaKh == UserID && hd.MaTrangThai == 1);
        //    ViewBag.PendingShipmentCount = query.Count(hd => hd.MaKh == UserID && hd.MaTrangThai == 2);
        //    ViewBag.CompletedCount = query.Count(hd => hd.MaKh == UserID && hd.MaTrangThai == 3);
        //    ViewBag.CancelledCount = query.Count(hd => hd.MaKh == UserID && hd.MaTrangThai == -1);
        //    return View("Default");
        //}

        public async Task<IViewComponentResult> InvokeAsync(string currentTab)
        {
            var UserID = HttpContext.User.Claims.SingleOrDefault(p => p.Type == MySetting.CART_CustomerID).Value;
            var query = _context.HoaDons;
            var model = new
            {
                CurrentTab = currentTab,
                TotalOrders = await query.CountAsync(hd => hd.MaKh == UserID),
                NewOrderCount = await query.CountAsync(hd => hd.MaKh == UserID && hd.MaTrangThai == 1),
                PendingShipmentCount = await query.CountAsync(hd => hd.MaKh == UserID && hd.MaTrangThai == 3),
                PaidCount = await query.CountAsync(hd => hd.MaKh == UserID && hd.MaTrangThai == 2),
                LichSuHoaDonCount = await query.CountAsync(hd => hd.MaKh == UserID && hd.MaTrangThai == 4),
                CancelledCount = await query.CountAsync(hd => hd.MaKh == UserID && hd.MaTrangThai == 0)
            };

            return View(model);
        }
    }
}
