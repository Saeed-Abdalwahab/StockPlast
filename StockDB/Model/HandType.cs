namespace StockDB.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using System.ComponentModel;

    [Table("HandType")]
    public partial class HandType
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public HandType()
        {
            DemondOrderDetails = new HashSet<DemondOrderDetail>();
        }

        [Key]
       
        public int HandType_id { get; set; }

        public string HandType_name { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DemondOrderDetail> DemondOrderDetails { get; set; }
    }
}
