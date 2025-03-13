using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using QuanLyKhoaTu.Helper;
using QuanLyKhoaTu.Models;

namespace QuanLyKhoaTu.Controllers
{
    public class UsersController : Controller
    {
        private readonly ModelDbContext _context;
        private readonly CheckUser _checkUser;
        public UsersController(ModelDbContext context, CheckUser checkUser)
        {
            _context = context;
            _checkUser = checkUser;
        }

        // GET: Users
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetUsers(string searchText = "", int page = 1, int limit = 50)
        {
            var query = _context.Users.AsQueryable(); // Đảm bảo là IQueryable để LINQ hoạt động đúng

            // 1️⃣ Lọc theo Username hoặc Name nếu có searchText
            if (!string.IsNullOrEmpty(searchText))
            {
                query = query.Where(u => u.Username.Contains(searchText) || u.Name.Contains(searchText));
            }

            // 2️⃣ Đếm tổng số Users sau khi lọc
            int totalUsers = await query.CountAsync();

            // 3️⃣ Phân trang chính xác với OFFSET
            var users = await query
                .OrderByDescending(u => u.Id)
                .Skip((page - 1) * limit) // Phân trang chính xác
                .Take(limit)
                .Select(u => new
                {
                    u.Id,
                    u.Name,
                    u.OrtherName,
                    u.Phone,
                    u.Username,
                    u.Email,
                    Role = u.AdminRole==true ? "Admin" : "User", 
                    u.AdminRole,
                    u.Enable
                })
                .ToListAsync();

            return Json(new
            {
                users,
                totalUsers,
                totalPages = (int)Math.Ceiling((double)totalUsers / limit), // Tổng số trang
                currentPage = page
            });
        }
        [HttpPost]
        public async Task<IActionResult> SaveUser(int id,string name,string orthername,string phone, string username, string password, bool adminRole)
        {
            MD5Hash _md5 = new MD5Hash();
            if (id == 0) // Thêm mới User
            {
                if (string.IsNullOrEmpty(password))
                {
                    //Mật khẩu mặc định 123456
                    password = _md5.GetMD5Working("123456");
                }
                else
                {
                    password = _md5.GetMD5Working(password);
                }
                var us = _context.Users.FirstOrDefault(u => u.Username == username);
                if (_context.Users.Any(x => x.Username == username))
                {
                    return Json(new { success = false,message="Đã tồn tại tên đăng nhập này!" });
                }
                else
                {

                    var newUser = new User
                    {
                        Name = name,
                        OrtherName = orthername,
                        Phone = phone,
                        Username = username,
                        Password = _md5.mahoamd5(password.Replace("&^%$", "")),
                        AdminRole = adminRole,
                        DateCreate = DateTime.Now,
                        Enable = true
                    };
                    _context.Users.Add(newUser);
                }
            }
            else // Cập nhật User (Không cập nhật mật khẩu nếu không nhập)
            {
                var user = await _context.Users.FindAsync(id);
                if (user == null)
                    return NotFound();

                user.Name = name;
                user.OrtherName = orthername;
                user.Phone = phone;
                user.Username = username;
                user.AdminRole = adminRole;

                if (!string.IsNullOrEmpty(password))
                {
                    string pass = _md5.GetMD5Working(password);
                    user.Password = _md5.mahoamd5(pass.Replace("&^%$", ""));
                }
            }

            await _context.SaveChangesAsync();
            return Json(new { success = true });
        }
        // 3️⃣ Xóa User
        [HttpPost]
        public async Task<IActionResult> DeleteUser(int id)
        {
            if (!_checkUser.CheckAdmin())
            {
                return Json(new { success = false, message = "Bạn chưa được cấp quyền xóa tài khoản!" });
            }
            else
            {
                if (_context.ZenCourses.Any(x => x.UserId == id))
                {
                    return Json(new { success = false, message = "Đang có khóa tu được tạo bởi người dùng này!" });
                }
                else
                {
                    var user = await _context.Users.FindAsync(id);
                    if (user == null)
                        return NotFound();

                    _context.Users.Remove(user);
                    await _context.SaveChangesAsync();
                    return Json(new { success = true });
                }
            }
        }
        [HttpPost]
        public async Task<IActionResult> ToggleEnable(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            // Đảo trạng thái Enable
            user.Enable = !user.Enable;

            await _context.SaveChangesAsync();
            return Json(new { success = true, newStatus = user.Enable });
        }

    }
}
