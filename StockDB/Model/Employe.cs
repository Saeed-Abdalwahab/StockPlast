namespace StockDB.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Employe")]
    public partial class Employe
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Employe()
        {
            Safes = new HashSet<Safe>();
            Stores = new HashSet<Store>();
            FromStocks = new HashSet<FromStock>();
            FromStocks1 = new HashSet<FromStock>();
        }

        [Key]
        public int emp_id { get; set; }
        [Display(Name ="اسم الموظف")]
        [Required(ErrorMessage ="لابد من ادخال اسم الموظف")]


        public string emp_name { get; set; }
        [Display(Name ="رقم الهاتف")]
        //[Required(ErrorMessage = "لابد من ادخال رقم الهاتف")]
        [Phone(ErrorMessage = "رقم الهاتف غير صحيح")]

        public string emp_mobile { get; set; }
        [Display(Name = "البريد الالكتروني")]
        [EmailAddress(ErrorMessage ="بريد الكتروني غير صحيح")]
        public string emp_mail { get; set; }

        [Display(Name = "الوظيفة")]
        [Required(ErrorMessage = "لابد من اختيار الوظيفة")]
        public int Job_id { get; set; }
        [Display(Name = "التخصص")]
        public int? jobDesc_id { get; set; }

        public virtual Job Job { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Safe> Safes { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Store> Stores { get; set; }

        public virtual JobDescription JobDescription { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<FromStock> FromStocks { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<FromStock> FromStocks1 { get; set; }

    }
}
