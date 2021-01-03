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
    public class QuanLySanPhamController : Controller
    {
        // GET: QuanLySanPham
        ShikoPetshopDbEntities1 db = new ShikoPetshopDbEntities1();
        public ActionResult Index()
        {
            return View(db.SanPhams.Where(n => n.DaXoa == 0).OrderBy(n => n.MaSP));
        }
        //TẠO MỚI
        [HttpGet]
        public  ActionResult TaoMoi()
        {
            //Load dropdownlist nhà cung cấp, loại sp, đã xóa
            ViewBag.MaNCC = new SelectList(db.NhaCungCaps.OrderBy(n => n.TenNCC), "MaNCC", "TenNCC");         
            ViewBag.MaLoaiSP = new SelectList(db.LoaiSanPhams.OrderBy(n => n.TenLoai), "MaLoaiSP", "TenLoai");
            return View();
        }

        //LẤY HÌNH ẢNH
        [ValidateInput(false)]
        [HttpPost]       
        public ActionResult TaoMoi(SanPham sp, HttpPostedFileBase[] HinhAnh)
        {
            //Load dropdownlist nhà cung cấp, loại sp
            ViewBag.MaNCC = new SelectList(db.NhaCungCaps.OrderBy(n => n.TenNCC), "MaNCC", "TenNCC");
            ViewBag.MaLoaiSP = new SelectList(db.LoaiSanPhams.OrderBy(n => n.MaLoaiSP), "MaLoaiSP", "TenLoai");                      
            //Kiểm tra hình ảnh đã tồn tại chưa
            if (HinhAnh[0].ContentLength > 0)
            {
                //Lấy tên hình ảnh
                var fileName = Path.GetFileName(HinhAnh[0].FileName);
                //Lấy hình ảnh chuyển vào thư mục hình ảnh
                var path = Path.Combine(Server.MapPath("~/Content/images/products/"), fileName);
                //Nếu thư mục chứa hình ảnh đó rồi thì xuất ra thông báo
                if (System.IO.File.Exists(path))
                {
                    ViewBag.upload = "Hình ảnh đã tồn tại";
                    return View();
                }
                else
                {
                    //Lấy hình ảnh đưa vào thư mục images/products
                    HinhAnh[0].SaveAs(path);                   
                    sp.HinhAnh = fileName;
                }              
            }
            db.SanPhams.Add(sp);
            db.SaveChanges();
            return RedirectToAction("Index","QuanLySanPham");
        }
        //CHỈNH SỦA SẢN PHẨM
        [HttpGet]
        public ActionResult ChinhSua(int? id)
        {

            if (id == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            SanPham sp = db.SanPhams.SingleOrDefault(n => n.MaSP == id);
            if (sp == null)
            {
                return HttpNotFound();
            }
            ViewBag.MaNCC = new SelectList(db.NhaCungCaps.OrderBy(n => n.TenNCC), "MaNCC", "TenNCC", sp.MaNCC);
            ViewBag.MaLoaiSP = new SelectList(db.LoaiSanPhams.OrderBy(n => n.MaLoaiSP), "MaLoaiSP", "TenLoai", sp.MaLoaiSP);
            return View(sp);

        }
        //LƯU SẢN PHẨM SAU KHI CHỈNH SỬA
        [ValidateInput(false)]
        [HttpPost]
        public ActionResult ChinhSua(SanPham model, HttpPostedFileBase HinhAnh)
        {          
            ViewBag.MaNCC = new SelectList(db.NhaCungCaps.OrderBy(n => n.TenNCC), "MaNCC", "TenNCC", model.MaNCC);
            ViewBag.MaLoaiSP = new SelectList(db.LoaiSanPhams.OrderBy(n => n.MaLoaiSP), "MaLoaiSP", "TenLoai", model.MaLoaiSP);
           
                //Lấy tên hình ảnh
                var fileName = Path.GetFileName(HinhAnh.FileName);
                //Lấy hình ảnh chuyển vào thư mục hình ảnh
                var path = Path.Combine(Server.MapPath("~/Content/images/products/"), fileName);

                //Lấy hình ảnh đưa vào thư mục images/products
                HinhAnh.SaveAs(path);
                model.HinhAnh = fileName;
           
            //Nếu dữ liệu đầu vào chắc chắn ok
            db.Entry(model).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");          
        }       
        //XÓA SẢN PHẨM
        [HttpGet]
        public ActionResult Xoa (int? id)
        {         
            if (id == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            SanPham sp = db.SanPhams.SingleOrDefault(n => n.MaSP == id);
            if (sp == null)
            {
                return HttpNotFound();
            }
            ViewBag.MaNCC = new SelectList(db.NhaCungCaps.OrderBy(n => n.TenNCC), "MaNCC", "TenNCC", sp.MaNCC);
            ViewBag.MaLoaiSP = new SelectList(db.LoaiSanPhams.OrderBy(n => n.MaLoaiSP), "MaLoaiSP", "TenLoai", sp.MaLoaiSP);
            return View(sp);
        }
        //XÁC NHẬN XÓA
        [HttpPost]
        public ActionResult Xoa(int id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SanPham model = db.SanPhams.SingleOrDefault(n => n.MaSP == id);
            if (model == null)
            {
                return HttpNotFound();
            }
            db.SanPhams.Remove(model);
            db.SaveChanges();
            return RedirectToAction("Index");

        }
    }
}