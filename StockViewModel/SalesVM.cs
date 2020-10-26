using StockDB.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static PlasticStatic.Enums;

namespace StockViewModel
{
   public class SalesVM
    {
        public SalesVM()
        {
            this.stocktransactionlist = new List<StockTransaction>();
            this.stocktranslist = new List<CustomstocktransVM>();
        }
        public int Sale_ID { get; set; }

        [Display(Name ="اسم العميل")]
        public string cust_name { get; set; }
        [Required(ErrorMessage ="مطلوب")]
        public int? cust_ID { get; set; }
        public int? DemondorderDetailid { get; set; }
      
        public List<StockTransaction> stocktransactionlist { get; set; }
        [Display(Name = "موبيل العميل")]

        public string Cust_phone { get; set; }
        [Display(Name = "اسم الشغل")]
        public string Demondorderdetailsname { get; set; }
        public double? invoicecost { get; set; }

        public double? TotalCost{ get; set; }
        public double? KiloGram { get; set; }
        [Required(ErrorMessage = "مطلوب")]

        public InvoiceSSAlestatus invoicestatus { get; set; }

        public   List<CustomstocktransVM> stocktranslist { get; set; }

        [Required(ErrorMessage ="مطلوب")]

        public string Serial { get; set; }
        [Required(ErrorMessage = "مطلوب")]

        [Display(Name = "تاريخ الفاتوره")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]

        [Column(TypeName = "date")]
        public DateTime? TransDate { get; set; }
        [Display(Name = "اوامر التشغيل")]

        public int? DemondOrderId { get; set; }

        public int? FromStockTypeID { get; set; }
        [Display(Name = "الملاحظات")]


        public string notes { get; set; }
        public double? sale_tax { get; set; }
        public double? sale_charge { get; set; }
        public double? sale_discount { get; set; }
        public int? discount_typ { get; set; }
        
    }
}
