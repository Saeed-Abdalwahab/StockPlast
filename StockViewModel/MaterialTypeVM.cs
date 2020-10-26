using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockViewModel
{
 public class MaterialTypeVM
    {
        public int mat_id { get; set; }
        [Required(ErrorMessage = "مطلوب")]
        [DisplayName("الخامة")]

        public string mat_name { get; set; }
    }
}
