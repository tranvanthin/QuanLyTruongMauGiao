using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using QuanLyTruongMauGiao.Models;

namespace QuanLyTruongMauGiao.Controllers
{
    public class NoiDungDanhGiasController : Controller
    {
        private QLMauGiao db = new QLMauGiao();

        // GET: NoiDungDanhGias
        public ActionResult Index()
        {
            TAIKHOAN user = (TAIKHOAN)Session["user"];
            if (Session["user"] != null && user.PhanQuyen == "Giáo viên")
            {
                return View(db.NOIDUNGDANHGIAs.ToList());
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        public string SinhMaNDDG()
        {
            var nddg = (from item in db.NOIDUNGDANHGIAs
                        select item).OrderByDescending(x => x.MaNDDG).FirstOrDefault();
            string maNDDG = "";
            if (nddg != null)
            {
                int soNDDG = int.Parse(nddg.MaNDDG.Substring(2, 3)) + 1;
                if (soNDDG < 10)
                {
                    maNDDG = String.Format("{0}00{1}", "ND", soNDDG);
                }
                else if (soNDDG < 100)
                {
                    maNDDG = String.Format("{0}0{1}", "ND", soNDDG);
                }
                else if (soNDDG < 1000)
                {
                    maNDDG = String.Format("{0}{1}", "ND", soNDDG);
                }
                else
                {
                    maNDDG = soNDDG.ToString();
                }
            }
            else
            {
                maNDDG = "ND001";
            }
            return maNDDG;
        }
        // GET: NoiDungDanhGias/Create
        public ActionResult Create()
        {
            TAIKHOAN user = (TAIKHOAN)Session["user"];
            if (Session["user"] != null && user.PhanQuyen == "Giáo viên")
            {
                ViewBag.MaNDDG = SinhMaNDDG();
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        // POST: NoiDungDanhGias/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MaNDDG,NoiDungDanhGia1")] NOIDUNGDANHGIA nOIDUNGDANHGIA)
        {
            if (ModelState.IsValid)
            {
                db.NOIDUNGDANHGIAs.Add(nOIDUNGDANHGIA);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(nOIDUNGDANHGIA);
        }

        // GET: NoiDungDanhGias/Edit/5
        public ActionResult Edit(string id)
        {
            TAIKHOAN user = (TAIKHOAN)Session["user"];
            if (Session["user"] != null && user.PhanQuyen == "Giáo viên")
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                NOIDUNGDANHGIA nOIDUNGDANHGIA = db.NOIDUNGDANHGIAs.Find(id);
                if (nOIDUNGDANHGIA == null)
                {
                    return HttpNotFound();
                }
                return View(nOIDUNGDANHGIA);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        // POST: NoiDungDanhGias/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MaNDDG,NoiDungDanhGia1")] NOIDUNGDANHGIA nOIDUNGDANHGIA)
        {
            if (ModelState.IsValid)
            {
                db.Entry(nOIDUNGDANHGIA).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(nOIDUNGDANHGIA);
        }

        // GET: NoiDungDanhGias/Delete/5
        public ActionResult Delete(string id)
        {
            TAIKHOAN user = (TAIKHOAN)Session["user"];
            if (Session["user"] != null && user.PhanQuyen == "Giáo viên")
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                NOIDUNGDANHGIA nOIDUNGDANHGIA = db.NOIDUNGDANHGIAs.Find(id);
                if (nOIDUNGDANHGIA == null)
                {
                    return HttpNotFound();
                }
                return View(nOIDUNGDANHGIA);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        // POST: NoiDungDanhGias/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            NOIDUNGDANHGIA nOIDUNGDANHGIA = db.NOIDUNGDANHGIAs.Find(id);
            try
            {
                var ketquaDG = db.KETQUADANHGIAs.Where(x => x.MaNDDG == nOIDUNGDANHGIA.MaNDDG);
                db.KETQUADANHGIAs.RemoveRange(ketquaDG);
                db.NOIDUNGDANHGIAs.Remove(nOIDUNGDANHGIA);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Có lỗi khi xóa: " + ex.Message;
                return View("Delete", nOIDUNGDANHGIA);
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
