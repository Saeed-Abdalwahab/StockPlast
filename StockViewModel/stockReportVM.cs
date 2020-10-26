using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockViewModel
{
  public  class stockReportVM
    {


        public string storeName { get; set; }

        public string itemname { get; set; }
        public string supliername { get; set; }
        public double? NoItem { get; set; }
        public double? Quantity { get; set; }
        public double? Weight { get; set; }

    }
}
