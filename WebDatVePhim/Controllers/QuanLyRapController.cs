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
    public class QuanLyRapController : Controller
    {
        // GET: RapPhim
        DatVeXemPhimATEntities db = new DatVeXemPhimATEntities();
        public ActionResult Index(int? page)
        {
            int pageNumber = page ?? 1;
            int pageSize = 10;
            return View(db.Raps.ToList().OrderBy(n => n.id_Rap).ToPagedList(pageNumber, pageSize));
        }
        [HttpGet]
        public ActionResult ThemMoi()
        {
            ViewBag.id_loaiRap = new SelectList(db.LoaiRaps.ToList(), "id_loaiRap", "tenLoaiRap");
            return View();
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult ThemMoi(Rap rap, HttpPostedFileBase fileUpLoad)
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
                ViewBag.id_loaiRap = new SelectList(db.LoaiRaps.ToList(), "id_loaiRap", "tenLoaiRap");
                
                db.Raps.Add(rap);
                db.SaveChanges();
                return RedirectToAction("Index");

            }
            return View();

        }
        //Chỉnh sửa phim
        [HttpGet]
        public ActionResult ChinhSua(int IdRap)
        {
            //lay doi tuong phim theo ma
            Rap rap = db.Raps.SingleOrDefault(n => n.id_Rap == IdRap);

            if (rap == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            ViewBag.id_loaiRap = new SelectList(db.LoaiRaps.ToList(), "id_loaiRap", "tenLoaiRap");

            return View(rap);
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult ChinhSua(Rap rap)
        {

            //thêm vào csdl
            if (ModelState.IsValid)
            {
                //Thực hiện cập nhật trong Model
                db.Entry(rap).State = System.Data.Entity.EntityState.Modified;

                db.SaveChanges();

            }
            return RedirectToAction("Index");
        }
        public ActionResult HienThi(int IdRap)
        {
            Rap rap = db.Raps.SingleOrDefault(n => n.id_Rap == IdRap);

            if (rap == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(rap);
        }

        [HttpGet]
        public ActionResult XoaPhim(int IdRap)
        {
            Rap rap = db.Raps.SingleOrDefault(n => n.id_Rap == IdRap);

            if (rap == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(rap);
        }
        [HttpPost, ActionName("XoaPhim")]
        public ActionResult XacNhanXoa(int IdRap)
        {
            Rap rap = db.Raps.SingleOrDefault(n => n.id_Rap == IdRap);
            if (rap == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            db.Raps.Remove(rap);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
       
    }
}