using Microsoft.AspNetCore.Mvc;
using WebApplication1.Helpers;
using WebApplication1.ViewModels;

namespace WebApplication1.ViewComponents
{
    public class CartViewComponent : ViewComponent
    {

        public IViewComponentResult Invoke()
        {
            var Cart = HttpContext.Session.Get<List<CartVM>>(MySetting.CART_KEY) ?? new List<CartVM>();

            return View("CartPanel", new CartModel
            {
                SoLuong = Cart.Select(p => p.MaHH).Count()
            });
        }

    }
}
