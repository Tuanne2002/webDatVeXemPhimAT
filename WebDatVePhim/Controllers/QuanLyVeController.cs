using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebDatVePhim.Models;
using PagedList.Mvc;
using PagedList;
using System.Globalization;

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
        public ActionResult ThongKeDoanhThu()
        {
            return View();
        }
        public ActionResult RevenueYear( int year)
        {

            var tickets = db.Ves.Where(v => v.ngayDat.HasValue && v.ngayDat.Value.Year == year).ToList();
            var totalRevenue = tickets.Sum(v => (double?)v.soTien) ?? 0;

            //var totalPopcornCount = db.Ves
            //    .Where(v => v.ngayDat == selectedDate && v.chiTietBapNuoc != null)
            //    .Count();
            var totalKhuyenMaiCount = tickets.Count(v => v.KhuyenMai != null);

            int totalGhe = 0;
            foreach (var ticket in tickets)
            {
                // Split chuỗi viTriGhe thành mảng các vị trí ghế
                var viTriGheArray = ticket.viTriGhe.Split(',');
                // Đếm số lượng vị trí ghế
                totalGhe += viTriGheArray.Length;
            }
            int totalBapNuocCount = 0;
            foreach (var ticket in tickets)
            {
                if (ticket.chiTietBapNuoc != null)
                {
                    var chiTietBapNuocArray = ticket.chiTietBapNuoc.Split(',');
                    totalBapNuocCount += chiTietBapNuocArray.Length;
                }
                else
                {
                    totalBapNuocCount += 0;
                }
            }
            var totalPopcornRevenue = db.Ves.Sum(v => (double?)v.tongTienBapNuoc) ?? 0;

            var model = new RevenueViewModel
            {
                SelectedYear = year,
                TotalRevenue = totalRevenue,
                TotalGheCount = totalGhe,
                TotalPopcornRevenue = totalPopcornRevenue,
                TotalKhuyenMaiCount = totalKhuyenMaiCount,
                TotalBapNuocCount = totalBapNuocCount
            };

            return PartialView("_RevenueYear", model);

        }
        public ActionResult BieuDoRevenueYear(int year)
        {
            var tickets = db.Ves
                .Where(v => v.ngayDat.HasValue && v.ngayDat.Value.Year == year)
                .ToList();

            // Tính tổng doanh thu theo tháng
            var monthlyRevenue = tickets
                .GroupBy(v => v.ngayDat.Value.Month)
                .Select(g => new
                {
                    Month = g.Key,
                    TotalRevenue = g.Sum(v => (double?)v.soTien) ?? 0
                })
                .OrderBy(m => m.Month)
                .ToList();

            return Json(monthlyRevenue, JsonRequestBehavior.AllowGet);
        }
        public ActionResult RevenueMonth(int? month, int? year)
        {
            int selectedMonth = month ?? DateTime.Today.Month;
            int selectedYear = year ?? DateTime.Today.Year;
            DateTime firstDayOfMonth = new DateTime(selectedYear, selectedMonth, 1);
            DateTime lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);
            var tickets = db.Ves
            .Where(v => v.ngayDat >= firstDayOfMonth && v.ngayDat <= lastDayOfMonth)
            .ToList();
            var totalRevenue = tickets.Sum(v => (double?)v.soTien) ?? 0;

          
            var totalKhuyenMaiCount = tickets.Count(v=>v.KhuyenMai != null);
         
            int totalGhe = 0;
            foreach (var ticket in tickets)
            {
                // Split chuỗi viTriGhe thành mảng các vị trí ghế
                var viTriGheArray = ticket.viTriGhe.Split(',');
                // Đếm số lượng vị trí ghế
                totalGhe += viTriGheArray.Length;
            }
            int totalBapNuocCount = 0;
            foreach (var ticket in tickets)
            {
                if(ticket.chiTietBapNuoc != null)
                {
                    var chiTietBapNuocArray = ticket.chiTietBapNuoc.Split(',');
                    totalBapNuocCount += chiTietBapNuocArray.Length;
                }
                else
                {
                    totalBapNuocCount += 0;
                }
            }
            var totalPopcornRevenue = db.Ves .Sum(v => (double?)v.tongTienBapNuoc) ?? 0;

            var model = new RevenueViewModel
            {
                SelectedMonth = selectedMonth,
                SelectedYear = selectedYear,
                TotalRevenue = totalRevenue,
                TotalGheCount = totalGhe,
                TotalPopcornRevenue = totalPopcornRevenue,
                TotalKhuyenMaiCount=totalKhuyenMaiCount,
                TotalBapNuocCount = totalBapNuocCount
            };

            return PartialView("_RevenueMonth", model);
        }
        public ActionResult MonthlyRevenueSummary(int month, int year)
        {
            var tickets = db.Ves
                .Where(v => v.ngayDat.HasValue && v.ngayDat.Value.Month == month && v.ngayDat.Value.Year == year)
                .ToList();

            var dailyRevenue = tickets
                .GroupBy(v => v.ngayDat.Value.Day)
                .Select(g => new
                {
                    Day = g.Key,
                    TotalRevenue = g.Sum(v => (double?)v.soTien) ?? 0
                })
                .OrderBy(d => d.Day)
                .ToList();

            return Json(dailyRevenue, JsonRequestBehavior.AllowGet);
        }
        public ActionResult RevenueSummary(DateTime? date)
        {
            if (!date.HasValue)
            {
                date = DateTime.Today; // Default to today's date if no date is provided
            }
            DateTime selectedDate = date.Value;

            var totalRevenue = db.Ves
                .Where(v => v.ngayDat == selectedDate)
                .Sum(v => (double?)v.soTien) ?? 0;

            //var totalPopcornCount = db.Ves
            //    .Where(v => v.ngayDat == selectedDate && v.chiTietBapNuoc != null)
            //    .Count();
            var totalKhuyenMaiCount = db.Ves
               .Where(v => v.ngayDat == selectedDate && v.KhuyenMai != null)
               .Count();
            var totalGheCount = db.Ves
              .Where(v => v.ngayDat == selectedDate).ToList();
            int totalGhe = 0;
            foreach (var ticket in totalGheCount)
            {
                // Split chuỗi viTriGhe thành mảng các vị trí ghế
                var viTriGheArray = ticket.viTriGhe.Split(',');
                // Đếm số lượng vị trí ghế
                totalGhe += viTriGheArray.Length;
            }
            int totalBapNuocCount = 0;
            foreach (var ticket in totalGheCount)
            {
                if (ticket.chiTietBapNuoc != null)
                {
                    var chiTietBapNuocArray = ticket.chiTietBapNuoc.Split(',');
                    totalBapNuocCount += chiTietBapNuocArray.Length;
                }
                else
                {
                    totalBapNuocCount += 0;
                }
            }

            var totalPopcornRevenue = db.Ves
            .Where(v => v.ngayDat == selectedDate && v.chiTietBapNuoc != null)
            .Sum(v => (double?)v.tongTienBapNuoc) ?? 0;

            var model = new RevenueViewModel
            {
                SelectedDate = selectedDate,
                TotalRevenue = totalRevenue,
                TotalGheCount = totalGhe,
                TotalPopcornRevenue = totalPopcornRevenue,
                TotalKhuyenMaiCount = totalKhuyenMaiCount,
                TotalBapNuocCount=totalBapNuocCount
            };

            return PartialView("_RevenueSummary", model);
        }
        public class RevenueViewModel
        {
            public int SelectedMonth { get; set; }
            public int SelectedYear { get; set; }
            public DateTime SelectedDate { get; set; }
            public double TotalRevenue { get; set; }
            public int TotalGheCount { get; set; }
            public double TotalPopcornRevenue { get; set; }
            public int TotalKhuyenMaiCount { get; set; }
            public int TotalBapNuocCount { get; set; }
        }

    }
}