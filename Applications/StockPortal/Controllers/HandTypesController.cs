using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using StockDB.Model;
using StockPortal.Helpers;
using StockPortal.Models;
using StockService;
using StockViewModel;

namespace StockPortal.Controllers
{
    [HandleError]
    [CustomAuthorize("انواع اليد")]
    [SessionExpire]
    public class HandTypesController : BaseController
    {
      private StockModel db = new StockModel();
      private  HandTypService handTypService = new HandTypService();

        // GET: HandTypes
        public ActionResult Index()
        {
            return View();
        }
        //Get All Hand Typ "Index"-- AjaxCall
        [HttpGet]
        public JsonResult GetHandTyps()
        {
            var AllHandTyps = handTypService.AllHandTyps();
            return Json(new { data = AllHandTyps }, JsonRequestBehavior.AllowGet);
        }

        //Get Hand Typ By Id
        [HttpGet]
        public JsonResult GetHandTypsbyid(int HandType_id)
        {
           
            var HandTyp = handTypService.GetHandtypByid(HandType_id);
            return Json(new { data = HandTyp }, JsonRequestBehavior.AllowGet);
        }
        //Method To Save Changes In Hand Typ Add -- Update

        [HttpPost]
        public JsonResult SaveHAntTypInDB(HandTypVM handTypVM )
        {
         
            return Json(handTypService.SaveinDatabase(handTypVM), JsonRequestBehavior.AllowGet);

        }

        //Return Partial view With Delet Confirmation msg 
        public ActionResult DeletHandTyp(int? HandType_id)
        {
            if (HandType_id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            HandTypVM handTypeVM = db.HandTypes.Select(x => new HandTypVM() {
                HandType_id = x.HandType_id,
                HandType_name = x.HandType_name
            }).FirstOrDefault(xx => xx.HandType_id == HandType_id);
            if (handTypeVM == null)
            {
                return HttpNotFound();
            }
            return PartialView(handTypeVM);
        }

        //Confirm Delet To Deled Hand  Typ
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public bool DeleteConfirmed(int id)
        {
            try {
            HandType handType = db.HandTypes.Find(id);
            db.HandTypes.Remove(handType);
            db.SaveChanges();
            return true;
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
