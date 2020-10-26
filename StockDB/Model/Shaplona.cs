namespace StockDB.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using System.ComponentModel;
    using Foolproof;

    [Table("Shaplona")]
    public partial class Shaplona
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Shaplona()
        {
            DemondOrderDetails = new HashSet<DemondOrderDetail>();
        }
        [Key]
        public int shap_id { get; set; }
        [Required(ErrorMessage ="مطلوب")]
        public int cust_id { get; set; }
        [Required(ErrorMessage = "مطلوب")]
        public string shap_name { get; set; }
        public string shap_pic { get; set; }
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}", ApplyFormatInEditMode = true)]
        public DateTime? shap_startDate { get; set; }
        [Required(ErrorMessage = "مطلوب")]
        public string ShapSerial { get; set; }
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}", ApplyFormatInEditMode = true)]
        //[GreaterThan("shap_startDate",ErrorMessage ="تاريخ الخروج اكبر من الدخول ",PassOnNull =true)]
        public DateTime? shap_endDate { get; set; }
        public virtual Customer Customer { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DemondOrderDetail> DemondOrderDetails { get; set; }
    }
}
