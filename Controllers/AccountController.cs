using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using QuanLyKhoaTu.Helper;
using System.Net.Http;
using QuanLyKhoaTu.Models;

namespace QuanLyKhoaTu.Controllers
{
    public class AccountController : Controller
    {
        ModelDbContext _context;
        CheckUser _checkUser;
        public AccountController(ModelDbContext dbContext, CheckUser checkUser)
        {
            _context = dbContext;
            _checkUser = checkUser;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        [Route("/Account/Login")]
        public async Task<IActionResult> Login(string username, string password)
        {
            MD5Hash md5 = new MD5Hash();
            var user = _context.Users.FirstOrDefault(x=>x.Username == username);
            if (user == null)
            {
                Console.WriteLine($"Username: {username} Password: {password}! không tồn tại");
                return Ok(new { success = false, message = "Không tồn tại người dùng này!" });
            }
            else
            {
                password = md5.GetMD5Working(password);
                string pass = md5.mahoamd5(password.Replace("&^%$", ""));
                if (user.Password != pass)
                {
                    Console.WriteLine($"Username: {username} Password: {password}! Sai mật khẩu");
                    return Ok(new { success = false, message = "Sai mật khẩu!" });
                }
                if (user.Enable == false)
                {
                    return Ok(new { success = false, message = "Bạn đang không có quyền truy cập phần mềm!" });
                }
                CookieOptions option = new CookieOptions();
                option.Expires = DateTime.Now.AddDays(1);
                // Encrypting input data
                var iduserEncrypted = md5.Encrypt(user.Id.ToString()); // Example usage, replace appropriately.
                var userRoleEncrypted = md5.Encrypt((user.AdminRole==true?"Admin":"User"));
                var usernameEncrypted = md5.Encrypt(username);
                Response.Cookies.Append("userId", iduserEncrypted, option);
                Response.Cookies.Append("userRole", userRoleEncrypted, option);
                Response.Cookies.Append("username", usernameEncrypted, option);
                return Ok(new { success = true, message = "Đăng nhập thành công!", redirectUrl = "/home" });
            }
        }
        [HttpPost]
        [Route("/Account/SubmitChangePassword")]
        public async Task<IActionResult> SubmitChangePassword(string password)
        {
            int userId = _checkUser.GetUserId();
            MD5Hash md5 = new MD5Hash();
            var user = _context.Users.Find(userId);
            if (user == null)
            {
                return Ok(new { success = false, message = "Không tìm thấy thông tin người dùng!" });
            }
            else
            {
                password = md5.GetMD5Working(password);
                string pass = md5.mahoamd5(password.Replace("&^%$", ""));
                user.Password = pass;
                _context.Update(user);
                await _context.SaveChangesAsync();

                return Ok(new { success = true, message = "Đổi mật khẩu thành công!" });
            }
        }
        public IActionResult SignOut() {
            Response.Cookies.Delete("userId");
            Response.Cookies.Delete("userRole");
            Response.Cookies.Delete("username");
            return Redirect("/");
        }

    }
}
