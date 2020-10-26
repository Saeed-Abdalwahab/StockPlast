namespace StockDB.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("AspNetUserWorker")]
    public partial class AspNetUserWorker
    {
        [Key]
        [Column(Order = 0)]
        public string UserId { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int WorkerId { get; set; }

        public int UserTypeId { get; set; }

        public bool IsActivte { get; set; }

        public int? ManagmentSpecializedId { get; set; }

        public int CompanyId { get; set; }

        public virtual AspNetUser AspNetUser { get; set; }

        public virtual AspNetUserType AspNetUserType { get; set; }
    }
}
