using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using Newtonsoft.Json;
using StockDB.Model;
using StockPortal.Helpers;
using StockPortal.Models;
using StockService;
using StockViewModel;
using static PlasticStatic.Enums;

namespace StockPortal.Controllers
{
    [HandleError]
    [CustomAuthorize("فاتوره المبيعات")]
    [SessionExpire]
    public class SalesController : BaseController
    {
        private StockModel db = new StockModel();
        private salesService Sale_Service = new salesService();

        public ActionResult Index()
        {
            return View(Sale_Service.Getsalesinvoice());
        }
       
        // GET: Sales/Create
        public ActionResult Create()
        {
            SalesVM salesVM = new SalesVM();

            salesVM.Serial = Sale_Service.GetMaxSerial();
            salesVM.TransDate = DateTime.Now;
                
            ViewBag.customers = new SelectList(db.Customers, "cust_id", "cust_name");
            ViewBag.DemondOrder_demoOrd_id = new SelectList(db.DemondOrders, "demoOrd_id", "demoOrd_serailNum");
            ViewBag.Safes = new SelectList(db.Safes, "safe_id", "safe_name");
   
            return View(salesVM);
        }


        public PartialViewResult fetchDemondorderDetailslist(string DemondorderDetailsids)
        {

            var test = new JavaScriptSerializer().Deserialize<List<int>>(DemondorderDetailsids);
            
            var list = Sale_Service.GetfinishedDemondorderdetails(test);
            return PartialView("_DemondorderDetails", list);
        }

