using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.ViewModels
{
    public class AccountOrderProductVm
    {
        public int ID { get; set; }
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public int Count { get; set; }
        public double Price { get; set; }
        public System.DateTime Datetime { get; set; }
        public Nullable<double> ProductPrice { get; set; }
        public Nullable<double> ProductDiscount { get; set; }
        public Nullable<int> ColorId { get; set; }
        public Nullable<int> SizeId { get; set; }
        public Nullable<System.DateTime> SyncDatetime { get; set; }
        public Nullable<int> OptionItemId { get; set; }

        public virtual ColorVm Color { get; set; }
        public virtual ProductVm Product { get; set; }
        public virtual ProductOptionItemVm ProductOptionItem { get; set; }
        public virtual SizeVm Size { get; set; }
    }
}
