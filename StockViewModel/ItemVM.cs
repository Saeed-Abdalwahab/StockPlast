using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockViewModel
{
   public class ItemVM
    {
        public int ID { get; set; }
        [Display(Name = "اسم الصنف")]
        public string Name { get; set; }
        [Required(ErrorMessage = "مطلوب")]

        [Display(Name ="نوع الخامه")]
        public int MatTypeId { get; set; }
        [Display(Name = "لون الخامه")]
        [Required(ErrorMessage = "مطلوب")]

        public int ColorID { get; set; }
        [Display(Name = "كود الصنف")]
        [Required(ErrorMessage = "مطلوب")]

        public string SerialNum { get; set; }
        [Display(Name = "اخر ثمن شراء")]
        [Range(.1, double.MaxValue, ErrorMessage = "رقم غير صحيح")]

        public double? PurchasingPrice { get; set; }
        [Display(Name = "اخر ثمن بيع")]
        [Range(.1, double.MaxValue,ErrorMessage ="رقم غير صحيح")]
        public double? SellingPrice { get; set; }
        [Display(Name = "المقاس")]
        [Required(ErrorMessage = "مطلوب")]

        public string Size { get; set; }
        [Display(Name = "السمك")]
        [Required(ErrorMessage = "مطلوب")]
        [Range(.1, double.MaxValue, ErrorMessage = "رقم غير صحيح")]

        public double Thickness { get; set; }



    }
}
