using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockViewModel
{
  public  class SafeReportVM
    {
        public int ID { get; set; }
        public double MoneyValue { get; set; }
        public string OperationType { get; set; }
        public string Date { get; set; }
        public string OperationFor { get; set; }
        public string SafeName { get; set; }
        public double ActualyValue { get; set; }
    }
}
