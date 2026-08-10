using DataLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.ViewModels
{
    public class StoreProductQuantityVm
    {
        public int ID { get; set; }
        public int StoreProductId { get; set; }
        public int Count { get; set; }
        public double Price { get; set; }
        //public double Discount { get; set; }
        public DiscountGroupVm DiscountGroup { get; set; }
        public int? DiscountGroupId { get; set; }
        public int DiscountPercent { get; set; }
        public DateTime? DiscountExpire { get; set; }
        public double BasePrice { get; set; }
        public double EarnestPrice { get; set; }
        public string Warranty { get; set; }
        public int DeliveryDateCount { get; set; }
        public string FullName { get; set; }
        public double DiscountValue { get; set; }
        public StoreShowVm Store { get; set; }
        public int ProductId { get; set; }
        public int? ColorId { get; set; }
        public int? SizeId { get; set; }
        public int? OptionItemId { get; set; }
        public ColorVm Color { get; set; }
        public SizeVm Size { get; set; }
        public ProductOptionItemVm OptionItem { get; set; }
        public List<PriceRangeVm> PriceRange { get; set; }

    }
}
