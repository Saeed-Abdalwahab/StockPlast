using StockPortal.Helpers;
using StockPortal.Models;
using StockService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StockPortal.Controllers
{
    [HandleError]
    [CustomAuthorize("المخازن")]
    [SessionExpire]
    public class stockReportController : BaseController
    {
        private stockReportService stockReportService = new stockReportService();
        // GET: StoresReport
        public ActionResult Index()
        {
            ViewBag.Items = new SelectList(stockReportService.Items(), "ID", "Name");
            return View();
        }
        [HttpGet]
        public ActionResult Avilableinstore(string storeid)
        {
            return Json(stockReportService.avilableinstore(int.Parse(storeid)), JsonRequestBehavior.AllowGet);
        }
        [HttpGet]

        public ActionResult avilable(string storetypeid,string itemid)
        {
            if (itemid == null||itemid=="")
            {
            return PartialView(stockReportService.avilableinstore(int.Parse(storetypeid)));
            }
            else
            {
                return PartialView(stockReportService.avilableinstore(int.Parse(storetypeid), int.Parse(itemid)));
            }
        }
        [HttpGet]

        public ActionResult avilableInFinshStore( string itemid)
        {
            if (itemid == null || itemid == "")
            {
                return PartialView(stockReportService.avilableinFinishstore());
            }
            else
            {
                return PartialView(stockReportService.avilableinFinishstore(int.Parse(itemid)));
            }
        }

    }
}