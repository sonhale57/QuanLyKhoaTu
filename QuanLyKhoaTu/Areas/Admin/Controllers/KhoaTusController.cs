using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Core.Common.CommandTrees;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using QuanLyKhoaTu.Models;

namespace QuanLyKhoaTu.Areas.Admin.Controllers
{
    public class KhoaTusController : Controller
    {
        private ModelDbContext db = new ModelDbContext();

        // GET: Admin/KhoaTus
        public ActionResult Index()
        {
            var khoaTus = db.KhoaTus.Include(k => k.LoaiKhoaTu);
            var loaiKhoaTus = db.LoaiKhoaTus.ToList() ;
            ViewBag.IdLoaiKhoaTu = new SelectList(db.LoaiKhoaTus, "id", "Ten");

            dynamic mymodel = new ExpandoObject();

            mymodel.LoaiKhoaTu = loaiKhoaTus;
            mymodel.KhoaTu = khoaTus;

            return View(mymodel);
        }
        public ActionResult DanhSachDangKy()
        {
            ViewBag.IdKhoaTu = new SelectList(db.KhoaTus.OrderByDescending(x=>x.id), "id", "Ten");
            return View();
        }
        public JsonResult GetDanhSach(int? id)
        {

            var linq = from k in db.KhoaTus
                       join dk in db.DangKyKhoaTus on k.id equals dk.IdKhoaTu
                       join ts in db.TuSinhs on dk.IdTuSinh equals ts.id
                       where k.id == id
                       select new
                       {
                           Ten = k.Ten,
                           Hoten = ts.Hoten,
                           Namsinh = ts.Namsinh,
                           Gioitinh = ts.Gioitinh,
                           Phapdanh = ts.Phapdanh,
                           SDT = ts.SDT,
                           Email = ts.Email,
                           CMND = ts.CMND,
                           NgayGhiDanh = dk.NgayGhiDanh,
                           Thoigian =dk.NgayGhiDanh.ToString(),
                           DiChuyen = dk.DiChuyen,
                           StatusCheckin = dk.Checkin,
                           DiChung = dk.DiCung
                       };

            return Json(linq.ToList(), JsonRequestBehavior.AllowGet);
        }

        // GET: Admin/KhoaTus/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            KhoaTu khoaTu = db.KhoaTus.Find(id);
            if (khoaTu == null)
            {
                return HttpNotFound();
            }
            return View(khoaTu);
        }

        // GET: Admin/KhoaTus/Create
        public ActionResult Create()
        {
            ViewBag.IdLoaiKhoaTu = new SelectList(db.LoaiKhoaTus, "id", "Ten");
            return View();
        }

        // POST: Admin/KhoaTus/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public ActionResult Create([Bind(Include = "id,Ten,Ngaybatdau,Ngayketthuc,IdLoaiKhoaTu,Chiphi,DiaDiem,Active")] KhoaTu khoaTu)
        {
            if (ModelState.IsValid)
            {
                khoaTu.Active = true;
                db.KhoaTus.Add(khoaTu);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.IdLoaiKhoaTu = new SelectList(db.LoaiKhoaTus, "id", "Ten", khoaTu.IdLoaiKhoaTu);
            return View(khoaTu);
        }

        // GET: Admin/KhoaTus/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            KhoaTu khoaTu = db.KhoaTus.Find(id);
            if (khoaTu == null)
            {
                return HttpNotFound();
            }
            ViewBag.IdLoaiKhoaTu = new SelectList(db.LoaiKhoaTus, "id", "Ten", khoaTu.IdLoaiKhoaTu);
            return View(khoaTu);
        }

        // POST: Admin/KhoaTus/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,Ten,Ngaybatdau,Ngayketthuc,IdLoaiKhoaTu,Chiphi,DiaDiem")] KhoaTu khoaTu)
        {
            if (ModelState.IsValid)
            {
                db.Entry(khoaTu).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.IdLoaiKhoaTu = new SelectList(db.LoaiKhoaTus, "id", "Ten", khoaTu.IdLoaiKhoaTu);
            return View(khoaTu);
        }

        // GET: Admin/KhoaTus/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            KhoaTu khoaTu = db.KhoaTus.Find(id);
            if (khoaTu == null)
            {
                return HttpNotFound();
            }
            return View(khoaTu);
        }

        // POST: Admin/KhoaTus/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            KhoaTu khoaTu = db.KhoaTus.Find(id);
            db.KhoaTus.Remove(khoaTu);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
