using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockViewModel
{
   public class CustomstocktransVM
    {
        public int ID { get; set; }
        public double totalprice { get; set; }
        public double? returnedQuantity { get; set; }
        public double? SelingPrice { get; set; }
        public double? ActualQuantity { get; set; }
        public double? HandPrice { get; set; }
        public int? HandCount { get; set; }

    }
}
