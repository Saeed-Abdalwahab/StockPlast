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
    [CustomAuthorize("التخصص الوظيفي")]
    [SessionExpire]
    public class JobDescriptionController : BaseController
    {
        private JobDescriptionservice JDS = new JobDescriptionservice();
        // GET: JobDescription
        public ActionResult Index()
        {
            ViewBag.Jobs = new SelectList(JDS.Jobes(), "job_id", "job_name");
            return View();
        }
        [HttpGet]
        public JsonResult Getall()
        {
            var all = JDS.Getall();
            return Json(new { data = all }, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult Save(JobDescriptionVM jobDescriptionVM)
        {

            return Json(new { msg = JDS.SaveinDatabase(jobDescriptionVM) }, JsonRequestBehavior.AllowGet);

        }

        [HttpGet]
        public JsonResult Getbyid(int id)
        {


            return Json(new { data = JDS.GetByID(id) }, JsonRequestBehavior.AllowGet);
        }
     

        [HttpPost]
        public JsonResult Delete(int id)
        {
            if (id == 1 || id == 2)
            {
                return Json(new { success = false }, JsonRequestBehavior.AllowGet);

            }
            return Json(new { success = JDS.Delete(id) }, JsonRequestBehavior.AllowGet);

        }
        [HttpGet]
        public JsonResult  FetchJobDesByJobID(int id)
        {
            var list = JDS.GetJobDescriptionByJobID(id);
            return Json(new { data = list }, JsonRequestBehavior.AllowGet);
        }
    }
}