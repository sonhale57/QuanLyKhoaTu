using QuanLyKhoaTu.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QuanLyKhoaTu.Controllers
{
   
    public class HomeController : Controller
    {
        private ModelDbContext db = new ModelDbContext();

        public ActionResult Index()
        {
            ViewBag.IdKhoaTu = new SelectList(db.KhoaTus, "id", "Ten");
            return View();
        }
        public ActionResult Dangky()
        {
            ViewBag.IdKhoaTu = new SelectList(db.KhoaTus, "id", "Ten");
            return View();
        }

        [HttpPost]
        public ActionResult Dangky([Bind(Include = "id,Hoten,Namsinh,Phapdanh,Email,CMND,Gioitinh,SDT,SDT_nguoithan,Updatetime,DiaChi")] TuSinh tuSinh,int IdKhoaTu,string Dichuyen,string Sizeao,string Ghichu,string Muon)
        {
            ViewBag.IdKhoaTu = new SelectList(db.KhoaTus, "id", "Ten");
            if (ModelState.IsValid)
            {
                db.TuSinhs.Add(tuSinh);
                db.SaveChanges();

                var dangky = new DangKyKhoaTu() { 
                    IdKhoaTu = IdKhoaTu,
                    IdTuSinh = tuSinh.id,
                    NgayGhiDanh=DateTime.Now,
                    DiChuyen = Dichuyen,
                    MuonAoTrang = Boolean.Parse(Muon)
                };
                var muonaotrang = new MuonAoTrang()
                {
                    IdKhoaTu = IdKhoaTu,
                    IdTuSinh = tuSinh.id,
                    SizeAo = Sizeao,
                    GhiChu = Ghichu
                };
                db.MuonAoTrangs.Add(muonaotrang);
                db.DangKyKhoaTus.Add(dangky);
                db.SaveChanges();
                return Redirect("/Home/Review/"+tuSinh.id);
            }
            return View();
        }
        public ActionResult Review(int? id)
        {
            var tuSinh = db.TuSinhs.Find(id);
            return View(tuSinh);
        }
        public ActionResult UpdateDiChung()
        {
            return View();
        }
        [HttpPost]
        public ActionResult UpdateDiChung(int IdKhoaTu,int Idedit, int IdTuSinh)
        {
            var tuSinh = db.DangKyKhoaTus.SingleOrDefault(m=>m.IdTuSinh==Idedit && m.IdKhoaTu ==IdKhoaTu);
            if (tuSinh == null)
            {
                return HttpNotFound();
            }
            else
            {
                tuSinh.DiCung = IdTuSinh;
                db.Entry(tuSinh).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tuSinh);
        }
        
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}