using Microsoft.AspNetCore.Mvc;
using WebApplication1.Data;
using WebApplication1.ViewModels;

namespace WebApplication1.ViewComponents
{
    public class MenuLoaiViewComponent : ViewComponent
    {
        private readonly EcommerceContext Db;

        public MenuLoaiViewComponent(EcommerceContext context)
        {
            Db = context;
        }

        public IViewComponentResult Invoke()
        {
            var data = Db.Loais.Select(l => new MenuLoaiVM
            {
                MaLoai = l.MaLoai,
                TenLoai = l.TenLoai,
                SoLuong = l.HangHoas.Count
            }).OrderBy(p => p.TenLoai);
            return View(data);
        }
    }
}
