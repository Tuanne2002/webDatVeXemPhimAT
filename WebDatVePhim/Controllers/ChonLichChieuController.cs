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
            //var lichChieuList = db.LichChieuPhims.Where(l => l.id_Phim == phimId).ToList();
            var lichChieuList = db.LichChieuPhims
                .Include(l => l.PhongChieu)
                .Include(l => l.PhongChieu.Ghes)
                .Where(l => l.id_Phim == phimId)
                .ToList();
            ViewBag.Phim = phim;
            var rapList = db.Raps.ToList();
            ViewBag.RapList = rapList;
            ViewBag.SelectedMovieId = phimId;
            var seatInfo = new Dictionary<int, string>();
            foreach (var lichChieu in lichChieuList)
            {
                var phongChieu = lichChieu.PhongChieu;
                var totalSeats = phongChieu.Ghes.Count();
                var availableSeats = phongChieu.Ghes.Count(g => g.tinhTrang == "Chua");
                seatInfo[phongChieu.id_PhongChieu] = $"{availableSeats}/{totalSeats}";
            }
            ViewBag.SeatInfo = seatInfo;
            return View(lichChieuList);
        }
       
        [HttpGet]
        public ActionResult FilterLichChieuByRap(int rapId, int movieId, string currentDate)
        {
            DateTime today = DateTime.Parse(currentDate);
            List<LichChieuPhim> lichChieuList;
            //var idPhongChieu = (int?)Session["idPhongChieu"];
            if (rapId == 0)
            {
                lichChieuList = db.LichChieuPhims
                    .Include(l => l.PhongChieu)
                    .Include(l => l.PhongChieu.Ghes)
                    .Where(l => l.ngayChieu >= today && l.id_Phim == movieId)
                    .OrderBy(l => l.ngayChieu)
                    .ThenBy(l => l.thoiGianBatDau)
                    .ToList();
            }
            else
            {
                lichChieuList = db.LichChieuPhims
                    .Include(l => l.PhongChieu)
                    .Include(l => l.PhongChieu.Ghes)
                    .Where(l => l.PhongChieu.id_Rap == rapId && l.ngayChieu >= today && l.id_Phim == movieId)
                    .OrderBy(l => l.ngayChieu)
                    .ThenBy(l => l.thoiGianBatDau)
                    .ToList();
            }
            var seatInfo = new Dictionary<int, string>();
            foreach (var lichChieu in lichChieuList)
            {
                var phongChieu = lichChieu.PhongChieu;
                var totalSeats = phongChieu.Ghes.Count();
                var availableSeats = phongChieu.Ghes.Count(g => g.tinhTrang == "Chua");
                seatInfo[phongChieu.id_PhongChieu] = $"{availableSeats}/{totalSeats}";
            }
            ViewBag.SeatInfo = seatInfo;
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