using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockViewModel
{
    public class FromStockDetailsVM
    {
        public int ID { get; set; }
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]

        public DateTime? TransDate { get; set; }
        public string cutTechName { get; set; }

        [DisplayName("امر التشغيل")]
        public string Serial { get; set; }

        [DisplayName("امر الشغل")]
        public string demoOrd_serailNum { get; set; }
        public string printTechName { get; set; }
        public string Custname { get; set; }
        

    }
}
