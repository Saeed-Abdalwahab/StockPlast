using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockViewModel
{
  public  class JobDescriptionVM
    {
        public int ID { get; set; }
        [Required(ErrorMessage = "مطلوب")]
        [DisplayName("التخصص")]
        public string Name { get; set; }
        [Required(ErrorMessage = "مطلوب")]
        [DisplayName("الوظيفة")]
        public int JobID { get; set; }
        public string Jobname { get; set; }

    }
}
