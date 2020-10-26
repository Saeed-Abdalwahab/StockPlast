namespace StockDB.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ToStock")]
    public partial class ToStock
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ToStock()
        {
            StockTransactions = new HashSet<StockTransaction>();
            CustomerPayments = new HashSet<CustomerPayment>();
        }

        public int ID { get; set; }

        public string InvoiceSerial { get; set; }

        [Column(TypeName = "date")]
        public DateTime? InvoiceDate { get; set; }

        public int? SupId { get; set; }

        public int? InvoiceStatus { get; set; }

        public int? ToStockTypeId { get; set; }

        public int? DemondOrderDetialsId { get; set; }

        public string Notes { get; set; }
        public int? FromStockId { get; set; }
        public double? Price { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CustomerPayment> CustomerPayments { get; set; }
        public virtual DemondOrderDetail DemondOrderDetail { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<FromStock> FromStocks { get; set; }

        public virtual FromStock FromStock { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<StockTransaction> StockTransactions { get; set; }

        public virtual supplier supplier { get; set; }
    }
}
