using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using QuanLyKhoaTu.Helper;

namespace QuanLyKhoaTu.ViewComponents
{
    public class UserInfoViewComponent : ViewComponent
    {
        private readonly CheckUser _checkUser;
        public UserInfoViewComponent(CheckUser checkUser)
        {
            _checkUser = checkUser;
        }
        public IViewComponentResult Invoke()
        {
            string username = _checkUser.GetUsername();
            return View("Default", username);
        }
    }
}
