using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebDatVePhim.Models;



namespace WebDatVePhim.Controllers
{
    public class ChonGheNgoiController : Controller
    {
        // GET: ChonGheNgoi
        DatVeXemPhimATEntities db = new DatVeXemPhimATEntities();

        public ActionResult Index(int lichChieuId, int phongChieuId)
        {
            var lichChieu = db.LichChieuPhims.Find(lichChieuId);
            if (lichChieu == null)
            {
                // Handle case where lichChieuId is not valid
                return RedirectToAction("Error", "Home");
            }
            Session["LichChieuId"] = lichChieuId;
            Session["PhimId"] = lichChieu.id_Phim;
            Session["TenPhim"] = lichChieu.Phim.tenPhim;
            Session["NgayChieu"] = lichChieu.ngayChieu;
            Session["TenPhongChieu"] = lichChieu.PhongChieu.tenPhongChieu;
            Session["ThoiGianBatDau"] = lichChieu.thoiGianBatDau;
            Session["ThoiGianKetThuc"] = lichChieu.thoiGianKetThuc;

            ViewBag.LichChieuId = lichChieuId;
            var seats = db.Ghes.Where(g => g.id_PhongChieu == phongChieuId).ToList();
            return View(seats);
        }

        [HttpPost]
        public JsonResult DatGhe(List<int> gheIds, int lichChieuId)
        {
            var result = new List<object>();

            foreach (var gheId in gheIds)
            {
                var ghe = db.Ghes.Find(gheId);
                if (ghe != null && ghe.tinhTrang != "DaDat")
                {
                    result.Add(new { success = true, gheId = gheId, viTriGhe = ghe.soHangGhe + ghe.soGheTrongHang });
                }
                else
                {
                    result.Add(new { success = false, gheId = gheId, message = "Ghế này đã được đặt rồi!" });
                }
            }

            var sessionSeats = Session["SelectedSeats"] as List<GheViTri> ?? new List<GheViTri>();
            var newSeats = result.Where(r => (bool)r.GetType().GetProperty("success").GetValue(r, null))
                                 .Select(r => new GheViTri { GheId = (int)r.GetType().GetProperty("gheId").GetValue(r, null), ViTri = (string)r.GetType().GetProperty("viTriGhe").GetValue(r, null) })
                                 .ToList();

            sessionSeats.AddRange(newSeats);
            Session["SelectedSeats"] = sessionSeats;
            Session["LichChieuId"] = lichChieuId;

            return Json(result);
        }
      

        public ActionResult ChonBapNuoc()
        {
            var lichChieuId = Session["LichChieuId"] as int?;
            var selectedSeats = Session["SelectedSeats"] as List<GheViTri>;
            if (!lichChieuId.HasValue)
            {
                // Handle case where lichChieuId is not available in Session
                return RedirectToAction("Error", "Home");
            }
            var bapNuocList = db.BapNuocs.Select(bn => new BapNuocDetail
            {
                Id = bn.id_BapNuoc,
                hinhAnh = bn.hinhAnh,
                TenBapNuoc = bn.tenBapNuoc,
                chiTietBapNuoc = bn.chiTietBapNuoc,
                GiaTien = bn.giaTien ?? 0,
                SoLuong = 0 // Initial quantity is 0
            }).ToList();

            var model = new ChonBapNuocViewModel
            {
                BapNuocSelections = bapNuocList,
                LichChieuId = lichChieuId.Value,
                SelectedSeats = selectedSeats
            };

            return View(model);
        }

