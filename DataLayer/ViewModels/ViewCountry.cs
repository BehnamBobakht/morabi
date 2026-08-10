using DataLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.ViewModels
{
    public class ViewCountry
    {
        public int Id { get; set; }
        public string FaName { get; set; }
        public string EnName { get; set; }
        public ViewCountry() { }

        public ViewCountry(Country country)
        {
            if (country != null)
            {
                this.Id = country.ID;
                this.FaName = country.FaName;
                this.EnName = country.EnName;
                //this.Picture = new ViewPicture(country.Picture);
            }
        }
    }
}
