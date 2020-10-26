using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockViewModel
{
   public class SupplierViewModel
    {
        [Required(ErrorMessage = "لابد من ادخال كود المورد")]
        public int sup_id { get; set; }
        [Required(ErrorMessage = "لابد من ادخال اسم المورد")]
        [DisplayName("اسم المورد")]
        public string sup_name { get; set; }
        [DisplayName("الموبايل")]

        public string sup_mobile { get; set; }
        [DisplayName("الايميل")]

        public string sup_mail { get; set; }
        [DisplayName("الشخص المسئول")]

        public string contact_name { get; set; }
        [DisplayName("رقم الواتساب")]

        public string WhatsApp { get; set; }

    }
}
