using PlasticStatic;
using StockDB.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Web;
using System.Web.Mvc;
using StockPortal.ViewModel;
using StockViewModel;

namespace StockPortal.Controllers
{

    public class ToStockController : Controller
    {
        private StockModel db = new StockModel();

        // GET: DemondOrdersReturn

        public ActionResult Index()
        {


            #region مرتجع تشغيل سيتم مراجعتة


            //List<ViewModel.ToStockVM> toStockVMlist = new List<ViewModel.ToStockVM>();
            //foreach (var item in toStockVMlist)
            //{
            //    toStockVMlist.Add(new ViewModel.ToStockVM
            //    {

            //        InvoiceSerial = item.InvoiceSerial,
            //        InvoiceDate = item.InvoiceDate,
            //        demoOrdDet_id = item.demoOrdDet_id,
            //        Notes = item.Notes,
            //        //st_id = item.st_id,
            //        //Quantity = item.Quantity,
            //        //Weight = item.Weight,
            //    });
            //}

            //return View(toStockVMlist.ToList());
            #endregion
            return View();
        }

        // GET: DemondOrdersReturn/Create
        public ActionResult AddDemondOrderReturn()
        {


            return View();
        }

        // POST: DemondOrdersReturn/Create

        ///////Saeed  Coment كانت عامله ايرور 
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult AddDemondOrderReturn(StockTransactionVM stockTransactionVM)
        //{


        //    {

        //    }
        //        db.SaveChanges();
        //    return View();

        //}



        #region اضافة امر ارتجاع سيتم مراجعتة


        //try
        //{
        //    ToStock tostockobj = new ToStock();
        //    tostockobj.InvoiceSerial = stockVM.InvoiceSerial;
        //    tostockobj.InvoiceDate = stockVM.InvoiceDate;
        //    tostockobj.Notes = stockVM.Notes;
        //    ViewBag.demoOrdDet_id = new SelectList(db.DemondOrderDetails, "demoOrdDet_id", "Name");
        //    //ViewBag.item = new SelectList(db.DemondOrderDetails)


        //    // TODO: Add insert logic here
        //    db.ToStocks.Add(tostockobj);
        //    db.SaveChanges();

        //    return RedirectToAction("Index");


        //}
        //catch
        //{

        //    return View();
        //}
        #endregion

    }
   
    
}
