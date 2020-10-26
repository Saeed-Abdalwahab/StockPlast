using StockDB.Model;
using StockPortal.ViewModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockViewModel
{
   public class purchasesreturnVM
    {
        public int ID { get; set; }

        [Display(Name = "رقم المرتجع")]
        public string Serial { get; set; }
        [Display(Name = "التاريخ")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Column(TypeName = "date")]
        public DateTime? TransDate { get; set; }
        public int? FromStockTypeID { get; set; }
        [Display(Name = "الملاحظات")]
        public string notes { get; set; }
        public double? invoicecost { get; set; }
        public int? toStockid { get; set; }
        [Required(ErrorMessage = "مطلوب")]

        public PlasticStatic.Enums.InvoiceStatus? InvoiceStatus { get; set; }
        public String supname { get; set; }
        public List<StockTransaction> stocktransactions { get; set; }
        public int SupplierID { get; set; }
        public int StoreID { get; set; }
        public List<Avaliableiteminstore> avaliableiteminstores { get; set; }


    }
}
