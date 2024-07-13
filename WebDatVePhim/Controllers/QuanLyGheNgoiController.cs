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
    public class QuanLyGheNgoiController : Controller
    {
        // GET: QuanLyGheNgoi
        DatVeXemPhimATEntities db = new DatVeXemPhimATEntities();

        public ActionResult Index(int? page)
        {
            int pageNumber = page ?? 1;
            int pageSize = 50;
            return View(db.Ghes.ToList().OrderBy(n => n.id_Ghe).ToPagedList(pageNumber, pageSize));
            //var seats = db.Ghes.ToList();
            //return View(seats);
        }
        [HttpGet]
        public ActionResult ThemMoi()
        {
            //đưa dữ liệu vào dropdownlist
            ViewBag.id_PhongChieu = new SelectList(db.PhongChieux.ToList(), "id_PhongChieu", "tenPhongChieu");
            return View();
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult ThemMoi(Ghe ghe)
        {
            ViewBag.id_PhongChieu = new SelectList(db.PhongChieux.ToList(), "id_PhongChieu", "tenPhongChieu");
            db.Ghes.Add(ghe);
            db.SaveChanges();
            return RedirectToAction("Index");

        }
        //Chỉnh sửa phim
        [HttpGet]
        public ActionResult ChinhSua(int IdGhe)
        {
            //lay doi tuong phim theo ma
            Ghe ghe = db.Ghes.SingleOrDefault(n => n.id_Ghe== IdGhe);

            if (ghe == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            ViewBag.id_PhongChieu = new SelectList(db.PhongChieux.ToList(), "id_PhongChieu", "tenPhongChieu");

            return View(ghe);
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult ChinhSua(Ghe ghe)
        {

            //thêm vào csdl
            if (ModelState.IsValid)
            {
                //Thực hiện cập nhật trong Model
                db.Entry(ghe).State = System.Data.Entity.EntityState.Modified;

                db.SaveChanges();

            }
            return RedirectToAction("Index");
        }
        public ActionResult HienThi(int IdGhe)
        {
            Ghe ghe = db.Ghes.SingleOrDefault(n => n.id_Ghe == IdGhe);

            if (ghe == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(ghe);
        }

        [HttpGet]
        public ActionResult XoaPhim(int IdGhe)
        {
            Ghe ghe = db.Ghes.SingleOrDefault(n => n.id_Ghe == IdGhe);

            if (ghe == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(ghe);
        }
        [HttpPost, ActionName("XoaPhim")]
        public ActionResult XacNhanXoa(int IdGhe)
        {
            Ghe ghe = db.Ghes.SingleOrDefault(n => n.id_Ghe == IdGhe);

            if (ghe == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            db.Ghes.Remove(ghe);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        //[HttpPost]
        //public JsonResult DatGhe(List<int> gheIds)
        //{
        //    var result = new List<object>();

        //    foreach (var gheId in gheIds)
        //    {
        //        var ghe = db.Ghes.Find(gheId);

        //        if (ghe != null && ghe.tinhTrang != "DaDat")
        //        {
        //            ghe.tinhTrang = "DaDat";
        //            db.SaveChanges();
        //            result.Add(new { success = true, gheId = gheId, viTriGhe = ghe.soHangGhe + ghe.soGheTrongHang });
        //        }
        //        else
        //        {
        //            result.Add(new { success = false, gheId = gheId, message = "Ghế này đã được đặt rồi!" });
        //        }
        //    }

        //    return Json(result);
        //}

        //[HttpPost]
        //public JsonResult LuuThongTinVe(List<GheViTri> gheViTriList)
        //{
        //    try
        //    {
        //        foreach (var gheViTri in gheViTriList)
        //        {
        //            var ve = new Ve()
        //            {
        //                id_Ghe = gheViTri.GheId,
        //                viTriGhe = gheViTri.ViTri,
        //                ngayDat = DateTime.Now,
        //                gioDat = DateTime.Now.TimeOfDay,
        //                maDat = Guid.NewGuid().ToString()
        //                // thêm các thuộc tính khác nếu cần
        //            };
        //            db.Ves.Add(ve);
        //        }
        //        db.SaveChanges();

        //        return Json(new { success = true });
        //    }
        //    catch (Exception ex)
        //    {
        //        return Json(new { success = false, message = ex.Message });
        //    }
        //}

        //public ActionResult ThongTinVe(string gheIds)
        //{
        //    var gheIdList = gheIds.Split(',').Select(int.Parse).ToList();
        //    var gheList = db.Ghes.Where(g => gheIdList.Contains(g.id_Ghe)).ToList();
        //    return View(gheList);
        //}
        //public class GheViTri
        //{
        //    public int GheId { get; set; }
        //    public string ViTri { get; set; }
        //}
        public JsonResult ResetGhe()
        {
            var allSeats = db.Ghes.ToList();
            foreach (var ghe in allSeats)
            {
                ghe.tinhTrang = "chua";
            }
            db.SaveChanges();
            return Json(new { success = true, message = "Đã reset tất cả ghế!" });
        }
        public ActionResult GiaoDienGhe()
        {
            var seats = db.Ghes.ToList();
            return View(seats);
        }
    }
}