using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.ViewModels
{
    public class DeliveryVm
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public int DeliveryTypeId { get; set; }
        public int? StoreId { get; set; }
        public bool IsUnknownPrice { get; set; }
        public double? BasePrice { get; set; }
        public Nullable<double> MinPriceForFree { get; set; }
        public int MinDays { get; set; }
        public int MaxDays { get; set; }
        public Nullable<double> CashOnDeliveryMaxPrice { get; set; }
        public int CityId { get; set; }
        public int StateId { get; set; }
        public bool Active { get; set; }
        public bool Deleted { get; set; }
        public bool CashOnDelivery { get; set; }
        public double PayementPrice { get; set; }
        public DeliveryTypeVm DeliveryType { get; set; }
        public StoreShowVm Store { get; set; }
        public CityVm City { get; set; }
        public StateVm State { get; set; }
    }
}
