using StockDB.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StockPortal.Controllers
{
    public class BanksController : Controller
    {
        private StockModel db = new StockModel();
        // GET: Banks
        public ActionResult Index()
        {
            return View();
        }
        public JsonResult RemoteCreate( string Name)
        {
            try
            {
                Bank bank = new Bank();
                bank.Name = Name;
                db.Banks.Add(bank);
                db.SaveChanges();
                return Json(new { data = bank.ID }, JsonRequestBehavior.AllowGet);
            }
            catch
            {

                return Json(false, JsonRequestBehavior.AllowGet);
            }

        }
    }
}