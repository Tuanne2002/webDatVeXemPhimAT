using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebDatVePhim.Models;
using PagedList;
using PagedList.Mvc;

namespace WebDatVePhim.Controllers
{
    public class QuanLyBinhLuanController : Controller
    {
        // GET: QuanLyBinhLuan
        DatVeXemPhimATEntities db = new DatVeXemPhimATEntities();
        public ActionResult Index(int? page)
        {
            int pageNumber = page ?? 1;
            int pageSize = 10;
            return View(db.BinhLuans.ToList().OrderBy(n => n.id_BinhLuan).ToPagedList(pageNumber, pageSize));
        }
        [HttpGet]
        public ActionResult ThemMoi()
        {
            return View();
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult ThemMoi(BinhLuan binhLuan)
        {
            db.BinhLuans.Add(binhLuan);
            db.SaveChanges();
            return RedirectToAction("Index");

        }
        //Chỉnh sửa phim
        [HttpGet]
        public ActionResult ChinhSua(int IdBinhLuan)
        {
            //lay doi tuong phim theo ma
            BinhLuan binhLuan = db.BinhLuans.SingleOrDefault(n => n.id_BinhLuan == IdBinhLuan);

            if (binhLuan == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(binhLuan);
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult ChinhSua(BinhLuan binhLuan)
        {

            //thêm vào csdl
            if (ModelState.IsValid)
            {
                //Thực hiện cập nhật trong Model
                db.Entry(binhLuan).State = System.Data.Entity.EntityState.Modified;

                db.SaveChanges();

            }
            return RedirectToAction("Index");
        }
        public ActionResult HienThi(int IdBinhLuan)
        {
            BinhLuan binhLuan = db.BinhLuans.SingleOrDefault(n => n.id_BinhLuan == IdBinhLuan);

            if (binhLuan == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(binhLuan);
        }

        [HttpGet]
        public ActionResult XoaPhim(int IdBinhLuan)
        {
            BinhLuan binhLuan = db.BinhLuans.SingleOrDefault(n => n.id_BinhLuan == IdBinhLuan);

            if (binhLuan == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(binhLuan);
        }
        [HttpPost, ActionName("XoaPhim")]
        public ActionResult XacNhanXoa(int IdBinhLuan)
        {
            BinhLuan binhLuan = db.BinhLuans.SingleOrDefault(n => n.id_BinhLuan == IdBinhLuan);

            if (binhLuan == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            
            db.BinhLuans.Remove(binhLuan);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}