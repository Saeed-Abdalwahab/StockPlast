using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using StockDB.Model;
using PlasticStatic;
using StockPortal.Helpers;
using StockPortal.Models;


namespace StockPortal.Controllers
{
    [HandleError]
    [CustomAuthorize("المخازن")]

    [SessionExpire]
    public class StoresController : Controller
    {
        private StockModel db = new StockModel();

        #region get method داله عرض بيانات المخازن


        // GET: Stores
        public ActionResult Index()
        {
         
            var stores = db.Stores;
            return View(stores.ToList());
        }

        #endregion

        

        #region get method داله اضافة بيانات المخازن

        [HttpGet]
        // GET: Stores/Create
        public ActionResult Create()
        {
            Store store = new Store();
            var ss = db.Stores.OrderByDescending(x => x.st_id).FirstOrDefault();

            store.st_SerialNum = (ss!=null?ss.st_id+1:1).ToString();
            FillViewBage();

            return View(store); //"Create",
        }
        #endregion

        #region post method داله اضافة بيانات المخازن
        // POST: Stores/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "st_id,st_SerialNum,emp_id,st_name,StoreType_ID,st_address")] Store store)
        {
    
            if (ModelState.IsValid)
            {
                bool checkname = db.Stores.Where(m => m.st_name == store.st_name&& m.StoreType_ID==store.StoreType_ID).Any();
                bool checkid = db.Stores.Where(m => m.st_SerialNum == store.st_SerialNum).Any();
                //List<Store> new List
                if (checkname)
                {
                    ModelState.AddModelError("st_name", "هذ المخزن موجود بسجل المخازن");
                    FillViewBage();
                    return View("Create", store);
                }
                else if (checkid)
                {
                    ModelState.AddModelError("st_SerialNum", "هذ المخزن موجود بسجل المخازن");
                    FillViewBage();
                }
                else
                {
                    FillViewBage();
                    db.Stores.Add(store);
                    db.SaveChanges();
                    return RedirectToAction("index");
                }
            }
            FillViewBage();



            return View(); //"Create",
        }


        #endregion

        #region get method داله تعديل بيانات المخازن
        // GET: Stores/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Store store = db.Stores.Find(id);
            FillViewBage();
            if (store == null)
            {
                return HttpNotFound();
            }
            ViewBag.StorTypeName = ((Enums.StoreType)store.StoreType_ID).GetDisplayName();
            FillViewBage();
            return View(store);
        }
        #endregion

        #region post method داله تعديل بيانات المخازن
        // POST: Stores/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "st_id,st_SerialNum,emp_id,st_name,StoreType_ID,st_address")] Store store)
        {
            bool checkname = db.Stores.Where(m => m.st_name == store.st_name &&m.StoreType_ID==store.StoreType_ID && m.st_id != store.st_id).Any();
            bool checkid = db.Stores.Where(m => m.st_SerialNum == store.st_SerialNum &&  m.st_id != store.st_id).Any();
            if (ModelState.IsValid)
            {
                if (checkname)
                {
                    FillViewBage();
                    ModelState.AddModelError("st_name", "هذ المخزن موجود بسجل المخازن");
                }


                else if (checkid)
                {
                    ModelState.AddModelError("st_SerialNum", "هذ المخزن موجود بسجل المخازن");
                    FillViewBage();
                }
                else
                {
                    
                    db.Entry(store).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("index");
                }

            }
            ViewBag.StorTypeName = ((Enums.StoreType)store.StoreType_ID).GetDisplayName();
            FillViewBage();
            return View(store);
        }

        #endregion

        #region get method داله حذف بيانات المخازن
        // GET: Stores/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Store store = db.Stores.Find(id);
            if (store == null)
            {
                return HttpNotFound();
            }
            return View(store);
        }
        #endregion

        #region post method داله حذف بيانات المخازن
        // POST: Stores/Delete/5
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {

                Store store = db.Stores.Find(id);
                db.Stores.Remove(store);
                db.SaveChanges();
                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }


            
        }
        #endregion

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Fillviewbage created by samir hussien 29-12-2019 get alll Storekeeper & all storetyps

        public void FillViewBage()
        {
        Store store = new Store();
        int StorekeeperEnum = int.Parse(Enums.EnumString.GetStringValue(Enums.job.Storekeeper));
        var employes = db.Employes.Include(e => e.Job).Where(p => p.Job_id == StorekeeperEnum).ToList();
        ViewBag.emp_id = new SelectList(employes, "emp_id", "emp_name", store.emp_id);
        ViewBag.StoreType_ID = new SelectList(db.StoreTypes, "ID", "Name", store.StoreType_ID);
        }
        #endregion
    }
}
