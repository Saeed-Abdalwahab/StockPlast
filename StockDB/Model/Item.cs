using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;

namespace StockDB.Model
{
    [Table("Item")]
    public partial class Item
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Item()
        {
            ItemSuppliers = new HashSet<ItemSupplier>();
        }

        public int ID { get; set; }
        [Display(Name ="نوع الخامه")]
        public int? MatTypeId { get; set; }
        [Display(Name = "لون الخامه")]

        public int? ColorID { get; set; }
        [Display(Name = "كود الصنف")]

        public string SerialNum { get; set; }
        [Display(Name = "اخر ثمن شراء")]

        public double? PurchasingPrice { get; set; }
        [Display(Name = "اخر ثمن بيع")]

        public double? SellingPrice { get; set; }
        [Display(Name = "المقاس")]

        public string Size { get; set; }
        [Display(Name = "السمك")]

        public double? Thickness { get; set; }
        [Display(Name = "اسم الصنف")]

        public string Name { get; set; }

        public virtual color color { get; set; }

        public virtual Material_Type Material_Type { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ItemSupplier> ItemSuppliers { get; set; }


        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DemondOrderDetail> DemondOrderDetails { get; set; }

    }
}
