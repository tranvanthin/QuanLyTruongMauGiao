using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using QuanLyTruongMauGiao.Models;
using PagedList;
using System.IO;

namespace QuanLyTruongMauGiao.Controllers
{
    public class TreController : Controller
    {
        private QLMauGiao db = new QLMauGiao();
        // GET: Tre
        public ActionResult Index(int? page)
        {
            TAIKHOAN user = (TAIKHOAN)Session["user"];
            if (Session["user"] != null && user.PhanQuyen == "Quản lý")
            {
                var tREs = db.TREs.Include(t => t.LOP).Include(t => t.PHUHUYNH);

                tREs = tREs.OrderBy(tr => tr.TenTre);
                int pageSize = 7;
                int pageNumber = (page ?? 1);
                return View(tREs.ToPagedList(pageNumber, pageSize));
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }

        }

        public PartialViewResult GetName(string name, int? page)
        {
            var contacts = db.TREs.Where(x => x.TenTre.Contains(name));
            contacts = contacts.OrderBy(tr => tr.TenTre);

            int pageSize = 7;
            int pageNumber = (page ?? 1);
            return PartialView("Index", contacts.ToPagedList(pageNumber, pageSize));
        }
        // GET: Tre/Details/5
        public ActionResult Details(string id)
        {
            TAIKHOAN user = (TAIKHOAN)Session["user"];
            if (Session["user"] != null && user.PhanQuyen == "Quản lý")
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
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
        public string SinhMaTre()
        {
            var tre = (from item in db.TREs
                       select item).OrderByDescending(x=>x.MaTre).FirstOrDefault();
            string maTre = "";
            if(tre != null)
            {
                int soTre = int.Parse(tre.MaTre.Substring(3, 3)) + 1;
                if (soTre < 10)
                {
                    maTre = String.Format("{0}00{1}", "TRE", soTre);
                }
                else if (soTre < 100)
                {
                    maTre = String.Format("{0}0{1}", "TRE", soTre);
                }
                else if (soTre < 1000)
                {
                    maTre = String.Format("{0}{1}", "TRE", soTre);
                }
                else
                {
                    maTre = soTre.ToString();
                }
            }
            else
            {
                maTre = "TRE001";
            }
            return maTre;
        }
        // GET: Tre/Create
        public ActionResult Create()
        {
            TAIKHOAN user = (TAIKHOAN)Session["user"];
            if (Session["user"] != null && user.PhanQuyen == "Quản lý")
            {
                ViewBag.MaTre = SinhMaTre();
                ViewBag.MaLop = new SelectList(db.LOPs, "MaLop", "TenLop");
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        // POST: Tre/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
       public ActionResult Create([Bind(Include = "MaTre,MaLop,MaPH,TenTre,NgaySinh,GioiTinh,QueQuan,DanToc,NgayNhapHoc,Anh")] TRE tRE)
        {
            if (ModelState.IsValid)
            {              
                try
                {
                    var f = Request.Files["InputImage"];
                    if (f.FileName != "")
                    {
                        string uploadPath = Server.MapPath("~/Image/Tre/") + tRE.MaTre.ToString() + ".png";
                        if (System.IO.File.Exists(uploadPath))
                            System.IO.File.Delete(uploadPath);
                        f.SaveAs(uploadPath);
                        tRE.Anh = tRE.MaTre + ".png";
                    }
                    db.TREs.Add(tRE);
                    var lop = (from item in db.LOPs where item.MaLop == tRE.MaLop select item).FirstOrDefault();
                    lop.SiSo++;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                catch(Exception ex)
                {
                    ViewBag.msg = ex.Message;
                }
            }
            ViewBag.MaTre = SinhMaTre();
            ViewBag.MaLop = new SelectList(db.LOPs, "MaLop", "TenLop", tRE.MaLop);
            ViewBag.MaPH = new SelectList(db.PHUHUYNHs, "MaPH", "TenPH", tRE.MaPH);
            return View(tRE);
        }

        // GET: Tre/Edit/5
        public ActionResult Edit(string id)
        {
            TAIKHOAN user = (TAIKHOAN)Session["user"];
            if (Session["user"] != null && user.PhanQuyen == "Quản lý")
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
                ViewBag.MaLop = new SelectList(db.LOPs, "MaLop", "TenLop", tRE.MaLop);
                return View(tRE);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        // POST: Tre/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MaTre,MaLop,MaPH,TenTre,NgaySinh,GioiTinh,QueQuan,DanToc,NgayNhapHoc,Anh")] TRE tRE)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var f = Request.Files["InputImage"];
                    if (f.FileName != "")
                    {
                        string uploadPath = Server.MapPath("~/Image/Tre/") + tRE.MaTre.ToString() + ".png";
                        if (System.IO.File.Exists(uploadPath))
                            System.IO.File.Delete(uploadPath);
                        f.SaveAs(uploadPath);
                        tRE.Anh = tRE.MaTre + ".png";
                    }

                    db.Entry(tRE).State = EntityState.Modified;

                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    ViewBag.msg = ex.Message;
                }
            }
            ViewBag.MaLop = new SelectList(db.LOPs, "MaLop", "TenLop", tRE.MaLop);
            return View(tRE);
        }

        // GET: Tre/Delete/5
        public ActionResult Delete(string id)
        {
            TAIKHOAN user = (TAIKHOAN)Session["user"];
            if (Session["user"] != null && user.PhanQuyen == "Quản lý")
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
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        // POST: Tre/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            TRE tRE = db.TREs.Find(id);
            try
            {
                //xóa các phiếu đánh giá của trẻ
                var phieuDG = db.PHIEUDANHGIAs.Where(p => p.MaTre == tRE.MaTre);
                foreach(var p in phieuDG.ToList())
                {
                    var dongDG = db.KETQUADANHGIAs.Where(x => x.MaPhieu == p.MaPhieu);
                    db.KETQUADANHGIAs.RemoveRange(dongDG);
                }
                db.PHIEUDANHGIAs.RemoveRange(phieuDG);
                //Xóa dữ liệu điểm danh và đăng ký bữa ăn của trẻ
                var dd = db.DiemDanhVaDangKyBuaAns.Where(p => p.MaTre == tRE.MaTre);
                db.DiemDanhVaDangKyBuaAns.RemoveRange(dd);
                //Xóa các phiếu thu tiền của trẻ
                var phieuTT = db.PHIEUTHUTIENs.Where(p => p.MaTre == tRE.MaTre);
                foreach (var p in phieuTT.ToList())
                {
                    var dongCP = db.DONGCHIPHIs.Where(x => x.MaPhieu == p.MaPhieu);
                    db.DONGCHIPHIs.RemoveRange(dongCP);
                }
                db.PHIEUTHUTIENs.RemoveRange(phieuTT);

                var lop = (from item in db.LOPs where item.MaLop == tRE.MaLop select item).FirstOrDefault();
                db.TREs.Remove(tRE);
                lop.SiSo = lop.SiSo - 1;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch(Exception ex)
            {
                ViewBag.msg = "Có lỗi khi xóa: " + ex.Message;
                return View("Delete", tRE);
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
