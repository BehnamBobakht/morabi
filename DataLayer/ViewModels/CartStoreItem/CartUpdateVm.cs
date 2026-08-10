using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataLayer.Enumarables;
namespace DataLayer.ViewModels
{

    public class CartUpdateVm
    {
        public int StoreProductQuantityId { get; set; }
        public int Count { get; set; }
        public int? AccountId { get; set; }
        public string SyncCode { get; set; }
        public int? AddressId { get; set; }
        public int? RebateId { get; set; }
        public string RebateCode { get; set; }
        public int? StoreId { get; set; }
        public int? DeliveryId { get; set; }
        public int? MerchantId { get; set; }
        public string GiftMessage { get; set; }
        public string CustomerMessage { get; set; }

        public CartUpdateType UpdateType { get; set; }
    }
}
