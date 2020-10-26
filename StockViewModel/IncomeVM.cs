using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static PlasticStatic.Enums;

namespace StockViewModel
{
    public class IncomeVM
    {
        public IncomeVM()
        {
            this.inc_ProfitStatus = new IncomeStatus();
        }

        [Required(ErrorMessage = "مطلوب")]
        public int inc_id { get; set; }

        [Display(Name = "الايراد")]
        [Required(ErrorMessage = "مطلوب")]
        public string inc_name { get; set; }

        [Required(ErrorMessage = "مطلوب")]
        [Display(Name = "نوع الايراد")]
        public IncomeStatus inc_ProfitStatus { get; set; } = IncomeStatus.OtherIncome;

        public string inc_ProfitStatusname { get; set; }
    }
}
