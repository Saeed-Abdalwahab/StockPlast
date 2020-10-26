using System.Web.Mvc;
using StockDB.Model;
using StockPortal.Helpers;
using StockPortal.Models;
using StockService;
using StockViewModel;
namespace StockPortal.Controllers
{
    [HandleError]
    [CustomAuthorize("الوظائف")]
    [SessionExpire]
    public class JobController : BaseController
    {
        private StockModel db = new StockModel();
        private JobService jobService = new JobService();

        // GET: Materialtype
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public JsonResult GetJobs()
        {
            var all = jobService.GettAll();
            return Json(new { data = all }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetJobbyid(int id)
        {

            var job = jobService.GetByID(id);
            return Json(new { data = job }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Save(JobVM jobvm)
        {

            return Json(jobService.SaveinDatabase(jobvm), JsonRequestBehavior.AllowGet);

        }


        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                if (id == 2 || id == 4 || id == 3)
                {
                    return Json(new { success = false }, JsonRequestBehavior.AllowGet);
                }
                Job job = db.Jobs.Find(id);
                db.Jobs.Remove(job);
                db.SaveChanges();
                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
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