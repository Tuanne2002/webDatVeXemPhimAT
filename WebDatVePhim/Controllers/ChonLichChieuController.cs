using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebDatVePhim.Models;
using System.Data.Entity;

namespace WebDatVePhim.Controllers
{
    public class ChonLichChieuController : Controller
    {
        // GET: ChonLichChieu
        DatVeXemPhimATEntities db = new DatVeXemPhimATEntities();

        public ActionResult Index(int phimId)
        {

            var phim = db.Phims.Find(phimId);
            var lichChieuList = db.LichChieuPhims.Where(l => l.id_Phim == phimId).ToList();
            ViewBag.Phim = phim;
            var rapList = db.Raps.ToList();
            ViewBag.RapList = rapList;


            return View(lichChieuList);
        }
       
        [HttpGet]
        public ActionResult FilterLichChieuByRap(int rapId)
        {

            List<LichChieuPhim> lichChieuList;
            
            if (rapId == 0)
            {
                // Lấy lịch chiếu của ngày gần nhất so với ngày hiện tại
                var today = DateTime.Today;
                lichChieuList = db.LichChieuPhims
                                  .Include(l => l.PhongChieu)
                                  .Where(l => l.ngayChieu >= today)
                                  .OrderBy(l => l.ngayChieu)
                                  .ThenBy(l => l.thoiGianBatDau)
                                  .ToList();
            }
            else
            {
                // Lấy danh sách các PhongChieu có id_Rap trùng với rapId
                var phongChieuList = db.PhongChieux.Where(pc => pc.id_Rap == rapId).ToList();

                // Lấy danh sách các LichChieuPhim thuộc các PhongChieu này
                lichChieuList = new List<LichChieuPhim>();
                foreach (var phongChieu in phongChieuList)
                {
                    var lichChieuCuaPhong = db.LichChieuPhims
                                              .Where(l => l.id_PhongChieu == phongChieu.id_PhongChieu)
                                              .ToList();
                    lichChieuList.AddRange(lichChieuCuaPhong);
                }
            }


            return PartialView("_LichChieuPartial", lichChieuList);
         
        }
        [HttpPost]
        public ActionResult ClearPhimSelections()
        {
            // Clear the session variables for selected popcorn and drinks
            Session.Remove("PhimId");

            return Json(new { success = true });
        }

    }
}