using DataLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.ViewModels
{
    public class ViewAccountAddress
    {
        public int Id { get; set; }
        public ViewAccount Account { get; set; }
        public ViewState State { get; set; }
        public ViewCity City { get; set; }
        public ViewCountry Country{ get; set; }
        public string NameFamily { get; set; }
        public string Mobile { get; set; }
        public string NationalCode { get; set; }
        public string Phone { get; set; }
        public string AddressValue { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public string PostalCode { get; set; }
        public bool LegalUser { get; set; }

        public ViewAccountAddress() { }

        public ViewAccountAddress(AccountAddress address)
        {
            if (address != null)
            {
                this.Id = address.ID;
                this.Account = new ViewAccount(address.Account, address);
                this.AddressValue = address.AddressValue;
                this.City = new ViewCity(address.City);
                this.Latitude = address.Latitude;
                this.Longitude = address.Longitude;
                this.Mobile = address.Mobile;
                this.Phone = address.Phone;
                this.NameFamily = address.NameFamily;
                this.PostalCode = address.PostalCode;
                this.NationalCode = address.NationalCode;
                this.State = new ViewState(address.State);
                this.Country= new ViewCountry(address.Country);
            }
        }
    }
}
