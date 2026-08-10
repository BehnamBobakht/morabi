using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.ViewModels
{
    public class PriceRangeVm
    {
        public int ID { get; set; }
        public double Price { get; set; }
        public int StartCount { get; set; }
        public Nullable<int> EndCount { get; set; }
        public int StoreProductQuantityId { get; set; }


    }
}
