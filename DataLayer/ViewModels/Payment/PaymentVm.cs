using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.ViewModels
{
    public class PaymentVm
    {

        public int ID { get; set; }
        public Nullable<int> SiteUserId { get; set; }
        public Nullable<int> AccountId { get; set; }
        public int StatusId { get; set; }
        public Nullable<int> MerchantId { get; set; }
        public int SubjectId { get; set; }
        public string RefNumber { get; set; }
        public string ExternalInfo1 { get; set; }
        public string ExternalInfo2 { get; set; }
        public string ExternalInfo3 { get; set; }
        public string ExternalInfo4 { get; set; }
        public double Amount { get; set; }
        public string IpAddress { get; set; }
        public System.DateTime Datetime { get; set; }
        public string Description { get; set; }
        public string Sign { get; set; }
        public Nullable<int> PaymentWebsiteId { get; set; }
        public Nullable<int> CurrencyTypeId { get; set; }
        public string ExternalInfo5 { get; set; }
        public string ExternalInfo6 { get; set; }
        public string ExternalInfo7 { get; set; }
        public string PaymentAddress { get; set; }
        public Nullable<bool> IsMobile { get; set; }
        public bool IsInPlace { get; set; }
        public CodeVm Status { get; set; }
        public MerchantVm Merchant { get; set; }


    }
}
