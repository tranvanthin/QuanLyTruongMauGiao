using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using QuanLyTruongMauGiao.Models;

namespace QuanLyTruongMauGiao.Controllers
{
    public class HomeController : Controller
    {
        private QLMauGiao db = new QLMauGiao();
        public ActionResult Index()
        {
            if (Request.Cookies["userid"] != null)
                ViewBag.username = Request.Cookies["userid"].Value;
            if (Request.Cookies["pwd"] != null)
                ViewBag.password = Request.Cookies["pwd"].Value;
            return View();
        }
        public ActionResult HomePage()
        {
            
            if (Session["user"] != null)
            {
                TAIKHOAN user = (TAIKHOAN)Session["user"];
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
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
        public ActionResult CheckLogin(string username, string password, bool RememberMe)
        {
            var users = db.TAIKHOANs.Where(x => x.TenTK == username);
            if (users.Count() == 0)
            {
                ViewBag.Error = "Không tồn tại tài khoản này";
            }
            else
            {
                TAIKHOAN user = users.Single();
                if (user.MatKhau != password)
                {
                    ViewBag.Error = "Sai mật khẩu";
                }
                else
                {
                    if(RememberMe == true)
                    {
                        Response.Cookies["userid"].Value = user.TenTK;
                        Response.Cookies["pwd"].Value = user.MatKhau;
                        Response.Cookies["userid"].Expires = DateTime.Now.AddDays(15);
                        Response.Cookies["pwd"].Expires = DateTime.Now.AddDays(15);
                    }
                    else
                    {
                        Response.Cookies["userid"].Expires = DateTime.Now.AddDays(-1);
                        Response.Cookies["pwd"].Expires = DateTime.Now.AddDays(-1);
                    }

                    user.TrangThaiHD = true;
                    db.SaveChanges();
                    Session["user"] = user;
                    return RedirectToAction("HomePage");
                }
            }
            ViewBag.username = username;
            ViewBag.password = password;
            return View("Index");
        }
        public ActionResult Logout()
        {
            TAIKHOAN user = (TAIKHOAN)Session["user"];
            if(Session["user"] != null)
            {
                var users = db.TAIKHOANs.Where(x => x.TenTK == user.TenTK).FirstOrDefault();
                users.TrangThaiHD = false;
                db.SaveChanges();
                Session.Remove("user");
                return RedirectToAction("Index");
            }
            else
            {
                return RedirectToAction("Index");
            }
            
        }

        public ActionResult ChangePassword()
        {
            if (Session["user"] != null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Index");
            }
        }
        [HttpPost]
        public ActionResult ChangePassword(string password_old, string password_new1, string password_new2)
        {
            if (password_old == "" || password_new1 == "" || password_new2 == "")
            {
                ViewBag.Error = "Không được để trống.";
            }
            else
            {
                TAIKHOAN user = (TAIKHOAN)Session["user"];
                if (password_old != user.MatKhau)
                {
                    ViewBag.Error = "Mật khẩu hiện tại không đúng.";
                }
                else
                {
                    if (password_new1 != password_new2)
                    {
                        ViewBag.Error = "Nhập lại mật khẩu mới phải trùng với mật khẩu mới.";
                    }
                    else
                    {
                        var users = db.TAIKHOANs.Where(x => x.TenTK == user.TenTK).FirstOrDefault();
                        users.MatKhau = password_new1;
                        db.SaveChanges();
                        ViewBag.msg = "Đổi mật khẩu thành công.";
                    }
                }

            }
            return View();
        }
        public ActionResult ForgotPassword()
        {
            return View();
        }
        [HttpPost]
        public ActionResult ForgotPassword(FormCollection frm)
        {
            string username = frm["username"];
            TAIKHOAN tk = db.TAIKHOANs.Find(username);
            string email = "";
            string password = Membership.GeneratePassword(12, 1);
            bool sendMail = false;
            if (tk == null)
            {
                ViewBag.Error = "Không tồn tại tài khoản này";
            }
            else
            {
                if(tk.PhanQuyen == "Quản lý" || tk.PhanQuyen == "Giáo viên")
                {
                    var gv = db.GIAOVIENs.Where(x => x.TenTK == tk.TenTK).FirstOrDefault();
                    if(gv.Email == null)
                    {
                        ViewBag.Error = "Tài khoản này chưa có email";
                    }
                    else
                    {
                        email = gv.Email;
                    }
                }
                else
                {
                    var ph = db.PHUHUYNHs.Where(x => x.TenTK == tk.TenTK).FirstOrDefault();
                    if (ph.Email == "" || ph.Email == null)
                    {
                        ViewBag.Error = "Tài khoản này chưa có email";
                    }
                    else
                    {

                        email = ph.Email;  
                    }
                }
            }
            if(email != "")
            {
                string to = email; //To address    
                string from = "thinlovevh@gmail.com"; //From address    
                MailMessage message = new MailMessage(from, to);
                StringBuilder mailbody = new StringBuilder();
                mailbody.AppendLine("Xin chào <br />");
                mailbody.AppendLine("Bạn đang nhận được email này vì chúng tôi đã nhận được yêu cầu đặt lại mật khẩu cho tài khoản của bạn.<br />");
                mailbody.AppendLine("Mật khẩu hiện tại của bạn là : "+ password + "<br />");
                mailbody.AppendLine("Regards,<br />");
                mailbody.AppendLine("Mầm non Minh Khai <br />");
                message.Subject = "Quên mật khẩu Mầm non Minh Khai";
                message.Body = mailbody.ToString();
                message.BodyEncoding = Encoding.UTF8;
                message.IsBodyHtml = true;
                SmtpClient client = new SmtpClient("smtp.gmail.com", 587); //Gmail smtp    
                System.Net.NetworkCredential basicCredential1 = new
                System.Net.NetworkCredential("thinlovevh@gmail.com", "Mixi090889@.");
                client.EnableSsl = true;
                client.UseDefaultCredentials = false;
                client.Credentials = basicCredential1;
                try
                {
                    client.Send(message);
                    sendMail = true;
                }

                catch (Exception ex)
                {
                    ViewBag.Error = ex.Message;
                }
            }
            if (sendMail)
            {
                try
                {
                    ViewBag.Message = "Email reset mật khẩu gửi thành công.";
                    tk.MatKhau = password;
                    db.SaveChanges();
                }
                catch(Exception ex)
                {
                    ViewBag.Error = ex.Message;
                }
            }
            return View();
        }
    }
}