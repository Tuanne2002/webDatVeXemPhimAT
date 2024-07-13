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
    public class QuanLyLichChieuController : Controller
    {
        // GET: QuanLyLichChieu
        DatVeXemPhimATEntities db = new DatVeXemPhimATEntities();
        public ActionResult Index(int? page)
        {
            int pageNumber = page ?? 1;
            int pageSize = 10;
            return View(db.LichChieuPhims.ToList().OrderBy(n => n.id_LichChieuPhim).ToPagedList(pageNumber, pageSize));
        }
        [HttpGet]
        public ActionResult ThemMoi()
        {
            //đưa dữ liệu vào dropdownlist
            ViewBag.id_Phim = new SelectList(db.Phims.ToList(), "id_Phim", "tenPhim");
            ViewBag.id_PhongChieu = new SelectList(db.PhongChieux.ToList(), "id_PhongChieu", "tenPhongChieu");
            ViewBag.id_Rap = new SelectList(db.Raps, "id_Rap", "tenRap");
            return View();
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult ThemMoi(LichChieuPhim lichChieuPhim)
        {
            ViewBag.id_Phim = new SelectList(db.Phims.ToList(), "id_Phim", "tenPhim");
            ViewBag.id_PhongChieu = new SelectList(db.PhongChieux.ToList(), "id_PhongChieu", "tenPhongChieu");
            ViewBag.id_Rap = new SelectList(db.Raps, "id_Rap", "tenRap");
            db.LichChieuPhims.Add(lichChieuPhim);
            db.SaveChanges();
            return RedirectToAction("Index");

        }
        public ActionResult GetPhongChieuByRap(int id_Rap)
        {
            var phongChieus = db.PhongChieux
                                .Where(pc => pc.id_Rap == id_Rap)
                                .Select(pc => new
                                {
                                    Value = pc.id_PhongChieu,
                                    Text = pc.tenPhongChieu
                                }).ToList();

            return Json(phongChieus, JsonRequestBehavior.AllowGet);
        }
        //Chỉnh sửa phim
        [HttpGet]
        public ActionResult ChinhSua(int IdLichChieuPhim)
        {
            //lay doi tuong phim theo ma
            LichChieuPhim lichChieuPhim = db.LichChieuPhims.SingleOrDefault(n => n.id_LichChieuPhim == IdLichChieuPhim);

            if (lichChieuPhim == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            ViewBag.id_Phim = new SelectList(db.Phims.ToList(), "id_Phim", "tenPhim");
            ViewBag.id_Rap = new SelectList(db.Raps, "id_Rap", "tenRap");
            ViewBag.id_PhongChieu = new SelectList(db.PhongChieux.ToList(), "id_PhongChieu", "tenPhongChieu");

            return View(lichChieuPhim);
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult ChinhSua(LichChieuPhim lichChieuPhim)
        {

            //thêm vào csdl
            if (ModelState.IsValid)
            {
                //Thực hiện cập nhật trong Model
                db.Entry(lichChieuPhim).State = System.Data.Entity.EntityState.Modified;

                db.SaveChanges();

            }
            return RedirectToAction("Index");
        }
        public ActionResult HienThi(int IdLichChieuPhim)
        {
            LichChieuPhim lichChieuPhim = db.LichChieuPhims.SingleOrDefault(n => n.id_LichChieuPhim == IdLichChieuPhim);

            if (lichChieuPhim == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(lichChieuPhim);
        }

        [HttpGet]
        public ActionResult XoaPhim(int IdLichChieuPhim)
        {
            LichChieuPhim lichChieuPhim = db.LichChieuPhims.SingleOrDefault(n => n.id_LichChieuPhim == IdLichChieuPhim);

            if (lichChieuPhim == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(lichChieuPhim);
        }
        [HttpPost, ActionName("XoaPhim")]
        public ActionResult XacNhanXoa(int IdLichChieuPhim)
        {
            LichChieuPhim lichChieuPhim = db.LichChieuPhims.SingleOrDefault(n => n.id_LichChieuPhim == IdLichChieuPhim);

            if (lichChieuPhim == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            db.LichChieuPhims.Remove(lichChieuPhim);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}