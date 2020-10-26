namespace StockDB.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Material_Type
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Material_Type()
        {
            Items = new HashSet<Item>();
        }

        [Key]
        public int mat_id { get; set; }
        [Required(ErrorMessage ="لابد من ادخال الخامة")]
        [DisplayName("الخامة")]
        public string mat_name { get; set; }
        public virtual ICollection<Item> Items { get; set; }
    }
}
