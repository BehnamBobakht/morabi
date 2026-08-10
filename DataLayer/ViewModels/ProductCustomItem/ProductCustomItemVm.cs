using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.ViewModels
{
    public class ProductCustomItemVm
    {
        public int ID { get; set; }
        public int FieldId { get; set; }
        public string SyncId { get; set; }
        public string Value { get; set; }
    }
}
