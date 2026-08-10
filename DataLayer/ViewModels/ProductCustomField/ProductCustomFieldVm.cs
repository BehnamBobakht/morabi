using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.ViewModels
{
    public class ProductCustomFieldVm
    {
        public int ID { get; set; }
        public string SyncId { get; set; }
        public int TypeId { get; set; }
        public string TypeLabel { get; set; }
        public string Name { get; set; }
        public Nullable<int> ShowNumber { get; set; }
        public bool IsRequired { get; set; }
        public string SyncName { get; set; }
        public Nullable<bool> Active { get; set; }
        public bool InSearchResult { get; set; }
        public bool Hide { get; set; }
        public string Value { get; set; }
        public Nullable<int> ValueItemId { get; set; }

        public List<ProductCustomItemVm> ProductCustomItemList { get; set; }
        public ProductCustomFieldVm()
        {
            ProductCustomItemList = new List<ProductCustomItemVm>();
        }
    }
}
