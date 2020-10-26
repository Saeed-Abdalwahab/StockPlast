using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using StockService;
using System.Web.Mvc;
using StockDB.Model;
using StockPortal.ViewModel;
using static PlasticStatic.Enums;

namespace StockPortal.Controllers
{
    public class StockTransactionsController : Controller
    {
        private StockModel db = new StockModel();
        StockTransaction_service stockTransaction_Service = new StockTransaction_service();


      

        // GET: StockTransactions
        public ActionResult Index(int fromstockid)
        {

            var stockTransactions = db.StockTransactions.Where(xx => xx.FromStockId == fromstockid && xx.OperationType == "-").ToList();
            if ((stockTransactions.Count() == 0))
            {
                stockTransactions = null;
            }
            TempData["fromstockid"] = fromstockid;
            return View(stockTransactions);
        }


        [HttpGet]
        public ActionResult Edit(int? stochTransactionId)
        {
            try
            {
                if (stochTransactionId == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                StockTransaction stockTransaction = db.StockTransactions.Find(stochTransactionId);
                if (stockTransaction == null)
                {
                    return HttpNotFound();
                }
                ViewBag.Avilableamount = stockTransaction_Service.Avilableamount(stockTransaction.ItemSupplier.Item.ID, (int)stockTransaction.ItemSupplierId, (int)stockTransaction.StoreId, (int)stockTransaction.Weight);


                ViewBag.ID = stockTransaction.ID;
                ViewBag.Noitem = stockTransaction.NoItem;
                ViewBag.wieght = stockTransaction.Weight;


                return PartialView();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        [HttpGet]
        public ActionResult Delete(string stochTransactionId)
        {
            int ID = int.Parse(stochTransactionId);
            try
            {
                if (stochTransactionId == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                StockTransaction stockTransaction = db.StockTransactions.FirstOrDefault(x => x.ID == ID);
                if (stockTransaction == null)
                {
                    return HttpNotFound();
                }
                ViewBag.ID = stockTransaction.ID;
                return PartialView();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



        [HttpPost]
        public JsonResult SaveDelete(string StockTransactionId)
        {
            var result = stockTransaction_Service.SaveDelet(StockTransactionId);
            return Json(result, JsonRequestBehavior.AllowGet);

        }

        [HttpGet]
        public ActionResult Create(int fromstockid)
        {
            TempData["fromstockid"] = fromstockid;

            StockViewModel.StockTransactionVM stockTransactionVM = new StockViewModel.StockTransactionVM();

            foreach (var item in db.Stores.Where(x => x.StoreType_ID == 1))
            {
                stockTransactionVM.Stores.Add(new SelectListItem { Text = item.st_name, Value = item.st_id.ToString() });
            }

            //بجيب السماء الشغل اللي ليها تاريخ دخول وليس لها تاريخ خروج 
            //And Get Al Not Completed 
            var demondorderid = db.FromStocks.FirstOrDefault(x => x.ID == fromstockid).DemondOrderId;
            var selectListItem= db.DemondOrderDetails.Where(xx => xx.Shaplona.shap_startDate!=null&&xx.Shaplona.shap_endDate==null&&xx.demoOrd_id == demondorderid&&(xx.status==(int)demondorderdetailstatus.NotStarted|| xx.status == (int)demondorderdetailstatus.Underconstruction)).Select(x => new SelectListItem
            {
                Value = x.demoOrdDet_id.ToString(),
                Text = x.Shaplona.shap_name.ToString()
            });

            ViewBag.Demondorderdetils = new SelectList(selectListItem, "Value", "Text");
            stockTransactionVM.FromStockId = fromstockid;
            ViewBag.demondorderid= demondorderid;
            return View(stockTransactionVM);
        }


        [HttpPost]
        public JsonResult Create(string itemsuplierid, string storeid, string Noitem, string fromstockid, string wegiht, string Demondorderdetailsid, string itemid)
        {

            var result = stockTransaction_Service.saveinDatabase(itemsuplierid, storeid, Noitem, fromstockid, wegiht, Demondorderdetailsid, itemid);

            return Json(result, JsonRequestBehavior.AllowGet);

        }

        public JsonResult FetchAvilableinStock(int DemondorderDetailsid, int? storeid, string fromstockid)
        {
            //determin Item Id From Demondorderdetails 
            int Itemid = (int)db.DemondOrderDetails.FirstOrDefault(x => x.demoOrdDet_id == DemondorderDetailsid).Item_id;
            int Fromstockid = int.Parse(fromstockid);
            var XXX = db.StockTransactions.Where(x => x.FromStockId== Fromstockid&&x.DemondOrderDetialsId==DemondorderDetailsid).Select(x =>new {wieght= x.Weight,storeid=x.StoreId }).ToList();
            List<Avaliableiteminstore> avaliableiteminstores = new List<Avaliableiteminstore>();

            var List = stockTransaction_Service.GetAvlibleQuntityandNoiteminstock(Itemid, storeid, Fromstockid);

            foreach (var item in XXX)
            {
                List.RemoveAll(x => x.storeid == item.storeid && x.weight == item.wieght);
            }

            return Json(new { data = List }, JsonRequestBehavior.AllowGet);

        }
    
        [HttpPost]
        public JsonResult SaveEdit(string StockTransactionId, string NewNoItem)
        {

            var result = stockTransaction_Service.SaveEditInDatabase(StockTransactionId, NewNoItem);
            return Json(result, JsonRequestBehavior.AllowGet);

        }



        public JsonResult FetchStores(int DemondorderDetailsid)
        {
            var test = db.DemondOrderDetails.FirstOrDefault(x => x.demoOrdDet_id == DemondorderDetailsid);

            int Itemid = (int)db.DemondOrderDetails.FirstOrDefault(x => x.demoOrdDet_id == DemondorderDetailsid).Item_id;
            var Stores = db.StockTransactions.Where(x => x.ItemSupplier.ItemId == Itemid&& x.Store.StoreType_ID==(int)PlasticStatic.Enums.StoreType.Raw)
                .Select(c => new
                {
                    ID = c.StoreId,
                    Text = c.Store.st_name

                }).Distinct().ToList();

            return Json(Stores, JsonRequestBehavior.AllowGet);
        }


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
        public List<SelectListItem> Itmsupplier(int? selectedDemondOrderDetialsId)
        {
            List<SelectListItem> list = new List<SelectListItem>();
            List<ItemSupplier> itemSuppliers = db.ItemSuppliers.ToList();
            foreach (var item in itemSuppliers)
            {
                list.Add(new SelectListItem { Value = item.ID.ToString(), Text = item.Item.Name + item.supplier.sup_name });
            }
            return list;
        }
    }
}
