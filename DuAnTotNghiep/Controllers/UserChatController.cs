using Microsoft.AspNetCore.Mvc;
using WebApplication1.Data;

namespace WebApplication1.Controllers
{
    public class UserChatController : Controller
    {
        private readonly EcommerceContext _context;

        public UserChatController(EcommerceContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult GetNhanViens()
        {
            var list = _context.NhanViens.Select(nv => new
            {
                maNv = "admin_" + nv.MaNv,  // ← Format sẵn
                hoTen = nv.HoTen
            }).ToList();

            Console.WriteLine($"✅ Trả về {list.Count} nhân viên");
            return Json(list);
        }
    }
}
