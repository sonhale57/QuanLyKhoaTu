using Microsoft.AspNetCore.Http;
using QuanLyKhoaTu.Models;
using System;
using System.Security.Cryptography;

namespace QuanLyKhoaTu.Helper
{
    public class CheckUser
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CheckUser(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public int GetUserId()
        {
            MD5Hash md5 = new MD5Hash();
            var userId = _httpContextAccessor.HttpContext?.Request.Cookies["userId"];
            if (!string.IsNullOrEmpty(userId))
            {
                return Convert.ToInt32(md5.Decrypt(userId));
            }
            return 0;
        }
        public string GetUsername()
        {
            MD5Hash md5 = new MD5Hash();
            var username = _httpContextAccessor.HttpContext?.Request.Cookies["username"];
            if (!string.IsNullOrEmpty(username))
            {
                return md5.Decrypt(username);
            }
            return "Unknow";
        }
        public bool CheckAdmin()
        {
            MD5Hash md5 = new MD5Hash();
            var userRole = _httpContextAccessor.HttpContext?.Request.Cookies["userRole"];
            if (!string.IsNullOrEmpty(userRole))
            {
                string role =  md5.Decrypt(userRole);
                return (role =="Admin"? true : false);
            }
            return false;
        }
    }
}
