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
    public class QuanLyPhimController : Controller
    {
        // GET: QuanLyPhim
        DatVeXemPhimATEntities db = new DatVeXemPhimATEntities();
        public ActionResult Index(int? page)
        {
            int pageNumber = page ?? 1;
            int pageSize = 10;
            return View(db.Phims.ToList().OrderBy(n => n.id_Phim).ToPagedList(pageNumber, pageSize));
        }
        [HttpGet]
        public ActionResult ThemMoi()
        {

            return View();
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult ThemMoi(Phim phim, HttpPostedFileBase fileUpLoad)
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
                phim.anhPhim = fileUpLoad.FileName;
                db.Phims.Add(phim);
                db.SaveChanges();
                return RedirectToAction("Index");

            }
            return View();
        }
        //Chỉnh sửa phim
        [HttpGet]
        public ActionResult ChinhSua(int IdPhim)
        {
            //lay doi tuong phim theo ma
            Phim phim = db.Phims.SingleOrDefault(n => n.id_Phim == IdPhim);

            if(phim == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(phim);
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult ChinhSua(Phim phim)
        {
          
            //thêm vào csdl
            if (ModelState.IsValid)
            {
                //Thực hiện cập nhật trong Model
                db.Entry(phim).State = System.Data.Entity.EntityState.Modified;
                
                db.SaveChanges();

            }
            return RedirectToAction("Index");
        }
        public ActionResult HienThi(int IdPhim)
        {
            Phim phim = db.Phims.SingleOrDefault(n => n.id_Phim == IdPhim);

            if (phim == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(phim);
        }

        [HttpGet]
        public ActionResult XoaPhim( int IdPhim)
        {
            Phim phim = db.Phims.SingleOrDefault(n => n.id_Phim == IdPhim);

            if (phim == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(phim);
        }
        [HttpPost,ActionName("XoaPhim")]
        public ActionResult XacNhanXoa(int IdPhim)
        {
            Phim phim = db.Phims.SingleOrDefault(n => n.id_Phim == IdPhim);
            if(phim == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            db.Phims.Remove(phim);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}