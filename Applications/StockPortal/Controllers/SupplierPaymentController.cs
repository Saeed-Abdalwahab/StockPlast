using System.Web.Mvc;
using StockService;
using StockViewModel;

namespace StockPortal.Controllers
{
    public class SupplierPaymentController : Controller
    {
        // GET: SupplierPayment
        private SupplierPaymentsService SupplierPaymentService = new SupplierPaymentsService();
        public ActionResult Index()
        {
            ViewBag.customers = new SelectList(SupplierPaymentService.Suppliers(), "sup_id", "sup_name");
            ViewBag.safes = new SelectList(SupplierPaymentService.Safes(), "safe_id", "safe_name");

            ViewBag.MaxSerial = SupplierPaymentService.GetMaxSerial();

            return View();
        }
        public JsonResult Getall()
        {
            return Json(new { data = SupplierPaymentService.GetAll() }, JsonRequestBehavior.AllowGet);

        }
        [HttpGet]
        public ActionResult GetbyID(int id)
        {
            return Json(new { data = SupplierPaymentService.GetByID(id) }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult Save(SupplierPaymentVM SupplierPaymentVM)
        {
            if (SupplierPaymentVM.ID == 0)
            {
                if (SupplierPaymentVM.PaymentValue > SupplierPaymentService.GetSafeBlance(SupplierPaymentVM.Safe_ID))
                {
                    return Json(new {  msg = "لا يوجد رصيد كافي لاتمام العملية", maxserial = SupplierPaymentService.GetMaxSerial() }, JsonRequestBehavior.AllowGet);

                }
                return Json(new { msg = SupplierPaymentService.SaveCreate(SupplierPaymentVM), maxserial = SupplierPaymentService.GetMaxSerial() }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                if (SupplierPaymentVM.PaymentValue > SupplierPaymentService.GetSafeBlance(SupplierPaymentVM.Safe_ID, SupplierPaymentVM.ID))
                {
                    return Json(new { msg = "لا يوجد رصيد كافي لاتمام العملية", maxserial = SupplierPaymentService.GetMaxSerial() }, JsonRequestBehavior.AllowGet);

                }
                return Json(new { msg = SupplierPaymentService.SaveEdit(SupplierPaymentVM), maxserial = SupplierPaymentService.GetMaxSerial() }, JsonRequestBehavior.AllowGet);

            }
        }

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {



            return Json(new { success = SupplierPaymentService.Delete(id), maxserial = SupplierPaymentService.GetMaxSerial() }, JsonRequestBehavior.AllowGet);


        }
    }
}