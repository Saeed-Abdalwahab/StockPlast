namespace StockDB.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class IncomeDetail
    {
        public IncomeDetail()
        {
            SafeTransactions = new HashSet<SafeTransaction>();

        }
        [Key]
        public int incDet_id { get; set; }

        public DateTime incDet_Date { get; set; }

        public int safe_id { get; set; }


        public double incDet_value { get; set; }

        public string incDet_notes { get; set; }

        public int incDet_status { get; set; }

        public int? Income_inc_id { get; set; }

        public virtual Income Income { get; set; }

        public virtual Safe Safe { get; set; }
        public virtual ICollection<SafeTransaction> SafeTransactions { get; set; }

    }
}
