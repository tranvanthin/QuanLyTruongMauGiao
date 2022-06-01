using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QuanLyTruongMauGiao.Models;

namespace QuanLyTruongMauGiao.Controllers
{
    public class ThongKeController : Controller
    {
        private QLMauGiao db = new QLMauGiao();
        // GET: ThongKe
        public ActionResult Index()
        {
            TAIKHOAN user = (TAIKHOAN)Session["user"];
            if (Session["user"] != null && user.PhanQuyen == "Quản lý")
            {
                ViewBag.Tre = db.TREs.Count();
                ViewBag.GiaoVien = db.GIAOVIENs.Count();
                ViewBag.PhieuThuTien = db.PHIEUTHUTIENs.Where(x => x.TrangThai == false).Count();
                ViewBag.AccountOL = db.TAIKHOANs.Where(x => x.TrangThaiHD == true).Count();
                //ViewBag.NgayDiHoc = db.NGAYDIHOCs.ToList();

                var queryDSL = from tre in db.TREs
                               group tre by tre.MaLop into g
                               select new
                               {
                                   MaLop = g.Key,
                                   SoTre = g.Count()
                               };
                var query = from lop in db.LOPs
                            join x in queryDSL on lop.MaLop equals x.MaLop
                            select new
                            {
                                MaLop = lop.MaLop,
                                TenLop = lop.TenLop,
                                DoTuoi = lop.DoTuoi,
                                SoTre = x.SoTre
                            };

                Dictionary<string, int> list = new Dictionary<string, int>();
                foreach (var item in query.ToList())
                {
                    list.Add(item.TenLop, item.SoTre);
                }
                ViewBag.DSLop = list;

                var queryDoTuoi = from t in query
                                  group t by t.DoTuoi into gr
                                  select new 
                                  {
                                      DoTuoi = gr.Key,
                                      SoTre = gr.Sum(x=>x.SoTre)
                                  };

                Dictionary<string, string> listDT = new Dictionary<string, string>();
                foreach (var item in queryDoTuoi.ToList())
                {
                    listDT.Add(item.DoTuoi.ToString(), Math.Round((float)item.SoTre * 100 / db.TREs.Count(),2).ToString());
                }
                ViewBag.DT = listDT;

                List<string> listDoTuoi = new List<string>();
                List<string> listPhanTram = new List<string>();
                foreach (var item in queryDoTuoi.ToList())
                {
                    listDoTuoi.Add(item.DoTuoi.ToString());
                    listPhanTram.Add(Math.Round((float)item.SoTre * 100 / db.TREs.Count(), 2).ToString());
                }
                ViewBag.DoTuoi = String.Join(",",listDoTuoi);
                ViewBag.PhanTram = String.Join(",", listPhanTram);
                return View(); 
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
    }
}