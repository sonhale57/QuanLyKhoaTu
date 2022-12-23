using QuanLyKhoaTu.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Data.Linq.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Xml.Linq;

namespace QuanLyKhoaTu.Controllers
{
    public class Dkkhoatu
    {
        public int id { get; set; }
        public string Ten { get; set; }
        public string Diadiem { get; set; }
        public string Poster { get; set; }
        public DateTime? From { get; set; }
        public DateTime? Todate { get; set; }
        public int? Trangthai { get; set; }
    }
    public class HomeController : Controller
    {
        private ModelDbContext db = new ModelDbContext();

        public ActionResult Index()
        {
            var linq = from kt in db.KhoaTus
                       where kt.Active == true
                       select kt;
            
            ViewBag.CountKT = linq.Count();
            if(linq.Count() > 0)
            {
                ViewBag.IdKhoaTu = new SelectList(linq.ToList(), "id", "Ten");
                ViewBag.Ngaybatdau = linq.First().Ngaybatdau.Value.AddDays(1).ToString("dd-MM-yyyy");
                ViewBag.Ngayketthuc = linq.First().Ngayketthuc.Value.ToString("dd-MM-yyyy");
                ViewBag.Diadiem = linq.First().DiaDiem;
                ViewBag.TenKhoaTu = linq.First().Ten;
                int chiphi = Convert.ToInt32(linq.First().Chiphi);
                if (chiphi > 0)
                {
                    ViewBag.Chiphi = chiphi + " đ";
                }
                else
                {
                    ViewBag.Chiphi = "Miễn phí";
                }
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
        public ActionResult Dangky(string Hoten,string Phapdanh,DateTime Namsinh,string Email,string SDT,string SDT_Nguoithan,string CMND,string Gioitinh,string DiaChi,int IdKhoaTu,string Dichuyen,string Sizeao,string Ghichu,string exampleRadios, string DiChung,string LinkFB)
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
                            LinkFB = LinkFB,
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
                    Checkin=false,
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
                return Redirect("/Home/Review/?id=" + idTS+"&idKT="+IdKhoaTu);
            }
            else
            {
                return Redirect("/Home/Review/?id=" + idTS + "&idKT=" + IdKhoaTu);
            }
        }

        public ActionResult Review(int? id,int idKT)
        {
            var tuSinh = db.TuSinhs.Find(id);
            var list = from k in db.KhoaTus
                       join dk in db.DangKyKhoaTus on k.id equals dk.IdKhoaTu
                       where dk.IdTuSinh == id
                       select k;
            ViewBag.data = list.ToList();
            var kt = db.KhoaTus.Find(idKT);
            ViewBag.TenKhoaTu = kt.Ten;
            ViewBag.Ngaybatdau = kt.Ngaybatdau.Value.ToString("dd-MM-yyyy hh:mm tt");
            ViewBag.DiaDiem = kt.DiaDiem;
            ViewBag.Poster = kt.Poster;

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
            if(SName=="" || SPhone == "" || SEmail == "")
            {
                return View("Error404");
            }
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
                           select new Dkkhoatu
                           {
                               id =k.id,
                               Ten =k.Ten,
                               Diadiem  =k.DiaDiem,
                                 From = k.Ngaybatdau,
                               Todate = k.Ngayketthuc,
                               Trangthai = dk.Trangthai
                           };

                ViewBag.data = list.ToList();
            }
            return View();
        }
        public ActionResult Error404()
        {
            return View();
        }
        public ActionResult UpdateInfo()
        {
            return View();
        }
        [HttpPost]
        public ActionResult UpdateInfo(int STT,string Hoten,string Phapdanh,string SDT,string SDTKhac,string DiaChi,string CMND,string Email)
        {
            var tuSinh = db.TuSinhs.Find(STT);
            tuSinh.Hoten = Hoten;
            tuSinh.Phapdanh = Phapdanh;
            tuSinh.CMND = CMND;
            tuSinh.SDT = SDT;
            tuSinh.SDT_nguoithan = SDTKhac;
            tuSinh.DiaChi = DiaChi;
            tuSinh.Email = Email;
            db.Entry(tuSinh).State = EntityState.Modified;
            db.SaveChanges();

            var linq = db.TuSinhs.Find(STT);
            ViewBag.idTS = linq.id;
            ViewBag.Hoten = linq.Hoten;
            ViewBag.Phapdanh = linq.Phapdanh;
            ViewBag.CMND = linq.CMND;
            ViewBag.Email = linq.Email;
            ViewBag.SDT = linq.SDT;
            ViewBag.DiaChi = linq.DiaChi;
            ViewBag.SDTKhac = linq.SDT_nguoithan;
            var list = from k in db.KhoaTus
                       join dk in db.DangKyKhoaTus on k.id equals dk.IdKhoaTu
                       where dk.IdTuSinh == STT
                       select new Dkkhoatu
                       {
                           id = k.id,
                           Ten = k.Ten,
                           Diadiem = k.DiaDiem,
                           From = k.Ngaybatdau,
                           Todate = k.Ngayketthuc,
                           Trangthai = dk.Trangthai
                       };
            ViewBag.data = list.ToList();
            return View("TraCuu");
        }
        public ActionResult ConfirmJoin()
        {
            return View();
        }
        [HttpPost]
        public ActionResult ConfirmJoin(int idTS, int idKT)
        {
            var dangky = db.DangKyKhoaTus.SingleOrDefault(m=>m.IdTuSinh ==idTS && m.IdKhoaTu==idKT);
            if(dangky == null )
            {
                return View("Error404");
            }
            else
            {
                dangky.Trangthai = 1;
                db.Entry(dangky).State = EntityState.Modified;
                db.SaveChanges();

                var linq = db.TuSinhs.Find(idTS);
                    ViewBag.idTS = linq.id;
                    ViewBag.Hoten = linq.Hoten;
                    ViewBag.Phapdanh = linq.Phapdanh;
                    ViewBag.CMND = linq.CMND;
                    ViewBag.Email = linq.Email;
                    ViewBag.SDT = linq.SDT;
                    ViewBag.DiaChi = linq.DiaChi;
                    ViewBag.SDTKhac = linq.SDT_nguoithan;
                    var list = from k in db.KhoaTus
                               join dk in db.DangKyKhoaTus on k.id equals dk.IdKhoaTu
                               where dk.IdTuSinh == idTS
                               select new Dkkhoatu
                               {
                                   id = k.id,
                                   Ten = k.Ten,
                                   Diadiem = k.DiaDiem,
                                   From = k.Ngaybatdau,
                                   Todate = k.Ngayketthuc,
                                   Trangthai = dk.Trangthai
                               };
                ViewBag.data = list.ToList();
                return View("TraCuu");
            }
        }

