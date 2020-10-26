using StockService;
using StockViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StockPortal.Controllers
{
    public class PurchasesReturnController : Controller
    {
        private purchasesreturnservice purchesiesreturnservice = new purchasesreturnservice();
        public ActionResult Index()
        {
            var list = purchesiesreturnservice.Index();
           
            return View(list);
        }

        [HttpGet]
        public ActionResult Edit(int fromstockid)
        {
            var salesreturned = purchesiesreturnservice.Edit(fromstockid);

            return View(salesreturned);
        }


        [HttpGet]
        public ActionResult Details(int fromstockid)
        {
            var salesreturned = purchesiesreturnservice.Edit(fromstockid);
            return View(salesreturned);
        }
 

        [HttpGet]
        public ActionResult create()
        {
            //ViewBag.PurchesesDate = purchesiesreturnservice.PurchesesDate(tostockid);
            ViewBag.Suppliers = new SelectList(purchesiesreturnservice.Suppliers(), "sup_id", "sup_name");
            ViewBag.RawStoes = new SelectList(purchesiesreturnservice.RawStores(), "st_id", "st_name");
           
            return View(/*purchesiesreturnservice.Create(tostockid)*/);
        }
        [HttpGet]
        public ActionResult AvilableinStock(int StoreID, int SupplierID)
        {
            var ListofAvilable = purchesiesreturnservice.AvilableinStock(SupplierID, StoreID);
            purchasesreturnVM purchasesreturnVM = new purchasesreturnVM();
            purchasesreturnVM.avaliableiteminstores = ListofAvilable;
            purchasesreturnVM.TransDate = DateTime.Now;
            purchasesreturnVM.Serial = purchesiesreturnservice.GetMaxSerial();
            return View(purchasesreturnVM);
        }

        [HttpPost]
        public ActionResult create(purchasesreturnVM purchasesreturnVM)
        {
            if (purchesiesreturnservice.SaveinDb(purchasesreturnVM))
            {
                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]

        public ActionResult Delete(int fromstockid)
        {
            return Json(new { success = purchesiesreturnservice.Delete(fromstockid) }, JsonRequestBehavior.AllowGet);
        }


    }
}