using StockDB.Model;
using StockPortal.Helpers;
using StockPortal.Models;
using StockService;
using StockViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace StockPortal.Controllers
{
    [HandleError]
    [CustomAuthorize("الايرادات اليوميه")]
    [SessionExpire]
    public class IncomeDetailsController : BaseController
    {
        // GET: IncomeDetails
        private IncomeDetailsservice incomeDetailsservice = new IncomeDetailsservice();
        StockModel db = new StockModel();

        public ActionResult Index()
        {
            ViewBag.income = new SelectList(db.Incomes.ToList(), "inc_id", "inc_name");
            ViewBag.safes = new SelectList(db.Safes.ToList(), "safe_id", "safe_name");
            return View();
        }
        [HttpGet]
        public JsonResult GetincomeDet()
        {
            var incomedetails = incomeDetailsservice.Getall();
            return Json(new { data = incomedetails }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult Save(IncomeDetailsVM incomeDetailsVM)
        {
            
            return Json(new { msg = incomeDetailsservice.SaveinDatabase(incomeDetailsVM) }, JsonRequestBehavior.AllowGet);

        }

        [HttpGet]
        public JsonResult getIncome(int IncDet_id)
        {


            return Json(new { data = incomeDetailsservice.GetByID(IncDet_id) }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetDelete(int? incDet_id)
        {
            if (incDet_id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var incomeDetailsVM = incomeDetailsservice.GetByID((int)incDet_id);
            if (incomeDetailsVM == null)
            {
                return HttpNotFound();
            }
            return PartialView(incomeDetailsVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public string Delete(int id)
        {
            return incomeDetailsservice.Delete(id);
        }
    }
}