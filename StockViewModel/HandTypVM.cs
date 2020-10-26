using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockViewModel
{
  public  class HandTypVM
    {
        
        public int HandType_id { get; set; }
        [Required(ErrorMessage ="مطلوب")]
        public string HandType_name { get; set; }


    }
}
