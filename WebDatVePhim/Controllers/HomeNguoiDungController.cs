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
        public ActionResult Search(string searchKeyword)
        {
            if (string.IsNullOrEmpty(searchKeyword))
            {
                return View("Search", new List<Phim>());
            }

            var result = db.Phims.Where(m => m.tenPhim.Contains(searchKeyword)).ToList();
            return View("Search", result);
        }
        public PartialViewResult SearchPartial(string searchKeyword)
        {
            if (string.IsNullOrEmpty(searchKeyword))
            {
                return PartialView(new List<Phim>());
            }

            var result = db.Phims.Where(m => m.tenPhim.Contains(searchKeyword)).ToList();
            return PartialView(result);
        }

        public JsonResult GetMovieSuggestions(string searchKeyword)
        {
            var suggestions = db.Phims
                                .Where(m => m.tenPhim.Contains(searchKeyword))
                                .Select(m => new
                                {
                                    m.tenPhim,
                                    m.anhPhim,
                                    m.id_Phim
                                })
                                .ToList();

            var result = suggestions.Select(m => new
            {
                m.tenPhim,
                m.anhPhim,
                DetailsUrl = Url.Action("Details", "NoiDungPhim", new { id = m.id_Phim })
            });

            return Json(result, JsonRequestBehavior.AllowGet);
        }


        

    }
}
