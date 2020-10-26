using System.Linq;
using System.Web.Mvc;
using StockPortal.Helpers;
using StockPortal.Models;

using StockDB;
using System.Data.Entity;
using System.Web;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using PlasticStatic;


namespace StockPortal.Controllers
{
    [HandleError]
    [SessionExpire]
    public class HomeController : BaseController
    {

        private StockDB.Model.StockModel db = new StockDB.Model.StockModel();
        public ApplicationUserManager UserManager
        {
            get
            {
                return HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
        }
        public ApplicationSignInManager SignInManager
        {
            get
            {
                return HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
        }
        public ActionResult Index()
        {

            if ((Request.Cookies["UserId"] != null))
            {
                return View();
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult MyAccount()
        {
            var UserID = Request.Cookies["UserId"].Value;
            var aspNetUserWorker = db.AspNetUserWorkers.Include(a => a.AspNetUser).Include(a => a.AspNetUserType).SingleOrDefault(r => r.UserId == UserID);

            if (aspNetUserWorker != null)
            {
                var obj = new ViewModel.SecurityUserData
                {

                    WorkerId = aspNetUserWorker.WorkerId,
                    UserId = aspNetUserWorker.AspNetUser.Id,
                    WorkerName = Request.Cookies["EmployeeName"].Value,
                    //WorkerName = getEmpName(aspNetUserWorker.WorkerId),
                    UserName = aspNetUserWorker.AspNetUser.UserName,
                    Email = aspNetUserWorker.AspNetUser.Email,
                    AccountStatus = aspNetUserWorker.IsActivte ? "الحساب مفعل" : "الحساب معطل",
                    UserTypeName = aspNetUserWorker.AspNetUserType.Name,

                };
                return View(obj);
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult MyAccount([Bind(Include = "OldPassword,NewPassword,NewPasswordConfirm")] ViewModel.SecurityUserData securityUserData)
        {
            var UserID = Request.Cookies["UserId"].Value;

            var aspNetUserWorker = db.AspNetUserWorkers.Include(a => a.AspNetUser).Include(a => a.AspNetUserType).SingleOrDefault(r => r.UserId == UserID);
            var user = UserManager.FindById(UserID);
            if (user != null && aspNetUserWorker != null)
            {
                var result = UserManager.ChangePassword(User.Identity.GetUserId(), securityUserData.OldPassword, securityUserData.NewPassword);
                if (result.Succeeded)
                {
                    //var securityEventLogic = new SecurityEventDbOperation();


                    return RedirectToAction("LogOff", "Account");
                }


                var obj = new ViewModel.SecurityUserData
                {
                    WorkerId = aspNetUserWorker.WorkerId,
                    UserId = aspNetUserWorker.AspNetUser.Id,
                    //WorkerName = CommonFunctions.GetWorkerName(aspNetUserWorker.WorkerId),
                    UserName = aspNetUserWorker.AspNetUser.UserName,
                    Email = aspNetUserWorker.AspNetUser.Email,
                    //AccountStatus = aspNetUserWorker.IsActivte ? "الحساب مفعل" : "الحساب معطل",
                    UserTypeName = aspNetUserWorker.AspNetUserType.Name,
                    // ManagmentSpecializedName = aspNetUserWorker.ManagmentSpecializedId == null ? "غير محدد" : managmentSpecializedList.Find(t => t.ID == aspNetUserWorker.ManagmentSpecializedId).Name
                };

                NotificationWithMessage(" ادخال كلمه مرور تحتوى على ارقام وحروف ولاتقل عن تسعه", true);
                return View(obj);
            }

            return RedirectToAction("Index");
        }

        #region methods
        public string getEmpName(int empId)
        {

            StockDB.Model.Employe empObj = db.Employes.FirstOrDefault(ee => ee.emp_id == empId);

            return empObj.emp_name;
        }
        #endregion

    }
}