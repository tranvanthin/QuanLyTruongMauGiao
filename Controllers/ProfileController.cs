using QuanLyTruongMauGiao.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using PagedList;

namespace QuanLyTruongMauGiao.Controllers
{
    public class ProfileController : Controller
    {
        // GET: Profile
        QLMauGiao db = new QLMauGiao();
        public ActionResult Index()
        {
            var user = Session["user"] as TAIKHOAN;
            if (user == null)
            {
                return RedirectToAction("Index", "home");
            }
            else if (user.PhanQuyen == "Quản lý")
                return View("ProfileAdmin");
            else if (user.PhanQuyen == "Giáo viên")
                return View("ProfileGV");
            else
                return View("ProfilePH");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditAdmin()
        {
            string magv = Request.Form["MaGV"];
            var user = (from item in db.GIAOVIENs where item.MaGV == magv select item).FirstOrDefault();
            user.DienThoai = Request.Form["DienThoai"];
            user.Email = Request.Form["Email"];

            var f = Request.Files["inputimg"];
            var account = (from item in db.TAIKHOANs where item.TenTK == user.TenTK select item).FirstOrDefault();
            string filename = magv + ".png";

            if (f.FileName != "")
            {
                string uploadPath = Server.MapPath("~/Image/Profile/") + filename;
                if (System.IO.File.Exists(uploadPath))
                    System.IO.File.Delete(uploadPath);
                f.SaveAs(uploadPath);
                account.AnhDaiDien = filename;
            }
            db.SaveChanges();
            Session["user"] = account;

            return RedirectToAction("Index", "Profile");

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditGV()
        {
            string magv = Request.Form["MaGV"];
            var user = (from item in db.GIAOVIENs where item.MaGV == magv select item).FirstOrDefault();
            user.DienThoai = Request.Form["DienThoai"];
            user.Email = Request.Form["Email"];


            var f = Request.Files["inputimg"];
            string filename = magv + ".png";
            var account = (from item in db.TAIKHOANs where item.TenTK == user.TenTK select item).FirstOrDefault();

            if (f.FileName != "")
            {
                string uploadPath = Server.MapPath("~/Image/Profile/") + filename;
                if (System.IO.File.Exists(uploadPath))
                    System.IO.File.Delete(uploadPath);
                f.SaveAs(uploadPath);
                account.AnhDaiDien = filename;
            }
            db.SaveChanges();
            Session["user"] = account;
            return RedirectToAction("Index", "Profile");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditPH()
        {
            string maph = Request.Form["MaPH"];
            var user = (from item in db.PHUHUYNHs where item.MaPH == maph select item).FirstOrDefault();
            user.DienThoai = Request.Form["DienThoai"];
            user.Email = Request.Form["Email"];
            var f = Request.Files["inputimg"];
            string filename = maph + ".png";
            var account = (from item in db.TAIKHOANs where item.TenTK == user.TenTK select item).FirstOrDefault();

            if (f.FileName != "")
            {
                string uploadPath = Server.MapPath("~/Image/Profile/") + filename;
                if (System.IO.File.Exists(uploadPath))
                    System.IO.File.Delete(uploadPath);
                f.SaveAs(uploadPath);
                account.AnhDaiDien = filename;
            }
            db.SaveChanges();
            Session["user"] = account;
            return RedirectToAction("Index", "Profile");

        }
        [ChildActionOnly]
        public ActionResult ThongBao()
        {
            try
            {
                TAIKHOAN user = (TAIKHOAN)Session["user"];
                var listTB = db.THONGBAO_TK.Where(x => x.TenTK == user.TenTK);
                ViewBag.SoTB = listTB.Where(x => x.DaXem == false).Count();
                listTB = listTB.OrderByDescending(x => x.THONGBAO.NgayTB).Take(5);
                return PartialView(listTB.ToList());
            }
            catch(Exception ex)
            {
                return View(ex.Message);
            }
        }
        public ActionResult ViewAllThongBao(int? page)
        {
            if (Session["user"] != null)
            {
                TAIKHOAN user = (TAIKHOAN)Session["user"];
                var listTB = db.THONGBAO_TK.Where(x => x.TenTK == user.TenTK).OrderByDescending(x => x.THONGBAO.NgayTB);
                if(user.PhanQuyen == "Quản lý")
                {
                    ViewBag.Layout = "~/Views/Shared/_Layout.cshtml";
                }
                else if(user.PhanQuyen == "Giáo viên")
                {
                    ViewBag.Layout = "~/Views/Shared/LayoutGV.cshtml";
                }
                else
                {
                    ViewBag.Layout = "~/Views/Shared/LayoutPH.cshtml";
                }
                int pageSize = 10;
                int pageNumber = (page ?? 1);
                return View(listTB.ToPagedList(pageNumber, pageSize));
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
        public ActionResult Details(string id)
        {
            if (Session["user"] != null)
            {
                TAIKHOAN user = (TAIKHOAN)Session["user"];
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                THONGBAO tHONGBAO = db.THONGBAOs.Find(id);
                if (tHONGBAO == null)
                {
                    return HttpNotFound();
                }
                if (user.PhanQuyen == "Quản lý")
                {
                    ViewBag.Layout = "~/Views/Shared/_Layout.cshtml";
                }
                else if (user.PhanQuyen == "Giáo viên")
                {
                    ViewBag.Layout = "~/Views/Shared/LayoutGV.cshtml";
                }
                else
                {
                    ViewBag.Layout = "~/Views/Shared/LayoutPH.cshtml";
                }
                THONGBAO_TK tbs = db.THONGBAO_TK.Where(tb => tb.TenTK == user.TenTK && tb.MaTB == id).FirstOrDefault();
                tbs.DaXem = true;
                db.SaveChanges();
                return View(tHONGBAO);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
        // GET: Profile/Delete/5
        public ActionResult Delete(string id)
        {
            if (Session["user"] != null)
            {
                TAIKHOAN user = (TAIKHOAN)Session["user"];
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                THONGBAO tHONGBAO = db.THONGBAOs.Find(id);
                if (tHONGBAO == null)
                {
                    return HttpNotFound();
                }
                if (user.PhanQuyen == "Quản lý")
                {
                    ViewBag.Layout = "~/Views/Shared/_Layout.cshtml";
                }
                else if (user.PhanQuyen == "Giáo viên")
                {
                    ViewBag.Layout = "~/Views/Shared/LayoutGV.cshtml";
                }
                else
                {
                    ViewBag.Layout = "~/Views/Shared/LayoutPH.cshtml";
                }
                return View(tHONGBAO);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        // POST: Profile/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            if (Session["user"] != null)
            {
                TAIKHOAN user = (TAIKHOAN)Session["user"];
                var tHONGBAO = db.THONGBAO_TK.Where(tb => tb.TenTK == user.TenTK && tb.MaTB == id).FirstOrDefault();
                db.THONGBAO_TK.Remove(tHONGBAO);
                db.SaveChanges();
                return RedirectToAction("ViewAllThongBao");
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
    }
}