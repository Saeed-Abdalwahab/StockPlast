using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using PlasticStatic;
using StockDB.Model;
using StockPortal.ViewModel;
using StockService;
using StockViewModel;
using static PlasticStatic.Enums;
using StockPortal.Helper;
using StockPortal.Helpers;
using StockPortal.Models;

namespace StockPortal.Controllers
{
    [HandleError]
    [CustomAuthorize("أوامر التشغيل")]
    [SessionExpire]
    public class FromStocksController : BaseController
    {
        private StockModel db = new StockModel();

        public ActionResult Index()
        {
            var fromStocks = db.FromStocks.Include(f => f.Employe).
               Include(f => f.DemondOrder).
               Include(ww => ww.DemondOrder.Customer).
               Include(ww => ww.DemondOrder.Safe).
               Include(ww => ww.DemondOrder.DemondOrderDetails);
            fromStocks.Where(o=>o.FromStockTypeID==(int)FromStockTypeId.OperationOrder).OrderBy(x => x.TransDate);

            return View(fromStocks.ToList());
        }


        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FromStock fromStock = db.FromStocks.Find(id);
            if (fromStock == null)
            {
                return HttpNotFound();
            }
            return View(fromStock);
        }

        public ActionResult Create()
        {
            FromStockVM fromStockVM = new FromStockVM();
            fromStockVM.selectedDemondOrderDetailsId = returnAllNotFinishedDemondOrderDetialsDDL(null);
            ViewBag.CutEmps = new SelectList(db.Employes.Where(x => x.Job_id == (int)Enums.job.Technician && x.jobDesc_id == (int)Enums.jobDescription.CutingTechnician).ToList(), "emp_id", "emp_name");
            ViewBag.PrintEmps = new SelectList(db.Employes.Where(x => x.Job_id == (int)Enums.job.Technician && x.jobDesc_id == (int)Enums.jobDescription.PrintingTechnician).ToList(), "emp_id", "emp_name");
            var Checkk = db.FromStocks.OrderByDescending(x => x.ID).FirstOrDefault();
            fromStockVM.Serial = (Checkk!=null? Checkk.ID + 1:1).ToString();
            fromStockVM.TransDate = DateTime.Now;
            return View(fromStockVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "PrintTechEmp_ID,CutTechEmp_ID,Serial,TransDate,arrayOfselectedString,notes")] FromStockVM fromStockVM)
        {
            if (ModelState.IsValid)
            {
                if (db.FromStocks.FirstOrDefault(x => x.Serial == fromStockVM.Serial) != null)
                {
                    fromStockVM.selectedDemondOrderDetailsId = returnAllNotFinishedDemondOrderDetialsDDL(null);
                    ViewBag.CutEmps = new SelectList(db.Employes.Where(x => x.Job_id == (int)Enums.job.Technician && x.jobDesc_id == (int)Enums.jobDescription.CutingTechnician).ToList(), "emp_id", "emp_name");
                    ViewBag.PrintEmps = new SelectList(db.Employes.Where(x => x.Job_id == (int)Enums.job.Technician && x.jobDesc_id == (int)Enums.jobDescription.PrintingTechnician).ToList(), "emp_id", "emp_name");

                    ModelState.AddModelError("Serial", "هذا الرقم مسجل من قبل");
                    return View("Create", fromStockVM);
                }
                int FromStockTypeId = int.Parse(Enums.EnumString.GetStringValue(Enums.FromStockTypeId.OperationOrder));

                FromStock fromStock = new FromStock();
                fromStock.PrintTechEmp_ID = fromStockVM.PrintTechEmp_ID;
                fromStock.CutTechEmp_ID = fromStockVM.CutTechEmp_ID;
                fromStock.FromStockTypeID = FromStockTypeId;
                fromStock.Serial = fromStockVM.Serial;
                fromStock.notes = fromStockVM.notes;
                fromStock.TransDate = fromStockVM.TransDate;
                fromStock.Status = (int)PlasticStatic.Enums.InvoiceStatus.Finish;
                foreach (var item in fromStockVM.arrayOfselectedString)
                {
                    fromStock.DemondOrderId = item;
                    db.FromStocks.Add(fromStock);
                    db.SaveChanges();
                }
                ViewBag.Msg = "تم الحفظ بنجاح ";
                return RedirectToAction("Edit",new { id =fromStock.ID});
            }
            ViewBag.msg = "لم يتم الحفظ تاكد من البيانات";

            return View(fromStockVM);
        }


        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FromStock fromStock = db.FromStocks.Find(id);
            if (fromStock == null)
            {
                return HttpNotFound();
            }
            //ViewBag.DemondOrderId = returnAllNotFinishedDemondOrderDetialsDDL(null);
            TempData["fromstockid"] = fromStock.ID;
            try
            {
            TempData["TechName"] = fromStock.Employe.emp_name;
            }
            catch
            {
                TempData["TechName"] = "لا يوجد اسم ";

            }
            int FromStockTypeId = int.Parse(Enums.EnumString.GetStringValue(Enums.FromStockTypeId.OperationOrder));

            FromStockVM fromStockVM = new FromStockVM();
            ViewBag.CutEmps = new SelectList(db.Employes.Where(x => x.Job_id == (int)Enums.job.Technician && x.jobDesc_id == (int)Enums.jobDescription.CutingTechnician).ToList(), "emp_id", "emp_name",fromStock.CutTechEmp_ID);
            ViewBag.PrintEmps = new SelectList(db.Employes.Where(x => x.Job_id == (int)Enums.job.Technician && x.jobDesc_id == (int)Enums.jobDescription.PrintingTechnician).ToList(), "emp_id", "emp_name",fromStock.PrintTechEmp_ID);
            ViewBag.cusTomerName = fromStock.DemondOrder.Customer.cust_name;
            fromStockVM.CutTechEmp_ID = (int)fromStock.CutTechEmp_ID;
            fromStockVM.PrintTechEmp_ID = (int)fromStock.PrintTechEmp_ID;
            fromStockVM.arrayOfselectedString.Add((int)fromStock.DemondOrderId);
            fromStockVM.notes = fromStock.notes;
            fromStockVM.Serial = fromStock.Serial;
            fromStockVM.TransDate = fromStock.TransDate;
            fromStockVM.ID = fromStock.ID;
            return View(fromStockVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit( FromStockVM fromStockVM)
        {
            if (ModelState.IsValid)
            {
                if (db.FromStocks.FirstOrDefault(x => x.Serial == fromStockVM.Serial&&x.ID!=fromStockVM.ID) != null)
                {
                    var fromStockol = db.FromStocks.Find(fromStockVM.ID);
                     ViewBag.CutEmps = new SelectList(db.Employes.Where(x => x.Job_id == (int)Enums.job.Technician && x.jobDesc_id == (int)Enums.jobDescription.CutingTechnician).ToList(), "emp_id", "emp_name", fromStockol.CutTechEmp_ID);
            ViewBag.PrintEmps = new SelectList(db.Employes.Where(x => x.Job_id == (int)Enums.job.Technician && x.jobDesc_id == (int)Enums.jobDescription.PrintingTechnician).ToList(), "emp_id", "emp_name", fromStockol.PrintTechEmp_ID);
            ViewBag.cusTomerName = fromStockol.DemondOrder.Customer.cust_name;
   
                    ModelState.AddModelError("Serial", "هذا الرقم مسجل من قبل");
                    return View("Edit", fromStockVM);
                }
                FromStock fromStock = db.FromStocks.Find(fromStockVM.ID);
                fromStock.PrintTechEmp_ID = fromStockVM.PrintTechEmp_ID;
                fromStock.CutTechEmp_ID = fromStockVM.CutTechEmp_ID;
                fromStock.Serial = fromStockVM.Serial;
                fromStock.notes = fromStockVM.notes;
                fromStock.TransDate = fromStockVM.TransDate;
                ViewBag.cusTomerName = fromStock.DemondOrder.Customer.cust_name;

                db.Entry(fromStock).State = EntityState.Modified;
                db.SaveChanges();
                ViewBag.Msg = "تم التعديل بنجاح";
                TempData["TechName"] = db.Employes.FirstOrDefault(x => x.emp_id == fromStockVM.PrintTechEmp_ID).emp_name;
                TempData["fromstockid"] = fromStock.ID;
                ViewBag.CutEmps = new SelectList(db.Employes.Where(x => x.Job_id == (int)Enums.job.Technician && x.jobDesc_id == (int)Enums.jobDescription.CutingTechnician).ToList(), "emp_id", "emp_name", fromStock.CutTechEmp_ID);
                ViewBag.PrintEmps = new SelectList(db.Employes.Where(x => x.Job_id == (int)Enums.job.Technician && x.jobDesc_id == (int)Enums.jobDescription.PrintingTechnician).ToList(), "emp_id", "emp_name", fromStock.PrintTechEmp_ID);
                return View(fromStockVM);
            }
            ViewBag.Msg = "حدثت مشكله لم يتم التعديل";
            return View(fromStockVM);
        }


        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FromStock fromStock = db.FromStocks.Find(id);
            if (fromStock == null)
            {
                return HttpNotFound();
            }
            return PartialView(fromStock);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            FromStock fromStock = db.FromStocks.FirstOrDefault(x=>x.ID==id);
            db.FromStocks.Remove(fromStock);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        #region methods
        public String ConcatOrderDetilsName(int? selectedDemondOrderDetialsId)
        {
            
  
          DemondOrderDetail demondOrderDetails = db.DemondOrderDetails.FirstOrDefault(x => x.demoOrd_id == selectedDemondOrderDetialsId);
           return  demondOrderDetails.DemondOrder.Customer.cust_name  + demondOrderDetails.DemondOrder.Safe.safe_name ;
         
        }

        public List<SelectListItem> returnAllNotFinishedDemondOrderDetialsDDL(int? selectedDemondOrderDetialsId)
        {
            //int statusnotfinished = int.Parse(Enums.EnumString.GetStringValue(Enums.demondOrderStatus.NotFinished));
            List<SelectListItem> list = new List<SelectListItem>();
            List<DemondOrder> demondOrderList = db.DemondOrders.Where(x=>x.DemondOrderDetails.Any(xx=>xx.status==(int)demondorderdetailstatus.NotStarted)).ToList();
            demondOrderList.RemoveAll(x => db.FromStocks.FirstOrDefault(xx => xx.DemondOrderId == x.demoOrd_id && xx.FromStockTypeID == (int)FromStockTypeId.OperationOrder) != null);
            foreach (DemondOrder item in demondOrderList)
            {
                list.Add(new SelectListItem { Value = item.demoOrd_id.ToString(), Text = item.Customer.cust_name, Selected = (item.demoOrd_id == selectedDemondOrderDetialsId ? true : false) });
            }
            return list;
        }
        #endregion

        public ActionResult FromStockDetails(int ID)
        {
            

            FromStockService fS = new FromStockService();


            //هنغيرها بعدين
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




        //#endregion    
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
