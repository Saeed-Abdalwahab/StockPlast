using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockDB.Model
{
    [Table("SafeTransaction")]
    public partial class SafeTransaction
    {
        public int ID { get; set; }

        public int? SafeID { get; set; }

        public int? IncomeDetailsID { get; set; }

        public int? ExpenseDetailsID { get; set; }

        public int? CustomerPaymentID { get; set; }

        public int? SupplierPaymentID { get; set; }

        public int? DemondorderID { get; set; }

        public double TransactionValue { get; set; }

        [Required]
        [StringLength(10)]
        public string OperationType { get; set; }

        [Column(TypeName = "date")]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}", ApplyFormatInEditMode = true)]
        public DateTime TransactionDate { get; set; }

        public virtual CustomerPayment CustomerPayment { get; set; }

        public virtual DemondOrder DemondOrder { get; set; }


        public virtual ExpenseDetail ExpenseDetail { get; set; }

        public virtual IncomeDetail IncomeDetail { get; set; }

        public virtual SupplierPayments SupplierPayment { get; set; }
        public virtual Safe Safe { get; set; }
    }
}
