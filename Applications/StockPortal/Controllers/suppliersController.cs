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

namespace StockPortal.Controllers
{
    [HandleError]
    [CustomAuthorize("الموردين")]
    [SessionExpire]
    public class suppliersController : BaseController
    {
        private StockModel db = new StockModel();
        private SupplierPaymentsService SupplierPaymentService = new SupplierPaymentsService();

        //Created by samir  hussein  دوال تقوم باضافة وتعديل وحذف بيانات الموردين
        #region get method داله عرض بيانات الموردين


        // GET: suppliers
        public ActionResult Index()
        {
            return View(db.suppliers.ToList());
        }

        #endregion

        // GET: suppliers/Details/5
        //public ActionResult Details(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    supplier supplier = db.suppliers.Find(id);
        //    if (supplier == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(supplier);
        //}

        // GET: suppliers/Create

        #region get method داله اضافة بيانات مورد جديد
        public ActionResult Create()
        {
            ViewBag.safes = new SelectList(db.Safes.ToList(), "safe_id", "safe_name");
            supplier supplier = new supplier();
            supplier.BlanceDate = DateTime.Now;
            var lastid = db.suppliers.OrderByDescending(x => x.sup_id).FirstOrDefault();
            supplier.Sup_Serial = lastid != null ? (lastid.sup_id + 1).ToString() : "1";

            return View(supplier);
        }
        #endregion

        #region post method داله اضافة بيانات مورد جديد
        // POST: suppliers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(supplier supplier)
        {
            if (ModelState.IsValid)
            {
                bool check = db.suppliers.Where(m => m.sup_name == supplier.sup_name).Any();
                ViewBag.safes = new SelectList(db.Safes.ToList(), "safe_id", "safe_name");

                if (check)
                {
                    ModelState.AddModelError("sup_name", "هذا المورد  مسجل في سجل الموردين");
                    return View("Create", supplier);
                }
                else if (db.suppliers.FirstOrDefault(x => x.Sup_Serial == supplier.Sup_Serial) != null)
                {
                    ModelState.AddModelError("Sup_Serial", "هذا الكود مسجل من قبل");
                    return View("Create", supplier);
                }
                else
                {
                    if (supplier.OpiningBlance > 0 && supplier.BlanceDate != null && supplier.SafeID > 0)
                    {
                        if(supplier.OpiningBlance> SupplierPaymentService.GetSafeBlance((int)supplier.SafeID))
                        {
                            ViewBag.safes = new SelectList(db.Safes.ToList(), "safe_id", "safe_name");

                            ModelState.AddModelError("OpiningBlance", "رصيد الخزنه لا يكفي");
                            return View("Create", supplier);

                        }
                            db.suppliers.Add(supplier);
                            db.SaveChanges();
                        SupplierPayments SupplierPayment = new SupplierPayments();
                        SupplierPayment.Supplier_ID = supplier.sup_id;
                        SupplierPayment.PaymentNumber = SupplierPaymentService.GetMaxSerial().ToString();
                        SupplierPayment.PaymentDate = supplier.BlanceDate;
                        SupplierPayment.PaymentValue = supplier.OpiningBlance;
                        SupplierPayment.Safe_ID = supplier.SafeID;
                        SupplierPayment.Notes = "رصيد افتتاحي";
                        SupplierPayment.OperationTyp = "-";
                        db.SupplierPayments.Add(SupplierPayment);
                        db.SaveChanges();

                        SafeTransaction safeTransaction = new SafeTransaction();
                        safeTransaction.TransactionValue = (double)SupplierPayment.PaymentValue;
                        safeTransaction.TransactionDate = (DateTime)SupplierPayment.PaymentDate;
                        safeTransaction.SupplierPaymentID = SupplierPayment.ID;
                        safeTransaction.OperationType = "-";
                        safeTransaction.SafeID = SupplierPayment.Safe_ID;
                        db.SafeTransactions.Add(safeTransaction);
                        db.SaveChanges();
                        SafeTransactionService.EditBlanseInSafe((int)SupplierPayment.Safe_ID);

                    }
                    else { 
                    db.suppliers.Add(supplier);
                    db.SaveChanges();
                    }
                    return RedirectToAction("Index");
                }
             
            }
            ViewBag.safes = new SelectList(db.Safes.ToList(), "safe_id", "safe_name");

            return View(supplier);
        }
        #endregion

        #region get method داله تعديل بيانات مورد

        // GET: suppliers/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            supplier supplier = db.suppliers.Find(id);
            if (supplier == null)
            {
                return HttpNotFound();
            }
            return View(supplier);
        }
        #endregion

        #region post method داله تعديل بيانات مورد

        // POST: suppliers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(/*[Bind(Include = "sup_id,sup_name,sup_mobile,sup_mail,contact_name,WhatsApp")]*/ supplier supplier)
        {
            if (ModelState.IsValid)
            {
                bool check = db.suppliers.Where(m => m.sup_id == supplier.sup_id && m.sup_name != supplier.sup_name).Any();
                if (check)
                {
                    ModelState.AddModelError("sup_name", "هذا المورد  مسجل في سجل الموردين");
                    return View("Edit", supplier);
                }
                else if (db.suppliers.FirstOrDefault(x => x.Sup_Serial == supplier.Sup_Serial &&x.sup_id!=supplier.sup_id) != null)
                {
                    ModelState.AddModelError("Sup_Serial", "هذا الكود مسجل من قبل");
                    return View("Edit", supplier);
                }
                else
                {
                    db.Entry(supplier).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
         
            }
            return View(supplier);
        }
        #endregion

        #region  get method داله حذف بيانات مورد

        // GET: suppliers/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            supplier supplier = db.suppliers.Find(id);
            if (supplier == null)
            {
                return HttpNotFound();
            }
            return View(supplier);
        }
        #endregion

        #region  post method داله حذف بيانات مورد
        // POST: suppliers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            supplier supplier = db.suppliers.Find(id);
            db.suppliers.Remove(supplier);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        #endregion



        public ActionResult RemotCreate()
        {
            supplier supplier = new supplier();
            return PartialView();
        }
        [HttpPost]
        public ActionResult RemotCreate([Bind(Include = "sup_name,sup_mobile,sup_mail,contact_name,WhatsApp")] supplier supplier)
        {
            if (ModelState.IsValid)
            {
                bool check = db.suppliers.Where(m => m.sup_name == supplier.sup_name).Any();
                if (check)
                {
                return Json( new { success = false, responseText = "هذا المورد  مسجل في سجل الموردين" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    db.suppliers.Add(supplier);
                    db.SaveChanges();
                    return Json(new
                    {
                        success = true,
                        value = supplier.sup_id,
                        text = supplier.sup_name
                    }, JsonRequestBehavior.AllowGet);
                }

            }
            return Json(new { success = false, responseText = "البيانات غير مكتمله تاكد من ادخال جميع الحقول" }, JsonRequestBehavior.AllowGet);

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
