using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.ViewModels
{
    public class CartStoreItemVm
    {
        public int ID { get; set; }
        public int ProductId { get; set; }
        public int StoreProductQuantityId { get; set; }
        public string ProductName { get; set; }
        public string Description { get; set; }

        public int Count { get; set; }
        public double SingleBasePrice { get; set; }
        public double SingleDiscount { get; set; }
        public int SingleDiscountPercent { get; set; }
        public double SinglePrice { get; set; }
        public double TotalPrice { get; set; }
        public bool Deleted { get; set; }
        public double ChangePrice { get; set; }
        public double SingleEarnestRemainingPrice { get; set; }
        public double TotalEarnestRemainingPrice { get; set; }
        public ProductShowVm Product { get; set; }
        public SizeVm Size { get; set; }
        public ColorVm Color { get; set; }
        public ProductOptionItemVm ProductOptionItem { get; set; }

    }
}
