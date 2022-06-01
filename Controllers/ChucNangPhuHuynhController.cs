using QuanLyTruongMauGiao.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using System.Data.Entity;

namespace QuanLyTruongMauGiao.Controllers
{
    public class ChucNangPhuHuynhController : Controller
    {
        private QLMauGiao db = new QLMauGiao();

        // GET: ChucNangPhuHuynh
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult DangKiBuaAn()
        {
            TAIKHOAN user = (TAIKHOAN)Session["user"];
            if (Session["user"] != null && user.PhanQuyen == "Phụ huynh")
            {
                DateTime monday = DateTime.Today.AddDays(-(int)DateTime.Today.DayOfWeek + (int)DayOfWeek.Monday);
                DateTime sunday = DateTime.Today.AddDays(-(int)DateTime.Today.DayOfWeek + 7);

                var users = (from item in db.PHUHUYNHs where item.TenTK == user.TenTK select item).FirstOrDefault();
                var tre = (from item in db.TREs where item.MaPH == users.MaPH select item).FirstOrDefault();
                var listNgay = from item in db.DiemDanhVaDangKyBuaAns
                               join t in db.TREs on item.MaTre equals t.MaTre
                               where DbFunctions.TruncateTime(item.Ngay) >= DbFunctions.TruncateTime(monday)
                               && DbFunctions.TruncateTime(item.Ngay) <= DbFunctions.TruncateTime(sunday) && t.MaTre == tre.MaTre
                               select new
                               {
                                   Ngay = item.Ngay
                               };
                var listTre = from item in db.DiemDanhVaDangKyBuaAns
                              join t in db.TREs on item.MaTre equals t.MaTre
                              where DbFunctions.TruncateTime(item.Ngay) >= DbFunctions.TruncateTime(monday)
                                && DbFunctions.TruncateTime(item.Ngay) <= DbFunctions.TruncateTime(sunday) && t.MaTre == tre.MaTre
                              select item;
                List<DateTime> list = new List<DateTime>();
                foreach (var item in listNgay.ToList())
                {
                    list.Add(item.Ngay);
                }
                ViewBag.ListNgay = list.ToList();
                ViewBag.ListTre = listTre.ToList();

                return View(db.THUCDONNGAYs.Where(x => x.Ngay >= monday && x.Ngay <= sunday).ToList());
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        public class ParamDangKyBuaAn
        {
            public DateTime Ngay { get; set; }
            public bool select { get; set; }
        }
        public string SinhMaDD()
        {
            var record = (from item in db.DiemDanhVaDangKyBuaAns
                          select item).OrderByDescending(x => x.ID).FirstOrDefault();
            string ID = "";
            if (record != null)
            {
                int soID = int.Parse(record.ID.Substring(1, 4)) + 1;
                if (soID < 10)
                {
                    ID = String.Format("{0}000{1}", "D", soID);
                }
                else if (soID < 100)
                {
                    ID = String.Format("{0}00{1}", "D", soID);
                }
                else if (soID < 1000)
                {
                    ID = String.Format("{0}0{1}", "D", soID);
                }
                else if (soID < 10000)
                {
                    ID = String.Format("{0}{1}", "D", soID);
                }
                else
                {
                    ID = soID.ToString();
                }
            }
            else
            {
                ID = "D0001";
            }
            return ID;
        }
        [HttpPost]
        public ActionResult DangKiBuaAn(ParamDangKyBuaAn[] list)
        {
            TAIKHOAN user = (TAIKHOAN)Session["user"];
            if (Session["user"] != null && user.PhanQuyen == "Phụ huynh")
            {
                DateTime monday = DateTime.Today.AddDays(-(int)DateTime.Today.DayOfWeek + (int)DayOfWeek.Monday);
                DateTime sunday = DateTime.Today.AddDays(-(int)DateTime.Today.DayOfWeek + 7);

                var users = (from item in db.PHUHUYNHs where item.TenTK == user.TenTK select item).FirstOrDefault();
                var tre = (from item in db.TREs where item.MaPH == users.MaPH select item).FirstOrDefault();
                try
                {
                    if (list != null)
                    {
                        foreach (var item in list)
                        {
                            var query = (from x in db.DiemDanhVaDangKyBuaAns
                                         where x.Ngay == item.Ngay && x.MaTre == tre.MaTre
                                         select x).FirstOrDefault();
                            if (query == null)
                            {
                                DiemDanhVaDangKyBuaAn dangKi = new DiemDanhVaDangKyBuaAn();
                                dangKi.ID = SinhMaDD();
                                dangKi.MaTre = tre.MaTre;
                                dangKi.Ngay = item.Ngay;
                                dangKi.DiemDanh = false;
                                dangKi.DangKiBuaAn = item.select;
                                db.DiemDanhVaDangKyBuaAns.Add(dangKi);
                            }
                            else
                            {
                                query.DangKiBuaAn = item.select;
                            }
                            db.SaveChanges();

                        }
                        return Json("Đăng kí bữa ăn thành công", JsonRequestBehavior.AllowGet);
                    }
                }
                catch (Exception ex)
                {
                    return Json(ex.Message, JsonRequestBehavior.AllowGet);
                }
                var listNgay = from item in db.DiemDanhVaDangKyBuaAns
                               join t in db.TREs on item.MaTre equals t.MaTre
                               where DbFunctions.TruncateTime(item.Ngay) >= DbFunctions.TruncateTime(monday)
                               && DbFunctions.TruncateTime(item.Ngay) <= DbFunctions.TruncateTime(sunday) && t.MaTre == tre.MaTre
                               select new
                               {
                                   Ngay = item.Ngay
                               };
                var listTre = from item in db.DiemDanhVaDangKyBuaAns
                              join t in db.TREs on item.MaTre equals t.MaTre
                              where DbFunctions.TruncateTime(item.Ngay) >= DbFunctions.TruncateTime(monday)
                                && DbFunctions.TruncateTime(item.Ngay) <= DbFunctions.TruncateTime(sunday) && t.MaTre == tre.MaTre
                              select item;
                List<DateTime> l = new List<DateTime>();
                foreach (var item in listNgay.ToList())
                {
                    l.Add(item.Ngay);
                }
                ViewBag.ListNgay = l.ToList();
                ViewBag.ListTre = listTre.ToList();
                return View(db.THUCDONNGAYs.Where(x => x.Ngay >= monday && x.Ngay <= sunday).ToList());
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }

        }

        public ActionResult XemDanhGia()
        {
            TAIKHOAN user = (TAIKHOAN)Session["user"];
            if (Session["user"] != null && user.PhanQuyen == "Phụ huynh")
            {
                var users = (from item in db.PHUHUYNHs where item.TenTK == user.TenTK select item).FirstOrDefault();
                var tre = (from item in db.TREs where item.MaPH == users.MaPH select item).FirstOrDefault();
                string namHoc = DateTime.Now.Year.ToString();
                var phieuDG = from ketquaDG in db.KETQUADANHGIAs
                              where ketquaDG.PHIEUDANHGIA.MaTre == tre.MaTre && ketquaDG.PHIEUDANHGIA.NamHoc == namHoc && ketquaDG.PHIEUDANHGIA.NgayTao.Month == DateTime.Now.Month
                              select ketquaDG;
                var gv = (from item in db.GIAOVIENs where item.MaGV == phieuDG.FirstOrDefault().PHIEUDANHGIA.MaGV select item).FirstOrDefault();
                ViewBag.Tre = tre;
                ViewBag.GiaoVien = gv;
                return View(phieuDG.ToList());
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
        public ActionResult DongHocPhi()
        {
            TAIKHOAN user = (TAIKHOAN)Session["user"];
            if (Session["user"] != null && user.PhanQuyen == "Phụ huynh")
            {
                var users = (from item in db.PHUHUYNHs where item.TenTK == user.TenTK select item).FirstOrDefault();
                var tre = (from item in db.TREs where item.MaPH == users.MaPH select item).FirstOrDefault();
                string id = tre.MaTre;
                var pHIEUTHUTIEN = db.PHIEUTHUTIENs.Where(x => x.MaTre == id && x.TrangThai == false).FirstOrDefault();
                return View(pHIEUTHUTIEN);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
        [HttpPost]
        public ActionResult DongHocPhi(string status)
        {
            TAIKHOAN user = (TAIKHOAN)Session["user"];
            if (Session["user"] != null && user.PhanQuyen == "Phụ huynh")
            {
                try
                {
                    var users = (from item in db.PHUHUYNHs where item.TenTK == user.TenTK select item).FirstOrDefault();
                    var tre = (from item in db.TREs where item.MaPH == users.MaPH select item).FirstOrDefault();
                    string id = tre.MaTre;
                    var pHIEUTHUTIEN = db.PHIEUTHUTIENs.Where(x => x.MaTre == id && x.TrangThai == false).FirstOrDefault();
                    pHIEUTHUTIEN.TrangThai = true;
                    db.SaveChanges();
                    return Json(status, JsonRequestBehavior.AllowGet);        
                }
                catch (Exception ex)
                {
                    return Json(ex.Message, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
    }
}