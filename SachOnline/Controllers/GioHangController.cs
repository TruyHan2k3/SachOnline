using SachOnline.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SachOnline.Controllers
{
    public class GioHangController : Controller
    {
        // GET: GioHang
        QLBANSACHEntities db = new QLBANSACHEntities();
        public List<GioHang> LayGioHang()
        {
            List<GioHang> lstGioHang = Session["GioHang"] as List<GioHang>;
            if (lstGioHang == null)
            {
                lstGioHang = new List<GioHang>();
                Session["GioHang"] = lstGioHang;
            }
            return lstGioHang;
        }
        public ActionResult ThemGioHang(int id, string url)
        { 
            List<GioHang> lstCart = LayGioHang();
            GioHang gioHang = lstCart.Find(n=>n.iMaSach == id);
            if (gioHang == null)
            {
                gioHang = new GioHang(id);
                lstCart.Add(gioHang);
            }
            else
            {
                gioHang.iSoLuong++;
            }
            return Redirect(url);
        }
        private int TongSoLuong()
        {
            int iTongSoLuong = 0;
            List<GioHang> lstGioHang = Session["GioHang"] as List <GioHang>;
            if (lstGioHang != null)
            {
                iTongSoLuong = lstGioHang.Sum(n=>n.iSoLuong);

            }
            return iTongSoLuong;
        }
        private double TongTien()
        {
            double dTongTen = 0;
            List<GioHang> lstGioHang = Session["GioHang"] as List<GioHang>;
            if(lstGioHang != null)
            {
                dTongTen = lstGioHang.Sum(n=>n.dThanhTien);
            }
            return dTongTen;
        }
        public ActionResult GioHang()
        {
            List<GioHang> lstGioHang = LayGioHang();
            if (lstGioHang.Count == 0)
            {
                return RedirectToAction("Index", "SachOnline");
            }
            ViewBag.TongSoLuong = TongSoLuong();
            ViewBag.TongTien = TongTien();
            return View(lstGioHang);

        }
        public ActionResult GioHangPartial()
        {
            ViewBag.TongSoLuong = TongSoLuong();
            ViewBag.TongTien = TongTien();
            return PartialView();
        }
        public ActionResult XoaSP(int iMaSach) { 
            List<GioHang> lstGioHang = LayGioHang();

            GioHang gioHang = lstGioHang.SingleOrDefault(n => n.iMaSach == iMaSach);
            if (gioHang != null)
            {
                lstGioHang.RemoveAll(n => n.iMaSach==iMaSach);
                if (lstGioHang.Count == 0)
                {
                    return RedirectToAction("Index", "SachOnline");
                }                
            }
            return RedirectToAction("GioHang");
        }
        public ActionResult CapNhat(int iMaSach, FormCollection f)
        {
            List<GioHang> lstGioHang= LayGioHang();
            GioHang gioHang = lstGioHang.SingleOrDefault(n => n.iMaSach == iMaSach);
            if (gioHang != null)
            {
                gioHang.iSoLuong = int.Parse(f["txtSoLuong"].ToString());
            }
            return RedirectToAction("GioHang");
        }
        public ActionResult XoaGioHang()
        {
            List<GioHang> lstGioHang = LayGioHang();
            lstGioHang.Clear();
            return RedirectToAction("Index", "SachOnline");
        }
        [HttpGet]
        public ActionResult DatHang() 
        {
            if (Session["TaiKhoan"] == null || Session["TaiKhoan"].ToString() == "")
            {
                return RedirectToAction("DangNhap", "User");
            }
            if (Session["GioHang"] == null)
            {
                return RedirectToAction("Index", "SachOnline");
            }
            List<GioHang> lstGioHang = LayGioHang();
            ViewBag.TongSoLuong = TongSoLuong();
            ViewBag.TongTien = TongTien();
            return View(lstGioHang);
        }
        [HttpPost]
        public ActionResult DatHang(FormCollection f)
        {
            List<GioHang> lstGioHang = LayGioHang();

            DONDATHANG ddh = new DONDATHANG();
            KHACHHANG kh = (KHACHHANG)Session["TaiKhoan"];

            ddh.MaKH = kh.MaKH;
            ddh.NgayDat = DateTime.Now;
            var NgayGiao = @String.Format("{0:dd/MM/yyyy}", f["NgayGiao"]);
            ddh.NgayGiao = DateTime.Parse(NgayGiao);

            ddh.TinhTrangGiaoHang = true;
            ddh.DaThanhToan = false;
            db.DONDATHANGs.Add(ddh);
            db.SaveChanges();

            foreach (var item in lstGioHang)
            {
                CHITIETDONTHANG ctdh = new CHITIETDONTHANG();  
                ctdh.MaDonHang = ddh.MaDonHang;
                ctdh.MaSach = item.iMaSach;
                ctdh.SoLuong = item.iSoLuong;
                ctdh.DonGia = (decimal)item.dDonGia;

                db.CHITIETDONTHANGs.Add(ctdh);
            }
            db.SaveChanges();
            Session["GioHang"] = null;
            return RedirectToAction("XacNhanDonHang", "GioHang");
        }

        public ActionResult XacNhanDonHang()
        {
            return View();
        }
        public ActionResult Index()
        {
            return View();
        }
    }
    
}