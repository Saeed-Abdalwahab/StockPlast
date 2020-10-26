namespace StockDB.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Customer")]
    public partial class Customer
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Customer()
        {
          
            Shaplonas = new HashSet<Shaplona>();
            DemondOrders = new HashSet<DemondOrder>();
            CustomerPayments = new HashSet<CustomerPayment>();
        }

        [Key]
        public int cust_id { get; set; }
        [DisplayName("رقم العميل")]
        [Required(ErrorMessage = "لابد من ادخال رقم العميل")]
        public string custSerial { get; set; }

        [DisplayName("اسم العميل")]
        [Required(ErrorMessage = "لابد من ادخال اسم العميل")]
        public string cust_name { get; set; }


        [Phone(ErrorMessage ="رقم الهاتف غير صحيح")]
        [Required(ErrorMessage = "لابد من ادخال رقم الهاتف")]
        [DisplayName("رقم الهاتف")]
        public string cust_mobile { get; set; }



        [DisplayName("البريد الالكتروني")]
        [EmailAddress(ErrorMessage = "بريد الكتروني غير صحيح")]
        public string cust_mail { get; set; }



        [DisplayName("الشخص المسئول")]
        public string contact_name { get; set; }



        [DisplayName("العنوان")]
        public string cust_address { get; set; }



        [Phone(ErrorMessage = "رقم الهاتف غير صحيح")]
        [DisplayName("رقم الواتساب")]
        public string WhatsApp { get; set; }
        public double? OpiningBlance { get; set; }
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]

        public DateTime? BlanceDate { get; set; }
        public int? SafeID { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CustomerPayment> CustomerPayments { get; set; }
    


       
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Shaplona> Shaplonas { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DemondOrder> DemondOrders { get; set; }
    }
}
