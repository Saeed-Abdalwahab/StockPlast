using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockDB.Model
{
    [Table("JobDescription")]
    public partial class JobDescription
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public JobDescription()
        {
            Employes = new HashSet<Employe>();
        }
        [Key]
        public int ID { get; set; }
        [Required(ErrorMessage = "لابد من اختيار التخصص")]
        [DisplayName("التخصص")]
        public string Name { get; set; }
        [Required(ErrorMessage = "لابد من اختيار الوظيفة")]
        [DisplayName("الوظيفة")]
        public int JobID { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Employe> Employes { get; set; }
        public virtual Job Job { get; set; }

    }
}
