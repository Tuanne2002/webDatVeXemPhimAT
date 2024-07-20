using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebDatVePhim.Models;

namespace WebDatVePhim.Controllers
{
    public class TrangAcountController : Controller
    {
        // GET: TrangAcount
        DatVeXemPhimATEntities db = new DatVeXemPhimATEntities();

        public ActionResult TrangAcount()
        {
            return View();
        }
        [HttpGet]
        public ActionResult Login()
        {
            return View(); 
        }

        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Register( NguoiDung ur)
        {
            if (ModelState.IsValid)
            {
                if(db.NguoiDungs.Any(x =>x.emailND == ur.emailND))
                {
                    ViewBag.Message = "Email đã tồn tại !!!";
                }
                else
                {
                    db.NguoiDungs.Add(ur);
                    db.SaveChanges();
                    Session["TaiKhoan"] = ur.taiKhoan; // Lưu tài khoản vào session
                    Response.Write("<script>alert('Đăng ký tài khoản thành công')</script>");
                    return RedirectToAction("Index", "HomeNguoiDung");
                }
            }
            return View();
        }

        [HttpPost]
        public ActionResult Login(NguoiDung ur)
        {
            var query = db.NguoiDungs.FirstOrDefault(m => m.taiKhoan == ur.taiKhoan && m.matKhau == ur.matKhau);
            if(query != null)
            {
                Session["TaiKhoan"] = ur.taiKhoan;
                Response.Write("<script>alert('Đăng nhập thành công!!!')</script>");
                return RedirectToAction("Index", "HomeNguoiDung");
            }
            else
            {
                Response.Write("<script>alert('Sai tài khoản hoặc mật khẩu')</script>");
            }
            return View();
        }

        public ActionResult Profile()
        {
            if (Session["TaiKhoan"] != null)
            {
                var taiKhoan = Session["TaiKhoan"].ToString();
                var user = db.NguoiDungs.FirstOrDefault(u => u.taiKhoan == taiKhoan);
                return View(user);
            }
            return RedirectToAction("Login");
        }


        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("Index", "HomeNguoiDung");
        }

        public ActionResult Edit()
        {
            if (Session["TaiKhoan"] != null)
            {
                var taiKhoan = Session["TaiKhoan"].ToString();
                var user = db.NguoiDungs.FirstOrDefault(u => u.taiKhoan == taiKhoan);
                return View(user);
            }
            return RedirectToAction("Login");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Update(NguoiDung updatedUser)
        {
            if (ModelState.IsValid)
            {
                var taiKhoan = Session["TaiKhoan"].ToString();
                var user = db.NguoiDungs.FirstOrDefault(u => u.taiKhoan == taiKhoan);
                if (user != null)
                {
                    user.hoVaTen = updatedUser.hoVaTen;
                    user.emailND = updatedUser.emailND;
                    user.dienThoai = updatedUser.dienThoai;
                    user.diaChi = updatedUser.diaChi;

                    db.SaveChanges();
                    return RedirectToAction("Profile");
                }
            }
            return View("Edit", updatedUser);
        }



    }
}