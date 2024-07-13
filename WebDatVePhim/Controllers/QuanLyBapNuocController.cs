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
    public class QuanLyBapNuocController : Controller
    {
        // GET: BapNuoc
        DatVeXemPhimATEntities db = new DatVeXemPhimATEntities();
        public ActionResult Index(int? page)
        {
            int pageNumber = page ?? 1;
            int pageSize = 10;
            return View(db.BapNuocs.ToList().OrderBy(n => n.id_BapNuoc).ToPagedList(pageNumber, pageSize));
        }
        [HttpGet]
        public ActionResult ThemMoi()
        {

            return View();
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult ThemMoi(BapNuoc bapNuoc, HttpPostedFileBase fileUpLoad)
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
                bapNuoc.hinhAnh = fileUpLoad.FileName;
                db.BapNuocs.Add(bapNuoc);
                db.SaveChanges();
                return RedirectToAction("Index");

            }
            return View();
        }
        //Chỉnh sửa phim
        [HttpGet]
        public ActionResult ChinhSua(int IdBapNuoc)
        {
            //lay doi tuong phim theo ma
            BapNuoc bapNuoc = db.BapNuocs.SingleOrDefault(n => n.id_BapNuoc == IdBapNuoc);

            if (bapNuoc == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(bapNuoc);
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult ChinhSua(BapNuoc bapNuoc)
        {

            //thêm vào csdl
            if (ModelState.IsValid)
            {
                //Thực hiện cập nhật trong Model
                db.Entry(bapNuoc).State = System.Data.Entity.EntityState.Modified;

                db.SaveChanges();

            }
            return RedirectToAction("Index");
        }
        public ActionResult HienThi(int IdBapNuoc)
        {
            BapNuoc bapNuoc = db.BapNuocs.SingleOrDefault(n => n.id_BapNuoc == IdBapNuoc);

            if (bapNuoc == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(bapNuoc);
        }

        [HttpGet]
        public ActionResult XoaPhim(int IdBapNuoc)
        {
            BapNuoc bapNuoc = db.BapNuocs.SingleOrDefault(n => n.id_BapNuoc == IdBapNuoc);

            if (bapNuoc == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(bapNuoc);
        }
        [HttpPost, ActionName("XoaPhim")]
        public ActionResult XacNhanXoa(int IdBapNuoc)
        {
            BapNuoc bapNuoc = db.BapNuocs.SingleOrDefault(n => n.id_BapNuoc == IdBapNuoc);
            if (bapNuoc == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            db.BapNuocs.Remove(bapNuoc);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
