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
    public class GiaDinhsController : Controller
    {
        private ModelDbContext db = new ModelDbContext();

        // GET: Admin/GiaDinhs
        public ActionResult Index()
        {
            var giaDinhs = db.GiaDinhs.Include(g => g.KhoaTu);
            return View(giaDinhs.ToList());
        }

        // GET: Admin/GiaDinhs/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GiaDinh giaDinh = db.GiaDinhs.Find(id);
            if (giaDinh == null)
            {
                return HttpNotFound();
            }
            return View(giaDinh);
        }

        // GET: Admin/GiaDinhs/Create
        public ActionResult Create()
        {
            ViewBag.IdKhoaTu = new SelectList(db.KhoaTus, "id", "Ten");
            return View();
        }

        // POST: Admin/GiaDinhs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,Ten,IdKhoaTu")] GiaDinh giaDinh)
        {
            if (ModelState.IsValid)
            {
                db.GiaDinhs.Add(giaDinh);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.IdKhoaTu = new SelectList(db.KhoaTus, "id", "Ten", giaDinh.IdKhoaTu);
            return View(giaDinh);
        }

        // GET: Admin/GiaDinhs/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GiaDinh giaDinh = db.GiaDinhs.Find(id);
            if (giaDinh == null)
            {
                return HttpNotFound();
            }
            ViewBag.IdKhoaTu = new SelectList(db.KhoaTus, "id", "Ten", giaDinh.IdKhoaTu);
            return View(giaDinh);
        }

        // POST: Admin/GiaDinhs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,Ten,IdKhoaTu")] GiaDinh giaDinh)
        {
            if (ModelState.IsValid)
            {
                db.Entry(giaDinh).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.IdKhoaTu = new SelectList(db.KhoaTus, "id", "Ten", giaDinh.IdKhoaTu);
            return View(giaDinh);
        }

        // GET: Admin/GiaDinhs/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GiaDinh giaDinh = db.GiaDinhs.Find(id);
            if (giaDinh == null)
            {
                return HttpNotFound();
            }
            return View(giaDinh);
        }

        // POST: Admin/GiaDinhs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            GiaDinh giaDinh = db.GiaDinhs.Find(id);
            db.GiaDinhs.Remove(giaDinh);
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
