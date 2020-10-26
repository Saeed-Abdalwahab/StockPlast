using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using StockService;
using StockViewModel;

namespace StockPortal.Controllers
{
    public class CustomerPaymentController : Controller
    {
        // GET: CustomerPayment
        private CustomerPaymentService CustomerPaymentService = new CustomerPaymentService();
        public ActionResult Index()
        {
            ViewBag.customers = new SelectList(CustomerPaymentService.Customers(), "cust_id", "cust_name");
            ViewBag.safes = new SelectList(CustomerPaymentService.Safes(), "safe_id", "safe_name");
            ViewBag.MaxSerial = CustomerPaymentService.GetMaxSerial();

            return View();
        }
        public JsonResult Getall()
        {
            return Json(new { data = CustomerPaymentService.GetAll() }, JsonRequestBehavior.AllowGet);

        }
        [HttpGet]
        public ActionResult GetbyID(int id)
        {
            return Json(new { data = CustomerPaymentService.GetByID(id) }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult Save(CustomerPaymentVM customerpaymentVM)
        {
            if (customerpaymentVM.ID == 0)
            {

                return Json(new { msg = CustomerPaymentService.SaveCreate(customerpaymentVM), maxserial = CustomerPaymentService.GetMaxSerial() }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                if (CustomerPaymentService.SafeBlanceCheck((int)customerpaymentVM.Safe_ID, customerpaymentVM.ID)+customerpaymentVM.PaymentValue < 0)
                {
                    return Json(new { msg = "رصيد الخزينه لا يسمح بتعديل هذه القيمه ", maxserial = CustomerPaymentService.GetMaxSerial() }, JsonRequestBehavior.AllowGet);

                }
                return Json(new { msg = CustomerPaymentService.SaveEdit(customerpaymentVM), maxserial = CustomerPaymentService.GetMaxSerial() }, JsonRequestBehavior.AllowGet);

            }
        }

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {



            return Json(new { success = CustomerPaymentService.Delete(id), maxserial = CustomerPaymentService.GetMaxSerial() }, JsonRequestBehavior.AllowGet);


        }
    }
}