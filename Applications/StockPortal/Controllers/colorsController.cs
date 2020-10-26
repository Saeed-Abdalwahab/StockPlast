using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using StockDB.Model;
using StockPortal.Controllers;
using StockPortal.Helpers;
using StockPortal.Models;
using StockService;
using StockViewModel;

namespace TransportPortal.Controllers
{
    [HandleError]
    [CustomAuthorize("الالوان")]

    [SessionExpire]
    public class colorsController : BaseController
    {
        private StockModel db = new StockModel();
        private ColorService colorService = new ColorService();
        #region get method داله عرض بيانات الالوان
        // GET: colors
        //public ActionResult Index()
        //{
        //    return View(db.colors.ToList());
        //}
        #endregion

        

        #region GetAllColors Josn
        public ActionResult GetAllColors()
        {
            return View();
        }
        [HttpGet]
        public JsonResult GetAllColorsForDataTable()
        {
            var AllColors = colorService.AllColors();
            return Json(new { data = AllColors }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region add && update new color get Method json
        [HttpGet]
        public JsonResult GetColorById(int color_id)
        {

            var color = colorService.GetColorById(color_id);
            return Json(new { data = color }, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Method To Save Changes In Color Add -- Update
        [HttpPost]
        public JsonResult SaveColorInDB(ColorVM colorVM)
        {

            bool checkname = db.colors.Where(m => m.color_name == colorVM.color_name).Any();
            if (checkname)
            {
                //ModelState.AddModelError("color_name", "هذا اللون موجود بسجل الالوان");
                throw new Exception();
            }

            return Json(colorService.SaveinDatabase(colorVM),JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region get method داله حذف بيانات الالوان
        // GET: colors/Delete/5
        public ActionResult _DeleteColor(int? color_id)
        {
            if (color_id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ColorVM colorVM = db.colors.Select(x => new ColorVM()
            {
                color_id = x.color_id,
                color_name = x.color_name
            }).FirstOrDefault(m => m.color_id == color_id);
            if (colorVM == null)
            {
                return HttpNotFound();
            }
            return PartialView(colorVM);
        }
        #endregion

        #region post method داله حذف بيانات الالوان
        // POST: colors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public bool DeleteConfirmed(int id)
        {
            try
            {
                color color = db.colors.Find(id);
                db.colors.Remove(color);
                db.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }
        #endregion



        #region get method داله اضافة بيانات الالوان

        //public ActionResult Create()
        //{
        //    return View();
        //}
        #endregion

        #region post method داله اضافة بيانات الالوان
        //// POST: colors/Create
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Create([Bind(Include = "color_id,color_name")] color color)
        //{
        //    bool checkname = db.colors.Where(m => m.color_name == color.color_name).Any();
        //    if (ModelState.IsValid)
        //    {
        //        if (checkname)
        //        {
        //            ModelState.AddModelError("color_name", "هذا اللون موجود بسجل الالوان");
        //        }
        //        else
        //        {
        //            db.colors.Add(color);
        //            db.SaveChanges();
        //            return RedirectToAction("Index");
        //        }

        //    }

        //    return View(color);
        //}
        #endregion

        #region get method داله تعديل بيانات الالوان
        //// GET: colors/Edit/5
        //public ActionResult Edit(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    color color = db.colors.Find(id);
        //    if (color == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(color);
        //}
       #endregion

        #region post method داله تعديل بيانات الالوان
        // POST: colors/Edit/5
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Edit([Bind(Include = "color_id,color_name")] color color)
        //{
        //    bool checkname = db.colors.Where(m => m.color_name == color.color_name && m.color_id != color.color_id).Any();

        //    if (ModelState.IsValid)
        //    {

        //        if (checkname)
        //        {
        //            ModelState.AddModelError("color_name", "هذا اللون موجود بسجل الالوان");
        //        }
        //        else
        //        {
        //            db.Entry(color).State = EntityState.Modified;
        //            db.SaveChanges();
        //            return RedirectToAction("Index");
        //        }

        //    }
        //    return View(color);
        //}
        #endregion

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
