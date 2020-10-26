namespace StockDB.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("FromStock")]
    public partial class FromStock
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public FromStock()
        {
            StockTransactions = new HashSet<StockTransaction>();
            ToStocks = new HashSet<ToStock>();
            CustomerPayments = new HashSet<CustomerPayment>();
        }

        public int ID { get; set; }
        
        public int? PrintTechEmp_ID { get; set; }
     
     

        public int? CutTechEmp_ID { get; set; }
        [Display(Name = "رقم امر التشغيل")]


        public string Serial { get; set; }

        [Display(Name = "التاريخ")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]

        [Column(TypeName = "date")]
        public DateTime? TransDate { get; set; }
        [Display(Name = "اوامر التشغيل")]

        public int? DemondOrderId { get; set; }

        public int? FromStockTypeID { get; set; }
        [Display(Name = "الملاحظات")]


        public string notes { get; set; }
        public double? sale_tax { get; set; }
        public double? sale_charge { get; set; }
        public double? sale_discount { get; set; }
        public double? invoicecost { get; set; }
        public int? ToStockId { get; set; }
        public int? Status { get; set; }



        public virtual DemondOrder DemondOrder { get; set; }
        //فني الطباعه
        public virtual Employe Employe { get; set; }
        ////فني التثطيع
        public virtual Employe Employe1 { get; set; }

        public virtual ToStock ToStock { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CustomerPayment> CustomerPayments { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<StockTransaction> StockTransactions { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]

        public virtual ICollection<ToStock> ToStocks { get; set; }
    }
}
