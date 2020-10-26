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
using StockPortal.Helpers;
using StockPortal.Models;

namespace StockPortal.Controllers
{
    [HandleError]
    [CustomAuthorize("الموظفين")]
    [SessionExpire]
    public class EmployesController : BaseController
    {
        private StockModel db = new StockModel();

        // GET: Employes/ عرض بيانات جميع الفنيين
        //public ActionResult Index()
        //{

        //   int techEnum=  int.Parse(Enums.EnumString.GetStringValue(Enums.job.Technician));
        //    var employes = db.Employes.Include(e => e.Job).Where(xx=>xx.Job_id== techEnum);

        //    return View(employes.ToList());
        //}


        // GET: Employes/Create // اضافة فني

        #region get method  اضافة فني 
        //public ActionResult Create()
        //{
        //    ViewBag.Job_id = new SelectList(db.Jobs, "job_id", "job_name");
        //    return View();
        //}
        #endregion

        #region post method اضافة فني جديد
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Create([Bind(Include = "emp_name,emp_mobile")] Employe employe)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        int techEnum = int.Parse(Enums.EnumString.GetStringValue(Enums.job.Technician));
        //        employe.Job_id = techEnum;
        //        db.Employes.Add(employe);
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }


        //    return View(employe);
        //}
        #endregion

        #region get method تعديل بيانات فني
        //public ActionResult Edit(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Employe employe = db.Employes.Find(id);
        //    if (employe == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    ViewBag.Job_id = new SelectList(db.Jobs, "job_id", "job_name", employe.Job_id);
        //    return View(employe);
        //}
        // تعديل بيانات فني+
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "emp_id,emp_name,emp_mobile")] Employe employe)
        {
            if (ModelState.IsValid)
            {
                //Set Employee As A Technician
                employe.Job_id = int.Parse(Enums.EnumString.GetStringValue(Enums.job.Technician));
                db.Entry(employe).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Job_id = new SelectList(db.Jobs, "job_id", "job_name", employe.Job_id);
            return View(employe);
        }
        #endregion

        #region get method حذف بيانات فني
        // GET: Employes/Delete/5
        //public ActionResult Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Employe employe = db.Employes.Find(id);
        //    if (employe == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(employe);
        //}
        #endregion

        #region post method حذف بيانات فني
        // POST: Employes/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public ActionResult DeleteConfirmed(int id)
        //{
        //    Employe employe = db.Employes.Find(id);
        //    db.Employes.Remove(employe);
        //    db.SaveChanges();
        //    return RedirectToAction("Index");
        //}
        #endregion







        //Created by samir  hussein 31-12-2019 - دوال تقوم باضافة وتعديل وحذف بيانات الموظفين والاداريين
        #region get method عرض بيانات جميع الموظفين
        public ActionResult IndexEmploye()
        {
            //int techEnum = int.Parse(Enums.EnumString.GetStringValue(Enums.job.Technician));
            var employes = db.Employes; /*.Include(e => e.Job).Where(xx => xx.Job_id != techEnum)*/
            return View(employes.ToList());
        }
        #endregion

        #region get method داله اضافة بيانات موظف 
        // GET: Employes/Create
        public ActionResult CreateEmploye()
        {
            ViewBag.joblist = new SelectList(db.Jobs.ToList(), "job_id", "job_name");
            ViewBag.jobdescription = new SelectList(db.JobDescriptions.ToList(), "ID", "Name");

            return View();
        }
        #endregion

        #region post method داله اضافة بيانات موظف 
        [HttpPost]
        [ValidateAntiForgeryToken]

        public ActionResult CreateEmploye(Employe employe)
        {
            if (ModelState.IsValid)
            {
                #region  طريقتين للتاكد من التحقق من عدم وجود البيان مرة اخري في قاعدة البيانات 
                //db.Employes.Any(p=>p.emp_name.Equals(employe.emp_name)) && db.Employes.Any(m => m.Job_id.Equals(employe.Job_id))
                //check != null

                //db.Employes.Any(p => p.emp_name.Equals(employe.emp_name))
                #endregion

                Employe check = db.Employes.FirstOrDefault(x => x.emp_name == employe.emp_name);
                if (check != null)
                {
                    ModelState.AddModelError("emp_name", "اسم الموظف موجود بسجل الموظفين");
                    List<Job> joblist = db.Jobs.ToList();
                    ViewBag.joblist = new SelectList(joblist, "job_id", "job_name");
                    ViewBag.jobdescription = new SelectList(db.JobDescriptions.ToList(), "ID", "Name");

                    //ViewBag.Job_id = new SelectList(db.Jobs, "job_id", "job_name");
                    return View("CreateEmploye" , employe);
                }
                else
                {
                    db.Employes.Add(employe);
                    db.SaveChanges();
                    return RedirectToAction("IndexEmploye");
                }
   
            }


            return View(employe);
        }
        #endregion

        #region get method داله لتعديل بيانات موظف 

        public ActionResult EditEmploye(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Employe employe = db.Employes.Find(id);
            ViewBag.Job_id = new SelectList(db.Jobs.ToList(), "job_id", "job_name", employe.Job_id);
            ViewBag.jobdescription = new SelectList(db.JobDescriptions.ToList(), "ID", "Name",employe.jobDesc_id);

            if (employe == null)
            {
                return HttpNotFound();
            }
            return View(employe);
        }

        #endregion

        #region post method داله لتعديل بيانات موظف 
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditEmploye(Employe employe)
        {
            ViewBag.Job_id = new SelectList(db.Jobs.ToList(), "job_id", "job_name", employe.Job_id);
            ViewBag.jobdescription = new SelectList(db.JobDescriptions.ToList(), "ID", "Name", employe.jobDesc_id);

            bool checkname = db.Employes.Where(m=>m.emp_name == employe.emp_name && m.emp_id!=employe.emp_id).Any();
            if (ModelState.IsValid)
            {
                if (checkname)
                {
                    ModelState.AddModelError("emp_name", "اسم الموظف موجود بسجل الموظفين");
                  
                    return View("EditEmploye");
                }
                else
                {
                    db.Entry(employe).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("IndexEmploye");
                }
              
            }
            

            return View();
        }
        #endregion

        #region get method داله لحذف بيانات موظف 
        [HttpGet]
        // GET: Employes/Delete/5
        public ActionResult DeleteEmploye(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Employe employe = db.Employes.Find(id);
            if (employe == null)
            {
                return HttpNotFound();
            }
            return View(employe);
        }
        #endregion

        #region post method داله لحذف بيانات موظف 
        // POST: Employes/Delete/5
        [HttpPost, ActionName("DeleteEmploye")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmedEmploye(int id)
        {
            Employe employe = db.Employes.Find(id);
            db.Employes.Remove(employe);
            db.SaveChanges();
            return RedirectToAction("IndexEmploye");
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


        #region employeeviewbage داله عرض كل الوظائف من قاعدةالبيانات

        //public void employeeviewbage()
        //{
        //    //int techEnum = int.Parse(Enums.EnumString.GetStringValue(Enums.job.Technician));
        //    Employe employe = db.Employes.Find(id);
        //    ViewBag.Job_id = new SelectList(db.Jobs, "job_id", "job_name", employe.Job_id); /*.Include(e => e.Employes).Where(xx => xx.job_id != techEnum), */
        //}
        #endregion
    }
}
