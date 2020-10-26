using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static PlasticStatic.Enums;

namespace StockViewModel
{
    public class PurchaseInvoiceViewModel
    {
        public int ID { get; set; }

        [DisplayName("رقم الفاتورة")]
        [Required(ErrorMessage ="ادخل رقم الفاتورة")]
        public string InvoiceSerial { get; set; }
        [Required(ErrorMessage = "ادخل تاريخ الفاتورة")]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}", ApplyFormatInEditMode = true)]
        [DisplayName(" تاريخ الفاتورة")]
        public DateTime? InvoiceDate { get; set; }
        [Required(ErrorMessage = "ادخل اسم المورد")]
        public int? SupId { get; set; }

        [DisplayName("اسم المورد ")]
        public string SupName { get; set; }

        [DisplayName("الملاحظات")]
        public string Notes { get; set; }

        public int? ItemID { get; set; }

        [DisplayName("الصنف")]
        public string ItemName { get; set; }

        [DisplayName("المخزن")]
        [Required(ErrorMessage = "مطلوب")]
        public int? StoreId { get; set; }

        [DisplayName("العدد")]
        public double? NoItem { get; set; }  

        [DisplayName("المخزن")]
        public string StoreName {get; set; }
        public int TransactionID { get; set; }

        [DisplayName("حاله الفاتوره")]
        public int? InvoiceStatus { get; set; }

        public string InvoiceStatusName { get; set; }

        public double? Weight { get; set; }

        public int? StockTransactioID { get; set; }
        public int? FromStockID { get; set; }

        public int? ToStockTypeId { get; set; }
        public double? itemprice { get; set; }
        public double? totalPurchesiescost { get; set; }
        public int? expiredtypid { get; set; }
        public expiredtype expiredType { get; set; }
        public string expiredtypname { get; set; }
         public int? demondordid { get; set; }
         public int? itemsupid { get; set; }
        [Required(ErrorMessage ="حدد كميه التالف")]

         public double finishweight { get; set; }
        public string demondordname { get; set; }
        public double? printexpired { get; set; }
        public double? cutexpired { get; set; }
        public double? filmexpired { get; set; }

    }
    public class ItemViewModel
    {
        public int ID { get; set; }
        public string Name { get; set; }


    }
}