        public ActionResult ConfirmThamGia()
        {
            return View();
        }
        [HttpPost]
        public ActionResult ConfirmThamGia(int idTS, int status,string Dichuyen)
        {
            var linqKT = db.KhoaTus.OrderByDescending(m => m.id).ToList();
            int idKT = linqKT.First().id;
            var linq = db.DangKyKhoaTus.SingleOrDefault(m=>m.IdKhoaTu== idKT && m.IdTuSinh == idTS);
            if(linq == null)
            {
                return View("Error404");
            }
            else
            {
                linq.Trangthai = status;
                linq.DiChuyen= Dichuyen;
                db.Entry(linq).State = EntityState.Modified;
                db.SaveChanges();
                ViewBag.close = "<script>closeModal();</script>";
            }
            return View("Index");
        }

        public ActionResult CancelJoin()
        {
            return View();
        }
        [HttpPost]
        public ActionResult CancelJoin(int idTS, int idKT)
        {
            var dangky = db.DangKyKhoaTus.SingleOrDefault(m => m.IdTuSinh == idTS && m.IdKhoaTu == idKT);
            if (dangky == null)
            {
                return View("Error404");
            }
            else
            {
                dangky.Trangthai = 2;
                db.Entry(dangky).State = EntityState.Modified;
                db.SaveChanges();

                var linq = db.TuSinhs.Find(idTS);
                ViewBag.idTS = linq.id;
                ViewBag.Hoten = linq.Hoten;
                ViewBag.Phapdanh = linq.Phapdanh;
                ViewBag.CMND = linq.CMND;
                ViewBag.Email = linq.Email;
                ViewBag.SDT = linq.SDT;
                ViewBag.DiaChi = linq.DiaChi;
                ViewBag.SDTKhac = linq.SDT_nguoithan;
                var list = from k in db.KhoaTus
                           join dk in db.DangKyKhoaTus on k.id equals dk.IdKhoaTu
                           where dk.IdTuSinh == idTS
                           select new Dkkhoatu
                           {
                               id = k.id,
                               Ten = k.Ten,
                               Diadiem = k.DiaDiem,
                               From = k.Ngaybatdau,
                               Todate = k.Ngayketthuc,
                               Trangthai = dk.Trangthai
                           };
                ViewBag.data = list.ToList();
                return View("TraCuu");
            }
        }
    }
}