using ShikoPetshop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ShikoPetshop.Controllers
{
    public class SanPhamController : Controller
    {
        ShikoPetshopDbEntities1 db = new ShikoPetshopDbEntities1();
        // GET: SanPham
        public ActionResult Index()
        {
            //Xuất ra toàn bộ thú cưng
            var lstSanPhamAll = db.SanPhams.Where(n => n.MaLoaiSP != 3);
            ViewBag.ListSPAll = lstSanPhamAll;
            //Xuất ra toàn bộ phụ kiện
            var lstSanPhamAllPK = db.SanPhams.Where(n => n.MaLoaiSP == 3);
            ViewBag.ListSPAllPK = lstSanPhamAllPK;
            return View();
        }
        public ActionResult SanPhamMoiVe()
        {
            //Lấy dữ liệu Chó nạp vào model (chó mới)
            var lstSanPhamChoMoi = db.SanPhams.Where(n => n.MaLoaiSP == 1 && n.Moi == 1);
            ViewBag.ListSPCM = lstSanPhamChoMoi;
            //Lấy dữ liệu Chó nạp vào model (mèo mới)
            var lstSanPhamMeoMoi = db.SanPhams.Where(n => n.MaLoaiSP == 2 && n.Moi == 1);
            ViewBag.ListSPMM = lstSanPhamMeoMoi;

            return View();
        }     
        //Tạo partialView
        [ChildActionOnly]
        public ActionResult SanPhamPartial()
        {
            //Lấy dữ liệu Chó nạp vào model (chó mới)
            //var lstSanPhamChoMoi = db.SanPhams.Where(n => n.MaLoaiSP == 1 && n.Moi == 1);
            return PartialView();
        }              
        //Load Mã Loại SP và mã nhà cung cấp
        public ActionResult SanPhamTheoNCC(int? MaLoaiSP, int? MaNCC)
        {
            //if (MaLoaiSP == null || MaNCC == null)
            //{
            //    return new HttpStatusCodeResult(HttpStatusCode.Badrequest);
            //}
            var lstSP_NCC = db.SanPhams.Where(n => n.MaLoaiSP == MaLoaiSP && n.MaNCC == MaNCC);
            if(lstSP_NCC.Count() == 0)
            {
                return HttpNotFound();
            }
            return View(lstSP_NCC);
        }
        //SẢN PHẨM DETAIL       
        public ActionResult SanPhamDetail(int masp)
        {
            var lstSPDT = db.SanPhams.Where(n => n.MaSP == masp);

            //Xuất ra toàn bộ thú cưng
            var lstSanPhamAll = db.SanPhams.Where(n => n.MaLoaiSP != 3);
            ViewBag.ListSPAll = lstSanPhamAll;
            return View(lstSPDT.OrderBy(n => n.MaSP));
        }
    }
}