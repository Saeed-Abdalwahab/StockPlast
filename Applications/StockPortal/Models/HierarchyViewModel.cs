using System.Collections.Generic;

namespace StockPortal.Models
{
    public class HierarchyViewModel
    {
        public string Id { get; set; }
        public string text { get; set; }
        //public string Code { get; set; }
        public int? perentId { get; set; }
        public bool ischecked { get; set; }
        public virtual List<HierarchyViewModel> children { get; set; }
    }
}