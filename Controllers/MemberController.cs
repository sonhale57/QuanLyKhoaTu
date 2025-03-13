using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.IO;
using OfficeOpenXml;
using QuanLyKhoaTu.Helper;
using QuanLyKhoaTu.Models;
using QRCoder; // Thư viện tạo QR Code
using System.Drawing;
using System.Drawing.Imaging;

namespace QuanLyKhoaTu.Controllers
{
    public class MemberController : Controller
    {
        private readonly ModelDbContext _context;
        private readonly CheckUser _checkUser;
        public MemberController(ModelDbContext context, CheckUser checkUser)
        {
            _context = context;
            _checkUser = checkUser;
        }
        [Route("thanh-vien")]
        public IActionResult Index()
        {
            return View();
        }

        [Route("thanh-vien-da-xoa")]
        public IActionResult TrashList()
        {
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> GetLists(string searchText = "", int page = 1, int limit = 50)
        {
            var query = _context.Members.Where(x=>x.Enable==true).AsQueryable(); // Đảm bảo là IQueryable để LINQ hoạt động đúng

            // 1️⃣ Lọc theo Username hoặc Name nếu có searchText
            if (!string.IsNullOrEmpty(searchText))
            {
                query = query.Where(u => u.OrtherName.Contains(searchText) || u.Name.Contains(searchText));
            }

            var existingCodes = (await _context.Members
                            .Where(u => u.Code.StartsWith("VD"))
                            .Select(u => u.Code.Substring(2)) // Lấy phần số (bỏ "VD")
                            .ToListAsync()) // Chuyển dữ liệu ra khỏi database trước
                            .Where(code => int.TryParse(code, out _)) // Lọc số hợp lệ
                            .Select(int.Parse) // Chuyển về kiểu số
                            .OrderBy(n => n) // Sắp xếp tăng dần
                            .ToList();

            int nextNumber = Enumerable.Range(1, existingCodes.Count + 1) // Tạo danh sách [1,2,3,...]
                .Except(existingCodes) // Loại bỏ các số đã có
                .First();

            string newCode = $"VD{nextNumber:D5}"; // VD00001, VD00002, ...

            int totalUsers = await query.CountAsync();

            // 3️⃣ Phân trang chính xác với OFFSET
            var members = await query
                .OrderByDescending(u => u.Id)
                .Skip((page - 1) * limit) // Phân trang chính xác
                .Take(limit)
                .Select(u => new
                {
                    u.Id,
                    u.Name,
                    u.Code,
                    u.OrtherName,
                    u.Phone,
                    u.OrtherPhone,
                    BirthDay=u.BirthDay.Value.ToString("dd/MM/yyyy"),
                    editBirthDay = u.BirthDay.Value.ToString("yyyy-MM-dd"),
                    Year = u.BirthDay.Value.Year,
                    DateCreate = u.DateCreate.Value.ToString("dd/MM/yyyy"),
                    u.DateUpdate,
                    u.UserId,
                    u.UpdateBy,
                    u.ImageIdentity,
                    StatusIdentity = (!string.IsNullOrEmpty(u.ImageIdentity)?true:false),
                    CountJoin = _context.JoinCourses.Count(x=>x.MemberId==u.Id),
                    NumberIdentity=(u.NumberIdentity==null?"Đang cập nhật":u.NumberIdentity),
                    u.PrintCount,
                    u.Address,
                    u.Gender,
                })
                .ToListAsync();

            return Json(new
            {
                members,
                totalUsers,
                newCode,
                totalPages = (int)Math.Ceiling((double)totalUsers / limit), // Tổng số trang
                currentPage = page
            });
        }
        [HttpGet]
        public async Task<IActionResult> GetRemoveLists(string searchText = "", int page = 1, int limit = 50)
        {
            var query = _context.Members.Where(x => x.Enable == false).AsQueryable(); // Đảm bảo là IQueryable để LINQ hoạt động đúng

            // 1️⃣ Lọc theo Username hoặc Name nếu có searchText
            if (!string.IsNullOrEmpty(searchText))
            {
                query = query.Where(u => u.OrtherName.Contains(searchText) || u.Name.Contains(searchText));
            }

            var existingCodes = (await _context.Members
                            .Where(u => u.Code.StartsWith("VD"))
                            .Select(u => u.Code.Substring(2)) // Lấy phần số (bỏ "VD")
                            .ToListAsync()) // Chuyển dữ liệu ra khỏi database trước
                            .Where(code => int.TryParse(code, out _)) // Lọc số hợp lệ
                            .Select(int.Parse) // Chuyển về kiểu số
                            .OrderBy(n => n) // Sắp xếp tăng dần
                            .ToList();

            // Tìm số nhỏ nhất bị thiếu
            int nextNumber = Enumerable.Range(1, existingCodes.Count + 1) // Tạo danh sách [1,2,3,...]
                .Except(existingCodes) // Loại bỏ các số đã có
                .First(); // Lấy số nhỏ nhất bị thiếu

            string newCode = $"VD{nextNumber:D5}"; // VD00001, VD00002, ...

            int totalUsers = await query.CountAsync();

            // 3️⃣ Phân trang chính xác với OFFSET
            var members = await query
                .OrderByDescending(u => u.Id)
                .Skip((page - 1) * limit) // Phân trang chính xác
                .Take(limit)
                .Select(u => new
                {
                    u.Id,
                    u.Name,
                    u.Code,
                    u.OrtherName,
                    u.Phone,
                    u.OrtherPhone,
                    BirthDay = u.BirthDay.Value.ToString("dd/MM/yyyy"),
                    editBirthDay = u.BirthDay.Value.ToString("yyyy-MM-dd"),
                    Year = u.BirthDay.Value.Year,
                    DateCreate = u.DateCreate.Value.ToString("dd/MM/yyyy"),
                    u.DateUpdate,
                    u.UserId,
                    u.UpdateBy,
                    StatusIdentity = (!string.IsNullOrEmpty(u.ImageIdentity) ? true : false),
                    CountJoin = _context.JoinCourses.Count(x => x.MemberId == u.Id),
                    NumberIdentity = (u.NumberIdentity == null ? "Đang cập nhật" : u.NumberIdentity),
                    u.PrintCount,
                    u.Address
                })
                .ToListAsync();

            return Json(new
            {
                members,
                totalUsers,
                newCode,
                totalPages = (int)Math.Ceiling((double)totalUsers / limit), // Tổng số trang
                currentPage = page
            });
        }

        [HttpPost]
        public async Task<IActionResult> Savechange(int id,string code,string name,string ortherName,string phone,string gender,string ortherPhone,DateTime birthDay, IFormFile image)
        {
            try
            {
                string imageUrl = null;
                if (image != null && image.Length > 0)
                {
                    Console.WriteLine($"Size before saving: {image.Length / 1024} KB");
                    var allowedExtensions = new[] { ".jpg", ".jpeg", ".png" };
                    var fileExtension = Path.GetExtension(image.FileName).ToLower();
                    if (!allowedExtensions.Contains(fileExtension))
                    {
                        return Json(new { success = false, message = "Chỉ chấp nhận file ảnh .jpg, .jpeg, .png!" });
                    }

                    var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads");
                    if (!Directory.Exists(uploadsFolder))
                    {
                        Directory.CreateDirectory(uploadsFolder);
                    }
                    var uniqueFileName = Guid.NewGuid().ToString() + ".jpg";
                    var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await image.CopyToAsync(stream);
                    }
                    imageUrl = "/uploads/" + uniqueFileName;
                }


                if (id == 0)
                {
                    var member = new Member
                    {
                        Code = code,
                        Name = name.Trim(),
                        OrtherName = ortherName.Trim(),
                        Phone = phone,
                        OrtherPhone = ortherPhone,
                        Gender = gender,
                        BirthDay = birthDay,
                        DateCreate = DateTime.Now,
                        UserId = _checkUser.GetUserId(), // 🔥 Lấy ID user từ session hoặc token
                        ImageIdentity = imageUrl,
                        PrintCount = 0,
                        Enable= true
                    };
                    _context.Members.Add(member);
                }
                else
                {
                    var member = await _context.Members.FindAsync(id);
                    if (member == null)
                        return NotFound();

                    member.Name = name;
                    member.OrtherName = ortherName;
                    member.Phone = phone;
                    member.Gender = gender;
                    member.OrtherPhone = ortherPhone;
                    member.BirthDay = birthDay;
                    member.DateUpdate = DateTime.Now;
                    member.UpdateBy = _checkUser.GetUserId();
                    if (imageUrl != null)
                    {
                        member.ImageIdentity = imageUrl;
                    }
                }
                await _context.SaveChangesAsync();
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = $"Lỗi: {ex.Message}" });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            if (!_checkUser.CheckAdmin())
            {
                return Json(new { success = false, message = "Bạn chưa được cấp quyền xóa thành viên!" });
            }
            else
            {
                var member = await _context.Members.FindAsync(id);
                if (member == null)
                    return Json(new { success = false, message = "Không tìm thấy thành viên!" });

                if (_context.JoinCourses.Any(x => x.MemberId == id))
                {
                    member.Enable = false;
                    _context.Entry(member);
                }
                else
                {
                    _context.Members.Remove(member);
                }

                await _context.SaveChangesAsync();
                return Json(new { success = true, message = "Đã xóa khóa tu thành công!" });
            }
        }
        [HttpGet]
        public async Task<IActionResult> GetMemberCard(int id)
        {
            var member = await _context.Members.FindAsync(id);
            if (member == null)
            {
                return Json(new { success = false, message = "Thành viên không tồn tại!" });
            }

            // 🔹 Lấy 2 chữ cuối của tên
            var nameParts = member.Name.Split(' ');
            string lastTwoWords = nameParts.Length > 1 ? nameParts[^2] + " " + nameParts[^1] : nameParts[0];

            // 🔹 Tạo QR Code từ member.Code
            //string qrCodeBase64 = GenerateQRCode(member.Code);

            return Json(new
            {
                success = true,
                id = member.Id,
                name = lastTwoWords, // Chỉ hiển thị 2 chữ cuối
                ortherName = member.OrtherName, // Chỉ hiển thị 2 chữ cuối
                code = member.Code,
                year = member.BirthDay?.Year,
                phone = member.Phone,
                image = string.IsNullOrEmpty(member.ImageIdentity) ? "/images/default-avatar.png" : member.ImageIdentity,
                //qrCode = $"data:image/png;base64,{qrCodeBase64}" // QR Code dưới dạng Base64
            });
        }

