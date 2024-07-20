using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebDatVePhim.Models;

namespace WebDatVePhim.Controllers
{
    public class TaiKhoanController : Controller
    {
        // GET: TaiKhoan
        DatVeXemPhimATEntities db = new DatVeXemPhimATEntities();

        public ActionResult Index(string searchString)
        {
            var allUsers = db.NguoiDungs.AsQueryable();

            // Lọc theo tên đăng nhập
            if (!string.IsNullOrEmpty(searchString))
            {
                allUsers = allUsers.Where(u => u.taiKhoan.Contains(searchString));
            }

            return View(allUsers.ToList());
        }

        public ActionResult Edit(int id)
        {
            // Load thông tin người dùng dựa trên id
            var user = db.NguoiDungs.Find(id);

            // Hiển thị view chỉnh sửa thông tin người dùng
            return View(user);
        }

        [HttpPost]
        public ActionResult Edit(NguoiDung updatedUser)
        {
            if (ModelState.IsValid)
            {
                // Cập nhật thông tin người dùng trong cơ sở dữ liệu
                db.Entry(updatedUser).State = EntityState.Modified;
                db.SaveChanges();

                // Chuyển hướng về trang danh sách người dùng
                return RedirectToAction("Index");
            }

            // Nếu có lỗi, hiển thị lại view chỉnh sửa
            return View(updatedUser);
        }

        public ActionResult Delete(int id)
        {
            var user = db.NguoiDungs.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var user = db.NguoiDungs.Find(id);
            db.NguoiDungs.Remove(user);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
