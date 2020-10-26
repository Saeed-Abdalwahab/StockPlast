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
    [CustomAuthorize("صفحات المنظومة")]
    [SessionExpire]
    public class SecurityRolesController : BaseController
    {
        private StockDB.Model.StockModel db = new StockDB.Model.StockModel();

        // GET: SecurityRoles
        public ActionResult Index(int? page, int? secModId, string secModName)
        {
            Session["PageLinkName"] = "أدور وحدات المنظومة";

            var aspNetModules = db.AspNetModules.ToList();
            ViewBag.SecurityModules = aspNetModules;

            var pageSize = 15;
            var pageNumber = page ?? 1;
            var securityModuleId = secModId ?? 12;
            var securityModuleName = secModName ?? "الأكواد الأساسية";

            var result = db.AspNetRoles.Include(a => a.AspNetModule).Where(o => o.AspNetModuleId == securityModuleId).OrderBy(r => r.ArName).ToList();
            ViewBag.SelectedSecurityModuleId = securityModuleId;
            ViewBag.SelectedSecurityModuleName = securityModuleName;
            return View(result.ToPagedList(pageNumber, pageSize));
        }

        // GET: SecurityRoles/Details/5
        public ActionResult Details(string id, int secModId, string secModName)
        {


            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AspNetRole aspNetRole = db.AspNetRoles.Find(id);
            if (aspNetRole == null)
            {
                return HttpNotFound();
            }

            ViewBag.SelectedSecurityModuleId = secModId;
            ViewBag.SelectedSecurityModuleName = secModName;

            var aspNetModules = db.AspNetModules.ToList();
            ViewBag.SecurityModules = aspNetModules;

            return View(aspNetRole);
        }

        // GET: SecurityRoles/Create
        public ActionResult Create(int secModId, string secModName)
        {
            ViewBag.AspNetModuleId = new SelectList(db.AspNetModules, "Id", "Name", secModId);

            ViewBag.SelectedSecurityModuleId = secModId;
            ViewBag.SelectedSecurityModuleName = secModName;

            var aspNetModules = db.AspNetModules.ToList();
            ViewBag.SecurityModules = aspNetModules;

            return View();
        }

        // POST: SecurityRoles/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,ArName,AspNetModuleId")] AspNetRole aspNetRole)
        {
            var secModIdHdn = Convert.ToInt32(Request.Form["hdnSelectedSecurityModuleId"]);
            var secModNameHdn = Request.Form["hdnSelectedSecurityModuleName"];
            var maxRoleList = db.AspNetRoles.Select(e => e.Id).ToList();
            var maxRoleId = maxRoleList.Select(int.Parse).Max();

            aspNetRole.AspNetModuleId = secModIdHdn;
            aspNetRole.Id = (maxRoleId + 1).ToString();

            ViewBag.AspNetModuleId = new SelectList(db.AspNetModules, "Id", "Name", aspNetRole.AspNetModuleId);
            ViewBag.SelectedSecurityModuleId = aspNetRole.AspNetModuleId;
            ViewBag.SelectedSecurityModuleName = Request.Form["hdnSelectedSecurityModuleName"];
            var aspNetModules = db.AspNetModules.ToList();
            ViewBag.SecurityModules = aspNetModules;

            if (ModelState.IsValid)
            {
                try
                {
                    db.AspNetRoles.Add(aspNetRole);
                    db.SaveChanges();
                    Notification(NotificationType.SaveSuccess, true);
                    return RedirectToAction("Index", "SecurityRoles", new { secModId = secModIdHdn, secModName = secModNameHdn });
                }
                catch (Exception)
                {
                    Notification(NotificationType.SaveError, true);
                    return View(aspNetRole);
                }

            }

            Notification(NotificationType.SaveError, true);
            return View(aspNetRole);
        }

        // GET: SecurityRoles/Edit/5
        public ActionResult Edit(string id, int secModId, string secModName)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AspNetRole aspNetRole = db.AspNetRoles.Find(id);
            if (aspNetRole == null)
            {
                return HttpNotFound();
            }

            ViewBag.SelectedSecurityModuleId = secModId;
            ViewBag.SelectedSecurityModuleName = secModName;

            var aspNetModules = db.AspNetModules.ToList();
            ViewBag.SecurityModules = aspNetModules;

            ViewBag.AspNetModuleId = new SelectList(db.AspNetModules, "Id", "Name", aspNetRole.AspNetModuleId);
            return View(aspNetRole);
        }

        // POST: SecurityRoles/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,ArName,AspNetModuleId")] AspNetRole aspNetRole)
        {
            var secModIdHdn = Convert.ToInt32(Request.Form["hdnSelectedSecurityModuleId"]);
            var secModNameHdn = Request.Form["hdnSelectedSecurityModuleName"];
            aspNetRole.AspNetModuleId = secModIdHdn;

            var aspNetModules = db.AspNetModules.ToList();
            ViewBag.AspNetModuleId = new SelectList(db.AspNetModules, "Id", "Name", aspNetRole.AspNetModuleId);
            ViewBag.SelectedSecurityModuleId = aspNetRole.AspNetModuleId;
            ViewBag.SelectedSecurityModuleName = Request.Form["hdnSelectedSecurityModuleName"];
            ViewBag.SecurityModules = aspNetModules;

            if (ModelState.IsValid)
            {
                try
                {
                    var oldObj = aspNetRole;
                    db.Entry(aspNetRole).State = EntityState.Modified;
                    db.SaveChanges();

                    Notification(NotificationType.UpdateSuccess, true);
                    return RedirectToAction("Index", "SecurityRoles", new { secModId = secModIdHdn, secModName = secModNameHdn });
                }
                catch (Exception)
                {
                    Notification(NotificationType.UpdateError, true);
                    return View(aspNetRole);
                }

            }
            Notification(NotificationType.UpdateError, true);
            return View(aspNetRole);
        }

        // GET: SecurityRoles/Delete/5
        public ActionResult Delete(string id, int secModId, string secModName)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AspNetRole aspNetRole = db.AspNetRoles.Find(id);
            if (aspNetRole == null)
            {
                return HttpNotFound();
            }

            ViewBag.SelectedSecurityModuleId = secModId;
            ViewBag.SelectedSecurityModuleName = secModName;

            var aspNetModules = db.AspNetModules.ToList();
            ViewBag.SecurityModules = aspNetModules;

            return View(aspNetRole);
        }

        // POST: SecurityRoles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            var secModIdHdn = Convert.ToInt32(Request.Form["hdnSelectedSecurityModuleId"]);
            var secModNameHdn = Request.Form["hdnSelectedSecurityModuleName"];

            var aspNetRole = db.AspNetRoles.Find(id);
            if (aspNetRole != null)
            {
                try
                {
                    db.AspNetRoles.Remove(aspNetRole);
                    db.SaveChanges();

                    Notification(NotificationType.DeleteSuccess, true);
                    return RedirectToAction("Index", "SecurityRoles", new { secModId = secModIdHdn, secModName = secModNameHdn });
                }
                catch (Exception)
                {
                    Notification(NotificationType.DeleteError, true);
                    return View(aspNetRole);
                }
            }
            Notification(NotificationType.DeleteInformation, true);
            return RedirectToAction("Index", "SecurityRoles", new { secModId = secModIdHdn, secModName = secModNameHdn });

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
