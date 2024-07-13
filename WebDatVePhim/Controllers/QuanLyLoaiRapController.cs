using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebDatVePhim.Models;
using PagedList;
using PagedList.Mvc;
using System.IO;

namespace WebDatVePhim.Controllers
{
    public class QuanLyLoaiRapController : Controller
    {
        DatVeXemPhimATEntities db = new DatVeXemPhimATEntities();
        // GET: QuanLyLoaiRap
        public ActionResult Index(int? page)
        {
            int pageNumber = page ?? 1;
            int pageSize = 10;
            return View(db.LoaiRaps.ToList().OrderBy(n => n.id_loaiRap).ToPagedList(pageNumber, pageSize));
        }
        [HttpGet]
        public ActionResult ThemMoi()
        {

            return View();
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult ThemMoi(LoaiRap loaiRap, HttpPostedFileBase fileUpLoad)
        {
            //kiểm tra đường dẫn ảnh 
            if (fileUpLoad == null)
            {
                ViewBag.ThongBao = "Vui lòng chọn hình ảnh";
                return View();
            }
            //thêm vào csdl
            if (ModelState.IsValid)
            {
                // lưu tên file
                var fileName = Path.GetFileName(fileUpLoad.FileName);
                //lưu đường file
                var path = Path.Combine(Server.MapPath("~/img"), fileName);
                // kiểm tra hình ảnh đã tồn tại chưa
                if (System.IO.File.Exists(path))
                {
                    ViewBag.ThongBao = "Hình ảnh đã tồn tại";
                }
                else
                {
                    fileUpLoad.SaveAs(path);
                }
                loaiRap.hinhAnh = fileUpLoad.FileName;
                db.LoaiRaps.Add(loaiRap);
                db.SaveChanges();
                return RedirectToAction("Index");

            }
            return View();
        }
        //Chỉnh sửa phim
        [HttpGet]
        public ActionResult ChinhSua(int IdLoaiRap)
        {
            //lay doi tuong phim theo ma
            LoaiRap loaiRap = db.LoaiRaps.SingleOrDefault(n => n.id_loaiRap == IdLoaiRap);

            if (loaiRap == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(loaiRap);
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult ChinhSua(LoaiRap loaiRap)
        {

            //thêm vào csdl
            if (ModelState.IsValid)
            {
                //Thực hiện cập nhật trong Model
                db.Entry(loaiRap).State = System.Data.Entity.EntityState.Modified;

                db.SaveChanges();

            }
            return RedirectToAction("Index");
        }
        public ActionResult HienThi(int IdLoaiRap)
        {
            LoaiRap loaiRap = db.LoaiRaps.SingleOrDefault(n => n.id_loaiRap == IdLoaiRap);

            if (loaiRap == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(loaiRap);
        }

        [HttpGet]
        public ActionResult Xoa(int IdLoaiRap)
        {
            LoaiRap loaiRap = db.LoaiRaps.SingleOrDefault(n => n.id_loaiRap == IdLoaiRap);

            if (loaiRap == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(loaiRap);
        }
        [HttpPost, ActionName("Xoa")]
        public ActionResult XacNhanXoa(int IdLoaiRap)
        {

            LoaiRap loaiRap = db.LoaiRaps.SingleOrDefault(n => n.id_loaiRap == IdLoaiRap);

            if (loaiRap == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            db.LoaiRaps.Remove(loaiRap);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}