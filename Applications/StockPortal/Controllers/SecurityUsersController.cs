using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using PagedList;
using StockPortal.Models;
using StockDB.Model;
using PlasticStatic;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using StockPortal.Helpers;
using StockPortal.ViewModel;
using Microsoft.AspNet.WebHooks.Utilities;
using System.Security.Cryptography;

namespace StockPortal.Controllers
{
    [HandleError]
    [CustomAuthorize("المستخدمين")]
    [SessionExpire]
    public class SecurityUsersController : BaseController
    {
        private StockDB.Model.StockModel db = new StockDB.Model.StockModel();
        public ApplicationUserManager UserManager
        {
            get
            {
                return HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
        }
        // GET: SecurityUsers
        public ActionResult Index(int? page)

        {
            Session["PageLinkName"] = "المستخدمين";

            var pageSize = 15;
            var pageNumber = page ?? 1;

            var result = new List<ViewModel.SecurityUserData>();
            var superAdminId = int.Parse(Enums.EnumString.GetStringValue(Enums.AspNetUserType.SuperAdmin));
            var adminId = int.Parse(Enums.EnumString.GetStringValue(Enums.AspNetUserType.Admin));


            List<AspNetUserWorker> aspNetUserWorkers;
           

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
                aspNetUserWorkers = db.AspNetUserWorkers.Where(e => e.AspNetUserType.ID != superAdminId && e.AspNetUserType.ID!= adminId).ToList();

            }



            foreach (var item in aspNetUserWorkers)
            {
                var worker = db.Employes.Where(a => a.emp_id == item.WorkerId).FirstOrDefault();
                if (worker != null)
                {
                    var obj = new ViewModel.SecurityUserData
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


        // GET: SecurityUsers/Details/5
        public ActionResult Details(string id)
        {
            //var managmentSpecializedList = CommonFunctions.GetAllTransportCenters();
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var aspNetUserWorker = db.AspNetUserWorkers.Include(a => a.AspNetUser).Include(a => a.AspNetUserType).SingleOrDefault(r => r.UserId == id);
            if (aspNetUserWorker == null)
            {
                return HttpNotFound();
            }

            var obj = new ViewModel.SecurityUserData
            {
                UserId = aspNetUserWorker.AspNetUser.Id,
                //WorkerName = CommonFunctions.GetWorkerName(aspNetUserWorker.WorkerId),
                UserName = aspNetUserWorker.AspNetUser.UserName,
                Email = aspNetUserWorker.AspNetUser.Email,
                AccountStatus = aspNetUserWorker.IsActivte ? "الحساب مفعل" : "الحساب معطل",
                UserTypeName = aspNetUserWorker.AspNetUserType.Name,
                ///ManagmentSpecializedName = aspNetUserWorker.ManagmentSpecializedId == null ? "غير محدد" : managmentSpecializedList.Find(t => t.ID == aspNetUserWorker.ManagmentSpecializedId).Name
            };

            return View(obj);
        }

        // GET: SecurityUsers/Create
        public ActionResult Create()
        {
            ViewBag.txtEmployeeSearch = new SelectList(db.Employes.ToList(), "emp_id", "emp_name");

            var superAdminId = int.Parse(Enums.EnumString.GetStringValue(Enums.AspNetUserType.SuperAdmin));

            var adminId = int.Parse(Enums.EnumString.GetStringValue(Enums.AspNetUserType.Admin));

            if (Sessions.UserTypeId == int.Parse(Enums.EnumString.GetStringValue(Enums.AspNetUserType.SuperAdmin)))
            {
                ViewBag.UserTypeId = new SelectList(db.AspNetUserTypes.ToList(), "ID", "Name");
            }
            else if (Sessions.UserTypeId == int.Parse(Enums.EnumString.GetStringValue(Enums.AspNetUserType.Admin)))
            {
                ViewBag.UserTypeId = new SelectList(db.AspNetUserTypes.Where(a=>a.ID!= superAdminId).ToList(), "ID", "Name");
            }
            else
            {
                var newList = db.AspNetUserTypes.Where(e => e.ID != superAdminId && e.ID != adminId).ToList();
                ViewBag.UserTypeId = new SelectList(newList, "ID", "Name");
            }


            return View();
        }

        // POST: SecurityUsers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(/*[Bind(Include = "UserTypeId,IsActivte")] */ AspNetUserWorker aspNetUserWorker)
        {
            ViewBag.txtEmployeeSearch = new SelectList(db.Employes.ToList(), "emp_id", "emp_name");

            //From Table Employee

            var superAdminId = int.Parse(Enums.EnumString.GetStringValue(Enums.AspNetUserType.SuperAdmin));

            var adminId = int.Parse(Enums.EnumString.GetStringValue(Enums.AspNetUserType.Admin));


         
            var User_Id = db.AspNetUsers.Where(a => a.Id == aspNetUserWorker.UserId || a.Email == aspNetUserWorker.AspNetUser.Email).FirstOrDefault();
            var Emp_Found= db.Employes.Where(a => a.emp_id == aspNetUserWorker.WorkerId).FirstOrDefault();

            var User_Emp_Found = db.AspNetUserWorkers.Any( a=>a.WorkerId == Emp_Found.emp_id);

            Employe Emp = db.Employes.Where(a => a.emp_id == aspNetUserWorker.WorkerId).FirstOrDefault();
            AspNetUser userObj = new AspNetUser();
            AspNetUserWorker UserWorker = new AspNetUserWorker();
            if (ModelState.IsValid)
            {

                if (User_Emp_Found == false)
                {


                    Guid id = Guid.NewGuid();
                    userObj.Id = id.ToString();
                    userObj.UserName = aspNetUserWorker.AspNetUser.UserName;
                    userObj.Email = aspNetUserWorker.AspNetUser.Email;
                    userObj.PhoneNumber = aspNetUserWorker.AspNetUser.PhoneNumber;

                    userObj.PasswordHash = HashPassword((123456).ToString());
                        //HashPassword(aspNetUserWorker.AspNetUser.PasswordHash);
                    userObj.SecurityStamp = Guid.NewGuid().ToString();

                    userObj.EmailConfirmed = false;
                    userObj.PhoneNumberConfirmed = false;
                    userObj.TwoFactorEnabled = false;
                    userObj.LockoutEnabled = true;
                    userObj.AccessFailedCount = 0;

                    db.AspNetUsers.Add(userObj);
                    db.SaveChanges();


                    UserWorker.UserId = userObj.Id;
                    UserWorker.IsActivte = aspNetUserWorker.IsActivte;
                    UserWorker.WorkerId = Emp.emp_id;
                    UserWorker.UserTypeId = aspNetUserWorker.UserTypeId;
                    db.AspNetUserWorkers.Add(UserWorker);
                    db.SaveChanges();



                }
                else {
                    if (Sessions.UserTypeId == int.Parse(Enums.EnumString.GetStringValue(Enums.AspNetUserType.SuperAdmin)))
                    {
                        ViewBag.UserTypeId = new SelectList(db.AspNetUserTypes.ToList(), "ID", "Name", aspNetUserWorker.UserTypeId);
                    }
                    else if (Sessions.UserTypeId == int.Parse(Enums.EnumString.GetStringValue(Enums.AspNetUserType.Admin)))
                    {
                        ViewBag.UserTypeId = new SelectList(db.AspNetUserTypes.Where(a => a.ID != superAdminId).ToList(), "ID", "Name", aspNetUserWorker.UserTypeId);
                    }
                    else
                    {
                        var newList = db.AspNetUserTypes.Where(e => e.ID != superAdminId && e.ID != adminId).ToList();
                        ViewBag.UserTypeId = new SelectList(newList, "ID", "Name");
                    }
                    Notification(NotificationType.SaveError, true);
                    return View(aspNetUserWorker);
                }


            var workername = userObj.UserName;
            var workerId = Emp.emp_id;
            var workerUsername = Emp.emp_name;
            var workerPassword = aspNetUserWorker.AspNetUser.PasswordHash;
            var workerEmail = Emp.emp_mail;

            ViewBag.Workername = workername;
            ViewBag.WorkerId = workerId;
            ViewBag.WorkerUsername = workerUsername;
            ViewBag.WorkerEmail = workerEmail;

            aspNetUserWorker.WorkerId = workerId;

            if (Sessions.UserTypeId == int.Parse(Enums.EnumString.GetStringValue(Enums.AspNetUserType.SuperAdmin)))
            {
                ViewBag.UserTypeId = new SelectList(db.AspNetUserTypes.ToList(), "ID", "Name", aspNetUserWorker.UserTypeId);
            }
            else if (Sessions.UserTypeId == int.Parse(Enums.EnumString.GetStringValue(Enums.AspNetUserType.Admin)))
            {
                ViewBag.UserTypeId = new SelectList(db.AspNetUserTypes.Where(a => a.ID != superAdminId).ToList(), "ID", "Name", aspNetUserWorker.UserTypeId);
            }
            else
            {
                var newList = db.AspNetUserTypes.Where(e => e.ID != superAdminId && e.ID != adminId).ToList();
                ViewBag.UserTypeId = new SelectList(newList, "ID", "Name");
            }
                Notification(NotificationType.SaveSuccess, true);
                return RedirectToAction("Index");
            }
            else
            {

                Notification(NotificationType.SaveError, true);
                return View(aspNetUserWorker);
            }


        }

        // GET: SecurityUsers/Edit/5
        public ActionResult Edit(string id)
        {
            var superAdminId = int.Parse(Enums.EnumString.GetStringValue(Enums.AspNetUserType.SuperAdmin));

            var adminId = int.Parse(Enums.EnumString.GetStringValue(Enums.AspNetUserType.Admin));

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var aspNetUserWorker = db.AspNetUserWorkers.Include(a => a.AspNetUser).Include(a => a.AspNetUserType).SingleOrDefault(r => r.UserId == id);

            if (aspNetUserWorker == null)
            {
                return HttpNotFound();
            }

            //var obj = new ViewModel.SecurityUserData
            //{
            //    UserId = aspNetUserWorker.AspNetUser.Id,
            //    WorkerName = GetWorkerName(aspNetUserWorker.WorkerId),
            //    UserName = aspNetUserWorker.AspNetUser.UserName,
            //    Email = aspNetUserWorker.AspNetUser.Email,
            //    AccountStatus = aspNetUserWorker.IsActivte ? "الحساب مفعل" : "الحساب معطل",
            //    UserTypeName = aspNetUserWorker.AspNetUserType.Name,
            //    ManagmentSpecializedName = aspNetUserWorker.ManagmentSpecializedId == null ? "غير محدد" : managmentSpecializedList.Find(t => t.ID == aspNetUserWorker.ManagmentSpecializedId).Name
            //};

            var WorkerName = db.Employes.Where(a=>a.emp_id==aspNetUserWorker.WorkerId).FirstOrDefault();
            ViewBag.WorkerName = WorkerName.emp_name;

            //CommonFunctions.GetWorkerName(aspNetUserWorker.WorkerId);
            //ViewBag.ManagmentSpecializedName = aspNetUserWorker.ManagmentSpecializedId == null ? "غير محدد" : managmentSpecializedList.Find(t => t.ID == aspNetUserWorker.ManagmentSpecializedId).Name;


            if (Sessions.UserTypeId == int.Parse(Enums.EnumString.GetStringValue(Enums.AspNetUserType.SuperAdmin)))
            {
                ViewBag.UserTypeId = new SelectList(db.AspNetUserTypes, "ID", "Name");
            }

            else if (Sessions.UserTypeId == int.Parse(Enums.EnumString.GetStringValue(Enums.AspNetUserType.SuperAdmin)))
            {
                var newList = db.AspNetUserTypes.Where(e => e.ID != superAdminId).ToList();
                ViewBag.UserTypeId = new SelectList(newList, "ID", "Name");
            }
            else {
                var newList = db.AspNetUserTypes.Where(e => e.ID != superAdminId&&e.ID!=adminId).ToList();
                ViewBag.UserTypeId = new SelectList(newList, "ID", "Name");
            }
            return View(aspNetUserWorker);
        }

        // POST: SecurityUsers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(string id, [Bind(Include = "UserTypeId,IsActivte,ManagmentSpecializedId,CompanyId,WorkerId")] AspNetUserWorker aspNetUserWorker)
        {
            var superAdminId = int.Parse(Enums.EnumString.GetStringValue(Enums.AspNetUserType.SuperAdmin));

            var adminId = int.Parse(Enums.EnumString.GetStringValue(Enums.AspNetUserType.Admin));

            var workerEmail = Request.Form["AspNetUser.Email"];
            aspNetUserWorker.UserId = id;
            if (Sessions.UserTypeId == int.Parse(Enums.EnumString.GetStringValue(Enums.AspNetUserType.SuperAdmin)))
            {
                ViewBag.UserTypeId = new SelectList(db.AspNetUserTypes, "ID", "Name", aspNetUserWorker.UserTypeId);
            }
            else if (Sessions.UserTypeId == int.Parse(Enums.EnumString.GetStringValue(Enums.AspNetUserType.Admin)))
            {
                ViewBag.UserTypeId = new SelectList(db.AspNetUserTypes.Where(a => a.ID != superAdminId), "ID", "Name", aspNetUserWorker.UserTypeId);
            }
            else
            {
                var newList = db.AspNetUserTypes.Where(e => e.ID != superAdminId && e.ID != adminId).ToList();
                ViewBag.UserTypeId = new SelectList(newList, "ID", "Name", aspNetUserWorker.UserTypeId);
            }


            if (ModelState.IsValid)
            {
                try
                {
                    var oldaspNetUserWorker = db.AspNetUserWorkers.AsNoTracking().Where(r => r.UserId == aspNetUserWorker.UserId && r.WorkerId == aspNetUserWorker.WorkerId).FirstOrDefault();


                    if (aspNetUserWorker.UserTypeId == 0) {

                        aspNetUserWorker.UserTypeId = oldaspNetUserWorker.UserTypeId;
                    }

                    db.Entry(aspNetUserWorker).State = EntityState.Modified;
                    db.SaveChanges();

                    var user = UserManager.FindById(id);
                    var olduser = user;
                    if (user != null && workerEmail != user.Email)
                    {



                        user.Email = workerEmail;
                        UserManager.Update(user);
                    }


                    Notification(NotificationType.UpdateSuccess, true);
                    return RedirectToAction("Index");
                }
                catch (Exception)
                {
                    Notification(NotificationType.UpdateError, true);
                    return View(aspNetUserWorker);
                }


            }


            Notification(NotificationType.UpdateInformation, true);
            return View(aspNetUserWorker);
        }

        // GET: SecurityUsers/Delete/5
        public ActionResult Delete(string id)
        {

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var aspNetUserWorker = db.AspNetUserWorkers.Include(a => a.AspNetUser).Include(a => a.AspNetUserType).SingleOrDefault(r => r.UserId == id);
            if (aspNetUserWorker == null)
            {
                return HttpNotFound();
                //throw new HttpException(404, "Page Not Found");
            }

            //var obj = new AspNetUserWorker
            //{
            //    UserId = aspNetUserWorker.AspNetUser.Id,
            //    // WorkerName = CommonFunctions.GetWorkerName(aspNetUserWorker.WorkerId),
            //     = aspNetUserWorker.AspNetUser.UserName,
            //    Email = aspNetUserWorker.AspNetUser.Email,
            //    AccountStatus = aspNetUserWorker.IsActivte ? "الحساب مفعل" : "الحساب معطل",
            //    UserTypeName = aspNetUserWorker.AspNetUserType.Name,
            //    //ManagmentSpecializedName = aspNetUserWorker.ManagmentSpecializedId == null ? "غير محدد" : managmentSpecializedList.Find(t => t.ID == aspNetUserWorker.ManagmentSpecializedId).Name
            //};

            return View(aspNetUserWorker);
        }

        // POST: SecurityUsers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)

        {
            // var managmentSpecializedList = CommonFunctions.GetAllTransportCenters();

            var aspNetUserWorker = db.AspNetUserWorkers.Include(a => a.AspNetUser).Where(r => r.UserId == id).FirstOrDefault();
            var aspNetUser = db.AspNetUsers.Find(id);
            if (aspNetUser != null)
            {
                try
                {

                    db.AspNetUsers.Remove(aspNetUser);
                    db.SaveChanges();



                    //var allRoles = db.AspNetRoles.ToList();
                    //var roles = UserManager.GetRoles(id);
                    //var userRolesIds = allRoles.Where(r => roles.ToArray().Contains(r.Name)).Select(r => r.Id).ToArray();

                    //UserManager.RemoveFromRoles(id, roles.ToArray());

                    //var user = UserManager.FindById(id);
                    //UserManager.Delete(user);

                    //db.AspNetUserWorkers.Remove(aspNetUserWorker);

                    //db.SaveChanges();




                    Notification(NotificationType.DeleteSuccess, true);
                    return RedirectToAction("Index");
                }
                catch (Exception)
                {
                    //var obj = new ViewModel.SecurityUserData
                    //{
                    //    UserId = aspNetUserWorker.AspNetUser.Id,
                    //    //WorkerName = CommonFunctions.GetWorkerName(aspNetUserWorker.WorkerId),
                    //    UserName = aspNetUserWorker.AspNetUser.UserName,
                    //    Email = aspNetUserWorker.AspNetUser.Email,
                    //    AccountStatus = aspNetUserWorker.IsActivte ? "الحساب مفعل" : "الحساب معطل",
                    //    UserTypeName = aspNetUserWorker.AspNetUserType.Name,
                    //    //ManagmentSpecializedName = aspNetUserWorker.ManagmentSpecializedId == null ? "غير محدد" : managmentSpecializedList.Find(t => t.ID == aspNetUserWorker.ManagmentSpecializedId).Name
                    //};

                    Notification(NotificationType.DeleteError, true);
                    return View(aspNetUser);
                }
            }

            Notification(NotificationType.DeleteInformation, true);
            return RedirectToAction("Index");

        }
        // GET: SecurityUsers/Reset/5
        public ActionResult Reset(string id)
        {

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var aspNetUserWorker = db.AspNetUserWorkers.Include(a => a.AspNetUser).Include(a => a.AspNetUserType).SingleOrDefault(r => r.UserId == id);
            if (aspNetUserWorker == null)
            {
                return HttpNotFound();
            }
            //var obj= aspNetUserWorker;
            var obj = new ViewModel.objPass
            {
                UserId = aspNetUserWorker.AspNetUser.Id,
                OldPassword="",
                NewPassword ="",
                NewPasswordConfirm=""

                //WorkerName = CommonFunctions.GetWorkerName(aspNetUserWorker.WorkerId),
                //UserName = aspNetUserWorker.AspNetUser.UserName,
                //Email = aspNetUserWorker.AspNetUser.Email,
                //AccountStatus = aspNetUserWorker.IsActivte ? "الحساب مفعل" : "الحساب معطل",
                //UserTypeName = aspNetUserWorker.AspNetUserType.Name
                
                //ManagmentSpecializedName = aspNetUserWorker.ManagmentSpecializedId == null ? "غير محدد" : managmentSpecializedList.Find(t => t.ID == aspNetUserWorker.ManagmentSpecializedId).Name
            };

            return View(obj);
        }

        [HttpPost, ActionName("Reset")]
        [ValidateAntiForgeryToken]
        public ActionResult ResetConfirmed(string id, ViewModel.objPass _obj)
        {


            var aspNetUserWorker = db.AspNetUserWorkers.Include(a => a.AspNetUser).Include(a => a.AspNetUserType).SingleOrDefault(r => r.UserId == id);

            if (aspNetUserWorker != null)
            {
                try
                {
                    var user = UserManager.FindById(id);
                    //aspNetUserWorker.AspNetUser.PasswordHash = HashPassword(_obj.NewPassword);
                    //var result = UserManager.ChangePassword(aspNetUserWorker.UserId, _obj.OldPassword, _obj.NewPassword);
                    //if (!result.Succeeded)
                    //{
                    //    return View(aspNetUserWorker);
                    //}
                    aspNetUserWorker.AspNetUser.PasswordHash = HashPassword((123456).ToString());
                    db.Entry(aspNetUserWorker).State = EntityState.Modified;
                    db.SaveChanges();
                    //var newPassword = "a123456A@";
                    //UserManager.RemovePassword(id);
                    //UserManager.AddPassword(id, newPassword);
                    Notification(NotificationType.ResetSuccess, true);
                    return RedirectToAction("Index");
                }
                catch (Exception)
                {
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

                    Notification(NotificationType.ResetError, true);
                    return View(obj);
                }
            }


            Notification(NotificationType.ResetError, true);
            return RedirectToAction("Index");

        }

        public static string HashPassword(string password)
        {
            byte[] salt;
            byte[] buffer2;
            if (password == null)
            {
                throw new ArgumentNullException("password");
            }
            using (Rfc2898DeriveBytes bytes = new Rfc2898DeriveBytes(password, 0x10, 0x3e8))
            {
                salt = bytes.Salt;
                buffer2 = bytes.GetBytes(0x20);
            }
            byte[] dst = new byte[0x31];
            Buffer.BlockCopy(salt, 0, dst, 1, 0x10);
            Buffer.BlockCopy(buffer2, 0, dst, 0x11, 0x20);
            return Convert.ToBase64String(dst);
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
