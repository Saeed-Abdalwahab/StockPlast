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
    [CustomAuthorize("المصروفات اليوميه")]
    [SessionExpire]
    public class ExpenseDetailsController : BaseController
    {
        // GET: ExpenseDetails
        private ExpenseDetailsservice expenseDetailsservice = new ExpenseDetailsservice();
        StockModel db = new StockModel();

        public ActionResult Index()
        {
            ViewBag.expenses = new SelectList(db.Expenses.ToList(), "exp_id", "exp_name");
            ViewBag.safes = new SelectList(db.Safes.ToList(), "safe_id", "safe_name");
            return View();
        }
        [HttpGet]
        public JsonResult GetexpenseDet()
        {
            var expensesdetails = expenseDetailsservice.Getall();
            return Json(new { data = expensesdetails }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult Save(ExpenseDetailsVM expenseDetailsVM)
        {

            return Json(new { msg = expenseDetailsservice.SaveinDatabase(expenseDetailsVM) }, JsonRequestBehavior.AllowGet);

        }

        [HttpGet]
        public JsonResult getExpense(int ExpDet_id)
        {


            return Json(new { data = expenseDetailsservice.GetByID(ExpDet_id) }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetDelete(int? ExpDet_id)
        {
            if (ExpDet_id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var expenseDetailsVM = expenseDetailsservice.GetByID((int)ExpDet_id);
            if (expenseDetailsVM == null)
            {
                return HttpNotFound();
            }
            return PartialView(expenseDetailsVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public bool Delete(int id)
        {
            return expenseDetailsservice.Delete(id);
        }
    }
}