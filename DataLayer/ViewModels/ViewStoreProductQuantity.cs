using DataLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.ViewModels
{
    public class ViewStoreProductQuantity
    {


        public int ID { get; set; }
        public int StoreProductId { get; set; }
        public int Count { get; set; }
        public double Price { get; set; }
        public double Discount { get; set; }
        public double BasePrice { get; set; }

        public string Warranty { get; set; }
        public int DeliveryDateCount { get; set; }
        public string FullName { get; set; }

        public ViewStoreProduct StoreProduct { get; set; }
        public List<ViewPriceRange> PriceRanges { get; set; }
        public ViewProductQuantity ProductQuantity { get; set; }
        public ViewStoreProductQuantity()
        {
            PriceRanges = new List<ViewPriceRange>();
        }

        public ViewStoreProductQuantity(StoreProductQuantity storeProduct, int count = 1,AccountAddress address=null)
        {
            if (storeProduct != null)
            {

                this.ID = storeProduct.ID;
                this.StoreProductId = storeProduct.StoreProductId;
                this.Count = storeProduct.Count;
                if (!Base.BaseWebsite.WebsiteSetting.HasPriceRange)
                {
                    this.Price = storeProduct.Price;

                }
                else
                {
                    this.Price = Base.BaseStore.GetFromPriceRange(count: count, storeProductQuantity: storeProduct);
                    /*foreach (var item in storeProduct.PriceRange)
                    {
                        this.PriceRanges.Add(new ViewPriceRange(item));
                    }*/
                }

                this.StoreProduct = new ViewStoreProduct(storeProduct.StoreProduct,false,address);
                this.Warranty = !string.IsNullOrEmpty(storeProduct.Warranty) ? storeProduct.Warranty : Resource.Lang.GuaranteeOfAuthenticity;
                this.DeliveryDateCount = storeProduct.StoreProduct.Store.SendType.Any() ? storeProduct.StoreProduct.Store.SendType.First().MaxDays : 1;

                if (storeProduct.ProductQuantity != null)
                {
                    this.FullName = $"{StoreProduct.ProductName} {(storeProduct.ProductQuantity.ColorId != null ? Resource.Lang.Color+ " : " + storeProduct.ProductQuantity.Color.Name : "")} {(storeProduct.ProductQuantity.SizeId != null ? Resource.Lang.Size + " : " + storeProduct.ProductQuantity.Size.Name : "")} {(storeProduct.ProductQuantity.OptionItemId != null ? storeProduct.ProductQuantity.ProductOptionItem.ProductOption.Name + " : " + storeProduct.ProductQuantity.ProductOptionItem.Value : "")}";
                    this.ProductQuantity = new ViewProductQuantity()
                    {
                        ColorId = storeProduct.ProductQuantity.ColorId,
                        SizeId = storeProduct.ProductQuantity.SizeId,
                        OptionItemId = storeProduct.ProductQuantity.OptionItemId
                    };
                }

            }
        }
    }
}
