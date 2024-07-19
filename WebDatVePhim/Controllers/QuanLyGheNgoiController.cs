using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebDatVePhim.Models;
using PagedList.Mvc;
using PagedList;

namespace WebDatVePhim.Controllers
{
    public class QuanLyGheNgoiController : Controller
    {
        // GET: QuanLyGheNgoi
        DatVeXemPhimATEntities db = new DatVeXemPhimATEntities();

        public ActionResult Index(int? page)
        {
            int pageNumber = page ?? 1;
            int pageSize = 50;
            return View(db.Ghes.ToList().OrderBy(n => n.id_Ghe).ToPagedList(pageNumber, pageSize));
            //var seats = db.Ghes.ToList();
            //return View(seats);
        }
        [HttpGet]
        public ActionResult ThemMoi()
        {
            //đưa dữ liệu vào dropdownlist
            ViewBag.id_PhongChieu = new SelectList(db.PhongChieux.ToList(), "id_PhongChieu", "tenPhongChieu");
            return View();
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult ThemMoi(Ghe ghe)
        {
            ViewBag.id_PhongChieu = new SelectList(db.PhongChieux.ToList(), "id_PhongChieu", "tenPhongChieu");
            db.Ghes.Add(ghe);
            db.SaveChanges();
            return RedirectToAction("Index");

        }
        //Chỉnh sửa phim
        [HttpGet]
        public ActionResult ChinhSua(int IdGhe)
        {
            //lay doi tuong phim theo ma
            Ghe ghe = db.Ghes.SingleOrDefault(n => n.id_Ghe== IdGhe);

            if (ghe == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            ViewBag.id_PhongChieu = new SelectList(db.PhongChieux.ToList(), "id_PhongChieu", "tenPhongChieu");

            return View(ghe);
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult ChinhSua(Ghe ghe)
        {

            //thêm vào csdl
            if (ModelState.IsValid)
            {
                //Thực hiện cập nhật trong Model
                db.Entry(ghe).State = System.Data.Entity.EntityState.Modified;

                db.SaveChanges();

            }
            return RedirectToAction("Index");
        }
        public ActionResult HienThi(int IdGhe)
        {
            Ghe ghe = db.Ghes.SingleOrDefault(n => n.id_Ghe == IdGhe);

            if (ghe == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(ghe);
        }

        [HttpGet]
        public ActionResult XoaPhim(int IdGhe)
        {
            Ghe ghe = db.Ghes.SingleOrDefault(n => n.id_Ghe == IdGhe);

            if (ghe == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(ghe);
        }
        [HttpPost, ActionName("XoaPhim")]
        public ActionResult XacNhanXoa(int IdGhe)
        {
            Ghe ghe = db.Ghes.SingleOrDefault(n => n.id_Ghe == IdGhe);

            if (ghe == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            db.Ghes.Remove(ghe);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        public JsonResult ResetGhe()
        {
            var allSeats = db.Ghes.ToList();
            foreach (var ghe in allSeats)
            {
                ghe.tinhTrang = "Chua";
            }
            db.SaveChanges();
            return Json(new { success = true, message = "Đã reset tất cả ghế!" });
        }
        public ActionResult GiaoDienGhe()
        {
            var seats = db.Ghes.ToList();
            return View(seats);
        }
    }
}