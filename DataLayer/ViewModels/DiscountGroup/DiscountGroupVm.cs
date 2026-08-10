using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.ViewModels
{
    public class DiscountGroupVm
    {
        public int ID { get; set; }
        public int TypeId { get; set; }
        public string Name { get; set; }
        public string Label { get; set; }
        public Nullable<double> PriceValue { get; set; }
        public Nullable<int> PercentValue { get; set; }
        public Nullable<System.DateTime> StartDatetime { get; set; }
        public Nullable<System.DateTime> EndDatetime { get; set; }
        public bool Active { get; set; }
        public string Description { get; set; }
        public Nullable<int> PictureId { get; set; }

        public  CodeVm Code { get; set; }
        public  PictureVm Picture { get; set; }
    }
}
