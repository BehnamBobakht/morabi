using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.ViewModels
{
    public class ProductQuantityVm
    {
        public int ID { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public Nullable<int> ColorId { get; set; }
        public Nullable<int> SizeId { get; set; }
        public Nullable<int> Count { get; set; }
        public Nullable<double> Price { get; set; }
        public  double ? EuroPrice { get; set; }

        public Nullable<int> OptionItemId { get; set; }
        public Nullable<int> StoreProductQuantityId { get; set; }
        public  ColorVm Color { get; set; }
        public  ProductOptionItemVm ProductOptionItem { get; set; }
        public  SizeVm Size { get; set; }

    }
}
