using StockService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StockPortal.Controllers
{
    public class SupplierAccountReportController : Controller
    {
        SupplierAccountReportService service = new SupplierAccountReportService();
        // GET: SupplierAccountReport
        public ActionResult Index(int ID)
        {
            var List = service.SupplierAccountReports(ID);
            if (List != null) { 
            ViewBag.PAymentTotal = List.Sum(x => x.Payment);
            ViewBag.SalesTotal   = List.Sum(x => x.Purchases);
            ViewBag.SalesReturn  = List.Sum(x => x.PurchasesReturn);
            ViewBag.totalBalance = List.LastOrDefault().TotalBalance;
            }
            else
            {
                ViewBag.PAymentTotal = 0;
                 ViewBag.SalesTotal  =0;
                 ViewBag.SalesReturn =0;
                 ViewBag.totalBalance=0;
            }
            return View(service.supplier(ID));
        }
        [HttpGet]
        public JsonResult GetAll(string ID)
        {
            return Json(new { data = service.SupplierAccountReports(int.Parse(ID)) }, JsonRequestBehavior.AllowGet);
        }
    }

}