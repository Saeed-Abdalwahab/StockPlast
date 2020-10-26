using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using PagedList;

using StockPortal.Helpers;
using StockPortal.Models;
using StockDB.Model;


namespace StockPortal.Controllers
{
    [HandleError]
    [CustomAuthorize("أنواع المستخدمين")]
    [SessionExpire]
    public class SecurityUserTypeController : BaseController
    {
        private StockDB.Model.StockModel db = new StockDB.Model.StockModel();

        // GET: SecurityUserType
        public ActionResult Index(int? page)
        {
            Session["PageLinkName"] = "أنواع المستخدمين";

            var pageSize = 15;
            var pageNumber = page ?? 1;

            var result = db.AspNetUserTypes.ToList();
            return View(result.ToPagedList(pageNumber, pageSize));
        }

        // GET: SecurityUserType/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AspNetUserType aspNetUserType = db.AspNetUserTypes.Find(id);
            if (aspNetUserType == null)
            {
                return HttpNotFound();
            }
            return View(aspNetUserType);
        }

        // GET: SecurityUserType/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: SecurityUserType/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Name")] AspNetUserType aspNetUserType)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    db.AspNetUserTypes.Add(aspNetUserType);
                    db.SaveChanges();


                    Notification(NotificationType.SaveSuccess, true);
                    return RedirectToAction("Index");

                }
                catch (Exception)
                {
                    Notification(NotificationType.SaveError, true);
                    return View(aspNetUserType);
                }

            }

            Notification(NotificationType.SaveError, true);
            return View(aspNetUserType);
        }

        // GET: SecurityUserType/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AspNetUserType aspNetUserType = db.AspNetUserTypes.Find(id);
            if (aspNetUserType == null)
            {
                return HttpNotFound();
            }
            return View(aspNetUserType);
        }

        // POST: SecurityUserType/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Name")] AspNetUserType aspNetUserType)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var oldObj = aspNetUserType;
                    db.Entry(aspNetUserType).State = EntityState.Modified;
                    db.SaveChanges();

                    Notification(NotificationType.UpdateSuccess, true);
                    return RedirectToAction("Index");
                }
                catch (Exception)
                {
                    Notification(NotificationType.UpdateError, true);
                    return View(aspNetUserType);
                }
            }
            Notification(NotificationType.UpdateError, true);
            return View(aspNetUserType);
        }

        // GET: SecurityUserType/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AspNetUserType aspNetUserType = db.AspNetUserTypes.Find(id);
            if (aspNetUserType == null)
            {
                return HttpNotFound();
            }
            return View(aspNetUserType);
        }

        // POST: SecurityUserType/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            AspNetUserType aspNetUserType = db.AspNetUserTypes.Find(id);
            if (aspNetUserType != null)
            {
                try
                {
                    db.AspNetUserTypes.Remove(aspNetUserType);
                    db.SaveChanges();

                    Notification(NotificationType.DeleteSuccess, true);
                    return RedirectToAction("Index");
                }
                catch (Exception)
                {
                    Notification(NotificationType.DeleteError, true);
                    return View(aspNetUserType);
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
