using SachOnline.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SachOnline.Controllers
{
    public class UserController : Controller
    {
        QLBANSACHEntities db = new QLBANSACHEntities();
        // GET: User
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public ActionResult DangKy() 
        { 
            return View();
        }
        [HttpPost]
        public ActionResult DangKy(FormCollection collection, KHACHHANG kh) 
        {
            var sHoTen = collection["HoTen"];
            var sTenDN = collection["TenDN"];
            var sMatKhau = collection["MatKhau"];
            var sMatKhauNL = collection["MatKhauNL"];
            var sEmail = collection["Email"];
            var sDienThoai = collection["DienThoai"];
            var sDiaChi = collection["DiaChi"];
            var dNgaySinh = String.Format("{0:MM/dd/yyyy}", collection["NgaySinh"]);
            if (String.IsNullOrEmpty(sHoTen))
            {
                ViewData["err1"] = "Họ Tên Không Được Để Trống";
            }
            else if (String.IsNullOrEmpty(sTenDN))
            {
                ViewData["err2"] = "Tên Đăng Nhập Không Được Để Trống";
            }
            else if (String.IsNullOrEmpty(sMatKhau)) 
            {
                ViewData["err3"] = "Vui Lòng Nhập Mật Khẩu";
            }
            else if (String.IsNullOrEmpty(sMatKhauNL))
            {
                ViewData["err4"] = "Vui Lòng Nhập Lại Mật Khẩu";
            }
            else if(sMatKhau != sMatKhauNL)
            {
                ViewData["err4"] = "Mật Khẩu Nhập Lại Không Khớp";
            }
            else if (String.IsNullOrEmpty(sEmail))
            {
                ViewData["err5"] = "Email Không Được Để Trống";
            }
            else if (String.IsNullOrEmpty(sDienThoai))
            {
                ViewData["err6"] = "Vui Lòng Nhập Số Điện Thoại";
            }
            else if(db.KHACHHANGs.SingleOrDefault(n => n.TaiKhoan == sTenDN) != null)
            {
                ViewBag.ThongBao = "Tên Đăng Nhập Đã Tồn Tại";
            }
            else if(db.KHACHHANGs.SingleOrDefault(n => n.Email == sEmail) != null)
            {
                ViewBag.ThongBao = "Email Đã Được Sử Dụng";
            }
            else
            {
                kh.HoTen = sHoTen;
                kh.Email = sEmail;
                kh.TaiKhoan = sTenDN;
                kh.MatKhau = sMatKhau;
                kh.DiaChiKH = sDiaChi;
                kh.DienThoaiKH = sDienThoai;
                kh.NgaySinh = DateTime.Parse(dNgaySinh);
                db.KHACHHANGs.Add(kh);
                db.SaveChanges();
                return RedirectToAction("DangNhap");
            }
            return this.DangKy();
        }

        [HttpGet]
        public ActionResult DangNhap()
        {
            return View();
        }
        [HttpPost]
        public ActionResult DangNhap(FormCollection collection)
        {
            var sTenDN = collection["TenDN"];
            var sMatKhau = collection["MatKhau"];

            if (String.IsNullOrEmpty(sTenDN))
            {
                ViewData["err1"] = "Bạn Chưa Nhập Ten Đăng Nhập";
            }
            else if(String.IsNullOrEmpty(sMatKhau))
            {
                ViewData["err2"] = "Vui Lòng Nhập Mật Khẩu";
            }
            else
            {
                KHACHHANG kh = db.KHACHHANGs.SingleOrDefault(n => n.TaiKhoan == sTenDN && n.MatKhau == sMatKhau);
                if (kh != null)
                {
                    ViewBag.ThongBao = "Đăng Nhập Thành Công";
                    Session["TaiKhoan"] = kh;
                    return RedirectToAction("Index", "SachOnline");
                }
                else
                {
                    ViewBag.ThongBao = "Tên Đăng Nhập Hoặc Mật Khẩu Không Đúng";
                }
            }
            return View();
        }
        public ActionResult DangXuat()
{
    // Xóa session của người dùng
    Session["TaiKhoan"] = null;

    // Chuyển hướng về trang chủ hoặc trang đăng nhập
    return RedirectToAction("Index", "SachOnline");
}
    }
}