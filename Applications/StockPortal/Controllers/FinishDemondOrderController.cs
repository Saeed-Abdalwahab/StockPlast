using StockService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static PlasticStatic.Enums;

namespace StockPortal.Controllers
{
    public class FinishDemondOrderController : Controller
    {
        // GET: FinishDemondOrder
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult FromStockDetails(int ID)
        {
            FromStockService fS = new FromStockService();
            var Model = fS.GetFromStockByID(ID);

            if (Model.type)
            {
                return View("FromStockDetails", Model.Data);
            }
            else
            {
                Model.AlertType = ErrorMessageEnum.danger.ToString();

                Model.ErrorMessage = "حدث خطأ حاول مرة اخرى";
                return Json(new { alert = Model }, JsonRequestBehavior.AllowGet);
            }

        }

        public ActionResult FromStockDetailsList(int ID)
        {
            FromStockService fS = new FromStockService();


            var Model = fS.FromStockDetailsList(ID);

            var result = Model.data;

            return Json(new { data = result }, JsonRequestBehavior.AllowGet);

        }


        public ActionResult DeliveryDemondOrder(int FromStockID,int demondorderdetailsid)
        {
            FromStockService fs = new FromStockService();


            ViewBag.FromStockID = FromStockID;

            ViewBag.demondorderdetailsid = demondorderdetailsid;

            ViewBag.demondorderdetailsstatus = fs.DemonndorderDetailsStatus(demondorderdetailsid);
            return PartialView("_FinshDemondOrder");
        }
        //public ActionResult CancelDeliveryDemondOrder(int FromStock)
        //{
        //    FromStockService fS = new FromStockService();


        //    //هنغيرها بعدين
        //    var Model = fS.GetFromStockByID(FromStock);

        //    if (Model.type)
        //    {
        //        return View("FromStockDetails", Model.Data);
        //    }
        //    else
        //    {
        //        Model.AlertType = ErrorMessageEnum.danger.ToString();

        //        Model.ErrorMessage = "حدث خطأ حاول مرة اخرى";
        //        return Json(new { alert = Model }, JsonRequestBehavior.AllowGet);
        //    }
           
        //}


    

    }
}