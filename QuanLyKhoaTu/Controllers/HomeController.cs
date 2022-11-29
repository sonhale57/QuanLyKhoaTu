using QuanLyKhoaTu.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Linq.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;

namespace QuanLyKhoaTu.Controllers
{
   
    public class HomeController : Controller
    {
        private ModelDbContext db = new ModelDbContext();

        public ActionResult Index()
        {
            var linq = from kt in db.KhoaTus
                       where kt.Active == true
                       select kt;
            ViewBag.IdKhoaTu = new SelectList(linq.ToList(), "id", "Ten");
            ViewBag.Ngaybatdau = linq.First().Ngaybatdau.Value.ToString("dd/MM/yyyy");
            ViewBag.Ngayketthuc = linq.First().Ngayketthuc.Value.ToString("dd/MM/yyyy");
            ViewBag.Diadiem = linq.First().DiaDiem;
            ViewBag.TenKhoaTu = linq.First().Ten;
            int chiphi = Convert.ToInt32(linq.First().Chiphi);
            if(chiphi> 0)
            {
                ViewBag.Chiphi = chiphi +" đ";
            }
            else
            {
                ViewBag.Chiphi = "Miễn phí";
            }
            return View();
        }
        public ActionResult Dangky()
        {
            var linq = from kt in db.KhoaTus
                       where kt.Active == true
                       select kt;
            ViewBag.IdKhoaTu = new SelectList(linq.ToList(), "id", "Ten");
            return View();
        }

        [HttpPost] 
        public ActionResult Dangky(string Hoten,string Phapdanh,DateTime Namsinh,string Email,string SDT,string SDT_Nguoithan,string CMND,string Gioitinh,string DiaChi,int IdKhoaTu,string Dichuyen,string Sizeao,string Ghichu,string exampleRadios, string DiChung)
        {
                
                int idTS;
                var linq = db.TuSinhs.SqlQuery("Select * from TuSinh where SDT ='"+SDT+"' and Email ='"+Email+"' and Hoten like N'%"+Hoten+"%'").ToList();
                var tuSinh = new TuSinh() { 
                            Hoten = Hoten,
                            Phapdanh = Phapdanh,
                            Namsinh = Namsinh,
                            Email = Email,
                            SDT = SDT,
                            SDT_nguoithan= SDT_Nguoithan,
                            CMND = CMND,
                            DiaChi= DiaChi,
                            Gioitinh= Gioitinh,
                            Updatetime= DateTime.Now
                 };
                if (linq == null || linq.Count ==0)
                {
                    db.TuSinhs.Add(tuSinh);
                    db.SaveChanges();
                     idTS = tuSinh.id;
               }
               else
               {
                    idTS = linq.First().id;   
                }
                var dangky = new DangKyKhoaTu() { 
                    IdKhoaTu = IdKhoaTu,
                    IdTuSinh = idTS,
                    NgayGhiDanh=DateTime.Now,
                    DiChuyen = Dichuyen,
                    Trangthai = 0,
                    MuonAoTrang = Boolean.Parse(exampleRadios) 
                };
                if (DiChung == "" || DiChung == null)
                {
                dangky.DiCung = null;
                }
                else
                {
                    dangky.DiCung = Convert.ToInt32(DiChung);
                }
            var linqdk = (from dk in db.DangKyKhoaTus
                            where dk.IdKhoaTu ==IdKhoaTu && dk.IdTuSinh ==idTS
                            select dk).ToList();
            if (linqdk == null || linqdk.Count == 0)
            {
                db.DangKyKhoaTus.Add(dangky);
                db.SaveChanges();
                return Redirect("/Home/Review/" + idTS);
            }
            else
            {
                return Redirect("/Home/Review/" + idTS);
            }
        }

        public ActionResult Review(int? id)
        {
            var tuSinh = db.TuSinhs.Find(id);
            return View(tuSinh);
        }
         
        public ActionResult About()
        {
            ViewBag.IdKhoaTu = new SelectList(db.KhoaTus, "id", "Ten");

            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public JsonResult CheckExists(int? idTS)
        {
            var linq = (from k in db.TuSinhs
                       where k.id == idTS
                       select k).ToList();
            return Json(linq.First().Hoten, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult TraCuu()
        {
            return View();
        }
        [HttpPost]
        public  ActionResult TraCuu(string SName,string SPhone,string SEmail) {
            
            var linq = db.TuSinhs.SqlQuery("Select * from TuSinh where SDT ='" + SPhone + "' and Email ='" + SEmail + "' and Hoten like N'%" + SName + "%'").ToList();
            if (linq == null || linq.Count == 0)
            {
                return View("Error404");
            }
            else
            {
                ViewBag.idTS = linq.First().id;
                ViewBag.Hoten = linq.First().Hoten;
                ViewBag.Phapdanh = linq.First().Phapdanh;
                ViewBag.CMND   = linq.First().CMND;
                ViewBag.Email   =linq.First().Email;
                ViewBag.SDT = linq.First().SDT;
                ViewBag.DiaChi = linq.First().DiaChi;
                ViewBag.SDTKhac = linq.First().SDT_nguoithan;
                int idTSinh = linq.First().id;
                var list = from k in db.KhoaTus
                           join dk in db.DangKyKhoaTus on k.id equals dk.IdKhoaTu
                           where dk.IdTuSinh ==idTSinh
                           select k;
                ViewBag.data = list.ToList();
            }
            return View();
        }
        public ActionResult Error404()
        {
            return View();
        }
    }
}