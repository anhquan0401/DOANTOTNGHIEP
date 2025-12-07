using Microsoft.AspNetCore.Mvc;
using WebApplication1.Data;

namespace WebApplication1.Areas.Admin.Controllers
{
    public class AdminChatController : Controller
    {
        private readonly EcommerceContext db;

        public AdminChatController(EcommerceContext context)
        {
            db = context;
        }


        [HttpGet]
        public IActionResult GetAllUsers()
        {
            try
            {
                var users = db.KhachHangs
                    .Select(kh => new
                    {
                        maKh = "user_" + kh.MaKh,  // ← FORMAT SẴN
                        hoTen = kh.HoTen
                    }).ToList();

                Console.WriteLine($"✅ API trả về {users.Count} users");
                foreach (var u in users)
                {
                    Console.WriteLine($"   - {u.hoTen} (ID: {u.maKh})");
                }
                return Json(users);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error GetAllUsers: {ex.Message}");
                return Json(new List<object>());
            }
        }

    }
}
