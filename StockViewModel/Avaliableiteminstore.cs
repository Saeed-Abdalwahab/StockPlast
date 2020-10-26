using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace StockPortal.ViewModel
{
    public class Avaliableiteminstore
    {
        public int ID { get; set; }

        [Display(Name ="الصنف")]
       public string itemname { get; set; }


        [Display(Name = "المورد")]
        public string supliername { get; set; }


        [Display(Name = "المخزن")]
        public string storename { get; set; }
        public int storeid { get; set; }

        [Display(Name = "فئه الوزن")]
        public double? weight { get; set; }
        [Display(Name = "عدد البكرات")]
        public double? Noitem { get; set; }
        public int? itemsuplierid { get; set; }
        public int itemid { get; set; }

        public double? ItemPrice { get; set; }
        public double? ReturnedItemPrice { get; set; }
        public double? ReturnedNoitem { get; set; }

       
        
   

    }
}