        public PartialViewResult fetchDemondorderDetailslistForEdit(string DemondorderDetailsids,string fromstockid)
        {

            var test = new JavaScriptSerializer().Deserialize<List<int>>(DemondorderDetailsids);

            var list = Sale_Service.GetfinishedDemondorderdetailsForEdit(test,int.Parse(fromstockid));
            return PartialView("_DemondorderDetails", list);
        }
        public PartialViewResult fetchDemondorderDetailslistForDetails(string DemondorderDetailsids, string fromstockid)
        {

            var test = new JavaScriptSerializer().Deserialize<List<int>>(DemondorderDetailsids);
            ViewBag.Details = true;
            var list = Sale_Service.GetfinishedDemondorderdetailsForDetails(test, int.Parse(fromstockid));
            return PartialView("_DemondorderDetails", list);
        }
        public JsonResult filldemondorderlist(int Custid)
        {
            var list = Sale_Service.GetfinishedDemondorderdetails(Custid).Select(x=>new
           {
               ID = x.demoOrdDet_id,
               Text =x.Shaplona.shap_name,

           }).Distinct().ToList();
            var custinfo = Sale_Service.Custinfo(Custid);
            return Json(new { Demondorderdetailslist=list , CustAddress= custinfo.cust_address, custphone=custinfo.cust_mobile}, JsonRequestBehavior.AllowGet);
        }
        public JsonResult filldemondorderlistForEdit(int Custid)
        {
            var list = Sale_Service.GetfinishedDemondorderdetailsForEdit(Custid).Select(x => new
            {
                ID = x.demoOrdDet_id,
                Text = x.Shaplona.shap_name,

            }).Distinct().ToList();
            var custinfo = Sale_Service.Custinfo(Custid);
            return Json(new { Demondorderdetailslist = list, CustAddress = custinfo.cust_address, custphone = custinfo.cust_mobile }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult filldemondorderlistForDetails(int Custid)
        {
            var list = Sale_Service.GetfinishedDemondorderdetailsFodetails(Custid).Select(x => new
            {
                ID = x.demoOrdDet_id,
                Text = x.Shaplona.shap_name,

            }).Distinct().ToList();
            var custinfo = Sale_Service.Custinfo(Custid);
            return Json(new { Demondorderdetailslist = list, CustAddress = custinfo.cust_address, custphone = custinfo.cust_mobile }, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public ActionResult Create(SalesVM salesvm)
        {

            if (Sale_Service.SaveinDb(salesvm))
            {
                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }
                return Json(new { success = false }, JsonRequestBehavior.AllowGet);


        }

        // GET: Sales/Edit/5
        public ActionResult Edit(int? fromstockid)
        {
            if (fromstockid == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FromStock fromStock = db.FromStocks.Find(fromstockid);
            SalesVM salesVM = new SalesVM();
         salesVM.stocktransactionlist= db.StockTransactions.Where(x=>x.FromStockId==fromStock.ID&&x.OperationType=="-").ToList();

         salesVM.notes  =fromStock.notes              ;
         salesVM.cust_ID  =fromStock.DemondOrder.cust_id              ;
         salesVM.sale_charge  = fromStock.sale_charge       ;
         salesVM.sale_discount  = fromStock.sale_discount     ;
            salesVM.Sale_ID = fromStock.ID;
         salesVM.Serial  = fromStock.Serial  ;
         salesVM.TransDate  = fromStock.TransDate         ;
         salesVM.TotalCost  = fromStock.invoicecost       ;
         salesVM.invoicestatus=(InvoiceSSAlestatus)            fromStock.Status                    ;

            var InvocCostOrignial = salesVM.stocktransactionlist.Sum(x => x.ItemPrice *x.Quantity+(x.DemondOrderDetail.Hand_Price!=null&&x.DemondOrderDetail.Hand_Count!=null?x.DemondOrderDetail.Hand_Price*x.DemondOrderDetail.Hand_Count:0));
            var TaxPercentage = fromStock.sale_tax / (InvocCostOrignial - salesVM.sale_discount) * 100;
            salesVM.sale_tax = TaxPercentage;

            ViewBag.customers = new SelectList(db.Customers, "cust_id", "cust_name", salesVM.cust_ID);
            ViewBag.Safes = new SelectList(db.Safes, "safe_id", "safe_name");
            return View(salesVM);
        }

        public ActionResult Details(int? fromstockid)
        {
            if (fromstockid == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FromStock fromStock = db.FromStocks.Find(fromstockid);
            SalesVM salesVM = new SalesVM();
            salesVM.stocktransactionlist = db.StockTransactions.Where(x => x.FromStockId == fromStock.ID && x.OperationType == "-").ToList();

            salesVM.notes = fromStock.notes;
            salesVM.cust_ID = fromStock.DemondOrder.cust_id;
            salesVM.sale_charge = fromStock.sale_charge;
            salesVM.sale_discount = fromStock.sale_discount;
            salesVM.Sale_ID = fromStock.ID;
            salesVM.Serial = fromStock.Serial;
            salesVM.TransDate = fromStock.TransDate;
            salesVM.TotalCost = fromStock.invoicecost;
            salesVM.invoicestatus = (InvoiceSSAlestatus)fromStock.Status;

            var InvocCostOrignial = salesVM.stocktransactionlist.Sum(x => x.ItemPrice * x.Quantity + (x.DemondOrderDetail.Hand_Price != null && x.DemondOrderDetail.Hand_Count != null ? x.DemondOrderDetail.Hand_Price * x.DemondOrderDetail.Hand_Count : 0));
            var TaxPercentage = fromStock.sale_tax / (InvocCostOrignial - salesVM.sale_discount) * 100;
            salesVM.sale_tax = TaxPercentage;

            ViewBag.customers = new SelectList(db.Customers, "cust_id", "cust_name", salesVM.cust_ID);
            ViewBag.Safes = new SelectList(db.Safes, "safe_id", "safe_name");
            return View(salesVM);
        }

        [HttpPost]
        public ActionResult Edit(SalesVM salesvm)
        {
            if (Sale_Service.SaveEditinDb(salesvm))
            {
                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { success = false }, JsonRequestBehavior.AllowGet);

        }


        // POST: Sales/Delete/5
        public JsonResult Delete(int ID)
        {
         
                return Json(new { status = Sale_Service.Delete(ID) }, JsonRequestBehavior.AllowGet);
          
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
