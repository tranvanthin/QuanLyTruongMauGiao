using QuanLyTruongMauGiao.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using PagedList;

namespace QuanLyTruongMauGiao.Controllers
{
    public class ChucNangGiaoVienController : Controller
    {
        private QLMauGiao db = new QLMauGiao();
        // GET: ChucNangGiaoVien
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Error()
        {
            return View();
        }
        public ActionResult Error1()
        {
            return View();
        }
        public ActionResult DiemDanh(string id)
        {
            TAIKHOAN user = (TAIKHOAN)Session["user"];
            if (Session["user"] != null && user.PhanQuyen == "Giáo viên")
            {
                var users = (from item in db.GIAOVIENs where item.TenTK == user.TenTK select item).FirstOrDefault();
                var lop = (from item in db.PHANCONGGIAOVIENs where item.MaGV == users.MaGV && item.NamHoc == DateTime.Now.Year.ToString() select item).FirstOrDefault();
                var tre = from item in db.TREs where item.MaLop == lop.MaLop select item;
                if (lop != null)
                {
                    if(tre.FirstOrDefault() != null)
                    {
                        var listMaTre = from item in db.DiemDanhVaDangKyBuaAns
                                        join t in db.TREs on item.MaTre equals t.MaTre
                                        where DbFunctions.TruncateTime(item.Ngay) == DbFunctions.TruncateTime(DateTime.Now) && t.MaLop == lop.MaLop
                                        select new
                                        {
                                            MaTre = item.MaTre
                                        };
                        var listTre = from item in db.DiemDanhVaDangKyBuaAns
                                      join t in db.TREs on item.MaTre equals t.MaTre
                                      where DbFunctions.TruncateTime(item.Ngay) == DbFunctions.TruncateTime(DateTime.Now) && t.MaLop == lop.MaLop
                                      select item;
                        List<string> list = new List<string>();
                        foreach (var item in listMaTre.ToList())
                        {
                            list.Add(item.MaTre);
                        }
                        ViewBag.ListMaTre = list.ToList();
                        ViewBag.ListTre = listTre.ToList();
                        ViewBag.Lop = lop.LOP.TenLop;
                        return View(tre.ToList());
                    }
                    return RedirectToAction("Error1");
                }
                return RedirectToAction("Error");

            }
            else
            {
                return RedirectToAction("Index", "Home");
            }

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
        public class ParamDiemDanh
        {
            public string MaTre { get; set; }
            public bool select { get; set; }
        }

        [HttpPost]
        public ActionResult DiemDanh(ParamDiemDanh[] list)
        {

            try
            {
                if (list != null)
                {
                    foreach (var item in list)
                    {
                        string maTre = item.MaTre.Trim();
                        var query = (from x in db.DiemDanhVaDangKyBuaAns
                                     where x.MaTre == maTre && DbFunctions.TruncateTime(x.Ngay) == DbFunctions.TruncateTime(DateTime.Now)
                                     select x).FirstOrDefault();
                        if (query == null)
                        {
                            DiemDanhVaDangKyBuaAn diemDanh = new DiemDanhVaDangKyBuaAn();
                            diemDanh.ID = SinhMaDD();
                            diemDanh.MaTre = item.MaTre.Trim();
                            diemDanh.Ngay = DateTime.Now;
                            diemDanh.DiemDanh = item.select;
                            diemDanh.DangKiBuaAn = false;
                            db.DiemDanhVaDangKyBuaAns.Add(diemDanh);
                        }
                        else
                        {
                            query.DiemDanh = item.select;
                        }
                        db.SaveChanges();

                    }
                    return Json("Điểm danh thành công", JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
            return View(db.TREs.ToList());
        }

        public ActionResult XemDanhSachLop(int? page)
        {
            TAIKHOAN user = (TAIKHOAN)Session["user"];
            if (Session["user"] != null && user.PhanQuyen == "Giáo viên")
            {
                var users = (from item in db.GIAOVIENs where item.TenTK == user.TenTK select item).FirstOrDefault();
                var MaLop = (from item in db.PHANCONGGIAOVIENs where item.MaGV == users.MaGV && item.NamHoc == DateTime.Now.Year.ToString() select item).FirstOrDefault();
                var tre = from item in db.TREs where item.MaLop == MaLop.MaLop select item;
                tre = tre.OrderBy(tr => tr.TenTre);
                int pageSize = 7;
                int pageNumber = (page ?? 1);
                if(MaLop != null)
                {
                    if (tre.FirstOrDefault() != null)
                    {
                        ViewBag.Lop = MaLop.LOP.TenLop;
                        return View(tre.ToPagedList(pageNumber, pageSize));
                    }
                    return RedirectToAction("Error1");
                }
                return RedirectToAction("Error");
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
        public ActionResult Xemchitiet(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TRE tRE = db.TREs.Find(id);
            if (tRE == null)
            {
                return HttpNotFound();
            }
            return View(tRE);
        }
        public ActionResult DanhSachDanhGia(int? page)
        {
            TAIKHOAN user = (TAIKHOAN)Session["user"];
            if (Session["user"] != null && user.PhanQuyen == "Giáo viên")
            {
                var users = (from item in db.GIAOVIENs where item.TenTK == user.TenTK select item).FirstOrDefault();
                var MaLop = (from item in db.PHANCONGGIAOVIENs where item.MaGV == users.MaGV && item.NamHoc == DateTime.Now.Year.ToString() select item).FirstOrDefault();
                var tre = from item in db.TREs where item.MaLop == MaLop.MaLop select item;
                tre = tre.OrderBy(tr => tr.TenTre);
                int pageSize = 7;
                int pageNumber = (page ?? 1);
                if (MaLop != null)
                {
                    if (tre.FirstOrDefault() != null)
                    {
                        ViewBag.Lop = MaLop.LOP.TenLop;
                        return View(tre.ToPagedList(pageNumber, pageSize));
                    }
                    return RedirectToAction("Error1");
                }
                return RedirectToAction("Error");
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
        public string SinhMaPhieu()
        {
            var record = (from item in db.PHIEUDANHGIAs
                          select item).OrderByDescending(x => x.MaPhieu).FirstOrDefault();
            string maPhieu = "";
            if (record != null)
            {
                int soP = int.Parse(record.MaPhieu.Substring(1, 4)) + 1;
                if (soP < 10)
                {
                    maPhieu = String.Format("{0}000{1}", "P", soP);
                }
                else if (soP < 100)
                {
                    maPhieu = String.Format("{0}00{1}", "P", soP);
                }
                else if (soP < 1000)
                {
                    maPhieu = String.Format("{0}0{1}", "P", soP);
                }
                else if (soP < 10000)
                {
                    maPhieu = String.Format("{0}{1}", "P", soP);
                }
                else
                {
                    maPhieu = soP.ToString();
                }
            }
            else
            {
                maPhieu = "P0001";
            }
            return maPhieu;
        }
        public class ParamDanhGia
        {
            public string MaNDDG { get; set; }
            public bool select { get; set; }
        }
        public ActionResult DanhGiaTre(string id)
        {
            TAIKHOAN user = (TAIKHOAN)Session["user"];
            if (Session["user"] != null && user.PhanQuyen == "Giáo viên")
            {
                var users = (from item in db.GIAOVIENs where item.TenTK == user.TenTK select item).FirstOrDefault();
                var tre = (from item in db.TREs where item.MaTre == id select item).FirstOrDefault();
                var lop = (from item in db.PHANCONGGIAOVIENs where item.MaGV == users.MaGV select item).FirstOrDefault();
                ViewBag.GiaoVien = users;
                ViewBag.Tre = tre;
                ViewBag.Lop = lop;
                return View(db.NOIDUNGDANHGIAs.ToList());
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
        [HttpPost]
        public ActionResult DanhGiaTre(string maTre,string maGV, ParamDanhGia[] KetQuaDG)
        {
            try
            {
                if (KetQuaDG != null)
                {
                    string namHoc = DateTime.Now.Year.ToString();
                        var query = (from x in db.PHIEUDANHGIAs
                                     where x.MaTre == maTre.Trim() && x.MaGV == maGV.Trim() && x.NamHoc == namHoc && x.NgayTao.Month == DateTime.Now.Month
                                     select x).FirstOrDefault();
                        if (query == null)
                        {
                            PHIEUDANHGIA phieu = new PHIEUDANHGIA();
                            phieu.MaPhieu = SinhMaPhieu();
                            phieu.MaTre = maTre.Trim();
                            phieu.MaGV = maGV.Trim();
                            phieu.NgayTao = DateTime.Now;
                            phieu.NamHoc = namHoc;
                            db.PHIEUDANHGIAs.Add(phieu);
                            foreach(var x in KetQuaDG)
                            {
                                KETQUADANHGIA kqDG = new KETQUADANHGIA();
                                kqDG.MaPhieu = phieu.MaPhieu;
                                kqDG.MaNDDG = x.MaNDDG.Trim();
                                kqDG.KetQua = x.select;
                                db.KETQUADANHGIAs.Add(kqDG);
                            }
                        }
                        else
                        {
                            var result = from kq in db.KETQUADANHGIAs
                                         join t in db.PHIEUDANHGIAs on kq.MaPhieu equals t.MaPhieu
                                         where t.MaTre == maTre.Trim() && t.MaGV == maGV.Trim() && t.NamHoc == namHoc && t.NgayTao.Month == DateTime.Now.Month
                                         select kq;
                            foreach(var x in KetQuaDG)
                            {
                                var kq = (from item in result
                                          where item.MaNDDG == x.MaNDDG
                                          select item).FirstOrDefault();
                                if(kq != null)
                                {
                                    kq.KetQua = x.select;
                                }
                                else
                                {
                                    KETQUADANHGIA kqDG = new KETQUADANHGIA();
                                    kqDG.MaPhieu = result.FirstOrDefault().MaPhieu;
                                    kqDG.MaNDDG = x.MaNDDG.Trim();
                                    kqDG.KetQua = x.select;
                                    db.KETQUADANHGIAs.Add(kqDG);
                                }
                            }

                        }
                        db.SaveChanges();

                    }
                    return Json("Đánh giá trẻ thành công", JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
        }
    }
}