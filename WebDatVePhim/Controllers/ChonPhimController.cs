using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebDatVePhim.Models;

namespace WebDatVePhim.Controllers
{
    public class ChonPhimController : Controller
    {
        // GET: ChonPhim
        DatVeXemPhimATEntities db = new DatVeXemPhimATEntities();
        public ActionResult Index()
        {
            var phimList = db.Phims.ToList();
            return View(phimList);
        }
    }
}