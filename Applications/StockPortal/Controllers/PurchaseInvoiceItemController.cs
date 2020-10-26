using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MasterDetails;
using StockPortal.Helper;
using StockService;
using StockViewModel;
using static PlasticStatic.Enums;

namespace StockPortal.Controllers
{
    public class PurchaseInvoiceItemController : BaseController
    {
        // GET: PurchaseInvoiceItem
        public ActionResult ItemByInvoiceID(int ID, Guid Key)
        {

            PurchaseInvoiceService ps = new PurchaseInvoiceService();
            

            var Data = ps.GetItemByInvoiceID(ID);
            var bag = Session.GetTableBag<PurchaseInvoiceViewModel>(Key, Data);
            var items = (from e in bag.Items
                         select new
                         {
                             TransactionID = e.ID,
                             NoItem = e.NoItem,
                             Quantity = e.Weight,
                             ItemID=e.ItemID,
                             ItemName=e.ItemName,
                             items=ps.GetAllItems(),
                             itemprice =e.itemprice,
                             TotalItemCost=e.itemprice*e.Weight*e.NoItem,
                             totalweight=e.Weight*e.NoItem,
                             

                             
                         }).ToList();
            ViewBag.totalprice = items.Sum(x => x.itemprice);

            return Json(new { data = items }, JsonRequestBehavior.AllowGet);
        }

        #region Details


        //الاصناف الى موجوده فى الفاتوره
        public ActionResult ItemDetails(int ID)
        {
            PurchaseInvoiceService ps = new PurchaseInvoiceService();

            var Data = ps.GetItemDetailsInvoiceID(ID);
            var items = (from e in Data
                         select new
                         {
                             ItemName = e.ItemName,
                             NoItem = e.NoItem,
                             Quantity = e.Weight,
                             StoreName = e.StoreName,
                             itemprice=e.itemprice,
                             totalitemweight=e.Weight*e.NoItem,
                             totalitemcost= Math.Round((double)(e.Weight * e.NoItem * e.itemprice), 2)

                         }).ToList();
            return Json(new { data = items }, JsonRequestBehavior.AllowGet);
        }

        #endregion


        #region Delete

        public ActionResult DeleteTransaction(int ID, Guid Key)
        {
            ViewBag.Key = Key;
            var bag = Session.GetTableBag<PurchaseInvoiceViewModel>(Key);
            return PartialView("_DeleteTransaction", bag[ID]);
        }
        [HttpPost]
        public ActionResult ConfirmDeleteTransaction(int ID, Guid Key)
        {
            ViewBag.Key = Key;

            AlertVM<PurchaseInvoiceViewModel> alert = new AlertVM<PurchaseInvoiceViewModel>();           
            var bag = Session.GetTableBag<PurchaseInvoiceViewModel>(Key);
            bag.Delete(ID);
            alert.type = true;
            alert.AlertType = ErrorMessageEnum.success.ToString();
            alert.ErrorMessage = "تم الحذف بنجاح";
            return Json(new {alert=alert }, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Edit

        [HttpGet]
        public ActionResult EditTransaction(int ID, Guid Key , string itemid , string itemno  , string quantity,string itemprice )
        {
            ViewBag.Key = Key;
            AlertVM<PurchaseInvoiceViewModel> alert = new AlertVM<PurchaseInvoiceViewModel>();
            PurchaseInvoiceViewModel mv = new PurchaseInvoiceViewModel();
            PurchaseInvoiceService ps = new PurchaseInvoiceService();

            mv.ID = ID;
            mv.ItemID =Convert.ToInt32(itemid);
            mv.NoItem = Convert.ToInt32(itemno) ;
            mv.Weight = Convert.ToInt32(quantity) ;
            mv.itemprice = Convert.ToInt32(itemprice) ;
            mv.ItemName = ps.GetAllItemsByID(Convert.ToInt32(itemid)).Name;
            mv.TransactionID = ID;
            
            

            var bag = Session.GetTableBag<PurchaseInvoiceViewModel>(Key);
            bag.Update(mv);
            alert.type = true;
            alert.AlertType = ErrorMessageEnum.success.ToString();
            alert.ErrorMessage = "تم التعديل بنجاح";
            return Json(new { alert = alert }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Create

        [HttpPost]
        public ActionResult savetransaction(Guid Key, string ItemID, string NoItem,string Quantity,string itemprice)
        {
            ViewBag.Key = Key;
            AlertVM<PurchaseInvoiceViewModel> alert = new AlertVM<PurchaseInvoiceViewModel>();
            PurchaseInvoiceViewModel mv = new PurchaseInvoiceViewModel();
            PurchaseInvoiceService ps = new PurchaseInvoiceService();

            mv.ItemID = Convert.ToInt32(ItemID);
            mv.NoItem = Convert.ToDouble(NoItem);
            mv.Weight = Convert.ToDouble(Quantity);
            mv.itemprice = Convert.ToDouble(itemprice);
            
            mv.ItemName = ps.GetAllItemsByID(Convert.ToInt32(ItemID)).Name;
            var bag = Session.GetTableBag<PurchaseInvoiceViewModel>(Key);
            bag.Insert(mv);

            alert.type = true;
            alert.AlertType = ErrorMessageEnum.success.ToString();
            alert.ErrorMessage = "تم الاضافة بنجاح";
            return Json(new { alert = alert }, JsonRequestBehavior.AllowGet);
        }
        #endregion

    }
}