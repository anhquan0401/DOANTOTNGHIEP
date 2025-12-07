using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApplication1.Areas.Admin.Controllers
{

    [Area("Admin")]
    // Sử dụng scheme AdminCookies cho cả hai
    [Authorize(AuthenticationSchemes = "AdminCookies", Policy = "RequireStaffAccess")]
    public class HomeAdminController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
