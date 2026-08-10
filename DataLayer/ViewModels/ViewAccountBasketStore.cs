using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.ViewModels
{
    public class ViewAccountBasketStore
    {
        public ViewAccountBasketStore()
        {
            Products = new List<ViewAccountBasket>();
        }

        public ViewStore Store { get; set; }
        public List<ViewAccountBasket> Products { get; set; }
        public ViewSendType SendType { get; set; }
        public ViewShippingCity ShippingCity { get; set; }
        public double Discount { get; set; }
        public double Price { get; set; }
        public int Count { get; set; }
    }
}
