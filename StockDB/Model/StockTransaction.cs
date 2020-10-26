namespace StockDB.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("StockTransaction")]
    public partial class StockTransaction
    {
        [Key]
        public int ID { get; set; }

        public int? ToStockId { get; set; }

        public int? StoreId { get; set; }

        public int? ItemSupplierId { get; set; }

        [StringLength(50)]
        public string OperationType { get; set; }
        [Display(Name ="العدد")]
        [Required(ErrorMessage ="مطلوب")]
        public double? NoItem { get; set; }
       
        [Display(Name = "الكميه")]

        public double? Quantity { get; set; }
     

       

        public double? ItemPrice { get; set; }
        public int? FromStockId { get; set; }

        public int? DemondOrderDetialsId { get; set; }
        public double? Weight { get; set; }
        public int? expiredtypid { get; set; }

        public virtual DemondOrderDetail DemondOrderDetail { get; set; }

        public virtual FromStock FromStock { get; set; }

        public virtual ItemSupplier ItemSupplier { get; set; }

        public virtual Store Store { get; set; }

        public virtual ToStock ToStock { get; set; }

    }
}
