using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.ViewModels
{
    public class ProductPiceHistoryVm
    {
        public int ID { get; set; }
        public Nullable<double> Price { get; set; }
        public Nullable<System.DateTime> Date { get; set; }
        public Nullable<int> ProductId { get; set; }

    }
}
