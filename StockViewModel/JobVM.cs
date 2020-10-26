using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockViewModel
{
   public class JobVM
    {
        public int job_id { get; set; }
        [Display(Name = "الوظيفة")]
        [Required(ErrorMessage = "لابد من اختيار الوظيفة")]
        public string job_name { get; set; }



    }
}
