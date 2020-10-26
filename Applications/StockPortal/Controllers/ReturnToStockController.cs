using StockPortal.Helper;
using StockService;
using StockViewModel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static PlasticStatic.Enums;

namespace StockPortal.Controllers
{
    public class ReturnToStockController : Controller
    {
        StockDB.Model.StockModel db = new StockDB.Model.StockModel();


        //Gett All Expired 'invoice' from to sotck based on fromstockiid
        public ActionResult GetreturnedItemByItemID(int FromStockID, int Demondorderdetailsid)
        {
            expireditemService FS = new expireditemService();

            AlertVM<PurchaseInvoiceViewModel> Alert = new AlertVM<PurchaseInvoiceViewModel>();

            var Resault = FS.GetreturnedTostockList(FromStockID, Demondorderdetailsid);

            if (Resault.type)
            {
                Alert.type = Resault.type;
                Alert.data = Resault.data;


                return Json(new { data = Alert.data, Alert = Alert }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                Alert.type = Resault.type;
                Alert.ErrorMessage = "حدث حطاء حاول مرة اخره";
                Alert.AlertType = ErrorMessageEnum.danger.ToString();
                return Json(new { data = Alert.data, Alert = Alert }, JsonRequestBehavior.AllowGet);
            }
        }



        //Add New Purchieses for returned
        [HttpGet]
        public ActionResult AddTOstockreturned(string ToStockid, string FromStockid, string itemsupID, string demondorderdetailsid = "0")
        {
            int ToStockID = Convert.ToInt32(ToStockid);
            int FromStock = Convert.ToInt32(FromStockid);
            int itemsupid = Convert.ToInt32(itemsupID);
            int demondorderdetid = Convert.ToInt32(demondorderdetailsid);
            expireditemService FS = new expireditemService();
                    var RowAllStore = FS.GetRawStores();
            ViewBag.supliers = new SelectList(FS.GetallitemSuplierslist(demondorderdetid), "Value", "Text");
                ViewBag.rawStoreId = new SelectList(RowAllStore.data, "st_id", "st_name");
            //IList<SelectListItem> list = Enum.GetValues(typeof(InvoiceStatus)).Cast<InvoiceStatus>().Select(x => new SelectListItem { Text = x.ToString(), Value = ((int)x).ToString() }).ToList();
            AlertVM<PurchaseInvoiceViewModel> alert = new AlertVM<PurchaseInvoiceViewModel>();
            if (Convert.ToInt32(ToStockID) == 0)

            {


                PurchaseInvoiceViewModel purchaseInvoiceViewModel = new PurchaseInvoiceViewModel();
                purchaseInvoiceViewModel.InvoiceDate = DateTime.Now;
                purchaseInvoiceViewModel.InvoiceSerial = FS.GetMaxSerial();
                purchaseInvoiceViewModel.demondordid = demondorderdetid;
                purchaseInvoiceViewModel.FromStockID = FromStock;
                //purchaseInvoiceViewModel.SupId = FS.getsupid(itemsupid);

                purchaseInvoiceViewModel.Weight = FS.Calculatecurrentquantityfordemonorderdetails(demondorderdetid);
                alert.type = true;
                //ViewBag.InvoiceStatus = new SelectList(list, "Value", "Text");

                var resualt = PartialView("~/Views/ReturnItem/_addReturnItem.cshtml", purchaseInvoiceViewModel).RenderToString();
                return Json(new { data = resualt, alert = alert }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var vm = FS.GetReturnedtostockByID(ToStockID, FromStock, (int)ToStockTypeId.OperationReturns).Data;

                ViewBag.rawStoreId = new SelectList(RowAllStore.data, "st_id", "st_name", vm.StoreId);
                alert.type = true;
                    var Data = PartialView("~/Views/ReturnItem/_DetailsReturnitem.cshtml", vm).RenderToString();
                    return Json(new { data = Data, alert = alert }, JsonRequestBehavior.AllowGet);
           

            }
        }




        //cancel save
        [HttpGet]
        public ActionResult CancelSave()
        {
            AlertVM<PurchaseInvoiceViewModel> alert = new AlertVM<PurchaseInvoiceViewModel>();
            alert.AlertType = ErrorMessageEnum.danger.ToString();
            alert.ErrorMessage = "لم يتم حقظ الفاتورة";
            alert.type = true;
            expireditemService FS = new expireditemService();

            var RowAllStore = FS.GetexpiredStores();
            ViewBag.rawStoreId = new SelectList(RowAllStore.data, "st_id", "st_name");
            var resualt = PartialView("~/Views/ReturnItem/_addReturnItem.cshtml").RenderToString();
            return Json(new { data = resualt, alert = alert }, JsonRequestBehavior.AllowGet);
        }


        //save in db.
        [HttpPost]
        public ActionResult Save(PurchaseInvoiceViewModel vm)
        {
            if (vm.StoreId == null || vm.StoreId == 0)
            {
                return RedirectToAction("FromStockDetails", "FinishDemondOrder", new { ID = vm.FromStockID });

            }
            expireditemService FS = new expireditemService();

            vm.ToStockTypeId = (int?)ToStockTypeId.OperationReturns;

            StockDB.Model.StockTransaction stockTransaction = new StockDB.Model.StockTransaction();
            

            StockDB.Model.ToStock tostock = new StockDB.Model.ToStock();
            tostock.InvoiceDate = vm.InvoiceDate;
            tostock.InvoiceSerial = vm.InvoiceSerial;
            tostock.InvoiceStatus = (int)InvoiceStatus.Finish;
            tostock.Notes = vm.Notes;
            tostock.SupId = FS.getsupid((int)vm.itemsupid);
            tostock.ToStockTypeId = vm.ToStockTypeId;
            tostock.FromStockId = vm.FromStockID;
            tostock.DemondOrderDetialsId = vm.demondordid;
            db.ToStocks.Add(tostock);
            db.SaveChanges();
            stockTransaction.ToStockId = tostock.ID;
            stockTransaction.Quantity = vm.Weight;
            stockTransaction.Weight = vm.Weight;
            stockTransaction.OperationType = "+";
            stockTransaction.ItemSupplierId = vm.itemsupid;
            stockTransaction.NoItem = 1;
            stockTransaction.StoreId = vm.StoreId;
            stockTransaction.DemondOrderDetialsId = vm.demondordid;
            db.StockTransactions.Add(stockTransaction);
            db.SaveChanges();
          var Expired_finishe_edit=  db.ToStocks.Where(x=>x.DemondOrderDetialsId==tostock.DemondOrderDetialsId&&(x.ToStockTypeId==(int?)ToStockTypeId.ExpiredItem)|| x.ToStockTypeId == (int?)ToStockTypeId.FinishedItem).ToList();
            foreach (var item in Expired_finishe_edit)
            {
              var expired_finished=  db.ToStocks.Find(item.ID);
                expired_finished.InvoiceStatus = (int)InvoiceStatus.Finish;

                db.Entry(expired_finished).State = EntityState.Modified;
                db.SaveChanges();
            }
            //Close or Make DemondorderDetails Be Completed
            StockDB.Model.DemondOrderDetail  demondOrderDetail = new StockDB.Model.DemondOrderDetail();
            demondOrderDetail= db.DemondOrderDetails.Find(vm.demondordid);
             demondOrderDetail.status =(int) demondorderdetailstatus.Completed;
            db.Entry(demondOrderDetail).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("FromStockDetails", "FinishDemondOrder", new { ID = vm.FromStockID });
           



        }



        #region Fill ViewBags
        private bool FillViewBag()
        {
            PurchaseInvoiceService pus = new PurchaseInvoiceService();
            expireditemService FS = new expireditemService();

            var RawAllStore = FS.GetexpiredStores();

            if (RawAllStore.type == true)
            {
                ViewBag.rawStoreId = new SelectList(RawAllStore.data, "st_id", "st_name");
            }
            else
            {
                return false;
            }
            return true;

        }
        #endregion


    }
}