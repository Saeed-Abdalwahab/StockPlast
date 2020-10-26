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
using StockViewModel;

namespace StockPortal.Controllers
{
    [HandleError]
    [CustomAuthorize("الاصناف")]
    [SessionExpire]
    public class ItemsController : BaseController
    {
        private StockModel db = new StockModel();

        // GET: Items
        public ActionResult Index()
        {
            List<Item> itemList = db.Items.ToList();
            return View(itemList);
        }

        // GET: Items/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Item item = db.Items.Find(id);
            if (item == null)
            {
                return HttpNotFound();
            }
            return View(item);
        }

        public ActionResult Create()
        {
            ViewBag.Mattypid = new SelectList(db.Material_Type.ToList(), "mat_id", "mat_name");
            ViewBag.Colorid = new SelectList(db.colors.ToList(), "color_id", "color_name");
            ItemVM itemVM = new ItemVM();
            var item = db.Items.OrderByDescending(x => x.ID).FirstOrDefault();
            itemVM.SerialNum = (item != null ? item.ID + 1 : 1).ToString();
            return View(itemVM);
        }

        // GET: Items/Create
        public PartialViewResult RemotCreate()
        {
            ViewBag.Mattypid= new SelectList(db.Material_Type.ToList(), "mat_id", "mat_name");
            ViewBag.Colorid = new SelectList(db.colors.ToList(), "color_id", "color_name");
            ItemVM itemVM = new ItemVM();
            var item = db.Items.OrderByDescending(x => x.ID).FirstOrDefault();
            itemVM.SerialNum = (item!=null ? item.ID+1:1).ToString();
            return PartialView(itemVM);
        }
        [HttpPost]
        public ActionResult RemotCreate([Bind(Include = "Name,MatTypeId,ColorID,SerialNum,PurchasingPrice,SellingPrice,Size,Thickness")] ItemVM itemVM)
        {
            try
            {
                ViewBag.Mattypid = new SelectList(db.Material_Type.ToList(), "mat_id", "mat_name");
                ViewBag.Colorid = new SelectList(db.colors.ToList(), "color_id", "color_name");
                if (ModelState.IsValid)
                {
                    Item item = new Item();
                    item.Name = itemVM.Name;
                    item.MatTypeId = itemVM.MatTypeId;
                    item.ColorID = itemVM.ColorID;
                    item.SerialNum = itemVM.SerialNum;
                    item.PurchasingPrice = itemVM.PurchasingPrice;
                    item.SellingPrice = itemVM.SellingPrice;
                    item.Thickness = itemVM.Thickness;
                    item.Size = itemVM.Size;
                    if (checke(item.SerialNum))
                    {
                        itemVM.SerialNum = db.Items.OrderByDescending(x => x.ID).FirstOrDefault().SerialNum;
                        return Json(new { success = false, responseText = "هذا الكود موجود من قبل برجاء اختيار كود اخر" }, JsonRequestBehavior.AllowGet);

                    }
                    db.Items.Add(item);
                    db.SaveChanges();
                    return Json(new
                    {
                        success = true,
                        value = item.ID,
                        text = item.Name
                    }, JsonRequestBehavior.AllowGet);
                }
                return Json(new { success = false, responseText = "البيانات غير مكتمله تاكد من ادخال جميع الحقول" }, JsonRequestBehavior.AllowGet);

            }
            catch
            {
                 return   new HttpStatusCodeResult(HttpStatusCode.Conflict, "حدثه مشكله ف البيانات");

            }
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Name,MatTypeId,ColorID,SerialNum,PurchasingPrice,SellingPrice,Size,Thickness")] ItemVM itemVM)
        {
            ViewBag.Mattypid = new SelectList(db.Material_Type.ToList(), "mat_id", "mat_name");
            ViewBag.Colorid = new SelectList(db.colors.ToList(), "color_id", "color_name");
            if (ModelState.IsValid)
            {
                Item item = new Item();
                item.Name = itemVM.Name;
                item.MatTypeId = itemVM.MatTypeId;
                item.ColorID = itemVM.ColorID;
                item.SerialNum = itemVM.SerialNum;
                item.PurchasingPrice = itemVM.PurchasingPrice;
                item.SellingPrice = itemVM.SellingPrice;
                item.Thickness = itemVM.Thickness;
                item.Size = itemVM.Size;
                if (checke(item.SerialNum))
                {
                    ViewBag.ErrorExist = "هذا الكود مستخدم برجاء ادخال كود اخر";
                    itemVM.SerialNum = db.Items.OrderByDescending(x => x.ID).FirstOrDefault().SerialNum;
                    return View(itemVM);

                }
                db.Items.Add(item);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(itemVM);
        }

        // GET: Items/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Item item = db.Items.Find(id);
            if (item == null)
            {
                return HttpNotFound();
            }
            ViewBag.Mattypid = new SelectList(db.Material_Type.ToList(), "mat_id", "mat_name");
            ViewBag.Colorid = new SelectList(db.colors.ToList(), "color_id", "color_name");
            ItemVM itemVM = new ItemVM();
            itemVM.Name = item.Name;
            itemVM.MatTypeId = (int)item.MatTypeId;
            itemVM.ColorID =(int)item.ColorID;
            itemVM.SerialNum = item.SerialNum;
            itemVM.PurchasingPrice = item.PurchasingPrice;
            itemVM.SellingPrice = item.SellingPrice;
            itemVM.Thickness =(double) item.Thickness;
            itemVM.Size = item.Size;
            itemVM.ID = item.ID;
            return View(itemVM);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Name,ID,MatTypeId,ColorID,SerialNum,PurchasingPrice,SellingPrice,Size,Thickness")] ItemVM itemVM)
        {
            ViewBag.Mattypid = new SelectList(db.Material_Type.ToList(), "mat_id", "mat_name");
            ViewBag.Colorid = new SelectList(db.colors.ToList(), "color_id", "color_name");
            if (ModelState.IsValid)
            {
                Item item = new Item();
                item.Name = itemVM.Name;
                item.MatTypeId = itemVM.MatTypeId;
                item.ColorID = itemVM.ColorID;
                item.SerialNum = itemVM.SerialNum;
                item.PurchasingPrice = itemVM.PurchasingPrice;
                item.SellingPrice = itemVM.SellingPrice;
                item.Thickness = itemVM.Thickness;
                item.Size = itemVM.Size;
                item.ID = itemVM.ID;
                if (checke(item.SerialNum,item.ID))
                {
                    ViewBag.ErrorExist = "هذا الكود مستخدم برجاء ادخال كود اخر";
                    itemVM.SerialNum = db.Items.OrderByDescending(x => x.ID).FirstOrDefault().SerialNum;
                    return View(itemVM);

                }
                db.Entry(item).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }          
            return View(itemVM);
        }

        // GET: Items/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Item item = db.Items.Find(id);
            if (item == null)
            {
                return HttpNotFound();
            }
            return View(item);
        }

        // POST: Items/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Item item = db.Items.Find(id);
            db.Items.Remove(item);
            db.SaveChanges();
            return RedirectToAction("Index");
        }



        private bool checke(string itemserial,int? ID=null)
        {
            if (ID == null)
            {
                var item = db.Items.FirstOrDefault(x => x.SerialNum == itemserial);
                return true ? item != null : false;
            }
            else
            {
                var item = db.Items.FirstOrDefault(x => x.SerialNum == itemserial&&x.ID!=ID);
                return true ? item != null : false;
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
