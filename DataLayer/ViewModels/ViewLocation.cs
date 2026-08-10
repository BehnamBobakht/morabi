using DataLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.ViewModels
{
    public class ViewLocation
    {
        public int CountryId { get; set; }
        public int CityId { get; set; }
        public int StateId { get; set; }
        public string StateName { get; set; }
        public string CityName { get; set; }
        public string CountryName { get; set; }
        public int AddressId { get; set; }
        

        public ViewLocation() { }

        
    }
}
