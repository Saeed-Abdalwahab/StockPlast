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
    [CustomAuthorize("أوامر الشغل")]
    [SessionExpire]
    public class DemondOrdersController : BaseController
    {
        private StockModel db = new StockModel();

        

        public ActionResult Index()
        {
            var demondOrder = db.DemondOrders.ToList();
            return View(demondOrder);
        }


        // GET: DemondOrders/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DemondOrder demondOrder = db.DemondOrders.Find(id);
            if (demondOrder == null)
            {
                return HttpNotFound();
            }
            return View(demondOrder);
        }

        // GET: DemondOrders/Create
        public ActionResult Create()
        {
            StockPortal.ViewModel.DemondOrderVM demondOrderVM = new ViewModel.DemondOrderVM();
            demondOrderVM.demoOrd_date = DateTime.Now.Date;
            var demondOrderObj = db.DemondOrders.OrderByDescending(x => x.demoOrd_id).FirstOrDefault();
            demondOrderVM.demoOrd_serailNum=(demondOrderObj==null?1: (demondOrderObj.demoOrd_id + 1)).ToString();
            var List = db.Customers.Select(x => new SelectListItem
            {
                Value = x.cust_id.ToString(),
                Text = x.cust_name + " -- " + x.cust_mobile.ToString()
            });
            ViewBag.Customers = new SelectList(List, "Value", "Text");
            ViewBag.safe_id = new SelectList(returnSafeList(), "ID", "Name");
            return View(demondOrderVM);
        }

   
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(StockPortal.ViewModel.DemondOrderVM demondOrder)
        {

            var List = db.Customers.Select(x => new SelectListItem
            {
                Value = x.cust_id.ToString(),
                Text = x.cust_name + " -- " + x.cust_mobile.ToString()
            });
            if (ModelState.IsValid)
            {
                if (db.DemondOrders.FirstOrDefault(x => x.demoOrd_serailNum == demondOrder.demoOrd_serailNum) != null)
                {
                    ViewBag.safe_id = new SelectList(returnSafeList(), "ID", "Name");

                    ViewBag.Customers = new SelectList(List, "Value", "Text", demondOrder.cust_id); ViewBag.safe_id = new SelectList(returnSafeList(), "ID", "Name");
                    ModelState.AddModelError("demoOrd_serailNum", "هذا الرقم مسجل من قبل");
                    return View(demondOrder);
                }
                if (demondOrder.demoOrd_deposit != null)
                {
                    if (demondOrder.safe_id == null)
                    {
            ViewBag.safe_id = new SelectList(returnSafeList(), "ID", "Name");

                        ViewBag.Customers = new SelectList(List, "Value", "Text", demondOrder.cust_id); ViewBag.safe_id = new SelectList(returnSafeList(), "ID", "Name");
                        ViewBag.errorMsg = "في حاله تحديد العربون لابد من ادخال الخزنه";
                        return View(demondOrder);
                    }
                }
                StockDB.Model.DemondOrder DemondOrderObj = new DemondOrder();
                
                DemondOrderObj.cust_id = demondOrder.cust_id;
                DemondOrderObj.safe_id = demondOrder.safe_id==null||demondOrder.demoOrd_deposit==null?null: demondOrder.safe_id;
                DemondOrderObj.demoOrd_serailNum = demondOrder.demoOrd_serailNum;
                DemondOrderObj.demoOrd_date = demondOrder.demoOrd_date;
                DemondOrderObj.demoOrd_notes = demondOrder.demoOrd_notes;
                DemondOrderObj.demoOrd_deposit = demondOrder.demoOrd_deposit;
                db.DemondOrders.Add(DemondOrderObj);
                db.SaveChanges();
                if(DemondOrderObj.safe_id != null&&DemondOrderObj.demoOrd_deposit!=null)
                {
                    CustomerPayment customerPayment = new CustomerPayment();
                    var CustPaymentCheck = db.CustomerPayments.OrderByDescending(x => x.ID).FirstOrDefault();
                    var MaxSerial = CustPaymentCheck!=null?CustPaymentCheck.ID+1:1;
                    customerPayment.Customer_ID = DemondOrderObj.cust_id;
                    customerPayment.OperationTyp = "+";
                    customerPayment.PaymentNumber = MaxSerial.ToString();
                    customerPayment.PaymentDate = DemondOrderObj.demoOrd_date;
                    customerPayment.PaymentValue = DemondOrderObj.demoOrd_deposit;
                    customerPayment.Safe_ID = DemondOrderObj.safe_id;
                    db.CustomerPayments.Add(customerPayment);
                    db.SaveChanges();

                    SafeTransaction safeTransaction = new SafeTransaction();
                    safeTransaction.DemondorderID = DemondOrderObj.demoOrd_id;
                    safeTransaction.TransactionDate = demondOrder.demoOrd_date;
                    safeTransaction.TransactionValue = (double)demondOrder.demoOrd_deposit;
                    safeTransaction.SafeID = demondOrder.safe_id;
                    safeTransaction.CustomerPaymentID = customerPayment.ID;

                    safeTransaction.OperationType = "+";
                    db.SafeTransactions.Add(safeTransaction);
                    db.SaveChanges();
                    SafeTransactionService.EditBlanseInSafe((int)DemondOrderObj.safe_id);

                }
                return RedirectToAction("Edit", new { orderD = DemondOrderObj.demoOrd_id });
            }

            ViewBag.Customers = new SelectList(List, "Value", "Text", demondOrder.cust_id); ViewBag.safe_id = new SelectList(returnSafeList(), "ID", "Name");
            return View(demondOrder);
        }

        // GET: DemondOrders/Edit/5
        public ActionResult Edit(int? orderD)
        {
            if (orderD == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            DemondOrder demondOrder = db.DemondOrders.Include(ee => ee.Customer).FirstOrDefault(ee => ee.demoOrd_id == orderD.Value);
            StockPortal.ViewModel.DemondOrderVM DemondOrderVMObj = new ViewModel.DemondOrderVM();
            DemondOrderVMObj.demoOrd_id = demondOrder.demoOrd_id;
            DemondOrderVMObj.cust_id = demondOrder.cust_id;
            DemondOrderVMObj.safe_id = demondOrder.safe_id;
            DemondOrderVMObj.SafeName = demondOrder.safe_id != null ? demondOrder.Safe.safe_name : "";
            DemondOrderVMObj.demoOrd_serailNum = demondOrder.demoOrd_serailNum;
            DemondOrderVMObj.demoOrd_date = demondOrder.demoOrd_date;
            DemondOrderVMObj.demoOrd_notes = demondOrder.demoOrd_notes;
            DemondOrderVMObj.demoOrd_deposit = demondOrder.demoOrd_deposit;
            if (demondOrder == null)
            {
                return HttpNotFound();
            }
            ViewBag.CusName = demondOrder.Customer.cust_name;
            ViewBag.demOrderDate = demondOrder.demoOrd_date.ToString("yyyy/MM/dd");
            var List = db.Customers.Select(x => new SelectListItem
            {
                Value = x.cust_id.ToString(),
                Text = x.cust_name + " -- " + x.cust_mobile.ToString()
            });
            ViewBag.Customers = new SelectList(List, "Value", "Text", demondOrder.cust_id);
            ViewBag.safe_id = new SelectList(returnSafeList(), "ID", "Name",demondOrder.safe_id);
            return View(DemondOrderVMObj);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(StockPortal.ViewModel.DemondOrderVM demondOrder, int orderD)
        {
            var List = db.Customers.Select(x => new SelectListItem
            {
                Value = x.cust_id.ToString(),
                Text = x.cust_name + " -- " + x.cust_mobile.ToString()
            });

            StockDB.Model.DemondOrder DemondOrderObj = db.DemondOrders.Find(demondOrder.demoOrd_id);
            if (ModelState.IsValid)
            {
                if (db.DemondOrders.FirstOrDefault(x => x.demoOrd_serailNum == demondOrder.demoOrd_serailNum&&x.demoOrd_id!=demondOrder.demoOrd_id) != null)
                {
                    ViewBag.safe_id = new SelectList(returnSafeList(), "ID", "Name");

                    ViewBag.Customers = new SelectList(List, "Value", "Text", demondOrder.cust_id); 
                    ViewBag.safe_id = new SelectList(returnSafeList(), "ID", "Name");
                    ModelState.AddModelError("demoOrd_serailNum", "هذا الرقم مسجل من قبل");
                    return View(demondOrder);
                }
                if (db.DemondOrderDetails.FirstOrDefault(x=>x.demoOrd_id==demondOrder.demoOrd_id) != null&&db.DemondOrders.Find(demondOrder.demoOrd_id).cust_id != demondOrder.cust_id)
                {
                    ViewBag.Customers = new SelectList(List, "Value", "Text", demondOrder.cust_id); ViewBag.safe_id = new SelectList(returnSafeList(), "ID", "Name");

                    ModelState.AddModelError("cust_id", "غير مسموح بالتعديل حاليا");

                    return View();
                }
                if (demondOrder.demoOrd_deposit != null)
                {
                    if (demondOrder.safe_id != DemondOrderObj.safe_id)
                    {
                        if (SafeBlance((int)demondOrder.safe_id, demondOrder.demoOrd_id)  < 0)
                        {
                            ViewBag.Customers = new SelectList(List, "Value", "Text", demondOrder.cust_id); ViewBag.safe_id = new SelectList(returnSafeList(), "ID", "Name");
                            ViewBag.errorMsg = "رصيد الخزينه لا يسمح بتغير الخزنه ";
                            return View(demondOrder);
                        }
                    }
                    if (demondOrder.safe_id == null)
                    {
                        
                        ViewBag.Customers = new SelectList(List, "Value", "Text", demondOrder.cust_id); ViewBag.safe_id = new SelectList(returnSafeList(), "ID", "Name");
                        ViewBag.errorMsg = "في حاله تحديد العربون لابد من ادخال الخزنه";
                        return View(demondOrder);
                    }
                    
                    else
                    {
                        if (SafeBlance((int)demondOrder.safe_id, demondOrder.demoOrd_id) + demondOrder.demoOrd_deposit < 0)
                        {
                            ViewBag.Customers = new SelectList(List, "Value", "Text", demondOrder.cust_id); ViewBag.safe_id = new SelectList(returnSafeList(), "ID", "Name");
                            ViewBag.errorMsg = "رصيد الخزينه لا يسمح بقيمه العربون ";
                            return View(demondOrder);
                        }
                    }
                }
              
                
                DemondOrderObj.demoOrd_id = demondOrder.demoOrd_id;
                DemondOrderObj.cust_id = demondOrder.cust_id;
                DemondOrderObj.safe_id = demondOrder.safe_id;
                DemondOrderObj.demoOrd_serailNum = demondOrder.demoOrd_serailNum;
                DemondOrderObj.demoOrd_date = demondOrder.demoOrd_date;
                DemondOrderObj.demoOrd_notes = demondOrder.demoOrd_notes;
                DemondOrderObj.demoOrd_deposit = demondOrder.demoOrd_deposit;

                db.Entry(DemondOrderObj).State = EntityState.Modified;
                db.SaveChanges();
                var SafeTransactionEdit = db.SafeTransactions.FirstOrDefault(x => x.DemondorderID == demondOrder.demoOrd_id);
                if (SafeTransactionEdit != null)
                {


                    SafeTransactionEdit.TransactionDate = demondOrder.demoOrd_date;
                    SafeTransactionEdit.TransactionValue = (double)demondOrder.demoOrd_deposit;
                    SafeTransactionEdit.SafeID = demondOrder.safe_id;

                    db.Entry(SafeTransactionEdit).State = EntityState.Modified;
                    db.SaveChanges();
                    SafeTransactionService.EditBlanseInSafe((int)DemondOrderObj.safe_id);
                    CustomerPayment customerPayment = db.CustomerPayments.FirstOrDefault(x => x.ID == SafeTransactionEdit.CustomerPaymentID);
                    if (customerPayment != null)
                    {
                        customerPayment.Customer_ID = DemondOrderObj.cust_id;
                        customerPayment.PaymentDate = DemondOrderObj.demoOrd_date;
                        customerPayment.PaymentValue = DemondOrderObj.demoOrd_deposit;
                        customerPayment.Safe_ID = DemondOrderObj.safe_id;
                        db.Entry(customerPayment).State = EntityState.Modified;
                        db.SaveChanges();

                    }

                }
                //else
                //{

                //    SafeTransaction safeTransaction = new SafeTransaction();
                //    safeTransaction.DemondorderID = DemondOrderObj.demoOrd_id;
                //    safeTransaction.TransactionDate = demondOrder.demoOrd_date;
                //    safeTransaction.TransactionValue = (double)demondOrder.demoOrd_deposit;
                //    safeTransaction.SafeID = demondOrder.safe_id;
                //    safeTransaction.OperationType = "+";
                //    db.SafeTransactions.Add(safeTransaction);
                //    db.SaveChanges();
                //}
           
                //else
                //{
                //    var CustPaymentCheck = db.CustomerPayments.OrderByDescending(x => x.ID).FirstOrDefault();
                //    var MaxSerial = CustPaymentCheck != null ? CustPaymentCheck.ID + 1 : 1;
                //    customerPayment.Customer_ID = DemondOrderObj.cust_id;
                //    customerPayment.OperationTyp = "+";
                //    customerPayment.PaymentNumber = MaxSerial.ToString();
                //    customerPayment.PaymentDate = DemondOrderObj.demoOrd_date;
                //    customerPayment.PaymentValue = DemondOrderObj.demoOrd_deposit;
                //    customerPayment.Safe_ID = DemondOrderObj.safe_id;
                //    db.CustomerPayments.Add(customerPayment);
                //    db.SaveChanges();

                //}

                ViewBag.success = "تم التعديل بنجاح";
                

                return RedirectToAction("Edit", new { orderD = orderD });
            }
            ViewBag.demOrderDate = demondOrder.demoOrd_date.ToString("yyyy/MM/dd");
            ViewBag.safe_id = ViewBag.safe_id = new SelectList(returnSafeList(), "ID", "Name");

            return View(demondOrder);
        }


        // POST: DemondOrders/Delete/5
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                DemondOrder demondOrder = db.DemondOrders.Find(id);
                SafeTransaction safeTransactionDelete = db.SafeTransactions.FirstOrDefault(x => x.DemondorderID == id);
                
                if (safeTransactionDelete != null)
                {
                    if(SafeBlance((int)safeTransactionDelete.SafeID, (int)safeTransactionDelete.DemondorderID) < 0)
                    {
                        return Json(new { success = false, msg = "رصيد الخزنه لا يسمح بعمليه الحذف" }, JsonRequestBehavior.AllowGet);

                    }
                    else
                    {
                        var safeid = safeTransactionDelete.SafeID;
                        db.SafeTransactions.Remove(safeTransactionDelete);
                        db.SaveChanges();
                        SafeTransactionService.EditBlanseInSafe((int)safeid);

                    }
                }
                if (demondOrder == null)
                {
                    return Json(new { success = false, msg = "هذ الامر غير مسجل او تم حذفه من قبل" }, JsonRequestBehavior.AllowGet);
                }
                db.DemondOrders.Remove(demondOrder);
                db.SaveChanges();
                return Json(new { success = true, msg = "تمت عمليه الحذف بنجاح" }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "حدثه مشكله ف البيانات");
            }

        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        #region methods 

        private List<ViewModel.BasicClass> returnSafeList()
        {
            List<ViewModel.BasicClass> returnSafeList = new List<ViewModel.BasicClass>();
            List<StockDB.Model.Safe> safeList = db.Safes.Include(ee => ee.Employe).ToList();
            foreach (var item in safeList)
            {
                returnSafeList.Add(new ViewModel.BasicClass { ID = item.safe_id, Name = item.safe_name });
            }
            return returnSafeList;
        }
        public List<SelectListItem> returnAllSafeDDL(int? selectedSafeId)
        {
            List<SelectListItem> list = new List<SelectListItem>();
            List<StockDB.Model.Safe> safeList = db.Safes.Include(ee => ee.Employe).ToList();
            foreach (StockDB.Model.Safe item in safeList)
            {
                list.Add(new SelectListItem { Value = item.safe_id.ToString(), Text = item.safe_name, Selected = (item.safe_id == selectedSafeId ? true : false) });
            }
            return list;
        }

        private double SafeBlance(int SafeID,int DemondorderID)
        {
            var val = db.SafeTransactions.Where(x => x.SafeID == SafeID && x.DemondorderID != DemondorderID).Sum(x=>x.OperationType=="+"?x.TransactionValue:-x.TransactionValue);
            return val;
        }

        #endregion 
    }
}
