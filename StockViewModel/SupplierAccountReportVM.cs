using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockViewModel
{
  public  class SupplierAccountReportVM
    {
        public DateTime? Date { get; set; }
        public double? Purchases { get; set; }
        public double? PurchasesReturn { get; set; }
        public double? Payment { get; set; }
        public double? TotalBalance { get; set; }
    }
}
