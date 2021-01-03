using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ShikoPetshop.Models;

namespace ShikoPetshop.Controllers
{
    public class QuanLyPhieuNhapController : Controller
    {
        ShikoPetshopDbEntities1 db = new ShikoPetshopDbEntities1();
        // GET: QuanLyPhieuNhap
        [HttpGet]
        public ActionResult NhapHang()
        {           
            ViewBag.MaNCC = db.NhaCungCaps;
            ViewBag.ListSanPham = db.SanPhams;
            return View();
        }
        [HttpPost]
        public ActionResult NhapHang(PhieuNhap model ,IEnumerable<ChiTietPhieuNhap> lstModel)
        {
            ViewBag.MaNCC = db.NhaCungCaps;
            ViewBag.ListSanPham = db.SanPhams;
            //Gắn đã xóa: false
            model.DaXoa = false;
            db.PhieuNhaps.Add(model);
            db.SaveChanges();
            //Lấy mã phiếu nhập gán cho lstChitietPhieuNhap
            SanPham sp;
            foreach(var item in lstModel)
            {
                //Cập nhật số lượng tồn
                sp = db.SanPhams.Single(n => n.MaSP == item.MaSP);
                sp.SoLuongTon += item.SoLuongNhap;
                //Gán mã phiếu nhập cho tất cả chi tiết phiếu nhập
                item.MaPN = model.MaPN;
            }
            db.ChiTietPhieuNhaps.AddRange(lstModel);
            db.SaveChanges();           
            return View();
        }
        //[HttpGet]
        //public ActionResult DSSPHethang()
        //{
        //    //Danh sách sản phẩm có số lượng tồn <=5
        //    var lstSP = db.SanPhams.Where(n => n.DaXoa == 0 && n.SoLuongTon <= 5);
        //    return View(lstSP);
        //}
    }
}