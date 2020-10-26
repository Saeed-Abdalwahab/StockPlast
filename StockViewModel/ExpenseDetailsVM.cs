using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static PlasticStatic.Enums;

namespace StockViewModel
{
   public class ExpenseDetailsVM
    {

        public ExpenseDetailsVM()
        {
            this.ExpDet_status = new ExpensDetailsStatus();
        }
        public int ExpDet_id { get; set; }
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Required(ErrorMessage ="مطلوب")]
        [Display(Name ="تاريخ المصروف")]
        public DateTime ExpDet_Date { get; set; }
        [Required(ErrorMessage = "مطلوب")]
        [Display(Name = "الخزينه")]

        public int safe_id { get; set; }
        [Required(ErrorMessage = "مطلوب")]
        [Display(Name = "نوع المصروف")]

        public int exp_id { get; set; }
        [Required(ErrorMessage = "مطلوب")]
        [Display(Name = "القيمه")]

        public double ExpDet_value { get; set; }
        [Display(Name = "ملاحظات")]

        public string ExpDet_notes { get; set; }
        [Display(Name = "الحاله")]
        [Required(ErrorMessage = "مطلوب")]

        public ExpensDetailsStatus ExpDet_status { get; set; }
        public string ExpDet_statusName { get; set; }

        public string expensename { get; set; }
        public string safename { get; set; }
    }
}
