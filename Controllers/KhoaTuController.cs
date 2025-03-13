using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuanLyKhoaTu.Helper;
using QuanLyKhoaTu.Models;

namespace QuanLyKhoaTu.Controllers
{
    public class KhoaTuController : Controller
    {
        private readonly ModelDbContext _context;
        private readonly CheckUser _checkUser;
        public KhoaTuController(ModelDbContext context, CheckUser checkUser)
        {
            _context = context;
            _checkUser = checkUser;
        }

        [Route("khoa-tu")]
        public IActionResult Index()
        {
            return View();
        }
        [Route("dang-ky")]
        public IActionResult Registration()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetLists(string searchText = "", int page = 1, int limit = 50)
        {
            var query = _context.ZenCourses.AsQueryable(); // Đảm bảo là IQueryable để LINQ hoạt động đúng

            // 1️⃣ Lọc theo Username hoặc Name nếu có searchText
            if (!string.IsNullOrEmpty(searchText))
            {
                query = query.Where(u => u.Name.Contains(searchText));
            }

            // 2️⃣ Đếm tổng số Users sau khi lọc
            int totalZens = await query.CountAsync();

            // 3️⃣ Phân trang chính xác với OFFSET
            var zens = await query
                .OrderByDescending(u => u.Id)
                .Skip((page - 1) * limit) // Phân trang chính xác
                .Take(limit)
                .Select(u => new
                {
                    u.Id,
                    u.Name,
                    from = u.Fromdate.Value.ToString("dd/MM/yyyy"),
                    to = u.Todate.Value.ToString("dd/MM/yyyy"),
                    Fromdate = u.Fromdate.Value.ToString("yyyy-MM-dd"),
                    Todate = u.Todate.Value.ToString("yyyy-MM-dd"),
                    Status = u.Todate >DateTime.Now ? "<span class='text-success'><i class='ti ti-point'></i> Đang diễn ra</span>" : "<span class='text-danger'><i class='ti ti-point'></i> Đã kết thúc</span>",
                    u.DateCreate,
                    CountJoin =  _context.JoinCourses.Where(x=>x.CourseId ==u.Id).Count()
                })
                .ToListAsync();

            return Json(new
            {
                zens,
                totalZens,
                totalPages = (int)Math.Ceiling((double)totalZens / limit), // Tổng số trang
                currentPage = page
            });
        }

