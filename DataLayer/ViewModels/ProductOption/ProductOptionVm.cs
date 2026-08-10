using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.ViewModels
{
    public class ProductOptionVm
    {
        public int ID { get; set; }
        public Nullable<int> ProductTypeId { get; set; }
        public string Name { get; set; }
        public Nullable<int> ShowNumber { get; set; }
      

    }
}
