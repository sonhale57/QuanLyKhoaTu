using QuanLyKhoaTu.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QuanLyKhoaTu.Areas.Admin.Controllers
{
    public class DashboardController : Controller
    {
        ModelDbContext db = new ModelDbContext();
        // GET: Admin/Dashboard
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Sendmail()
        {
            return View();
        }
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(string username, string password)
        {
            var user = db.Users.SingleOrDefault(m => m.Username == username && m.Password == password);
            if (user != null)
            {
                if (user.Enable == false || user.Enable == null)
                {
                    TempData["err"] = "<div class='alert alert-danger'>Tài khoản chưa được kích hoạt!</div>";
                }
                else
                {
                    Session["Name"] = user.Name;
                    Session["id"] = user.id;
                    Session.Add("CurrentUser", user);
                    return RedirectToAction("Index");
                }
            }
            else
            {
                TempData["err"] = "<div class='alert alert-danger'>Sai tên tài khoản hoặc mật khẩu!</div>";
            }
            return View();
        }

        public ActionResult Logout()
        {
            Session["Name"] = null;
            Session["id"] = null;
            return RedirectToAction("Index");
        }
    }
}