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

namespace QuanLyTruongMauGiao.Controllers
{
    public class ThuTienController : Controller
    {
        private QLMauGiao db = new QLMauGiao();

        // GET: ThuTien
        public ActionResult Index(string TrangThai,int? page)
        {
            TAIKHOAN user = (TAIKHOAN)Session["user"];
            if (Session["user"] != null && user.PhanQuyen == "Quản lý")
            {
                var pHIEUTHUTIENs = db.PHIEUTHUTIENs.Include(p => p.TRE);
                pHIEUTHUTIENs = pHIEUTHUTIENs.OrderBy(p => p.MaPhieu);
                int pageSize = 10;
                int pageNumber = (page ?? 1);
                return View(pHIEUTHUTIENs.ToPagedList(pageNumber, pageSize));
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
        public PartialViewResult GetThucDonByTrangThai(string TrangThai, int? page)
        {
            var pHIEUTHUTIENs = db.PHIEUTHUTIENs.Include(p => p.TRE);
            if (TrangThai == "HoanThanh")
            {
                pHIEUTHUTIENs = pHIEUTHUTIENs.Where(x => x.TrangThai == true);
            }
            if (TrangThai == "ChuaHoanThanh")
            {
                pHIEUTHUTIENs = pHIEUTHUTIENs.Where(x => x.TrangThai == false);
            }
            pHIEUTHUTIENs = pHIEUTHUTIENs.OrderBy(p => p.MaPhieu);
            int pageSize = 10;
            int pageNumber = (page ?? 1);
            return PartialView("Index", pHIEUTHUTIENs.ToPagedList(pageNumber, pageSize));
        }

        // GET: ThuTien/Details/5
        public ActionResult Details(string id)
        {
            TAIKHOAN user = (TAIKHOAN)Session["user"];
            if (Session["user"] != null && user.PhanQuyen == "Quản lý")
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                PHIEUTHUTIEN pHIEUTHUTIEN = db.PHIEUTHUTIENs.Find(id);
                if (pHIEUTHUTIEN == null)
                {
                    return HttpNotFound();
                }
                var list = from d in db.DONGCHIPHIs
                           join cp in db.CHIPHIs on d.MaChiPhi equals cp.MaChiPhi
                           where d.MaPhieu == id
                           select new
                           {
                               MaPhieu = id,
                               Ma = d.MaChiPhi,
                               ThanhTien = cp.DonGia * d.SoLuong
                           };
                var listChiPhi = from d in db.DONGCHIPHIs
                                 where d.MaPhieu == id
                                 select d;
                ViewBag.ListChiPhi = listChiPhi.ToList();
                decimal tongTien = 0;
                foreach (var item in list.ToList())
                {
                    tongTien += item.ThanhTien;
                }
                ViewBag.TongTien = (long)tongTien;
                return View(pHIEUTHUTIEN); 
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        public string SinhMaPhieu()
        {
            var phieu = (from item in db.PHIEUTHUTIENs
                        select item).OrderByDescending(x=>x.MaPhieu).FirstOrDefault();
            string maPhieu = "";
            if(phieu != null)
            {
                int soPhieu = int.Parse(phieu.MaPhieu.Substring(2, 3)) + 1;
                if (soPhieu < 10)
                {
                    maPhieu = String.Format("{0}00{1}", "PT", soPhieu);
                }
                else if (soPhieu < 100)
                {
                    maPhieu = String.Format("{0}0{1}", "PT", soPhieu);
                }
                else if (soPhieu < 1000)
                {
                    maPhieu = String.Format("{0}{1}", "PT", soPhieu);
                }
                else
                {
                    maPhieu = soPhieu.ToString();
                }
            }
            else
            {
                maPhieu = "PT001";
            }
            return maPhieu;
        }

        // GET: ThuTien/Create
        public ActionResult Create()
        {
            TAIKHOAN user = (TAIKHOAN)Session["user"];
            if (Session["user"] != null && user.PhanQuyen == "Quản lý")
            {
                ViewBag.MaPhieu = SinhMaPhieu();
                ViewBag.NgayLap = DateTime.Now.ToString("dd-MM-yyyy");
                ViewBag.Tre = db.TREs;
                ViewBag.ChiPhi = db.CHIPHIs;
                return View(db.CHIPHIs.ToList()); 
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        // POST: ThuTien/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public ActionResult Create(string maPhieu, string maTre,IEnumerable<DONGCHIPHI> chiPhi) 
        {
            if (maPhieu != null && maTre != null && chiPhi != null)
            {
                try
                {
                    PHIEUTHUTIEN phieu = new PHIEUTHUTIEN();
                    phieu.MaPhieu = maPhieu;
                    phieu.MaTre = maTre;
                    phieu.NgayLapPhieu = DateTime.Now;
                    phieu.TrangThai = false;

                    db.PHIEUTHUTIENs.Add(phieu);

                    foreach (var item in chiPhi.ToList())
                    {
                        DONGCHIPHI dongCP = new DONGCHIPHI();
                        dongCP.MaPhieu = item.MaPhieu;
                        dongCP.MaChiPhi = item.MaChiPhi;
                        dongCP.SoLuong = item.SoLuong;
                        db.DONGCHIPHIs.Add(dongCP);
                    }
                    db.SaveChanges();
                    return Json("Tạo phiếu thu thành công", JsonRequestBehavior.AllowGet);
                }
                catch (Exception ex)
                {

                    ViewBag.error = ex.Message;
                }
            }
            ViewBag.MaPhieu = SinhMaPhieu();
            ViewBag.NgayLap = DateTime.Now.ToString("dd-MM-yyyy");
            ViewBag.Tre = db.TREs;
            ViewBag.ChiPhi = db.CHIPHIs;
            return View(db.CHIPHIs.ToList());
        }

        // GET: PhuHuynh/Delete/5
        public ActionResult Delete(string id)
        {
            TAIKHOAN user = (TAIKHOAN)Session["user"];
            if (Session["user"] != null && user.PhanQuyen == "Quản lý")
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                PHIEUTHUTIEN pHIEUTHUTIEN = db.PHIEUTHUTIENs.Find(id);
                if (pHIEUTHUTIEN == null)
                {
                    return HttpNotFound();
                }
                var list = from d in db.DONGCHIPHIs
                           join cp in db.CHIPHIs on d.MaChiPhi equals cp.MaChiPhi
                           where d.MaPhieu == id
                           select new
                           {
                               MaPhieu = id,
                               Ma = d.MaChiPhi,
                               ThanhTien = cp.DonGia * d.SoLuong
                           };
                var listChiPhi = from d in db.DONGCHIPHIs
                                 where d.MaPhieu == id
                                 select d;
                ViewBag.ListChiPhi = listChiPhi.ToList();
                decimal tongTien = 0;
                foreach (var item in list.ToList())
                {
                    tongTien += item.ThanhTien;
                }
                ViewBag.TongTien = (long)tongTien;
                return View(pHIEUTHUTIEN);
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
            PHIEUTHUTIEN pHIEUTHUTIEN = db.PHIEUTHUTIENs.Find(id);
            var list = from d in db.DONGCHIPHIs
                       join cp in db.CHIPHIs on d.MaChiPhi equals cp.MaChiPhi
                       where d.MaPhieu == id
                       select new
                       {
                           MaPhieu = id,
                           Ma = d.MaChiPhi,
                           ThanhTien = cp.DonGia * d.SoLuong
                       };
            var listChiPhi = from d in db.DONGCHIPHIs
                             where d.MaPhieu == id
                             select d;
            ViewBag.ListChiPhi = listChiPhi.ToList();
            decimal tongTien = 0;
            foreach (var item in list.ToList())
            {
                tongTien += item.ThanhTien;
            }
            ViewBag.TongTien = (long)tongTien;
            try
            {
                var dongCP = db.DONGCHIPHIs.Where(x => x.MaPhieu == pHIEUTHUTIEN.MaPhieu);
                db.DONGCHIPHIs.RemoveRange(dongCP);
                db.PHIEUTHUTIENs.Remove(pHIEUTHUTIEN);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Có lỗi khi xóa: " + ex.Message;
                return View("Delete", pHIEUTHUTIEN);
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
