using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.ViewModels
{
   public class ProductExteraVm
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public List<ProductTypeVm> Categories { get; set; }
    }
}
