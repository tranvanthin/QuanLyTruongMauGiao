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
    public class GiaoVienController : Controller
    {
        private QLMauGiao db = new QLMauGiao();
        // GET: GiaoVien
        public ActionResult Index(int? page)
        {
            TAIKHOAN user = (TAIKHOAN)Session["user"];
            if (Session["user"] != null && user.PhanQuyen == "Quản lý")
            {
                var gIAOVIENs = db.GIAOVIENs.Include(g => g.TAIKHOAN);
                gIAOVIENs = gIAOVIENs.OrderBy(gv => gv.TenGV);
                int pageNumber = (page ?? 1);
                int pageSize = 10;
                return View(gIAOVIENs.ToPagedList(pageNumber,pageSize));
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        // GET: GiaoVien/Details/5
        public ActionResult Details(string id)
        {
            TAIKHOAN user = (TAIKHOAN)Session["user"];
            if (Session["user"] != null && user.PhanQuyen == "Quản lý")
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                GIAOVIEN gIAOVIEN = db.GIAOVIENs.Find(id);
                if (gIAOVIEN == null)
                {
                    return HttpNotFound();
                }
                return View(gIAOVIEN);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
        public string SinhMaGiaoVien()
        {
            var gv = (from item in db.GIAOVIENs
                       select item).OrderByDescending(x => x.MaGV).FirstOrDefault();
            string maGV = "";
            if (gv != null)
            {
                int soGV = int.Parse(gv.MaGV.Substring(2, 3)) + 1;
                if (soGV < 10)
                {
                    maGV = String.Format("{0}00{1}", "gv", soGV);
                }
                else if (soGV < 100)
                {
                    maGV = String.Format("{0}0{1}", "gv", soGV);
                }
                else if (soGV < 1000)
                {
                    maGV = String.Format("{0}{1}", "gv", soGV);
                }
                else
                {
                    maGV = soGV.ToString();
                }
            }
            else
            {
                maGV = "gv001";
            }
            
            return maGV;
        }
        public string SinhTKGV()
        {
            var gv = db.TAIKHOANs.Where(t => t.TenTK.StartsWith("gv")).OrderByDescending(x => x.TenTK).FirstOrDefault();
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
        // GET: GiaoVien/Create
        public ActionResult Create()
        {
            TAIKHOAN user = (TAIKHOAN)Session["user"];
            if (Session["user"] != null && user.PhanQuyen == "Quản lý")
            {
                 ViewBag.TKGV = SinhTKGV();
                 ViewBag.MaGV = SinhMaGiaoVien();
                 return View();
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        // POST: GiaoVien/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MaGV,TenGV,NgaySinh,GioiTinh,QueQuan,DienThoai,Email,LoaiHopDong,KinhNghiem,TrinhDo,ChuyenNganh,LoaiTotNghiep,Anh,TenTK")] GIAOVIEN gIAOVIEN)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var f = Request.Files["InputImage"];
                    if (f.FileName != "")
                    {
                        string uploadPath = Server.MapPath("~/Image/GiaoVien/") + gIAOVIEN.MaGV.ToString() + ".png";
                        if (System.IO.File.Exists(uploadPath))
                            System.IO.File.Delete(uploadPath);
                        f.SaveAs(uploadPath);
                        gIAOVIEN.Anh = gIAOVIEN.MaGV + ".png";
                    }

                    TAIKHOAN tk = new TAIKHOAN();
                    tk.TenTK = gIAOVIEN.TenTK;
                    tk.MatKhau = "123456";
                    tk.PhanQuyen = "Giáo viên";
                    tk.TrangThaiHD = false;
                    tk.AnhDaiDien = "Default.png";
                    db.TAIKHOANs.Add(tk);
                    db.GIAOVIENs.Add(gIAOVIEN);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                    catch (Exception ex)
                {
                    ViewBag.msg = ex.Message;
                }
            }
            ViewBag.TKGV = SinhTKGV();
            ViewBag.MaGV = SinhMaGiaoVien();
            return View(gIAOVIEN);
        }

        // GET: GiaoVien/Edit/5
        public ActionResult Edit(string id)
        {
            TAIKHOAN user = (TAIKHOAN)Session["user"];
            if (Session["user"] != null && user.PhanQuyen == "Quản lý")
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                GIAOVIEN gIAOVIEN = db.GIAOVIENs.Find(id);
                if (gIAOVIEN == null)
                {
                    return HttpNotFound();
                }
                return View(gIAOVIEN);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        // POST: GiaoVien/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MaGV,TenGV,NgaySinh,GioiTinh,QueQuan,DienThoai,Email,LoaiHopDong,KinhNghiem,TrinhDo,ChuyenNganh,LoaiTotNghiep,Anh,TenTK")] GIAOVIEN gIAOVIEN)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var f = Request.Files["InputImage"];
                    if (f.FileName != "")
                    {
                        string uploadPath = Server.MapPath("~/Image/GiaoVien/") + gIAOVIEN.MaGV.ToString() + ".png";
                        if (System.IO.File.Exists(uploadPath))
                            System.IO.File.Delete(uploadPath);
                        f.SaveAs(uploadPath);
                        gIAOVIEN.Anh = gIAOVIEN.MaGV + ".png";
                    }
                    db.Entry(gIAOVIEN).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    ViewBag.msg = ex.Message;
                }
            }
            return View(gIAOVIEN);
        }

        // GET: GiaoVien/Delete/5
        public ActionResult Delete(string id)
        {
            TAIKHOAN user = (TAIKHOAN)Session["user"];
            if (Session["user"] != null && user.PhanQuyen == "Quản lý")
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                GIAOVIEN gIAOVIEN = db.GIAOVIENs.Find(id);
                if (gIAOVIEN == null)
                {
                    return HttpNotFound();
                }
                return View(gIAOVIEN);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        // POST: GiaoVien/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            GIAOVIEN gIAOVIEN = db.GIAOVIENs.Find(id);
            try
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
                var tk = (from item in db.TAIKHOANs where item.TenTK == gIAOVIEN.TenTK select item).FirstOrDefault();
                db.GIAOVIENs.Remove(gIAOVIEN);
                db.TAIKHOANs.Remove(tk);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Có lỗi khi xóa: " + ex.Message;
                return View("Delete", gIAOVIEN);
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
