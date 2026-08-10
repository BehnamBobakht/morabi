using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataLayer.Entities;

namespace DataLayer.ViewModels
{
    public class ViewShipping
    {
        public int Id { get; set; }
        public Nullable<int> StoreId { get; set; }
        public int Price { get; set; }
        public Nullable<int> MinPriceForFree { get; set; }
        public int MaxDays { get; set; }
        public int CurrentPrice { get; set; }

        public ViewShipping()
        {

        }
        public ViewShipping(Shipping shipping)
        {
            this.Id = shipping.ID;
            this.StoreId = shipping.StoreId;
            this.Price = shipping.Price;
            this.MinPriceForFree = shipping.MinPriceForFree;
            this.MaxDays = shipping.MaxDays;

        }

    }
}
