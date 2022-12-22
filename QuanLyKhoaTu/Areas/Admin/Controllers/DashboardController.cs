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
            ViewBag.IdKhoaTu = new SelectList(db.KhoaTus.OrderByDescending(x=>x.Active), "id", "Ten");
            return View();
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Sendmail(int idKhoaTu, string content)
        {
            try                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                          
            {
                if (ModelState.IsValid)
                {
                    var linq = from ts in db.TuSinhs
                               join dk in db.DangKyKhoaTus
                               on ts.id equals dk.IdTuSinh
                               where dk.IdKhoaTu== idKhoaTu && dk.Trangthai==0
                               select ts;

                    List<string> listEmail = new List<string>();
                    listEmail.Add("sonhale57.data@gmail.com");
                    listEmail.Add("sonhale57.tech@gmail.com");
                    listEmail.Add("quybow@gmail.com");
                    var senderEmail = new MailAddress("sonhale57@gmail.com", "Ban Văn Hóa Thông Tin Chùa Vạn Đức");
                    var password = "okwfibwakuxpyvij";
                    var sub = "QUAN TRỌNG: Xác nhận tham gia khóa tu";
                    var body = content;
                    var smtp = new SmtpClient
                    {
                        Host = "smtp.gmail.com",
                        Port = 587,
                        EnableSsl = true,
                        DeliveryMethod = SmtpDeliveryMethod.Network,
                        UseDefaultCredentials = false,
                        Credentials = new NetworkCredential(senderEmail.Address, password)
                    };
                    foreach(var item in linq) {
                        var receiverEmail = new MailAddress(item.Email, "Tu Sinh");
                        using (var mess = new MailMessage(senderEmail, receiverEmail)
                        {
                            Subject = sub,
                            Body = body,
                            IsBodyHtml= true
                        })
                        {
                            smtp.Send(mess);
                        }
                    }
                    string json = "{\"status\":\"ok\"}";
                    Response.Clear();
                    Response.ContentType = "application/json; charset=utf-8";
                    Response.Write(json);
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