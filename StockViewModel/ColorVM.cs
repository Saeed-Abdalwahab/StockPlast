using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockViewModel
{
  public  class ColorVM
    {
        public int color_id { get; set; }
        [Required(ErrorMessage = "لابد من ادخال اللون")]
        [DisplayName("اللون")]
        public string color_name { get; set; }
    }
}
