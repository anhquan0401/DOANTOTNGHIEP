using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using System;
using WebApplication1.Data;
using WebApplication1.Helpers;
using WebApplication1.ViewModels;

namespace WebApplication1.Controllers
{
    [Authorize(AuthenticationSchemes = "UserCookies", Policy = "RequireUserRole")]
    public class ContactController : Controller
    {
        private readonly EcommerceContext _context;
        private readonly IMapper _mapper;

        public ContactController(EcommerceContext context, IMapper mapper) { 
            _context = context;
            _mapper = mapper;
        }
        [HttpGet]
        public ActionResult LienHe()
        {
            return View();
        }

        [Authorize]
        public IActionResult ContactSuccess()
        {
            return View("ContactSuccess");
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LienHe(ContactVM model)
        {
            var customerId = HttpContext.User.Claims.SingleOrDefault(p => p.Type == MySetting.CART_CustomerID).Value;
            var contact = new GopY
            {
               MaGy = Guid.NewGuid(),
               MaKh = customerId,
               NoiDung = model.Content,
               HoTen = model.HoTen,
               Email = model.Email,
               DienThoai = model.Phone,
               NgayGy = DateOnly.FromDateTime(DateTime.Now)
            };
            if (ModelState.IsValid)
            {
                _context.Add(contact);
                await _context.SaveChangesAsync();
                return View("ContactSuccess");
            }
            return View(contact);
        }
    }
}
