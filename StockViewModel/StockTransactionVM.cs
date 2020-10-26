using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;


namespace StockViewModel
{
    public class StockTransactionVM
    {
        public StockTransactionVM()
        {
            this.Stores = new List<SelectListItem>();

            //this.quantity_No_StockTransaction = new Avaliableiteminstore(); //commented by mohamed ibrahim 
        }

        public List<SelectListItem> Stores { get; set; }


        [Required(ErrorMessage = "مطلوب")]
        [Display(Name = "الصنف")]
        public int itemid { get; set; }

        [Display(Name = "المورد")]
        [Required(ErrorMessage = "مطلوب")]
        public int? suplierid { get; set; }

        [Display(Name = "المخزن")]

        public int Storeid { get; set; }

        [Display(Name = "الكميه المطلوبه")]
        [Required(ErrorMessage = "مطلوب")]

        [Remote("CheckQuantity", "StockTransactions", AdditionalFields = "itemid,suplierid,Storeid", HttpMethod = "GET")]
        public double? Quantity { get; set; }

        [Display(Name = "العدد المطلوب")]
        [Remote("Checkitem", "StockTransactions", AdditionalFields = "itemid,suplierid,Storeid", HttpMethod = "GET")]
        [Required(ErrorMessage = "مطلوب")]

        public int ID { get; set; }
        public string ItemName { get; set; }
        public string SupName { get; set; }
        public string StoreName { get; set; }
        public double? NoItem { get; set; }
        public int? ItemSupID { get; set; }



        [Required(ErrorMessage = "مطلوب")]
        [Display(Name = "اسم الشغل")]
        public int Demondorderdetailsid { get; set; }
        [Required(ErrorMessage = "مطلوب")]


        public double? Weight { get; set; }
        public double? Total { get; set; }
        public int? FromStockId { get; set; }
        public int? status { get; set; }

        public string Demonordname { get; set; }


        //public Avaliableiteminstore quantity_No_StockTransaction { get; set; } //commented by mohamed ibrahim 
    }
}
