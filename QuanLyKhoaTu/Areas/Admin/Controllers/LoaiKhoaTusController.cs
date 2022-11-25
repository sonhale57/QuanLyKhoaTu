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
    public class LoaiKhoaTusController : Controller
    {
        private ModelDbContext db = new ModelDbContext();

        // GET: Admin/LoaiKhoaTus
        public ActionResult Index()
        {
            return View(db.LoaiKhoaTus.ToList());
        }

        // GET: Admin/LoaiKhoaTus/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LoaiKhoaTu loaiKhoaTu = db.LoaiKhoaTus.Find(id);
            if (loaiKhoaTu == null)
            {
                return HttpNotFound();
            }
            return View(loaiKhoaTu);
        }

        // GET: Admin/LoaiKhoaTus/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/LoaiKhoaTus/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public ActionResult Create([Bind(Include = "id,Ten,Updatetime")] LoaiKhoaTu loaiKhoaTu)
        {
            if (ModelState.IsValid)
            {
                loaiKhoaTu.Updatetime = DateTime.Now;
                db.LoaiKhoaTus.Add(loaiKhoaTu);
                db.SaveChanges();
                return Redirect("/Admin/KhoaTus");
            }

            return View(loaiKhoaTu);
        }

        // GET: Admin/LoaiKhoaTus/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LoaiKhoaTu loaiKhoaTu = db.LoaiKhoaTus.Find(id);
            if (loaiKhoaTu == null)
            {
                return HttpNotFound();
            }
            return View(loaiKhoaTu);
        }

        // POST: Admin/LoaiKhoaTus/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,Ten,Updatetime")] LoaiKhoaTu loaiKhoaTu)
        {
            if (ModelState.IsValid)
            {
                db.Entry(loaiKhoaTu).State = EntityState.Modified;
                db.SaveChanges();
                return Redirect("/Admin/KhoaTus");
            }
            return View(loaiKhoaTu);
        }

        // GET: Admin/LoaiKhoaTus/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LoaiKhoaTu loaiKhoaTu = db.LoaiKhoaTus.Find(id);
            if (loaiKhoaTu == null)
            {
                return HttpNotFound();
            }
            return View(loaiKhoaTu);
        }

        // POST: Admin/LoaiKhoaTus/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            LoaiKhoaTu loaiKhoaTu = db.LoaiKhoaTus.Find(id);
            db.LoaiKhoaTus.Remove(loaiKhoaTu);
            db.SaveChanges();
            return Redirect("/Admin/KhoaTus/Index");
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
