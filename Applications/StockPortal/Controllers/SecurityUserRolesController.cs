using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using StockPortal.Models;
using StockDB.Model;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using StockPortal.Helpers;
using PlasticStatic;
using PagedList;

namespace StockPortal.Controllers
{
    [HandleError]
    [CustomAuthorize("أدوار المستخدمين")]
    [SessionExpire]
    public class SecurityUserRolesController : BaseController
    {
        private StockDB.Model.StockModel db = new StockDB.Model.StockModel();
        public ApplicationUserManager UserManager
        {
            get
            {
                return HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
        }

        [HttpGet]
        public ActionResult Index(int? page,string UserID="")
        {
            var UW_U_E = (from UW in db.AspNetUserWorkers
                     join u in db.AspNetUsers on UW.UserId equals u.Id
                     join e in db.Employes on UW.WorkerId equals e.emp_id
                     select new {
                         Id=u.Id,
                         emp_name=e.emp_name
                     }).ToList();
            ViewBag.txtEmployeeSearch = new SelectList(UW_U_E, "Id", "emp_name");


            Session["PageLinkName"] = "المستخدمين";

            var pageSize = 15;
            var pageNumber = page ?? 1;

            var result = new List<ViewModel.SecurityUserData>();
            var superAdminId = int.Parse(Enums.EnumString.GetStringValue(Enums.AspNetUserType.SuperAdmin));
            var adminId = int.Parse(Enums.EnumString.GetStringValue(Enums.AspNetUserType.Admin));

            List<AspNetUserWorker> aspNetUserWorkers;
            if (UserID != "")
            {
                if (Request.Cookies["UserTypeId"].Value == Enums.EnumString.GetStringValue(Enums.AspNetUserType.SuperAdmin))
                {
                    aspNetUserWorkers = db.AspNetUserWorkers.Where(a=>a.UserId==UserID).ToList();
                }
                else if (Request.Cookies["UserTypeId"].Value == Enums.EnumString.GetStringValue(Enums.AspNetUserType.Admin))
                {
                    aspNetUserWorkers = db.AspNetUserWorkers.Where(e => e.AspNetUserType.ID != superAdminId).Where(a => a.UserId == UserID).ToList();
                }
                else
                {
                    aspNetUserWorkers = db.AspNetUserWorkers.Where(e => e.AspNetUserType.ID != superAdminId && e.AspNetUserType.ID != adminId).Where(a => a.UserId == UserID).ToList();

                }
            }
            else { 
                 
                if (Request.Cookies["UserTypeId"].Value == Enums.EnumString.GetStringValue(Enums.AspNetUserType.SuperAdmin))
                {
                    aspNetUserWorkers = db.AspNetUserWorkers.ToList();
                }
                else if (Request.Cookies["UserTypeId"].Value == Enums.EnumString.GetStringValue(Enums.AspNetUserType.Admin))
                {
                    aspNetUserWorkers = db.AspNetUserWorkers.Where(e => e.AspNetUserType.ID != superAdminId).ToList();
                }
                else
                {
                    aspNetUserWorkers = db.AspNetUserWorkers.Where(e => e.AspNetUserType.ID != superAdminId && e.AspNetUserType.ID != adminId).ToList();

                }
            }
           



            foreach (var item in aspNetUserWorkers)
            {
                var worker = db.Employes.Where(a => a.emp_id == item.WorkerId).FirstOrDefault();
                if (worker != null)
                {
                    var obj = new ViewModel.SecurityUserData()
                    {
                        UserId = item.AspNetUser.Id,
                        WorkerName = worker.emp_name,
                        WorkerId = worker.emp_id,
                        //WorkerName = CommonFunctions.GetWorkerName(item.WorkerId),
                        UserName = item.AspNetUser.UserName,
                        Email = item.AspNetUser.Email,
                        AccountStatus = item.IsActivte ? "الحساب مفعل" : "الحساب معطل",
                        UserTypeName = item.AspNetUserType.Name,
                        //ManagmentSpecializedName = item.ManagmentSpecializedId == null ? "غير محدد" : managmentSpecializedList.Find(t => t.ID == item.ManagmentSpecializedId).Name
                    };

                    result.Add(obj);
                }
            }

            var orderedResult = result.ToList();// .OrderBy(r => r.ManagmentSpecializedName).ToList();

            return View(orderedResult.ToPagedList(pageNumber, pageSize));

          

        }


        public ActionResult GetUserRoles(string id)
        {
            Session["PageLinkName"] = "أدوار المستخدمين";

            ViewBag.UserId = id;
            //var managmentSpecializedList = CommonFunctions.GetAllTransportCenters();

            var user = UserManager.FindById(id);
            if (user != null)
            {
                var aspNetUserWorker = db.AspNetUserWorkers.Include(a => a.AspNetUser).Include(a => a.AspNetUserType).SingleOrDefault(r => r.UserId == user.Id);
                if (aspNetUserWorker == null)
                {
                    Notification(NotificationType.SearchInformation, true);
                    return RedirectToAction("Index");
                }

                var obj = new ViewModel.SecurityUserData
                {
                    UserId = aspNetUserWorker.AspNetUser.Id,
                    // WorkerName = CommonFunctions.GetWorkerName(aspNetUserWorker.WorkerId),
                    UserName = aspNetUserWorker.AspNetUser.UserName,
                    Email = aspNetUserWorker.AspNetUser.Email,
                    AccountStatus = aspNetUserWorker.IsActivte ? "الحساب مفعل" : "الحساب معطل",
                    UserTypeName = aspNetUserWorker.AspNetUserType.Name,
                    //ManagmentSpecializedName = aspNetUserWorker.ManagmentSpecializedId == null ? "غير محدد" : managmentSpecializedList.Find(t => t.ID == aspNetUserWorker.ManagmentSpecializedId).Name
                };


                //GetRolesHierarchy(id);


                return View(obj);
            }

            Notification(NotificationType.SearchInformation, true);
            return RedirectToAction("Index");


        }


        public JsonResult GetRolesHierarchy(string id)
        {
            var loginedUserRoles = Request.Cookies["UserTypeId"].Value == Enums.EnumString.GetStringValue(Enums.AspNetUserType.SuperAdmin) ?
                db.AspNetRoles.OrderBy(r => r.ArName).Select(e => e.Name).OrderBy(e => e.ToString()).ToList() : UserManager.GetRoles(Sessions.UserId).OrderBy(r => r.ToString()).ToList();

            var userRoles = UserManager.GetRoles(id);
            var selectedModules = db.AspNetRoles.Where(e => loginedUserRoles.Contains(e.Name)).Select(e => e.AspNetModuleId).ToArray();
            var modules = db.AspNetModules.Where(e => selectedModules.Contains(e.Id)).ToList();
            ViewBag.modules = modules;

            return GetHierarchy(modules, userRoles.ToArray(), loginedUserRoles.ToArray());
        }

        private JsonResult GetHierarchy(List<AspNetModule> modules, string[] userRoles, string[] loginedUserRoles)
        {
            var roles = db.AspNetRoles.Where(e => loginedUserRoles.Contains(e.Name)).ToList();

            var records = modules.Select(l => new HierarchyViewModel
            {
                Id = l.Id.ToString() + "Module",
                text = l.Name,
                ischecked = false,
                children = GetChildren(roles, l.Id, userRoles)
            }).ToList();

            return Json(records, JsonRequestBehavior.AllowGet);
        }

        private List<HierarchyViewModel> GetChildren(List<AspNetRole> rolesList, int aspNetModuleId, string[] userRoles)
        {
            return rolesList.Where(l => l.AspNetModuleId == aspNetModuleId)
                .Select(l => new HierarchyViewModel
                {
                    Id = l.Id + "Role",
                    text = l.ArName,
                    ischecked = userRoles.Contains(l.Name),
                    perentId = aspNetModuleId
                }).ToList();
        }

        [HttpPost, ActionName("GetUserRoles")]
        [ValidateAntiForgeryToken]
        public ActionResult SetUserRoles(string id)
        {
            ViewBag.UserId = id;
            //var managmentSpecializedList = CommonFunctions.GetAllTransportCenters();
            var user = UserManager.FindById(id);
            if (user != null)
            {
                var aspNetUserWorker = db.AspNetUserWorkers.Include(a => a.AspNetUser).Include(a => a.AspNetUserType).SingleOrDefault(r => r.UserId == user.Id);
                if (aspNetUserWorker == null)
                {
                    Notification(NotificationType.SaveInformation, true);
                    return RedirectToAction("Index");
                }

                var obj = new ViewModel.SecurityUserData
                {
                    UserId = aspNetUserWorker.AspNetUser.Id,
                    //WorkerName = CommonFunctions.GetWorkerName(aspNetUserWorker.WorkerId),
                    UserName = aspNetUserWorker.AspNetUser.UserName,
                    Email = aspNetUserWorker.AspNetUser.Email,
                    AccountStatus = aspNetUserWorker.IsActivte ? "الحساب مفعل" : "الحساب معطل",
                    UserTypeName = aspNetUserWorker.AspNetUserType.Name,
                    //ManagmentSpecializedName = aspNetUserWorker.ManagmentSpecializedId == null ? "غير محدد" : managmentSpecializedList.Find(t => t.ID == aspNetUserWorker.ManagmentSpecializedId).Name
                };

                var roles = db.AspNetRoles.ToList();
                var userRoles = UserManager.GetRoles(id).ToArray();

                var selectedRoles = Request.Form["hdnCheckedRoles"];
                var selectedRolesWithoutRole = selectedRoles.Replace("Role", "");

                var selectedRolesArr = selectedRolesWithoutRole.Split(',').Where(r => !r.Contains("Module")).ToArray();
                var userRolesIds = roles.Where(r => userRoles.Contains(r.Name)).Select(r => r.Id).ToArray();

                var addUserRoles = selectedRolesArr.Except(userRolesIds).ToArray();
                var deleteUserRoles = userRolesIds.Except(selectedRolesArr).ToArray();

                try
                {
                    if (addUserRoles.Length > 0)
                    {
                        UserManager.AddToRoles(id, roles.Where(r => addUserRoles.Contains(r.Id)).Select(r => r.Name).ToArray());
                    }

                    if (deleteUserRoles.Length > 0)
                    {
                        UserManager.RemoveFromRoles(id, roles.Where(r => deleteUserRoles.Contains(r.Id)).Select(r => r.Name).ToArray());
                    }

                    Notification(NotificationType.SaveSuccess, true);
                    return View(obj);
                }
                catch (Exception)
                {
                    Notification(NotificationType.SaveError, true);

                    return View(obj);
                }

            }

            Notification(NotificationType.SaveInformation, true);
            return RedirectToAction("Index");
        }
    }
}