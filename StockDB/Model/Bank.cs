namespace StockDB.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    [Table("Bank")]
    public partial class Bank
    {
        public Bank()
        {
            CustomerPayments = new HashSet<CustomerPayment>();
            SupplierPayments = new HashSet<SupplierPayments>();

        }
        [Key]
        public int ID { get; set; }
        [DisplayName("البنك")]
        public string Name { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CustomerPayment> CustomerPayments { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SupplierPayments> SupplierPayments { get; set; }
    }
}
