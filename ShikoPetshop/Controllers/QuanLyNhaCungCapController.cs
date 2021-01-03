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
    public class QuanLyNhaCungCapController : Controller
    {
        // GET: QuanLyNhaCungCap
        ShikoPetshopDbEntities1 db = new ShikoPetshopDbEntities1();
        public ActionResult Index()
        {
            return View(db.NhaCungCaps.OrderBy(n => n.MaNCC));
        }
        //TAO MOI
        [HttpGet]
        public ActionResult TaoMoi()
        {            
            return View();
        }
        //LƯU THÔNG TIN VỪA TẠO MỚI
        [ValidateInput(false)]
        [HttpPost]
        public ActionResult TaoMoi(NhaCungCap ncc)
        {                    
            db.NhaCungCaps.Add(ncc);
            db.SaveChanges();
            return RedirectToAction("Index", "QuanLyNhaCungCap");
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
            NhaCungCap ncc = db.NhaCungCaps.SingleOrDefault(n => n.MaNCC == id);
            if (ncc == null)
            {
                return HttpNotFound();
            }          
            return View(ncc);
        }
        //LƯU THÔNG TIN CHỈNH SỬA
        [ValidateInput(false)]
        [HttpPost]
        public ActionResult ChinhSua(NhaCungCap model)
        {                       
            //Nếu dữ liệu đầu vào chắc chắn ok
            db.Entry(model).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        //XÓA NHÀ CUNG CẤP
        [HttpGet]
        public ActionResult Xoa(int? id)
        {
            if (id == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            NhaCungCap ncc = db.NhaCungCaps.SingleOrDefault(n => n.MaNCC == id);
            if (ncc == null)
            {
                return HttpNotFound();
            }          
            return View(ncc);
        }
        //XÁC NHẬN XÓA
        [HttpPost]
        public ActionResult Xoa(int id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NhaCungCap model = db.NhaCungCaps.SingleOrDefault(n => n.MaNCC == id);
            if (model == null)
            {
                return HttpNotFound();
            }
            db.NhaCungCaps.Remove(model);
            db.SaveChanges();
            return RedirectToAction("Index");

        }
    }
}