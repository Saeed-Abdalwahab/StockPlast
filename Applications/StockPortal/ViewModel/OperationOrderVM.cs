using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace StockPortal.ViewModel
{
    public class OperationOrderVM
    {
        public int ID { get; set; }
        [DisplayName("الفني")]
        [Required(ErrorMessage = "* مطلوب")]
        public int EmployeeId { get; set; }
        [DisplayName("رقم امر التشغيل")]
        public string OperationNumber { get; set; }
        [Required(ErrorMessage = "* مطلوب")]
        [DisplayName("تاريخ امر التشغيل")]
        public DateTime OperationDate { get; set; }
        [Required(ErrorMessage = "* مطلوب")]
        [DisplayName("امر الشغل")]
        public int DemondOrderDetailsId { get; set; }
        public string DemondOrderDetailsIdString { get; set; }
        public IEnumerable<string> demondOrderDetailsIdList { get; set; }
        public IEnumerable<SelectListItem> selectedDemondOrderDetailsId { get; set; }
        [Required(ErrorMessage = "* مطلوب")]
        [DisplayName("اسم امر الشغل")]
        public string[] arrayOfselectedString { get; set; }



    }
}