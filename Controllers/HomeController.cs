using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuanLyKhoaTu.Helper;
using QuanLyKhoaTu.Models;
namespace QuanLyKhoaTu.Controllers
{
    public class HomeController : Controller
    {
        private readonly ModelDbContext _context;
        private readonly CheckUser _checkUser;
        public HomeController(ModelDbContext context, CheckUser checkUser)
        {
            _context = context;
            _checkUser = checkUser;
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Index()
        {
            return View();
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        [Route("dang-nhap")]
        public IActionResult Login()
        {
            return View();
        }
        public IActionResult Privacy()
        {
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> Onload()
        {
            int courseId = 0;
            string courseName = "";
            int count_thamgia = 0, count_dave = 0, count_controng = 0;
            // Lấy khóa tu đang diễn ra nếu có
            var activeCourse = _context.ZenCourses
                                .Where(x => x.Todate > DateTime.UtcNow)
                                .OrderBy(x => x.Fromdate) // Lấy khóa tu sớm nhất còn diễn ra
                                .FirstOrDefault();

            if (activeCourse != null)
            {
                courseId = activeCourse.Id;
                courseName = activeCourse.Name;
                count_thamgia = _context.JoinCourses.Count(x => x.CourseId == courseId && x.StatusJoin == true);
                count_dave = _context.JoinCourses.Count(x => x.CourseId == courseId && x.StatusJoin == false);
            }
            else
            {
                // Nếu không có khóa nào đang diễn ra, lấy khóa gần nhất
                var latestCourse = _context.ZenCourses
                                    .OrderByDescending(x => x.Id)
                                    .FirstOrDefault();
                if (latestCourse != null)
                {
                    courseId = latestCourse.Id;
                    courseName = latestCourse.Name;
                }
            }
            var count_tongchongu = _context.Beds.Count(x => x.IsAvailable == true);
            var sogiuong_daconguoi = _context.JoinCourses.Count(j => j.CourseId == courseId && j.StatusJoin == true && j.BedId != null);
            count_controng = count_tongchongu - sogiuong_daconguoi;
            return Json(new {
                courseName,
                count_thamgia,
                count_dave,
                count_khoatu = _context.ZenCourses.Count(),
                count_tongchongu,
                count_tongthanhvien = _context.Members.Count(x=>x.Enable==true),
                count_controng,
            });
        }
        [HttpGet]
        public async Task<IActionResult> Loadlist(string searchText = "", string sort = "date")
        {
            int courseId = 0;
            string courseName = "";

            // Lấy khóa tu đang diễn ra nếu có
            var activeCourse = _context.ZenCourses
                                .Where(x => x.Todate > DateTime.UtcNow)
                                .OrderBy(x => x.Fromdate) // Lấy khóa tu sớm nhất còn diễn ra
                                .FirstOrDefault();

            if (activeCourse != null)
            {
                courseId = activeCourse.Id;
                courseName = activeCourse.Name;
            }
            else
            {
                // Nếu không có khóa nào đang diễn ra, lấy khóa gần nhất
                var latestCourse = _context.ZenCourses
                                    .OrderByDescending(x => x.Id)
                                    .FirstOrDefault();
                if (latestCourse != null)
                {
                    courseId = latestCourse.Id;
                    courseName = latestCourse.Name;
                }
            }

            var joinCourseQuery = from j in _context.JoinCourses
                                  join member in _context.Members on j.MemberId equals member.Id
                                  join bed in _context.Beds on j.BedId equals bed.Id
                                  where j.CourseId == courseId
                                  select new
                                  {
                                      Code = member.Code,
                                      Name = member.Name,
                                      OrtherName = member.OrtherName,
                                      Phone = member.Phone,
                                      OrtherPhone = member.OrtherPhone,
                                      Gender = member.Gender,
                                      Year = member.BirthDay.Value.Year,
                                      Bed = bed.Name,
                                      BedId = j.BedId,
                                      Fromdate = j.Fromdate,
                                      Todate = j.Todate,
                                      StatusJoin = j.StatusJoin,
                                      j.DateCreate,
                                      Status = j.StatusJoin==true ? "Đang tham gia" : "Đã về"
                                  };

            // Chuyển dữ liệu về List để có thể sử dụng LINQ tiếp theo
            var joinCourse = await joinCourseQuery.ToListAsync();

            // 1️⃣ Lọc dữ liệu theo searchText
            if (!string.IsNullOrEmpty(searchText))
            {
                joinCourse = joinCourse
                    .Where(x => x.Name.Contains(searchText, StringComparison.OrdinalIgnoreCase) ||
                                x.OrtherName.Contains(searchText, StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }

            // 2️⃣ Sắp xếp theo yêu cầu
            joinCourse = sort switch
            {
                "bed" => joinCourse.OrderBy(x => x.BedId).ToList(),
                "name" => joinCourse.OrderBy(x => x.Name).ToList(),
                "date" => joinCourse.OrderBy(x => x.DateCreate).ToList(),
                "date_desc" => joinCourse.OrderByDescending(x => x.DateCreate).ToList(),
                "status" => joinCourse.OrderBy(x => x.StatusJoin).ToList(),
                _ => joinCourse.OrderByDescending(x => x.DateCreate).ToList()
            };

            return Json(new { data = joinCourse, courseId, courseName });
        }
    }
}
