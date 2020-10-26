using MasterDetails;
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
    public class FinishItemController : Controller
    {
        // GET: FinishItem

        #region Get Item
        public ActionResult GetItemByToStockID(int ToStockID, Guid Key)
        {
            expireditemService finshItem = new expireditemService();


            var Data = finshItem.GetCompletedItemList(ToStockID).data;
            var bag = Session.GetTableBag<PurchaseInvoiceViewModel>(Key, Data);
            var items = (from e in bag.Items
                         select new PurchaseInvoiceViewModel
                         {
                             TransactionID = e.ID,
                             Weight = e.Weight,
                             NoItem = e.NoItem

                         }).ToList();
            return Json(new { data = items }, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Create

        [HttpPost]
        public ActionResult SaveFinishItem(Guid Key, string Quantity, string TransactioID, string NoItem, string demondorderdetailsid,string fromstockid)
        {
            ViewBag.Key = Key;
            AlertVM<PurchaseInvoiceViewModel> alert = new AlertVM<PurchaseInvoiceViewModel>();
            PurchaseInvoiceViewModel mv = new PurchaseInvoiceViewModel();
            expireditemService expireditemService = new expireditemService();
            //double? OldTotal = Convert.ToDouble(Total);
            mv.FromStockID = Convert.ToInt32(fromstockid);
            mv.Weight = Convert.ToInt32(Quantity);
            mv.NoItem = Convert.ToInt32(NoItem);
            mv.demondordid= Convert.ToInt32(demondorderdetailsid);
            double? NewTotal = mv.Weight * mv.NoItem;
            

            //if (NewTotal > expireditemService.Calculatecurrentquantityfordemonorderdetails((int)mv.demondordid))
            //{
            //    alert.type = false;
            //    alert.AlertType = ErrorMessageEnum.danger.ToString();
            //    alert.ErrorMessage = "هذه القيمه اكبر من اجمالى الصنف";
            //    return Json(new { alert = alert }, JsonRequestBehavior.AllowGet);
            //}

            var bag = Session.GetTableBag<PurchaseInvoiceViewModel>(Key);
            bag.Insert(mv);

            alert.type = true;
            alert.AlertType = ErrorMessageEnum.success.ToString();
            alert.ErrorMessage = "تم الاضافة بنجاح";
            return Json(new { alert = alert }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Edit
        public ActionResult EditFinishItem(int ID, Guid Key, string quantity, string TransactioID, string NoItem, string Total)
        {


            ViewBag.Key = Key;
            AlertVM<PurchaseInvoiceViewModel> alert = new AlertVM<PurchaseInvoiceViewModel>();
            PurchaseInvoiceViewModel mv = new PurchaseInvoiceViewModel();
            FromStockService fromStock = new FromStockService();

            double? OldTotal = Convert.ToDouble(Total);

            mv.Weight = Convert.ToInt32(quantity);
            mv.NoItem = Convert.ToInt32(NoItem);

            double? NewTotal = mv.Weight * mv.NoItem;

            //if (NewTotal > OldTotal)
            //{
            //    alert.type = false;
            //    alert.AlertType = ErrorMessageEnum.danger.ToString();
            //    alert.ErrorMessage = "هذه القيمه اكبر من اجمالى الصنف";
            //    return Json(new { alert = alert }, JsonRequestBehavior.AllowGet);
            //}


            mv.ID = ID;
            mv.Weight = Convert.ToInt32(quantity);
            mv.NoItem = Convert.ToInt32(NoItem);
            mv.TransactionID = ID;



            var bag = Session.GetTableBag<PurchaseInvoiceViewModel>(Key);
            bag.Update(mv);
            alert.type = true;
            alert.AlertType = ErrorMessageEnum.success.ToString();
            alert.ErrorMessage = "تم التعديل بنجاح";
            return Json(new { alert = alert }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Delete
        public ActionResult DeleteFinshItem(int ID, Guid Key)
        {
            ViewBag.Key = Key;
            var bag = Session.GetTableBag<PurchaseInvoiceViewModel>(Key);
            return PartialView("~/Views/FinishItem/_DeleteFinishItem.cshtml", bag[ID]);
        }

        public ActionResult ConfirmDeleteFinshItem(int ID, Guid Key)
        {
            ViewBag.Key = Key;

            AlertVM<PurchaseInvoiceViewModel> alert = new AlertVM<PurchaseInvoiceViewModel>();
            var bag = Session.GetTableBag<PurchaseInvoiceViewModel>(Key);
            bag.Delete(ID);
            alert.type = true;
            alert.AlertType = ErrorMessageEnum.success.ToString();
            alert.ErrorMessage = "تم الحذف بنجاح";
            return Json(new { alert = alert }, JsonRequestBehavior.AllowGet);
        }
        #endregion
    }
}