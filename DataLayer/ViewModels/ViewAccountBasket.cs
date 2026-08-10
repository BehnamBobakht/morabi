using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.ViewModels
{
    public class ViewAccountBasket
    {
        public int Id { get; set; }
        public ViewAccount Account { get; set; }
        public ViewProduct Product { get; set; }
        public ViewColor Color { get; set; }
        public ViewSize Size { get; set; }
        public ViewProductOptionItem ProductOptionItem { get; set; }

        public ViewShopReseller ShopReseller { get; set; }
        public ViewStore Store { get; set; }
        public int? ColorId { get; set; }
        public int? SizeId { get; set; }
        public int? OptionItemId { get; set; }
        public int? StoreId { get; set; }
        public string StoreName { get; set; }

        public int Count { get; set; }
        public double Price { get; set; }
        public double ProductPrice { get; set; }
        public double Discount { get; set; }
        public DateTime Datetime { get; set; }
        public double PercentDiscount { get; set; }

        public ViewAccountBasket() { }
    }
}
