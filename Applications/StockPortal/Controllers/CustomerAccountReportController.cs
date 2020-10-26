using StockService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StockPortal.Controllers
{
    public class CustomerAccountReportController : Controller
    {
        CustomerAccountReportService service = new CustomerAccountReportService();
        // GET: CustomerAccountReport
        public ActionResult Index(int ID)
        {
            var CutomerTransactionList = service.customerAccountReports(ID);
            if (CutomerTransactionList != null) { 
            ViewBag.PAymentTotal = CutomerTransactionList.Sum(x => x.Payment);
            ViewBag.SalesTotal = CutomerTransactionList.Sum(x => x.Sales);
            ViewBag.SalesReturn = CutomerTransactionList.Sum(x => x.SalesReturn);
            ViewBag.Discounts = CutomerTransactionList.Sum(x => x.Discount);
            ViewBag.totalBalance = CutomerTransactionList.LastOrDefault().TotalBalance;
            }
            else
            {
                ViewBag.PAymentTotal = 0;

                ViewBag.SalesTotal = 0;
                ViewBag.SalesReturn = 0;
                ViewBag.Discounts = 0;
                ViewBag.totalBalance = 0;
            }

            return View(service.customer(ID));
        }
        [HttpGet]
        public JsonResult GetAll(string ID)
        {
            return Json(new { data = service.customerAccountReports(int.Parse(ID)) },JsonRequestBehavior.AllowGet) ;
        }
    }
}