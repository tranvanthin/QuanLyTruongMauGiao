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
    public class LopController : Controller
    {
        private QLMauGiao db = new QLMauGiao();
        public void UpdateSiSo()
        {
            var lop = from item in db.TREs
                      group item by item.MaLop into g
                      select new
                      {
                          MaLop = g.Key,
                          SiSo = g.Count()
                      };
            foreach(var item in lop.ToList())
            {
                var l = db.LOPs.Where(x => x.MaLop == item.MaLop).FirstOrDefault();
                l.SiSo = item.SiSo;
            }
            db.SaveChanges();
        }
        // GET: Lop
        public ActionResult Index(int? page)
        {
            TAIKHOAN user = (TAIKHOAN)Session["user"];
            if (Session["user"] != null && user.PhanQuyen == "Quản lý")
            {
                UpdateSiSo();
                var dslop = from item in db.LOPs select item;
                dslop = dslop.OrderBy(lop => lop.MaLop);
                int pagenumber = (page ?? 1);
                int pagesize = 10;
                return View(dslop.ToPagedList(pagenumber,pagesize));
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }

        }
        public ActionResult AddTre(string id)
        {
            LOP Lop = db.LOPs.Find(id);
            return View(Lop);
        }
        public PartialViewResult AddTrePV(string id, string MaLop)
        {
            TRE tre = (from item in db.TREs where item.MaTre == id select item).FirstOrDefault();
            tre.MaLop = MaLop;
            LOP Lop = db.LOPs.Find(MaLop);
            Lop.SiSo++;
            db.SaveChanges();
            return PartialView("AddTre", Lop);
        }
        // GET: Lop/Details/5
        public ActionResult Details(string id)
        {
            TAIKHOAN user = (TAIKHOAN)Session["user"];
            if (Session["user"] != null && user.PhanQuyen == "Quản lý")
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                LOP lOP = db.LOPs.Find(id);
                if (lOP == null)
                {
                    return HttpNotFound();
                }
                return View(lOP);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }

        }
        public string SinhMaLop()
        {
            var lop = (from item in db.LOPs
                       select item).OrderByDescending(x => x.MaLop).FirstOrDefault();
            string maLop = "";
            if(lop != null)
            {
                int soLop = int.Parse(lop.MaLop.Substring(1, 4)) + 1;
                if (soLop < 10)
                {
                    maLop = String.Format("{0}000{1}", "L", soLop);
                }
                else if (soLop < 100)
                {
                    maLop = String.Format("{0}00{1}", "L", soLop);
                }
                else if (soLop < 1000)
                {
                    maLop = String.Format("{0}0{1}", "L", soLop);
                }
                else if (soLop < 10000)
                {
                    maLop = String.Format("{0}{1}", "L", soLop);
                }
                else
                {
                    maLop = soLop.ToString();
                }
            }
            else
            {
                maLop = "L0001";
            }
            return maLop;
        }
        // GET: Lop/Create
        public ActionResult Create()
        {
            TAIKHOAN user = (TAIKHOAN)Session["user"];
            if (Session["user"] != null && user.PhanQuyen == "Quản lý")
            {
                ViewBag.MaLop = SinhMaLop();
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        // POST: Lop/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MaLop,TenLop,SiSo,DoTuoi,Khoi")] LOP lOP)
        {
            if (ModelState.IsValid)
            {
                db.LOPs.Add(lOP);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(lOP);
        }

        // GET: Lop/Edit/5
        public ActionResult Edit(string id)
        {
            TAIKHOAN user = (TAIKHOAN)Session["user"];
            if (Session["user"] != null && user.PhanQuyen == "Quản lý")
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                LOP lOP = db.LOPs.Find(id);
                if (lOP == null)
                {
                    return HttpNotFound();
                }
                return View(lOP);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }

        }

        // POST: Lop/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MaLop,TenLop,SiSo,DoTuoi,Khoi")] LOP lOP)
        {
            if (ModelState.IsValid)
            {
                db.Entry(lOP).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(lOP);
        }

        // GET: Lop/Delete/5
        public ActionResult Delete(string id)
        {
            TAIKHOAN user = (TAIKHOAN)Session["user"];
            if (Session["user"] != null && user.PhanQuyen == "Quản lý")
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                LOP lOP = db.LOPs.Find(id);
                if (lOP == null)
                {
                    return HttpNotFound();
                }
                return View(lOP);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        // POST: Lop/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            LOP lOP = db.LOPs.Find(id);
            db.LOPs.Remove(lOP);
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
