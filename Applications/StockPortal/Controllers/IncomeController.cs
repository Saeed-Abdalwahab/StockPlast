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
    [CustomAuthorize("انواع الايرادات")]
    [SessionExpire]
    public class IncomeController : BaseController
    {
        private IncomeService incomeService = new IncomeService();
        private StockModel db = new StockModel();
        // GET: Income
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public JsonResult GetIncomes()
        {
            var allincomes = incomeService.Getall();
            return Json(new { data = allincomes }, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult SaveIncome(IncomeVM incomeVM)
        {

            return Json(new { msg = incomeService.SaveinDatabase(incomeVM) }, JsonRequestBehavior.AllowGet);

        }

        [HttpGet]
        public JsonResult getIncome(int inc_id)
        {


            return Json(new { data = incomeService.GetByID(inc_id) }, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult GetDelete(int? inc_id)
        {
            if (inc_id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var incomevm = incomeService.GetByID((int)inc_id);
            if (incomevm == null)
            {
                return HttpNotFound();
            }
            if (db.IncomeDetails.Any(a => a.Income_inc_id == inc_id) == true) 
            {
                new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            return PartialView(incomevm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public bool Delete(int id)
        {
            
            var x= incomeService.Delete(id);      
            return x;
            
               

        }




    }
}