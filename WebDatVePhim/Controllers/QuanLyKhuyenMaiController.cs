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
    public class QuanLyKhuyenMaiController : Controller
    {
        // GET: KhuyenMai
        DatVeXemPhimATEntities db = new DatVeXemPhimATEntities();
        public ActionResult Index(int? page)
        {
            int pageNumber = page ?? 1;
            int pageSize = 10;
            return View(db.KhuyenMais.ToList().OrderBy(n => n.id_KhuyenMai).ToPagedList(pageNumber, pageSize));
        }
        [HttpGet]
        public ActionResult ThemMoi()
        {

            return View();
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult ThemMoi(KhuyenMai khuyenMai, HttpPostedFileBase fileUpLoad)
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
                khuyenMai.hinhAnh = fileUpLoad.FileName;
                db.KhuyenMais.Add(khuyenMai);
                db.SaveChanges();
                return RedirectToAction("Index");

            }
            return View();
        }
        //Chỉnh sửa phim
        [HttpGet]
        public ActionResult ChinhSua(int IdKhuyenMai)
        {
            //lay doi tuong phim theo ma
            KhuyenMai khuyenMai = db.KhuyenMais.SingleOrDefault(n => n.id_KhuyenMai == IdKhuyenMai);

            if (khuyenMai == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(khuyenMai);
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult ChinhSua(KhuyenMai khuyenMai)
        {

            //thêm vào csdl
            if (ModelState.IsValid)
            {
                //Thực hiện cập nhật trong Model
                db.Entry(khuyenMai).State = System.Data.Entity.EntityState.Modified;

                db.SaveChanges();

            }
            return RedirectToAction("Index");
        }
        public ActionResult HienThi(int IdKhuyenMai)
        {
            KhuyenMai khuyenMai = db.KhuyenMais.SingleOrDefault(n => n.id_KhuyenMai == IdKhuyenMai);

            if (khuyenMai == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(khuyenMai);
        }

        [HttpGet]
        public ActionResult XoaPhim(int IdKhuyenMai)
        {
            KhuyenMai khuyenMai = db.KhuyenMais.SingleOrDefault(n => n.id_KhuyenMai == IdKhuyenMai);

            if (khuyenMai == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(khuyenMai);
        }
        [HttpPost, ActionName("XoaPhim")]
        public ActionResult XacNhanXoa(int IdKhuyenMai)
        {
            KhuyenMai khuyenMai = db.KhuyenMais.SingleOrDefault(n => n.id_KhuyenMai == IdKhuyenMai);
            if (khuyenMai == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            db.KhuyenMais.Remove(khuyenMai);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}