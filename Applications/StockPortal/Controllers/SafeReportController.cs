using StockDB.Model;
using StockService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StockPortal.Controllers
{
    public class SafeReportController : Controller
    {
        SafeReportService safeReportService = new SafeReportService();
        StockModel db = new StockModel();

        // GET: SafeReport
        public ActionResult Index(int SafeID)
        {

            return View(db.Safes.Find(SafeID));
        }
        public JsonResult GetReport(string SafeID)
        {
            return Json(new { data = safeReportService.safeReportVMs(int.Parse(SafeID)) }, JsonRequestBehavior.AllowGet);
        }
    }
}