using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ShikoPetshop.Models;

namespace ShikoPetshop.Controllers
{
    public class GioHangController : Controller
    {
        ShikoPetshopDbEntities1 db = new ShikoPetshopDbEntities1();
        public List<ItemGioHang> LayGioHang()
        {
            //Giỏ hàng đã tồn tại
            List<ItemGioHang> lstGioHang = Session["GioHang"] as List<ItemGioHang>;
            if (lstGioHang == null)
            {
                //Nếu giỏ hàng không tồn tại thì khởi tạo giỏ hàng
                lstGioHang = new List<ItemGioHang>();
                Session["GioHang"] = lstGioHang;
                return lstGioHang;
            }
            return lstGioHang;
        }
        //Thêm giỏ hàng (Load lại trang)
        public ActionResult ThemGioHang(int MaSP, string strURL)
        {
            //Kiểm tra sp có tồn tại trong SCDL hay không
            SanPham sp = db.SanPhams.SingleOrDefault(n => n.MaSP == MaSP);
            if (sp == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            //Lấy giỏ hàng
            List<ItemGioHang> lstGioHang = LayGioHang();
            //Trường hợp sp đã tồn tại trong giỏ hàng
            ItemGioHang spCheck = lstGioHang.SingleOrDefault(n => n.MaSP == MaSP);

            if (spCheck != null)
            {
                //Kiểm tra số lượng tồn trước khi cho khác mua hàng
                if (sp.SoLuongTon <  spCheck.SoLuong)
                {
                    return View("ThongBao", "GioHang");
                }
                spCheck.SoLuong++;
                spCheck.ThanhTien = spCheck.SoLuong * spCheck.DonGia;
                return Redirect(strURL);
            }

            ItemGioHang itemGH = new ItemGioHang(MaSP);
            lstGioHang.Add(itemGH);
            return Redirect(strURL);
        }
        public ActionResult CongSP(int MaSP, string strURL)
        {
            //Kiểm tra sp có tồn tại trong SCDL hay không
            SanPham sp = db.SanPhams.SingleOrDefault(n => n.MaSP == MaSP);
           
            //Lấy giỏ hàng
            List<ItemGioHang> lstGioHang = LayGioHang();
            //Trường hợp sp đã tồn tại trong giỏ hàng
            ItemGioHang spCheck = lstGioHang.SingleOrDefault(n => n.MaSP == MaSP);

            if (spCheck != null)
            {
                //Kiểm tra số lượng tồn trước khi cho khác mua hàng
                if (sp.SoLuongTon < spCheck.SoLuong)
                {
                    return View("ThongBao", "GioHang");
                }
                spCheck.SoLuong++;
                spCheck.ThanhTien = spCheck.SoLuong * spCheck.DonGia;
                return Redirect(strURL);
            }          
            return Redirect(strURL);
        }
        public ActionResult TruSP(int MaSP, string strURL)
        {
            //Kiểm tra sp có tồn tại trong SCDL hay không
            SanPham sp = db.SanPhams.SingleOrDefault(n => n.MaSP == MaSP);

            //Lấy giỏ hàng
            List<ItemGioHang> lstGioHang = LayGioHang();
            //Trường hợp sp đã tồn tại trong giỏ hàng
            ItemGioHang spCheck = lstGioHang.SingleOrDefault(n => n.MaSP == MaSP);

            if (spCheck != null)
            {
                //Kiểm tra số lượng tồn trước khi cho khác mua hàng
                if (sp.SoLuongTon < spCheck.SoLuong)
                {
                    return View("ThongBao", "GioHang");
                }
                if(spCheck.SoLuong >1)
                {
                    spCheck.SoLuong--;
                    spCheck.ThanhTien = spCheck.SoLuong * spCheck.DonGia;
                    return Redirect(strURL);
                }             
               
                return Redirect(strURL);
            }
            return Redirect(strURL);
        }
        //Tính tổng số lượng
        public double TinhTongSoLuong()
        {
            List<ItemGioHang> lstGioHang = Session["GioHang"] as List<ItemGioHang>;
            if (lstGioHang == null)
            {
                return 0;
            }

            return lstGioHang.Sum(n => n.SoLuong);

        }
        //Tính tổng tiền
        public decimal TinhTongTien()
        {
            List<ItemGioHang> lstGioHang = Session["GioHang"] as List<ItemGioHang>;
            if (lstGioHang == null)
            {
                return 0;
            }
            return lstGioHang.Sum(n => n.ThanhTien);

        }
        // GET: GioHang
        public ActionResult XemGioHang()
        {
            
            //Lấy giỏ hàng
            List<ItemGioHang> lstGioHang = LayGioHang();
            ViewBag.TongTien = TinhTongTien();
            ViewBag.TongSoLuong = TinhTongSoLuong();
            return View(lstGioHang);
        }
        //Sửa giỏ hàng
        [HttpGet]
        public ActionResult SuaGioHang(int MaSP)
        {
            //Kiểm tra session giỏ hàng tồn tại chưa
            if (Session["GioHang"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            //Kiểm tra sp có trong CSDL hay không
            SanPham sp = db.SanPhams.SingleOrDefault(n => n.MaSP == MaSP);
            if (sp == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            //Lấy giỏ hàng từ session
            List<ItemGioHang> lstGioHang = LayGioHang();  
            //Kiểm tra sp tồn tại trong giỏ hàng hay không
            ItemGioHang spCheck = lstGioHang.SingleOrDefault(n => n.MaSP == MaSP);

            if (spCheck == null)
            {
                return RedirectToAction("Index", "Home");

            }

            ViewBag.GioHang = lstGioHang;
            return View(spCheck);
        }
        //Cập nhật giỏ hàng
        [HttpPost]
       
        public ActionResult CapNhatGioHang(ItemGioHang itemGH)
        {          
            SanPham spCheck = db.SanPhams.SingleOrDefault(n => n.MaSP == itemGH.MaSP);
            if(spCheck.SoLuongTon < itemGH.SoLuong)
            {
                return View("ThongBao");
            }
            //Cập nhật số lượng trong session giỏ hàng
            List<ItemGioHang> lstGH = LayGioHang();
            ItemGioHang itemGHUpdate = lstGH.Find(n => n.MaSP == itemGH.MaSP);
            //Cập nhật
            itemGHUpdate.SoLuong = itemGH.SoLuong;
            itemGHUpdate.ThanhTien = itemGH.SoLuong * itemGH.DonGia;
            return Redirect("XemGioHang");
        }
        //Xóa giỏ hàng
        public ActionResult XoaGioHang(int MaSP)
        {
            //Kiểm tra session giỏ hàng tồn tại chưa
            if (Session["GioHang"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            //Kiểm tra sp có trong CSDL hay không
            SanPham sp = db.SanPhams.SingleOrDefault(n => n.MaSP == MaSP);
            if (sp == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            //Lấy giỏ hàng từ session
            List<ItemGioHang> lstGioHang = LayGioHang();
            //Kiểm tra sp tồn tại trong giỏ hàng hay không
            ItemGioHang spCheck = lstGioHang.SingleOrDefault(n => n.MaSP == MaSP);

            if (spCheck == null)
            {
                return RedirectToAction("Index", "Home");

            }
            //Tiến hành xóa
            lstGioHang.Remove(spCheck);
            return RedirectToAction("XemGioHang");
        }
        //Đặt hàng
        public ActionResult DatHang(KhachHang kh)
        {
            //Kiểm tra session giỏ hàng tồn tại chưa
            if (Session["GioHang"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            KhachHang khang = new KhachHang();
            //Kiểm tra người mua có phải là thanh viên hay không
            if (Session["TaiKhoan"] == null)
            {
                //Thêm khách hàng vào bảng KhachHang  (Vãng lai)            
                khang = kh;
                db.KhachHangs.Add(khang);
                db.SaveChanges();
            }
            else
            {
                //Đối với khách hàng đã có tài khoản (là thành viên)
                ThanhVien tv = Session["TaiKhoan"] as ThanhVien;
                khang.MaThanhVien = tv.MaThanhVien;
                khang.TenKH = tv.HoTen;
                khang.DiaChi = tv.DiaChi;
                khang.Email = tv.Email;
                khang.SoDienThoai = tv.SoDienThoai;
                db.KhachHangs.Add(khang);
                db.SaveChanges();
            }
            //Thêm đơn hàng
            DonDatHang ddh = new DonDatHang();
            ddh.MaKH = khang.MaKH;
            ddh.NgayDat = DateTime.Now;
            ddh.TinhTrangGiaoHang = 0;
            ddh.DaThanhToan = false;
            ddh.UuDai = 0;
            db.DonDatHangs.Add(ddh);
            db.SaveChanges();           
            //Thêm chi tiết đơn hàng
            List<ItemGioHang> lstGH = LayGioHang();
            foreach (var item in lstGH)
            {
                ChiTietDondatHang ctdh = new ChiTietDondatHang();
                ctdh.MaDDH = ddh.MaDDH;
                ctdh.MaSP = item.MaSP;
                ctdh.TenSP = item.TenSP;
                ctdh.SoLuong = item.SoLuong;
                ctdh.DonGia = item.DonGia;
                db.ChiTietDondatHangs.Add(ctdh);
            }
            db.SaveChanges();
            Session["GioHang"] = null;            
            return RedirectToAction("XemGioHang", "GioHang");
            
        }
        public ActionResult GioHangPartial()
        {
            if(TinhTongSoLuong() == 0)
            {
                ViewBag.TongSoLuong = 0;
                ViewBag.TongTien = 0;
                return PartialView();
            }
            ViewBag.TongSoLuong = TinhTongSoLuong();
            ViewBag.TongTien = TinhTongTien();
            return PartialView();
        }
    }
}