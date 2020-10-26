using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace StockPortal.ViewModel
{
    public class DemondOrderVM
    {
        public int demoOrd_id { get; set; }
        [DisplayName("العميل")]
        [Required(ErrorMessage = "* مطلوب")]
        public int cust_id { get; set; }
        [DisplayName("الخزينه")]

        public int? safe_id { get; set; }
        [DisplayName("رقم الامر")]
        [Required(ErrorMessage = "* مطلوب")]
        public string demoOrd_serailNum { get; set; }
        [DisplayName("تاريخ الامر")]
        [Required(ErrorMessage = "* مطلوب")]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}", ApplyFormatInEditMode = true)]
        public DateTime demoOrd_date { get; set; }
        public string demoOrd_datetring { get; set; }
        [DisplayName("ملاحظات")]
        public string demoOrd_notes { get; set; }
        [DisplayName("العربون")]
        public double? demoOrd_deposit { get; set; }

        public string CustomerName { get; set; }
        public string SafeName { get; set; }
        public string demoOrd_dateString { get; set; }
    }
}