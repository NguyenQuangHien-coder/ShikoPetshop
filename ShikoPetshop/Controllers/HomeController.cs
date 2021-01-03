using ShikoPetshop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
//using ShikoPetshop.Models;
using CaptchaMvc.HtmlHelpers;

namespace ShikoPetshop.Controllers
{
    public class HomeController : Controller
    {
        ShikoPetshopDbEntities1 db = new ShikoPetshopDbEntities1();
        // GET: Home
        public ActionResult Index()
        {
            //Lấy dữ liệu Chó nạp vào model (chó mới)
            var lstSanPhamChoMoi = db.SanPhams.Where(n => n.MaLoaiSP == 1 && n.Moi == 1);
            ViewBag.ListSPCM = lstSanPhamChoMoi;
            //Lấy dữ liệu Chó nạp vào model (mèo mới)
            var lstSanPhamMeoMoi = db.SanPhams.Where(n => n.MaLoaiSP == 2 && n.Moi == 1);
            ViewBag.ListSPMM = lstSanPhamMeoMoi;
            return View();
        }
        public ActionResult Cho()
        {
            //Lấy dữ liệu Chó nạp vào model
            var lstSanPhamCho = db.SanPhams.Where(n => n.MaLoaiSP == 1);
            ViewBag.ListSPCho = lstSanPhamCho;          
            return View();
        }
        public ActionResult Meo()
        {
            //Lấy dữ liệu Mèo nạp vào model
            var lstSanPhamMeo = db.SanPhams.Where(n => n.MaLoaiSP == 2);
            ViewBag.ListSPMeo = lstSanPhamMeo;
            return View();
        }
        public ActionResult SPKhac()
        {
            //Lấy dữ liệu Phụ Kiện nạp vào model
            var lstSanPhamKhac = db.SanPhams.Where(n => n.MaLoaiSP == 3);
            ViewBag.ListSPKhac = lstSanPhamKhac;
            return View();
        }
        public ActionResult NuocNgoai()
        {
            //Lấy dữ liệu thứ cưng Ngoại Quốc nạp vào model
            var lstSanPhamNN = db.SanPhams.Where(n => n.MaLoaiSP != 3 && n.MaNCC == 1 );
            ViewBag.ListSPNN = lstSanPhamNN;
            return View();
        }
        public ActionResult NoiDia()
        {
            //Lấy dữ liệu thứ cưng Nội Địa nạp vào model
            var lstSanPhamND = db.SanPhams.Where(n => n.MaLoaiSP != 3 && n.MaNCC == 2);
            ViewBag.ListSPND = lstSanPhamND;
            return View();
        }
        //Xây dựng Action Đăng Nhập
        //public ActionResult DangNhap()
        //{
        //    return RedirectToAction("Index", "Home");
        //}
        [ChildActionOnly]
        public ActionResult SanPhamPartial()
        {           
            return PartialView();
        }
        public ActionResult MenuPartial()
        {
            //Truy vấn về 1 list các SP
            var lstSP = db.SanPhams;
            return PartialView(lstSP);
        }
        //ĐĂNG KÝ
        [HttpGet]
        public ActionResult DangKy()
        {
            ViewBag.CauHoi = new SelectList(LoadCauHoi());
            return View();
        }
        [HttpPost]
        public ActionResult DangKy(ThanhVien tv, FormCollection f)
        {
            ViewBag.CauHoi = new SelectList(LoadCauHoi());
            if (this.IsCaptchaValid("Captcha is not valid"))
            {
                ViewBag.ThongBao = "Thao tác thành công";
                //Thêm khách hàng vào CSDL
                db.ThanhViens.Add(tv);
                db.SaveChanges();
                return View();
            }
            ViewBag.ThongBao = "Sai mã Captcha";
            return View();
        }
        //Câu hỏi bí mật
        public List<string> LoadCauHoi()
        {
            List<string> lstCauHoi = new List<string>();
            lstCauHoi.Add("Con vật mà bạn yêu thích?");
            lstCauHoi.Add("Màu sắc mà bạn yêu thích?");
            return lstCauHoi;
        }
       //Xây dựng action đăng nhập
       [HttpPost]
       public ActionResult DangNhap(FormCollection f)
        {
            //Kiểm tra tên đăng nhập và mật khẩu
            string sTaiKhoan = f["txtTenDangNhap"].ToString();
            string sMatKhau = f["txtMatKhau"].ToString();
            ThanhVien tv = db.ThanhViens.SingleOrDefault(n => n.TaiKhoan == sTaiKhoan && n.MatKhau == sMatKhau);
            if (tv != null)
            {
                Session["TaiKhoan"] = tv;
                return RedirectToAction("Index");
            }
            //Điều hướng
            return RedirectToAction("Index","Home");
        }
        //Đăng Xuất       
        public ActionResult DangXuat()
        {
            Session["TaiKhoan"] = null;
            return RedirectToAction("Index", "Home");
        }
    }
}