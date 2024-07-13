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
    public class QuanLyPhongChieuController : Controller
    {
        // GET: QuanLyPhongChieu
        DatVeXemPhimATEntities db = new DatVeXemPhimATEntities();
        public ActionResult Index(int? page)
        {
            int pageNumber = page ?? 1;
            int pageSize = 10;
            return View(db.PhongChieux.ToList().OrderBy(n => n.id_PhongChieu).ToPagedList(pageNumber, pageSize));
        }
        [HttpGet]
        public ActionResult ThemMoi()
        {
            //đưa dữ liệu vào dropdownlist
            ViewBag.id_Rap = new SelectList( db.Raps.ToList(),"id_Rap","tenRap");
            return View();
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult ThemMoi(PhongChieu phongChieu)
        {
            ViewBag.id_Rap = new SelectList(db.Raps.ToList(), "id_Rap", "tenRap");
            db.PhongChieux.Add(phongChieu);
            db.SaveChanges();
            return RedirectToAction("Index");

        }
        //Chỉnh sửa phim
        [HttpGet]
        public ActionResult ChinhSua(int IdPhongChieu)
        {
            //lay doi tuong phim theo ma
            PhongChieu phongChieu = db.PhongChieux.SingleOrDefault(n => n.id_PhongChieu == IdPhongChieu);

            if (phongChieu == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            ViewBag.id_Rap = new SelectList(db.Raps.ToList(), "id_Rap", "tenRap");

            return View(phongChieu);
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult ChinhSua(PhongChieu phongChieu)
        {

            //thêm vào csdl
            if (ModelState.IsValid)
            {
                //Thực hiện cập nhật trong Model
                db.Entry(phongChieu).State = System.Data.Entity.EntityState.Modified;

                db.SaveChanges();

            }
            return RedirectToAction("Index");
        }
        public ActionResult HienThi(int IdPhongChieu)
        {
            PhongChieu phongChieu = db.PhongChieux.SingleOrDefault(n => n.id_PhongChieu == IdPhongChieu);

            if (phongChieu == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(phongChieu);
        }

        [HttpGet]
        public ActionResult XoaPhim(int IdPhongChieu)
        {
            PhongChieu phongChieu = db.PhongChieux.SingleOrDefault(n => n.id_PhongChieu == IdPhongChieu);

            if (phongChieu == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(phongChieu);
        }
        [HttpPost, ActionName("XoaPhim")]
        public ActionResult XacNhanXoa(int IdPhongChieu)
        {
            PhongChieu phongChieu = db.PhongChieux.SingleOrDefault(n => n.id_PhongChieu == IdPhongChieu);

            if (phongChieu == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            db.PhongChieux.Remove(phongChieu);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
       
    }
}