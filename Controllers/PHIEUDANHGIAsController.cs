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
    public class PHIEUDANHGIAsController : Controller
    {
        private QLMauGiao db = new QLMauGiao();

        // GET: PHIEUDANHGIAs
        public ActionResult Index()
        {
            var pHIEUDANHGIAs = db.PHIEUDANHGIAs.Include(p => p.GIAOVIEN).Include(p => p.TRE);
            return View(pHIEUDANHGIAs.ToList());
        }

        // GET: PHIEUDANHGIAs/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PHIEUDANHGIA pHIEUDANHGIA = db.PHIEUDANHGIAs.Find(id);
            if (pHIEUDANHGIA == null)
            {
                return HttpNotFound();
            }
            return View(pHIEUDANHGIA);
        }

        // GET: PHIEUDANHGIAs/Create
        public ActionResult Create()
        {
            ViewBag.MaGV = new SelectList(db.GIAOVIENs, "MaGV", "TenGV");
            ViewBag.MaTre = new SelectList(db.TREs, "MaTre", "MaLop");
            return View();
        }

        // POST: PHIEUDANHGIAs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MaPhieu,MaTre,MaGV,NgayTao,NamHoc")] PHIEUDANHGIA pHIEUDANHGIA)
        {
            if (ModelState.IsValid)
            {
                db.PHIEUDANHGIAs.Add(pHIEUDANHGIA);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.MaGV = new SelectList(db.GIAOVIENs, "MaGV", "TenGV", pHIEUDANHGIA.MaGV);
            ViewBag.MaTre = new SelectList(db.TREs, "MaTre", "MaLop", pHIEUDANHGIA.MaTre);
            return View(pHIEUDANHGIA);
        }

        // GET: PHIEUDANHGIAs/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PHIEUDANHGIA pHIEUDANHGIA = db.PHIEUDANHGIAs.Find(id);
            if (pHIEUDANHGIA == null)
            {
                return HttpNotFound();
            }
            ViewBag.MaGV = new SelectList(db.GIAOVIENs, "MaGV", "TenGV", pHIEUDANHGIA.MaGV);
            ViewBag.MaTre = new SelectList(db.TREs, "MaTre", "MaLop", pHIEUDANHGIA.MaTre);
            return View(pHIEUDANHGIA);
        }

        // POST: PHIEUDANHGIAs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MaPhieu,MaTre,MaGV,NgayTao,NamHoc")] PHIEUDANHGIA pHIEUDANHGIA)
        {
            if (ModelState.IsValid)
            {
                db.Entry(pHIEUDANHGIA).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.MaGV = new SelectList(db.GIAOVIENs, "MaGV", "TenGV", pHIEUDANHGIA.MaGV);
            ViewBag.MaTre = new SelectList(db.TREs, "MaTre", "MaLop", pHIEUDANHGIA.MaTre);
            return View(pHIEUDANHGIA);
        }

        // GET: PHIEUDANHGIAs/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PHIEUDANHGIA pHIEUDANHGIA = db.PHIEUDANHGIAs.Find(id);
            if (pHIEUDANHGIA == null)
            {
                return HttpNotFound();
            }
            return View(pHIEUDANHGIA);
        }

        // POST: PHIEUDANHGIAs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            PHIEUDANHGIA pHIEUDANHGIA = db.PHIEUDANHGIAs.Find(id);
            db.PHIEUDANHGIAs.Remove(pHIEUDANHGIA);
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
