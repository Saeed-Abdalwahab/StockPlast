using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockViewModel
{
   public class PaymentTypVM
    {
        public int ID { get; set; }

        [StringLength(50,ErrorMessage ="عدد حروف غير مسموح")]
        [Display(Name ="طريقه الدفع")]
        [Required(ErrorMessage = "مطلوب")]

        public string Name { get; set; }
    }
}
