using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.ViewModels
{
    public class AccountOrderVm
    {
        public int ID { get; set; }
        public int AccountId { get; set; }
        public Nullable<int> AddressId { get; set; }
        public Nullable<int> PaymentTypeId { get; set; }
        public Nullable<int> RebateId { get; set; }
        public int StatusId { get; set; }
        public double Price { get; set; }
        public System.DateTime Datetime { get; set; }
        public Nullable<double> DiscountPrice { get; set; }
        public string Description { get; set; }
        public Nullable<double> RebatePrice { get; set; }
        public Nullable<int> StoreId { get; set; }
        public Nullable<int> BenefitPercent { get; set; }
        public Nullable<int> ClearingId { get; set; }
        public Nullable<int> ParentId { get; set; }
        public Nullable<bool> IsGift { get; set; }
        public string GiftDescription { get; set; }
        public Nullable<bool> HasFactor { get; set; }
        public Nullable<int> StoreFactorDocId { get; set; }
        public Nullable<int> DeliveryId { get; set; }
        public double DeliveryPrice { get; set; }
        public string CustomerMessage { get; set; }
        public string UniqueId { get; set; }

        public AccountAddressVm AccountAddress { get; set; }
        public  List<AccountOrderVm> AccountOrder1 { get; set; }
        public StoreShowVm Store { get; set; }
      
        public  List<AccountOrderProductVm> AccountOrderProduct { get; set; }
        public  CodeVm Code { get; set; }
        //public virtual PaymentTypeVm PaymentType { get; set; }
        public  RebateVm Rebate { get; set; }
        public  PaymentVm Payment { get; set; }

    }
}
