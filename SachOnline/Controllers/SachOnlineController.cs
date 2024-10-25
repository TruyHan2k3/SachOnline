using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SachOnline.Models;

namespace SachOnline.Controllers
{
    public class SachOnlineController : Controller
    {
        QLBANSACHEntities db = new QLBANSACHEntities();
        // GET: SachOnline
        public ActionResult Index()
        {
            var lstSach = db.SACHes.OrderByDescending(s => s.NgayCapNhat).Take(6).ToList();
            return View(lstSach);
        }

        public ActionResult ChuDePartial()
        {
            var lstChuDe = db.CHUDEs.ToList();
            return PartialView(lstChuDe);
        }
        public ActionResult NhaXuatBanPartial()
        {
            var lstNhaXuatBan = db.NHAXUATBANs.ToList();
            return PartialView(lstNhaXuatBan);
        }
        public ActionResult SliderPartial()
        {
            return PartialView();
        }
        public ActionResult NavbarPartial()
        {
            return PartialView();
        }
        public ActionResult SachBanNhieuPartial()
        {
            var lstSachBanNhieu = db.SACHes.OrderByDescending(s => s.SoLuongBan).Take(6).ToList();
            return PartialView(lstSachBanNhieu);
        }
        public ActionResult SachTheoChuDe(int id)
        {
            var lstSachChuDe = from s in db.SACHes where s.MaCD == id select s;
            return View("Index", lstSachChuDe);
        }
        public ActionResult NhaXuatBan(int id)
        {
            var lstNhaXuatBan = from s in db.SACHes where s.MaNXB == id select s;
            return View("Index", lstNhaXuatBan);
        }
        public ActionResult ChiTietSach(int id)
        {            
                var sach = db.SACHes.SingleOrDefault(s => s.MaSach == id);
                if (sach == null)
                {
                    return HttpNotFound();
                }
                return View(sach);
        }
    }
}