        // 🔹 Hàm tạo QR Code
        //private string GenerateQRCode(string text)
        //{
        //    using (QRCodeGenerator qrGenerator = new QRCodeGenerator())
        //    {
        //        using (QRCodeData qrCodeData = qrGenerator.CreateQrCode(text, QRCodeGenerator.ECCLevel.Q))
        //        {
        //            using (QRCode qrCode = new QRCode(qrCodeData))
        //            {
        //                using (Bitmap qrBitmap = qrCode.GetGraphic(10))
        //                {
        //                    using (MemoryStream ms = new MemoryStream())
        //                    {
        //                        qrBitmap.Save(ms, ImageFormat.Png);
        //                        return Convert.ToBase64String(ms.ToArray());
        //                    }
        //                }
        //            }
        //        }
        //    }
        //}



        [HttpPost]
        public async Task<IActionResult> Restore(int id)
        {
            if (!_checkUser.CheckAdmin())
            {
                return Json(new { success = false, message = "Bạn chưa được cấp quyền xóa thành viên!" });
            }
            else
            {
                var member = await _context.Members.FindAsync(id);
                if (member == null)
                    return Json(new { success = false, message = "Không tìm thấy thành viên!" });

                member.Enable =true;
                _context.Entry(member);
                await _context.SaveChangesAsync();
                return Json(new { success = true, message = "Đã xóa khóa tu thành công!" });
            }
        }
        [HttpPost]
        public async Task<IActionResult> ImportMembers(IFormFile file)
        {
            if (!_checkUser.CheckAdmin())
            {
                return Json(new { success = false, message = "Bạn chưa được cấp quyền xóa khóa tu!" });
            }
            else
            {
                if (file == null || file.Length == 0)
                {
                    return Json(new { success = false, message = "Vui lòng chọn file Excel!" });
                }

                var members = new List<Member>();

                try
                {
                    using (var stream = new MemoryStream())
                    {
                        await file.CopyToAsync(stream);

                        // 🔥 Thiết lập LicenseContext để tránh lỗi
                        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                        using (var package = new ExcelPackage(stream))
                        {
                            ExcelWorksheet worksheet = package.Workbook.Worksheets[0]; // Lấy sheet đầu tiên
                            int rowCount = worksheet.Dimension.Rows; // Lấy số dòng

                            for (int row = 2; row <= rowCount; row++) // Bắt đầu từ dòng 2 để bỏ qua tiêu đề
                            {
                                string code = worksheet.Cells[row, 1].Value?.ToString()?.Trim();
                                string name = worksheet.Cells[row, 2].Value?.ToString()?.Trim();
                                string ortherName = worksheet.Cells[row, 3].Value?.ToString()?.Trim();
                                string phone = worksheet.Cells[row, 4].Value?.ToString()?.Trim();
                                string ortherPhone = worksheet.Cells[row, 5].Value?.ToString()?.Trim();
                                string birthDayStr = worksheet.Cells[row, 6].Value?.ToString()?.Trim();
                                string CCCDStr = worksheet.Cells[row, 7].Value?.ToString()?.Trim();
                                DateTime? birthDay = null;

                                if (DateTime.TryParse(birthDayStr, out DateTime parsedDate))
                                {
                                    birthDay = parsedDate;
                                }

                                if (!string.IsNullOrEmpty(name) && !string.IsNullOrEmpty(code))
                                {
                                    members.Add(new Member
                                    {
                                        Name = name,
                                        Code = code,
                                        OrtherName = ortherName,
                                        Phone = phone,
                                        OrtherPhone = ortherPhone,
                                        BirthDay = birthDay,
                                        DateCreate = DateTime.Now,
                                        UserId = _checkUser.GetUserId(),
                                        NumberIdentity = CCCDStr,
                                        PrintCount = 0,Enable=true
                                    });
                                }
                            }
                        }
                    }

                    if (members.Count > 0)
                    {
                        _context.Members.AddRange(members);
                        await _context.SaveChangesAsync();
                        return Json(new { success = true, message = $"Đã nhập {members.Count} thành viên thành công!" });
                    }
                    else
                    {
                        return Json(new { success = false, message = "Không có dữ liệu hợp lệ trong file Excel!" });
                    }
                }
                catch (Exception ex)
                {
                    return Json(new { success = false, message = $"Lỗi: {ex.Message}" });
                }
            }
        }
        //End Import
    
    
        
    }
}
