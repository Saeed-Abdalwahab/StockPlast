namespace StockDB.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Safe")]
    public partial class Safe
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Safe()
        {

            DemondOrders = new HashSet<DemondOrder>();
            ExpenseDetails = new HashSet<ExpenseDetail>();
            IncomeDetails = new HashSet<IncomeDetail>();
          
            CustomerPayments = new HashSet<CustomerPayment>();
            SafeTransactions = new HashSet<SafeTransaction>();
        }

        [Key]

        public int safe_id { get; set; }
        [DisplayName ("امين الخزينة")]
        public int? emp_id { get; set; }
        [DisplayName("اسم الخزينة")]
        [Required(ErrorMessage = "لابد من ادخال اسم الخزينة")]
        public string safe_name { get; set; }
        [DisplayName("الرصيد الافتتاحي")]
        public double? safe_totalBalance { get; set; }
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [DisplayName("تاريخ الرصيد")]
        [Required(ErrorMessage = "لابد من ادخال تاريخ الرصيد")]
        public DateTime safe_date { get; set; }


        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CustomerPayment> CustomerPayments { get; set; }


        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DemondOrder> DemondOrders { get; set; }
        public virtual ICollection<SafeTransaction> SafeTransactions { get; set; }

        public virtual Employe Employe { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ExpenseDetail> ExpenseDetails { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<IncomeDetail> IncomeDetails { get; set; }

    

    }
}
