namespace StockDB.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Store")]
    public partial class Store
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Store()
        {

            StockTransactions = new HashSet<StockTransaction>();
        }

        [Key]
        public int st_id { get; set; }
        [DisplayName("كود المخزن")]
        [Required(ErrorMessage = "لابد من ادخال كود المخزن")]
        public string st_SerialNum { get; set; }
        [DisplayName("امين المخزن")]
        public int? emp_id { get; set; }
        [Required(ErrorMessage = "لابد من ادخال اسم المخزن")]
        [DisplayName("اسم المخزن")]
        public string st_name { get; set; }
        [Required(ErrorMessage = "لابد من اختيار نوع المخزن")]
        [DisplayName("نوع المخزن")]
        public int StoreType_ID { get; set; }
        [DisplayName("العنوان")]
        public string st_address { get; set; }

        public virtual Employe Employe { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<StockTransaction> StockTransactions { get; set; }

        public virtual StoreType StoreType { get; set; }
    }
}
