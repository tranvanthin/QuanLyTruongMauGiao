using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using PagedList;
using QuanLyTruongMauGiao.Models;

namespace QuanLyTruongMauGiao.Controllers
{
    public class TaiKhoanController : Controller
    {
        private QLMauGiao db = new QLMauGiao();
        // GET: TaiKhoan
        public ActionResult Index(int? page)
        {
            TAIKHOAN user = (TAIKHOAN)Session["user"];
            if (Session["user"] != null && user.PhanQuyen == "Quản lý")
            {
                var taikhoan = from item in db.TAIKHOANs select item;
                //var taikhoan = from item in db.TAIKHOANs select item;

                taikhoan = taikhoan.OrderBy(tr => tr.TenTK);
                int pageSize = 10;
                int pageNumber = (page ?? 1);

                return View(taikhoan.ToPagedList(pageNumber,pageSize));
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }

        }
        public PartialViewResult GetQuyen(string quyen, int? page)
        {
            //var taikhoan = db.TAIKHOANs.Where(x => x.PhanQuyen.Contains(quyen));
            var taikhoan = from item in db.TAIKHOANs 
                           where item.PhanQuyen.Contains(quyen) 
                           orderby item.TenTK
                           select item;

            int pageSize = 7;
            int pageNumber = (page ?? 1);
            return PartialView("Index", taikhoan.ToPagedList(pageNumber, pageSize));
        }
        // GET: TaiKhoan/Details/5
        public ActionResult Details(string id)
        {
            TAIKHOAN user = (TAIKHOAN)Session["user"];
            if (Session["user"] != null && user.PhanQuyen == "Quản lý")
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                TAIKHOAN tAIKHOAN = db.TAIKHOANs.Find(id);
                if (tAIKHOAN == null)
                {
                    return HttpNotFound();
                }
                return View(tAIKHOAN);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
        public ActionResult ResetPass(string id)
        {
            TAIKHOAN tAIKHOAN = db.TAIKHOANs.Find(id);
            if (tAIKHOAN == null)
            {
                return HttpNotFound();
            }
            return View(tAIKHOAN);
        }
        [HttpPost, ActionName("ResetPass")]
        [ValidateAntiForgeryToken]
        public ActionResult ConfirmReset(string TenTK)
        {
            var tk = db.TAIKHOANs.Find(TenTK);
            tk.MatKhau = "123456";
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        public string SinhTKGV()
        {
            var gv = db.TAIKHOANs.Where(t=>t.TenTK.StartsWith("gv")).OrderByDescending(x => x.TenTK).FirstOrDefault();
            string tkGV = "";
            if (gv != null)
            {
                int soTKGV = int.Parse(gv.TenTK.Substring(2, 3)) + 1;
                if (soTKGV < 10)
                {
                    tkGV = String.Format("{0}00{1}", "gv", soTKGV);
                }
                else if (soTKGV < 100)
                {
                    tkGV = String.Format("{0}0{1}", "gv", soTKGV);
                }
                else if (soTKGV < 1000)
                {
                    tkGV = String.Format("{0}{1}", "gv", soTKGV);
                }
                else
                {
                    tkGV = soTKGV.ToString();
                }
            }
            else
            {
                tkGV = "gv001";
            }

            return tkGV;
        }
        public string SinhTKPH()
        {
            var ph = db.TAIKHOANs.Where(t => t.TenTK.StartsWith("ph")).OrderByDescending(x => x.TenTK).FirstOrDefault();
            string tkPH = "";
            if (ph != null)
            {
                int soTKPH = int.Parse(ph.TenTK.Substring(2, 3)) + 1;
                if (soTKPH < 10)
                {
                    tkPH = String.Format("{0}00{1}", "ph", soTKPH);
                }
                else if (soTKPH < 100)
                {
                    tkPH = String.Format("{0}0{1}", "ph", soTKPH);
                }
                else if (soTKPH < 1000)
                {
                    tkPH = String.Format("{0}{1}", "ph", soTKPH);
                }
                else
                {
                    tkPH = soTKPH.ToString();
                }
            }
            else
            {
                tkPH = "ph001";
            }

            return tkPH;
        }
        // GET: TaiKhoan/Create
        public ActionResult Create()
        {
            TAIKHOAN user = (TAIKHOAN)Session["user"];
            if (Session["user"] != null && user.PhanQuyen == "Quản lý")
            {
                ViewBag.QuanLy = db.TAIKHOANs.Where(t => t.TenTK.StartsWith("Admin")).ToList().Count() + 1;
                ViewBag.GiaoVien = SinhTKGV();
                ViewBag.PhuHuynh = SinhTKPH();
                return View();

            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        // POST: TaiKhoan/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "TenTK,MatKhau,PhanQuyen,TrangThaiHD,AnhDaiDien")] TAIKHOAN tAIKHOAN)
        {
            if (ModelState.IsValid)
            {
                
                try
                {
                    var f = Request.Files["InputImage"];
                    if (f.FileName != "")
                    {
                        string uploadPath = Server.MapPath("~/Image/Profile/") + tAIKHOAN.TenTK.ToString() + ".png";
                        if (System.IO.File.Exists(uploadPath))
                            System.IO.File.Delete(uploadPath);
                        f.SaveAs(uploadPath);
                        tAIKHOAN.AnhDaiDien = tAIKHOAN.TenTK + ".png";
                    }
                    db.TAIKHOANs.Add(tAIKHOAN);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    ViewBag.msg = ex.Message;
                }
            }
            ViewBag.QuanLy = db.TAIKHOANs.Where(x => x.PhanQuyen == "Quản lý").ToList().Count() + 1;
            ViewBag.GiaoVien = SinhTKGV();
            ViewBag.PhuHuynh = SinhTKPH();
            return View(tAIKHOAN);
        }

        // GET: TaiKhoan/Edit/5
        public ActionResult Edit(string id)
        {
            TAIKHOAN user = (TAIKHOAN)Session["user"];
            if (Session["user"] != null && user.PhanQuyen == "Quản lý")
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                TAIKHOAN tAIKHOAN = db.TAIKHOANs.Find(id);
                if (tAIKHOAN == null)
                {
                    return HttpNotFound();
                }
                return View(tAIKHOAN);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        // POST: TaiKhoan/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "TenTK,MatKhau,PhanQuyen,TrangThaiHD,AnhDaiDien")] TAIKHOAN tAIKHOAN)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var f = Request.Files["InputImage"];
                    if (f.FileName != "")
                    {
                        string uploadPath = Server.MapPath("~/Image/Profile/") + tAIKHOAN.TenTK.ToString() + ".png";
                        if (System.IO.File.Exists(uploadPath))
                            System.IO.File.Delete(uploadPath);
                        f.SaveAs(uploadPath);
                        tAIKHOAN.AnhDaiDien = tAIKHOAN.TenTK + ".png";
                    }

                    db.Entry(tAIKHOAN).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    ViewBag.msg = ex.Message;
                }
            }
            return View(tAIKHOAN);
        }
        // GET: TaiKhoan/Delete/5
        public ActionResult Delete(string id)
        {
            TAIKHOAN user = (TAIKHOAN)Session["user"];
            if (Session["user"] != null && user.PhanQuyen == "Quản lý")
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                TAIKHOAN tAIKHOAN = db.TAIKHOANs.Find(id);
                if (tAIKHOAN == null)
                {
                    return HttpNotFound();
                }
                return View(tAIKHOAN);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        // POST: PhuHuynh/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            TAIKHOAN tAIKHOAN = db.TAIKHOANs.Find(id);
            try
            {
                if(tAIKHOAN.PhanQuyen == "Quản lý" || tAIKHOAN.PhanQuyen == "Giáo viên")
                {
                    var gIAOVIEN = db.GIAOVIENs.Where(x => x.TenTK == tAIKHOAN.TenTK).FirstOrDefault();
                    if(gIAOVIEN != null)
                    {
                        var pcGV = db.PHANCONGGIAOVIENs.Where(x => x.MaGV == gIAOVIEN.MaGV);
                        db.PHANCONGGIAOVIENs.RemoveRange(pcGV);

                        var pdg = db.PHIEUDANHGIAs.Where(p => p.MaGV == gIAOVIEN.MaGV);
                        foreach (var item in pdg.ToList())
                        {
                            var dongDG = db.KETQUADANHGIAs.Where(x => x.MaPhieu == item.MaPhieu);
                            db.KETQUADANHGIAs.RemoveRange(dongDG);
                        }
                        db.PHIEUDANHGIAs.RemoveRange(pdg);
                        db.GIAOVIENs.Remove(gIAOVIEN);
                    }
                }
                else
                {
                    var pHUHUYNH = db.PHUHUYNHs.Where(x => x.TenTK == tAIKHOAN.TenTK).FirstOrDefault();
                    if(pHUHUYNH != null)
                    {
                        var tRE = db.TREs.Where(x => x.MaPH == pHUHUYNH.MaPH);
                        foreach (var tre in tRE.ToList())
                        {
                            var phieuDG = db.PHIEUDANHGIAs.Where(p => p.MaTre == tre.MaTre);
                            foreach (var p in phieuDG.ToList())
                            {
                                var dongDG = db.KETQUADANHGIAs.Where(x => x.MaPhieu == p.MaPhieu);
                                db.KETQUADANHGIAs.RemoveRange(dongDG);
                            }
                            db.PHIEUDANHGIAs.RemoveRange(phieuDG);

                            var dd = db.DiemDanhVaDangKyBuaAns.Where(p => p.MaTre == tre.MaTre);
                            db.DiemDanhVaDangKyBuaAns.RemoveRange(dd);

                            var phieuTT = db.PHIEUTHUTIENs.Where(p => p.MaTre == tre.MaTre);
                            foreach (var p in phieuTT.ToList())
                            {
                                var dongCP = db.DONGCHIPHIs.Where(x => x.MaPhieu == p.MaPhieu);
                                db.DONGCHIPHIs.RemoveRange(dongCP);
                            }
                            db.PHIEUTHUTIENs.RemoveRange(phieuTT);

                            var lop = (from item in db.LOPs where item.MaLop == item.MaLop select item).FirstOrDefault();
                            db.TREs.Remove(tre);
                            lop.SiSo = lop.SiSo - 1;
                        }
                        db.PHUHUYNHs.Remove(pHUHUYNH);
                    }
                    
                }
                db.TAIKHOANs.Remove(tAIKHOAN);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Có lỗi khi xóa: " + ex.Message;
                return View("Delete", tAIKHOAN);
            }

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
