using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static PlasticStatic.Enums;

namespace StockViewModel
{
    public class IncomeDetailsVM
    {
        public IncomeDetailsVM()
        {
            this.IncDet_status = new IncomeDetailsStatus();
        }
        public int incDet_id { get; set; }

        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Required(ErrorMessage = "مطلوب")]
        [Display(Name = "تاريخ المصروف")]
        public DateTime incDet_Date { get; set; }

        [Required(ErrorMessage = "مطلوب")]
        [Display(Name = "الخزينه")]
        public int safe_id { get; set; }

        [Required(ErrorMessage = "مطلوب")]
        [Display(Name = "نوع الايراد")]
        public int? inc_id { get; set; }

        [Required(ErrorMessage = "مطلوب")]
        [Display(Name = "القيمه")]
        public double incDet_value { get; set; }

        [Display(Name = "ملاحظات")]
        public string incDet_notes { get; set; }

        [Display(Name = "الحاله")]
        [Required(ErrorMessage = "مطلوب")]
        public IncomeDetailsStatus IncDet_status { get; set; }
        public string IncDet_statusName { get; set; }

        public string incomename { get; set; }
        public string safe_name { get; set; }

        public int? Income_inc_id { get; set; }

       
    }
}
