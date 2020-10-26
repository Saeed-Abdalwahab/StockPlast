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
    public class FinshToStockController : Controller
    {
        // GET: FinshItem
        StockDB.Model.StockModel db = new StockDB.Model.StockModel();


        #region Get Finsh Item By ItemID
        public ActionResult GetFinshItemByItemID(int FromStockID, int Demondorderdetailsid)
        {
            ReturnItemService FS = new ReturnItemService();


            AlertVM<PurchaseInvoiceViewModel> Alert = new AlertVM<PurchaseInvoiceViewModel>();

            var Resault = FS.GetfinishTostockList(FromStockID, Demondorderdetailsid);

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


        [HttpGet]
        public ActionResult AddTOstockFinish(string ToStockid, string FromStockid, string demondorderdetailsid = "0")
        {
            expireditemService FS = new expireditemService();
            int ToStockID = Convert.ToInt32(ToStockid);
            int FromStock = Convert.ToInt32(FromStockid);
            int DemondorderdetailsID = Convert.ToInt32(demondorderdetailsid);
            var FinishAllStore = FS.GetExpiredAllStore();
            //IList<SelectListItem> list = Enum.GetValues(typeof(InvoiceStatus)).Cast<InvoiceStatus>().Select(x => new SelectListItem { Text = x.ToString(), Value = ((int)x).ToString() }).ToList();
            AlertVM <PurchaseInvoiceViewModel> alert = new AlertVM<PurchaseInvoiceViewModel>();
            if (Convert.ToInt32(ToStockID) == 0)

            {
                PurchaseInvoiceViewModel purchaseInvoiceViewModel = new PurchaseInvoiceViewModel();
                ViewBag.StoreId = new SelectList(FinishAllStore.data, "st_id", "st_name");

                purchaseInvoiceViewModel.InvoiceDate = DateTime.Now;
                purchaseInvoiceViewModel.InvoiceSerial = FS.GetMaxSerial();
                purchaseInvoiceViewModel.itemsupid = FS.GetallitemSupliersid(DemondorderdetailsID);

                alert.type = true;
                //ViewBag.InvoiceStatus = new SelectList(list, "Value", "Text");
                var resualt = PartialView("~/Views/FinshToStock/_EditFinishItem.cshtml", purchaseInvoiceViewModel).RenderToString();
                return Json(new { data = resualt, alert = alert }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var vm = FS.GetTOstockcompletedByID(ToStockID, FromStock, (int)ToStockTypeId.FinishedItem).Data;
                if (db.ToStocks.Find(Convert.ToInt32(ToStockID)).InvoiceStatus == (int?)InvoiceStatus.NotFinish)
                {


                    alert.type = true;
                    ViewBag.StoreId = new SelectList(FinishAllStore.data, "st_id", "st_name", vm.StoreId);
                    var Data = PartialView("~/Views/FinshToStock/_EditFinishItem.cshtml", vm).RenderToString();
                    return Json(new { data = Data, alert = alert }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    alert.type = true;
                    ViewBag.StoreId = new SelectList(FinishAllStore.data, "st_id", "st_name", vm.StoreId);
                    var Data = PartialView("~/Views/FinshToStock/_DetailsToStockFinish.cshtml", vm).RenderToString();
                    return Json(new { data = Data, alert = alert }, JsonRequestBehavior.AllowGet);
                }

            }
        }

        [HttpPost]
        public JsonResult checktotalquantity(string finishedwieght,string demondorderdetailsid,string tostockid)
         {
            expireditemService expireditem = new expireditemService();
         var original=   expireditem.Calculatecurrentquantityfordemonorderdetails(Convert.ToInt32( demondorderdetailsid),tostockid);
            if(original<Convert.ToDouble( finishedwieght))
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
            var resualt = PartialView("~/Views/FinshToStock/_FinishItem.cshtml").RenderToString();
            return Json(new { data = resualt, alert = alert }, JsonRequestBehavior.AllowGet);
        }


        public ActionResult Save(PurchaseInvoiceViewModel vm/*, Guid Key*/)
        {
            vm.ToStockTypeId= (int?)ToStockTypeId.FinishedItem;

            AlertVM<PurchaseInvoiceViewModel> alert = new AlertVM<PurchaseInvoiceViewModel>();
            IList<SelectListItem> list = Enum.GetValues(typeof(InvoiceStatus)).Cast<InvoiceStatus>().Select(x => new SelectListItem { Text = x.ToString(), Value = ((int)x).ToString() }).ToList();
            ViewBag.InvoiceStatus = new SelectList(list, "Value", "Text", vm.InvoiceStatus);
            expireditemService ps = new expireditemService();


            if (vm.StoreId == null || vm.StoreId == 0)
            {
                alert.type = false;
                alert.AlertType = ErrorMessageEnum.danger.ToString();
                alert.ErrorMessage = "برجاء تحديد المخزن";
            }
          else  if (ps.Addpurchaseinvoisefinish(vm))
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


            var resualt = PartialView("~/Views/FinshToStock/_FinishItem.cshtml").RenderToString();

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
                var Data = PartialView("~/Views/FinshToStock/_DetailsToStockFinish.cshtml", vm).RenderToString();

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
        public ActionResult DeleteToStockReturn(int ID,int demonorderdetailsid)
        {
            var ps = new PurchaseInvoiceService();
            AlertVM<PurchaseInvoiceViewModel> alert = new AlertVM<PurchaseInvoiceViewModel>();

            var vm = ps.GetPurchaseInvoiceByID(ID);

            if (vm != null)
            {
                try { 
                if (db.DemondOrderDetails.Find(demonorderdetailsid).status != (int?)demondorderdetailstatus.Completed)
                {
                    alert.type = true;
                    var Data = PartialView("~/Views/FinshToStock/_DeleteToStockFinish.cshtml", vm).RenderToString();
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
                catch
                {
                    alert.AlertType = "danger";
                    alert.ErrorMessage = "حدث حطاء حاول مرة اخره";
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
            AlertVM<PurchaseInvoiceViewModel> alert = new AlertVM<PurchaseInvoiceViewModel>();
            var ps = new PurchaseInvoiceService();
           
            if (ps.DeletFinishinvoice(vm.ID))
            {
                alert.AlertType = ErrorMessageEnum.success.ToString();
                alert.ErrorMessage = "تم الحذف بنجاح";
                alert.type = true;
                return Json(new { alert = alert    }, JsonRequestBehavior.AllowGet);
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

            var RawAllStore = FS.GetExpiredAllStore();

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