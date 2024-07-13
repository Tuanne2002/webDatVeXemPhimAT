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
            var movie = db.Phims.FirstOrDefault(m => m.id_Phim == id);
            

            if (movie == null)
            {
                return HttpNotFound();
            }

            return View(movie);
        }
    }
}