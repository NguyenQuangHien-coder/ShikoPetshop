using System.Linq;
using System.Web.Mvc;
using ShikoPetshop.Models;

namespace ShikoPetshop.Controllers
{
    public class TimKiemController : Controller
    {
        // GET: TimKiem
        ShikoPetshopDbEntities1 db = new ShikoPetshopDbEntities1();       
        public ActionResult KQTimKiem(string sTuKhoa)
        {           
            //Tìm kiếm tên sản phẩm
            var lstSP = db.SanPhams.Where(n => n.TenSP.Contains(sTuKhoa));
            return View(lstSP.OrderBy(n=>n.TenSP));          
        }            
    }
}