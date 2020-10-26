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
    [CustomAuthorize("العملاء")]

    [SessionExpire]
    public class CustomersController : BaseController
    {
        private StockModel db = new StockModel();
        private SafeTransactionService SafeTransactionService = new SafeTransactionService();
        private CustomerPaymentService customerPaymentService = new CustomerPaymentService();
        #region get method داله عرض بيانات العملاء
        // GET: Customers
        public ActionResult Index()
        {
            return View(db.Customers.ToList());
        }
        #endregion

        // GET: Customers/Details/5
        //public ActionResult Details(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Customer customer = db.Customers.Find(id);
        //    if (customer == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(customer);
        //}

        // GET: Customers/Create

        #region get method داله اضافة بيانات عميل جديد
        public ActionResult Create()
        {
            Customer customer = new Customer();
            var cust = db.Customers.OrderByDescending(x => x.cust_id).FirstOrDefault();
            customer.custSerial = (cust!=null?cust.cust_id + 1:1).ToString();
            ViewBag.safes = new SelectList(db.Safes.ToList(), "safe_id", "safe_name");
            customer.BlanceDate = DateTime.Now; 
            return View(customer);
        }
        #endregion

        #region post method داله اضافة بيانات عميل جديد

        // POST: Customers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Customer customer)
        {

            if (ModelState.IsValid)
            {
                bool checkname = db.Customers.Where(m => m.cust_name == customer.cust_name).Any();
                bool checkid = db.Customers.Where(m => m.custSerial == customer.custSerial).Any();

                if (checkname)
                {
                    ModelState.AddModelError("cust_name","هذا العميل موجود بسجل العملاء");
                }
                else if (checkid)
                {
                    ModelState.AddModelError("custSerial", "هذا العميل موجود بسجل العملاء");
                }
                else
                {
                    db.Customers.Add(customer);
                    db.SaveChanges();
                    if (customer.BlanceDate != null && customer.OpiningBlance > 0 && customer.SafeID > 0)
                    {
                        CustomerPayment customerPayment = new CustomerPayment();
                        customerPayment.Customer_ID = customer.cust_id;
                        customerPayment.Safe_ID = customer.SafeID;
                        customerPayment.PaymentValue = customer.OpiningBlance;
                        customerPayment.PaymentDate = customer.BlanceDate;
                        customerPayment.Notes = "رصيد افتتاحي";
                        customerPayment.PaymentNumber = customerPaymentService.GetMaxSerial().ToString();
                        customerPayment.OperationTyp = "+";
                        db.CustomerPayments.Add(customerPayment);
                        db.SaveChanges();

                        SafeTransaction safeTransaction = new SafeTransaction();
                        safeTransaction.TransactionValue = (double)customerPayment.PaymentValue;
                        safeTransaction.TransactionDate = (DateTime)customerPayment.PaymentDate;
                        safeTransaction.CustomerPaymentID = customerPayment.ID;
                        safeTransaction.OperationType = "+";
                        safeTransaction.SafeID = customerPayment.Safe_ID;
                        db.SafeTransactions.Add(safeTransaction);
                        db.SaveChanges();
                        SafeTransactionService.EditBlanseInSafe((int)customerPayment.Safe_ID);

                    }
                    return RedirectToAction("Index" , customer);
                }

            }
            ViewBag.safes = new SelectList(db.Safes.ToList(), "safe_id", "safe_name");

            return View(customer);
        }
        #endregion

        #region get method داله تعديل بيانات عميل

        // GET: Customers/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Customer customer = db.Customers.Find(id);
            if (customer == null)
            {
                return HttpNotFound();
            }
            return View(customer);
        }
        #endregion

        #region post method داله تعديل بيانات عميل

        // POST: Customers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "cust_id,custSerial,cust_name,cust_mobile,cust_mail,contact_name,cust_address,WhatsApp")] Customer customer)
        {
            bool checkname = db.Customers.Where(m => m.cust_name == customer.cust_name && m.cust_id !=customer.cust_id).Any();
            bool checkid = db.Customers.Where(m => m.custSerial == customer.custSerial && m.cust_id != customer.cust_id).Any();
            if (ModelState.IsValid)
            {
                if (checkname)
                {
                    ModelState.AddModelError("cust_name", "هذا العميل موجود بسجل العملاء");
                }
                else if (checkid)
                {
                    ModelState.AddModelError("custSerial", "هذا العميل موجود بسجل العملاء");
                }
                else
                {
                    db.Entry(customer).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
 
            }
            return View(customer);
        }
        #endregion

        #region get method داله حذف بيانات بيانات عميل


        // GET: Customers/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Customer customer = db.Customers.Find(id);
            if (customer == null)
            {
                return HttpNotFound();
            }
            return View(customer);
        }
        #endregion

        #region get method داله حذف بيانات بيانات عميل
        // POST: Customers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Customer customer = db.Customers.Find(id);
            db.Customers.Remove(customer);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        #endregion


        public ActionResult RemotCreate()
        {
            Customer customer = new Customer();
            var cust = db.Customers.OrderByDescending(x => x.cust_id).FirstOrDefault();
            customer.custSerial = (cust != null ? cust.cust_id + 1 : 1).ToString();
            return PartialView(customer);
        }
        [HttpPost]
        public ActionResult RemotCreate([Bind(Include = "custSerial,cust_name,cust_mobile,cust_mail,contact_name,cust_address,WhatsApp")] Customer customer)
        {
            if (ModelState.IsValid)
            {
                var checkid = db.Customers.FirstOrDefault(m => m.custSerial == customer.custSerial);

                 if (checkid!=null)
                {
                    return Json(new { success = false, responseText = "هذا السريل موجود بسجل العملاء" }, JsonRequestBehavior.AllowGet);

                }
                else
                {
                    db.Customers.Add(customer);
                    db.SaveChanges();
                    return Json(new
                    {
                        success = true,
                        value = customer.cust_id,
                        text = customer.cust_name
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
