using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockDB.Model
{
    [Table("CustomerPayment")]
    public partial class CustomerPayment
    {
        public CustomerPayment()
        {
            SafeTransactions = new HashSet<SafeTransaction>();
        }
        public int ID { get; set; }

        public int? Customer_ID { get; set; }

        public int? PaymentTyp_ID { get; set; }

        public DateTime? PaymentDate { get; set; }

        public double? PaymentValue { get; set; }

        public int? Bank_ID { get; set; }

        public int? Safe_ID { get; set; }

        public string Notes { get; set; }

        public int? cheque_status { get; set; }

        [StringLength(50,ErrorMessage ="رقم غير صحيح")]
        public string PaymentNumber { get; set; }

        [StringLength(1,ErrorMessage ="عمليه غير صحيحه")]
        public string OperationTyp { get; set; }

        public int? FromstockID { get; set; }

        public int? ToStockID { get; set; }

        public virtual Bank Bank { get; set; }

        public virtual Customer Customer { get; set; }

        public virtual FromStock FromStock { get; set; }

        public virtual ToStock ToStock { get; set; }
        public virtual PaymentType PaymentType { get; set; }

        public virtual Safe Safe { get; set; }
        public virtual ICollection<SafeTransaction> SafeTransactions { get; set; }

    }
}
