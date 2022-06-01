using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using QuanLyTruongMauGiao.Models;
using QuanLyTruongMauGiao.Models.NoDatabase;
using PagedList;

namespace QuanLyTruongMauGiao.Controllers
{
    public class PhanCongGiaoVienController : Controller
    {
        private QLMauGiao db = new QLMauGiao();
        // GET: PhanCongGiaoVien
        public ActionResult Index(string searchstr)
        {
            TAIKHOAN user = (TAIKHOAN)Session["user"];
            if (Session["user"] != null && user.PhanQuyen == "Quản lý")
            {
                var phanCong = db.PHANCONGGIAOVIENs.Include(p => p.GIAOVIEN).Include(p => p.LOP);
                if (!String.IsNullOrEmpty(searchstr))
                {
                    phanCong = phanCong.Where(x => x.LOP.TenLop == searchstr);
                }

                var result = from p in phanCong
                             group p by new { p.MaLop, p.NamHoc } into g
                             select new PCGiaoVien()
                             {
                                 MaLop = g.Key.MaLop,
                                 NamHoc = g.Key.NamHoc,
                                 MaGV = g.Select(gv => gv.MaGV).ToList()
                             };

                ViewBag.pHANCONGGIAOVIENs = phanCong.ToList();
                result = result.Where(x=>x.NamHoc == DateTime.Now.Year.ToString()).OrderBy(x => x.MaLop);

                return View(result.ToList()); 
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
        public string SinhMaPCGV()
        {
            var pcgv = (from item in db.PHANCONGGIAOVIENs
                      select item).OrderByDescending(x=>x.MaPC).FirstOrDefault();
            string maPC = "";
            if(pcgv != null)
            {
                int soPCGV = int.Parse(pcgv.MaPC.Substring(2, 3)) + 1;
                if (soPCGV < 10)
                {
                    maPC = String.Format("{0}00{1}", "PC", soPCGV);
                }
                else if (soPCGV < 100)
                {
                    maPC = String.Format("{0}0{1}", "PC", soPCGV);
                }
                else if (soPCGV < 1000)
                {
                    maPC = String.Format("{0}{1}", "PC", soPCGV);
                }
                else
                {
                    maPC = soPCGV.ToString();
                }
            }
            else
            {
                maPC = "PC001";
            }
            return maPC;
        }
        // GET: PhanCongGiaoVien/Create
        public ActionResult Create()
        {
            TAIKHOAN user = (TAIKHOAN)Session["user"];
            if (Session["user"] != null && user.PhanQuyen == "Quản lý")
            {
                ViewBag.MaGV = db.GIAOVIENs;
                ViewBag.MaLop = new SelectList(db.LOPs, "MaLop", "TenLop");
                return View(); 
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(FormCollection frm)
        {
            string MaLop = frm["MaLop"];
            string NamHoc = frm["NamHoc"];
            if (ModelState.IsValid)
            {
                var query = from pcgv in db.PHANCONGGIAOVIENs
                            where pcgv.MaLop == MaLop && pcgv.NamHoc == NamHoc
                            select pcgv;
                if (query.Count() != 0)
                {
                    ViewBag.Error = "Lớp này đã được phân công";
                }
                else
                {
                    PHANCONGGIAOVIEN pc1 = new PHANCONGGIAOVIEN();
                    PHANCONGGIAOVIEN pc2 = new PHANCONGGIAOVIEN();
                    try
                    {
                        pc1.MaPC = SinhMaPCGV();
                        pc1.MaLop = frm["MaLop"];
                        pc1.NamHoc = frm["NamHoc"];
                        pc1.MaGV = frm["MaGV1"];
                        db.PHANCONGGIAOVIENs.Add(pc1);
                        db.SaveChanges();
                        pc2.MaPC = SinhMaPCGV();
                        pc2.MaLop = frm["MaLop"];
                        pc2.NamHoc = frm["NamHoc"];
                        pc2.MaGV = frm["MaGV2"];
                        db.PHANCONGGIAOVIENs.Add(pc2);
                                   
                        db.SaveChanges();
                        return RedirectToAction("Index");
                    }
                    catch (Exception ex)
                    {
                        ViewBag.Error = ex.Message;
                    }
                }

               
            }
            ViewBag.MaGV = db.GIAOVIENs;
            ViewBag.MaLop = new SelectList(db.LOPs, "MaLop", "TenLop");
            return View();
        }
        public ActionResult Edit(string id1, string id2)
        {
            TAIKHOAN user = (TAIKHOAN)Session["user"];
            if (Session["user"] != null && user.PhanQuyen == "Quản lý")
            {
                if (id1 == null || id2 == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                var phanCong = db.PHANCONGGIAOVIENs.Include(p => p.GIAOVIEN).Include(p => p.LOP);

                var result = from p in phanCong
                             group p by new { p.MaLop, p.NamHoc } into g
                             select new PCGiaoVien()
                             {
                                 MaLop = g.Key.MaLop,
                                 NamHoc = g.Key.NamHoc,
                                 MaGV = g.Select(gv => gv.MaGV).ToList()
                             };
                PCGiaoVien giaovien = null;
                foreach (var item in result)
                {
                    if (item.MaLop.Equals(id1) && item.NamHoc.Equals(id2))
                    {
                        giaovien = item;
                    }
                }
                if (giaovien == null)
                {
                    return HttpNotFound();
                }

                ViewBag.MaGV = db.GIAOVIENs;
                ViewBag.MaLop = db.LOPs;

                return View(giaovien); 
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        // POST: PhanCongGiaoVien/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(FormCollection frm)
        {
            string MaLop = frm["MaLop"];
            string NamHoc = frm["NamHoc"];
            try
            { 
                var phanCongEdit = db.PHANCONGGIAOVIENs.Where(pc => pc.MaLop == MaLop && pc.NamHoc == NamHoc).ToList();
                //foreach (var item in phanCongRemove)
                //{
                //    db.PHANCONGGIAOVIENs.Remove(item);            
                //}
                int i = 1;
                foreach(var item in phanCongEdit)
                {
                    if(i == 1)
                    {
                        item.MaGV = frm["MaGV1"];                    
                    }
                    if(i == 2)
                    {
                        item.MaGV = frm["MaGV2"];
                    }
                    i++;
                }
                try
                {
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    ViewBag.Error = ex.Message;
                }                
            }
            catch (Exception ex)
            {

                ViewBag.Error = ex.Message;
            }

            var phanCong = db.PHANCONGGIAOVIENs.Where(pc => pc.MaLop == MaLop && pc.NamHoc == NamHoc);
            var result = from p in phanCong
                         group p by new { p.MaLop, p.NamHoc } into g
                         select new PCGiaoVien()
                         {
                             MaLop = g.Key.MaLop,
                             NamHoc = g.Key.NamHoc,
                             MaGV = g.Select(gv => gv.MaGV).ToList()
                         };
            PCGiaoVien giaovien = result.FirstOrDefault();
            ViewBag.MaGV = db.GIAOVIENs;
            ViewBag.MaLop = db.LOPs;
            
            return View(giaovien);

        }

        // GET: PhanCongGiaoVien/Delete/5
        public ActionResult Delete(string id1,string id2)
        {
            TAIKHOAN user = (TAIKHOAN)Session["user"];
            if (Session["user"] != null && user.PhanQuyen == "Quản lý")
            {
                if (id1 == null || id2 == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                var phanCong = db.PHANCONGGIAOVIENs.Include(p => p.GIAOVIEN).Include(p => p.LOP);

                var result = from p in phanCong
                             group p by new { p.MaLop, p.NamHoc } into g
                             select new PCGiaoVien()
                             {
                                 MaLop = g.Key.MaLop,
                                 NamHoc = g.Key.NamHoc,
                                 MaGV = g.Select(gv => gv.MaGV).ToList()
                             };
                PCGiaoVien giaovien = null;
                foreach (var item in result)
                {
                    if (item.MaLop.Equals(id1) && item.NamHoc.Equals(id2))
                    {
                        giaovien = item;
                    }
                }
                if (giaovien == null)
                {
                    return HttpNotFound();
                }

                ViewBag.pHANCONGGIAOVIENs = phanCong.ToList();

                return View(giaovien); 
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        // POST: PhanCongGiaoVien/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id1, string id2)
        {
            var phanCongRemove = db.PHANCONGGIAOVIENs.Where(pc => pc.MaLop == id1 && pc.NamHoc == id2).ToList();
            foreach (var item in phanCongRemove)
            {
                db.PHANCONGGIAOVIENs.Remove(item);
            }
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
