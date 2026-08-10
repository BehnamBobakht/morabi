using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.ViewModels
{
    public class RebateVm
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string CodeValue { get; set; }
        public double MinInvoiceValue { get; set; }
        public double MaxInvoiceValue { get; set; }
        public double MaxRebatePrice { get; set; }
        public int PercentValue { get; set; }
        public double PriceValue { get; set; }
        public int UseCount { get; set; }
        public Nullable<System.DateTime> StartDatetime { get; set; }
        public Nullable<System.DateTime> EndDatetime { get; set; }
        public bool IsPriceRebate { get; set; }
        public bool Active { get; set; }
        public Nullable<int> StoreId { get; set; }
        public bool Deleted { get; set; }
        public int UsedCount { get; set; }
        public double RebateFinalValue { get; set; }
        public StoreShowVm Store { get; set; }

        

    }
}
