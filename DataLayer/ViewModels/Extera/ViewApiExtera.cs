using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.ViewModels
{
    public class ViewApiExtera
    {
        public int Count { get; set; }
        public double TotalPrice { get; set; }
        public double ShippingPrice { get; set; }
        public double TotalDiscount { get; set; }

        public double MinPrice { get; set; }
        public double ? MaxPrice { get; set; }

        public object List { get; set; }
    }
}
