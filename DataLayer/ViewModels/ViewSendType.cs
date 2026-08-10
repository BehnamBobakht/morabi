using DataLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.ViewModels
{
    public class ViewSendType
    {
        public int Id;
        public string Name { get; set; }
        public string Label { get; set; }
        public double? BasePrice { get; set; }
        public double? PerProductPrice { get; set; }
        public double? MaxPrice { get; set; }
        public double? MinPriceForFree { get; set; }
        public int MaxDays { get; set; }
        public double CurrentPrice { get; set; }
        public ViewSendType()
        {
        }
            public ViewSendType(SendType sendType)
        {
            this.Id = sendType.ID;
            this.Name = sendType.Name;
            this.Label = sendType.Label;
            this.BasePrice = sendType.BasePrice;
            this.PerProductPrice = sendType.PerProductPrice;
            this.MaxPrice = sendType.MaxPrice;
            this.MaxDays = sendType.MaxDays;
            this.MinPriceForFree = sendType.MinPriceForFree;

        }

    }
}
