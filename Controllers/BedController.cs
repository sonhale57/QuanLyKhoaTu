using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuanLyKhoaTu.Helper;
using QuanLyKhoaTu.Models;

namespace QuanLyKhoaTu.Controllers
{
    public class BedController : Controller
    {
        private readonly ModelDbContext _context;
        private readonly CheckUser _checkUser;
        public BedController(ModelDbContext context, CheckUser checkUser)
        {
            _context = context;
            _checkUser = checkUser;
        }
        [Route("so-do-cho-ngu")]
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> GetListArea()
        {
            return Json(new { areas = _context.Areas.ToListAsync()});
        }

        [HttpGet]
        public async Task<IActionResult> GetBedMatrix(int areaId)
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

            // Nhóm theo `RowNumber` để hiển thị dạng ma trận
            var bedMatrix = beds
                .GroupBy(b => b.RowNumber)
                .Select(g => new
                {
                    RowNumber = g.Key,
                    Beds = g.ToList()
                })
                .ToList();

            return Json(new { success = true, data = bedMatrix });
        }
        [HttpPost]
        public async Task<IActionResult> Savechange(int id, string name, int rowNumber, int bedNumber, int areaId,bool isAvailable)
        {
            try
            {
                if (id == 0)
                {
                    var bed = new Bed
                    {
                        Name = name.Trim(),
                        RowNumber = rowNumber,
                        BedNumber = bedNumber,
                        AreaId = areaId,
                        IsAvailable = isAvailable
                    };
                    _context.Beds.Add(bed);
                }
                else
                {
                    var bed = await _context.Beds.FindAsync(id);
                    if (bed == null)
                        return NotFound();

                    bed.Name = name;
                    bed.BedNumber = bedNumber;
                    bed.RowNumber = rowNumber;
                    bed.IsAvailable = isAvailable;
                    bed.AreaId = areaId;
                }
                await _context.SaveChangesAsync();
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = $"Lỗi: {ex.Message}" });
            }
        }
    }
}
