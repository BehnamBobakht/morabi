using DataLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.ViewModels
{
   public class ViewPriceRange
    {
        public int Id { get; set; }
        public double Price { get; set; }
        public int StartCount { get; set; }
        public int? EndCount { get; set; }
        public int StoreProductQuantityId { get; set; }
        public ViewPriceRange(PriceRange priceRange)
        {
            this.Id = priceRange.ID;
            this.Price = priceRange.Price;
            this.StartCount = priceRange.StartCount;
            this.EndCount = priceRange.EndCount;
            this.StoreProductQuantityId = priceRange.StoreProductQuantityId;
        }

    }

}
