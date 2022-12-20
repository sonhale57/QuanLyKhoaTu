using QuanLyKhoaTu.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Services.Description;

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
        [HttpGet]
        public ActionResult Sendmail()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Sendmail(string receiver, string subject, string message)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var senderEmail = new MailAddress("superbrain.noreply@gmail.com", "Jamil");
                    var receiverEmail = new MailAddress("sonhale57@gmail.com", "Receiver");
                    var password = "rhewihyggxsizliv";
                    var sub = subject;
                    var body = message;
                    var smtp = new SmtpClient
                    {
                        Host = "smtp.gmail.com",
                        Port = 587,
                        EnableSsl = true,
                        DeliveryMethod = SmtpDeliveryMethod.Network,
                        UseDefaultCredentials = false,
                        Credentials = new NetworkCredential(senderEmail.Address, password)
                    };
                    using (var mess = new MailMessage(senderEmail, receiverEmail)
                    {
                        Subject = "Test nè",
                        Body = "Đây là nội dung mail test"
                    })
                    {
                        smtp.Send(mess);
                    }
                    return View();
                }
            }
            catch (Exception)
            {
                ViewBag.Error = "Some Error";
            }
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