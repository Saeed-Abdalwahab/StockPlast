using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using StockService;
using StockViewModel;
using StockPortal.Helper;
using static PlasticStatic.Enums;
using MasterDetails;
using StockDB.Model;
using System.Data.Entity;
using PlasticStatic;
using StockPortal.Helpers;
using StockPortal.Models;

namespace StockPortal.Controllers
{
    [HandleError]
    [CustomAuthorize("فاتورة المشتريات")]
    [SessionExpire]
    public class PurchaseInvoiceController : BaseController
    {
        private StockModel db = new StockModel();
        
        // GET: PurchaseInvoice
        #region Index
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public ActionResult AllPurchaseInvoice()
        {
            PurchaseInvoiceService ps = new PurchaseInvoiceService();
            var query = ps.PurchaseInvoiceList();
            return Json(new { data = query }, JsonRequestBehavior.AllowGet);
        }
        #endregion


        #region Add Purchase Invoice
        public ActionResult AddPurchaseInvoice(int id = 0)
        {
            PurchaseInvoiceService pus = new PurchaseInvoiceService();
            FillViewBag();
            ViewBag.supliers = new SelectList(db.suppliers, "sup_id", "sup_name");
            IList<SelectListItem> list = Enum.GetValues(typeof(InvoiceStatus)).Cast<InvoiceStatus>().Select(x => new SelectListItem { Text = x.GetDisplayName(), Value = ((int)x).ToString() }).ToList();

            AlertVM<PurchaseInvoiceViewModel> alert = new AlertVM<PurchaseInvoiceViewModel>();
            var ps = new PurchaseInvoiceService();
            ViewBag.items = ps.GetallItems();

            if (id == 0)

            {
                PurchaseInvoiceViewModel purchaseInvoiceViewModel = new PurchaseInvoiceViewModel();
                alert.type = true;
                ViewBag.InvoiceStatus = new SelectList(list, "Value", "Text");
                purchaseInvoiceViewModel.InvoiceSerial = ps.GetMaxSerial();
                purchaseInvoiceViewModel.InvoiceDate = DateTime.Now;
                var resualt = PartialView("_editPurchaseInvoice", purchaseInvoiceViewModel).RenderToString();
                return Json(new { data = resualt, alert = alert }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var vm = ps.GetPurchaseInvoiceByID(id);
                if (vm.InvoiceStatus == (int?)InvoiceStatus.NotFinish)
                {
                    var RawAllStore = pus.GetRawAllStore();
                    alert.type = true;
                    ViewBag.InvoiceStatus = new SelectList(list, "Value", "Text", vm.InvoiceStatus);
                    ViewBag.StoreId = new SelectList(RawAllStore.data, "st_id", "st_name",vm.StoreId);
                    var Data = PartialView("_editPurchaseInvoice", vm).RenderToString();
                    return Json(new { data = Data, alert = alert }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    alert.AlertType = ErrorMessageEnum.danger.ToString();
                    alert.ErrorMessage = "لم يمكن  تعديل هذه الفاتورة";
                    alert.type = false;
                    return Json(new { alert = alert }, JsonRequestBehavior.AllowGet);
                }

            }


        }
        public ActionResult DetailsInvoiceID(int id = 0)
        {

            var ps = new PurchaseInvoiceService();
            AlertVM<PurchaseInvoiceViewModel> alert = new AlertVM<PurchaseInvoiceViewModel>();
            var vm = ps.GetPurchaseInvoiceByID(id);

            if (vm != null)
            {
                alert.type = true;
                var Data = PartialView("_DetailsTransaction", vm).RenderToString();

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


        public ActionResult Save(PurchaseInvoiceViewModel vm, Guid Key)
        {
           
            AlertVM<PurchaseInvoiceViewModel> alert = new AlertVM<PurchaseInvoiceViewModel>();
            ViewBag.Key = Key;
            IList<SelectListItem> list = Enum.GetValues(typeof(InvoiceStatus)).Cast<InvoiceStatus>().Select(x => new SelectListItem { Text = x.ToString(), Value = ((int)x).ToString() }).ToList();
            ViewBag.InvoiceStatus = new SelectList(list, "Value", "Text", vm.InvoiceStatus);
            PurchaseInvoiceService ps = new PurchaseInvoiceService();

            if (FillViewBag()!=true)
            {
                alert.type = false;
                alert.AlertType = ErrorMessageEnum.danger.ToString();
                alert.ErrorMessage = "حدث حطاء حاول مرة اخره";
                return Json(new {alert = alert }, JsonRequestBehavior.AllowGet);
            }

            if (vm.StoreId == null || vm.StoreId == 0)
            {
                alert.type = false;
                alert.AlertType = ErrorMessageEnum.danger.ToString();
                alert.ErrorMessage = "برجاء تحديد المخزن";
                var resualt = PartialView("_PurchaseInvoice").RenderToString();
                return Json(new { data = resualt, alert = alert }, JsonRequestBehavior.AllowGet);
            }

            if (ModelState.IsValid)
            {               
                ViewBag.Key = Key;
                var bag = Session.GetTableBag<PurchaseInvoiceViewModel>(Key);
                List<PurchaseInvoiceViewModel> insert = bag.Inserted;
                List<PurchaseInvoiceViewModel> Modified = bag.Modified;
                List<PurchaseInvoiceViewModel> Deleted = bag.Deleted;
                

                if (ps.AddPurchaseInvoice(vm, insert, Modified, Deleted))
                {
                    alert.type = true;
                    alert.AlertType = ErrorMessageEnum.success.ToString();
                    alert.ErrorMessage = "تم حفظ الفاتورة";
                    //var itmsuplidist = db.StockTransactions.Where(x => x.ToStockId == vm.ID).Select(x => x.ItemSupplierId).ToList();
                   //var lastinvoice= db.ToStocks.Where(x => x.SupId == vm.SupId).OrderByDescending(x => x.InvoiceDate).FirstOrDefault();
                   // var items = db.ItemSuppliers.Where(x => x.SupId == lastinvoice.SupId).Select(x => x.Item);
                   // foreach (var i in items)
                   // {
                   //     var editeditem = db.Items.Find(i.ID);
                   //     editeditem.PurchasingPrice = insert.FirstOrDefault(x => x.ItemID == editeditem.ID).itemprice;

                   //     db.Entry(editeditem).State = EntityState.Modified;
                   //     db.SaveChanges();

                   // }
                    //determin all item in purcheces
            

                }
                else
                {
                    alert.type = false;
                    alert.AlertType = ErrorMessageEnum.danger.ToString();
                    alert.ErrorMessage = "حدث حطاء حاول مرة اخره";
                }


                var resualt = PartialView("_PurchaseInvoice").RenderToString();

                return Json(new { data = resualt, alert = alert }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                alert.type = false;

                var resualt= PartialView("_editPurchaseInvoice", vm).RenderToString();


                //var resualt = CovertParialView.RenderViewToString(ControllerContext, "_editPurchaseInvoice", vm, FillViewBag());
                return Json(new { data = resualt, alert = alert }, JsonRequestBehavior.AllowGet);
            }

        }


        public ActionResult CancelSave()
        {
            AlertVM<PurchaseInvoiceViewModel> alert = new AlertVM<PurchaseInvoiceViewModel>();
            alert.AlertType = ErrorMessageEnum.danger.ToString();
            alert.ErrorMessage = "لم يتم حقظ الفاتورة";
            alert.type = true;
            var resualt = PartialView("_PurchaseInvoice").RenderToString();
            return Json(new { data = resualt , alert = alert }, JsonRequestBehavior.AllowGet);
        }


        #endregion


        #region Delete Purchase Invoice

        [HttpPost]
        public ActionResult DeletePurchaseInvoice(int ID)
        {
            var ps = new PurchaseInvoiceService();
            AlertVM<PurchaseInvoiceViewModel> alert = new AlertVM<PurchaseInvoiceViewModel>();

            var vm = ps.GetPurchaseInvoiceByID(ID);

            if (vm != null)
            {

                if (vm.InvoiceStatus == (int?)InvoiceStatus.NotFinish)
                {
                    alert.type = true;
                    var Data = PartialView("_DeletePurchaseInvoice", vm).RenderToString();
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
        public ActionResult ConfirmDeletePurchaseInvoice(int ID)
        {
            ViewBag.ID = ID;
            var ps = new PurchaseInvoiceService();
            AlertVM<PurchaseInvoiceViewModel> alert = new AlertVM<PurchaseInvoiceViewModel>();

            if (ps.DeletePurchaseInvoice(ID))
            {
                alert.AlertType = ErrorMessageEnum.success.ToString();
                alert.ErrorMessage = "تم الحذف بنجاح";
                alert.type = true;
                return Json(new { alert = alert }, JsonRequestBehavior.AllowGet);
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


        #region Auto Complet
        [HttpGet]
        public ActionResult GetSuplierByName(string searchtext)
        {
            SupplierService ps = new SupplierService();

           List<SupplierViewModel> resualt = ps.returnSupSearch(searchtext);
            //return Json(new {data=resualt},JsonRequestBehavior.AllowGet);
            return Json(new { data = resualt }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetItemByName(string searchtext)
        {
            ItemService IS = new ItemService();
            List<ItemVM> resualt = IS.returnitemSearch(searchtext);
            return Json(new { data = resualt }, JsonRequestBehavior.AllowGet);
        }

        #endregion


        #region Fill ViewBage
        private bool FillViewBag()
        {
            SupplierService ps = new SupplierService();
            PurchaseInvoiceService pus = new PurchaseInvoiceService();

            var Supplier = ps.SupplierList();
            var RawAllStore = pus.GetRawAllStore();
            var Item = pus.GetAllItems();

            if (Supplier.type == true)
            {
                ViewBag.sup_id = new SelectList(Supplier.data, "supid", "supname");
            }
            else
            {
                return false;
            }
            if (RawAllStore.type == true)
            {
                ViewBag.StoreId = new SelectList(RawAllStore.data, "st_id", "st_name");
            }
            else
            {
                return false;
            }
           
           

            if (Item.type == true)
            {
                ViewBag.ItemID = new SelectList(Item.data, "ID", "Name");
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