        [HttpPost]
        public async Task<IActionResult> Savechange(int id, string name, DateTime fromdate, DateTime todate)
        {
            MD5Hash _md5 = new MD5Hash();
            if (id == 0)
            {
                var zenCourse = new ZenCourse
                {
                    Name = name,
                    Fromdate = fromdate,
                    Todate = todate,
                    UserId = _checkUser.GetUserId(),
                    DateCreate = DateTime.Now
                };
                _context.ZenCourses.Add(zenCourse);
            }
            else
            {
                var user = await _context.ZenCourses.FindAsync(id);
                if (user == null)
                    return NotFound();

                user.Name = name;
                user.Fromdate = fromdate;
                user.Todate = todate;
            }

            await _context.SaveChangesAsync();
            return Json(new { success = true });
        }
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            if (!_checkUser.CheckAdmin())
            {
                return Json(new { success = false, message = "Bạn chưa được cấp quyền xóa khóa tu!" });
            }
            else
            {
                var course = await _context.ZenCourses.FindAsync(id);
                if (course == null)
                    return Json(new { success = false, message = "Không tìm thấy khóa tu!" });

                _context.ZenCourses.Remove(course);
                await _context.SaveChangesAsync();
                return Json(new { success = true, message = "Đã xóa khóa tu thành công!" });
            }
        }
        [HttpGet]
        public async Task<IActionResult> GetListKhoaTu()
        {
            return Json(new { zens = _context.ZenCourses.Where(x=>x.Todate>DateTime.Now).ToListAsync() });
        }
        [HttpGet]
        public async Task<IActionResult> GetListMember(int courseId)
        {
            var members = await _context.Members
                        .Where(m => !_context.JoinCourses
                            .Any(j => j.CourseId == courseId && j.MemberId == m.Id && j.StatusJoin == true))
                        .Select(m => new
                        {
                            m.Id,
                            m.Code,
                            m.Name,
                            m.OrtherName,
                            text = $"{m.Code} - {m.Name} ({m.OrtherName})"
                        })
                        .ToListAsync();

            return Json(new { members });
        }
        [HttpPost]
        public async Task<IActionResult> submitRegistrationOld(int memberId, int courseId, int bedId, bool receivePhone, bool receiveCCCD)
        {
            var regis = _context.JoinCourses.FirstOrDefault(x => x.MemberId == memberId && x.CourseId == courseId);
            if (regis == null)
            {
                regis = new JoinCourse()
                {
                    MemberId = memberId,
                    CourseId = courseId,
                    BedId = bedId,
                    DateCreate = DateTime.Now,
                    UserId = _checkUser.GetUserId(),
                    StatusJoin =true,
                    Fromdate = DateTime.Today,
                    ReceiveCCCD = receiveCCCD,
                    ReceivePhone = receivePhone
                };
            _context.JoinCourses.Add(regis);
            }
            else
            {
                regis.BedId = bedId;
                regis.UpdateBy = _checkUser.GetUserId();
                regis.DateUpdate = DateTime.Now;
                regis.ReceiveCCCD = receiveCCCD;
                regis.ReceivePhone = receivePhone;
                regis.StatusJoin = true;
                regis.Todate = null;
                _context.Entry(regis);
            }
            await _context.SaveChangesAsync();
            return Json(new { success = true });
        }

