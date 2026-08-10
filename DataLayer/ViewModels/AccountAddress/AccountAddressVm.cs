using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.ViewModels
{
    public class AccountAddressVm
    {
        public int ID { get; set; }
        public int? SelectedId { get; set; }
        public Nullable<int> AccountId { get; set; }
        public Nullable<int> CountryId { get; set; }
        public Nullable<int> StateId { get; set; }
        public Nullable<int> CityId { get; set; }
        public string NameFamily { get; set; }
        public string Phone { get; set; }
        public string Mobile { get; set; }
        public string AddressValue { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public string PostalCode { get; set; }
        public string NationalCode { get; set; }
        public bool LegalUser { get; set; }

        public CityVm City { get; set; }
        public StateVm State { get; set; }

    }
}
