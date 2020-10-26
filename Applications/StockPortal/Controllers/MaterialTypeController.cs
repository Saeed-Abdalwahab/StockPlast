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
    [CustomAuthorize("الخامات")]

    [SessionExpire]
    public class MaterialTypeController : Controller
    {
        private StockModel db = new StockModel();
        private MaterialTypeService MaterialTypeService = new MaterialTypeService();

        // GET: Materialtype
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public JsonResult GetMaterials()
        {
            var AllMterials = MaterialTypeService.GettAll();
            return Json(new { data = AllMterials }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetMaterialbyid(int id)
        {

            var Material = MaterialTypeService.GetByID(id);
            return Json(new { data = Material }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Save(MaterialTypeVM materialTypeVM)
        {

            return Json(MaterialTypeService.SaveinDatabase(materialTypeVM), JsonRequestBehavior.AllowGet);

        }


        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                Material_Type material_Type = db.Material_Type.Find(id);
                db.Material_Type.Remove(material_Type);
                db.SaveChanges();
                return Json(new { success =true}, JsonRequestBehavior.AllowGet);
            }
            catch 
            {
                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
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