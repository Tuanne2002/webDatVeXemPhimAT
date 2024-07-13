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
    public class QuanLyVeController : Controller
    {
        // GET: QuanLyVe
        DatVeXemPhimATEntities db = new DatVeXemPhimATEntities();

        public ActionResult Index(int? page)
        {
            int pageNumber = page ?? 1;
            int pageSize = 50;
            return View(db.Ves.ToList().OrderBy(n => n.id_Ve).ToPagedList(pageNumber, pageSize));
        }
        //Chỉnh sửa phim
        [HttpGet]
        public ActionResult ChinhSua(int IdVe)
        {
            //lay doi tuong phim theo ma
            Ve ve = db.Ves.SingleOrDefault(n => n.id_Ve == IdVe);

            if (ve == null)
            {
                Response.StatusCode = 404;
                return null;
            }

            return View(ve);
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult ChinhSua(Ve ve)
        {

            //thêm vào csdl
            if (ModelState.IsValid)
            {
                //Thực hiện cập nhật trong Model
                db.Entry(ve).State = System.Data.Entity.EntityState.Modified;

                db.SaveChanges();

            }
            return RedirectToAction("Index");
        }
        public ActionResult HienThi(int IdVe)
        {
            Ve ve = db.Ves.SingleOrDefault(n => n.id_Ve == IdVe);

            if (ve == null)
            {
                Response.StatusCode = 404;
                return null;
            }

            return View(ve);
        }

        [HttpGet]
        public ActionResult Xoa(int IdVe)
        {
            Ve ve = db.Ves.SingleOrDefault(n => n.id_Ve == IdVe);

            if (ve == null)
            {
                Response.StatusCode = 404;
                return null;
            }

            return View(ve);
        }
        [HttpPost, ActionName("Xoa")]
        public ActionResult XacNhanXoa(int IdVe)
        {
            Ve ve = db.Ves.SingleOrDefault(n => n.id_Ve == IdVe);

            if (ve == null)
            {
                Response.StatusCode = 404;
                return null;
            }

           
            db.Ves.Remove(ve);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}