using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockViewModel
{
   public class AlertVM<T>
    {
        public string ErrorMessage { get; set; }
        public string AlertType { get; set; }
        public bool type { get; set; }

        public List<T> data { get; set; }
        public T Data { get; set; }
    }
}