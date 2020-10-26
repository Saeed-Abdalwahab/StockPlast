using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using StockPortal.Models;
using StockDB.Model;
using PagedList;
using StockPortal.Helpers;

namespace StockPortal.Controllers
{
    [HandleError]
    [CustomAuthorize("وحدات المنظومة")]
    
    [SessionExpire]
    public class SecurityModulesController : BaseController
    {
        private StockDB.Model.StockModel db = new StockDB.Model.StockModel();

        // GET: SecurityModules
        public ActionResult Index(int? page)
        {
            Session["PageLinkName"] = "وحدات المنظومة";

            var pageSize = 15;
            var pageNumber = page ?? 1;

            var result = db.AspNetModules.ToList();
            return View(result.ToPagedList(pageNumber, pageSize));
        }

        // GET: SecurityModules/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AspNetModule aspNetModule = db.AspNetModules.Find(id);
            if (aspNetModule == null)
            {
                return HttpNotFound();
            }
            return View(aspNetModule);
        }

        // GET: SecurityModules/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: SecurityModules/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name")] AspNetModule aspNetModule)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    db.AspNetModules.Add(aspNetModule);
                    db.SaveChanges();


                    Notification(NotificationType.SaveSuccess, true);
                    return RedirectToAction("Index");
                }
                catch (Exception)
                {
                    Notification(NotificationType.SaveError, true);
                    return View(aspNetModule);
                }
            }

            Notification(NotificationType.SaveError, true);
            return View(aspNetModule);
        }

        // GET: SecurityModules/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AspNetModule aspNetModule = db.AspNetModules.Find(id);
            if (aspNetModule == null)
            {
                return HttpNotFound();
            }

            return View(aspNetModule);
        }

        // POST: SecurityModules/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name")] AspNetModule aspNetModule)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var oldObj = db.AspNetModules.Find(aspNetModule.Id);
                    db.Entry(oldObj).State = EntityState.Detached;
                    //   var oldObj = db.AspNetModules.Attach(aspNetModule);
                    db.Entry(aspNetModule).State = EntityState.Modified;
                    db.SaveChanges();


                    Notification(NotificationType.UpdateSuccess, true);
                    return RedirectToAction("Index");
                }
                catch (Exception)
                {
                    Notification(NotificationType.UpdateError, true);
                    return View(aspNetModule);
                }
            }

            Notification(NotificationType.UpdateError, true);
            return View(aspNetModule);
        }

        // GET: SecurityModules/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AspNetModule aspNetModule = db.AspNetModules.Find(id);
            if (aspNetModule == null)
            {
                return HttpNotFound();
            }
            return View(aspNetModule);
        }

        // POST: SecurityModules/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            AspNetModule aspNetModule = db.AspNetModules.Find(id);
            if (aspNetModule != null)
            {
                try
                {
                    db.AspNetModules.Remove(aspNetModule);
                    db.SaveChanges();

                    Notification(NotificationType.DeleteSuccess, true);
                    return RedirectToAction("Index");
                }
                catch (Exception)
                {
                    Notification(NotificationType.DeleteError, true);
                    return View(aspNetModule);
                }
            }
            Notification(NotificationType.DeleteInformation, true);
            return RedirectToAction("Index");
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
