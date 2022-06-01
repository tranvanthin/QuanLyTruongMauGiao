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
    public class ThucDonController : Controller
    {
        private QLMauGiao db = new QLMauGiao();

        // GET: ThucDon
        public ActionResult Index(int? page)
        {
            TAIKHOAN user = (TAIKHOAN)Session["user"];
            if (Session["user"] != null && user.PhanQuyen == "Quản lý")
            {
                IQueryable<THUCDONNGAY> thucdons = db.THUCDONNGAYs;
                thucdons = thucdons.OrderBy(td => td.Ngay);
                int pageSize = 10;
                int pageNumber = (page ?? 1);
                return View(thucdons.ToPagedList(pageNumber, pageSize)); 
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }

        }
        public PartialViewResult GetThucDonByDate(DateTime? startdate, DateTime? enddate, int? page)
        {
            var thucDons = db.THUCDONNGAYs.Where(td => td.Ngay >= startdate && td.Ngay <= enddate);
            thucDons = thucDons.OrderBy(td => td.Ngay);

            int pageSize = 10;
            int pageNumber = (page ?? 1);
            return PartialView("Index", thucDons.ToPagedList(pageNumber, pageSize));
        }

        // GET: ThucDon/Details/5
        public ActionResult Details(string id)
        {
            TAIKHOAN user = (TAIKHOAN)Session["user"];
            if (Session["user"] != null && user.PhanQuyen == "Quản lý")
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                THUCDONNGAY tHUCDONNGAY = db.THUCDONNGAYs.Find(id);
                if (tHUCDONNGAY == null)
                {
                    return HttpNotFound();
                }
                return View(tHUCDONNGAY); 
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
        public string SinhMaTDN()
        {
            var tdn = (from item in db.THUCDONNGAYs
                        select item).OrderByDescending(x=>x.MaTDN).FirstOrDefault();
            string maTDN = "";
            if(tdn != null)
            {
                int soTDN = int.Parse(tdn.MaTDN.Substring(3, 3)) + 1;
                if (soTDN < 10)
                {
                    maTDN = String.Format("{0}00{1}", "TDN", soTDN);
                }
                else if (soTDN < 100)
                {
                    maTDN = String.Format("{0}0{1}", "TDN", soTDN);
                }
                else if (soTDN < 1000)
                {
                    maTDN = String.Format("{0}{1}", "TDN", soTDN);
                }
                else
                {
                    maTDN = soTDN.ToString();
                }
            }
            else
            {
                maTDN = "TDN001";
            }
            return maTDN;
        }
        // GET: ThucDon/Create
        public ActionResult Create()
        {
            TAIKHOAN user = (TAIKHOAN)Session["user"];
            if (Session["user"] != null && user.PhanQuyen == "Quản lý")
            {
                ViewBag.MaTDN = SinhMaTDN();
                return View(); 
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        // POST: ThucDon/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(FormCollection frm)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    THUCDONNGAY tdn = new THUCDONNGAY();
                    tdn.MaTDN = frm["MaTDN"];
                    tdn.Ngay = DateTime.Parse(frm["Ngay"]);
                    tdn.BuaSang = frm["BuaSang"];
                    tdn.BuaTrua = frm["BuaTrua"];
                    tdn.BuaXe = frm["BuaXe"];
                    db.THUCDONNGAYs.Add(tdn);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    ViewBag.msg = ex.Message;
                }
            }
            ViewBag.MaTDN = SinhMaTDN();
            return View();
        }

        // GET: ThucDon/Edit/5
        public ActionResult Edit(string id)
        {
            TAIKHOAN user = (TAIKHOAN)Session["user"];
            if (Session["user"] != null && user.PhanQuyen == "Quản lý")
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                THUCDONNGAY tHUCDONNGAY = db.THUCDONNGAYs.Find(id);
                if (tHUCDONNGAY == null)
                {
                    return HttpNotFound();
                }
                return View(tHUCDONNGAY); 
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        // POST: ThucDon/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MaTDN,Ngay,BuaSang,BuaTrua,BuaXe")] THUCDONNGAY tHUCDONNGAY)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tHUCDONNGAY).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tHUCDONNGAY);
        }

        // GET: ThucDon/Delete/5
        public ActionResult Delete(string id)
        {
            TAIKHOAN user = (TAIKHOAN)Session["user"];
            if (Session["user"] != null && user.PhanQuyen == "Quản lý")
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                THUCDONNGAY tHUCDONNGAY = db.THUCDONNGAYs.Find(id);
                if (tHUCDONNGAY == null)
                {
                    return HttpNotFound();
                }
                return View(tHUCDONNGAY); 
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        // POST: ThucDon/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            THUCDONNGAY tHUCDONNGAY = db.THUCDONNGAYs.Find(id);
            db.THUCDONNGAYs.Remove(tHUCDONNGAY);
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
