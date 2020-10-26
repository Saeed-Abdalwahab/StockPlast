using MasterDetails;
using StockPortal.Helper;
using StockService;
using StockViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static PlasticStatic.Enums;

namespace StockPortal.Controllers
{
    public class expireditemController : Controller
    {
        // GET: expireditem

        StockDB.Model.StockModel db = new StockDB.Model.StockModel();


        #region Get expired Item By ItemID
            //Gett All Expired 'invoice' from to sotck based on fromstockiid
        public ActionResult GetexpiredItemByItemID(int FromStockID,int Demondorderdetailsid)
        {
            expireditemService FS = new expireditemService();

            AlertVM<PurchaseInvoiceViewModel> Alert = new AlertVM<PurchaseInvoiceViewModel>();

            var Resault = FS.GetExpiredTostockList(FromStockID, Demondorderdetailsid);

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
        #endregion

        //just Save in Bag
        //[HttpPost]
        //public ActionResult SaveExpiredItem(Guid Key, string Quantity, string TransactioID, string expiredtypid, string Total,string Demondorderdetailsid,string fromstockid)
        //{
        //    expireditemService expireditemService = new expireditemService();
        //    ViewBag.Key = Key;
        //    AlertVM<PurchaseInvoiceViewModel> alert = new AlertVM<PurchaseInvoiceViewModel>();
        //    PurchaseInvoiceViewModel mv = new PurchaseInvoiceViewModel();

        //    double? OldTotal = Convert.ToDouble(Total);
            
        //    mv.Weight = Convert.ToInt32(Quantity);
        //    mv.expiredtypid = Convert.ToInt32(expiredtypid);
        //    mv.demondordid = Convert.ToInt32(Demondorderdetailsid);
        //    mv.FromStockID = Convert.ToInt32(fromstockid);
        //    mv.expiredtypname = Enum.GetName(typeof(expiredtype), Convert.ToInt32(expiredtypid));
   
        //    double? NewTotal = mv.Weight;
        //    var bag = Session.GetTableBag<PurchaseInvoiceViewModel>(Key);
        //   var suminbag= bag.Items.Sum(x => x.Weight);

        //    if (NewTotal > expireditemService.Calculatecurrentquantityfordemonorderdetails((int)mv.demondordid,null /*tostockid*/,(suminbag!=null?(double)suminbag:0)))
        //    {
        //        alert.type = false;
        //        alert.AlertType = ErrorMessageEnum.danger.ToString();
        //        alert.ErrorMessage = "هذه القيمه اكبر من اجمالى الصنف";
        //        return Json(new { alert = alert }, JsonRequestBehavior.AllowGet);
        //    }
            

        //    bag.Insert(mv);


        //    alert.type = true;
        //    alert.AlertType = ErrorMessageEnum.success.ToString();
        //    alert.ErrorMessage = "تم الاضافة بنجاح";
        //    return Json(new { alert = alert }, JsonRequestBehavior.AllowGet);
        //}


        public ActionResult GetItemByToStockID(int ToStockID, Guid Key)
        {
            expireditemService finshItem = new expireditemService();
            var Data = finshItem.GetExpiredItemList(ToStockID).data;
            var bag = Session.GetTableBag<PurchaseInvoiceViewModel>(Key, Data);
        
            var items = (from e in bag.Items
                         select new PurchaseInvoiceViewModel
                         {
                             TransactionID = e.ID,
                             Weight = e.Weight,
                             expiredtypname = e.expiredtypname

                         }).ToList();
            return Json(new { data = items }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult AddTOstockexpired(string ToStockid, string FromStockid , string demondorderdetailsid = "0")
        {
            int ToStockID = Convert.ToInt32(ToStockid);
            int FromStock = Convert.ToInt32(FromStockid);
            expireditemService FS = new expireditemService();

            AlertVM<PurchaseInvoiceViewModel> alert = new AlertVM<PurchaseInvoiceViewModel>();

            var expiredstores = FS.GetexpiredStores();
            if (Convert.ToInt32(ToStockID) == 0)

            {
                PurchaseInvoiceViewModel purchaseInvoiceViewModel = new PurchaseInvoiceViewModel();
                purchaseInvoiceViewModel.InvoiceDate = DateTime.Now;
                purchaseInvoiceViewModel.InvoiceSerial = FS.GetMaxSerial();
                purchaseInvoiceViewModel.itemsupid = FS.GetallitemSupliersid(Convert.ToInt32(demondorderdetailsid));

                ViewBag.expiredstores = new SelectList(expiredstores.data, "st_id", "st_name");

                alert.type = true;
                
                var resualt = PartialView("~/Views/expireditem/_EditExpiredItem.cshtml", purchaseInvoiceViewModel).RenderToString();
                return Json(new { data = resualt, alert = alert }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var vm = FS.GetTOstockExpiredByID(ToStockID, FromStock , (int)ToStockTypeId.ExpiredItem).Data;
                ViewBag.expiredstores = new SelectList(expiredstores.data, "st_id", "st_name",vm.StoreId);

     
                if (db.ToStocks.Find(Convert.ToInt32(ToStockID)).InvoiceStatus == (int?)InvoiceStatus.NotFinish)
                {
                    
                    alert.type = true;
                    ViewBag.StoreId = new SelectList(expiredstores.data, "st_id", "st_name", vm.StoreId);
                    var Data = PartialView("~/Views/expireditem/_EditExpiredItem.cshtml", vm).RenderToString();
                    return Json(new { data = Data, alert = alert }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    alert.type = true;
                    ViewBag.StoreId = new SelectList(expiredstores.data, "st_id", "st_name", vm.StoreId);
                    var Data = PartialView("~/Views/expireditem/_DetailsToStockReturn.cshtml", vm).RenderToString();
                    return Json(new { data = Data, alert = alert }, JsonRequestBehavior.AllowGet);
                }

            }
        }

        //Calculate What Expired Can added
        [HttpPost]
        public JsonResult checktotalquantity(string cutexpiredweight, string printexpiredweight, string filmexpiredweight, string demondorderdetailsid, string tostockid)
        {
            expireditemService expireditem = new expireditemService();
            var original = expireditem.Calculatecurrentquantityfordemonorderdetails(Convert.ToInt32(demondorderdetailsid), tostockid);
            var cutweight = cutexpiredweight == null|| cutexpiredweight == "" ? 0 : Convert.ToDouble(cutexpiredweight);
            var printweight = printexpiredweight == null || printexpiredweight == "" ? 0 : Convert.ToDouble(printexpiredweight);
            var filmwieght = filmexpiredweight == null || filmexpiredweight == "" ? 0 : Convert.ToDouble(filmexpiredweight);
            var totalexpired = cutweight + printweight + filmwieght;
            if (original < totalexpired)
            {
                return Json(new { original = original, status = false }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { original = original, status = true }, JsonRequestBehavior.AllowGet);

            }
        }
        public ActionResult CancelSave()
        {
            AlertVM<PurchaseInvoiceViewModel> alert = new AlertVM<PurchaseInvoiceViewModel>();
            alert.AlertType = ErrorMessageEnum.danger.ToString();
            alert.ErrorMessage = "لم يتم حقظ الفاتورة";
            alert.type = true;
            var resualt = PartialView("~/Views/expireditem/_expireditem.cshtml").RenderToString();
            return Json(new { data = resualt, alert = alert }, JsonRequestBehavior.AllowGet);
        }


        public ActionResult Save(PurchaseInvoiceViewModel vm  /*,int returnStockTransactioID*/)
        {

            vm.ToStockTypeId = (int?)ToStockTypeId.ExpiredItem;
            //vm.StockTransactioID = returnStockTransactioID;

            AlertVM<PurchaseInvoiceViewModel> alert = new AlertVM<PurchaseInvoiceViewModel>();
            IList<SelectListItem> list = Enum.GetValues(typeof(InvoiceStatus)).Cast<InvoiceStatus>().Select(x => new SelectListItem { Text = x.ToString(), Value = ((int)x).ToString() }).ToList();
            //ViewBag.InvoiceStatus = new SelectList(list, "Value", "Text", vm.InvoiceStatus);
            expireditemService ps = new expireditemService();


            if (vm.StoreId == null || vm.StoreId == 0)
            {
                alert.type = false;
                alert.AlertType = ErrorMessageEnum.danger.ToString();
                alert.ErrorMessage = "برجاء تحديد المخزن ";
            }

          else  if (ps.Addpurchesesinvoiceexpired(vm))
            {
                alert.type = true;
                alert.AlertType = ErrorMessageEnum.success.ToString();
                alert.ErrorMessage = "تم حفظ الفاتورة";
            }
            else
            {
                alert.type = false;
                alert.AlertType = ErrorMessageEnum.danger.ToString();
                alert.ErrorMessage = "حدث حطاء حاول مرة اخره";
            }


            var resualt = PartialView("~/Views/expireditem/_expireditem.cshtml").RenderToString();

            return Json(new { data = resualt, alert = alert }, JsonRequestBehavior.AllowGet);


        }



        #region Delete Purchase Invoice



        public ActionResult DetailsInvoiceID(int id = 0)
        {

            var ps = new PurchaseInvoiceService();
            AlertVM<PurchaseInvoiceViewModel> alert = new AlertVM<PurchaseInvoiceViewModel>();
            var vm = ps.GetPurchaseInvoiceByID(id);

            if (vm != null)
            {
                alert.type = true;
                var Data = PartialView("~/Views/expireditem/_DetailsToStockReturn.cshtml", vm).RenderToString();

                return Json(new { data = Data, alert = alert }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                alert.AlertType = "danger";
                alert.ErrorMessage = "حدث حطاء حاول مرة اخره";
                alert.type = false;
                return Json(new { alert = alert }, JsonRequestBehavior.AllowGet);
            }

        }


        [HttpPost]
        public ActionResult DeleteToStockReturn(int ID, int demondorderdetailsid)
        {
            var ps = new PurchaseInvoiceService();
            AlertVM<PurchaseInvoiceViewModel> alert = new AlertVM<PurchaseInvoiceViewModel>();

            var vm = ps.GetPurchaseInvoiceByID(ID);

            if (vm != null)
            {

                //alert.type = true;
                //var Data = PartialView("_DeleteToStockReturn", vm).RenderToString();
                //return Json(new { data = Data, alert = alert }, JsonRequestBehavior.AllowGet);

                //DisapleDelet if status is finished
                if ( db.DemondOrderDetails.Find(demondorderdetailsid).status != (int?)demondorderdetailstatus.Completed)
                    {
                        alert.type = true;
                        var Data = PartialView("_DeleteToStockReturn", vm).RenderToString();
                        return Json(new { data = Data, alert = alert }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        alert.AlertType = ErrorMessageEnum.danger.ToString();
                        alert.ErrorMessage = "لايمكن حذف هذه الفاتورة";
                        alert.type = false;
                        return Json(new { alert = alert }, JsonRequestBehavior.AllowGet);
                    }

            }
            else
            {
                alert.AlertType = "danger";
                alert.ErrorMessage = "حدث حطاء حاول مرة اخره";
                alert.type = false;
                return Json(new { alert = alert }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult ConfirmDeleteToStockReturn(PurchaseInvoiceViewModel vm)
        {
            
            var ps = new PurchaseInvoiceService();
            AlertVM<PurchaseInvoiceViewModel> alert = new AlertVM<PurchaseInvoiceViewModel>();


            if (ps.DeletePurchaseInvoice(vm.ID))
            {

                alert.AlertType = ErrorMessageEnum.success.ToString();
                alert.ErrorMessage = "تم الحذف بنجاح";
                alert.type = true;
                return Json(new { alert = alert  }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                alert.AlertType = ErrorMessageEnum.danger.ToString();
                alert.ErrorMessage = "توجد مشكلة أثناء تنفيذ الحذف";
                alert.type = false;
                return Json(new { alert = alert }, JsonRequestBehavior.AllowGet);

            }
        }

        #endregion
        #region Fill ViewBags
        private bool FillViewBag()
        {
            PurchaseInvoiceService pus = new PurchaseInvoiceService();
            expireditemService FS = new expireditemService();

            var RawAllStore = FS.GetexpiredStores();

            if (RawAllStore.type == true)
            {
                ViewBag.StoreId = new SelectList(RawAllStore.data, "st_id", "st_name");
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
