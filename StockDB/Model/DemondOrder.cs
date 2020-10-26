namespace StockDB.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("DemondOrder")]
    public partial class DemondOrder
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public DemondOrder()
        {
            DemondOrderDetails = new HashSet<DemondOrderDetail>();
            SafeTransactions = new HashSet<SafeTransaction>();

        }

        [Key]
        public int demoOrd_id { get; set; }

        public int cust_id { get; set; }

        public int? safe_id { get; set; }

        public string demoOrd_serailNum { get; set; }
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]

        [Column(TypeName = "date")]
        public DateTime demoOrd_date { get; set; }

        public string demoOrd_notes { get; set; }

        public double? demoOrd_deposit { get; set; }

        public virtual Customer Customer { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DemondOrderDetail> DemondOrderDetails { get; set; }


        public virtual Safe Safe { get; set; }
        public virtual ICollection<FromStock> FromStocks { get; set; }
        public virtual ICollection<SafeTransaction> SafeTransactions { get; set; }

    }
}