        [HttpGet]
        public async Task<IActionResult> GetnewCode()
        {
            var existingCodes = (await _context.Members
                            .Where(u => u.Enable==true && u.Code.StartsWith("VD"))
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
            return Json(new { newCode });
        }
        [HttpPost]
        public async Task<IActionResult> SubmitDKNew(int bedId,int courseId,bool receivePhone,bool receiveCCCD, int id, string code, string name,string gender, string ortherName, string phone, string ortherPhone, DateTime birthDay, IFormFile image)
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

                var member = new Member
                {
                    Code = code,
                    Name = name.Trim(),
                    OrtherName = ortherName.Trim(),
                    Gender = gender,
                    Phone = phone,
                    OrtherPhone = ortherPhone,
                    BirthDay = birthDay,
                    DateCreate = DateTime.Now,
                    UserId = _checkUser.GetUserId(), // 🔥 Lấy ID user từ session hoặc token
                    ImageIdentity = imageUrl,
                    PrintCount = 0,
                    Enable=true
                };
                _context.Members.Add(member);
                await _context.SaveChangesAsync(); // 🔥 Lưu để lấy `member.Id`

                var reg = new JoinCourse()
                {
                    MemberId = member.Id,
                    CourseId = courseId,
                    BedId = bedId,
                    DateCreate = DateTime.Now,
                    UserId = _checkUser.GetUserId(),
                    StatusJoin = true,
                    ReceiveCCCD = receiveCCCD,
                    ReceivePhone = receivePhone,
                    Fromdate =DateTime.Today
                };
                _context.JoinCourses.Add(reg);
                await _context.SaveChangesAsync(); // 🔥 Lưu tiếp JoinCourse
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = $"Lỗi: {ex.Message}" });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetListBed_Change(int areaId, int courseId)
        {
            var beds = await _context.Beds
                    .Where(b => b.AreaId == areaId && b.IsAvailable == true)
                    .GroupJoin(
                        _context.JoinCourses.Where(jc => jc.CourseId == courseId && jc.StatusJoin == true),
                        b => b.Id, // Khóa chính của `Beds`
                        jc => jc.BedId, // Khóa ngoại của `JoinCourses`
                        (b, jc) => new { Bed = b, JoinCourse = jc }
                    )
                    .Where(x => !x.JoinCourse.Any()) // Lọc giường chưa có đăng ký
                    .Select(x => new
                    {
                        id = x.Bed.Id,
                        name = x.Bed.Name
                    })
                    .ToListAsync();

            return Json(new { beds });
        }
        [HttpGet]
        public async Task<IActionResult> GetBedMatrix(int areaId, int courseId)
        {
            var beds = await _context.Beds
                .Where(b => b.AreaId == areaId)
                .OrderBy(b => b.RowNumber)
                .ThenBy(b => b.BedNumber)
                .Select(b => new
                {
                    b.Id,
                    b.RowNumber,
                    b.BedNumber,
                    b.Name,
                    b.IsAvailable,
                    b.AreaId
                })
                .ToListAsync();

            // 🔍 Lấy danh sách giường đã được đăng ký trong khóa tu
            var occupiedBeds = await _context.JoinCourses
                .Where(j => j.CourseId == courseId && j.StatusJoin == true && j.BedId != null)
                .Select(j => new { j.BedId, j.MemberId, MemberName = j.Member.Name,OrtherName = j.Member.OrtherName, Guidienthoai = j.ReceivePhone,Guicccd = j.ReceiveCCCD })
                .ToListAsync();

            // Duyệt qua danh sách giường để kiểm tra trạng thái
            var bedMatrix = beds
                .GroupBy(b => b.RowNumber)
                .Select(g => new
                {
                    RowNumber = g.Key,
                    Beds = g.Select(b => new
                    {
                        b.Id,
                        b.RowNumber,
                        b.BedNumber,
                        b.Name,
                        b.IsAvailable,
                        b.AreaId,
                        Available = occupiedBeds.Any(o => o.BedId == b.Id), // Kiểm tra giường có người không
                        OccupiedBy = occupiedBeds.Where(o => o.BedId == b.Id).Select(o => o.MemberId).FirstOrDefault(),
                        MemberName = occupiedBeds.Where(o=>o.BedId==b.Id).Select(o=>o.MemberName).FirstOrDefault(),
                        MemberOrtherName = occupiedBeds.Where(o=>o.BedId==b.Id).Select(o=>o.OrtherName).FirstOrDefault(),
                        guidienthoai = occupiedBeds.Where(o=>o.BedId==b.Id).Select(o=>o.Guidienthoai).FirstOrDefault(),
                        guicccd = occupiedBeds.Where(o=>o.BedId==b.Id).Select(o=>o.Guicccd).FirstOrDefault()
                    }).ToList()
                })
                .ToList();

            return Json(new { success = true, data = bedMatrix,chodaconguoi = occupiedBeds });
        }

        [HttpPost]
        public async Task<IActionResult> submitDive(int memberId, int courseId)
        {
            var regis = _context.JoinCourses.FirstOrDefault(x => x.MemberId == memberId && x.CourseId == courseId);
            if (regis == null)
            {
                return Json(new { success = false,message="Chưa thấy thông tin đăng ký của thành viên!" });
            }
            regis.UpdateBy = _checkUser.GetUserId();
            regis.DateUpdate = DateTime.Now;
            regis.StatusJoin = false;
            regis.Todate = DateTime.Now;
            _context.Entry(regis);
            await _context.SaveChangesAsync();
            return Json(new { success = true });
        }

        [HttpPost]
        public async Task<IActionResult> submitchangeBed(int memberId, int courseId,int bedId)
        {
            var regis = _context.JoinCourses.FirstOrDefault(x => x.MemberId == memberId && x.CourseId == courseId);
            if (regis == null)
            {
                return Json(new { success = false, message = "Chưa thấy thông tin đăng ký của thành viên!" });
            }
            regis.UpdateBy = _checkUser.GetUserId();
            regis.DateUpdate = DateTime.Now;
            regis.StatusJoin = true;
            regis.BedId = bedId;

            _context.Entry(regis);
            await _context.SaveChangesAsync();
            return Json(new { success = true });
        }
    }
}
