using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace WebApplication1.ViewComponents
{
    public class UserNameViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var userName = User.Identity.IsAuthenticated ? User.Identity.Name : null;
            return View("NameUserPanel", userName);
        }
    }
}
