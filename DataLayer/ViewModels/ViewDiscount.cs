using DataLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.ViewModels
{
    public class ViewDiscount
    {
        public int Id { get; set; }
        public string RowId { get; set; }
        public ViewDiscountGroup Group { get; set; }
        public ViewCode Type { get; set; }
        public ViewShop Shop { get; set; }
        public ViewProductType ProductType { get; set; }
        public ViewProductCategory ProductCategory { get; set; }
        public ViewProductSubCategory ProductSubCategory { get; set; }
        public ViewProduct Product { get; set; }
        public ViewStoreProductQuantity StoreProductQuantity { get; set; }

        public int? GroupId { get; set; }
        public double? PriceValue { get; set; }
        public int? PercentValue { get; set; }
        public DateTime? StartDatetime { get; set; }
        public DateTime? EndDatetime { get; set; }
        public bool? Active { get; set; }
        public int? StoreId { get; set; }
        public ViewDiscount()
        {

        }
        public ViewDiscount(Discount discount, int index, string MaxZero)
        {
            this.Id = discount.ID;
            this.RowId = Persia.Number.ConvertToPersian((index + 1).ToString(MaxZero));
            this.Group = new ViewDiscountGroup(discount.DiscountGroup);
            this.Active = discount.Active;
            this.EndDatetime = discount.EndDatetime;
            this.PercentValue = discount.PercentValue;
            this.PriceValue = discount.PriceValue;
            this.Product = new ViewProduct(discount.Product);
            this.ProductCategory = new ViewProductCategory(discount.ProductCategory);
            this.ProductSubCategory = new ViewProductSubCategory(discount.ProductSubCategory);
            this.ProductType = new ViewProductType(discount.ProductType);
            this.Shop = new ViewShop(discount.Shop);
            this.StartDatetime = discount.StartDatetime;
            this.Type = new ViewCode(discount.Code);
            this.StoreProductQuantity = new ViewStoreProductQuantity(discount.StoreProductQuantity);
            this.StoreId = discount.StoreId;
        }
    }
}
