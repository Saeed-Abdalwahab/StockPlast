using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using PlasticStatic;
using StockDB.Model;
using StockPortal.Helpers;
using StockPortal.Models;

namespace StockPortal.Controllers
{
    [HandleError]
    [CustomAuthorize("الخزائن")]

    [SessionExpire]
    public class SafesController : Controller
    {
        private StockModel db = new StockModel();

        // GET: Safes
        public ActionResult Index()
        {

            var safes = db.Safes.Include(s => s.Employe);
            return View(safes.ToList());
        }

        // GET: Safes/Details/5
        //public ActionResult Details(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Safe safe = db.Safes.Find(id);
        //    if (safe == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(safe);
        //}




        // GET: Safes/Create
        [HttpGet]
        public ActionResult Create()
        {
         
            int SafekeeperEnum = int.Parse(Enums.EnumString.GetStringValue(Enums.job.Safekeeper));
            var employes = db.Employes.Include(e => e.Job).Where(p => p.Job_id == SafekeeperEnum).ToList();
            ViewBag.emp_id = new SelectList(employes, "emp_id", "emp_name");
            Safe safe = new Safe();
            safe.safe_date = DateTime.Now;
            return View(safe);
        }

        // POST: Safes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "safe_id,emp_id,safe_name,safe_totalBalance,safe_date")] Safe safe)
        {
            bool checkname = db.Safes.Where(m => m.safe_name == safe.safe_name).Any();
            if (ModelState.IsValid)
            {
                if (checkname)
                {
                    ModelState.AddModelError("safe_name", "هذه الخزينة موجود بسجل الخزائن");
                    int SafekeeperEnum = int.Parse(Enums.EnumString.GetStringValue(Enums.job.Safekeeper));
                    var employes = db.Employes.Include(e => e.Job).Where(p => p.Job_id == SafekeeperEnum).ToList();
 

                }
                else
                {
                    int SafekeeperEnum = int.Parse(Enums.EnumString.GetStringValue(Enums.job.Safekeeper));
                    var employes = db.Employes.Include(e => e.Job).Where(p => p.Job_id == SafekeeperEnum).ToList();
                    ViewBag.emp_id = new SelectList(employes, "emp_id", "emp_name");
                    db.Safes.Add(safe);
                    db.SaveChanges();
                    SafeTransaction safeTransaction = new SafeTransaction();
                    safeTransaction.SafeID = safe.safe_id;
                    safeTransaction.TransactionDate = safe.safe_date;
                    safeTransaction.OperationType = "+";
                    safeTransaction.TransactionValue = Convert.ToDouble(safe.safe_totalBalance);
                    db.SafeTransactions.Add(safeTransaction);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
              
            }
            int SafekeeperEnum1 = int.Parse(Enums.EnumString.GetStringValue(Enums.job.Safekeeper));
            var employes1 = db.Employes.Include(e => e.Job).Where(p => p.Job_id == SafekeeperEnum1).ToList();
            ViewBag.emp_id = new SelectList(employes1, "emp_id", "emp_name");
            //ViewBag.emp_id = new SelectList(db.Employes, "emp_id", "emp_name", safe.emp_id);
            return View("Create", safe);
        }

        // GET: Safes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Safe safe = db.Safes.Find(id);
            if (safe == null)
            {
                return HttpNotFound();
            }
            int SafekeeperEnum = int.Parse(Enums.EnumString.GetStringValue(Enums.job.Safekeeper));
            var employes = db.Employes.Include(e => e.Job).Where(p => p.Job_id == SafekeeperEnum).ToList();
            ViewBag.emp_id = new SelectList(employes, "emp_id", "emp_name");
            return View(safe);
        }

        // POST: Safes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "safe_id,emp_id,safe_name,safe_totalBalance,safe_date")] Safe safe)
        {
            bool checkname = db.Safes.Where(m => m.safe_name == safe.safe_name && m.safe_id!=safe.safe_id).Any();
            int SafekeeperEnum = int.Parse(Enums.EnumString.GetStringValue(Enums.job.Safekeeper));
            var employes = db.Employes.Include(e => e.Job).Where(p => p.Job_id == SafekeeperEnum).ToList();
            ViewBag.emp_id = new SelectList(employes, "emp_id", "emp_name");
            if (ModelState.IsValid)
            {
                SafeTransaction safeTransaction = db.SafeTransactions.FirstOrDefault(x => x.SafeID == safe.safe_id && x.CustomerPaymentID == null && x.DemondorderID == null && x.ExpenseDetailsID == null && x.IncomeDetailsID == null && x.SupplierPaymentID == null && x.OperationType == "+");
                Safe oldSafe = db.Safes.Find(safe.safe_id);

               
                 if (oldSafe.safe_totalBalance != safe.safe_totalBalance || oldSafe.safe_date != safe.safe_date)
                {
                    if (db.SafeTransactions.FirstOrDefault(x => x.SafeID == safe.safe_id && (x.SupplierPaymentID != null || x.CustomerPaymentID != null || x.IncomeDetailsID != null || x.ExpenseDetailsID != null || x.DemondorderID != null)) != null)

                    {
                        ModelState.AddModelError("safe_totalBalance", "لا يمكن تعديل الرصيد الافتتاحي للخزنه بعد ان تمت عليها حركات ");
                        ModelState.AddModelError("safe_date", "لا يمكن تعديل تاريخ الخزنه بعد ان تمت عليها حركات");
          
                        return View(safe);
                    }
                }
                if (checkname)
                {
                    ModelState.AddModelError("safe_name", "هذه الخزينة موجود بسجل الخزائن");
                }
                else
                {
                    oldSafe.safe_date = safe.safe_date;
                    oldSafe.safe_name = safe.safe_name;
                    oldSafe.safe_totalBalance = safe.safe_totalBalance;
                    oldSafe.emp_id = safe.emp_id;
                    db.Entry(oldSafe).State = EntityState.Modified;
                    db.SaveChanges();
                    if (safeTransaction != null)
                    {
                        safeTransaction.TransactionValue = Convert.ToDouble(safe.safe_totalBalance);
                        safeTransaction.TransactionDate = safe.safe_date;
                        db.Entry(safeTransaction).State = EntityState.Modified;
                        db.SaveChanges();

                    }
                    return RedirectToAction("Index");

                }
                 
                

            }
         
            return View(safe);
        }

        // GET: Safes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Safe safe = db.Safes.Find(id);
            if (safe == null)
            {
                return HttpNotFound();
            }
            return View(safe);
        }

        // POST: Safes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Safe safe = db.Safes.Find(id);
            db.Safes.Remove(safe);
            db.SaveChanges();
            return RedirectToAction("Index");
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
