using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using QuanLyKhoaTu.Models;

namespace QuanLyKhoaTu.Areas.Admin.Controllers
{
    public class TuSinhsController : Controller
    {
        private ModelDbContext db = new ModelDbContext();

        // GET: Admin/TuSinhs
        public ActionResult Index()
        {
            var tuSinhs = db.TuSinhs.Include(t => t.User);
            return View(tuSinhs.ToList());
        }

        // GET: Admin/TuSinhs/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TuSinh tuSinh = db.TuSinhs.Find(id);
            if (tuSinh == null)
            {
                return HttpNotFound();
            }
            return View(tuSinh);
        }

        // GET: Admin/TuSinhs/Create
        public ActionResult Create()
        {
            ViewBag.IdUser = new SelectList(db.Users, "id", "Name");
            return View();
        }

        // POST: Admin/TuSinhs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,Hinhanh,Hoten,Namsinh,Phapdanh,Email,CMND,Gioitinh,SDT,SDT_nguoithan,IdUser,Updatetime,QRCode,DiaChi")] TuSinh tuSinh)
        {
            if (ModelState.IsValid)
            {
                db.TuSinhs.Add(tuSinh);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.IdUser = new SelectList(db.Users, "id", "Name", tuSinh.IdUser);
            return View(tuSinh);
        }

        // GET: Admin/TuSinhs/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TuSinh tuSinh = db.TuSinhs.Find(id);
            if (tuSinh == null)
            {
                return HttpNotFound();
            }
            ViewBag.IdUser = new SelectList(db.Users, "id", "Name", tuSinh.IdUser);
            return View(tuSinh);
        }

        // POST: Admin/TuSinhs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,Hinhanh,Hoten,Namsinh,Phapdanh,Email,CMND,Gioitinh,SDT,SDT_nguoithan,IdUser,Updatetime,QRCode,DiaChi")] TuSinh tuSinh)
        {
            if (ModelState.IsValid)
            {
                tuSinh.Updatetime= DateTime.Now;
                db.Entry(tuSinh).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.IdUser = new SelectList(db.Users, "id", "Name", tuSinh.IdUser);
            return View(tuSinh);
        }

        // GET: Admin/TuSinhs/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TuSinh tuSinh = db.TuSinhs.Find(id);
            if (tuSinh == null)
            {
                return HttpNotFound();
            }
            return View(tuSinh);
        }

        // POST: Admin/TuSinhs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            TuSinh tuSinh = db.TuSinhs.Find(id);
            db.TuSinhs.Remove(tuSinh);
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
