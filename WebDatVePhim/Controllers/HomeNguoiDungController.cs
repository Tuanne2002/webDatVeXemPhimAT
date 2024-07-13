using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebDatVePhim.Models;

namespace WebDatVePhim.Controllers
{
    public class HomeNguoiDungController : Controller
    {
        DatVeXemPhimATEntities db = new DatVeXemPhimATEntities();
        // GET: Home
        public ActionResult Index()
        {
            return View();
        }

        public PartialViewResult PhimMoiPartial()
        {
            var listPhimMoi = db.Phims.ToList();
           
            return PartialView(listPhimMoi);
        }
    }
}
