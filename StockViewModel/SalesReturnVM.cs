using StockDB.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StockViewModel
{
    public  class SalesReturnVM
    {
        public int ID { get; set; }
        [Required(ErrorMessage ="مطلوب")]
        public string InvoiceSerial { get; set; }

        [Column(TypeName = "date")]
        [Required(ErrorMessage = "مطلوب")]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}", ApplyFormatInEditMode = true)]

        public DateTime? InvoiceDate { get; set; }
        public string cutomername { get; set; }
        public int demondorderdetailsid { get; set; }
        [Required(ErrorMessage = "مطلوب")]

        public PlasticStatic.Enums.InvoiceStatus? InvoiceStatus { get; set; }

        public int? ToStockTypeId { get; set; } = (int)PlasticStatic.Enums.ToStockTypeId.SalesReturns;
        [Required(ErrorMessage = "مطلوب")]

        public int storeid { get; set; }


        public string Notes { get; set; }
        public int? FromStockId { get; set; }
        public double? Price { get; set; }
        public List<StockTransaction> stocktransactions { get; set; }

    }
}
