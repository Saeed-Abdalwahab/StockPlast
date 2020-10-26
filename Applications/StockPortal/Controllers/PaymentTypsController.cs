using StockDB.Model;
using StockService;
using StockViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StockPortal.Controllers
{
    public class PaymentTypsController : Controller
    {
        // GET: PaymentTyps
        private StockModel db = new StockModel();
        private PaymenttypService paymenttypService = new PaymenttypService();

        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public JsonResult GetJobs()
        {
            var all = paymenttypService.GettAll();
            return Json(new { data = all }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetByID(int id)
        {

            var job = paymenttypService.GetByID(id);
            return Json(new { data = job }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Save(PaymentTypVM PaymentTypVM)
        {

            return Json(paymenttypService.SaveinDatabase(PaymentTypVM), JsonRequestBehavior.AllowGet);

        }


        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
   
                PaymentType paymentType = db.PaymentTypes.Find(id);
                db.PaymentTypes.Remove(paymentType);
                db.SaveChanges();
                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }
        }


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}