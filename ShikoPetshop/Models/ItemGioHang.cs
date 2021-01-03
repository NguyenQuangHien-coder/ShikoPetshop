using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShikoPetshop.Models
{
    public class ItemGioHang
    {
        public string TenSP { get; set; }
        public int MaSP { get; set; }
        public int SoLuong { get; set; }
        public decimal DonGia { get; set; }
        public string HinhAnh { get; set; }
        public decimal ThanhTien { get; set; }
        public ItemGioHang(int iMaSP, int sl)
        {
            using (ShikoPetshopDbEntities1 db = new ShikoPetshopDbEntities1())
            {
                this.MaSP = iMaSP;
                SanPham sp = db.SanPhams.Single(n => n.MaSP == iMaSP);
                this.TenSP = sp.TenSP;
                this.HinhAnh = sp.HinhAnh;
                this.DonGia = sp.DonGia.Value;
                this.SoLuong = sl;
                this.ThanhTien = DonGia * SoLuong;

            }
        }
        public ItemGioHang(int iMaSP)
        {
            using (ShikoPetshopDbEntities1 db = new ShikoPetshopDbEntities1())
            {
                this.MaSP = iMaSP;
                SanPham sp = db.SanPhams.Single(n => n.MaSP == iMaSP);
                this.TenSP = sp.TenSP;
                this.SoLuong = 1;
                this.HinhAnh = sp.HinhAnh;
                this.DonGia = sp.DonGia.Value;
                this.ThanhTien = DonGia * SoLuong;

            }
        }
    }
}