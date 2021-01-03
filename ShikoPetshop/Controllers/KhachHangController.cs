using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ShikoPetshop.Models;

namespace ShikoPetshop.Controllers
{
    public class KhachHangController : Controller
    {
        // GET: KhachHang
        ShikoPetshopDbEntities1 db = new ShikoPetshopDbEntities1();
        public ActionResult Index()
        {
            //Truy vấn dữ liệu thông qua câu lệnh
            //lst sẽ lấy toàn bộ dữ liệu
            //var lstKH = from KH in db.KhachHangs select KH;
            var lstKH = db.KhachHangs.ToList();
            return View(lstKH);
        }
        public ActionResult Index1()
        {
            var lstKH = from KH in db.KhachHangs select KH;
            return View(lstKH);
        }
        public ActionResult TruyVanDoiTuong()
        {
            //Truy vấn 1 đối tượng
            KhachHang khachHang = db.KhachHangs.SingleOrDefault(n => n.MaKH == 2);
            return View(khachHang);
        }
        public ActionResult SortDuLieu()
        {
            //Phương thức sắp xếp dữ liệu
            //Sort ngược lại sử dụng OrderBy thay vì OrderByDescending
            List<KhachHang> list = db.KhachHangs.OrderByDescending(n => n.TenKH).ToList();
            return View(list);
        }   
        public ActionResult GroupDuLieu()
        {
            //Xuất ra danh sách thành viên thường và thành viên VIP
            List<ThanhVien> lstKH = db.ThanhViens.OrderByDescending(n => n.TaiKhoan).ToList();
            return View(lstKH);
        }
    }
}