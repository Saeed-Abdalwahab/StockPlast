using StockDB.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StockPortal.ViewModel
{
    public class FromStockVM
    {
        public FromStockVM()
        {
            this.arrayOfselectedString = new List<int>();
        }
        public int ID { get; set; }
        [Display(Name = "فني طباعه")]
        [Required(ErrorMessage = "مطلوب")]
        public int PrintTechEmp_ID { get; set; }
        [Display(Name = "فني تقطيع")]
        [Required(ErrorMessage = "مطلوب")]
        public int CutTechEmp_ID { get; set; }
        [Display(Name = "رقم امر التشغيل")]
        [Required(ErrorMessage = "مطلوب")]

        public string Serial { get; set; }
        [Display(Name = "التاريخ")]
        [Required(ErrorMessage = "مطلوب")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]

        [Column(TypeName = "date")]
        public DateTime? TransDate { get; set; }
        [Display(Name = "ملاحظات")]

        public string notes { get; set; }
        public IEnumerable<SelectListItem> selectedDemondOrderDetailsId { get; set; }
        [Display(Name =  "اوامر الشغل")]
        public string DemondOrderDetailsIdString { get; set; }
        [Required(ErrorMessage = "مطلوب")]
        [DisplayName("اسم امر الشغل")]
        public List<int> arrayOfselectedString { get; set; }

    }
}