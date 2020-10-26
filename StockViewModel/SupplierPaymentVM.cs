using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static PlasticStatic.Enums;

namespace StockViewModel
{
 public   class SupplierPaymentVM
    {
        public int ID { get; set; }
        [Required(ErrorMessage = "مطلوب")]

        public int Supplier_ID { get; set; }
        [Required(ErrorMessage = "مطلوب")]

        public int PaymentTyp_ID { get; set; }
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}", ApplyFormatInEditMode = true)]
        [Required(ErrorMessage = "مطلوب")]
        public DateTime PaymentDate { get; set; }
        [Required(ErrorMessage = "مطلوب")]

        public double? PaymentValue { get; set; }
        [Required(ErrorMessage = "مطلوب")]
        public int Bank_ID { get; set; }
        [Required(ErrorMessage = "مطلوب")]
        public int Safe_ID { get; set; }

        public string Notes { get; set; }
        [Required(ErrorMessage = "مطلوب")]

        public cheque_status cheque_status { get; set; }
        [StringLength(50, ErrorMessage = "رقم غير صحيح")]
        public string PaymentNumber { get; set; }

        public string SupplierName { get; set; }
        public string ChequStatusName { get; set; }
        public string PaymentTypName { get; set; }
        public string BankName { get; set; }
        public string SafeName { get; set; }
    }
}
