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
    public class GiaoThoesController : Controller
    {
        private ModelDbContext db = new ModelDbContext();

        // GET: Admin/GiaoThoes
        public ActionResult Index()
        {
            return View(db.GiaoThoes.ToList());
        }

        // GET: Admin/GiaoThoes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GiaoTho giaoTho = db.GiaoThoes.Find(id);
            if (giaoTho == null)
            {
                return HttpNotFound();
            }
            return View(giaoTho);
        }

        // GET: Admin/GiaoThoes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/GiaoThoes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,Hoten,PhapDanh,SDT,TuVien,LinkFb,Namsinh,Hinhanh")] GiaoTho giaoTho)
        {
            if (ModelState.IsValid)
            {
                db.GiaoThoes.Add(giaoTho);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(giaoTho);
        }

        // GET: Admin/GiaoThoes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GiaoTho giaoTho = db.GiaoThoes.Find(id);
            if (giaoTho == null)
            {
                return HttpNotFound();
            }
            return View(giaoTho);
        }

        // POST: Admin/GiaoThoes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,Hoten,PhapDanh,SDT,TuVien,LinkFb,Namsinh,Hinhanh")] GiaoTho giaoTho)
        {
            if (ModelState.IsValid)
            {
                db.Entry(giaoTho).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(giaoTho);
        }

        // GET: Admin/GiaoThoes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GiaoTho giaoTho = db.GiaoThoes.Find(id);
            if (giaoTho == null)
            {
                return HttpNotFound();
            }
            return View(giaoTho);
        }

        // POST: Admin/GiaoThoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            GiaoTho giaoTho = db.GiaoThoes.Find(id);
            db.GiaoThoes.Remove(giaoTho);
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
