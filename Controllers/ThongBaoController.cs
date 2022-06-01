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
    public class ThongBaoController : Controller
    {
        private QLMauGiao db = new QLMauGiao();

        // GET: ThongBao
        public ActionResult Index(int? page)
        {
            TAIKHOAN user = (TAIKHOAN)Session["user"];
            if (Session["user"] != null && user.PhanQuyen == "Quản lý")
            {
                IQueryable<THONGBAO> thongbaos = db.THONGBAOs;
                thongbaos = thongbaos.OrderByDescending(tb => tb.NgayTB);
                int pageSize = 10;
                int pageNumber = (page ?? 1);
                return View(thongbaos.ToPagedList(pageNumber, pageSize));
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
        public PartialViewResult GetThongBaoByDate(DateTime? startdate, DateTime? enddate, int? page)
        {
            var thongbaos = db.THONGBAOs.Where(tb => tb.NgayTB >= startdate && tb.NgayTB <= enddate);
            thongbaos = thongbaos.OrderByDescending(tb => tb.NgayTB);

            int pageSize = 10;
            int pageNumber = (page ?? 1);
            return PartialView("Index", thongbaos.ToPagedList(pageNumber, pageSize));
        }
        // GET: ThongBao/Details/5
        public ActionResult Details(string id)
        {
            TAIKHOAN user = (TAIKHOAN)Session["user"];
            if (Session["user"] != null && user.PhanQuyen == "Quản lý")
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                THONGBAO tHONGBAO = db.THONGBAOs.Find(id);
                if (tHONGBAO == null)
                {
                    return HttpNotFound();
                }
                return View(tHONGBAO);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
        public string SinhMaTB()
        {
            var tb = (from item in db.THONGBAOs
                       select item).OrderByDescending(x => x.MaTB).FirstOrDefault();
            string maTB = "";
            if (tb != null)
            {
                int soTB = int.Parse(tb.MaTB.Substring(2, 3)) + 1;
                if (soTB < 10)
                {
                    maTB = String.Format("{0}00{1}", "TB", soTB);
                }
                else if (soTB < 100)
                {
                    maTB = String.Format("{0}0{1}", "TB", soTB);
                }
                else if (soTB < 1000)
                {
                    maTB = String.Format("{0}{1}", "TB", soTB);
                }
                else
                {
                    maTB = soTB.ToString();
                }
            }
            else
            {
                maTB = "TB001";
            }
            return maTB;
        }
        // GET: ThongBao/Create
        public ActionResult Create()
        {
            TAIKHOAN user = (TAIKHOAN)Session["user"];
            if (Session["user"] != null && user.PhanQuyen == "Quản lý")
            {
                ViewBag.MaTB = SinhMaTB();
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        // POST: ThongBao/Create
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
                    THONGBAO tb = new THONGBAO();
                    tb.MaTB = frm["MaTB"];
                    tb.NgayTB = DateTime.Now;
                    tb.TenTB = frm["TenTB"];
                    tb.NoiDung = frm["NoiDung"];
                    db.THONGBAOs.Add(tb);
                    String obj = frm["object"];
                    if(obj == "all")
                    {
                        var objects = db.TAIKHOANs;
                        foreach(var item in objects.ToList())
                        {
                            THONGBAO_TK t = new THONGBAO_TK();
                            t.MaTB = tb.MaTB;
                            t.TenTK = item.TenTK;
                            t.DaXem = false;
                            db.THONGBAO_TK.Add(t);
                        }
                    }
                    else if(obj == "gv")
                    {
                        var objects = db.TAIKHOANs.Where(tk => tk.PhanQuyen == "Giáo viên");
                        foreach (var item in objects.ToList())
                        {
                            THONGBAO_TK t = new THONGBAO_TK();
                            t.MaTB = tb.MaTB;
                            t.TenTK = item.TenTK;
                            t.DaXem = false;
                            db.THONGBAO_TK.Add(t);
                        }
                    }
                    else if(obj == "ph")
                    {
                        var objects = db.TAIKHOANs.Where(tk => tk.PhanQuyen == "Phụ huynh");
                        foreach (var item in objects.ToList())
                        {
                            THONGBAO_TK t = new THONGBAO_TK();
                            t.MaTB = tb.MaTB;
                            t.TenTK = item.TenTK;
                            t.DaXem = false;
                            db.THONGBAO_TK.Add(t);
                        }
                    }
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    ViewBag.msg = ex.Message;
                }
            }
            ViewBag.MaTB = SinhMaTB();
            return View();
        }

        // GET: ThongBao/Edit/5
        public ActionResult Edit(string id)
        {
            TAIKHOAN user = (TAIKHOAN)Session["user"];
            if (Session["user"] != null && user.PhanQuyen == "Quản lý")
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                THONGBAO tHONGBAO = db.THONGBAOs.Find(id);
                if (tHONGBAO == null)
                {
                    return HttpNotFound();
                }
                return View(tHONGBAO);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        // POST: ThongBao/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(FormCollection frm)
        {
            THONGBAO tHONGBAO = db.THONGBAOs.Find(frm["MaTB"]);
            if (tHONGBAO == null)
            {
                return HttpNotFound();
            }
            if (ModelState.IsValid)
            {
                try
                {  
                    tHONGBAO.NgayTB = DateTime.Now;
                    tHONGBAO.TenTB = frm["TenTB"];
                    tHONGBAO.NoiDung = frm["NoiDung"];
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    ViewBag.msg = ex.Message;
                }
            }
            return View(tHONGBAO);
        }

        // GET: ThongBao/Delete/5
        public ActionResult Delete(string id)
        {
            TAIKHOAN user = (TAIKHOAN)Session["user"];
            if (Session["user"] != null && user.PhanQuyen == "Quản lý")
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                THONGBAO tHONGBAO = db.THONGBAOs.Find(id);
                if (tHONGBAO == null)
                {
                    return HttpNotFound();
                }
                return View(tHONGBAO);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        // POST: ThongBao/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            THONGBAO tHONGBAO = db.THONGBAOs.Find(id);
            try
            {
                var result = db.THONGBAO_TK.Where(x => x.MaTB == tHONGBAO.MaTB);
                db.THONGBAO_TK.RemoveRange(result);
                db.THONGBAOs.Remove(tHONGBAO);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Có lỗi khi xóa: " + ex.Message;
                return View("Delete", tHONGBAO);
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
