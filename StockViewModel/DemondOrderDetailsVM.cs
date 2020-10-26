using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockViewModel
{
  public  class DemondOrderDetailsVM
    {
        public int demoOrdDet_id { get; set; }
        [Required(ErrorMessage = "* مطلوب")]
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
        //[RegularExpression(@"^(\.?\d+\.?\d*)+[Xx*×]+(\.?\d+\.?\d*)+$", ErrorMessage = "طول * عرض او طول X عرض")]
        public string size { get; set; }
        [DisplayName("عدد الالوان")]
        public int? colorcount { get; set; }
        [DisplayName("الالوان")]
        public string colorname { get; set; }
        [DisplayName("نوع اليد")]
        [Required(ErrorMessage = "* مطلوب")]

        public int HandTypeId { get; set; }

        [DisplayName("سعر البيع ")]

        public double? Selling_Price { get; set; }
        public int? HandColorID { get; set; }
        [DisplayName("عدد اليد")]

        public int? Hand_Count { get; set; }
        [DisplayName("سعر اليد")]

        public double? Hand_Price { get; set; }
        [DisplayName("الصنف")]
        [Required(ErrorMessage = "* مطلوب")]

        public int Item_id { get; set; }
        public int Cust_id { get; set; }
        public string Notes { get; set; }
    }
}
