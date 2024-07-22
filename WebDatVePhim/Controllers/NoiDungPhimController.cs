using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebDatVePhim.Models;

namespace WebDatVePhim.Controllers
{
    
    public class NoiDungPhimController : Controller 
    {
        DatVeXemPhimATEntities db = new DatVeXemPhimATEntities();
        // GET: NoiDungPhim
        public ActionResult NoiDungPhim()
        {
            return View();
        }
        public ActionResult LichChieu()
        {
            return View();
        }
        public ActionResult Details(int id)
        {
            var movie = db.Phims.Include("BinhLuans.NguoiDung").FirstOrDefault(p => p.id_Phim == id);
            if (movie == null)
            {
                return HttpNotFound();
            }
            return View(movie);
        }

        [HttpPost]
        [Authorize]
        public ActionResult AddComment(int id_Phim, string noiDung, int soSao)
        {
            if (ModelState.IsValid)
            {
                var taiKhoan = Session["TaiKhoan"] as string;
                if (string.IsNullOrEmpty(taiKhoan))
                {
                    return RedirectToAction("Login", "TrangAcount");
                }

                var user = db.NguoiDungs.FirstOrDefault(u => u.taiKhoan == taiKhoan);
                if (user != null)
                {
                    var comment = new BinhLuan
                    {
                        id_Phim = id_Phim,
                        id_NguoiDung = user.id_NguoiDung,
                        noiDung = noiDung,
                        soSao = soSao
                    };

                    db.BinhLuans.Add(comment);
                    db.SaveChanges();
                    return RedirectToAction("Details", new { id = id_Phim });
                }
            }

            return RedirectToAction("Details", new { id = id_Phim });
        }



    }
}