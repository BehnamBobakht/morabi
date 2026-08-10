using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.ViewModels
{
    public class CartVm
    {
        public CartVm()
        {
            CartStoreList = new List<CartStoreVm>();
            MerchantList = new List<MerchantVm>();
            DeliveryList = new List<DeliveryVm>();
        }
        public int ID { get; set; }
        public string SyncCode { get; set; }
        public Nullable<int> AccountId { get; set; }
        public Nullable<int> MerchantId { get; set; }
        public int ItemCount { get; set; }
        public Nullable<int> AddressId { get; set; }
        public Nullable<int> RebateId { get; set; }
        public double RebatePrice { get; set; }
        public double BasePrice { get; set; }
        public double Discount { get; set; }
        public double TotalDeliveryPrice { get; set; }
        public double TotalPrice { get; set; }
        public double PayementPrice { get; set; }
        public System.DateTime CreateDate { get; set; }
        public System.DateTime LastUpdateDate { get; set; }
        public string GiftMessage { get; set; }
        public string CustomerMessage { get; set; }
        public Nullable<int> DeliveryId { get; set; }
        public bool CashOnDelivery { get; set; }
        public string RebateCode { get; set; }
        public double EarnestRemainingPrice { get; set; }
        public bool? HaveAddress { get; set; }
        public AccountAddressVm AddressVm{ get; set; }
        public List<CartStoreVm> CartStoreList { get; set; }
        public List<MerchantVm> MerchantList { get; set; }
        public List<DeliveryVm> DeliveryList { get; set; }


    }
}
