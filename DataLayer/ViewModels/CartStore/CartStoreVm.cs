using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.ViewModels
{
    public class CartStoreVm
    {
        public CartStoreVm()
        {
            CartStoreItemList = new List<CartStoreItemVm>();
            DeliveryList = new List<DeliveryVm>();
        }
        public int ID { get; set; }
        public int StoreId { get; set; }
        public string StoreName { get; set; }
        public Nullable<int> DeliveryId { get; set; }
        public double DeliveryPrice { get; set; }
        public Nullable<int> RebateId { get; set; }
        public double RebatePrice { get; set; }
        public double BasePrice { get; set; }
        public double Discount { get; set; }
        public double TotalPrice { get; set; }
        public double PayementPrice { get; set; }
        public double EarnestRemainingPrice { get; set; }

        public List<CartStoreItemVm> CartStoreItemList { get; set; }
        public List<DeliveryVm> DeliveryList { get; set; }

    }
}
