using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PurchaseInvoiceViewModel
{
    public class PurchaseInvoiceViewModel
    {
        public int ID { get; set; }
        public string InvoiceSerial { get; set; }
        public DateTime? InvoiceDate { get; set; }
        public int? SupId { get; set; }
        public string SupName { get; set; }
        public string Notes { get; set; }
    }
}
