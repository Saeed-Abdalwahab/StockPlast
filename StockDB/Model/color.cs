namespace StockDB.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("color")]
    public partial class color
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public color()
        {
            Items = new HashSet<Item>();
            DemondOrderDetails = new HashSet<DemondOrderDetail>();

        }

        [Key]
        
        public int color_id { get; set; }
        [Required (ErrorMessage ="لابد من ادخال اللون")]
        [DisplayName ("اللون")]
        public string color_name { get; set; }
        public virtual ICollection<DemondOrderDetail> DemondOrderDetails { get; set; }

        public virtual ICollection<Item> Items { get; set; }
    }
    }