        [HttpPost]
        public JsonResult LuuBapNuoc(List<BapNuocDetail> bapNuocSelections)
        {
            try
            {
                if (bapNuocSelections != null)
                {
                    // Lọc các bắp nước đã chọn (Số lượng > 0)
                    var selectedBapNuoc = bapNuocSelections.Where(bn => bn.SoLuong > 0).ToList();

                    // Lưu vào Session
                    Session["SelectedBapNuoc"] = selectedBapNuoc;
                }
                else
                {
                    Session["SelectedBapNuoc"] = new List<BapNuocDetail>();
                }
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }
        [HttpPost]
        public ActionResult ClearBapNuocSelections()
        {
            Session.Remove("SelectedBapNuoc");

            return Json(new { success = true });
        }
        [HttpPost]
        public ActionResult ClearGheNgoiSelections()
        {
            Session.Remove("SelectedSeats");

            return Json(new { success = true });
        }
        [HttpPost]
        public ActionResult ClearLichChieuSelections()
        {
            Session.Remove("LichChieuId");
           

            return Json(new { success = true });
        }      
        public ActionResult ThongTinVe()
        {
            var selectedSeats = Session["SelectedSeats"] as List<GheViTri>;
            var bapNuocSelections = Session["SelectedBapNuoc"] as List<BapNuocDetail>;
            var khuyenMaiSelections = Session["SelectedKhuyenMai"] as List<KhuyenMai>;
            var lichChieuId = Session["LichChieuId"] as int?;
            var lichChieu = db.LichChieuPhims.Find(lichChieuId);
            var phimId = Session["PhimId"] as int?;
            var tenPhim = Session["TenPhim"] as string;
            var ngayChieu = Session["NgayChieu"] as DateTime?;
            var tenPhongChieu = Session["TenPhongChieu"] as string;
            var thoiGianBatDau = Session["ThoiGianBatDau"] as DateTime?;
            var thoiGianKetThuc = Session["ThoiGianKetThuc"] as DateTime?;
            var phim = db.Phims.Find(phimId.Value);

            if (selectedSeats == null || bapNuocSelections == null)
            {
                return RedirectToAction("Index");
            }
            double price = khuyenMaiSelections?.Sum(km => km.giaKhuyenMai ?? 0) ?? 0;
            double giamGia= 1;
            if (khuyenMaiSelections != null && khuyenMaiSelections.Any())
            {
                giamGia = price/100;
            }
            var giaVe = 75000 * selectedSeats.Count;
            var giaBapNuoc = bapNuocSelections.Sum(bn => bn.GiaTien * bn.SoLuong);
            var total = (giaVe + giaBapNuoc) * giamGia;
       
            var model = new XemLaiVeViewModel
            {
                ViTriGhe = string.Join(", ", selectedSeats.Select(s => s.ViTri)),
                GiaVe = 75000 * selectedSeats.Count,
                BapNuocSelections = bapNuocSelections,
                TongSoTien = total /*(double)(75000 * selectedSeats.Count + bapNuocSelections.Sum(bn => bn.GiaTien * bn.SoLuong))*/,
                TenPhim = lichChieu.Phim.tenPhim,
                TenPhongChieu = lichChieu.PhongChieu.tenPhongChieu,
                ThoiGianBatDau = lichChieu.thoiGianBatDau.GetValueOrDefault().ToString("hh\\:mm"),
                ThoiGianKetThuc = lichChieu.thoiGianKetThuc.GetValueOrDefault().ToString("hh\\:mm"),
                NgayChieu = ngayChieu?.ToString("dd/MM/yyyy"),
                TenRap = lichChieu.PhongChieu.Rap.tenRap,
                DiaChiRap = lichChieu.PhongChieu.Rap.diaChiRap,
                KhuyenMaiSelections = khuyenMaiSelections,
        
            };
            return View(model);
        }
        public ActionResult XemLaiVe()
        {
            var selectedSeats = Session["SelectedSeats"] as List<GheViTri>;
            var bapNuocSelections = Session["SelectedBapNuoc"] as List<BapNuocDetail>;
            var lichChieuId = Session["LichChieuId"] as int?;
            var phimId = Session["PhimId"] as int?;
            var tenPhim = Session["TenPhim"] as string;
            var ngayChieu = Session["NgayChieu"] as DateTime?;
            var tenPhongChieu = Session["TenPhongChieu"] as string;
            var thoiGianBatDau = Session["ThoiGianBatDau"] as DateTime?;
            var thoiGianKetThuc = Session["ThoiGianKetThuc"] as DateTime?;
            var khuyenMaiSelections = Session["SelectedKhuyenMai"] as List<KhuyenMai>;
            double price = khuyenMaiSelections?.Sum(km => km.giaKhuyenMai ?? 0) ?? 0;
            double giamGia = 1;
            if (khuyenMaiSelections != null && khuyenMaiSelections.Any())
            {
                giamGia = 1 - price / 100;
            }
            var giaVe = 75000 * selectedSeats.Count;
            var giaBapNuoc = bapNuocSelections.Sum(bn => bn.GiaTien * bn.SoLuong);
            var total = (giaVe + giaBapNuoc) * giamGia;
            if (!lichChieuId.HasValue)
            {
                // Handle case where lichChieuId is not available in Session
                return RedirectToAction("Error", "Home");
            }

            var lichChieu = db.LichChieuPhims.Find(lichChieuId);
            var phim = db.Phims.Find(lichChieu.id_Phim);
            var randomMaDat = Session["MaDat"] as string;
            var model = new XemLaiVeViewModel
            {
                ViTriGhe = string.Join(", ", selectedSeats.Select(s => s.ViTri)),
                GiaVe = 75000 * selectedSeats.Count,
                BapNuocSelections = bapNuocSelections,
                TongSoTien = total,
                TenPhim = lichChieu.Phim.tenPhim,
                TenPhongChieu = lichChieu.PhongChieu.tenPhongChieu,
                ThoiGianBatDau = lichChieu.thoiGianBatDau.GetValueOrDefault().ToString("hh\\:mm"),
                ThoiGianKetThuc = lichChieu.thoiGianKetThuc.GetValueOrDefault().ToString("hh\\:mm"),
                NgayChieu = ngayChieu?.ToString("dd/MM/yyyy"),
                TenRap = lichChieu.PhongChieu.Rap.tenRap,
                DiaChiRap = lichChieu.PhongChieu.Rap.diaChiRap,
                KhuyenMaiSelections = khuyenMaiSelections,
                maDat = randomMaDat
            };


            Session.Remove("SelectedSeats");
            Session.Remove("SelectedBapNuoc");
            Session.Remove("LichChieuId");
            Session.Remove("PhimId");
            return View(model);
        }
        public static string GenerateRandomNumber()
        {
            Random random = new Random();
            int randomNumber = random.Next(100000, 1000000); // Generates a number between 100000 and 999999
            return randomNumber.ToString();
        }
        [HttpPost]
        public ActionResult ThanhToanVe()
        {
            try
            {
                var selectedSeats = Session["SelectedSeats"] as List<GheViTri>;
                var bapNuocSelections = Session["SelectedBapNuoc"] as List<BapNuocDetail>;
                var lichChieuId = Session["LichChieuId"] as int?;
                var khuyenMaiSelections = Session["SelectedKhuyenMai"] as List<KhuyenMai>;

                if (selectedSeats == null || bapNuocSelections == null)
                {
                    return Json(new { success = false, message = "Thông tin không đầy đủ." });
                }
                var chiTietBapNuoc = string.Join(",", bapNuocSelections.Select(bn => $"{bn.TenBapNuoc} - {bn.SoLuong}"));
                //var khuyenMaiIds = string.Join(",", khuyenMaiSelections.Select(km => km.id_KhuyenMai));
                var khuyenMaiIds = khuyenMaiSelections != null ? string.Join(",", khuyenMaiSelections.Select(km => km.id_KhuyenMai)) : "";
                double price = khuyenMaiSelections?.Sum(km => km.giaKhuyenMai ?? 0) ?? 0;
                double giamGia = 1;
                if (khuyenMaiSelections != null && khuyenMaiSelections.Any())
                {
                    giamGia =1- price / 100;
                }

                var giaVe = 75000 * selectedSeats.Count;
                var giaBapNuoc = bapNuocSelections.Sum(bn => bn.GiaTien * bn.SoLuong);
                var total = (giaVe + giaBapNuoc) * giamGia;
                var randomMaDat = GenerateRandomNumber();
                var ve = new Ve
                {
                    ngayDat = DateTime.Now,
                    gioDat = DateTime.Now.TimeOfDay,
                    //maDat = Guid.NewGuid().ToString(),
                    maDat = randomMaDat,
                    soTien = total,
                    viTriGhe = string.Join(",", selectedSeats.Select(s => s.ViTri)),
                    id_LichChieuPhim = lichChieuId,
                    chiTietBapNuoc = chiTietBapNuoc,
                    id_KhuyenMai = !string.IsNullOrEmpty(khuyenMaiIds) ? int.Parse(khuyenMaiIds) : (int?)null,
                    tongTienBapNuoc =(int?) giaBapNuoc
                };          
                db.Ves.Add(ve);
                db.SaveChanges();
                foreach (var bapNuoc in bapNuocSelections)
                {
                    if (bapNuoc.SoLuong > 0)
                    {
                        var veBapNuoc = new Ve_BapNuoc
                        {
                            id_Ve = ve.id_Ve,
                            id_BapNuoc = bapNuoc.Id,
                            soLuong = bapNuoc.SoLuong
                        };
                        db.Ve_BapNuoc.Add(veBapNuoc);
                    }
                }
                db.SaveChanges();
                foreach (var seat in selectedSeats)
                {
                    var ghe = db.Ghes.Find(seat.GheId);
                    ghe.tinhTrang = "DaDat";
                }
                db.SaveChanges();
                Session["MaDat"] = ve.maDat;
                return Json(new { success = true, veId = ve.id_Ve });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        public ActionResult ChonKhuyenMai()
        {
            var khuyenMaiList = db.KhuyenMais.ToList();
            var model = new ChonKhuyenMaiViewModel
            {
                KhuyenMaiSelections = khuyenMaiList
            };
            return View(model);
        }

        [HttpPost]
        public JsonResult LuuKhuyenMai(List<int> khuyenMaiIds)
        {
            try
            {
                if (khuyenMaiIds != null)
                {
                    var selectedKhuyenMai = db.KhuyenMais.Where(km => khuyenMaiIds.Contains(km.id_KhuyenMai)).ToList();
                    Session["SelectedKhuyenMai"] = selectedKhuyenMai;
                }
                else
                {
                    Session["SelectedKhuyenMai"] = new List<KhuyenMai>();
                }

                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }
        public ActionResult XoaKhuyenMai(int id)
        {
            
            Session.Remove("SelectedKhuyenMai"); // Xóa khuyến mãi trong session
                                                 // Hoặc xóa từ database nếu có

            return Json(new { success = true }); // Trả về kết quả JSON
        }
        [HttpPost]
        public ActionResult ClearKhuyenMaiSelections()
        {
            Session.Remove("SelectedKhuyenMai");
            return Json(new { success = true });
        }
        public class ChonKhuyenMaiViewModel
        {
            public List<KhuyenMai> KhuyenMaiSelections { get; set; }
            public List<int> SelectedKhuyenMaiIds { get; set; }
        }
        public class XemLaiVeViewModel
    {
            public int VeId { get; set; }
            public string ViTriGhe { get; set; }
            public double GiaVe { get; set; }
            public List<BapNuocDetail> BapNuocSelections { get; set; }
            public List<KhuyenMai> KhuyenMaiSelections { get; set; }
            public double TongSoTien { get; set; }
            public string TenPhim { get; internal set; }
            public string TenPhongChieu { get; internal set; }
            public string ThoiGianBatDau { get; internal set; }
            public string ThoiGianKetThuc { get; internal set; }
            public string NgayChieu { get; internal set; }
            public string DiaChiRap { get; internal set; }
            public string TenRap { get; internal set; }
            public string maDat { get; set; }

        }
        public class ChonBapNuocViewModel
        {
            internal List<GheViTri> SelectedSeats;

            public int VeId { get; set; }
            public List<BapNuocDetail> BapNuocSelections { get; set; }
            public int LichChieuId { get; set; }
        }
        public class BapNuocDetail
        {
            public int Id { get; set; }
            public string hinhAnh { get; set; }
            public string TenBapNuoc { get; set; }
            public string chiTietBapNuoc { get; set; }
            public double GiaTien { get; set; }
            public int SoLuong { get; set; }
        }
    
        public class GheViTri
        {
            public int GheId { get; set; }
            public string ViTri { get; set; }
        }
    }
}