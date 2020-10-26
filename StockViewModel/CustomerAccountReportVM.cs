using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockViewModel
{
  public  class CustomerAccountReportVM
    {
        public DateTime? Date { get; set; }
        public double? Sales { get; set; }
        public double? SalesReturn { get; set; }
        public double? Payment { get; set; }
        public double? Discount { get; set; }
        public double? TotalBalance { get; set; }
    }
}
