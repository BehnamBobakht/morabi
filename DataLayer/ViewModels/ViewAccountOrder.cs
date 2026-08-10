using DataLayer.Base;
using DataLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.ViewModels
{
    public class ViewAccountOrder
    {
        public int Id { get; set; }
        public string RowId { get; set; }
        public string Description { get; set; }

        public ViewAccount Account { get; set; }
        public ViewPaymentType PaymentType { get; set; }
        public ViewAccountAddress AccountAddress { get; set; }
        public string DeliveryType { get; set; }
        public double DeliveryPrice { get; set; }
        public ViewCode Status { get; set; }
        public string NextStatus { get; set; }

        public ViewRebate Rebate { get; set; }
        public ViewSendType SendType { get; set; }
        public double? SendPrice { get; set; }
        public double ProductsPrice { get; set; }
        public double Discount { get; set; }
        public double Price { get; set; }
        public int ProductCount { get; set; }
        public int CommentCount { get; set; }
        public DateTime Datetime { get; set; }
        public int? StoreId { get; set; }
        public string StoreName { get; set; }
        public int? BenefitPercent { get; set; }
        public string UniqueId { get; set; }

        public List<ViewAccountOrderProduct> AccountOrderProducts { get; set; }

        public ViewAccountOrder() { }

        public ViewAccountOrder(AccountOrder order, AccountAddress address = null)
        {
            this.Id = order.ID;
            this.UniqueId = order.UniqueId;
            this.Account = new ViewAccount(order.Account, address);
            this.PaymentType = new ViewPaymentType(order.PaymentType);
            this.AccountAddress = new ViewAccountAddress(order.AccountAddress);
            this.Status = new ViewCode(order.Code);
            //this.Rebate = order.RebateId != null ? new ViewRebate(order.Rebate) : null;
            this.Price = order.Price;
            this.Datetime = order.Datetime;
            this.StoreId = order.StoreId;
            this.StoreName = order.StoreId.HasValue ? order.Store.Name : "";
            this.AccountOrderProducts = new List<ViewAccountOrderProduct>();
            foreach (var item in order.AccountOrderProduct)
            {
                AccountOrderProducts.Add(new ViewAccountOrderProduct(item));
            }
            if (BaseWebsite.WebsiteSetting.HasCentralizedDelivery)
            {
                if (order.ParentId.HasValue)
                {
                    DeliveryPrice = order.AccountOrder2.DeliveryPrice;
                    this.DeliveryType = order.AccountOrder2.Delivery?.DeliveryType.Name;
                }
            }
            else
            {
                DeliveryPrice = order.DeliveryPrice;
                this.DeliveryType = order.Delivery?.DeliveryType.Name;
            }

        }

        public ViewAccountOrder(AccountOrder order, int index, string MaxZero)
        {
            this.Id = order.ID;
            this.UniqueId = order.UniqueId;
            this.Description = order.AccountOrder2.Description;
            this.RowId = Persia.Number.ConvertToPersian((index + 1).ToString(MaxZero));
            this.Account = new ViewAccount(order.Account);
            this.PaymentType = new ViewPaymentType(order.PaymentType);
            this.AccountAddress = new ViewAccountAddress(order.AccountAddress);
            this.Status = new ViewCode(order.Code);
            this.StoreId = order.StoreId;
            this.StoreName = order.StoreId.HasValue ? order.Store.Name : "";

            switch (this.Status.Label)
            {
                case nameof(Enumarables.Enum_Code.ORDER_STATUS_SUCCESS):
                    {
                        this.NextStatus = Enumarables.Enum_Code.ORDER_STATUS_POST.ToString();
                        break;
                    }
                    //case nameof(Enumarables.Enum_Code.ORDER_STATUS_PROCESS):
                    //    {
                    //        this.NextStatus = Enumarables.Enum_Code.ORDER_STATUS_POST.ToString();
                    //        break;
                    //    }
            }
            this.Price = order.Price;
            this.Datetime = order.Datetime;
            int ProductCount = 0;
            this.CommentCount = order.AccountOrderComment.Count;
            this.AccountOrderProducts = new List<ViewAccountOrderProduct>();
            foreach (var item in order.AccountOrderProduct)
            {
                ProductCount = ProductCount + item.Count;
                AccountOrderProducts.Add(new ViewAccountOrderProduct(item));
            }
            this.ProductCount = ProductCount;
            this.BenefitPercent = order.BenefitPercent;
            if (BaseWebsite.WebsiteSetting.HasCentralizedDelivery)
            {
                if (order.ParentId.HasValue)
                {
                    DeliveryPrice = order.AccountOrder2.DeliveryPrice;
                    this.DeliveryType = order.AccountOrder2.Delivery?.DeliveryType.Name;
                }
            }
            else
            {
                DeliveryPrice = order.DeliveryPrice;
                this.DeliveryType = order.Delivery?.DeliveryType.Name;
            }

        }
    }
}
