using System.Linq;
using System.Web.Mvc;
using WebDatVePhim.Models;

namespace WebDatVePhim.Controllers
{
    public class ReviewController : Controller
    {
        private DatVeXemPhimATEntities db = new DatVeXemPhimATEntities();

        // GET: Review
        public ActionResult Index()
        {
            var binhLuans = db.BinhLuans.Include("NguoiDung").Include("Phim").ToList();
            return View(binhLuans);
        }

        // GET: Review/Create
        public ActionResult Create()
        {
            ViewBag.id_NguoiDung = new SelectList(db.NguoiDungs, "id_NguoiDung", "hoVaTen");
            ViewBag.id_Phim = new SelectList(db.Phims, "id_Phim", "tenPhim");
            return View();
        }

        // POST: Review/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id_BinhLuan,id_NguoiDung,id_Phim,noiDung,soSao,tongBinhLuan")] BinhLuan binhLuan)
        {
            if (ModelState.IsValid)
            {
                db.BinhLuans.Add(binhLuan);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.id_NguoiDung = new SelectList(db.NguoiDungs, "id_NguoiDung", "hoVaTen", binhLuan.id_NguoiDung);
            ViewBag.id_Phim = new SelectList(db.Phims, "id_Phim", "tenPhim", binhLuan.id_Phim);
            return View(binhLuan);
        }
    }
}
