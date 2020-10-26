namespace StockDB.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using System.ComponentModel;


    public partial class DemondOrderDetail
    {
        public DemondOrderDetail()
        {
            StockTransactions = new HashSet<StockTransaction>();
            ToStocks = new HashSet<ToStock>();
        }

        [Key]
        public int demoOrdDet_id { get; set; }
        //[Required(ErrorMessage = "* مطلوب")]
        public int demoOrd_id { get; set; }
        
        [Required(ErrorMessage = "* مطلوب")]
        [DisplayName("السيريل")]
        public int shap_id { get; set; }
        [Required(ErrorMessage = "* مطلوب")]
        [DisplayName("الكمية")]
        public int demondQuantity { get; set; }
        [DisplayName("الكمية الفعلية")]
        public double? ActualQuantity { get; set; }
        [DisplayName("الحالة")]
        public int? status { get; set; }
        [DisplayName("مقاس العميل")]
        [Required(ErrorMessage = "* مطلوب")]
        //[RegularExpression(@"^\d+[Xx*]\d+$", ErrorMessage = "طول * عرض او طول X عرض")]
        public string size { get; set; }
        [DisplayName("عدد الالوان")]
        public int? colorcount { get; set; }
        [DisplayName("الالوان")]
        public string colorname { get; set; }
        [DisplayName("نوع اليد")]
        public int? HandTypeId { get; set; }


 
        [DisplayName("سعر البيع ")]

        public double? Selling_Price { get; set; }
        [DisplayName("عدد اليد")]

        public int? Hand_Count { get; set; }
        [DisplayName("سعر اليد")]

        public double? Hand_Price { get; set; }
        [DisplayName("الصنف")]
        public int? Item_id { get; set; }
        [DisplayName("لون اليد")]

        public int? HandColorID { get; set; }
        public string Notes { get; set; }
        public virtual color color { get; set; }

        public virtual DemondOrder DemondOrder { get; set; }
        public virtual HandType HandType { get; set; }
        public virtual Item Item { get; set; }

        public virtual Shaplona Shaplona { get; set; }
        public virtual ICollection<StockTransaction> StockTransactions { get; set; }
        public virtual ICollection<ToStock> ToStocks { get; set; }
    }
}
