using StockDB.Model;
using StockService;
using StockViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StockPortal.Controllers
{
    public class SalesReturnController : Controller
    {
        SalesReturnservice salesReturnservice = new SalesReturnservice();
        // GET: SalesReturn
        public ActionResult Index(int fromstockid)
        {
            var list = salesReturnservice.Index(fromstockid);
            if (list.Count==0)
            {
                return RedirectToAction("Create", new { fromstockid = fromstockid });
            }
            return View(list);
        }
        [HttpGet]
        public ActionResult create(int fromstockid)
        {
           
            ViewBag.stores = new SelectList(salesReturnservice.GetexpiredStores(), "st_id", "st_name");
            ViewBag.salesDate = salesReturnservice.SalesDate(fromstockid);
            return View(salesReturnservice.Create(fromstockid));
        }
        [HttpPost]
        public ActionResult create(SalesReturnVM salesReturnVM)
        {

            if (salesReturnservice.SaveinDb(salesReturnVM))
            {
                return Json(new { success = true}, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public ActionResult Edit(int tostockid)
        {
            var salesreturned = salesReturnservice.Edit(tostockid);
            ViewBag.stores = new SelectList(salesReturnservice.GetexpiredStores(), "st_id", "st_name",salesreturned.storeid);
            ViewBag.salesDate = salesReturnservice.SalesDate((int)salesreturned.FromStockId);

            return View(salesreturned);
        }
        [HttpGet]
        public ActionResult Details(int tostockid)
        {
            var salesreturned = salesReturnservice.Edit(tostockid);
            return View(salesreturned);
        }
        [HttpPost]
        public ActionResult Edit(SalesReturnVM salesReturnVM)
        {

            if (salesReturnservice.SaveinDb(salesReturnVM))
            {
                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]

        public ActionResult Delete(int tostockid)
        {
            return Json(new { success = salesReturnservice.Delete(tostockid) }, JsonRequestBehavior.AllowGet);
        }



    }
}