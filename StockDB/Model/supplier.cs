namespace StockDB.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("supplier")]
    public partial class supplier
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public supplier()
        {
            ItemSuppliers = new HashSet<ItemSupplier>();
          
            ToStocks = new HashSet<ToStock>();
            SupplierPayments = new HashSet<SupplierPayments>();
        }

        [Key]
        [Required(ErrorMessage = "لابد من ادخال كود المورد")]
        public int sup_id { get; set; }
        [Required (ErrorMessage ="لابد من ادخال اسم المورد")]
        [DisplayName ("اسم المورد")]
        public string sup_name { get; set; }
        [Phone(ErrorMessage = "رقم الهاتف غير صحيح")]
        [DisplayName("رقم الهاتف")]
        [Required(ErrorMessage = "لابد من ادخال رقم الهاتف")]

        public string sup_mobile { get; set; }
        [DisplayName("البريد الالكتروني")]
        [EmailAddress(ErrorMessage = "بريد الكتروني غير صحيح")]

        public string sup_mail { get; set; }
        [DisplayName("الشخص المسئول")]
        //[Required(ErrorMessage = "لابد من ادخال اسم الشخص المسئول")]

        public string contact_name { get; set; }
        [Phone(ErrorMessage = "رقم الهاتف غير صحيح")]
        [DisplayName("رقم الواتساب")]

        public string WhatsApp { get; set; }

        public double? OpiningBlance { get; set; }
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]

        public DateTime? BlanceDate { get; set; }
        public int? SafeID { get; set; }
        [DisplayName("رقم المورد")]
        [Required(ErrorMessage = "مطلوب")]

        public string Sup_Serial { get; set; }
        public virtual ICollection<ItemSupplier> ItemSuppliers { get; set; }
    

  
        public virtual ICollection<ToStock> ToStocks { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SupplierPayments> SupplierPayments { get; set; }

    }
}
