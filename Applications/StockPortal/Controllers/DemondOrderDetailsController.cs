using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using StockDB.Model;
using StockPortal.ViewModel;
using StockViewModel;
using static PlasticStatic.Enums;

namespace StockPortal.Controllers
{
    public class DemondOrderDetailsController : Controller
    {
        private StockModel db = new StockModel();

        // GET: DemondOrderDetails
        public ActionResult Index(int demOrdId)
        {
            var demondOrderDetails = db.DemondOrderDetails.
                Include(d => d.DemondOrder).
                Include(d => d.HandType).
                Include(d => d.Shaplona).Where(ee => ee.demoOrd_id == demOrdId);
            return View(demondOrderDetails.ToList());
        }

        // GET: DemondOrderDetails/Create
        public ActionResult Create(int demOrdId)
        {
            //Determin Cust From Demond order
           int Custid= db.DemondOrders.FirstOrDefault(x => x.demoOrd_id == demOrdId).cust_id;
            ViewBag.HandTypeId = new SelectList(db.HandTypes, "HandType_id", "HandType_name");
            //Determin Shaplons Based On Cust in Demond Order ....  السريل اللي موجود موجودهفي اسماء الشغل تكون تابعه لنفس العميل 
            ViewBag.shap_id = new SelectList(db.Shaplonas.Where(x=>x.cust_id== Custid), "shap_id", "shap_name");
            ViewBag.Item_Id= new SelectList(db.Items, "ID", "Name");
            ViewBag.Colors= new SelectList(db.colors, "color_id", "color_name");
            ViewBag.Cust_id = Custid;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(DemondOrderDetailsVM demondOrderDetailVM, int demOrdId)
        {
                DemondOrderDetail demondOrderDetail = new DemondOrderDetail();  
            if (ModelState.IsValid)
            {
                demondOrderDetail.demoOrd_id = demOrdId;
                demondOrderDetail.Item_id = demondOrderDetailVM.Item_id;
                demondOrderDetail.shap_id = demondOrderDetailVM.shap_id;
                demondOrderDetail.HandTypeId = demondOrderDetailVM.HandTypeId;
                demondOrderDetail.Hand_Count = demondOrderDetailVM.Hand_Count;
                demondOrderDetail.Hand_Price = demondOrderDetailVM.Hand_Price;
                demondOrderDetail.demondQuantity = demondOrderDetailVM.demondQuantity;
                demondOrderDetail.colorcount = demondOrderDetailVM.colorcount;
                demondOrderDetail.colorname = demondOrderDetailVM.colorname ;
                demondOrderDetail.size = demondOrderDetailVM.size;
                demondOrderDetail.HandColorID = demondOrderDetailVM.HandColorID;
                demondOrderDetail.status =(int) demondorderdetailstatus.NotStarted;
                demondOrderDetail.Selling_Price = demondOrderDetailVM.Selling_Price;
                demondOrderDetail.Notes = demondOrderDetailVM.Notes;
                db.DemondOrderDetails.Add(demondOrderDetail);
                db.SaveChanges();
                return RedirectToAction("Index", new { demOrdId = demOrdId });
            }


            //Determin Cust From Demond order
            int Custid = db.DemondOrders.FirstOrDefault(x => x.demoOrd_id == demOrdId).cust_id;
            ViewBag.HandTypeId = new SelectList(db.HandTypes, "HandType_id", "HandType_name");
            //Determin Shaplons Based On Cust in Demond Order ....  السريل اللي موجود موجودهفي اسماء الشغل تكون تابعه لنفس العميل 
            ViewBag.shap_id = new SelectList(db.Shaplonas.Where(x => x.cust_id == Custid), "shap_id", "shap_name");
            ViewBag.Item_Id = new SelectList(db.Items, "ID", "Name");
            ViewBag.Colors = new SelectList(db.colors, "color_id", "color_name");

            ViewBag.Cust_id = Custid;
            return View(demondOrderDetailVM);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DemondOrderDetail demondOrderDetail = db.DemondOrderDetails.Find(id);
            if (demondOrderDetail == null)
            {
                return HttpNotFound();
            }

            DemondOrderDetailsVM demondOrderDetailsVM = new DemondOrderDetailsVM();
            demondOrderDetailsVM.demoOrdDet_id = demondOrderDetail.demoOrdDet_id;
            demondOrderDetailsVM.demoOrd_id = demondOrderDetail.demoOrd_id;
            demondOrderDetailsVM.Item_id = (int)demondOrderDetail.Item_id;
            demondOrderDetailsVM.shap_id = demondOrderDetail.shap_id;
            demondOrderDetailsVM.HandTypeId = (int)demondOrderDetail.HandTypeId;
            demondOrderDetailsVM.Hand_Count = demondOrderDetail.Hand_Count;
            demondOrderDetailsVM.Hand_Price = demondOrderDetail.Hand_Price;
            demondOrderDetailsVM.demondQuantity = demondOrderDetail.demondQuantity;
            demondOrderDetailsVM.colorcount = demondOrderDetail.colorcount;
            demondOrderDetailsVM.colorname = demondOrderDetail.colorname;
            demondOrderDetailsVM.size = demondOrderDetail.size;
            demondOrderDetailsVM.HandColorID = demondOrderDetail.HandColorID;
            demondOrderDetailsVM.Notes = demondOrderDetail.Notes;
            demondOrderDetailsVM.Selling_Price = demondOrderDetail.Selling_Price;
            demondOrderDetailsVM.ActualQuantity = demondOrderDetail.ActualQuantity;
            demondOrderDetailsVM.Cust_id = demondOrderDetail.DemondOrder.cust_id;

            ViewBag.HandTypeId = new SelectList(db.HandTypes, "HandType_id", "HandType_name",demondOrderDetailsVM.HandTypeId);
            ViewBag.shap_id = new SelectList(db.Shaplonas.Where(x => x.cust_id == demondOrderDetail.DemondOrder.cust_id), "shap_id", "shap_name");
            ViewBag.Item_Id = new SelectList(db.Items, "ID", "Name",demondOrderDetailsVM.Item_id);
            ViewBag.Colors = new SelectList(db.colors, "color_id", "color_name",demondOrderDetailsVM.HandColorID);

            return View(demondOrderDetailsVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(DemondOrderDetailsVM demondOrderDetailsVM)
        {
            DemondOrderDetail demondOrderDetail = db.DemondOrderDetails.Find(demondOrderDetailsVM.demoOrdDet_id);
            if (ModelState.IsValid)
            {
                demondOrderDetail.demoOrdDet_id = demondOrderDetailsVM.demoOrdDet_id;
                demondOrderDetail.demoOrd_id = demondOrderDetailsVM.demoOrd_id;
                demondOrderDetail.Item_id = demondOrderDetailsVM.Item_id;
                demondOrderDetail.shap_id = demondOrderDetailsVM.shap_id;
                demondOrderDetail.HandTypeId = demondOrderDetailsVM.HandTypeId;
                demondOrderDetail.Hand_Count = demondOrderDetailsVM.Hand_Count;
                demondOrderDetail.Hand_Price = demondOrderDetailsVM.Hand_Price;
                demondOrderDetail.demondQuantity = demondOrderDetailsVM.demondQuantity;
                demondOrderDetail.colorcount = demondOrderDetailsVM.colorcount;
                demondOrderDetail.colorname = demondOrderDetailsVM.colorname;
                demondOrderDetail.size = demondOrderDetailsVM.size;
                demondOrderDetail.Notes = demondOrderDetailsVM.Notes;
                demondOrderDetail.Selling_Price = demondOrderDetailsVM.Selling_Price;
                db.Entry(demondOrderDetail).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index",new { demOrdId=demondOrderDetail.demoOrd_id });
            }
            ViewBag.HandTypeId = new SelectList(db.HandTypes, "HandType_id", "HandType_name", demondOrderDetailsVM.HandTypeId);
            ViewBag.shap_id = new SelectList(db.Shaplonas.Where(x => x.cust_id == demondOrderDetailsVM.Cust_id), "shap_id", "shap_name", demondOrderDetailsVM.shap_id);
            ViewBag.Item_Id = new SelectList(db.Items, "ID", "Name", demondOrderDetailsVM.Item_id);
            ViewBag.Colors = new SelectList(db.colors, "color_id", "color_name",demondOrderDetailsVM.HandColorID);

            return View(demondOrderDetailsVM);
        }

        // GET: DemondOrderDetails/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DemondOrderDetail demondOrderDetail = db.DemondOrderDetails.Find(id);
            if (demondOrderDetail == null)
            {
                return HttpNotFound();
            }
            return View(demondOrderDetail);
        }

        // POST: DemondOrderDetails/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public JsonResult DeleteConfirmed(int id)
        {
            DemondOrderDetail demondOrderDetail = db.DemondOrderDetails.Find(id);
            db.DemondOrderDetails.Remove(demondOrderDetail);
            db.SaveChanges();

            return Json(true);
        }

[HttpGet]
        public PartialViewResult RemotCreate(int demOrdId)
        {
            //Determin Cust From Demond order
            int Custid = db.DemondOrders.FirstOrDefault(x => x.demoOrd_id == demOrdId).cust_id;
            ViewBag.HandTypeId = new SelectList(db.HandTypes, "HandType_id", "HandType_name");
            //Determin Shaplons Based On Cust in Demond Order ....  السريل اللي موجود موجودهفي اسماء الشغل تكون تابعه لنفس العميل 
            ViewBag.shap_id = new SelectList(db.Shaplonas.Where(x => x.cust_id == Custid), "shap_id", "shap_name");
            ViewBag.Item_Id = new SelectList(db.Items, "ID", "Name");
            ViewBag.Cust_id = Custid;

            DemondOrderDetailsVM demondOrderDetailsVM = new DemondOrderDetailsVM();
            demondOrderDetailsVM.Cust_id = Custid;
            demondOrderDetailsVM.demoOrd_id = demOrdId;
            return PartialView(demondOrderDetailsVM);
        }
        [HttpPost]

        public ActionResult RemotCreate(DemondOrderDetailsVM demondOrderDetailVM )
        {
            DemondOrderDetail demondOrderDetail = new DemondOrderDetail();
            if (ModelState.IsValid)
            {
                try { 
                demondOrderDetail.demoOrd_id = demondOrderDetailVM.demoOrd_id;
                demondOrderDetail.Item_id = demondOrderDetailVM.Item_id;
                demondOrderDetail.shap_id = demondOrderDetailVM.shap_id;
                demondOrderDetail.HandTypeId = demondOrderDetailVM.HandTypeId;
                demondOrderDetail.Hand_Count = demondOrderDetailVM.Hand_Count;
                demondOrderDetail.Hand_Price = demondOrderDetailVM.Hand_Price;
                demondOrderDetail.demondQuantity = demondOrderDetailVM.demondQuantity;
                demondOrderDetail.colorcount = demondOrderDetailVM.colorcount;
                demondOrderDetail.colorname = demondOrderDetailVM.colorname;
                demondOrderDetail.size = demondOrderDetailVM.size;
                demondOrderDetail.Selling_Price = demondOrderDetailVM.Selling_Price;
                db.DemondOrderDetails.Add(demondOrderDetail);
                db.SaveChanges();
                return Json(new
                {
                    value = demondOrderDetail.demoOrdDet_id ,
                    text = db.Shaplonas.Find(demondOrderDetailVM.shap_id).shap_name
                }, JsonRequestBehavior.AllowGet);

                }
                catch
                {
                 return   new HttpStatusCodeResult(HttpStatusCode.Conflict, "حدثه مشكله ف البيانات");

                }
            }
            else
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "البيانات غير مكتمله");

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


        #region  methods 
    
        #endregion
    }
}
