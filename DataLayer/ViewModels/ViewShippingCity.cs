using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataLayer.Entities;

namespace DataLayer.ViewModels
{
    public class ViewShippingCity
    {
        public int Id { get; set; }
        public Nullable<int> StoreId { get; set; }
        
        public string CountyName { get; set; }
        public string CityName { get; set; }
        public string StateName { get; set; }
        public int StateId { get; set; }
        public int CityId { get; set; }
        public int CountryId { get; set; }
        public int? SendTime { get; set; }
        public decimal? SendPrice { get; set; }
        public int? SendPricePost { get; set; }
        public int? SendPricePostP { get; set; }
        public int? SendTimePost { get; set; }
        public int? SendTimePostP { get; set; }
        public ViewShippingCity()
        {

        }
        public ViewShippingCity(ShippingCity shipping)
        {
            this.Id = shipping.ShippingCityId;
            this.StoreId = shipping.StoreId;
            this.SendPrice = shipping.SendPrice;
            this.SendTime = shipping.SendTime;
            this.CityName = shipping.City.Name;
            this.StateName = shipping.State.Name;
            this.CountyName = shipping.Country.FaName;
            this.CityId = shipping.CityId.Value;
            this.StateId = shipping.StateId.Value;
            this.CountryId = shipping.CountryId.Value;
            this.SendPricePost = shipping.SendPricePost;
            this.SendPricePostP = shipping.SendPricePostP;
            this.SendTimePost = shipping.SendTimePost;
            this.SendTimePostP = shipping.SendTimePostP;
        }

    }
}
