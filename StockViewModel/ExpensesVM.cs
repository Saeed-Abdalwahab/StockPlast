using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static PlasticStatic.Enums;

namespace StockViewModel
{
  public  class ExpensesVM
    {
        public ExpensesVM() {
            this.exp_ProfitStatus = new ExpensStatus();
        }

        [Required(ErrorMessage ="مطلوب")]
        public int exp_id { get; set; }
        [Display(Name ="المصروف")]
        [Required(ErrorMessage = "مطلوب")]

        public string exp_name { get; set; }
        [Required(ErrorMessage = "مطلوب")]
        [Display(Name = "نوع المصروف")]

        public ExpensStatus exp_ProfitStatus { get; set; } = ExpensStatus.OtherExpinse;
        public string exp_profitstautsname { get; set; }
    }
}
