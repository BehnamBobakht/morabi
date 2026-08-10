using DataLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.ViewModels
{
    public class ViewProductQuantity
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string FullName { get; set; }
        public Nullable<int> ColorId { get; set; }

        public Nullable<int> SizeId { get; set; }
        public int? OptionItemId { get; set; }
        public double ProductPrice { get; set; }
        public double Price { get; set; }
        public double Discount { get; set; }
        public ViewColor Color { get; set; }
        public ViewSize Size { get; set; }
        public ViewProductOptionItem OptionItem { get; set; }

        public ViewProductQuantity() { }
        public ViewProductQuantity(ProductQuantity productQuantity)
        {
            this.Id = productQuantity.ID;
            this.ProductId = productQuantity.ProductId;
            this.ProductPrice = productQuantity.Price.HasValue ? productQuantity.Price.Value : 0;
            this.Price = DataLayer.Base.BaseStore.GetDiscountPrice(productQuantity);
            this.Discount = ProductPrice - Price;
            this.ColorId = productQuantity.ColorId;
            this.SizeId = productQuantity.SizeId;

            if (productQuantity.ColorId.HasValue)
                this.Color = new ViewColor(productQuantity.Color, false);
            if (productQuantity.SizeId.HasValue)
                this.Size = new ViewSize(productQuantity.Size, false);
            this.OptionItemId = productQuantity.OptionItemId;
            if (productQuantity.OptionItemId.HasValue)
                this.OptionItem = new ViewProductOptionItem(productQuantity.ProductOptionItem, true);
            this.FullName = $"{productQuantity.Product.Name} {(productQuantity.ColorId != null ? "رنگ : " + productQuantity.Color.Name : "")} {(productQuantity.SizeId != null ? "سایز : " + productQuantity.Size.Name : "")}{(productQuantity.OptionItemId != null ? productQuantity.ProductOptionItem.ProductOption.Name + " : " + productQuantity.ProductOptionItem.Value : "")}";
        }
    }
}
