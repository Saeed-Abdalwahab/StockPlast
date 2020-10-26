using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using StockDB.Model;
using StockPortal.Helpers;
using StockPortal.Models;
using StockService;
using StockViewModel;

namespace StockPortal.Controllers
{
    [HandleError]
    [CustomAuthorize("انواع المصروفات")]
    [SessionExpire]
    public class ExpensesController : BaseController
    {
        private ExpenseService expenseService = new ExpenseService();
        // GET: Expenses
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public JsonResult Getexpenses()
        {
            var allexpenses = expenseService.Getall();
            return Json(new { data = allexpenses }, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult SaveExpinse(ExpensesVM expensesVM)
        {

            return Json(new { msg= expenseService.SaveinDatabase(expensesVM) }, JsonRequestBehavior.AllowGet);

        }

        [HttpGet]
        public JsonResult getExpense(int exp_id)
        {

            
            return Json(new { data = expenseService.GetByID(exp_id) }, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult GetDelete(int? exp_id)
        {
            if (exp_id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var expensevm = expenseService.GetByID((int)exp_id);
            if (expensevm == null)
            {
                return HttpNotFound();
            }
            return PartialView(expensevm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public bool Delete(int id)
        {
          return  expenseService.Delete(id);
        }


    }
}
