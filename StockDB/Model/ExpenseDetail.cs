namespace StockDB.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class ExpenseDetail
    {
        public ExpenseDetail()
        {
            SafeTransactions = new HashSet<SafeTransaction>();

        }
        [Key]
        public int ExpDet_id { get; set; }

        public DateTime ExpDet_Date { get; set; }

        public int safe_id { get; set; }

        public int exp_id { get; set; }

        public double ExpDet_value { get; set; }

        public string ExpDet_notes { get; set; }

        public int ExpDet_status { get; set; }

        public virtual Expense Expense { get; set; }

        public virtual Safe Safe { get; set; }
        public virtual ICollection<SafeTransaction> SafeTransactions { get; set; }

    }
}
